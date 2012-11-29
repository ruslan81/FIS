using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BLL;

public partial class Administrator_Adminisration_UserControls_DealersTab_UserControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SaveDealerButton.ButtOnClick += new EventHandler(SaveDealerButton_Click);
            NewDealerButton.ButtOnClick += new EventHandler(NewDealerButton_Click);
            EditDealerButton.ButtOnClick += new EventHandler(EditDealerButton_Click);
            CancelDealerButton.ButtOnClick += new EventHandler(CancelDealerButton_Click);
            DeleteDealerButton.ButtOnClick += new EventHandler(DeleteDealerButton_Click);
            if (!IsPostBack)
            {
                ClearDealerAdditionalInfo();
                Session["DealersTab_UserControl_DealersIds"] = null;
            }
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    public bool LoadDealersTable()//Возвращает False, если организация субдилер, у которой не может быть субдилеров
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        List<int> dealersIds = new List<int>();

        dataBlock.OpenConnection();
        int orgTypeId = dataBlock.organizationTable.GetOrgTypeId(orgId);
        if (orgTypeId == dataBlock.organizationTable.DealerTypeId || orgTypeId == dataBlock.organizationTable.PredealerTypeId)
        {
            //dealersIds = dataBlock.organizationTable.Get_AllOrganizationsId(dataBlock.organizationTable.DealerTypeId);
            dealersIds = dataBlock.organizationTable.Get_AllDealersId(orgId);

            Session["DealersTab_UserControl_DealersIds"] = dealersIds;

            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("DEALERNAME", typeof(string)));
            dt.Columns.Add(new DataColumn("LOGIN", typeof(string)));
            dt.Columns.Add(new DataColumn("REG_DATE", typeof(string)));
            dt.Columns.Add(new DataColumn("ENDREG_DATE", typeof(string)));
            dt.Columns.Add(new DataColumn("COUNTRY", typeof(string)));
            dt.Columns.Add(new DataColumn("CITY", typeof(string)));

            int dealerInfoId = 0;
            DateTime date = new DateTime();
            int userId = 1;
            foreach (int id in dealersIds)
            {
                userId = 0;

                dr = dt.NewRow();
                dr["DEALERNAME"] = dataBlock.organizationTable.GetOrganizationName(id);
                //LOGIN
                if (dataBlock.usersTable.Get_AllUsersId(id, dataBlock.usersTable.DealerUserTypeId) != null)
                {
                    userId = dataBlock.usersTable.Get_AllUsersId(id, dataBlock.usersTable.DealerUserTypeId)[0];
                    dr["LOGIN"] = dataBlock.usersTable.Get_UserName(userId);
                }
                //REG_DATE
                if (DateTime.TryParse(dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_RegDate), out date))
                    dr["REG_DATE"] = date.ToLongDateString() + " " + date.ToShortTimeString();
                //END_REG_DATE
                if (DateTime.TryParse(dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_EndOfRegistrationDate), out date))
                    dr["ENDREG_DATE"] = date.ToLongDateString();
                //COUNTRY
                dr["COUNTRY"] = dataBlock.organizationTable.GetOrgCountryName(id);
                //CITY              
                dr["CITY"] = dataBlock.organizationTable.GetAdditionalOrgInfo(id, DataBaseReference.OrgInfo_City);
                //
                dt.Rows.Add(dr);
            }
            DealersDataGrid.DataSource = dt;
            DealersDataGrid.DataBind();
        }
        else
        {
            return false;
        }
        dataBlock.CloseConnection();

        return true;

    }

    private void LoadDealerAdditionalInfo(int orgId, int userDealerId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");

        int userInfoId = 0;
        DateTime date = new DateTime();
        dataBlock.usersTable.OpenConnection();
        dataBlock.organizationTable.OpenConnection();
        //DEALERNAME
        DetailedInfo_DealerName_TextBox.Text = dataBlock.organizationTable.GetOrganizationName(orgId);
        //ONOFF
        string onOffStr = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_ONOFF);
        onOffStr = onOffStr.ToLower();
        if (onOffStr == "true" || onOffStr == "false")
        {
            bool OnChecked = Convert.ToBoolean(onOffStr);
            DetailedInfo_ONOFF_CheckBox.Checked = OnChecked;
        }
        //Login
        DetailedInfo_Login_TextBox.Text = dataBlock.usersTable.Get_UserName(userDealerId);
        //Password
        DetailedInfo_Password_TextBox.Text = dataBlock.usersTable.Get_UserPassword(userDealerId);
        DetailedInfo_PasswordConfirm_TextBox.Text = dataBlock.usersTable.Get_UserPassword(userDealerId);
        //REG_DATE
        if(DateTime.TryParse(dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_RegistrationDate), out date))
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
        if( DetailedInfo_TimeZone_DropDown.Items.FindByValue(dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_TimeZone))!=null)
            DetailedInfo_TimeZone_DropDown.Items.FindByValue(dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_TimeZone)).Selected = true;
        //Country
        if (DetailedInfo_Country_DropDown.Items.FindByText(dataBlock.organizationTable.GetOrgCountryName(orgId)) != null)
            DetailedInfo_Country_DropDown.Items.FindByText(dataBlock.organizationTable.GetOrgCountryName(orgId));
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
        if (DetailedInfo_ReportsLang_DropDown.Items.FindByValue(dataBlock.usersTable.GetUserInfoValue(userDealerId, userInfoId)) != null)
            DetailedInfo_ReportsLang_DropDown.Items.FindByValue(dataBlock.usersTable.GetUserInfoValue(userDealerId, userInfoId)).Selected = true;

        dataBlock.usersTable.CloseConnection();
        dataBlock.organizationTable.CloseConnection();
    }

    private void ClearDealerAdditionalInfo()
    {
        //SURNAME
        DetailedInfo_DealerName_TextBox.Text = "";
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
        dataBlock.OpenConnection();
        List<KeyValuePair<string, int>> orgCountries = dataBlock.organizationTable.GetAllCountry();
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

    protected void DealersDataGrid_RadioButton_Checked(object sender, EventArgs e)
    {
        try
        {
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
            foreach (DataGridItem oldrow in DealersDataGrid.Items)
            {
                ((RadioButton)oldrow.FindControl("DealersDataGrid_RadioButton")).Checked = false;
            }
            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            DataGridItem row = (DataGridItem)rb.NamingContainer;
            ((RadioButton)row.FindControl("DealersDataGrid_RadioButton")).Checked = true;
            int dealerId = ((List<int>)Session["DealersTab_UserControl_DealersIds"])[row.DataSetIndex];
            Selected_DealersDataGrid_Index.Value = dealerId.ToString();
            int userId = 0;
            dataBlock.usersTable.OpenConnection();
            List<int> usersList = dataBlock.usersTable.Get_AllUsersId(dealerId, dataBlock.usersTable.DealerUserTypeId);
            dataBlock.usersTable.CloseConnection();
            if (usersList != null)
                userId = usersList[0];
           // else throw new Exception("Нет пользователей для этого дилера");
            LoadDealerAdditionalInfo(dealerId,userId);
            DealersDataGridUpdatePanel.Update();
            DetailedInformationUpdatePanel.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void EditDealerButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (Selected_DealersDataGrid_Index.Value == "")
                throw new Exception("Выберите дилера!");
            EditDealer(true);
            DetailedInformationPanel.Enabled = true;
            //DealersTabContainer.ActiveTab = DetailedInfoTab;         Сделать переключение вкладок!
            NewOrEditUser_hdnField.Value = "edit";
            UpdatePanel1.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void NewDealerButton_Click(object sender, EventArgs e)
    {
        try
        {
          /*  foreach (DataGridItem oldrow in DealersDataGrid.Items)
            {
                ((RadioButton)oldrow.FindControl("UsersDataGrid_RadioButton")).Checked = false;
            }*/
            EditDealer(true);
            DetailedInformationPanel.Enabled = true;
            ClearDealerAdditionalInfo();
            //DealersTabContainer.ActiveTab = DetailedInfoTab;         Сделать переключение вкладок!
            NewOrEditUser_hdnField.Value = "new";
            UpdatePanel1.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void CancelDealerButton_Click(object sender, EventArgs e)
    {
        DetailedInformationPanel.Enabled = false;
        EditDealer(false);
        //DealersTabContainer.ActiveTab = DealersTab;          Сделать переключение вкладок!
        UpdatePanel1.Update();        
    }

    protected void SaveDealerButton_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);

        try
        {
            int dealerId = -1;
            int userId = -1;
             
            string Login = DetailedInfo_Login_TextBox.Text;
            string password = DetailedInfo_Password_TextBox.Text;
            string passwordConfirm = DetailedInfo_PasswordConfirm_TextBox.Text;
            string dealerName = DetailedInfo_DealerName_TextBox.Text;
            int countryId = Convert.ToInt32(DetailedInfo_Country_DropDown.SelectedValue);

            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
            int orgTypeId = dataBlock.organizationTable.GetOrgTypeId(orgId);
            switch (NewOrEditUser_hdnField.Value)
            {
                case "new":
                    {
                        int neworgTypeId;
                        if (orgTypeId == dataBlock.organizationTable.DealerTypeId)
                            neworgTypeId = dataBlock.organizationTable.SubdealerTypeId;
                        else
                            neworgTypeId = dataBlock.organizationTable.DealerTypeId;

                        dealerId = dataBlock.organizationTable.AddNewOrganization(dealerName, neworgTypeId, countryId, 1, orgId);
                        UserFromTable newUser = new UserFromTable(Login, password, "", "", DateTime.Now, "");
                        userId = dataBlock.usersTable.AddNewUser(newUser, dataBlock.usersTable.DealerUserTypeId, 1, dealerId, 0);
                    } break;
                case "edit":
                    {
                        dealerId = Convert.ToInt32(Selected_DealersDataGrid_Index.Value);
                        dataBlock.organizationTable.SetOrganizationName(dealerName, dealerId);
                        dataBlock.organizationTable.SetOrgCountryAndRegion(dealerId, countryId, 1);//тут регион ставится в 1, если надо будет регать где-то регион - поправить!

                        userId = dataBlock.usersTable.Get_AllUsersId(dealerId, dataBlock.usersTable.DealerUserTypeId)[0];
                        int userRoleId = dataBlock.usersTable.GetUserRoleId(userId);
                        string OldPass = dataBlock.usersTable.Get_UserPassword(userId);
                        string oldName = dataBlock.usersTable.Get_UserName(userId);
                        UserFromTable newUser = new UserFromTable(Login, password, "", "", DateTime.Now, "");
                        dataBlock.usersTable.EditUser(OldPass, oldName, newUser, dataBlock.usersTable.DealerUserTypeId, userRoleId, dealerId, curUserId);
                    } break;
            }
            if (userId < 0)
                throw new Exception("Не выбран пользователь или произошла ошибка редактирования!");

            SaveAdditionalInformation(dealerId, dataBlock);
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
            EditDealer(false);
            LoadDealersTable();
            //DealersTabContainer.ActiveTab = DealersTab;   Сделать переключение вкладок!
            UpdatePanel1.Update();
        }
    }

    protected void DeleteDealerButton_Click(object sender, EventArgs e)
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

    private void EditDealer(bool editClicked)
    {
        if (editClicked)
        {
            EditDealerButton.Enabled = false;
            SaveDealerButton.Enabled = true;
            CancelDealerButton.Visible = true;
            DeleteDealerButton.Enabled = false;
            NewDealerButton.Enabled = false;
            DealersTab.Enabled = false;
        }
        else
        {
            EditDealerButton.Enabled = true;
            SaveDealerButton.Enabled = false;
            DeleteDealerButton.Enabled = true;
            CancelDealerButton.Visible = false;
            NewDealerButton.Enabled = true;
            DealersTab.Enabled = true;
        }
    }


    private void RaiseException(Exception ex)
    {
        Session["AdministrationTabException"] = ex;
        RaiseBubbleEvent(null, new EventArgs());
    }
}
