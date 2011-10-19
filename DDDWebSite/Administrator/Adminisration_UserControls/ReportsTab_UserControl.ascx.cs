using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BLL;

public partial class Administrator_Adminisration_UserControls_ReportsTab_UserControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ReportsTab_AddReportButton.ButtOnClick += new EventHandler(ReportsTab_AddReportButton_Click);
            if (!IsPostBack)
            {
                Session["ReportsTab_UserControl_ReportsIds"] = null;
            }
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void ReportsDataGrid_RadioButton_Checked(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
          /*  foreach (DataGridItem oldrow in ReportsDataGrid.Items)
            {
                ((RadioButton)oldrow.FindControl("ReportsDataGrid_RadioButton")).Checked = false;
            }*/
            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            DataGridItem row = (DataGridItem)rb.NamingContainer;
            ((RadioButton)row.FindControl("ReportsDataGrid_RadioButton")).Checked = true;
            int reportId = ((List<int>)Session["ReportsTab_UserControl_ReportsIds"])[row.DataSetIndex];
            Selected_ReportsDataGrid_Index.Value = reportId.ToString();

            dataBlock.OpenConnection();            
            //Загружаем описание отчета
            LoadReportNote(reportId, dataBlock);
            //делает кнопку добавить неактивной, если отчет уже добавлен и наоборот       
            DateTime outdatetime = new DateTime();
            if (DateTime.TryParse(dataBlock.reportsTable.GetReportUserOrg_BDATE(reportId, orgId), out outdatetime))
            {
                ReportsTab_AddReportButton.Enabled = false;
                AccessPanel1.Enabled = true;
                //Загружаем закладку доступа
                LoadReportsAccessInfo(reportId, orgId, dataBlock);
                AccessTab_SaveButton.Enabled = true;
                AccessTab_AvailableForAllButton.Enabled = true;
                AccessTab_UnAvailableForAllButton.Enabled = true;
            }
            else
            {
                AccessPanel1.Enabled = false;
                ReportsTab_AddReportButton.Enabled = true;
            }
            ///////////////////////
            ReportsUpdatePanel.Update();
            /*ReportsTab_ButtonsUpdateTable.Update();
            ReportsDataGridUpdatePanel.Update();
            AccessTab_DataGridUpdateTable.Update();
            AccessTab_ButtonsUpdateTable.Update();*/
            //////////////////////
            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.CloseConnection();
            RaiseException(ex);
        }
    }

    public void LoadReportsTable()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        List<int> reportsIds = new List<int>();

        dataBlock.OpenConnection();

        reportsIds = dataBlock.reportsTable.GetAllReportsIds();
        Session["ReportsTab_UserControl_ReportsIds"] = reportsIds;

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("CODE", typeof(string)));
        dt.Columns.Add(new DataColumn("TYPE", typeof(string)));
        dt.Columns.Add(new DataColumn("NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("PRICE", typeof(string)));
        dt.Columns.Add(new DataColumn("UPDATE_DATE", typeof(string)));
        dt.Columns.Add(new DataColumn("BEGIN_DATE", typeof(string)));
        dt.Columns.Add(new DataColumn("END_DATE", typeof(string)));

        DateTime date = new DateTime();
        string dateStringToShow = "";
        foreach (int reportId in reportsIds)
        {
            dr = dt.NewRow();
            dr["CODE"] = reportId.ToString();
            dr["TYPE"] = dataBlock.reportsTable.GetReportTypeName(reportId, 0);
            dr["NAME"] = dataBlock.reportsTable.GetReportName(reportId);
            dr["PRICE"] = dataBlock.reportsTable.GetReportPrice(reportId);
            dr["UPDATE_DATE"] = dataBlock.reportsTable.GetReportDateUpdate(reportId).ToShortDateString();
            //Дата добавления в организацию.
            if (DateTime.TryParse(dataBlock.reportsTable.GetReportUserOrg_BDATE(reportId, orgId), out date))
                dateStringToShow = date.ToShortDateString();
            else
                dateStringToShow = "Не добавлен";
            dr["BEGIN_DATE"] = dateStringToShow;
            //Дата окончания в организации.
            if (DateTime.TryParse(dataBlock.reportsTable.GetReportUserOrg_EDATE(reportId, orgId), out date))
                dateStringToShow ="До " + date.ToShortDateString();
            else
                dateStringToShow = "-";
            dr["END_DATE"] = dateStringToShow;
            dt.Rows.Add(dr);
        }

        ReportsDataGrid.DataSource = dt;
        ReportsDataGrid.DataBind();
        dataBlock.CloseConnection();
    }
    private void LoadReportNote(int reportId, DataBlock dataBlock)
    {
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        string reportNote = dataBlock.reportsTable.GetUserReport_Note(reportId);
        if (reportNote.Trim() == "")
            reportNote = "Описание отчета отсутствует.";

        ReportAboutLabel.Text = reportNote;
        ReportNoteUpdatePanel.Update();
    }
    private void LoadReportsAccessInfo(int reportId, int orgId, DataBlock dataBlock)
    {
        List<KeyValuePair<string, int>> userTypes = new List<KeyValuePair<string,int>>();

        userTypes = dataBlock.usersTable.GetAllUsersRoles();

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("USER_TYPE_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("USERTYPE", typeof(string)));
        dt.Columns.Add(new DataColumn("ENABLEDFROM", typeof(string)));

        DateTime date = new DateTime();
        string dateStringToShow = "";
        foreach (KeyValuePair<string, int> userType in userTypes)
        {
            dr = dt.NewRow();
            dr["USER_TYPE_ID"] = userType.Value.ToString();
            dr["USERTYPE"] = userType.Key.Trim();

            if (DateTime.TryParse(dataBlock.reportsTable.GetReportUserRoles_SETDATE(reportId, orgId, userType.Value), out date))
                dateStringToShow = date.ToShortDateString();
            else
                dateStringToShow = "Не доступен";

            dr["ENABLEDFROM"] = dateStringToShow;
            dt.Rows.Add(dr);
        }

        AccessDataGrid.DataSource = dt;
        AccessDataGrid.DataBind();

        int i = 0;

        foreach (DataGridItem oldrow in AccessDataGrid.Items)
        {
            if(dataBlock.reportsTable.GetReportUserRoles_SETDATE(reportId, orgId, userTypes[i].Value).Trim() =="")
                ((CheckBox)oldrow.FindControl("AccessDataGrid_CheckBox")).Checked = false;
            else
                ((CheckBox)oldrow.FindControl("AccessDataGrid_CheckBox")).Checked = true;
            i++;
        }
    }

    protected void ReportsTab_AddReportButton_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            int reportId = Convert.ToInt32(Selected_ReportsDataGrid_Index.Value);

            dataBlock.reportsTable.AddOrSetReportUserOrg_ActivateReportForORG(reportId, orgId, curUserId);
            string billName = @"Invoice for report """ + dataBlock.reportsTable.GetReportName(reportId) + @"""";
            ///Выставляем счет, когда добавляем отчет. Потом по-любому все убрать!!!!
            dataBlock.invoiceTable.AddInvoice(dataBlock.invoiceTable.InvoiceType_addReportInvoice, dataBlock.invoiceTable.Status_NotPaid,
                orgId, billName, DateTime.Now, DateTime.Now.AddDays(7), new DateTime());
            /////////////////////////////////////////////////////
            dataBlock.CommitTransaction();
            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.RollbackConnection();
            dataBlock.CloseConnection();

            RaiseException(ex);
        }
        LoadReportsTable();
        ReportsTab_AddReportButton.Enabled = false;
        AccessPanel1.Enabled = false;
        ReportsUpdatePanel.Update();
    }
    protected void ReportsTab_DelReportButton_only4Test_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            int reportId = Convert.ToInt32(Selected_ReportsDataGrid_Index.Value);
            dataBlock.reportsTable.OpenConnection();
            dataBlock.reportsTable.OpenTransaction();
            dataBlock.reportsTable.AddOrSetReportUserOrg_DEactivateReportForORG(reportId, orgId, 0);
            dataBlock.reportsTable.CommitTransaction();
            dataBlock.reportsTable.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.reportsTable.RollbackConnection();
            dataBlock.reportsTable.CloseConnection();
            RaiseException(ex);
        }
        LoadReportsTable();
        ReportsTab_AddReportButton.Enabled = false;
        AccessPanel1.Enabled = false;
        ReportsUpdatePanel.Update();
    }

    protected void AccessTab_SaveButton_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        int userRoleId = -1;
        bool isActive = false;
        try
        {
            dataBlock.reportsTable.OpenConnection();
            dataBlock.reportsTable.OpenTransaction();
            int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
            int selectedReportId = Convert.ToInt32(Selected_ReportsDataGrid_Index.Value);
            foreach (DataGridItem oldrow in AccessDataGrid.Items)
            {
                userRoleId = Convert.ToInt32(oldrow.Cells[0].Text);
                isActive = ((CheckBox)oldrow.FindControl("AccessDataGrid_CheckBox")).Checked;
                dataBlock.reportsTable.AddOrSetReportUserRoles_SETDATE(selectedReportId, orgId, userRoleId, isActive, curUserId);
            }

            dataBlock.reportsTable.CommitTransaction();
            dataBlock.reportsTable.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.reportsTable.RollbackConnection();
            dataBlock.reportsTable.CloseConnection();
            RaiseException(ex);
        }
    }
    protected void AccessTab_AvailableForAllButton_Click(object sender, EventArgs e)
    {
        foreach (DataGridItem oldrow in AccessDataGrid.Items)
        {
            ((CheckBox)oldrow.FindControl("AccessDataGrid_CheckBox")).Checked = true;
        }
        AccessTab_DataGridUpdateTable.Update();
    }
    protected void AccessTab_UnAvailableForAllButton_Click(object sender, EventArgs e)
    {
        foreach (DataGridItem oldrow in AccessDataGrid.Items)
        {
            ((CheckBox)oldrow.FindControl("AccessDataGrid_CheckBox")).Checked = false;
        }
        AccessTab_DataGridUpdateTable.Update();
    }

    private void RaiseException(Exception ex)
    {
        Session["AdministrationTabException"] = ex;
        RaiseBubbleEvent(null, new EventArgs());
    }
}
