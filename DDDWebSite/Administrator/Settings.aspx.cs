﻿using System;
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
            /*Choises_CANCEL_Button.ButtOnClick += new EventHandler(CancelButtonPressed);
            Choises_ADD_Button.ButtOnClick += new EventHandler(AddButtonPressed);
            Choises_EDIT_Button.ButtOnClick += new EventHandler(EditButtonPressed);
            Choises_SAVE_Button.ButtOnClick += new EventHandler(SaveButtonPressed);
            Choises_DELETE_Button.ButtOnClick += new EventHandler(DeleteButtonPressed);*/

            if (!IsPostBack)
            {
                UserControlsForAll_BlueButton pan = ((UserControlsForAll_BlueButton)Page.Master.FindControl("SettingsMasterButt"));
                pan.Enabled = false;
                //((LinkButton)Page.Master.FindControl("SettingsMasterButt")).Enabled = false;
                /*TreeViews_LoadList();
                ReminderTreeView.Nodes[0].Selected = true;
                UsersTreeView.Nodes[0].Select();
                UsersTreeView_NodeChanged(null, null);
                Load_DriverTab();*/


                string connectionString = System.Configuration.ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
                BLL.DataBlock dataBlock = new BLL.DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
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
                dataBlock.CloseConnection();
                ((Panel)Master.FindControl("AdditionalConditionsPanel")).Visible = false;

                Session["Settings_GeneralTabException"] = null;
                //Status.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //AJAX BEGIN

    /// <summary>
    ///Получить Общие настройки
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<KeyValuePair<int, MapItem>> GetGeneralSettings(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        try
        {
            int orgId = Convert.ToInt32(OrgID);

            dataBlock.organizationTable.OpenConnection();
            List<KeyValuePair<string, int>> orgInfo = new List<KeyValuePair<string, int>>();
            orgInfo = dataBlock.organizationTable.GetAllOrgInfos();
            List<KeyValuePair<int, MapItem>> generalSettings = new List<KeyValuePair<int, MapItem>>();

            generalSettings.Add(new KeyValuePair<int, MapItem>(0, new MapItem("Наименование организации", dataBlock.organizationTable.GetOrganizationName(orgId))));
            generalSettings.Add(new KeyValuePair<int, MapItem>(0, new MapItem("Страна", dataBlock.organizationTable.GetOrgCountryName(orgId))));
            generalSettings.Add(new KeyValuePair<int, MapItem>(0, new MapItem("Регион", dataBlock.organizationTable.GetOrgRegionName(orgId))));

            foreach (KeyValuePair<string, int> pair in orgInfo)
            {
                String value = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, pair.Value);
                generalSettings.Add(new KeyValuePair<int, MapItem>(pair.Value, new MapItem(pair.Key, value)));
            }

            return generalSettings;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить Логотип для общих настроек
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static String GetGeneralLogo(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        try
        {
            int orgId = Convert.ToInt32(OrgID);
            dataBlock.organizationTable.OpenConnection();
            String result = "";
            result = dataBlock.organizationTable.GetOrgImage(orgId);
            if (result == null) { result = "../css/icons/company-middle.png"; }
            else { result = "data:image/jpeg;base64," + result; }
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
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
            throw ex;
            //return false;
        }
        finally
        {
            dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить Логотип для общих настроек
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static void SaveGeneralLogo(String OrgID, String logo)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        try
        {
            int orgId = Convert.ToInt32(OrgID);
            dataBlock.organizationTable.OpenConnection();

            if (logo.Equals("../css/icons/company-middle.png"))
            {
                dataBlock.organizationTable.SaveOrgImage(orgId, null);
            }
            else
            {
                logo = logo.Substring(logo.IndexOf(",") + 1);
                dataBlock.organizationTable.SaveOrgImage(orgId, logo);
            }

        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
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
            throw ex;
            //return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить настройки группы
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static GroupData GetGroupSettings(String CardID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int groupID = int.Parse(CardID);

            int num = 0;
            GroupData gd = new GroupData(groupID);
            gd.Name = dataBlock.cardsTable.GetGroupNameById(groupID);
            gd.Comment = dataBlock.cardsTable.GetGroupCommentById(groupID);
            gd.Number = ++num;
            gd.cardType = dataBlock.cardsTable.GetGroupCardTypeById(groupID);

            return gd;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            foreach (GroupData item in GroupSettings)
            {
                dataBlock.cardsTable.UpdateGroup(item.grID, item.Name, item.Comment, item.cardType);
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
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
            throw ex;
            //return false;
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            int cardType = int.Parse(CardType);

            dataBlock.cardsTable.CreateGroup(orgID, Name, Comment, cardType);

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
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
            throw ex;
            //return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить настройки водителя
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static CardData GetDriverSettings(String CardID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            int cardId = int.Parse(CardID);
            if (cardId == -1) return null;

            dataBlock.OpenConnection();
            CardData gd = new CardData(cardId);
            UserData ud = new UserData();
            ud.id = dataBlock.cardsTable.GetCardUserId(cardId);

            int userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
            ud.name=dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Surname);
            ud.surname=dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Patronimic);
            ud.patronimic = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_License);
            ud.license = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_License_Giver);
            ud.licGiver = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Vehicle);
            ud.vehicle = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Comment);
            ud.comment = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_PhoneNumber);
            ud.phone = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Birthday);
            ud.birthday = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);

            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardGiver);
            gd.CardGiver = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardGivenDate);
            gd.GivenDate = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardFromDate);
            gd.FromDate = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardToDate);
            gd.ToDate = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Country);
            gd.Country = dataBlock.usersTable.GetUserInfoValue(ud.id, userInfoId);

            userInfoId = dataBlock.usersTable.GetUserInfoNameIdExt(DataBaseReference.UserInfo_Image);
            ud.image64 = dataBlock.usersTable.GetUserInfoValueExt(ud.id, userInfoId);
            if (ud.image64 == null) { ud.image64 = "../css/icons/driver-icon.png"; }
            else { ud.image64 = "data:image/jpeg;base64," + ud.image64; }

            gd.cardImage = dataBlock.cardsTable.GetCardImage(cardId);
            if (gd.cardImage == null) { gd.cardImage = "../css/icons/user-card-icon.png"; }
            else { gd.cardImage = "data:image/jpeg;base64," + gd.cardImage; }

            gd.user = ud;
            gd.Name = dataBlock.cardsTable.GetCardName(cardId);
            gd.Number = dataBlock.cardsTable.GetCardNumber(cardId);
            gd.Comment = dataBlock.cardsTable.GetCardNote(cardId);
            gd.groupID = dataBlock.cardsTable.GetCardGroupID(cardId);
            gd.groupName = dataBlock.cardsTable.GetGroupNameById(gd.groupID);

            return gd;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Сохранить Настройки водителей
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool SaveDriverSettings(String OrgID, CardData DriverSettings,UserData UserSettings)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            /*dataBlock.cardsTable.ChangeCardName(DriverSettings.Name, DriverSettings.grID);
            dataBlock.cardsTable.ChangeCardNumber(DriverSettings.Number, DriverSettings.grID, 0);
            dataBlock.cardsTable.ChangeCardComment(DriverSettings.Comment, DriverSettings.grID);*/

            dataBlock.cardsTable.ChangeCardName(UserSettings.surname + " " + UserSettings.name, DriverSettings.grID);
            dataBlock.cardsTable.ChangeCardGroup(DriverSettings.groupID, DriverSettings.grID);
            dataBlock.cardsTable.ChangeCardNumber(DriverSettings.Number, DriverSettings.grID,0);

            int userId = dataBlock.cardsTable.GetCardUserId(DriverSettings.grID);
            int userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, UserSettings.name);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Surname);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, UserSettings.surname);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Patronimic);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, UserSettings.patronimic);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_License);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, UserSettings.license);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_License_Giver);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, UserSettings.licGiver);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Vehicle);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, UserSettings.vehicle);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Comment);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, UserSettings.comment);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_PhoneNumber);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, UserSettings.phone);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Birthday);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, UserSettings.birthday);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Country);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, DriverSettings.Country);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardGiver);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, DriverSettings.CardGiver);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardGivenDate);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, DriverSettings.GivenDate);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardFromDate);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, DriverSettings.FromDate);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardToDate);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, DriverSettings.ToDate);

            userInfoId = dataBlock.usersTable.GetUserInfoNameIdExt(DataBaseReference.UserInfo_Image);

            if (UserSettings.image64.Equals("../css/icons/driver-icon.png"))
            {
                dataBlock.usersTable.EditUserInfoExt(userId, userInfoId, null);
            }
            else
            {
                UserSettings.image64 = UserSettings.image64.Substring(UserSettings.image64.IndexOf(",") + 1);
                dataBlock.usersTable.EditUserInfoExt(userId, userInfoId, UserSettings.image64);
            }

            if (DriverSettings.cardImage.Equals("../css/icons/user-card-icon.png"))
            {
                dataBlock.cardsTable.SaveCardImage(DriverSettings.grID, null);
            }
            else
            {
                DriverSettings.cardImage = DriverSettings.cardImage.Substring(DriverSettings.cardImage.IndexOf(",") + 1);
                dataBlock.cardsTable.SaveCardImage(DriverSettings.grID, DriverSettings.cardImage);
            }

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
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
    public static int DeleteDrivers(String OrgID, List<MapItem> DriverIDs)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();

            int orgID = int.Parse(OrgID);
            int groupId = 0;
            for (int i = 0; i < DriverIDs.Count; i++)
            {
                int driverID = int.Parse(DriverIDs[i].Value);
                int userId = dataBlock.cardsTable.GetCardUserId(driverID);
                groupId = dataBlock.cardsTable.GetCardGroupID(driverID);
                dataBlock.cardsTable.DeleteCardSoft(driverID);
                dataBlock.usersTable.DeleteUserSoft(userId);
            }
            return groupId;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
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
            throw ex;
            //return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить настройки одного ТС
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static VehicleData GetTransportSettings(String CardID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            int cardId = int.Parse(CardID);
            if (cardId == -1) return null;

            dataBlock.OpenConnection();
            CardData gd = new CardData(cardId);
            gd.Name = dataBlock.cardsTable.GetCardName(cardId);
            gd.Number = dataBlock.cardsTable.GetCardNumber(cardId);
            gd.Comment = dataBlock.cardsTable.GetCardNote(cardId);
            gd.groupID = dataBlock.cardsTable.GetCardGroupID(cardId);
            gd.groupName = dataBlock.cardsTable.GetGroupNameById(gd.groupID);

            VehicleData vd = new VehicleData();
            vd.Card = gd;
            vd.id = dataBlock.vehiclesTables.GetVehicle_byCardId(cardId);

            int vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_GarageNumber);
            vd.GarageNumber = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MakeYear);
            vd.MakeYear = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_FuelTank1);
            vd.Tank1 = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_FuelTank2);
            vd.Tank2 = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_LoadCarryingCapacity);
            vd.Capacity = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_FuelType);
            vd.FuelType = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MRO1);
            vd.TO1 = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MRO2);
            vd.TO2 = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_NominalTurns);
            vd.Turns = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MaxSpeed);
            vd.MaxVelocity = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_Manoeuvring);
            vd.Manevring = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_City);
            vd.City = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_Highway);
            vd.Magistral = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_NomFuelConsumption);
            vd.Consumption = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_ColdStart);
            vd.ColdStart = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_HotStop);
            vd.HotStop = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_Type);
            vd.vehType = dataBlock.vehiclesTables.GetVehicleInfoValue(vd.id, vehInfoId);

            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameIdExt(DataBaseReference.Vehicle_VehiclePhotoAddress);
            vd.vehicleImage = dataBlock.vehiclesTables.GetVehicleInfoValueExt(vd.id, vehInfoId);
            if (vd.vehicleImage == null) { vd.vehicleImage = "../css/icons/transport-icon.png"; }
            else { vd.vehicleImage = "data:image/jpeg;base64," + vd.vehicleImage; }

            int deviceId = dataBlock.vehiclesTables.GetVehicleDeviceId(vd.id);
            vd.CalibratorCard=dataBlock.deviceTable.GetDeviceInfo(deviceId, dataBlock.deviceTable.Device_Calibration_Card);
            vd.Calibrator = dataBlock.deviceTable.GetDeviceInfo(deviceId, dataBlock.deviceTable.Device_Calibration_User);
            vd.CalibrReason = dataBlock.deviceTable.GetDeviceInfo(deviceId, dataBlock.deviceTable.Device_Calibration_Cause);
            vd.NextCalibrDate = dataBlock.deviceTable.GetDeviceInfo(deviceId, dataBlock.deviceTable.Device_Calibration_Next);
            vd.EquipmentType = dataBlock.deviceTable.GetDeviceInfo(deviceId, dataBlock.deviceTable.Device_Type);
            vd.Serial = dataBlock.deviceTable.GetDeviceInfo(deviceId, dataBlock.deviceTable.Device_Num);
            vd.LastReadDate = dataBlock.deviceTable.GetDeviceInfo(deviceId, dataBlock.deviceTable.Device_Date_Read_Last);
            vd.EquipmentName = dataBlock.deviceTable.GetDeviceName(deviceId);
            vd.SIMNum = dataBlock.deviceTable.GetDeviceInfo(deviceId, dataBlock.deviceTable.Device_Phone_Num_Sim);
            vd.EquipmentFirmware = dataBlock.deviceTable.GetDeviceInfo(deviceId, dataBlock.deviceTable.Device_Firmware);    

            return vd;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить список типов напоминаний
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetDeviceTypeList()
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            List<MapItem> result = new List<MapItem>();

            List<KeyValuePair<string, int>> list=dataBlock.deviceTable.GetAllDeviceTypes();
            foreach (KeyValuePair<string, int> item in list) {
                result.Add(new MapItem(Convert.ToString(item.Value),item.Key));
            }

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить список типов напоминаний
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetDeviceFirmwareList()
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            List<MapItem> result = new List<MapItem>();
            
            List<int> list = dataBlock.deviceTable.GetAllDeviceFirmwareIds();
            foreach (int id in list)
            {
                string model = dataBlock.deviceTable.GetDeviceFirmware_deviceModel(id);
                string date = Convert.ToString(dataBlock.deviceTable.GetDeviceFirmware_dateProduction(id));
                string version = dataBlock.deviceTable.GetDeviceFirmware_version(id);
                result.Add(new MapItem(Convert.ToString(id), model+" "+date+" "+version));
            }

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
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
    public static bool SaveTransportSettings(String OrgID, List<VehicleData> TransportSettings)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            foreach (VehicleData item in TransportSettings)
            {
                dataBlock.cardsTable.ChangeCardName(item.Card.Name, item.Card.grID);
                dataBlock.cardsTable.ChangeCardNumber(item.Card.Number, item.Card.grID, 0);
                dataBlock.cardsTable.ChangeCardComment(item.Card.Comment, item.Card.grID);
                dataBlock.cardsTable.ChangeCardGroup(item.Card.groupID, item.Card.grID);

                int vehId = dataBlock.vehiclesTables.GetVehicle_byCardId(item.Card.grID);
                int deviceId=dataBlock.vehiclesTables.GetVehicleDeviceId(vehId);
                int vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_GarageNumber);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.GarageNumber);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MakeYear);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.MakeYear);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_FuelTank1);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.Tank1);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_FuelTank2);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.Tank2);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_LoadCarryingCapacity);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.Capacity);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_FuelType);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.FuelType);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MRO1);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.TO1);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MRO2);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.TO2);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_NominalTurns);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.Turns);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MaxSpeed);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.MaxVelocity);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_Manoeuvring);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.Manevring);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_City);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.City);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_Highway);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.Magistral);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_NomFuelConsumption);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.Consumption);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_ColdStart);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.ColdStart);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_HotStop);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.HotStop);
                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_Type);
                dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, item.vehType);

                vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameIdExt(DataBaseReference.Vehicle_VehiclePhotoAddress);

                if (item.vehicleImage.Equals("../css/icons/transport-icon.png"))
                {
                    dataBlock.vehiclesTables.EditVehicleInfoExt(vehId, vehInfoId, null);
                }
                else
                {
                    item.vehicleImage = item.vehicleImage.Substring(item.vehicleImage.IndexOf(",") + 1);
                    dataBlock.vehiclesTables.EditVehicleInfoExt(vehId, vehInfoId, item.vehicleImage);
                }

                dataBlock.deviceTable.EditDeviceInfo(deviceId,dataBlock.deviceTable.Device_Calibration_Card,item.CalibratorCard);
                dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Calibration_User, item.Calibrator);
                dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Calibration_Cause, item.CalibrReason);
                dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Calibration_Next, DateTime.Parse(item.NextCalibrDate));
                dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Type, item.EquipmentType);
                dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Num, item.Serial);
                dataBlock.deviceTable.EditDeviceName(deviceId, item.EquipmentName);
                dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Phone_Num_Sim, item.SIMNum);
                dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Firmware, item.EquipmentFirmware);
                dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Date_Read_Last, DateTime.Parse(item.LastReadDate));

            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
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
    public static int DeleteTransports(String OrgID, List<MapItem> TransportIDs)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.organizationTable.OpenConnection();
            int orgID = int.Parse(OrgID);
            int groupId = 0;
            for (int i = 0; i < TransportIDs.Count; i++)
            {
                int cardId = int.Parse(TransportIDs[i].Value);
                int vehId = dataBlock.vehiclesTables.GetVehicle_byCardId(cardId);
                int devId = dataBlock.vehiclesTables.GetVehicleDeviceId(vehId);
                groupId = dataBlock.cardsTable.GetCardGroupID(cardId);
                dataBlock.cardsTable.DeleteCardSoft(cardId);
                dataBlock.vehiclesTables.DeleteVehicleSoft(vehId);
                dataBlock.deviceTable.DeleteDeviceSoft(devId);
            }
            return groupId;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            List<MapItem> result = new List<MapItem>();
            List<int> ids = dataBlock.cardsTable.GetAllGroupIds(orgID, dataBlock.cardsTable.driversCardTypeId);
            foreach (int index in ids)
            {
                string name = dataBlock.cardsTable.GetGroupNameById(index);
                result.Add(new MapItem(Convert.ToString(index), name));
            }
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

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
            throw ex;
            //return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получение списка типов
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetTypeListTransports()
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        try
        {
            dataBlock.OpenConnection();
            List<MapItem> result = new List<MapItem>();
            List<KeyValuePair<string, int>> types=dataBlock.vehiclesTables.GetAllVehTypes();

            foreach (KeyValuePair<string, int> pair in types)
            {
                result.Add(new MapItem(Convert.ToString(pair.Value), pair.Key));
            }
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
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
    public static List<MapItem> GetGroupListGroups(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            List<MapItem> result = new List<MapItem>();
            result.Add(new MapItem(Convert.ToString(1), "Водители"));
            result.Add(new MapItem(Convert.ToString(2), "ТС"));
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            int userID = dataBlock.usersTable.Get_UserID_byName(UserID);
            //int userID = int.Parse(UserID);
            //dataBlock.cardsTable.CreateNewCard(data.Name, data.Number, dataBlock.cardsTable.driversCardTypeId, orgID, data.Comment, userID, data.groupID);

            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Создание водителя
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static int CreateNewDriver(string OrgID, string UserID, UserData data, CardData cardData)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            int userID = dataBlock.usersTable.Get_UserID_byName(UserID);
            string orgName = dataBlock.organizationTable.GetOrganizationName(orgID);
            UserFromTable user = new UserFromTable("","","1","1",new DateTime(),orgName);

            int userId=dataBlock.usersTable.AddNewUser(user,1,1,orgID,userID);

            int userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, data.name);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Surname);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, data.surname);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Patronimic);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, data.patronimic);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_License);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, data.license);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_License_Giver);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, data.licGiver);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Vehicle);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, data.vehicle);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Comment);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, data.comment);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_PhoneNumber);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, data.phone);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Birthday);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, data.birthday);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardGiver);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, cardData.CardGiver);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardGivenDate);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, cardData.GivenDate);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardFromDate);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, cardData.FromDate);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_CardToDate);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, cardData.ToDate);
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Country);
            dataBlock.usersTable.EditUserInfo(userId, userInfoId, cardData.Country);

            userInfoId = dataBlock.usersTable.GetUserInfoNameIdExt(DataBaseReference.UserInfo_Image);

            if (data.image64.Equals("../css/icons/driver-icon.png"))
            {
                dataBlock.usersTable.EditUserInfoExt(userId, userInfoId, null);
            }
            else
            {
                data.image64 = data.image64.Substring(data.image64.IndexOf(",") + 1);
                dataBlock.usersTable.EditUserInfoExt(userId, userInfoId, data.image64);
            }

            //int userID = int.Parse(UserID);
            int cardId=dataBlock.cardsTable.CreateNewCard(data.surname+" "+data.name, cardData.Number, dataBlock.cardsTable.driversCardTypeId, orgID, userId, cardData.Comment, userID, cardData.groupID);

            if (cardData.cardImage.Equals("../css/icons/user-card-icon.png"))
            {
                dataBlock.cardsTable.SaveCardImage(cardId, null);
            }
            else
            {
                cardData.cardImage = cardData.cardImage.Substring(cardData.cardImage.IndexOf(",") + 1);
                dataBlock.cardsTable.SaveCardImage(cardId, cardData.cardImage);
            }

            return cardId;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
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
    public static int CreateCardTransport(string OrgID, string UserID, VehicleData data)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            //int userID = int.Parse(UserID);
            int userID = dataBlock.usersTable.Get_UserID_byName(UserID);
            int newId = dataBlock.cardsTable.CreateNewCard(data.Card.Name, data.Card.Number, dataBlock.cardsTable.vehicleCardTypeId, orgID, 0, data.Card.Comment, userID, data.Card.groupID);
            int vehId = dataBlock.vehiclesTables.AddNewVehicle("", "", data.Card.Number,1,1,newId,new DateTime(),1);

            int deviceId = dataBlock.deviceTable.AddNewDevice(Convert.ToInt32(data.EquipmentType), data.EquipmentName, Convert.ToInt32(data.EquipmentFirmware));
            dataBlock.vehiclesTables.EditVehicleDeviceId(vehId,deviceId);

            int vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_GarageNumber);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.GarageNumber);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MakeYear);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.MakeYear);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_FuelTank1);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.Tank1);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_FuelTank2);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.Tank2);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_LoadCarryingCapacity);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.Capacity);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_FuelType);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.FuelType);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MRO1);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.TO1);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MRO2);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.TO2);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_NominalTurns);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.Turns);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_MaxSpeed);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.MaxVelocity);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_Manoeuvring);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.Manevring);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_City);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.City);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_Highway);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.Magistral);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_NomFuelConsumption);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.Consumption);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_ColdStart);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.ColdStart);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_HotStop);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.HotStop);
            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.Vehicle_Type);
            dataBlock.vehiclesTables.EditVehicleInfo(vehId, vehInfoId, data.vehType);

            vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameIdExt(DataBaseReference.Vehicle_VehiclePhotoAddress);

            if (data.vehicleImage.Equals("../css/icons/transport-icon.png"))
            {
                dataBlock.vehiclesTables.EditVehicleInfoExt(vehId, vehInfoId, null);
            }
            else
            {
                data.vehicleImage = data.vehicleImage.Substring(data.vehicleImage.IndexOf(",") + 1);
                dataBlock.vehiclesTables.EditVehicleInfoExt(vehId, vehInfoId, data.vehicleImage);
            }

            dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Calibration_Card, data.CalibratorCard);
            dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Calibration_User, data.Calibrator);
            dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Calibration_Cause, data.CalibrReason);
            dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Calibration_Next, DateTime.Parse(data.NextCalibrDate));
            //dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Type, data.EquipmentType);
            dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Num, data.Serial);
            dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Phone_Num_Sim, data.SIMNum);
            dataBlock.deviceTable.EditDeviceInfo(deviceId, dataBlock.deviceTable.Device_Date_Read_Last, DateTime.Parse(data.LastReadDate));

            return newId;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        List<SettingsData> result = new List<SettingsData>();

        try
        {
            dataBlock.OpenConnection();
            List<KeyValuePair<string, int>> allKeys = new List<KeyValuePair<string, int>>();
            allKeys = dataBlock.criteriaTable.GetAllCriteria_Name_n_Id();

            CriteriaTable oneCriteria = new CriteriaTable(connectionString, ConfigurationManager.AppSettings["language"], dataBlock.sqlDb);
            dataBlock.criteriaTable.OpenConnection();

            foreach (KeyValuePair<string, int> key in allKeys)
            {
                oneCriteria = dataBlock.criteriaTable.LoadCriteria(key.Value);

                SettingsData sd = new SettingsData(oneCriteria.KeyId);
                sd.MeasureName = oneCriteria.MeasureName;
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
            throw ex;
            //return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.criteriaTable.CloseConnection();
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
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
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
            throw ex;
            //return;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить список пользователей
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetUserList(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            List<MapItem> result = new List<MapItem>();
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);

            List<int> users2 = dataBlock.usersTable.Get_AllUsersId(orgID, 2);
            List<int> users3 = dataBlock.usersTable.Get_AllUsersId(orgID, 3);
            List<int> users4 = dataBlock.usersTable.Get_AllUsersId(orgID, 4);
            List<int> users = new List<int>();
            users.AddRange(users2);
            users.AddRange(users3);
            users.AddRange(users4);

            foreach (int id in users)
            {
                string name = dataBlock.usersTable.Get_UserName(id);
                result.Add(new MapItem(id.ToString(), name));
            }

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить список напоминаний
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<RemindData> GetRemindList(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            List<RemindData> result = new List<RemindData>();
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);

            List<int> ids = dataBlock.remindTable.GetAllRemindIds(orgID);
            foreach (int id in ids)
            {
                RemindData rd = new RemindData();
                rd.date = dataBlock.remindTable.GetRemindLastDate(id).ToString();
                rd.userId = dataBlock.remindTable.GetRemindUser(id);
                rd.userName = dataBlock.usersTable.Get_UserName(rd.userId);
                rd.sourceId = dataBlock.remindTable.GetRemindSource(id);
                rd.sourceType = dataBlock.remindTable.GetRemindSourceType(id);
                switch (rd.sourceType)
                {
                    case 0: { rd.sourceName = dataBlock.cardsTable.GetCardHolderNameByCardId(rd.sourceId); break; }
                    case 1: { rd.sourceName = dataBlock.cardsTable.GetGroupNameById(rd.sourceId); break; }
                    case 2: { rd.sourceName = dataBlock.organizationTable.GetOrganizationName(rd.sourceId); break; }
                };
                rd.id = id;
                rd.active = dataBlock.remindTable.GetRemindActive(id) ? 1 : 0;
                rd.periodType = dataBlock.remindTable.GetRemindPeriod(id);
                rd.type = dataBlock.remindTable.GetRemindType(id);
                result.Add(rd);
            }

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить список типов напоминаний
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetRemindTypeList()
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            List<MapItem> result = new List<MapItem>();
            //for (int i = 1; i < 8; i++) {
            for (int i = 1; i < 3; i++)
            {
                result.Add(new MapItem(Convert.ToString(i), dataBlock.remindTable.GetRemindTypeName(i)));
            }
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Получить список типов напоминаний
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetRemindPeriodTypeList()
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            List<MapItem> result = new List<MapItem>();
            for (int i = 1; i < 5; i++)
            {
                result.Add(new MapItem(Convert.ToString(i), dataBlock.remindTable.GetRemindPeriodName(i)));
            }
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Удаление напоминаний
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool DeleteReminds(String OrgID, List<MapItem> RemindIDs)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            for (int i = 0; i < RemindIDs.Count; i++)
            {
                int remindID = int.Parse(RemindIDs[i].Value);
                dataBlock.remindTable.DeleteRemind(remindID);
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Сохранить напоминания
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool SaveRemindSettings(String OrgID, List<RemindData> RemindSettings)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            foreach (RemindData item in RemindSettings)
            {
                bool active = item.active == 1 ? true : false;
                DateTime time = dataBlock.remindTable.GetRemindLastDate(item.id);
                dataBlock.remindTable.UpdateRemind(item.id, active, item.userId, item.sourceType, item.sourceId, item.periodType, time, item.type);
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }

    /// <summary>
    ///Создание напоминания
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static bool CreateRemind(string OrgID, RemindData data)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            int orgID = int.Parse(OrgID);
            bool active = data.active == 1 ? true : false;
            dataBlock.remindTable.CreateNewRemind(orgID, active, data.userId, data.sourceType, data.sourceId, data.periodType, DateTime.Today, data.type);
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
            //return false;
        }
        finally
        {
            //dataBlock.organizationTable.CloseConnection();
            dataBlock.CloseConnection();
        }
    }
    //AJAX END

    /*private void TreeViews_LoadList()
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
        string Language = ConfigurationManager.AppSettings["language"];
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
        string Language = ConfigurationManager.AppSettings["language"];
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
            string Language = ConfigurationManager.AppSettings["language"];
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
    } */


}
