using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BLL;

public partial class Administrator_Settings_UserControls_EmailSheduler : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void LoadAllDropDowns(DataBlock dataBlock)
    {
        PeriodTypeDropDown.Items.Clear();
        ReportTypeDropDown.Items.Clear();
        ReportNameDropDown.Items.Clear();

        PeriodTypeDropDown.Items.Add(new ListItem("Минуты", "0"));
        PeriodTypeDropDown.Items.Add(new ListItem("Часы", "1"));
        PeriodTypeDropDown.Items.Add(new ListItem("Дни", "2"));
        PeriodTypeDropDown.Items.Add(new ListItem("Месяцы", "3"));
        PeriodTypeDropDown.Items.Add(new ListItem("Годы", "4"));

        //Грузим типы отчетов
        List<KeyValuePair<string, int>> reportTypes = dataBlock.reportsTable.GetAllReportTypes();
        foreach (KeyValuePair<string, int> type in reportTypes)
        {
            ReportTypeDropDown.Items.Add(new ListItem(type.Key, type.Value.ToString()));
        }
        if (ReportTypeDropDown.Items.Count > 0)
            ReportTypeDropDown.Items[0].Selected = true;
        else
            throw new Exception("Не найдено отчетов");
        ReportTypeDropDown_SelectedIndexChanged(null, null);

        /*//Грузим названия отчетов в соответсвии с выбранным типом отчета
        List<int> reportIds = dataBlock.reportsTable.GetAllReportsIds(Convert.ToInt32(ReportTypeDropDown.SelectedValue));
        foreach (int id in reportIds)
        {
            ReportNameDropDown.Items.Add(new ListItem(dataBlock.reportsTable.GetReportName(id), id.ToString()));
        }*/
        //Грузим email пользователя
        int userId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
        EmailAddressTextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_Email);
    }

    private DataTable createDataSource(DataBlock dataBlock)
    {
        DataTable newDataTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "SheduleId";
        string Col_6 = "CardsName";
        string Col_2 = "ReportName";
        string Col_3 = "LastSendDate";
        string Col_4 = "Period";
        string Col_5 = "EmailAddress";

        newDataTable.Columns.Add(new DataColumn(Col_1, typeof(int)));
        newDataTable.Columns.Add(new DataColumn(Col_6, typeof(string)));
        newDataTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        newDataTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        newDataTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        newDataTable.Columns.Add(new DataColumn(Col_5, typeof(string)));
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        int userId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);

        List<SingleEmailSchedule> shedulesList = dataBlock.emailScheduleTable.GetAllEmailShedules(orgId, userId);
        string period = "";
        foreach (SingleEmailSchedule shedule in shedulesList)
        {
            period = "";
            dr = newDataTable.NewRow();
            dr[Col_1] = shedule.EMAIL_SCHEDULE_ID;
            dr[Col_6] = dataBlock.cardsTable.GetCardName(shedule.CARD_ID);
            dr[Col_2] = dataBlock.reportsTable.GetReportName(shedule.REPORT_ID);
            dr[Col_3] = shedule.LAST_SEND_DATE.ToShortDateString() + " " + shedule.LAST_SEND_DATE.ToShortTimeString();
            switch (shedule.PERIOD_TYPE)// 0-Минуты, 1-дни, 2-месяцы, 3-годы
            {
                case 0:
                    period += "Минуты: ";
                    break;
                case 1:
                    period += "Часы: ";
                    break;
                case 2:
                    period += "Дни: ";
                    break;
                case 3:
                    period += "Месяцы: ";
                    break;
                case 4:
                    period += "Годы: ";
                    break;
            }
            period += shedule.PERIOD;
            dr[Col_4] = period;
            dr[Col_5] = shedule.EMAIL_ADDRESS;
            newDataTable.Rows.Add(dr);
        }
        return newDataTable;
    }

    public void LoadShedulesDataGrid()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        try
        {
            Selected_SheduleDataGrid_Index.Value = "";
            ShedulesDataGridPanel.Visible = true;
            ShedulesEdit.Visible = false;
            dataBlock.OpenConnection();
            ShedulesDataGrid.DataSource = createDataSource(dataBlock);
            ShedulesDataGrid.DataBind();
            LoadAllDropDowns(dataBlock);
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            dataBlock.CloseConnection();
            ShedulesEdit_UpdatePanel.Update();
        }
    }

    public void EditShedule()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        try
        {
            int shedId = 0;
            if (!int.TryParse(Selected_SheduleDataGrid_Index.Value, out shedId))
                throw new Exception("Выберите запись!");

            dataBlock.OpenConnection();
            ShedulesDataGridPanel.Visible = false;
            ShedulesEdit.Visible = true;
           
            SingleEmailSchedule shedule = dataBlock.emailScheduleTable.GetEmailShedule(shedId);
            //Грузим типы отчетов
            int reportTypeID = dataBlock.reportsTable.GetUserReport_TypeID(shedule.REPORT_ID);
           /* ReportTypeDropDown.Items.Clear();
            List<KeyValuePair<string, int>> reportTypes = dataBlock.reportsTable.GetAllReportTypes();
            foreach (KeyValuePair<string, int> type in reportTypes)
            {
                ReportTypeDropDown.Items.Add(new ListItem(type.Key, type.Value.ToString()));
            }
            */

            ReportTypeDropDown.SelectedIndex = ReportTypeDropDown.Items.IndexOf(ReportTypeDropDown.Items.FindByValue(reportTypeID.ToString()));
          /*  ReportTypeDropDown_SelectedIndexChanged(null, null);*/
            //Загружаем ReportNameDropDown
            ReportNameDropDown.Items.Clear();
            List<int> Ids = dataBlock.reportsTable.GetAllReportsIds(reportTypeID);
            foreach (int id in Ids)
            {
                ReportNameDropDown.Items.Add(new ListItem(dataBlock.reportsTable.GetReportName(id), id.ToString()));
            }
            ReportNameDropDown.SelectedIndex = ReportNameDropDown.Items.IndexOf(ReportNameDropDown.Items.FindByValue(shedule.REPORT_ID.ToString()));
            //
            PeriodTypeDropDown.SelectedIndex = PeriodTypeDropDown.Items.IndexOf(PeriodTypeDropDown.Items.FindByValue(shedule.PERIOD_TYPE.ToString()));
            PeriodTextBox.Text = shedule.PERIOD.ToString();
            CardNameDropDown.SelectedIndex = CardNameDropDown.Items.IndexOf(CardNameDropDown.Items.FindByValue(shedule.CARD_ID.ToString()));
            EmailAddressTextBox.Text = shedule.EMAIL_ADDRESS;
        }
        catch (Exception ex)
        {
            //Session["Settings_GeneralTabException"] = ex;
            //RaiseBubbleEvent(null, new EventArgs());
            throw ex;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    public void DeleteShedule()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        try
        {
            dataBlock.OpenConnection();
            int shedId = Convert.ToInt32(Selected_SheduleDataGrid_Index.Value);
            dataBlock.emailScheduleTable.DeleteShedule(shedId);
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    public void CreateNewShedule()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        try
        {
            dataBlock.OpenConnection();
            ShedulesDataGridPanel.Visible = false;
            ShedulesEdit.Visible = true;
            LoadAllDropDowns(dataBlock);
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    public void ReportTypeDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        try
        {
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            dataBlock.OpenConnection();
            //Загружаем ReportNameDropDown
            ReportNameDropDown.Items.Clear();
            List<int> Ids = dataBlock.reportsTable.GetAllReportsIds(Convert.ToInt32(ReportTypeDropDown.SelectedValue));
            foreach (int id in Ids)
            {
                ReportNameDropDown.Items.Add(new ListItem(dataBlock.reportsTable.GetReportName(id), id.ToString()));
            }
            //Загружаем Названия карт. Если Это ДДД водителя или PLF, то карты водителей, если ДДД ТС- то карты ТС
            CardNameDropDown.Items.Clear();
            DeviceNumber_PLFONLY_DropDown.Items.Clear();
            if (Convert.ToInt32(ReportTypeDropDown.SelectedValue) == 2)//Если тип отчета - ДДД ТС(он всегда теперь должен быть с ID==2.
            {
                Ids = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.vehicleCardTypeId);
                foreach (int id in Ids)
                {
                    CardNameDropDown.Items.Add(new ListItem(dataBlock.cardsTable.GetCardName(id), id.ToString()));
                }
            }
            else
            {
                Ids = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.driversCardTypeId);
                foreach (int id in Ids)
                {
                    CardNameDropDown.Items.Add(new ListItem(dataBlock.cardsTable.GetCardName(id), id.ToString()));
                }
                if (Convert.ToInt32(ReportTypeDropDown.SelectedValue) == 3)//Если выбран тип отчета - PLF(3 всегда), то нужно загрузить кроме названия карт еще и название бортовых устройств.
                {
                    //dataBlock.cardsTable.
                }
            }
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
        finally
        {
            ShedulesEdit_UpdatePanel.Update();
            dataBlock.CloseConnection();
        }
    }

    protected void SheduleDataGrid_RadioButton_Checked(object sender, EventArgs e)
    {
      /*  foreach (DataGridItem oldrow in DriversDataGrid.Items)
        {
            ((RadioButton)oldrow.FindControl("DriversDataGrid_RadioButton")).Checked = false;
        }*/

        //Set the new selected row
        RadioButton rb = (RadioButton)sender;
        DataGridItem row = (DataGridItem)rb.NamingContainer;
        ((RadioButton)row.FindControl("SheduleDataGrid_RadioButton")).Checked = true;
        Selected_SheduleDataGrid_Index.Value = row.Cells[1].Text;
    }

    public void SaveAllNewInformation()
    {
        int periodType = Convert.ToInt32(PeriodTypeDropDown.SelectedValue);
        int period = Convert.ToInt32(PeriodTextBox.Text);
        int cardId = Convert.ToInt32(CardNameDropDown.SelectedValue);
        int reportId = Convert.ToInt32(ReportNameDropDown.SelectedValue);
        int PlfDevice = 0;
        int.TryParse(DeviceNumber_PLFONLY_DropDown.SelectedValue, out PlfDevice);
        string destEmail = EmailAddressTextBox.Text;
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        try
        {
            dataBlock.OpenConnection();
            int userId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
            dataBlock.emailScheduleTable.AddEmailSchedule(orgId, userId, reportId, cardId, period, periodType, destEmail);
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
        finally
        {
            dataBlock.CloseConnection();
        }   
    }

    public void SaveAllUpdatedInformation()
    {
        int periodType = Convert.ToInt32(PeriodTypeDropDown.SelectedValue);
        int period = Convert.ToInt32(PeriodTextBox.Text);
        int cardId = Convert.ToInt32(CardNameDropDown.SelectedValue);
        int reportId = Convert.ToInt32(ReportNameDropDown.SelectedValue);
        int PlfDevice = 0;
        int.TryParse(DeviceNumber_PLFONLY_DropDown.SelectedValue, out PlfDevice);
        string destEmail = EmailAddressTextBox.Text;
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        int shedId = 0;
        if (!int.TryParse(Selected_SheduleDataGrid_Index.Value, out shedId))
            throw new Exception("Выберите запись!");

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        try
        {
            dataBlock.OpenConnection();
            int userId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
            dataBlock.emailScheduleTable.EditEmailSchedule(shedId ,orgId, userId, reportId, cardId, period, periodType, destEmail);
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
        finally
        {
            dataBlock.CloseConnection();
        }   
    }
}
