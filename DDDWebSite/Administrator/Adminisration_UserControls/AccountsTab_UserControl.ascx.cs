using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BLL;

public partial class Administrator_Adminisration_UserControls_AccountsTab_UserControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SaveAccountButton.ButtOnClick += new EventHandler(SaveAccountButton_Click);
            NewAccountButton.ButtOnClick += new EventHandler(NewAccountButton_Click);
            EditAccountButton.ButtOnClick += new EventHandler(EditAccountButton_Click);
            CancelAccountButton.ButtOnClick += new EventHandler(CancelAccountButton_Click);
            DeleteAccountButton.ButtOnClick += new EventHandler(DeleteAccountButton_Click);
            if (!IsPostBack)
            {
                ClearAccountAdditionalInfo();
                Session["AccountsTab_UserControl_AccountsIds"] = null;
            }
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    public bool LoadAccountsTable()//Возвращает False, если организация субдилер, у которой не может быть субдилеров
    {
      string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        List<int> accountsIds = new List<int>();

        dataBlock.OpenConnection();
        int orgTypeId = dataBlock.organizationTable.GetOrgTypeId(orgId);
        if (orgTypeId == dataBlock.organizationTable.DealerTypeId || orgTypeId == dataBlock.organizationTable.PredealerTypeId)
        {
            //accountsIds = dataBlock.organizationTable.Get_AllOrganizationsId(dataBlock.organizationTable.AccountTypeId);
            accountsIds = dataBlock.organizationTable.Get_AllDealersId(orgId);
            accountsIds.AddRange(dataBlock.organizationTable.Get_AllOrganizationsId((orgId)));


            Session["AccountsTab_UserControl_AccountsIds"] = accountsIds;
        
        /*      DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("ACCOUNTTYPE", typeof(string)));
            dt.Columns.Add(new DataColumn("ACCOUNTNAME", typeof(string)));
            dt.Columns.Add(new DataColumn("LOGIN", typeof(string)));
            dt.Columns.Add(new DataColumn("REG_DATE", typeof(string)));
            dt.Columns.Add(new DataColumn("ENDREG_DATE", typeof(string)));
            dt.Columns.Add(new DataColumn("COUNTRY", typeof(string)));
            dt.Columns.Add(new DataColumn("CITY", typeof(string)));
            dt.Columns.Add(new DataColumn("ORGID", typeof(int)));

            int accountInfoId = 0;
            DateTime date = new DateTime();
            int userId = 1;
            foreach (int id in accountsIds)
            {
                userId = 0;
                dr = dt.NewRow();
                dr["ACCOUNTTYPE"] = dataBlock.organizationTable.GetOrgTypeName(id);
                dr["ACCOUNTNAME"] = dataBlock.organizationTable.GetOrganizationName(id);
                //LOGIN
                if (dataBlock.usersTable.Get_AllUsersId(id, dataBlock.usersTable.DealerUserTypeId).Count>0)
                {
                    userId = dataBlock.usersTable.Get_AllUsersId(id, dataBlock.usersTable.DealerUserTypeId)[0];
                    dr["LOGIN"] = dataBlock.usersTable.Get_UserName(userId);
                }
                else
                    dr["LOGIN"] = "Недоступно";
                //REG_DATE
                if (DateTime.TryParse(dataBlock.organizationTable.GetAdditionalOrgInfo(id, DataBaseReference.OrgInfo_RegistrationDate), out date))
                    dr["REG_DATE"] = date.ToLongDateString() + " " + date.ToShortTimeString();
                //END_REG_DATE
                if (DateTime.TryParse(dataBlock.organizationTable.GetAdditionalOrgInfo(id, DataBaseReference.OrgInfo_EndOfRegistrationDate), out date))
                    dr["ENDREG_DATE"] = date.ToLongDateString();
                //COUNTRY
                dr["COUNTRY"] = dataBlock.organizationTable.GetOrgCountryName(id);
                //CITY              
                dr["CITY"] = dataBlock.organizationTable.GetAdditionalOrgInfo(id, DataBaseReference.OrgInfo_City);
                //ORGID
                dr["ORGID"] = id;
                //
                dt.Rows.Add(dr);*/
            }
           /* AccountsDataGrid.DataSource = dt;
            AccountsDataGrid.DataBind();
        }
        else
        {
            return false;
        }
        dataBlock.CloseConnection();*/
        return true;
    }

    private void LoadAccountAdditionalInfo(int orgId, int userAccountId)
    {
        Selected_AccountsDataGrid_Index.Value = orgId.ToString();
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");

        int userInfoId = 0;
        DateTime date = new DateTime();
        dataBlock.usersTable.OpenConnection();
        dataBlock.organizationTable.OpenConnection();
        //DEALERNAME
        DetailedInfo_AccountName_TextBox.Text = dataBlock.organizationTable.GetOrganizationName(orgId);
        //ONOFF
        string onOffStr = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_ONOFF);
        onOffStr = onOffStr.ToLower();
        if (onOffStr == "true" || onOffStr == "false")
        {
            bool OnChecked = Convert.ToBoolean(onOffStr);
            DetailedInfo_ONOFF_CheckBox.Checked = OnChecked;
        }
        //Login
        DetailedInfo_Login_TextBox.Text = dataBlock.usersTable.Get_UserName(userAccountId);
        //Password
        DetailedInfo_Password_TextBox.Text = dataBlock.usersTable.Get_UserPassword(userAccountId);
        DetailedInfo_PasswordConfirm_TextBox.Text = dataBlock.usersTable.Get_UserPassword(userAccountId);
        //REG_DATE
        if (DateTime.TryParse(dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_RegistrationDate), out date))
            DetailedInfo_RegDate_TextBox.Text = date.ToLongDateString() + " " + date.ToShortTimeString();
        //END_REG_DATE
        if (DateTime.TryParse(dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_EndOfRegistrationDate), out date))
            DetailedInfo_RegEndDate_TextBox.Text = date.ToLongDateString() + " " + date.ToShortTimeString();
        // Phone number
        DetailedInfo_PhoneNumb_TextBox.Text = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_PhoneNumber);
        // Fax
        DetailedInfo_Fax_TextBox.Text = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_Fax);
        // EMail
        DetailedInfo_Email_TextBox.Text = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_Email);
        ////dropdowns///
        LoadAllDropDowns();
        //TimeZone
        if (DetailedInfo_TimeZone_DropDown.Items.FindByValue(dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_TimeZone)) != null)
            DetailedInfo_TimeZone_DropDown.Items.FindByValue(dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_TimeZone)).Selected = true;
        //Country
        if (DetailedInfo_Country_DropDown.Items.FindByText(dataBlock.organizationTable.GetOrgCountryName(orgId)) != null)
            DetailedInfo_Country_DropDown.Items.FindByText(dataBlock.organizationTable.GetOrgCountryName(orgId)).Selected = true;
        //AccType
        string accTypeId = dataBlock.organizationTable.GetOrgTypeId(orgId).ToString();
        if (AccountTypeDropDown.Items.FindByValue(accTypeId) != null)
            AccountTypeDropDown.Items.FindByValue(accTypeId).Selected = true;
        //City
        DetailedInfo_Region_TextBox.Text = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_City);
        //ZipCode
        DetailedInfo_ZipCode_TextBox.Text = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_ZIP);
        //Address 1
        DetailedInfo_AddressOne_TextBox.Text = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_AddressOne);
        //Address 2
        DetailedInfo_AddressTwo_TextBox.Text = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_AddressTwo);
        //SiteLang
        string curLang = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_SiteLang);
        if (DetailedInfo_SiteLang_DropDown.Items.FindByValue(curLang) != null)
            DetailedInfo_SiteLang_DropDown.Items.FindByValue(curLang).Selected = true;
        //ReportsLang
        userInfoId = dataBlock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_ReportsLang);
        if (DetailedInfo_ReportsLang_DropDown.Items.FindByValue(dataBlock.usersTable.GetUserInfoValue(userAccountId, userInfoId)) != null)
            DetailedInfo_ReportsLang_DropDown.Items.FindByValue(dataBlock.usersTable.GetUserInfoValue(userAccountId, userInfoId)).Selected = true;

        dataBlock.usersTable.CloseConnection();
        dataBlock.organizationTable.CloseConnection();
    }

    private void ClearAccountAdditionalInfo()
    {
        //SURNAME
        DetailedInfo_AccountName_TextBox.Text = "";
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
        //ONOFF
        DetailedInfo_ONOFF_CheckBox.Checked = true;
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
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);

        DetailedInfo_TimeZone_DropDown.Items.Clear();
        DetailedInfo_Country_DropDown.Items.Clear();
        DetailedInfo_SiteLang_DropDown.Items.Clear();
        DetailedInfo_ReportsLang_DropDown.Items.Clear();
        AccountTypeDropDown.Items.Clear();
        dataBlock.OpenConnection();
        List<KeyValuePair<string, int>> orgCountries = dataBlock.organizationTable.GetAllCountry();
        ////////////////
        int orgTypeId = dataBlock.organizationTable.GetOrgTypeId(orgId);
        List<KeyValuePair<string, int>> orgTypes = dataBlock.organizationTable.GetAllOrganizationTypes();
        foreach (KeyValuePair<string, int> orgType in orgTypes)
        {
            if(orgTypeId == orgType.Value)
                continue;
            if(orgTypeId == dataBlock.organizationTable.SubdealerTypeId)
            {
                if(orgType.Value == dataBlock.organizationTable.DealerTypeId)
                    continue;
                if(orgType.Value == dataBlock.organizationTable.PredealerTypeId)
                    continue;
            }
            if(orgTypeId == dataBlock.organizationTable.DealerTypeId)
            {
                   if(orgType.Value == dataBlock.organizationTable.PredealerTypeId)
                    continue;
            }
           AccountTypeDropDown.Items.Add(new ListItem(orgType.Key, orgType.Value.ToString()));
        }
        /////////////
        dataBlock.CloseConnection();
        foreach (KeyValuePair<string, int> country in orgCountries)
        {
            DetailedInfo_Country_DropDown.Items.Add(new ListItem(country.Key, country.Value.ToString()));
        }
       
        string Russian = "STRING_RU";
        string English = "STRING_RU";
        DetailedInfo_SiteLang_DropDown.Items.Add(new ListItem("Русский", Russian));
        DetailedInfo_SiteLang_DropDown.Items.Add(new ListItem("English", English));
        DetailedInfo_ReportsLang_DropDown.Items.Add(new ListItem("Русский", Russian));
        DetailedInfo_ReportsLang_DropDown.Items.Add(new ListItem("English", English));

        //TIMEZONE
        DetailedInfo_TimeZone_DropDown.Items.Add(new ListItem("Minsk +2", "0"));
    }

   /* protected void AccountsDataGrid_RadioButton_Checked(object sender, EventArgs e)
    {
        try
        {
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
           /* foreach (DataGridItem oldrow in AccountsDataGrid.Items)
            {
                ((RadioButton)oldrow.FindControl("AccountsDataGrid_RadioButton")).Checked = false;
            }*/
            //Set the new selected row
          /*  RadioButton rb = (RadioButton)sender;
            DataGridItem row = (DataGridItem)rb.NamingContainer;
            ((RadioButton)row.FindControl("AccountsDataGrid_RadioButton")).Checked = true;
            int accountId = ((List<int>)Session["AccountsTab_UserControl_AccountsIds"])[row.DataSetIndex];
            Selected_AccountsDataGrid_Index.Value = accountId.ToString();
            int userId = -1;
            dataBlock.OpenConnection();
            List<int> usersList = dataBlock.usersTable.Get_AllUsersId(accountId, dataBlock.usersTable.DealerUserTypeId);
            dataBlock.CloseConnection();
            if (usersList.Count >0)
                userId = usersList[0];
            // else throw new Exception("Нет пользователей для этого дилера");
            LoadAccountAdditionalInfo(accountId, userId);
            AccountsDataGridUpdatePanel.Update();
            DetailedInformationUpdatePanel.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }*/

    protected void EditAccountButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (Selected_AccountsDataGrid_Index.Value == "")
                throw new Exception("Выберите дилера!");
            EditAccount(true);
            DetailedInformationPanel.Enabled = true;
            //AccountsTabContainer.ActiveTab = DetailedInfoTab;         Сделать переключение вкладок!
            NewOrEditUser_hdnField.Value = "edit";
            DetailedInformationUpdatePanel.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void NewAccountButton_Click(object sender, EventArgs e)
    {
        try
        {
            /*  foreach (DataGridItem oldrow in AccountsDataGrid.Items)
              {
                  ((RadioButton)oldrow.FindControl("UsersDataGrid_RadioButton")).Checked = false;
              }*/
            EditAccount(true);
            DetailedInformationPanel.Enabled = true;
            ClearAccountAdditionalInfo();
            //AccountsTabContainer.ActiveTab = DetailedInfoTab;         Сделать переключение вкладок!
            NewOrEditUser_hdnField.Value = "new";
            DetailedInformationUpdatePanel.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void CancelAccountButton_Click(object sender, EventArgs e)
    {
        DetailedInformationPanel.Enabled = false;
        EditAccount(false);
        //AccountsTabContainer.ActiveTab = AccountsTab;          Сделать переключение вкладок!
        DetailedInformationUpdatePanel.Update();
    }

    protected void SaveAccountButton_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);

        try
        {
            int accountId = -1;
            int userId = -1;

            string Login = DetailedInfo_Login_TextBox.Text;
            string password = DetailedInfo_Password_TextBox.Text;
            string passwordConfirm = DetailedInfo_PasswordConfirm_TextBox.Text;
            string accountName = DetailedInfo_AccountName_TextBox.Text;
            int accountType = Convert.ToInt32(AccountTypeDropDown.SelectedValue);
            int countryId = Convert.ToInt32(DetailedInfo_Country_DropDown.SelectedValue);

            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
            int orgTypeId = dataBlock.organizationTable.GetOrgTypeId(orgId);
            switch (NewOrEditUser_hdnField.Value)
            {
                case "new":
                    {
                        accountId = dataBlock.organizationTable.AddNewOrganization(accountName, accountType, countryId, 1, orgId);
                        UserFromTable newUser = new UserFromTable(Login, password, "", "", DateTime.Now, "");
                        if (accountType != dataBlock.organizationTable.ClientTypeId)
                            userId = dataBlock.usersTable.AddNewUser(newUser, dataBlock.usersTable.DealerUserTypeId, 1, accountId, 0);
                        else
                        {
                            userId = dataBlock.usersTable.AddNewUser(newUser, dataBlock.usersTable.AdministratorUserTypeId, 1, accountId, curUserId);
                            dataBlock.usersTable.AddUserInfoValue(userId, DataBaseReference.UserInfo_DealerId, orgId.ToString());
                        }
                    } break;
                case "edit":
                    {
                        accountId = Convert.ToInt32(Selected_AccountsDataGrid_Index.Value);
                        string oldName = dataBlock.organizationTable.GetOrganizationName(accountId);
                        //dataBlock.organizationTable.SetOrganizationName(accountName, accountId);
                        //dataBlock.organizationTable.SetOrgCountryAndRegion(accountId, countryId, 1);//тут регион ставится в 1, если надо будет регать где-то регион - поправить!                        
                        dataBlock.organizationTable.EditOrganization(oldName, accountName, accountType, countryId, 1);                 
                        /*userId = dataBlock.usersTable.Get_AllUsersId(accountId, dataBlock.usersTable.AccountUserTypeId)[0];
                        int userRoleId = dataBlock.usersTable.GetUserRoleId(userId);
                        string OldPass = dataBlock.usersTable.Get_UserPassword(userId);
                        string oldName = dataBlock.usersTable.Get_UserName(userId);
                        UserFromTable newUser = new UserFromTable(Login, password, "", "", DateTime.Now, "");
                        dataBlock.usersTable.EditUser(OldPass, oldName, newUser, dataBlock.usersTable.AccountUserTypeId, userRoleId, accountId, curUserId);*/
                    } break;
            }
            if (accountId < 0)
                throw new Exception("Не выбран аккаунт или произошла ошибка редактирования!");

            SaveAdditionalInformation(accountId, dataBlock);
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
            EditAccount(false);
            LoadAccountsTable();
            //AccountsTabContainer.ActiveTab = AccountsTab;   Сделать переключение вкладок!
            DetailedInformationUpdatePanel.Update();
            //this.Parent.
           // ((UpdatePanel)Page.FindControl("AccountsTreeUpdatePanel")).Update();
        }
    }

    protected void DeleteAccountButton_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        /*
                try
                {
                    int userId = Convert.ToInt32(Selected_UsersDataGrid_Index.Value);
                    int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
                    throw new Exception("Эта функция пока не работает");
                }
                catch (Exception ex)
                {
                    RaiseException(ex);
                }*/
    }

    private void SaveAdditionalInformation(int orgId, DataBlock dataBlock)
    {
        bool ONOFF = DetailedInfo_ONOFF_CheckBox.Checked;
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
        //ONOFF
        dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_ONOFF, ONOFF.ToString());
        //City
        dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_City, city);
        //ZIP
        dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_ZIP, zipCode);
        //timeZone
        dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_TimeZone, timeZone.ToString());
        //ADDRESS ONE
        dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_AddressOne, addressOne);
        //ADDRESS TWO
        dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_AddressTwo, addressTwo);
        //PHONE NUMBER
        dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_PhoneNumber, phoneNumb);
        //FAX
        dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_Fax, FAXNumb);
        //EMAIL
        dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_Email, email);
        //SITELANG
        dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_SiteLang, siteLang);
        //REPORTSLANG
        dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_ReportsLang, reportsLang);
    }

    private void EditAccount(bool editClicked)
    {
        if (editClicked)
        {
            EditAccountButton.Enabled = false;
            SaveAccountButton.Enabled = true;
            CancelAccountButton.Visible = true;
            DeleteAccountButton.Enabled = false;
            NewAccountButton.Enabled = false;
            //AccountsTab.Enabled = false;
        }
        else
        {
            EditAccountButton.Enabled = true;
            SaveAccountButton.Enabled = false;
            DeleteAccountButton.Enabled = true;
            CancelAccountButton.Visible = false;
            NewAccountButton.Enabled = true;
            //AccountsTab.Enabled = true;
        }
    }

    public void SelectItemInDataGrid(int orgId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        Selected_AccountsDataGrid_Index.Value = orgId.ToString();
        int userId = -1;
        dataBlock.OpenConnection();
        List<int> usersList = dataBlock.usersTable.Get_AllUsersId(orgId, dataBlock.usersTable.DealerUserTypeId);
        dataBlock.CloseConnection();
        if (usersList.Count > 0)
            userId = usersList[0];
        // else throw new Exception("Нет пользователей для этого дилера");
        LoadAccountAdditionalInfo(orgId, userId);
        DetailedInformationUpdatePanel.Update();
    }

    private void RaiseException(Exception ex)
    {
        Session["AdministrationTabException"] = ex;
        RaiseBubbleEvent(null, new EventArgs());
    }
}
