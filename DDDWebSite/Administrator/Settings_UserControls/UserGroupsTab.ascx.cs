using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Administrator_Settings_UserControls_UserGroupsTab : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
         //   GroupsDataGrid.DataSource = createDataSource();
         //   GroupsDataGrid.DataBind();
        }
    }

    private DataTable createDataSource()
    {
        DataTable newDataTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "№";
        string Col_2 = "Название";
        string Col_3 = "Комментарий";

        newDataTable.Columns.Add(new DataColumn(Col_1, typeof(int)));
        newDataTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        newDataTable.Columns.Add(new DataColumn(Col_3, typeof(string)));

        for (int i = 0; i < 10; i++)
        {
            dr = newDataTable.NewRow();
            dr[Col_1] = i;
            dr[Col_2] = "Группа номер " + i.ToString();
            dr[Col_3] = "Это комментарий к группе номер " + i.ToString();
            newDataTable.Rows.Add(dr);
        }
        return newDataTable;
    }

    public bool ShowEdit
    {
        get
        {
            return EditPanel.Visible;
        }
        set
        {
            if (value == true)
                Load_EditInfo();
            EditPanel.Visible = value;
            GroupsDataGridPanel.Visible = !value;           
        }
    }

    public void Load_EditInfo()
    {
        try
        {
            string selectedIndexString = Selected_GroupsDataGrid_Index.Value;
            if (selectedIndexString == "")
                throw new Exception("Выберите группу для редактирования");
            int selectedIndex = Convert.ToInt32(selectedIndexString);
            Edit_GroupNameTextBox.Text = GroupsDataGrid.Items[selectedIndex].Cells[2].Text;
            Edit_GroupCommentTextBox.Text = GroupsDataGrid.Items[selectedIndex].Cells[3].Text;
        }
        catch (Exception ex)
        {
            throw ex;
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
            ShowEdit = false;
        }
    }

    protected void GroupsDataGrid_RadioButton_Checked(object sender, EventArgs e)
    {
      /*  foreach (DataGridItem oldrow in GroupsDataGrid.Items)
        {
            ((RadioButton)oldrow.FindControl("GroupsDataGrid_RadioButton")).Checked = false;
        }*/

        //Set the new selected row
        RadioButton rb = (RadioButton)sender;
        DataGridItem row = (DataGridItem)rb.NamingContainer;
        ((RadioButton)row.FindControl("GroupsDataGrid_RadioButton")).Checked = true;
        Selected_GroupsDataGrid_Index.Value = row.ItemIndex.ToString();
    }

    public void LoadAllGroups()
    {
        GroupsDataGrid.DataSource = createDataSource();
        GroupsDataGrid.DataBind();
    }
}
