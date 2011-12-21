using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BLL;

public partial class Administrator_Settings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Choises_CANCEL_Button.ButtOnClick += new EventHandler(CancelButtonPressed);
            Choises_ADD_Button.ButtOnClick += new EventHandler(AddButtonPressed);
            Choises_EDIT_Button.ButtOnClick += new EventHandler(EditButtonPressed);
            Choises_SAVE_Button.ButtOnClick += new EventHandler(SaveButtonPressed);
            Choises_DELETE_Button.ButtOnClick += new EventHandler(DeleteButtonPressed);

            if (!IsPostBack)
            {
                UserControlsForAll_BlueButton pan = ((UserControlsForAll_BlueButton)Page.Master.FindControl("SettingsMasterButt"));
                pan.Enabled = false;
                //((LinkButton)Page.Master.FindControl("SettingsMasterButt")).Enabled = false;
                TreeViews_LoadList();
                ReminderTreeView.Nodes[0].Selected = true;
                UsersTreeView.Nodes[0].Select();
                UsersTreeView_NodeChanged(null, null);
                Load_DriverTab();


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

                ///////////////////////////
                dataBlock.CloseConnection();
                ((Panel)Master.FindControl("AdditionalConditionsPanel")).Visible = false;

                Session["Settings_GeneralTabException"] = null;
                Status.Text = "";
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            StatusUpdatePanel.Update();
        }
    }

    //AJAX BEGIN

    /// <summary>
    ///Получить Общие настройки
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<KeyValuePair<int,MapItem>> GetGeneralSettings(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            int orgId = Convert.ToInt32(OrgID);

            dataBlock.organizationTable.OpenConnection();
            List<KeyValuePair<string, int>> orgInfo = new List<KeyValuePair<string, int>>();
            orgInfo = dataBlock.organizationTable.GetAllOrgInfos();
            List<KeyValuePair<int, MapItem>> generalSettings = new List<KeyValuePair<int, MapItem>>();

            generalSettings.Add(new KeyValuePair<int,MapItem>(0, new MapItem("Наименование организации", dataBlock.organizationTable.GetOrganizationName(orgId))));
            generalSettings.Add(new KeyValuePair<int,MapItem>(0, new MapItem("Страна", dataBlock.organizationTable.GetOrgCountryName(orgId))));
            generalSettings.Add(new KeyValuePair<int,MapItem>(0, new MapItem("Регион", dataBlock.organizationTable.GetOrgRegionName(orgId))));

            foreach (KeyValuePair<string, int> pair in orgInfo)
            {
                String value=dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, pair.Value);
                generalSettings.Add(new KeyValuePair<int, MapItem>(pair.Value, new MapItem(pair.Key,value)));
            }

            return generalSettings;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Сохранить Общие настройки
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool SaveGeneralSettings(String OrgID, List<MapItem> GeneralSettings)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.organizationTable.OpenConnection();
            int orgID = int.Parse(OrgID);
            foreach (MapItem item in GeneralSettings)
            {
                dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgID, int.Parse(item.Key.Trim()), item.Value.Trim());
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить настройки групп
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<GroupData> GetGroupsSettings(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            List<GroupData> result = new List<GroupData>();
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);

            List<int> ids = dataBlock.cardsTable.GetAllGroupIds(orgID);
            int num = 0;
            foreach (int i in ids)
            {
                GroupData gd = new GroupData(i);
                gd.Name = dataBlock.cardsTable.GetGroupNameById(i);
                gd.Comment = dataBlock.cardsTable.GetGroupCommentById(i);
                gd.Number = ++num;
                gd.cardType = dataBlock.cardsTable.GetGroupCardTypeById(i);
                result.Add(gd);
            }

            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Сохранить Настройки групп
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool SaveGroupSettings(String OrgID, List<GroupData> GroupSettings)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            foreach (GroupData item in GroupSettings)
            {
                dataBlock.cardsTable.UpdateGroup(item.grID,item.Name,item.Comment,item.cardType);
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Удаление групп
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool DeleteGroups(String OrgID, List<MapItem> GroupIDs)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            for (int i = 0; i < GroupIDs.Count; i++)
            {
                int groupID = int.Parse(GroupIDs[i].Value);
                dataBlock.cardsTable.DeleteGroup(orgID, groupID);
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Создание групп
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool CreateGroup(String OrgID, String Name, String Comment, String CardType)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            int cardType = int.Parse(CardType);

            dataBlock.cardsTable.CreateGroup(orgID,Name,Comment,cardType);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить настройки водителей
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<CardData> GetDriversSettings(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        List<CardData> result = new List<CardData>();
        try
        {
            //dataBlock.organizationTable.OpenConnection();
            int orgID = int.Parse(OrgID);

            dataBlock.OpenConnection();
            List<int> cardsList = dataBlock.cardsTable.GetAllCardIds(orgID, dataBlock.cardsTable.driversCardTypeId);
            foreach (int cardId in cardsList)
            {
                CardData gd = new CardData(cardId);
                gd.Name = dataBlock.cardsTable.GetCardName(cardId);
                gd.Number = dataBlock.cardsTable.GetCardNumber(cardId);
                gd.Comment = dataBlock.cardsTable.GetCardNote(cardId);
                gd.groupID = dataBlock.cardsTable.GetCardGroupID(cardId);
                gd.groupName = dataBlock.cardsTable.GetGroupNameById(gd.groupID);
                result.Add(gd);
            }
            //dataBlock.CloseConnection();
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Сохранить Настройки водителей
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool SaveDriverSettings(String OrgID, List<CardData> DriverSettings)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            foreach (CardData item in DriverSettings)
            {
                dataBlock.cardsTable.ChangeCardName(item.Name,item.grID);
                dataBlock.cardsTable.ChangeCardNumber(item.Number, item.grID, 0);
                dataBlock.cardsTable.ChangeCardComment(item.Comment, item.grID);
                dataBlock.cardsTable.ChangeCardGroup(item.groupID, item.grID);
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Удаление водителей
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool DeleteDrivers(String OrgID, List<MapItem> DriverIDs)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            for (int i = 0; i < DriverIDs.Count; i++)
            {
                int driverID = int.Parse(DriverIDs[i].Value);
                dataBlock.cardsTable.DeleteCardAndAllFiles(driverID);
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить настройки ТС
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<CardData> GetTransportsSettings(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        List<CardData> result = new List<CardData>();
        try
        {
            //dataBlock.organizationTable.OpenConnection();
            int orgID = int.Parse(OrgID);

            dataBlock.OpenConnection();
            List<int> cardsList = dataBlock.cardsTable.GetAllCardIds(orgID, dataBlock.cardsTable.vehicleCardTypeId);
            foreach (int cardId in cardsList)
            {
                CardData gd = new CardData(cardId);
                gd.Name = dataBlock.cardsTable.GetCardName(cardId);
                gd.Number = dataBlock.cardsTable.GetCardNumber(cardId);
                gd.Comment = dataBlock.cardsTable.GetCardNote(cardId);
                gd.groupID = dataBlock.cardsTable.GetCardGroupID(cardId);
                gd.groupName = dataBlock.cardsTable.GetGroupNameById(gd.groupID);
                result.Add(gd);
            }
            //dataBlock.CloseConnection();
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Сохранить Настройки ТС
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool SaveTransportSettings(String OrgID, List<CardData> TransportSettings)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            foreach (CardData item in TransportSettings)
            {
                dataBlock.cardsTable.ChangeCardName(item.Name, item.grID);
                dataBlock.cardsTable.ChangeCardNumber(item.Number, item.grID, 0);
                dataBlock.cardsTable.ChangeCardComment(item.Comment, item.grID);
                dataBlock.cardsTable.ChangeCardGroup(item.groupID, item.grID);
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Удаление ТС
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool DeleteTransports(String OrgID, List<MapItem> TransportIDs)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.organizationTable.OpenConnection();
            int orgID = int.Parse(OrgID);
            for (int i = 0; i < TransportIDs.Count; i++)
            {
                int transportID = int.Parse(TransportIDs[i].Value);
                dataBlock.cardsTable.DeleteCardAndAllFiles(transportID);
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получение списка групп
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetGroupListDrivers(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            List<MapItem> result = new List<MapItem>();
            List<int> ids=dataBlock.cardsTable.GetAllGroupIds(orgID,dataBlock.cardsTable.driversCardTypeId);
            foreach (int index in ids) {
                string name = dataBlock.cardsTable.GetGroupNameById(index);
                result.Add(new MapItem(Convert.ToString(index),name));
            }
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получение списка групп
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetGroupListTransports(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            List<MapItem> result = new List<MapItem>();
            List<int> ids = dataBlock.cardsTable.GetAllGroupIds(orgID, dataBlock.cardsTable.vehicleCardTypeId);
            foreach (int index in ids)
            {
                string name = dataBlock.cardsTable.GetGroupNameById(index);
                result.Add(new MapItem(Convert.ToString(index), name));
            }
            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Создание карты водителя
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool CreateCardDriver(string OrgID, string UserID, CardData data)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            int userID = int.Parse(UserID);
            dataBlock.cardsTable.CreateNewCard(data.Name,data.Number,dataBlock.cardsTable.driversCardTypeId,orgID,data.Comment,userID, data.groupID);
            
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Создание карты ТС
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool CreateCardTransport(string OrgID, string UserID, CardData data)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            int userID = int.Parse(UserID);
            dataBlock.cardsTable.CreateNewCard(data.Name, data.Number, dataBlock.cardsTable.vehicleCardTypeId, orgID, data.Comment, userID, data.groupID);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить настройки по умолчанию
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<SettingsData> GetDefaultSettings()
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        List<SettingsData> result = new List<SettingsData>();
           
        try{
            dataBlock.OpenConnection();
            List<KeyValuePair<string, int>> allKeys = new List<KeyValuePair<string, int>>();
            allKeys = dataBlock.criteriaTable.GetAllCriteria_Name_n_Id();

            CriteriaTable oneCriteria = new CriteriaTable(connectionString, "STRING_EN", dataBlock.sqlDb);

            foreach (KeyValuePair<string, int> key in allKeys)
            {
                oneCriteria = dataBlock.criteriaTable.LoadCriteria(key.Value);

                SettingsData sd = new SettingsData(oneCriteria.KeyId);
                sd.MeasureName=oneCriteria.MeasureName;
                sd.CriteriaName = oneCriteria.CriteriaName;
                sd.CriteriaNote = oneCriteria.CriteriaNote;
                sd.MinValue = oneCriteria.MinValue;
                sd.MaxValue = oneCriteria.MaxValue;

                result.Add(sd);
            }

            return result;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Сохранить настройки по умолчанию
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static void SaveDefaultSettings(List<SettingsData> DefaultSettings)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            foreach (SettingsData data in DefaultSettings)
            {
                dataBlock.criteriaTable.EditCriteria(data.keyID, data.CriteriaNote, data.MinValue, data.MaxValue);
            }
        }
        catch (Exception ex)
        {
            return;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }
    //AJAX END

    private void TreeViews_LoadList()
    {
        UsersTreeView.Nodes.Add( new TreeNode("Общие", "General"));
        UsersTreeView.Nodes.Add(new TreeNode("Группы", "Groups"));
        UsersTreeView.Nodes.Add(new TreeNode("Водители", "Drivers"));
        UsersTreeView.Nodes.Add(new TreeNode("ТС", "Vehicles"));
        UsersTreeView.Nodes.Add(new TreeNode("Установки по умолчанию", "Factor"));
        
        AdditionalTreeView.Nodes.Add(new TreeNode("POI", "POI"));
        AdditionalTreeView.Nodes.Add(new TreeNode("Маршруты", "Route"));

        ReminderTreeView.Nodes.Add(new TreeNode("Отправка отчетов на Email", "EmailSchedule"));
        ReminderTreeView.Nodes.Add(new TreeNode("Превышение скорости", "OverSpeeding"));
        ReminderTreeView.Nodes.Add(new TreeNode("Превышение оборотов", "OverRPM"));
        ReminderTreeView.Nodes.Add(new TreeNode("Превышение времени движения", "OverTime"));
        ReminderTreeView.Nodes.Add(new TreeNode("Превышение расхода топлива", "OverFuelCons"));
        ReminderTreeView.Nodes.Add(new TreeNode("Следующее ТО", "NextTO"));
        ReminderTreeView.Nodes.Add(new TreeNode("Выезд из геозоны", "OutOfGeoZone"));
        ReminderTreeView.Nodes.Add(new TreeNode("Нарушение маршрута", "OverRoute"));    
    }

    protected void UsersTreeView_NodeChanged(object sender, EventArgs e)
    {
        try
        {
            GeneralTab1.Visible = false;
            UserGroupsTab1.Visible = true;
            UserDriversTab1.Visible = false;
            UserVehicleTab1.Visible = false;
            Coefficient1.Visible = false;
            EmailSheduler1.Visible = false;
            HideButtons(false);
            EditButtonPressed_EnableButtons(false);
            ReminderOverSpeedingTab1.Visible = false;

            return;

            if (ReminderTreeView.SelectedNode != null)//тут все Деревья из других закладок аккордиона
            {
                ReminderTreeView.SelectedNode.Selected = false;
            }
            if (AdditionalTreeView.SelectedNode != null)
            {
                AdditionalTreeView.SelectedNode.Selected = false;
            }
            if (UsersTreeView.SelectedNode != null)
            {
                SettingName.Text = UsersTreeView.SelectedNode.Text;
                switch (UsersTreeView.SelectedValue)
                {
                    case "General":
                        {
                            GeneralTab1.Visible = true;
                        } break;
                    case "Groups":
                        {
                            UserGroupsTab1.LoadAllGroups();
                            UserGroupsTab1.Visible = true;
                        } break;
                    case "Drivers":
                        {
                            UserDriversTab1.LoadDriversDataGrid();
                            UserDriversTab1.Visible = true;                           
                        } break;
                    case "Vehicles":
                        {
                            UserVehicleTab1.LoadVehiclesDataGrid();
                            UserVehicleTab1.Visible = true;     
                        } break;
                    case "Factor":
                        {
                            Coefficient1.LoadKoefList();
                            Coefficient1.Visible = true;
                            HideButtons(true);
                        } break;
                }
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        StatusUpdatePanel.Update();
    }

    protected void ReminderTreeView_NodeChanged(object sender, EventArgs e)
    {
        try
        {
            ReminderOverSpeedingTab1.Visible = false;
            EmailSheduler1.Visible = false;

            if (UsersTreeView.SelectedNode != null)//тут все контролы из других закладок
            {
                UsersTreeView.SelectedNode.Selected = false;
                GeneralTab1.Visible = false;
                UserGroupsTab1.Visible = false;
                UserDriversTab1.Visible = false;
                UserVehicleTab1.Visible = false;
                Coefficient1.Visible = false;
            }
            if (AdditionalTreeView.SelectedNode != null)
            {
                AdditionalTreeView.SelectedNode.Selected = false;
            }
            if (ReminderTreeView.SelectedNode != null)
            {
                SettingName.Text = ReminderTreeView.SelectedNode.Text;
                switch (ReminderTreeView.SelectedValue)
                {
                    case "EmailSchedule":
                        {
                            EmailSheduler1.LoadShedulesDataGrid();
                            EmailSheduler1.Visible = true;
                           // Choises_DELETE_Button.Visible = true;
                        } break;
                    case "OverSpeeding":
                        {
                            ReminderOverSpeedingTab1.Visible = true;
                        } break;
                    case "OverRPM":
                        { } break;
                    case "OverTime":
                        { } break;
                    case "OverFuelCons":
                        { } break;
                    case "NextTO":
                        { } break;
                    case "OutOfGeoZone":
                        { } break;
                    case "OverRoute":
                        { } break;
                }
            }
            EditButtonPressed_EnableButtons(false);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            StatusUpdatePanel.Update();
        }
    }

    protected void AdditionalTreeView_NodeChanged(object sender, EventArgs e)
    {
        try
        {
            EditButtonPressed_EnableButtons(false);
            EmailSheduler1.Visible = false;

            if (UsersTreeView.SelectedNode != null)//тут все контролы из других закладок
            {
                UsersTreeView.SelectedNode.Selected = false;
                GeneralTab1.Visible = false;
                UserGroupsTab1.Visible = false;
                UserDriversTab1.Visible = false;
                UserVehicleTab1.Visible = false;
                Coefficient1.Visible = false;
            }
            if (ReminderTreeView.SelectedNode != null)//тут все контролы из других закладок
            {
                ReminderTreeView.SelectedNode.Selected = false;
                ReminderOverSpeedingTab1.Visible = false;
            }
            if (AdditionalTreeView.SelectedNode != null)
            {
                SettingName.Text = AdditionalTreeView.SelectedNode.Text;
                switch (AdditionalTreeView.SelectedValue)
                {
                    case "POI":
                        { } break;
                    case "Route":
                        { } break;
                }
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            StatusUpdatePanel.Update();
        }
    }

    protected void AddButtonPressed(object sender, EventArgs e)
    {
        try
        {
            int accordionIndex = -1;
            if (int.TryParse(AccordionSelectedPane.Value, out accordionIndex))
            {
                #region "DataAccordion.SelectedIndex == 0"
                if (accordionIndex == 0)
                {
                    switch (UsersTreeView.SelectedValue)
                    {
                        case "General":
                            {

                            } break;
                        case "Groups":
                            {
                            } break;
                        case "Drivers":
                            {
                                UserDriversTab1.ShowAddNew = true;
                                EditButtonPressed_EnableButtons(true);
                                Session["NewPressed"] = true;
                            } break;
                        case "Vehicles":
                            {
                                UserVehicleTab1.ShowAddNew = true;
                                EditButtonPressed_EnableButtons(true);
                                Session["NewPressed"] = true;
                            } break;
                        case "Factor":
                            { } break;
                        case "POI":
                            { } break;
                        case "Route":
                            { } break;
                    }
                }
                #endregion
                #region "DataAccordion.SelectedIndex == 1"
                if (accordionIndex == 1)
                {
                    switch (ReminderTreeView.SelectedValue)
                    {
                        case "EmailSchedule":
                            {
                                EmailSheduler1.CreateNewShedule();
                                EditButtonPressed_EnableButtons(true);
                                Session["NewPressed"] = true;
                            } break;
                        case "OverSpeeding":
                            {
                                ReminderOverSpeedingTab1.Enabled = true;
                                EditButtonPressed_EnableButtons(true);
                            } break;
                        case "OverRPM":
                            { } break;
                        case "OverTime":
                            { } break;
                        case "OverFuelCons":
                            { } break;
                        case "NextTO":
                            { } break;
                        case "OutOfGeoZone":
                            { } break;
                        case "OverRoute":
                            { } break;
                    }
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            StatusUpdatePanel.Update();
        }
    }

    protected void EditButtonPressed(object sender, EventArgs e)
    {
        try
        {
            int accordionIndex = -1;
            if (int.TryParse(AccordionSelectedPane.Value, out accordionIndex))
            {
                #region "DataAccordion.SelectedIndex == 0"
                if (accordionIndex == 0)
                {
                    switch (UsersTreeView.SelectedValue)
                    {
                        case "General":
                            {
                                GeneralTab1.Enabled = true;
                                EditButtonPressed_EnableButtons(true);
                            } break;
                        case "Groups":
                            {
                                UserGroupsTab1.ShowEdit = true;
                                EditButtonPressed_EnableButtons(true);

                            } break;
                        case "Drivers":
                            {
                                UserDriversTab1.ShowEdit = true;
                                EditButtonPressed_EnableButtons(true);
                                Session["NewPressed"] = false;

                            } break;
                        case "Vehicles":
                            {
                                UserVehicleTab1.ShowEdit = true;
                                EditButtonPressed_EnableButtons(true);
                                Session["NewPressed"] = false;
                            } break;
                        case "Factor":
                            { } break;
                        case "POI":
                            { } break;
                        case "Route":
                            { } break;
                    }
                }
                #endregion
                #region "DataAccordion.SelectedIndex == 1"
                if (accordionIndex == 1)
                {
                    switch (ReminderTreeView.SelectedValue)
                    {
                        case "EmailSchedule":
                            {
                                EmailSheduler1.EditShedule();
                                EditButtonPressed_EnableButtons(true);
                                Session["NewPressed"] = false;
                            } break;
                        case "OverSpeeding":
                            {
                                ReminderOverSpeedingTab1.Enabled = true;
                                EditButtonPressed_EnableButtons(true);
                            } break;
                        case "OverRPM":
                            { } break;
                        case "OverTime":
                            { } break;
                        case "OverFuelCons":
                            { } break;
                        case "NextTO":
                            { } break;
                        case "OutOfGeoZone":
                            { } break;
                        case "OverRoute":
                            { } break;
                    }
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            StatusUpdatePanel.Update();
        }
    }

    protected void SaveButtonPressed(object sender, EventArgs e)
    {  
        string Language = "STRING_EN";
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, Language);
        dataBlock.OpenConnection();
        int userId = dataBlock.usersTable.Get_UserID_byName(User.Identity.Name);
        bool isNew = Convert.ToBoolean(Session["NewPressed"]);
        int Org_id = dataBlock.usersTable.Get_UserOrgId(userId);
        dataBlock.CloseConnection();                            
        try
        {
             //Server Validation
            if (!Page.IsValid)
                throw new Exception("Не все поля верно заполнены");
            //
            int accordionIndex = -1;
            if (int.TryParse(AccordionSelectedPane.Value, out accordionIndex))
            {
                #region "DataAccordion.SelectedIndex == 0"
                if (accordionIndex == 0)
                {
                    switch (UsersTreeView.SelectedValue)
                    {
                        case "General":
                            {
                                GeneralTab1.SaveChanges(Org_id, Language);
                                CancelButtonPressed(sender, e);
                            } break;
                        case "Groups":
                            {

                            } break;
                        case "Drivers":
                            {
                                if (isNew)
                                    UserDriversTab1.SaveAllNewInformation();
                                else
                                    UserDriversTab1.SaveAllUpdatedInformation();
                            } break;
                        case "Vehicles":
                            {
                                if (isNew)
                                    UserVehicleTab1.SaveAllNewInformation();
                                else
                                    UserVehicleTab1.SaveAllUpdatedInformation();
                            } break;
                        case "Factor":
                            { } break;
                        case "POI":
                            { } break;
                        case "Route":
                            { } break;
                    }
                }
                #endregion
                #region "DataAccordion.SelectedIndex == 1"
                if (accordionIndex == 1)
                {
                    switch (ReminderTreeView.SelectedValue)
                    {
                        case "EmailSchedule":
                            {
                                if (isNew)
                                    EmailSheduler1.SaveAllNewInformation();
                                else
                                    EmailSheduler1.SaveAllUpdatedInformation();
                                CancelButtonPressed(null, null);
                            } break;
                        case "OverSpeeding":
                            {                               
                            } break;
                        case "OverRPM":
                            { } break;
                        case "OverTime":
                            { } break;
                        case "OverFuelCons":
                            { } break;
                        case "NextTO":
                            { } break;
                        case "OutOfGeoZone":
                            { } break;
                        case "OverRoute":
                            { } break;
                    }
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            StatusUpdatePanel.Update();
        }
    }

    protected void DeleteButtonPressed(object sender, EventArgs e)
    {
        string Language = "STRING_EN";
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, Language);
        dataBlock.OpenConnection();
        int userId = dataBlock.usersTable.Get_UserID_byName(User.Identity.Name);
        bool isNew = Convert.ToBoolean(Session["NewPressed"]);
        int Org_id = dataBlock.usersTable.Get_UserOrgId(userId);
        dataBlock.CloseConnection();
        try
        {
            //Server Validation
            if (!Page.IsValid)
                throw new Exception("Не все поля верно заполнены");
            //
            int accordionIndex = -1;
            if (int.TryParse(AccordionSelectedPane.Value, out accordionIndex))
            {
                #region "DataAccordion.SelectedIndex == 0"
                if (accordionIndex == 0)
                {
                    switch (UsersTreeView.SelectedValue)
                    {
                        case "General":
                            {
                            } break;
                        case "Groups":
                            {

                            } break;
                        case "Drivers":
                            {
                            } break;
                        case "Vehicles":
                            {
                            } break;
                        case "Factor":
                            { } break;
                        case "POI":
                            { } break;
                        case "Route":
                            { } break;
                    }
                }
                #endregion
                #region "DataAccordion.SelectedIndex == 1"
                if (accordionIndex == 1)
                {
                    switch (ReminderTreeView.SelectedValue)
                    {
                        case "EmailSchedule":
                            {
                                EmailSheduler1.DeleteShedule();
                                CancelButtonPressed(sender, e);
                            } break;
                        case "OverSpeeding":
                            {
                            } break;
                        case "OverRPM":
                            { } break;
                        case "OverTime":
                            { } break;
                        case "OverFuelCons":
                            { } break;
                        case "NextTO":
                            { } break;
                        case "OutOfGeoZone":
                            { } break;
                        case "OverRoute":
                            { } break;
                    }
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            StatusUpdatePanel.Update();
        }
    }

    private void EditButtonPressed_EnableButtons(bool YES)
    {
        if (YES)
        {
            Choises_SAVE_Button.Enabled = true;
            Choises_CANCEL_Button.Enabled = true;
            Choises_ADD_Button.Enabled = false;
            Choises_EDIT_Button.Enabled = false;
            Choises_DELETE_Button.Enabled = true;
        }
        else
        {
            Choises_SAVE_Button.Enabled = false;
            Choises_CANCEL_Button.Enabled = false;
            Choises_ADD_Button.Enabled = true;
            Choises_EDIT_Button.Enabled = true;
            Choises_DELETE_Button.Enabled = true;
        }
    }

    protected void CancelButtonPressed(object sender, EventArgs e)
    {
        int accordionIndex = -1;
        if (int.TryParse(AccordionSelectedPane.Value, out accordionIndex))
        {
            #region "DataAccordion.SelectedIndex == 0"
            if (accordionIndex == 0)
            {
                switch (UsersTreeView.SelectedValue)
                {
                    case "General":
                        {
                            UsersTreeView_NodeChanged(sender, e);
                        } break;
                    case "Groups":
                        {
                            UsersTreeView_NodeChanged(sender, e);
                        } break;
                    case "Drivers":
                        {
                            UsersTreeView_NodeChanged(sender, e);
                        } break;
                    case "Vehicles":
                        {
                            UsersTreeView_NodeChanged(sender, e);
                        } break;
                    case "Factor":
                        { } break;
                    case "POI":
                        { } break;
                    case "Route":
                        { } break;
                }
            }
            #endregion
            #region "DataAccordion.SelectedIndex == 1"
            if (accordionIndex == 1)
            {
                switch (ReminderTreeView.SelectedValue)
                {
                    case "EmailSchedule":
                        {
                            ReminderTreeView_NodeChanged(sender, e);
                        } break;
                    case "OverSpeeding":
                        {
                            ReminderTreeView_NodeChanged(sender, e);
                        } break;
                    case "OverRPM":
                        { } break;
                    case "OverTime":
                        { } break;
                    case "OverFuelCons":
                        { } break;
                    case "NextTO":
                        { } break;
                    case "OutOfGeoZone":
                        { } break;
                    case "OverRoute":
                        { } break;
                }
            }
            #endregion
        }
    }

    private void HideButtons(bool YES)
    {
        Choises_SAVE_Button.Visible = !YES;
        Choises_CANCEL_Button.Visible = !YES;
        Choises_ADD_Button.Visible = !YES;
        Choises_EDIT_Button.Visible = !YES;
        Choises_DELETE_Button.Visible = !YES;
    }

    private void Load_DriverTab()
    {
        try
        {
            string Language = "STRING_EN";
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, Language);
            int Org_id = -1;
            dataBlock.OpenConnection();
            int userId = dataBlock.usersTable.Get_UserID_byName(User.Identity.Name);
            Org_id = dataBlock.usersTable.Get_UserOrgId(userId);
            dataBlock.CloseConnection();

            GeneralTab1.LoadAllInfo(Org_id, Language);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            StatusUpdatePanel.Update();
        }
    }

    protected override bool OnBubbleEvent(object source, EventArgs args)
    {
        if (Session["Settings_GeneralTabException"] != null)
        {
            if (Session["Settings_GeneralTabException"].ToString() != "HideThisWindow")
            {
                CancelButtonPressed(source, args);
                Exception ex = (Exception)Session["Settings_GeneralTabException"];
                Status.Text = ex.Message;
                Session["Settings_GeneralTabException"] = null;
            }
            else
            {
                Session["Settings_GeneralTabException"] = null;
                CancelButtonPressed(source, args);
            }
            StatusUpdatePanel.Update();
        }

        return true;
    }

    
}
