using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BLL;

public partial class Administrator_GeneralTab : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
        if (Session["GeneralTableRowsSession"] != null)
        {
            GeneralTable.Rows.AddRange(((List<TableRow>)Session["GeneralTableRowsSession"]).ToArray());
        }
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

    public override bool Visible
    {
        get
        {
            return base.Visible;
        }
        set
        {
            base.Visible = value;
            Enabled = false;
        }
    }

    public void LoadAllInfo(int orgId, string Language)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, Language);
        try
        {

            dataBlock.organizationTable.OpenConnection();
            List<KeyValuePair<string, int>> allOrgInfos = new List<KeyValuePair<string, int>>();
            allOrgInfos = dataBlock.organizationTable.GetAllOrgInfos();
            // GeneralTable.Rows.Clear();

            List<TableRow> rows = new List<TableRow>();
            TableRow row;
            TableCell cell;
            Label label;
            TextBox textBox;
            foreach (KeyValuePair<string, int> pair in allOrgInfos)
            {
                row = new TableRow();

                cell = new TableCell();
                label = new Label();
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.Width = new Unit(35, UnitType.Percentage);
                label.Text = pair.Key;
                cell.Controls.Add(label);
                row.Cells.Add(cell);

                cell = new TableCell();
                textBox = new TextBox();
                textBox.Width = new Unit(80, UnitType.Percentage);
                textBox.ID = pair.Value.ToString();
                textBox.Text = dataBlock.organizationTable.GetAdditionalOrgInfo(orgId, pair.Value);
                cell.Controls.Add(textBox);
                row.Cells.Add(cell);

                rows.Add(row);
            }
            GeneralTable.Rows.AddRange(rows.ToArray());

            User_General_FullNameTextBox.Text = dataBlock.organizationTable.GetOrganizationName(orgId);
            User_General_CountryTextBox.Text = dataBlock.organizationTable.GetOrgCountryName(orgId);
            User_General_RegionTextBox.Text = dataBlock.organizationTable.GetOrgRegionName(orgId);
            Session["GeneralTableRowsSession"] = rows;
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
        finally
        {
            dataBlock.organizationTable.CloseConnection();
        }
    }

    public void SaveChanges(int orgId, string Language)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, Language);
        try
        {
            dataBlock.organizationTable.OpenConnection();
            List<KeyValuePair<string, int>> allOrgInfos = new List<KeyValuePair<string, int>>();
            allOrgInfos = dataBlock.organizationTable.GetAllOrgInfos();
            TextBox tempTextBox;
            foreach (KeyValuePair<string, int> pair in allOrgInfos)
            {
                tempTextBox = new TextBox();
                tempTextBox = (TextBox)GeneralTablePanel.FindControl(pair.Value.ToString());
                dataBlock.organizationTable.AddOrEditAdditionalOrgInfo(orgId, pair.Value, tempTextBox.Text);
            }
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
        finally
        {
            dataBlock.organizationTable.CloseConnection();
        }
    }
}
