using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BLL;

public partial class Administrator_Adminisration_UserControls_LogTab_UserControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LogFilterTable_StartDateTextBox.Text = "";
            LogFilterTable_EndDateTextBox.Text = "";
            LogFilterTable_StartTimeTextBox.Text = "";
            LogFilterTable_EndTimeTextBox.Text = "";
            LogFilterTable_NoteTextTextBox.Text = "";
        }
    }

    public void LoadUsersTable()
    {
        string currentLanguage = "STRING_RU";
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, currentLanguage);
        HistoryTable historyTable = new HistoryTable(connectionString, currentLanguage, dataBlock.sqlDb);
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        List<int> usersIds = new List<int>();
        List<UserFromTable> userFromTableList = new List<UserFromTable>();

        dataBlock.OpenConnection();//выборка по типам пользователей
        usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.DriverUserTypeId));
        usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.AdministratorUserTypeId));
        usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.ManagerUserTypeId));
        DataTable data = historyTable.GetAllHistorysForUsers(usersIds);
        LogDataGrid.DataSource = data;
        LogDataGrid.DataBind();
        
        List<KeyValuePair<string, int>> actions = historyTable.GetAllActions();
        LogFilterTable_EventDropDown.Items.Clear();
        
        LogFilterTable_EventDropDown.Items.Add(new ListItem("Все", "-1", true));
        foreach (KeyValuePair<string, int> action in actions)
            LogFilterTable_EventDropDown.Items.Add(new ListItem(action.Key, action.Value.ToString()));

        dataBlock.CloseConnection();
    }

    protected void ApplyLogFilterButton_Click(object sender, EventArgs e)
    {
        string currentLanguage = "STRING_RU";
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, currentLanguage);
        HistoryTable historyTable = new HistoryTable(connectionString, currentLanguage, dataBlock.sqlDb);
        try
        {
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            List<int> usersIds = new List<int>();
            List<UserFromTable> userFromTableList = new List<UserFromTable>();
            DateTime from;
            DateTime to;
            if (DateTime.TryParse(LogFilterTable_StartDateTextBox.Text, out from))
            {
                if (LogFilterTable_StartTimeTextBox.Text != "")
                    from = from.Add(TimeSpan.Parse(LogFilterTable_StartTimeTextBox.Text));
            }
            else
                from = new DateTime();
            if (DateTime.TryParse(LogFilterTable_EndDateTextBox.Text, out to))
            {
                if (LogFilterTable_EndTimeTextBox.Text != "")
                    to = to.Add(TimeSpan.Parse(LogFilterTable_EndTimeTextBox.Text));
                else
                {
                    to = to.AddHours(23);
                    to = to.AddMinutes(59);
                }
            }
            else
                to = DateTime.Now;

            int actionId = Convert.ToInt32(LogFilterTable_EventDropDown.SelectedValue);
            string searchString = LogFilterTable_NoteTextTextBox.Text.Trim();

            dataBlock.OpenConnection();//выборка по типам пользователей
            usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.DriverUserTypeId));
            usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.AdministratorUserTypeId));
            usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.ManagerUserTypeId));
            DataTable data = historyTable.GetAllHistorysForUsers(usersIds, from, to, actionId, searchString);
            LogDataGrid.DataSource = data;
            LogDataGrid.DataBind();

            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }


   
}
