using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BLL;

public partial class AdministratorS_CreatingFormsControls_GeneralTab_CretingFormsControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            OrganizationFullNameTextBox.Text = "";
            OrganizationTypeDropDown.Items.Clear();
            OrgCountryDropDownList.Items.Clear();
            OrgRegionDropDownList.Items.Clear();
            Session["OrgEditControl_BubbleException"] = "";
        }
        Session["CreateOrgVisible"] = true;
    }

    public bool Enabled
    {
        get
        {
            return GeneralTable.Enabled;
        }
        set
        {
            GeneralTable.Enabled = value;
            GeneralLogoPanel.Enabled = value;
        }
    }

    public void LoadOrgInfo(int id)
    {
        OrganizationTypeDropDown.Items.Clear();
        OrgCountryDropDownList.Items.Clear();
        OrgRegionDropDownList.Items.Clear();

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        OrganizationFromTable uft = new OrganizationFromTable();
        uft.FillWithInfo(id, dataBlock.organizationTable);
        if (id > 0)
        {
            OrganizationFullNameTextBox.Text = uft.orgName;
            OldOrgName.Value = uft.orgName;
        }
        List<KeyValuePair<string, int>> orgTypes = new List<KeyValuePair<string, int>>();
        orgTypes = dataBlock.organizationTable.GetAllOrganizationTypes();
        foreach (KeyValuePair<string, int> type in orgTypes)
        {
            OrganizationTypeDropDown.Items.Add(new ListItem(type.Key, type.Value.ToString()));
            if (id > 0)
                if (type.Key == uft.orgType)
                    OrganizationTypeDropDown.Items[OrganizationTypeDropDown.Items.Count - 1].Selected = true;
        }
        List<KeyValuePair<string, int>> orgCountries = new List<KeyValuePair<string, int>>();
        orgCountries = dataBlock.organizationTable.GetAllCountry();
        int currentCountryId = -1;
        foreach (KeyValuePair<string, int> country in orgCountries)
        {
            OrgCountryDropDownList.Items.Add(new ListItem(country.Key, country.Value.ToString()));
            if (id > 0)
                if (country.Key == uft.countryName)
                {
                    OrgCountryDropDownList.Items[OrgCountryDropDownList.Items.Count - 1].Selected = true;
                    currentCountryId = country.Value;
                }
        }
        if (currentCountryId == -1)
        {
            if (OrgCountryDropDownList.Items[0] != null)
            {
                currentCountryId = 1;
                OrgCountryDropDownList.Items[0].Selected = true;
            }
            else
                throw new Exception("Ошибки в таблицах стран и регионов!");
        }
        List<KeyValuePair<string, int>> orgRegions = new List<KeyValuePair<string, int>>();
        orgRegions = dataBlock.organizationTable.GetAllRegions(currentCountryId);
        foreach (KeyValuePair<string, int> region in orgRegions)
        {
            OrgRegionDropDownList.Items.Add(new ListItem(region.Key, region.Value.ToString()));
            if (id > 0)
                if (region.Key == uft.regionName)
                    OrgRegionDropDownList.Items[OrgRegionDropDownList.Items.Count - 1].Selected = true;
        }
        currentHiddenOrgId.Value = id.ToString();
    }

    protected void ChangeDrName_OK_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
            bool AddingNewOrg = (bool)Session["AddingNewOrg"];
            if (AddingNewOrg == false)
            {
                string orgName = OrganizationFullNameTextBox.Text;
                int countryId = Convert.ToInt32(OrgCountryDropDownList.SelectedValue);
                int regionId = Convert.ToInt32(OrgRegionDropDownList.SelectedValue);
                int orgType = Convert.ToInt32(OrganizationTypeDropDown.SelectedValue);
                dataBlock.organizationTable.EditOrganization(OldOrgName.Value, orgName, orgType, countryId, regionId);
                //this.Visible = false;
            }
            else
            {
                string orgName = OrganizationFullNameTextBox.Text;
                int countryId = Convert.ToInt32(OrgCountryDropDownList.SelectedValue);
                int regionId = Convert.ToInt32(OrgRegionDropDownList.SelectedValue);
                int orgType = Convert.ToInt32(OrganizationTypeDropDown.SelectedValue);
                dataBlock.organizationTable.AddNewOrganization(orgName, orgType, countryId, regionId);
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            Session["UserEditControl_BubbleException"] = ex.Message;
        }
        finally
        {
            Session["CreateOrgVisible"] = false;
            RaiseBubbleEvent(sender, e);
        }
    }
    protected void ChangeDrName_Cancel_Click(object sender, ImageClickEventArgs e)
    {
        Session["CreateOrgVisible"] = false;
        RaiseBubbleEvent(sender, e);
    }

    protected void OrgCountryDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        int countryId = Convert.ToInt32(OrgCountryDropDownList.SelectedValue);
        OrgRegionDropDownList.Items.Clear();

        List<KeyValuePair<string, int>> orgRegions = new List<KeyValuePair<string, int>>();
        orgRegions = dataBlock.organizationTable.GetAllRegions(countryId);
        foreach (KeyValuePair<string, int> region in orgRegions)
        {
            OrgRegionDropDownList.Items.Add(new ListItem(region.Key, region.Value.ToString()));
        }

    }
}

   
