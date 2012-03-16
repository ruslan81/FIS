using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using System.Configuration;

public partial class Administrator_Administration : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ((UserControlsForAll_BlueButton)Page.Master.FindControl("AdministrationMasterButt")).Enabled = false;
                //((LinkButton)Page.Master.FindControl("AdministrationMasterButt")).Enabled = false;
                string connectionString = System.Configuration.ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
                BLL.DataBlock dataBlock = new BLL.DataBlock(connectionString, "STRING_EN");
                dataBlock.OpenConnection();
                int userId = dataBlock.usersTable.Get_UserID_byName(User.Identity.Name);
                int orgId = dataBlock.usersTable.Get_UserOrgId(userId);
                Session["CURRENT_ORG_ID"] = orgId;
                //ORG NAME сверху
                string curOrgName = "";
                curOrgName = dataBlock.usersTable.Get_UserOrgName(userId);
                ((Label)Master.FindControl("CompanyHeaderOrgName")).Text = curOrgName;
                //USER NAME сверху
                curOrgName = dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_Surname);
                curOrgName += " " + dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_Name);
                ((Label)Master.FindControl("UserNameHeaderName")).Text = curOrgName;
                ///////////////////////////
                ((Panel)Master.FindControl("MainConditionsPanel")).Visible = false;
                ((Panel)Master.FindControl("AdditionalConditionsPanel")).Visible = false;
                //AdminAccordion.SelectedIndex = 0;

                //Прячем/показываем нужны меню, в зависимости от типа организации
             /*   int orgTypeId = dataBlock.organizationTable.GetOrgTypeId(orgId);
                if (orgTypeId != dataBlock.organizationTable.DealerTypeId && orgTypeId != dataBlock.organizationTable.PredealerTypeId )
                {
                   // AccountsAccordionPane2_Header.Visible = false;
                    AccountsAccordionPane2_Panel.Visible = false;
                }
                //*/
                dataBlock.CloseConnection();

                InvisibleAccordionButton_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }

    protected void SaveAccountsHandler(object s, EventArgs e)
    {
        LoadAccountsTree();
        AccountsTreeUpdatePanel.Update();
    }

    protected void InvisibleAccordionButton_Click(object sender, EventArgs e)
    {
        try
        {
            UsersTab_UserControl1.Visible = false;
            //DealersTab_UserControl1.Visible = false;
            GeneralData_UserControl1.Visible = false;
            LogTab_UserControl1.Visible = false;
            InvoicesTab_UserControl1.Visible = false;
            ReportsTab_UserControl1.Visible = false;
            //ClientsTab_UserControl1.Visible = false;
            AccountsTab_UserControl1.Visible = false;

            // int accordionSelectedPaneIndex = AdminAccordion.SelectedIndex;
            int accordionSelectedPaneIndex = Convert.ToInt32(AccordionSelectedPane.Value);
            switch (accordionSelectedPaneIndex)
            {
                case 0://General Data
                    {
                        //AdminAccordion.SelectedIndex = 0;
                        GeneralData_UserControl1.Visible = true;
                        GeneralData_UserControl1.LoadAllLists();
                    } break;
               /* case 1://Dealers
                    {
                        DealersTab_UserControl1.Visible = true;
                        DealersTab_UserControl1.LoadDealersTable();
                    } break;*/
                case 2://Users
                    {
                        //AdminAccordion.SelectedIndex = 2;
                        UsersTab_UserControl1.Visible = true;
                        UsersTab_UserControl1.LoadUsersTable();
                    } break;
                case 3://Reports
                    {
                        ReportsTab_UserControl1.Visible = true;
                        ReportsTab_UserControl1.LoadReportsTable();
                    } break;
                case 4://Bills
                    {
                        InvoicesTab_UserControl1.Visible = true;
                        InvoicesTab_UserControl1.LoadInvoicesTable();
                    } break;
                case 5://Log
                    {
                        LogTab_UserControl1.Visible = true;
                        LogTab_UserControl1.LoadUsersTable();
                    } break;
                /*case 6://Clients
                    {
                        ClientsTab_UserControl1.Visible = true;
                        ClientsTab_UserControl1.LoadClientsTable();
                    } break;*/
                case 7://Accounts
                    {
                        LoadAccountsTree();
                        AccountsTab_UserControl1.Visible = true;
                        AccountsTab_UserControl1.LoadAccountsTable();
                        AccountsTreeUpdatePanel.Update();
                    } break;
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }

    private void LoadAccountsTree()
    {
        string connectionString = System.Configuration.ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        BLL.DataBlock dataBlock = new BLL.DataBlock(connectionString, "STRING_EN");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        AccountsTreeView.Nodes.Clear();
        dataBlock.OpenConnection();
        string orgName = dataBlock.organizationTable.GetOrganizationName(orgId);
        AccountsTreeView.Nodes.Add(new TreeNode(orgName, orgId.ToString()));

        AccountsTreeView.Nodes[0].ChildNodes.Add(new TreeNode("Dealers"));
        AccountsTreeView.Nodes[0].ChildNodes.Add(new TreeNode("Clients"));

        List<int> accountsIds = new List<int>();
        accountsIds = dataBlock.organizationTable.Get_AllDealersId(orgId);
        accountsIds.AddRange(dataBlock.organizationTable.Get_AllOrganizationsId((orgId)));
        //Session["AccountsTab_UserControl_AccountsIds"] = accountsIds;
        int orgTypeId = -1;
        foreach (int id in accountsIds)
        {
            orgTypeId = dataBlock.organizationTable.GetOrgTypeId(id);
            if (orgTypeId == dataBlock.organizationTable.DealerTypeId || orgTypeId == dataBlock.organizationTable.SubdealerTypeId)
                AccountsTreeView.Nodes[0].ChildNodes[0].ChildNodes.Add(new TreeNode(dataBlock.organizationTable.GetOrganizationName(id), id.ToString()));
            else
                AccountsTreeView.Nodes[0].ChildNodes[1].ChildNodes.Add(new TreeNode(dataBlock.organizationTable.GetOrganizationName(id), id.ToString()));
        }
        dataBlock.CloseConnection();

        AccountsTreeView.Nodes[0].Expand();
    }

    protected void AccountsTreeView_NodeChanged(object s, EventArgs e)
    {
        try
        {
            int selectedOrgId = -1;
            if (int.TryParse(AccountsTreeView.SelectedValue, out selectedOrgId))
            {
                AccountsTab_UserControl1.SelectItemInDataGrid(selectedOrgId);
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            StatusUpdatePanel.Update();
        }
    }

    protected override bool OnBubbleEvent(object source, EventArgs args)
    {
        if (Session["AdministrationTabException"] != null)
        {
            if (Session["AdministrationTabException"].ToString() != "HideThisWindow")
            {
                //CancelButtonPressed(source, args);
                Exception ex = (Exception)Session["AdministrationTabException"];
                Status.Text = ex.Message;
                Session["AdministrationTabException"] = null;
                StatusUpdatePanel.Update();
            }
            else
            {
                Session["AdministrationTabException"] = null;
                //CancelButtonPressed(source, args);
            }
        }
        //DataContentUpdatePanel.Update();
       // AccordionUpdatePanel.Update();
        return true;
    }

    protected void GeneralDataLoad(object sender, EventArgs e)
    {
        ClientScriptManager script = Page.ClientScript;
        if (!script.IsStartupScriptRegistered(this.GetType(), "Alert"))
        {
        //    script.RegisterStartupScript(this.GetType(), "Alert", "alert('test!');", true);
        }
    }


    //AJAX BEGIN

    /// <summary>
    ///Получить данные по журналу
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetEvents()
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        HistoryTable historyTable = new HistoryTable(connectionString, "STRING_EN", dataBlock.sqlDb);
        try
        {
            dataBlock.OpenConnection();
            List<MapItem> result = new List<MapItem>();
            List<KeyValuePair<string, int>> actions = historyTable.GetAllActions();
            result.Add(new MapItem("-1","Все"));
            foreach (KeyValuePair<string, int> action in actions)
                result.Add(new MapItem(action.Value.ToString(),action.Key));
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить данные по журналу
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<JournalData> GetJournal(String OrgID, String StartDate, String EndDate, String eventType, String searchString)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        HistoryTable historyTable = new HistoryTable(connectionString, "STRING_EN", dataBlock.sqlDb);

        try
        {
            dataBlock.OpenConnection();
            List<JournalData> result = new List<JournalData>();

            int orgId = Convert.ToInt32(OrgID);
            List<int> usersIds = new List<int>();
            DateTime from = Convert.ToDateTime(StartDate);
            DateTime to = Convert.ToDateTime(EndDate);
            int actionId = Convert.ToInt32(eventType);

            usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.DriverUserTypeId));
            usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.AdministratorUserTypeId));
            usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.ManagerUserTypeId));
            DataTable data = historyTable.GetAllHistorysForUsers(usersIds, from, to, actionId, searchString);
            foreach (DataRow row in data.Rows) {
                JournalData jd = new JournalData();
                jd.dateTime = row["Дата и время"].ToString();
                jd.user = row["Пользователь"].ToString();
                jd.note = row["Описание"].ToString();
                result.Add(jd);
            }

            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }
}
