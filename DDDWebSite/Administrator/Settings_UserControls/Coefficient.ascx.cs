using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BLL;

public partial class Administrator_Settings_UserControls_Coefficient : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
        }
    }

    public void LoadKoefList()
    {
        string CurrentLanguage = ConfigurationManager.AppSettings["language"];
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, CurrentLanguage);
        List<KeyValuePair<string, int>> allKeys = new List<KeyValuePair<string, int>>();
        allKeys = dataBlock.criteriaTable.GetAllCriteria_Name_n_Id();

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("KEY_ID", typeof(int)));
        dt.Columns.Add(new DataColumn("KEY_NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("MEASURE_NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("KEY_VALUE_MIN", typeof(int)));
        dt.Columns.Add(new DataColumn("KEY_VALUE_MAX", typeof(int)));
        dt.Columns.Add(new DataColumn("KEY_NOTE", typeof(string)));

        CriteriaTable oneCriteria = new CriteriaTable(connectionString, CurrentLanguage, dataBlock.sqlDb);

        foreach (KeyValuePair<string, int> key in allKeys)
        {
            oneCriteria = dataBlock.criteriaTable.LoadCriteria(key.Value);

            dr = dt.NewRow();
            dr["KEY_ID"] = oneCriteria.KeyId;
            dr["MEASURE_NAME"] = oneCriteria.MeasureName;
            dr["KEY_NAME"] = oneCriteria.CriteriaName;
            dr["KEY_VALUE_MIN"] = oneCriteria.MinValue;
            dr["KEY_VALUE_MAX"] = oneCriteria.MaxValue;
            dr["KEY_NOTE"] = oneCriteria.CriteriaNote;
            dt.Rows.Add(dr);
        }
        KoeffGrid.DataSource = dt;
        KoeffGrid.DataBind();
    }

    protected void KoefDataGrid_Edit(Object s, DataGridCommandEventArgs e)
    {
        try
        {
            KoeffGrid.EditItemIndex = e.Item.ItemIndex;
            LoadKoefList();
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
    }
    protected void KoefDataGrid_Cancel(Object s, DataGridCommandEventArgs e)
    {
        try
        {
            KoeffGrid.EditItemIndex = -1;
            LoadKoefList();
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
    }
    protected void KoefDataGrid_Update(Object s, DataGridCommandEventArgs e)
    {
        int gridItemIndex = e.Item.ItemIndex;
        int koefId = Convert.ToInt32(e.Item.Cells[0].Text);
        string dataField = "";
        string cellText = "";
        int cellNumber = 0;
        int MinValue = 0;
        int MaxValue = 0;
        string Note = "";
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        try
        {
            foreach (object dc in KoeffGrid.Columns)
            {
                if (dc is BoundColumn)
                {
                    if (((BoundColumn)dc).Visible && ((BoundColumn)dc).ReadOnly!=true)
                    {
                        dataField = ((BoundColumn)dc).DataField;
                        cellText = ((TextBox)e.Item.Cells[cellNumber].Controls[0]).Text;
                        if(dataField == "KEY_VALUE_MIN")
                            MinValue = Convert.ToInt32(cellText);
                        if(dataField == "KEY_VALUE_MAX")
                            MaxValue = Convert.ToInt32(cellText);
                        if (dataField == "KEY_NOTE")
                            Note = cellText;
                    }
                }
                cellNumber++;
            }
            dataBlock.criteriaTable.EditCriteria(koefId, Note, MinValue, MaxValue);
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
        finally
        {
            LoadKoefList();
            KoefDataGrid_Cancel(s, e);
        }
    }
}
