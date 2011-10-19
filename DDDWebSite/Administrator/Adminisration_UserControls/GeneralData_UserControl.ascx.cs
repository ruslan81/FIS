using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Configuration;
using System.Data;

public partial class Administrator_Adminisration_UserControls_GeneralData_UserControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            RegistrationDataTab_SaveButton.ButtOnClick += new EventHandler(SaveButtonPressed);
            if (!IsPostBack)
            {
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }

    public void LoadAllLists()
    {
        LoadTabContainer();
        LoadStatisticGrid();
        LoadMessagesGrid();
    }

    private void LoadTabContainer()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        dataBlock.OpenConnection();
        int userId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        DateTime date = new DateTime();
        //GeneralInfo
        date = dataBlock.usersTable.Get_TimeConnect(userId);
        GeneralDataTab_Table_CurConnectDateValue.Text = date.ToLongDateString() + " " + date.ToShortTimeString();
        if (DateTime.TryParse(dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_RegistrationDate), out date))
            GeneralDataTab_Table_RegDateValue.Text = date.ToLongDateString() + " " + date.ToShortTimeString();
        if (DateTime.TryParse(dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_EndOfRegistrationDate), out date))
            GeneralDataTab_Table_EndRegDateValue.Text = date.ToLongDateString();
        GeneralDataTab_Table_LicenseTypeValue.Text = "Flat";
        //Registration data
        RegistrationDataTab_RegCodeValue.Text = orgId.ToString();

        RegistrationDataTab_CityDropDown.Items.Clear();
        RegistrationDataTab_TownDropDown.Items.Clear();
        List<KeyValuePair<string, int>> orgCountries = dataBlock.organizationTable.GetAllCountry();
        string orgCountryName = dataBlock.organizationTable.GetOrgCountryName(orgId);
        int countryId = 1;
        foreach (KeyValuePair<string, int> country in orgCountries)
        {
            RegistrationDataTab_CityDropDown.Items.Add(new ListItem(country.Key, country.Value.ToString()));
            if (country.Key == orgCountryName)
            {
                RegistrationDataTab_CityDropDown.Items[RegistrationDataTab_CityDropDown.Items.Count - 1].Selected = true;
                countryId = country.Value;
            }
        }

        List<KeyValuePair<string, int>> orgRegions = dataBlock.organizationTable.GetAllRegions(countryId);

        string orgCRegionName = dataBlock.organizationTable.GetOrgRegionName(orgId);
        foreach (KeyValuePair<string, int> region in orgRegions)
        {
            RegistrationDataTab_TownDropDown.Items.Add(new ListItem(region.Key, region.Value.ToString()));
            if (region.Key == orgCRegionName)
            {
                RegistrationDataTab_TownDropDown.Items[RegistrationDataTab_TownDropDown.Items.Count - 1].Selected = true;
            }
        }

        RegistrationDataTab_AddressTextBox.Text = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_Address);

        //Language
        string CurrentLanguage = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_SiteLang);
        string Russian = "STRING_RU";
        string English = "STRING_EN";
        RegistrationDataTab_LanguageDropDown.Items.Clear();
        RegistrationDataTab_LanguageDropDown.Items.Add(new ListItem("Русский", Russian));
        RegistrationDataTab_LanguageDropDown.Items.Add(new ListItem("English", English));
        if (CurrentLanguage == Russian)
            RegistrationDataTab_LanguageDropDown.Items[0].Selected = true;
        if (CurrentLanguage == English)
            RegistrationDataTab_LanguageDropDown.Items[1].Selected = true;

        RegistrationDataTab_SaveDataPeriodTextBox.Text = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_StorageDataPeriod); ;
        dataBlock.CloseConnection();
    }

    private void LoadStatisticGrid()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn(" ", typeof(string)));
        dt.Columns.Add(new DataColumn("Текущее", typeof(string)));
        dt.Columns.Add(new DataColumn("Доступно", typeof(string)));
        dataBlock.OpenConnection();
        dr = dt.NewRow();
        dr[" "] = "Количество пользователей";
        dr["Текущее"] = dataBlock.usersTable.Get_AllUsersId(orgId).Count;
        dr["Доступно"] = "20";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr[" "] = "Количество водителей";
        dr["Текущее"] = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.driversCardTypeId).Count;
        dr["Доступно"] = "25";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr[" "] = "Количество машин";
        dr["Текущее"] = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.vehicleCardTypeId).Count;
        dr["Доступно"] = "30";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr[" "] = "Количество отчетов";
        dr["Текущее"] = "???";
        dr["Доступно"] = "???";
        dt.Rows.Add(dr);

        dataBlock.CloseConnection();

        StatisticGrid.DataSource = dt;
        StatisticGrid.DataBind();
    }

    private void LoadMessagesGrid()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("Отправитель", typeof(string)));
        dt.Columns.Add(new DataColumn("Тема", typeof(string)));
        dt.Columns.Add(new DataColumn("Дата", typeof(string)));
        dt.Columns.Add(new DataColumn("Срок окончания", typeof(string)));

        dr = dt.NewRow();
        dr["Отправитель"] = "GoldMedium";
        dr["Тема"] = "Необходимо обновить регистрационную информацию";
        dr["Дата"] = "17.01.2011";
        dr["Срок окончания"] = "25.01.2011";
        dt.Rows.Add(dr);

        MessagesGrid.DataSource = dt;
        MessagesGrid.DataBind();
    }

    protected void SaveButtonPressed(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int countryId = Convert.ToInt32(RegistrationDataTab_CityDropDown.SelectedValue);
            int regionId = Convert.ToInt32(RegistrationDataTab_TownDropDown.SelectedValue);
            string address = RegistrationDataTab_AddressTextBox.Text;
            string selectedLanguage = RegistrationDataTab_LanguageDropDown.SelectedValue;
            int saveDataPeriod = Convert.ToInt32(RegistrationDataTab_SaveDataPeriodTextBox.Text);


            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);

            dataBlock.organizationTable.OpenConnection();
            dataBlock.organizationTable.OpenTransaction();
            dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_Address, address);
            dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_SiteLang, selectedLanguage);
            dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_StorageDataPeriod, saveDataPeriod.ToString());
            dataBlock.organizationTable.SetOrgCountryAndRegion(orgId, countryId, regionId);
            dataBlock.organizationTable.CommitTransaction();
            dataBlock.organizationTable.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.organizationTable.RollbackConnection();
            dataBlock.organizationTable.CloseConnection();
            Session["AdministrationTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
    }

    protected void RegistrationDataTab_TownDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        int countryId = Convert.ToInt32(RegistrationDataTab_CityDropDown.SelectedValue);
        RegistrationDataTab_TownDropDown.Items.Clear();

        List<KeyValuePair<string, int>> orgRegions = new List<KeyValuePair<string, int>>();
        orgRegions = dataBlock.organizationTable.GetAllRegions(countryId);
        foreach (KeyValuePair<string, int> region in orgRegions)
        {
            RegistrationDataTab_TownDropDown.Items.Add(new ListItem(region.Key, region.Value.ToString()));
        }
    }
}
