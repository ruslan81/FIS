using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BLL;

public partial class Administrator_Adminisration_UserControls_ClientsTab_UserControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SaveClientButton.ButtOnClick += new EventHandler(SaveClientButton_Click);
            NewClientButton.ButtOnClick += new EventHandler(NewClientButton_Click);
            EditClientButton.ButtOnClick += new EventHandler(EditClientButton_Click);
            CancelClientButton.ButtOnClick += new EventHandler(CancelClientButton_Click);
            DeleteClientButton.ButtOnClick += new EventHandler(DeleteClientButton_Click);
            if (!IsPostBack)
            {
                ClearClientAdditionalInfo();
                Session["ClientsTab_UserControl_ClientsIds"] = null;
            }
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    public void LoadClientsTable()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        List<int> clientsIds = new List<int>();

        dataBlock.OpenConnection();
        int orgTypeId = dataBlock.organizationTable.GetOrgTypeId(orgId);
        clientsIds = dataBlock.organizationTable.Get_AllOrganizationsId(orgId);

        Session["ClientsTab_UserControl_ClientsIds"] = clientsIds;

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("CLIENTNAME", typeof(string)));
        dt.Columns.Add(new DataColumn("REG_DATE", typeof(string)));
        dt.Columns.Add(new DataColumn("ENDREG_DATE", typeof(string)));
        dt.Columns.Add(new DataColumn("COUNTRY", typeof(string)));
        dt.Columns.Add(new DataColumn("CITY", typeof(string)));

        int clientInfoId = 0;
        DateTime date = new DateTime();
        int userId = 1;
        foreach (int id in clientsIds)
        {
            userId = 0;

            dr = dt.NewRow();
            dr["CLIENTNAME"] = dataBlock.organizationTable.GetOrganizationName(id);
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
            //
            dt.Rows.Add(dr);
        }
        ClientsDataGrid.DataSource = dt;
        ClientsDataGrid.DataBind();
        dataBlock.CloseConnection();
    }

    private void LoadClientAdditionalInfo(int orgId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");

        int userInfoId = 0;
        DateTime date = new DateTime();
        dataBlock.OpenConnection();
        //DEALERNAME
        DetailedInfo_ClientName_TextBox.Text = dataBlock.organizationTable.GetOrganizationName(orgId);
        //ONOFF
        string onOffStr = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_ONOFF);
        onOffStr = onOffStr.ToLower();
        if (onOffStr == "true" || onOffStr == "false")
        {
            bool OnChecked = Convert.ToBoolean(onOffStr);
            DetailedInfo_ONOFF_CheckBox.Checked = OnChecked;
        }
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
      
        dataBlock.usersTable.CloseConnection();
        dataBlock.organizationTable.CloseConnection();
    }

    private void ClearClientAdditionalInfo()
    {
        //SURNAME
        DetailedInfo_ClientName_TextBox.Text = "";
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

    protected void ClientsDataGrid_RadioButton_Checked(object sender, EventArgs e)
    {
        try
        {
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
            foreach (DataGridItem oldrow in ClientsDataGrid.Items)
            {
                ((RadioButton)oldrow.FindControl("ClientsDataGrid_RadioButton")).Checked = false;
            }
            //Set the new selected row
            RadioButton rb = (RadioButton)sender;
            DataGridItem row = (DataGridItem)rb.NamingContainer;
            ((RadioButton)row.FindControl("ClientsDataGrid_RadioButton")).Checked = true;
            int clientId = ((List<int>)Session["ClientsTab_UserControl_ClientsIds"])[row.DataSetIndex];
            Selected_ClientsDataGrid_Index.Value = clientId.ToString();
            LoadClientAdditionalInfo(clientId);
            ClientsDataGridUpdatePanel.Update();
            DetailedInformationUpdatePanel.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void EditClientButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (Selected_ClientsDataGrid_Index.Value == "")
                throw new Exception("Выберите дилера!");
            EditClient(true);
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

    protected void NewClientButton_Click(object sender, EventArgs e)
    {
        try
        {
            /*  foreach (DataGridItem oldrow in DealersDataGrid.Items)
              {
                  ((RadioButton)oldrow.FindControl("UsersDataGrid_RadioButton")).Checked = false;
              }*/
            EditClient(true);
            DetailedInformationPanel.Enabled = true;
            ClearClientAdditionalInfo();
            //DealersTabContainer.ActiveTab = DetailedInfoTab;         Сделать переключение вкладок!
            NewOrEditUser_hdnField.Value = "new";
            UpdatePanel1.Update();
        }
        catch (Exception ex)
        {
            RaiseException(ex);
        }
    }

    protected void CancelClientButton_Click(object sender, EventArgs e)
    {
        DetailedInformationPanel.Enabled = false;
        EditClient(false);
        //DealersTabContainer.ActiveTab = DealersTab;          Сделать переключение вкладок!
        UpdatePanel1.Update();
    }

    protected void SaveClientButton_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);

        try
        {
            int newClientId = -1;
            int userId = -1;

            string clientName = DetailedInfo_ClientName_TextBox.Text;
            int countryId = Convert.ToInt32(DetailedInfo_Country_DropDown.SelectedValue);

            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
            int orgTypeId = dataBlock.organizationTable.GetOrgTypeId(orgId);
            switch (NewOrEditUser_hdnField.Value)
            {
                case "new":
                    {
                        int neworgTypeId = dataBlock.organizationTable.ClientTypeId;
                        newClientId = dataBlock.organizationTable.AddNewOrganization(clientName, neworgTypeId, countryId, 1, orgId);
                        if (clientName.Length > 9)
                            clientName = clientName.Substring(0, 9);

                        UserFromTable newUser = new UserFromTable(clientName, "123", "", "", DateTime.Now, "");
                        userId = dataBlock.usersTable.AddNewUser(newUser, dataBlock.usersTable.AdministratorUserTypeId, 1, newClientId, 0);
                        dataBlock.usersTable.AddUserInfoValue(userId, DataBaseReference.UserInfo_DealerId, orgId.ToString());
                    } break;
                case "edit":
                    {
                        newClientId = Convert.ToInt32(Selected_ClientsDataGrid_Index.Value);
                        dataBlock.organizationTable.SetOrganizationName(clientName, newClientId);
                        dataBlock.organizationTable.SetOrgCountryAndRegion(newClientId, countryId, 1);//тут регион ставится в 1, если надо будет регать где-то регион - поправить!
                    } break;
            }
            if (userId < 0)
                throw new Exception("Не выбран пользователь или произошла ошибка редактирования!");

            SaveAdditionalInformation(newClientId, dataBlock);
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
            EditClient(false);
            LoadClientsTable();
            //DealersTabContainer.ActiveTab = DealersTab;   Сделать переключение вкладок!
            UpdatePanel1.Update();
        }
    }

    protected void DeleteClientButton_Click(object sender, EventArgs e)
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

    private void EditClient(bool editClicked)
    {
        if (editClicked)
        {
            EditClientButton.Enabled = false;
            SaveClientButton.Enabled = true;
            CancelClientButton.Visible = true;
            DeleteClientButton.Enabled = false;
            NewClientButton.Enabled = false;
            ClientsTab.Enabled = false;
        }
        else
        {
            EditClientButton.Enabled = true;
            SaveClientButton.Enabled = false;
            DeleteClientButton.Enabled = true;
            CancelClientButton.Visible = false;
            NewClientButton.Enabled = true;
            ClientsTab.Enabled = true;
        }
    }

    private void RaiseException(Exception ex)
    {
        Session["AdministrationTabException"] = ex;
        RaiseBubbleEvent(null, new EventArgs());
    }
}
