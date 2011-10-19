using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ExitButt2.ButtOnClick += new EventHandler(ExitFromSite);

            ReportsMasterButt.ButtOnClick += new EventHandler(ReportsRedirect);
            DataMasterButt.ButtOnClick += new EventHandler(DataRedirect);
            SettingsMasterButt.ButtOnClick += new EventHandler(SettingsRedirect);

            HelpMasterButt.ButtOnClick += new EventHandler(HelpRedirect);
            AdministrationMasterButt.ButtOnClick += new EventHandler(AdministrationRedirect);


            if (!IsPostBack)
            {
                MasterPageExceptionString.Text = "";

                string connectionString = System.Configuration.ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
                BLL.DataBlock dataBlock = new BLL.DataBlock(connectionString, "STRING_EN");
                dataBlock.OpenConnection();
                int userId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
                string UserType = dataBlock.usersTable.Get_UserTypeStr(userId);
                dataBlock.CloseConnection();                
                if (UserType == "Driver")
                {
                    SettingsMasterButt.Visible = false;
                    AdministrationMasterButt.Visible = false;
                    DataMasterButt.Visible = false;
                }
                if (UserType == "Manager")
                {
                    SettingsMasterButt.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            MasterPageExceptionString.Text = "Ошибка мастер страницы: " +  ex.Message;
        }
    }

    protected void ReportsRedirect(object sender, EventArgs e)
    {
        MakeAllEnabled();
        Response.Redirect("~/Administrator/Reports.aspx");
    }

    protected void DataRedirect(object sender, EventArgs e)
    {
        MakeAllEnabled();
        Response.Redirect("~/Administrator/Data.aspx");
    }

    protected void SettingsRedirect(object sender, EventArgs e)
    {
        MakeAllEnabled();
        Response.Redirect("~/Administrator/Settings.aspx");
    }

    protected void HelpRedirect(object sender, EventArgs e)
    {
        MakeAllEnabled();
        Response.Redirect("~/Administrator/ReportServiceTest.aspx");
    }

    protected void AdministrationRedirect(object sender, EventArgs e)
    {
        MakeAllEnabled();
        Response.Redirect("~/Administrator/Administration.aspx");
    }

    private void MakeAllEnabled()
    {
        ReportsMasterButt.Enabled = true;
        DataMasterButt.Enabled = true;
        SettingsMasterButt.Enabled = true;
        HelpMasterButt.Enabled = true;
        AdministrationMasterButt.Enabled = true;
    }
   
    public void ResizeAdditionalConditionsDiv(int pixels)
    {
        System.Web.UI.HtmlControls.HtmlGenericControl outputDiv = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("addConditionsDivId");
        if (outputDiv != null)
            outputDiv.Style.Add("height", pixels.ToString() + "px");
        AddConditionsUpdatePanel.Update();
    }

    protected void ExitFromSite(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        //FormsAuthentication.RedirectToLoginPage();
        Response.Redirect("~/loginPage.aspx");
    }
}
