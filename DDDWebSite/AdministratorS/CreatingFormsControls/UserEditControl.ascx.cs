using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BLL;

public partial class AdministratorS_CreatingFormsControls_UserEditControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoginTextBox.Text = "";
            PasswortTextBox.Text = "";
            UserTypeDropDownList.Items.Clear();
            UserRoleDropDownList.Items.Clear();
            OrganizationNameDropDownList.Items.Clear();
            Session["UserEditControl_BubbleException"] = "";
        }
    }

    private void UpdateControl()
    {
        LoginTextBox.Text = "";
        PasswortTextBox.Text = "";
        UserTypeDropDownList.Items.Clear();
        UserRoleDropDownList.Items.Clear();
        OrganizationNameDropDownList.Items.Clear();
    }

    public bool Enabled
    {
        get
        {
            return UserEditPanel.Enabled;
        }
        set
        {
            UserEditPanel.Enabled = value;
        }
    }

    protected void ChangeDrName_OK_Click(object sender, ImageClickEventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
          
            UserFromTable uft = new UserFromTable();
            bool AddingNewUser = (bool)Session["AddingNewUser"];
            if (AddingNewUser == false)
            {
                uft.name = LoginTextBox.Text;
                uft.pass = PasswortTextBox.Text;
                uft.userRole = UserRoleDropDownList.SelectedItem.Text;
                int userRoleId = Convert.ToInt32(UserRoleDropDownList.SelectedItem.Value);
                uft.userType = UserTypeDropDownList.SelectedItem.Text;
                int userTypeId = Convert.ToInt32(UserTypeDropDownList.SelectedItem.Value);
                int orgId = Convert.ToInt32(OrganizationNameDropDownList.SelectedValue);
                dataBlock.usersTable.EditUser(OldPassHF.Value, OldNameHF.Value, uft, userTypeId, userRoleId, orgId, curUserId);
                //this.Visible = false;
            }
            else
            {
                uft.name = LoginTextBox.Text;
                uft.pass = PasswortTextBox.Text;
                uft.userRole = UserRoleDropDownList.SelectedItem.Text;
                int userRoleId = Convert.ToInt32(UserRoleDropDownList.SelectedItem.Value);
                uft.userType = UserTypeDropDownList.SelectedItem.Text;
                int userTypeId = Convert.ToInt32(UserTypeDropDownList.SelectedItem.Value);
                int orgId = Convert.ToInt32(OrganizationNameDropDownList.SelectedValue);
                dataBlock.usersTable.AddNewUser(uft, userTypeId, userRoleId, orgId, curUserId);
            }
            dataBlock.CommitTransaction();
            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.RollbackConnection();
            dataBlock.CloseConnection();
            Status.Text = ex.Message;
            Session["UserEditControl_BubbleException"] = ex.Message;
        }
        finally
        {
            UpdateControl();
            RaiseBubbleEvent(sender, e);
        }
    }

    protected void ChangeDrName_Cancel_Click(object sender, ImageClickEventArgs e)
    {
        UpdateControl();
        RaiseBubbleEvent(sender, e);
    }

    public void LoadUserInfo(int id)
    {
        UserTypeDropDownList.Items.Clear();
        UserRoleDropDownList.Items.Clear();

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        UserFromTable uft = new UserFromTable();

        dataBlock.OpenConnection();
        uft.FillWithInfo(id, dataBlock.usersTable);
        if (id > 0)
        {
            LoginTextBox.Text = uft.name;
            PasswortTextBox.Text = uft.pass;
            OldNameHF.Value = uft.name;
            OldPassHF.Value = uft.pass;
        }
        List<KeyValuePair<string, int>> userTypes = new List<KeyValuePair<string, int>>();
        userTypes = dataBlock.usersTable.GetAllUsersTypes();
        foreach (KeyValuePair<string, int> type in userTypes)
        {
            UserTypeDropDownList.Items.Add(new ListItem(type.Key, type.Value.ToString()));
            if (id > 0)
                if (type.Key == uft.userType)
                    UserTypeDropDownList.Items[UserTypeDropDownList.Items.Count - 1].Selected = true;
        }

        List<KeyValuePair<string, int>> userRoles = new List<KeyValuePair<string, int>>();
        userRoles = dataBlock.usersTable.GetAllUsersRoles();
        foreach (KeyValuePair<string, int> role in userRoles)
        {
            UserRoleDropDownList.Items.Add(new ListItem(role.Key, role.Value.ToString()));
            if (id > 0)
                if (role.Key == uft.userRole)
                    UserRoleDropDownList.Items[UserRoleDropDownList.Items.Count - 1].Selected = true;
        }

        List<KeyValuePair<string, int>> orgNames = new List<KeyValuePair<string, int>>();

        orgNames = dataBlock.organizationTable.GetAllOrganizationNames();
        foreach (KeyValuePair<string, int> name in orgNames)
        {
            OrganizationNameDropDownList.Items.Add(new ListItem(name.Key, name.Value.ToString()));
            if (id > 0)
                if (name.Key == uft.orgName)
                    OrganizationNameDropDownList.Items[OrganizationNameDropDownList.Items.Count - 1].Selected = true;
        }
        dataBlock.CloseConnection();
    }
}
