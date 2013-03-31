using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BLL;

public partial class Administrator_Adminisration_UserControls_UsersTab_UserControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            NewUserButton.ButtOnClick += new EventHandler(NewUserButton_Click);
            EditUserButton.ButtOnClick += new EventHandler(EditUserButton_Click);
            SaveUserButton.ButtOnClick += new EventHandler(SaveUserButton_Click);
            CancelUserButton.ButtOnClick += new EventHandler(CancelUserButton_Click);
            DeleteUserButton.ButtOnClick += new EventHandler(DeleteUserButton_Click);
            if (!IsPostBack)
            {
                ClearUserAdditionalInfo();
                Session["UsersTab_UserControl_UsersIds"] = null;
            }
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    public void LoadUsersTable()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        List<int> usersIds = new List<int>();
        List<UserFromTable> userFromTableList = new List<UserFromTable>();

        dataBlock.OpenConnection();
        usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.DriverUserTypeId));
        usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.AdministratorUserTypeId));
        usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.ManagerUserTypeId));
        usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.DealerUserTypeId));
        foreach (int id in usersIds)
        {
            userFromTableList.Add(new UserFromTable().FillWithInfo(id, dataBlock.usersTable));
        }
        Session["UsersTab_UserControl_UsersIds"] = usersIds;

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("DEALER", typeof(string)));
        dt.Columns.Add(new DataColumn("SURNAME", typeof(string)));
        dt.Columns.Add(new DataColumn("NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("PAtronimic", typeof(string)));
        dt.Columns.Add(new DataColumn("LOGIN", typeof(string)));
        dt.Columns.Add(new DataColumn("REG_DATE", typeof(string)));
        dt.Columns.Add(new DataColumn("ROLE", typeof(string)));
        dt.Columns.Add(new DataColumn("USER_TYPE", typeof(string)));
        dt.Columns.Add(new DataColumn("STATE", typeof(string)));

        int userInfoId = 0;
        int dealerId;
        string dealerName = "";
        DateTime date = new DateTime();
        foreach (UserFromTable users in userFromTableList)
        {
            dr = dt.NewRow();
            //DEALER
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_DealerId);
            if (int.TryParse(dataBlock.usersTable.GetUserInfoValue(users.id, userInfoId), out dealerId))
            {
                dealerName = dataBlock.organizationTable.GetOrganizationName(dealerId);
            }
            else
                dealerName = "???";
            if(dealerName.Trim()=="")
                dealerName = "???";
            dr["DEALER"] = dealerName;
            //SURNAME
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Surname);
            dr["SURNAME"] = dataBlock.usersTable.GetUserInfoValue(users.id, userInfoId);
            //NAME
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
            dr["NAME"] = dataBlock.usersTable.GetUserInfoValue(users.id, userInfoId);
            //Patronimic
            userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Patronimic);
            dr["PAtronimic"] = dataBlock.usersTable.GetUserInfoValue(users.id, userInfoId);
            //LOGIN
            dr["LOGIN"] = users.name;
            //REG_DATE
            if (DateTime.TryParse(dataBlock.usersTable.GetUserInfoValue(users.id, DataBaseReference.UserInfo_RegDate), out date))
                dr["REG_DATE"] = date.ToLongDateString() + " " + date.ToShortTimeString();
            //ROLE
            dr["ROLE"] = users.userRole;
            //USER_TYPE
            dr["USER_TYPE"] = users.userType;
            //STATE
            if (users.timeConnection == (new DateTime()))
                dr["STATE"] = "Вход не производился";
            else
                dr["STATE"] = "Последний вход " + users.timeConnection.ToLongDateString() + " " + users.timeConnection.ToShortTimeString();
            //
            dt.Rows.Add(dr);
        }
        dataBlock.CloseConnection();
        UsersDataGrid.DataSource = dt;
        UsersDataGrid.DataBind();
    }

    private void LoadUserAdditionalInfo(int userId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);

        int userInfoId = 0;
        DateTime date = new DateTime();
        dataBlock.usersTable.OpenConnection();
        //SURNAME
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Surname);
        DetailedInfo_SurName_TextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        //NAME
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
        DetailedInfo_Name_TextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        //Patronimic
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Patronimic);
        DetailedInfo_Patronimic_TextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        //ONOFF
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_ONOFF);
        string onOffStr = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        onOffStr = onOffStr.ToLower();
        if (onOffStr == "true" || onOffStr == "false")
        {
            bool OnChecked = Convert.ToBoolean(onOffStr);
            DetailedInfo_ONOFF_CheckBox.Checked = OnChecked;
        }
        //Login
        DetailedInfo_Login_TextBox.Text = dataBlock.usersTable.Get_UserName(userId);
        //Password
        DetailedInfo_Password_TextBox.Text = dataBlock.usersTable.Get_UserPassword(userId);
        DetailedInfo_PasswordConfirm_TextBox.Text = dataBlock.usersTable.Get_UserPassword(userId);
        //RegDate
        if (DateTime.TryParse(dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_RegDate), out date))
            DetailedInfo_RegDate_TextBox.Text = date.ToLongDateString() + " " + date.ToShortTimeString();
        //ExpireDate
        if (DateTime.TryParse(dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.OrgInfo_EndOfRegistrationDate), out date))
            DetailedInfo_RegEndDate_TextBox.Text = date.ToLongDateString();
        // Phone number
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_PhoneNumber);
        DetailedInfo_PhoneNumb_TextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        // Fax
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Fax);
        DetailedInfo_Fax_TextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        // EMail
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Email);
        DetailedInfo_Email_TextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        ////dropdowns///
        LoadAllDropDowns();
        //UserRole
        DetailedInfo_UserRole_DropDown.Items.FindByText(dataBlock.usersTable.Get_UserRoleName(userId)).Selected = true;
        //UserType
        string userTypeString = dataBlock.usersTable.Get_UserTypeStr(userId);
        if (userTypeString == "DealerUser")
        {
            DetailedInfo_UserType_DropDown.Items.Clear();
            DetailedInfo_UserType_DropDown.Items.Add(new ListItem(userTypeString, dataBlock.usersTable.DealerUserTypeId.ToString()));
        }
        else
            DetailedInfo_UserType_DropDown.Items.FindByText(userTypeString).Selected = true;
        //TimeZone
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_TimeZone);
        string DropDownValue = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        if (DetailedInfo_TimeZone_DropDown.Items.FindByValue(DropDownValue) != null)
            DetailedInfo_TimeZone_DropDown.Items.FindByValue(DropDownValue).Selected = true;
        //Dealer
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_DealerId);
        DropDownValue = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        if (DetailedInfo_Dealer_DropDown.Items.FindByValue(DropDownValue) != null)
            DetailedInfo_Dealer_DropDown.Items.FindByValue(DropDownValue).Selected = true;     
        else
            DetailedInfo_Dealer_DropDown.Items.FindByValue("-1").Selected = true;
        //Country
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Country);
        DropDownValue = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        if (DetailedInfo_Country_DropDown.Items.FindByText(DropDownValue) != null)
            DetailedInfo_Country_DropDown.Items.FindByText(DropDownValue).Selected = true;
        //City
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_City);
        DetailedInfo_Region_TextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        //ZipCode
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_ZIP);
        DetailedInfo_ZipCode_TextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        //Address 1
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_AddressOne);
        DetailedInfo_AddressOne_TextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        //Address 2
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_AddressTwo);
        DetailedInfo_AddressTwo_TextBox.Text = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        //SiteLang
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_SiteLang);
        DropDownValue = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        if (DetailedInfo_SiteLang_DropDown.Items.FindByValue(DropDownValue) != null)
            DetailedInfo_SiteLang_DropDown.Items.FindByValue(DropDownValue).Selected = true;
        //ReportsLang
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_ReportsLang);
        DropDownValue = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
        if (DetailedInfo_ReportsLang_DropDown.Items.FindByValue(DropDownValue) != null)
            DetailedInfo_ReportsLang_DropDown.Items.FindByValue(DropDownValue).Selected = true;
    }

    private void ClearUserAdditionalInfo()
    {
        //SURNAME
        DetailedInfo_SurName_TextBox.Text = "";
        //NAME
        DetailedInfo_Name_TextBox.Text = "";
        //Patronimic
        DetailedInfo_Patronimic_TextBox.Text = "";
        //Login
        DetailedInfo_Login_TextBox.Text = "";
        //Password
        DetailedInfo_Password_TextBox.Text = "";
        DetailedInfo_PasswordConfirm_TextBox.Text = "";
        //RegDate
        DetailedInfo_RegDate_TextBox.Text = "";
        //ExpireDate
        DetailedInfo_RegEndDate_TextBox.Text = "";
        // Phone number
        DetailedInfo_PhoneNumb_TextBox.Text = "";
        // Fax
        DetailedInfo_Fax_TextBox.Text = "";
        // EMail
        DetailedInfo_Email_TextBox.Text = "";
        ////dropdowns///
        LoadAllDropDowns();
        //City
        DetailedInfo_Region_TextBox.Text = "";
        //ZipCode
        DetailedInfo_ZipCode_TextBox.Text = "";
        //Address 1
        DetailedInfo_AddressOne_TextBox.Text = "";
        //Address 2
        DetailedInfo_AddressTwo_TextBox.Text = "";
    }

    private void LoadAllDropDowns()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);

        DetailedInfo_TimeZone_DropDown.Items.Clear();
        DetailedInfo_Country_DropDown.Items.Clear();
        DetailedInfo_UserRole_DropDown.Items.Clear();
        DetailedInfo_UserType_DropDown.Items.Clear();
        DetailedInfo_Dealer_DropDown.Items.Clear();
        DetailedInfo_SiteLang_DropDown.Items.Clear();
        DetailedInfo_ReportsLang_DropDown.Items.Clear();
        UsersFilterTable_RoleDropDownList.Items.Clear();
        UsersFilterTable_TypeDropDownList.Items.Clear();
        UsersFilterTable_DealerDropDownList.Items.Clear();

        dataBlock.OpenConnection();
        List<KeyValuePair<string, int>> orgCountries = dataBlock.organizationTable.GetAllCountry();
        foreach (KeyValuePair<string, int> country in orgCountries)
        {
            DetailedInfo_Country_DropDown.Items.Add(new ListItem(country.Key, country.Value.ToString()));
        }

        UsersFilterTable_RoleDropDownList.Items.Add(new ListItem("Все", "-1"));
        List<KeyValuePair<string, int>> userRoles = dataBlock.usersTable.GetAllUsersRoles();
        foreach (KeyValuePair<string, int> role in userRoles)
        {
            DetailedInfo_UserRole_DropDown.Items.Add(new ListItem(role.Key, role.Value.ToString()));
            UsersFilterTable_RoleDropDownList.Items.Add(new ListItem(role.Key, role.Value.ToString()));
        }

        UsersFilterTable_TypeDropDownList.Items.Add(new ListItem("Все", "-1"));
        List<KeyValuePair<string, int>> userTypes = dataBlock.usersTable.GetAllUsersTypes();
        foreach (KeyValuePair<string, int> type in userTypes)
        {
            if (type.Value == dataBlock.usersTable.DealerUserTypeId)
                continue;
            DetailedInfo_UserType_DropDown.Items.Add(new ListItem(type.Key, type.Value.ToString()));
            UsersFilterTable_TypeDropDownList.Items.Add(new ListItem(type.Key, type.Value.ToString()));//фильтр
        }

        string Russian = ConfigurationManager.AppSettings["language"];
        string English = ConfigurationManager.AppSettings["language"];
        DetailedInfo_SiteLang_DropDown.Items.Add(new ListItem("Русский", Russian));
        DetailedInfo_SiteLang_DropDown.Items.Add(new ListItem("English", English));
        DetailedInfo_ReportsLang_DropDown.Items.Add(new ListItem("Русский", Russian));
        DetailedInfo_ReportsLang_DropDown.Items.Add(new ListItem("English", English));

        //DEALER
        DetailedInfo_Dealer_DropDown.Items.Add(new ListItem("Нет дилера", "-1"));
        UsersFilterTable_DealerDropDownList.Items.Add(new ListItem("Все", "-1"));
        List<int> dealersIds = dataBlock.organizationTable.Get_AllDealersId(orgId);
        foreach (int dealerId in dealersIds)
        {
            DetailedInfo_Dealer_DropDown.Items.Add(new ListItem(dataBlock.organizationTable.GetOrganizationName(dealerId), dealerId.ToString()));
            UsersFilterTable_DealerDropDownList.Items.Add(new ListItem(dataBlock.organizationTable.GetOrganizationName(dealerId), dealerId.ToString()));
        }
        //TIMEZONE
        DetailedInfo_TimeZone_DropDown.Items.Add(new ListItem("Minsk +2", "0"));

        dataBlock.CloseConnection();
    }

    protected void UsersDataGrid_RadioButton_Checked(object sender, EventArgs e)
    {
        try
        {
         /*   foreach (DataGridItem oldrow in UsersDataGrid.Items)
            {
                ((RadioButton)oldrow.FindControl("UsersDataGrid_RadioButton")).Checked = false;
            }*/
            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            DataGridItem row = (DataGridItem)rb.NamingContainer;
            ((RadioButton)row.FindControl("UsersDataGrid_RadioButton")).Checked = true;
            int userId = ((List<int>)Session["UsersTab_UserControl_UsersIds"])[row.DataSetIndex];
            Selected_UsersDataGrid_Index.Value = userId.ToString();
            LoadUserAdditionalInfo(userId);
            UsersDataGridUpdatePanel.Update();
            DetailedInformationUpdatePanel.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void EditUserButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (Selected_UsersDataGrid_Index.Value == "")
                throw new Exception("Выберите пользователя!");
            EditUser(true);
            DetailedInformationPanel.Enabled = true;
            //UsersTabContainer.ActiveTab = DetailedInfoTab;
            NewOrEditUser_hdnField.Value = "edit";
            UpdatePanel1.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void NewUserButton_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (DataGridItem oldrow in UsersDataGrid.Items)
            {
                ((RadioButton)oldrow.FindControl("UsersDataGrid_RadioButton")).Checked = false;
            }
            EditUser(true);
            DetailedInformationPanel.Enabled = true;
            ClearUserAdditionalInfo();
            //UsersTabContainer.ActiveTab = DetailedInfoTab;
            NewOrEditUser_hdnField.Value = "new";
            UpdatePanel1.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void CancelUserButton_Click(object sender, EventArgs e)
    {
        DetailedInformationPanel.Enabled = false;
        EditUser(false);
        //UsersTabContainer.ActiveTab = UsersTab;
        UpdatePanel1.Update();
    }

    protected void SaveUserButton_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        try
        {
            int userId = -1;
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            string Login = DetailedInfo_Login_TextBox.Text;
            string password = DetailedInfo_Password_TextBox.Text;
            string passwordConfirm = DetailedInfo_PasswordConfirm_TextBox.Text;
            int UserRoleId = Convert.ToInt32(DetailedInfo_UserRole_DropDown.SelectedValue);
            int userTypeId = Convert.ToInt32(DetailedInfo_UserType_DropDown.SelectedValue);

            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);

            switch (NewOrEditUser_hdnField.Value)
            {
                case "new":
                    {
                        UserFromTable newUser = new UserFromTable(Login, password, "", "", DateTime.Now, "");
                        userId = dataBlock.usersTable.AddNewUser(newUser, userTypeId, UserRoleId, orgId, curUserId);
                    } break;
                case "edit":
                    {
                        userId = Convert.ToInt32(Selected_UsersDataGrid_Index.Value);
                        string OldPass = dataBlock.usersTable.Get_UserPassword(userId);
                        string oldName = dataBlock.usersTable.Get_UserName(userId);
                        UserFromTable newUser = new UserFromTable(Login, password, "", "", DateTime.Now, "");
                        dataBlock.usersTable.EditUser(OldPass, oldName, newUser, userTypeId, UserRoleId, orgId, curUserId);
                    } break;
            }
            if (userId < 0)
                throw new Exception("Не выбран пользователь!");
            
            SaveAdditionalInformation(userId, dataBlock);
            dataBlock.CommitTransaction();
            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.RollbackConnection();
            dataBlock.CloseConnection();
            RaiseException(ex);
        }
        finally
        {
            DetailedInformationPanel.Enabled = false;
            EditUser(false);
            LoadUsersTable();
            //UsersTabContainer.ActiveTab = UsersTab;
            UpdatePanel1.Update();
        }
    }

    protected void DeleteUserButton_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        try
        {
            int userId = Convert.ToInt32(Selected_UsersDataGrid_Index.Value);
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            throw new Exception("Эта функция пока не работает");
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    private void SaveAdditionalInformation(int userId, DataBlock dataBlock)
    {
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        string surName = DetailedInfo_SurName_TextBox.Text;
        string name = DetailedInfo_Name_TextBox.Text;
        string patronimic = DetailedInfo_Patronimic_TextBox.Text;
        bool ONOFF = DetailedInfo_ONOFF_CheckBox.Checked;
        string dealerID = DetailedInfo_Dealer_DropDown.SelectedItem.Value;   
        int countryId = Convert.ToInt32(DetailedInfo_Country_DropDown.SelectedValue);
        string city = DetailedInfo_Region_TextBox.Text;
        string zipCode = DetailedInfo_ZipCode_TextBox.Text;
        int timeZone = Convert.ToInt32(DetailedInfo_TimeZone_DropDown.SelectedItem.Value);
        string addressOne = DetailedInfo_AddressOne_TextBox.Text;
        string addressTwo = DetailedInfo_AddressTwo_TextBox.Text;
        string phoneNumb = DetailedInfo_PhoneNumb_TextBox.Text;
        string FAXNumb = DetailedInfo_Fax_TextBox.Text;
        string email = DetailedInfo_Email_TextBox.Text;
        string siteLang = DetailedInfo_SiteLang_DropDown.SelectedValue;
        string reportsLang = DetailedInfo_ReportsLang_DropDown.SelectedValue;
        int userInfoId;
        //SURNAME
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Surname);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, surName);
        //NAME
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, name);
        //PATRONIMIC
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Patronimic);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, patronimic);
        //ONOFF
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_ONOFF);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, ONOFF.ToString());       
        //DEALER
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_DealerId);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, dealerID);
        //Country
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Country);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, countryId.ToString());
        //City
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_City);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, city);
        //ZIP
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_ZIP);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, zipCode);
        //timeZone
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_TimeZone);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, timeZone.ToString());
        //ADDRESS ONE
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_AddressOne);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, addressOne);
        //ADDRESS TWO
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_AddressTwo);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, addressTwo);
        //PHONE NUMBER
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_PhoneNumber);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, phoneNumb);
        //FAX
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Fax);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, FAXNumb);
        //EMAIL
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Email);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, email);
        //SITELANG
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_SiteLang);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, siteLang);
        //REPORTSLANG
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_ReportsLang);
        dataBlock.usersTable.EditUserInfo(userId, userInfoId, reportsLang);
    }
    
    private void EditUser(bool editClicked)
    {
        if (editClicked)
        {
            EditUserButton.Enabled = false;
            SaveUserButton.Enabled = true;
            CancelUserButton.Visible = true;
            DeleteUserButton.Enabled = false;
            NewUserButton.Enabled = false;
            UsersTab.Enabled = false;
        }
        else
        {
            EditUserButton.Enabled = true;
            SaveUserButton.Enabled = false;
            DeleteUserButton.Enabled = true;
            CancelUserButton.Visible = false;
            NewUserButton.Enabled = true;
            UsersTab.Enabled = true;
        }
    }

    private void RaiseException(Exception ex)
    {
        Session["AdministrationTabException"] = ex;
        RaiseBubbleEvent(null, new EventArgs());
    }

    protected void ApplyUsersFilter_Click(object sender, EventArgs e)
    {
        try
        {
            int userTypeId = Convert.ToInt32(UsersFilterTable_TypeDropDownList.SelectedValue);
            int userRoleId = Convert.ToInt32(UsersFilterTable_RoleDropDownList.SelectedValue);
            int selectedDealerId = Convert.ToInt32(UsersFilterTable_DealerDropDownList.SelectedValue);


            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            List<int> usersIds = new List<int>();

            dataBlock.OpenConnection();
            if (userTypeId != -1)
            {
                usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, userTypeId));
            }
            else
            {
                usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.DriverUserTypeId));
                usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.AdministratorUserTypeId));
                usersIds.AddRange(dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.ManagerUserTypeId));
            }
            Session["UsersTab_UserControl_UsersIds"] = usersIds;

            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("DEALER", typeof(string)));
            dt.Columns.Add(new DataColumn("SURNAME", typeof(string)));
            dt.Columns.Add(new DataColumn("NAME", typeof(string)));
            dt.Columns.Add(new DataColumn("PAtronimic", typeof(string)));
            dt.Columns.Add(new DataColumn("LOGIN", typeof(string)));
            dt.Columns.Add(new DataColumn("REG_DATE", typeof(string)));
            dt.Columns.Add(new DataColumn("ROLE", typeof(string)));
            dt.Columns.Add(new DataColumn("USER_TYPE", typeof(string)));
            dt.Columns.Add(new DataColumn("STATE", typeof(string)));

            int userInfoId = 0;
            //int dealerId;
            string dealerName = "";
            DateTime date = new DateTime();
            int dealerId;
            foreach (int userId in usersIds)
            {
                if (userRoleId != -1)
                    if (dataBlock.usersTable.GetUserRoleId(userId) != userRoleId)
                        continue;
                /*if (userTypeId != -1)
                    if (dataBlock.usersTable.Get_UserTypeId(userId) != userTypeId)
                        continue;*/
                if (selectedDealerId != -1)
                {
                    userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_DealerId);
                    if (int.TryParse(dataBlock.usersTable.GetUserInfoValue(userId, userInfoId), out dealerId))
                    {
                        if (selectedDealerId != dealerId)
                            continue;
                        else
                            dealerName = dataBlock.organizationTable.GetOrganizationName(dealerId);
                    }
                    else
                        continue;
                }
                else
                {
                    userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_DealerId);
                    if (int.TryParse(dataBlock.usersTable.GetUserInfoValue(userId, userInfoId), out dealerId))
                    {
                        dealerName = dataBlock.organizationTable.GetOrganizationName(dealerId);
                    }
                    else
                        dealerName = "???";
                    if (dealerName.Trim() == "")
                        dealerName = "???";
                }
                dr = dt.NewRow();
                //DEALER
                dr["DEALER"] = dealerName;
                //SURNAME
                userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Surname);
                dr["SURNAME"] = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
                //NAME
                userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
                dr["NAME"] = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
                //Patronimic
                userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Patronimic);
                dr["PAtronimic"] = dataBlock.usersTable.GetUserInfoValue(userId, userInfoId);
                //LOGIN
                dr["LOGIN"] = dataBlock.usersTable.Get_UserName(userId);
                //REG_DATE
                if (DateTime.TryParse(dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_RegDate), out date))
                    dr["REG_DATE"] = date.ToLongDateString() + " " + date.ToShortTimeString();
                //ROLE
                dr["ROLE"] = dataBlock.usersTable.Get_UserRoleName(userId);
                //USER_TYPE
                dr["USER_TYPE"] = dataBlock.usersTable.Get_UserTypeStr(userId);
                //STATE
                date = dataBlock.usersTable.Get_TimeConnect(userId);
                if (date == (new DateTime()))
                    dr["STATE"] = "Вход не производился";
                else
                    dr["STATE"] = "Последний вход " + date.ToLongDateString() + " " + date.ToShortTimeString();
                //
                dt.Rows.Add(dr);
            }
            dataBlock.CloseConnection();
            UsersDataGrid.DataSource = dt;
            UsersDataGrid.DataBind();
            UsersDataGridUpdatePanel.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }
}