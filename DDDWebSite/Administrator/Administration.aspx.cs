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
                
                //ORG NAME сверху
                string curOrgName = "";
                curOrgName = dataBlock.usersTable.Get_UserOrgName(userId);
                ((Label)Master.FindControl("CompanyHeaderOrgName")).Text = curOrgName;
                //USER NAME сверху
                curOrgName = dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_Surname);
                curOrgName += " " + dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_Name);
                ((Label)Master.FindControl("UserNameHeaderName")).Text = curOrgName;

                int orgId = dataBlock.usersTable.Get_UserOrgId(userId);
                Session["CURRENT_ORG_ID"] = orgId;

                //выставляем кук, чтобы можно было передать его в метод, вызываемый ч/з ajax
                Response.Cookies["CURRENT_ORG_ID"].Value = Convert.ToString(orgId);
                Response.Cookies["CURRENT_USERNAME"].Value = User.Identity.Name;

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
    ///Получить данные по статистике
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static StatisticData GetStatistic(String OrgID, String UserName)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        InvoiceTable invoiceTable = new InvoiceTable(connectionString, "STRING_EN", dataBlock.sqlDb);
        try
        {
            dataBlock.OpenConnection();
            int orgId = Convert.ToInt32(OrgID);
            int userId = dataBlock.usersTable.Get_UserID_byName(UserName);
            StatisticData result = new StatisticData();

            result.usersCount = dataBlock.usersTable.Get_AllUsersId(orgId).Count.ToString();
            result.driversCount = dataBlock.cardsTable.GetAllCardIds(orgId,dataBlock.cardsTable.driversCardTypeId).Count.ToString();
            result.vehiclesCount = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.vehicleCardTypeId).Count.ToString();
            result.invoicesCount = invoiceTable.GetAllInvoices(orgId).Count.ToString();

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
    /// Получить данные по сообщения
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MessageData> GetMessages(String UserName)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        InvoiceTable invoiceTable = new InvoiceTable(connectionString, "STRING_EN", dataBlock.sqlDb);
        try
        {
            dataBlock.OpenConnection();
            //int orgId = Convert.ToInt32(OrgID);
            int userId = dataBlock.usersTable.Get_UserID_byName(UserName);
            List<MessageData> result = new List<MessageData>();

            List<int> ids = dataBlock.usersTable.GetAllMessagesIds(userId);
            foreach(int id in ids)
            {
                MessageData md = new MessageData();
                md.id = id;
                md.sender = dataBlock.usersTable.GetMessageSender(id);
                md.topic = dataBlock.usersTable.GetMessageTopic(id);
                md.date = dataBlock.usersTable.GetMessageDate(id).ToShortDateString();
                md.endDate = dataBlock.usersTable.GetMessageEndDate(id).ToShortDateString();
                result.Add(md);
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

    /// <summary>
    /// Удалить сообщения
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static void DeleteMessages(List<MapItem> messageIds)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            foreach (MapItem id in messageIds)
            {
                dataBlock.usersTable.DeleteMessage(Convert.ToInt32(id.Value));
            }
        }
        catch (Exception ex)
        {
            return;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    /// Получить данные по дилерам
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<DealerData> GetDealers(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        
        try
        {
            dataBlock.OpenConnection();
            int orgId = Convert.ToInt32(OrgID);
            //int userId = dataBlock.usersTable.Get_UserID_byName(UserName);
            List<DealerData> result = new List<DealerData>();

            List<int> ids=dataBlock.organizationTable.Get_AllDealersId(orgId);
            foreach (int id in ids) {
                DealerData dd = new DealerData();
                dd.id = id;
                dd.name = dataBlock.organizationTable.GetOrganizationName(id);
                //dd.login = dataBlock.usersTable.Get_UserName(id);
                dd.country = dataBlock.organizationTable.GetOrgCountryId(id).ToString();
                dd.city = dataBlock.organizationTable.GetOrgRegionId(id).ToString();
                
                DateTime date = new DateTime();
                if (DateTime.TryParse(dataBlock.organizationTable.GetAdditionalOrgInfo(id, 1), out date))
                { dd.date = date.ToShortDateString(); }
                else { dd.date = "Неизвестно"; }
                if (DateTime.TryParse(dataBlock.organizationTable.GetAdditionalOrgInfo(id, 2), out date))
                { dd.endDate = date.ToShortDateString();}
                else { dd.endDate = "Неизвестно"; }

                result.Add(dd);
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

    /// <summary>
    /// Получить данные по пользователям
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<UserData> GetUsers(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            dataBlock.OpenConnection();
            int orgId = Convert.ToInt32(OrgID);
            //int userId = dataBlock.usersTable.Get_UserID_byName(UserName);
            List<UserData> result = new List<UserData>();

            List<int> ids = dataBlock.usersTable.Get_AllUsersId(orgId);
            foreach (int id in ids) {
                UserData ud = new UserData();
                ud.dealer = dataBlock.organizationTable.GetOrganizationName(id);
                ud.login = dataBlock.usersTable.Get_UserName(id);
                ud.id = id;
                int userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
                ud.name = dataBlock.usersTable.GetUserInfoValue(id, userInfoId);
                userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Surname);
                ud.surname = dataBlock.usersTable.GetUserInfoValue(id, userInfoId);
                userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Patronimic);
                ud.patronimic = dataBlock.usersTable.GetUserInfoValue(id, userInfoId);
                userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_RegDate);
                ud.date = dataBlock.usersTable.GetUserInfoValue(id, userInfoId);
                ud.roleId = dataBlock.usersTable.Get_UserTypeId(id);
                ud.role = dataBlock.usersTable.Get_UserTypeStr(id);

                DateTime date = dataBlock.usersTable.Get_TimeConnect(id);
                if (date == null)
                {
                    ud.state = "Отключен";
                }
                else {
                    ud.state = "Подключен " + date.ToShortDateString() + " " + date.ToShortTimeString();
                }
                
                result.Add(ud);
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

    /// <summary>
    ///Получить данные по событиям журнала
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
            result.Add(new MapItem("-1", "Все"));
            foreach (KeyValuePair<string, int> action in actions)
                result.Add(new MapItem(action.Value.ToString(), action.Key));
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
    ///Получить данные по статусам счетов
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetInvoiceStatuses()
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        InvoiceTable invoiceTable = new InvoiceTable(connectionString, "STRING_EN", dataBlock.sqlDb);
        try
        {
            dataBlock.OpenConnection();
            List<MapItem> result = new List<MapItem>();

            List<Int32> statuses = invoiceTable.GetAllInvoiceStatuses();
            foreach (int status in statuses)
            {
                string name = invoiceTable.GetInvoiceStatusName(status);
                result.Add(new MapItem(status.ToString(), name));
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

    /// <summary>
    ///Получить общие данные
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static UserGeneralData GetGeneralData(String OrgID, String UserName)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            dataBlock.OpenConnection();
            int orgId = Convert.ToInt32(OrgID);
            //int userId = Convert.ToInt32(UserName);
            int userId = dataBlock.usersTable.Get_UserID_byName(UserName);
            UserGeneralData ud = new UserGeneralData();
            //ud.connectDate=dataBlock.usersTable.Get_TimeConnect(userId).ToString();

            DateTime date = dataBlock.usersTable.Get_TimeConnect(userId);
            ud.connectDate = date.ToLongDateString() + " " + date.ToShortTimeString();
            if (DateTime.TryParse(dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, 1), out date))
            { ud.registerDate = date.ToLongDateString() + " " + date.ToShortTimeString(); }
            else { ud.registerDate = "Неизвестно"; }
            if (DateTime.TryParse(dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, 2), out date))
            { ud.endDate = date.ToLongDateString(); }
            else { ud.endDate = "Неизвестно"; }

            ud.licenseType = "Flat";

            return ud;
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
    ///Получить детальные общие данные
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static UserGeneralDetailedData GetGeneralDetailedData(String OrgID, String UserName)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            dataBlock.OpenConnection();
            int orgId = Convert.ToInt32(OrgID);;
            int userId = dataBlock.usersTable.Get_UserID_byName(UserName);
            UserGeneralDetailedData ud = new UserGeneralDetailedData();

            string dealerName;
            int dealerId;
            int userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_DealerId);
            string temp = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            if (int.TryParse(dataBlock.usersTable.GetUserInfoValue(userId, userInfoId), out dealerId))
            {
                dealerName = dataBlock.organizationTable.GetOrganizationName(dealerId);
            }
            else
                dealerName = "???";
            if (dealerName.Trim() == "")
                dealerName = "???";

            ud.orgName = dealerName;

            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
            ud.orgLogin = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            ud.password = dataBlock.usersTable.Get_UserPassword(userId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Country);
            ud.country = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_City);
            ud.city = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_ZIP);
            ud.index = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_AddressOne);
            ud.address1 = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_AddressTwo);
            ud.address2 = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.OrgInfo_PhoneNumber);
            ud.phone = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.OrgInfo_Fax);
            ud.fax = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.OrgInfo_Email);
            ud.mail = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.OrgInfo_TimeZone);
            ud.timeZone = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
            return ud;
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
    ///Сохранить детальные общие данные
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static void SaveGeneralDetailedData(String OrgID, String UserName, UserGeneralDetailedData ud)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            dataBlock.OpenConnection();
            int orgId = Convert.ToInt32(OrgID);
            int userId = dataBlock.usersTable.Get_UserID_byName(UserName);

            int userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_DealerId);
            //dataBlock.usersTable.EditUserInfo(userId,userInfoId,ud.orgName);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, ud.orgLogin);

            dataBlock.usersTable.EditUserPassword(userId,ud.password);

            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Country);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, ud.country);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_City);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, ud.city);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_ZIP);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, ud.index);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_AddressOne);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, ud.address1);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_AddressTwo);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, ud.address2);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.OrgInfo_PhoneNumber);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, ud.phone);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.OrgInfo_Fax);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, ud.fax);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.OrgInfo_Email);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, ud.mail);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.OrgInfo_TimeZone);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, ud.timeZone);
        }
        catch (Exception ex)
        {
            return;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Сохранить данные по дилерам
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static void SaveDealersData(String OrgID,  List<DealerData> list)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            dataBlock.OpenConnection();
            int orgId = Convert.ToInt32(OrgID);
            foreach(DealerData dd in list){
                dataBlock.organizationTable.SetOrganizationName(dd.name,dd.id);
                dataBlock.organizationTable.SetOrgCountryAndRegion(dd.id, Convert.ToInt32(dd.country), Convert.ToInt32(dd.city));
                dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(dd.id, 2, dd.endDate);
            }
        }
        catch (Exception ex)
        {
            return;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Создать нового дилера
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static void CreateNewDealer(String OrgID, DealerData data)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            dataBlock.OpenConnection();
            int orgId = Convert.ToInt32(OrgID);
            int id=dataBlock.organizationTable.AddNewOrganization(data.name, 5, Convert.ToInt32(data.country), Convert.ToInt32(data.city),orgId);
            dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(id, 1, data.date);
            dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(id, 2, data.endDate);
        }
        catch (Exception ex)
        {
            return;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Создать нового дилера
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static void DeleteDealers(String OrgID, List<MapItem> ids)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            dataBlock.OpenConnection();
            int orgId = Convert.ToInt32(OrgID);
            foreach (MapItem id in ids)
            {
                dataBlock.organizationTable.DeleteOrganization(Convert.ToInt32(id.Value));
            }
        }
        catch (Exception ex)
        {
            return;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить список стран
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetCountries()
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        List<MapItem> result = new List<MapItem>();
        try
        {
            dataBlock.OpenConnection();
            List<int> ids = dataBlock.usersTable.GetAllCountries();
            foreach (int id in ids) {
                string name=dataBlock.usersTable.GetCountryName(id);
                result.Add(new MapItem(id.ToString(),name));
            }
            return result;
        }
        catch (Exception ex)
        {
            return result;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить список городов
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetCities(String CountryID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        List<MapItem> result = new List<MapItem>();
        try
        {
            int countryId = Convert.ToInt32(CountryID);

            dataBlock.OpenConnection();
            List<int> ids = dataBlock.usersTable.GetAllCities(countryId);
            foreach (int id in ids)
            {
                string name = dataBlock.usersTable.GetCityName(id);
                result.Add(new MapItem(id.ToString(), name));
            }
            return result;
        }
        catch (Exception ex)
        {
            return result;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить список типов пользователей
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetUserTypes()
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        List<MapItem> result = new List<MapItem>();
        try
        {
            dataBlock.OpenConnection();
            List<KeyValuePair<string,int>> ids = dataBlock.usersTable.GetAllUsersTypes();
            foreach (KeyValuePair<string, int> id in ids)
            {
                result.Add(new MapItem(id.Value.ToString(), id.Key));
            }
            return result;
        }
        catch (Exception ex)
        {
            return result;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить список часовых зон
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetTimeZones()
    {
        List<MapItem> result = new List<MapItem>();
        try
        {
            int key=1;
            foreach (TimeZoneInfo timeZone in TimeZoneInfo.GetSystemTimeZones())
            {
                result.Add(new MapItem(key.ToString(), timeZone.DisplayName));
                key++;
            }
            return result;
        }
        catch (Exception ex)
        {
            return result;
        }
        finally
        {
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
            foreach (DataRow row in data.Rows)
            {
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

    /// <summary>
    ///Получить данные по счетам
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<InvoiceData> GetInvoices(String OrgID, String StartDate, String EndDate, String statusType)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        InvoiceTable invoiceTable = new InvoiceTable(connectionString, "STRING_EN", dataBlock.sqlDb);

        try
        {
            dataBlock.OpenConnection();
            List<InvoiceData> result = new List<InvoiceData>();

            int orgId = Convert.ToInt32(OrgID);
            List<int> usersIds = new List<int>();
            DateTime from = Convert.ToDateTime(StartDate);
            DateTime to = Convert.ToDateTime(EndDate);
            int statusId = Convert.ToInt32(statusType);

            List<int> invoices = invoiceTable.GetAllInvoices(orgId);
            foreach (int invoice in invoices)
            {
                InvoiceData id = new InvoiceData();
                id.name = invoiceTable.GetInvoiceName(invoice);
                DateTime begDate = invoiceTable.GetDateInvoice(invoice);
                if (begDate.CompareTo(from) < 0 || begDate.CompareTo(to) > 0)
                {
                    continue;
                }
                id.beginDate = begDate.ToString();
                id.endDate = invoiceTable.GetDatePaymentTerm(invoice).ToString();
                id.payDate = invoiceTable.GetDatePayment(invoice);
                id.statusId = invoiceTable.GetInvoiceStatusId(invoice);
                if (statusId != 0 && id.statusId != statusId)
                {
                    continue;
                }
                id.status = invoiceTable.GetInvoiceStatusName(id.statusId);
                result.Add(id);
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
