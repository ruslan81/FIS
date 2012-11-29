using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BLL;

public partial class Administrator_Adminisration_UserControls_InvoicesTab_UserControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            InvoicesTab_PayInvoiceButton.ButtOnClick += new EventHandler(InvoicesTab_PayInvoiceButton_Click);
            if (!IsPostBack)
            {
                Session["InvoicesTab_UserControl_UsersIds"] = null;
            }
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void InvoicesDataGrid_RadioButton_Checked(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        try
        {
          /*  foreach (DataGridItem oldrow in InvoicesDataGrid.Items)
            {
                ((RadioButton)oldrow.FindControl("InvoicesDataGrid_RadioButton")).Checked = false;
            }*/
            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            DataGridItem row = (DataGridItem)rb.NamingContainer;
            ((RadioButton)row.FindControl("InvoicesDataGrid_RadioButton")).Checked = true;
            int invoiceId = ((List<int>)Session["InvoicesTab_UserControl_UsersIds"])[row.DataSetIndex];
            Selected_InvoicesDataGrid_Index.Value = invoiceId.ToString();
            //LoadUserAdditionalInfo(userId); dataBlock.reportsTable.OpenConnection();

            //dataBlock.invoiceTable.OpenConnection();
            dataBlock.OpenConnection();
            //делает кнопку Оплатить неактивной, если счет уже оплачен и наоборот       
            DateTime outdatetime = new DateTime();
            if (DateTime.TryParse(dataBlock.invoiceTable.GetDatePayment(invoiceId), out outdatetime))
            {
                InvoicesTab_PayInvoiceButton.Enabled = false;
            }
            else
            {
                InvoicesTab_PayInvoiceButton.Enabled = true;
            }
            ///////////////////////
            InvoicesDataGridUpdatePanel.Update();
            InvoicesTab_ButtonsUpdateTable.Update();
            //////////////////////
            //dataBlock.invoiceTable.CloseConnection();
            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.CloseConnection();
//            dataBlock.invoiceTable.CloseConnection();
            RaiseException(ex);
        }
    }

    public void LoadInvoicesTable()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        List<int> invoicesIds = new List<int>();

        // dataBlock.invoiceTable.OpenConnection();
        dataBlock.OpenConnection();

        invoicesIds = dataBlock.invoiceTable.GetAllInvoices(orgId);
        Session["InvoicesTab_UserControl_UsersIds"] = invoicesIds;

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("INVOICEDATE", typeof(string)));
        dt.Columns.Add(new DataColumn("PAYTERMDATE", typeof(string)));
        dt.Columns.Add(new DataColumn("STATUS", typeof(string)));
        dt.Columns.Add(new DataColumn("PAYDATE", typeof(string)));

        DateTime date = new DateTime();
        string dateStringToShow = "";
        foreach (int invoiceId in invoicesIds)
        {
            dr = dt.NewRow();
            dr["NAME"] = dataBlock.invoiceTable.GetInvoiceName(invoiceId);
            date = dataBlock.invoiceTable.GetDateInvoice(invoiceId);
            dr["INVOICEDATE"] = date.ToShortDateString();
            date = dataBlock.invoiceTable.GetDatePaymentTerm(invoiceId);
            dr["PAYTERMDATE"] = date.ToShortDateString();
            dr["STATUS"] = dataBlock.invoiceTable.GetInvoiceStatus(invoiceId);
              //PAYTERMDATE - Дата оплаты
            if (DateTime.TryParse(dataBlock.invoiceTable.GetDatePayment(invoiceId), out date))
                dateStringToShow = date.ToShortDateString();
            else
                dateStringToShow = "-";
            dr["PAYDATE"] = dateStringToShow;
            dt.Rows.Add(dr);
        }

        InvoicesDataGrid.DataSource = dt;
        InvoicesDataGrid.DataBind();
        //dataBlock.invoiceTable.CloseConnection();
        dataBlock.CloseConnection();
    }

    protected void InvoicesTab_PayInvoiceButton_Click(object sender, EventArgs e)
    {
        string currentLanguage = "STRING_RU";
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, currentLanguage);
        try
        {
            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
            int invoiceId = Convert.ToInt32(Selected_InvoicesDataGrid_Index.Value);

            //old version
          /*  dataBlock.invoiceTable.OpenConnection();
            dataBlock.invoiceTable.OpenTransaction();
            dataBlock.invoiceTable.PayABill(invoiceId);
            string invoiceName = dataBlock.invoiceTable.GetInvoiceName(invoiceId);
            dataBlock.invoiceTable.CommitTransaction();
            dataBlock.invoiceTable.CloseConnection();*/

            //test version
            dataBlock.invoiceTable.PayABill(invoiceId);
            string invoiceName = dataBlock.invoiceTable.GetInvoiceName(invoiceId);
            dataBlock.CommitTransaction();
            dataBlock.CloseConnection();
            //

            LoadInvoicesTable();
            InvoicesDataGridUpdatePanel.Update();
            InvoicesTab_ButtonsUpdateTable.Update();
            ////    добавление записи в журнал. потом возможно перенести куда надо(в логику).
            DB.SQL.SQLDB sqlDb = new DB.SQL.SQLDB(connectionString);
            HistoryTable history = new HistoryTable(connectionString, currentLanguage, sqlDb);
            sqlDb.OpenConnection();
            history.AddHistoryRecord("FN_INVOICE", "INVOICE_STATUS_ID", dataBlock.invoiceTable.Status_Paid, curUserId, history.invoicePaid, "#" + invoiceId + @" :""" + invoiceName + @"""", sqlDb);
            sqlDb.CloseConnection();
            ////
        }
        catch (Exception ex)
        {
            dataBlock.invoiceTable.RollbackConnection();
            dataBlock.invoiceTable.CloseConnection();
            RaiseException(ex);
        }
    }

    protected void InvoicesTab_UNPayInvoiceButton_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        try
        {
            int invoiceId = Convert.ToInt32(Selected_InvoicesDataGrid_Index.Value);


           dataBlock.OpenConnection();
           dataBlock.OpenTransaction();
           dataBlock.invoiceTable.UnPayABill(invoiceId);
           dataBlock.CommitTransaction();
           dataBlock.CloseConnection();
          /*  dataBlock.invoiceTable.OpenConnection();
            dataBlock.invoiceTable.OpenTransaction();
            dataBlock.invoiceTable.UnPayABill(invoiceId);
            dataBlock.invoiceTable.CommitTransaction();
            dataBlock.invoiceTable.CloseConnection();*/
            LoadInvoicesTable();
            InvoicesDataGridUpdatePanel.Update();
            InvoicesTab_ButtonsUpdateTable.Update();
        }
        catch (Exception ex)
        {
            dataBlock.RollbackConnection();
            dataBlock.CloseConnection();
            /*dataBlock.invoiceTable.RollbackConnection();
            dataBlock.invoiceTable.CloseConnection();*/
            RaiseException(ex);
        }
    }

    private void RaiseException(Exception ex)
    {
        Session["AdministrationTabException"] = ex;
        RaiseBubbleEvent(null, new EventArgs());
    }

    protected void InvoicesDataGrid_Sort(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
    }
}
