using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using MySql.Data;
using MySql.Data.Types;
using MySql.Data.MySqlClient;
using System.Data;
using BLL;

public partial class AdministratorS_CreatingForms : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                LoadOrganizationsList();
                LoadUsersList();
                LoadStringList();
                LoadOrgInfosList();
                LoadVehAndFuelTypeList();
                LoadDeviceList();
                LoadCriteriaList();
                LoadMeasureList();
                LoadReportList();
                LoadReportTypesList();
                Status.Text = "";
            }
            DragPanel.Visible = false;
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        headerUpdatePanel.Update();
    }

    private void LoadUsersList()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];

        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        List<int> usersIds = new List<int>();
        List<UserFromTable> userFromTableList = new List<UserFromTable>();

        dataBlock.usersTable.OpenConnection();
        usersIds = dataBlock.usersTable.Get_AllUsersId();
        foreach (int id in usersIds)
        {
            userFromTableList.Add(new UserFromTable().FillWithInfo(id, dataBlock.usersTable));
        }
        dataBlock.usersTable.CloseConnection();
        Session["CreatingForms_UsersId"] = usersIds;
        UsersDataGrid.DataSource = ReportDataLoader.UsersList(userFromTableList);
        UsersDataGrid.DataBind();
    }

    private void LoadOrganizationsList()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];

        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        List<int> orgIds = new List<int>();
        List<OrganizationFromTable> orgFromTableList = new List<OrganizationFromTable>();

        dataBlock.organizationTable.OpenConnection();
        orgIds = dataBlock.organizationTable.Get_AllOrganizationsId();
        foreach (int id in orgIds)
        {
            orgFromTableList.Add(new OrganizationFromTable().FillWithInfo(id, dataBlock.organizationTable));
        }
        dataBlock.organizationTable.CloseConnection();
        Session["CreatingForms_OrgId"] = orgIds;
        EnterPriseDataGrid.DataSource = ReportDataLoader.OrganizationsList(orgFromTableList);
        EnterPriseDataGrid.DataBind();
    }

    private void LoadStringList()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        string query = "SELECT * FROM fd_string";
        MySqlDataAdapter dAdapter = new MySqlDataAdapter(query, connectionString);
        MySqlCommandBuilder cBuilder = new MySqlCommandBuilder(dAdapter);
        DataTable dTable = new DataTable();
        dAdapter.Fill(dTable);
        FD_stringDataGrid.DataSource = dTable;
        FD_stringDataGrid.DataBind();
    }

    private void LoadOrgInfosList()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        List<KeyValuePair<string, int>> allOrgInfos = new List<KeyValuePair<string,int>>();
        dataBlock.organizationTable.OpenConnection();
        allOrgInfos = dataBlock.organizationTable.GetAllOrgInfos();
        dataBlock.organizationTable.CloseConnection();
        OrgInfosDataGrid.DataSource = ReportDataLoader.OrgInfoDataSorce(allOrgInfos);
        OrgInfosDataGrid.DataBind();
    }

    private void LoadVehAndFuelTypeList()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        //___________________Vehicle Type
        List<KeyValuePair<string, int>> allVehTypes = new List<KeyValuePair<string, int>>();
        dataBlock.vehiclesTables.OpenConnection();
        allVehTypes = dataBlock.vehiclesTables.GetAllVehTypes();

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("VEHICLE_TYPE_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("STRID_VEHICLE_TYPE_NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("FUEL_TYPE", typeof(string)));
        foreach (KeyValuePair<string, int> vehType in allVehTypes)
        {
            dr = dt.NewRow();
            dr["VEHICLE_TYPE_ID"] = vehType.Value.ToString();
            dr["STRID_VEHICLE_TYPE_NAME"] = vehType.Key;
            dr["FUEL_TYPE"] = dataBlock.vehiclesTables.GetVehTypeFuelName(vehType.Value);
            dt.Rows.Add(dr);
        }
        dataBlock.vehiclesTables.CloseConnection();
        VehicleTypesDataGrid.DataSource = dt;
        VehicleTypesDataGrid.DataBind();
        //___________________Fuel
        List<KeyValuePair<string, int>> allFuelTypes = new List<KeyValuePair<string, int>>();
        dataBlock.vehiclesTables.OpenConnection();
        allFuelTypes = dataBlock.vehiclesTables.GetAllFuelTypes();

        dt = new DataTable();
        dt.Columns.Add(new DataColumn("FUEL_TYPE_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("STRID_FUEL_TYPE_NAME", typeof(string)));
        foreach (KeyValuePair<string, int> fuelType in allFuelTypes)
        {
            dr = dt.NewRow();
            dr["FUEL_TYPE_ID"] = fuelType.Value.ToString();
            dr["STRID_FUEL_TYPE_NAME"] = fuelType.Key;
            dt.Rows.Add(dr);
        }
        dataBlock.vehiclesTables.CloseConnection();
        FuelTypeDataGrid.DataSource = dt;
        FuelTypeDataGrid.DataBind();
        LoadVehFuelTypeToDropDownList();
    }

    private void LoadVehFuelTypeToDropDownList()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.vehiclesTables.OpenConnection();
            List<KeyValuePair<string, int>> allFuelTypes = new List<KeyValuePair<string, int>>();
            allFuelTypes = dataBlock.vehiclesTables.GetAllFuelTypes();
            AddNewVehicleTypeDropDownList.Items.Clear();
            foreach (KeyValuePair<string, int> pair in allFuelTypes)
            {
                AddNewVehicleTypeDropDownList.Items.Add(new ListItem(pair.Key, pair.Value.ToString()));
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.vehiclesTables.CloseConnection();
        }
    }

    protected void OrgInfoLinkBtn_Click(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            dataBlock.organizationTable.DeleteOrgInfo(Convert.ToInt32(e.Item.Cells[0].Text));
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            LoadStringList();
            LoadOrgInfosList();
            FdStringUpdatePanel.Update();
        }

    }

    protected void VehTypeDelLinkBtn_Click(object sender, DataGridCommandEventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.vehiclesTables.OpenConnection();
            if (e.CommandName == "Edit")
            {
                NewVehicleTypesPanel.Visible = true;
                EditNewVehicleTypeButton.Visible = true;
                AddNewVehicleTypeButton.Visible = false;
                LoadVehFuelTypeToDropDownList();
               /////////// VehicleTypesDataGrid - ОТСЮДА
                int vehTypeId = Convert.ToInt32(e.Item.Cells[0].Text);
                string thisTypeName = dataBlock.vehiclesTables.GetVehicleTypeName_byVehicleTypeId(vehTypeId);
                int thisfuelTypeId = dataBlock.vehiclesTables.GetVehTypeFuelId(vehTypeId);
                Session["VehicleTypeId"] = vehTypeId;
                NewVehicleTypeTextBox.Text = thisTypeName;
            }
            if (e.CommandName == "Delete")
            {
                dataBlock.vehiclesTables.DeleteVehicleType(Convert.ToInt32(e.Item.Cells[0].Text));                
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.vehiclesTables.CloseConnection();
            LoadStringList();
            LoadVehAndFuelTypeList();
           // VehicleTypesUpdatePanel.Update();
        }
    }

    protected void FuelTypeDelLinkBtn_Click(object sender, DataGridCommandEventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.vehiclesTables.OpenConnection();
            dataBlock.vehiclesTables.DeleteFuelType(Convert.ToInt32(e.Item.Cells[0].Text));
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.vehiclesTables.CloseConnection();
            LoadStringList();
            LoadVehAndFuelTypeList();
            FdStringUpdatePanel.Update();
        }
    }

    protected void AddOrgInfoBtnClick(object sender, EventArgs e)
    {
         string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
         DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.organizationTable.OpenConnection();
            dataBlock.organizationTable.AddOrgInfo(NewOrgInfoName.Text);           
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            AddOrgInfoPanel.Visible = false;
            dataBlock.organizationTable.CloseConnection();
            LoadStringList();
            LoadOrgInfosList();
            FdStringUpdatePanel.Update();
        }
    }

    protected void AddNewVehicleTypeButtonClick(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            string newTypeName = NewVehicleTypeTextBox.Text.Trim();
            int fuelTypeId  = Convert.ToInt32(AddNewVehicleTypeDropDownList.SelectedItem.Value);
            dataBlock.vehiclesTables.OpenConnection();
            dataBlock.vehiclesTables.AddNewVehicleType(newTypeName, fuelTypeId);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            NewVehicleTypesPanel.Visible = false;
            dataBlock.vehiclesTables.CloseConnection();
            LoadStringList();
            LoadVehAndFuelTypeList();
            FdStringUpdatePanel.Update();
        }
    }

    protected void CancelNewVehicleTypeButtonClick(object sender, EventArgs e)
    {
        NewVehicleTypesPanel.Visible = false;
    }

    protected void EditVehicleTypeButtonClick(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.vehiclesTables.OpenConnection();
            dataBlock.vehiclesTables.OpenTransaction();
            string newTypeName = NewVehicleTypeTextBox.Text.Trim();
            int fuelTypeId  = Convert.ToInt32(AddNewVehicleTypeDropDownList.SelectedItem.Value);
            int vehTypeId = (int)Session["VehicleTypeId"];
            dataBlock.vehiclesTables.EditVehicleType(vehTypeId, newTypeName, fuelTypeId);
            dataBlock.vehiclesTables.CommitTransaction();
        }
        catch (Exception ex)
        {
            dataBlock.vehiclesTables.RollbackConnection();
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.vehiclesTables.CloseConnection(); 
            NewVehicleTypesPanel.Visible = false;
            dataBlock.vehiclesTables.CloseConnection();
            LoadStringList();
            LoadVehAndFuelTypeList();
            FdStringUpdatePanel.Update();
        }

    }

    protected void AddFuelTypeButtonClick(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.vehiclesTables.OpenConnection();
            dataBlock.vehiclesTables.AddNewFuelType(AddFuelTypeTextBox.Text.Trim());
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            AddFuelTypePanel.Visible = false;
            dataBlock.vehiclesTables.CloseConnection();
            LoadStringList();
            LoadVehAndFuelTypeList();
            FdStringUpdatePanel.Update();
        }
    }

    protected void CancelFuelTypeButtonClick(object sender, EventArgs e)
    {
        AddFuelTypePanel.Visible = false;
        AddFuelTypeTextBox.Text = "";
    }

    protected void ShowAddOrgInfoBtnClick(object o, EventArgs e)
    {
        AddOrgInfoPanel.Visible = true;
    }

    protected void ShowAddVehicleTypeButton_Click(object sender, EventArgs e)
    {
        LoadVehFuelTypeToDropDownList();
        NewVehicleTypesPanel.Visible = true;
        EditNewVehicleTypeButton.Visible = false;
        AddNewVehicleTypeButton.Visible = true;
        NewVehicleTypeTextBox.Text = "";
    }

    protected void ShowAddFuelTypeButton_Click(object sender, EventArgs e)
    {
        AddFuelTypePanel.Visible = true;
    }

    protected void FD_stringDataGrid_Edit(Object s, DataGridCommandEventArgs e)
    {
        FD_stringDataGrid.EditItemIndex = e.Item.ItemIndex;
        LoadStringList();
    }

    protected void FD_stringDataGrid_Cancel(Object s, DataGridCommandEventArgs e)
    {
        FD_stringDataGrid.EditItemIndex = -1;
        LoadStringList();
    }

    protected void FD_stringDataGrid_Update(Object s, DataGridCommandEventArgs e)
    {
        int gridItemIndex = e.Item.ItemIndex;
        int stringId = Convert.ToInt32(e.Item.Cells[0].Text);
        string dataField = "";
        string cellText = "";
        KeyValuePair<string, string> pair;
        List<KeyValuePair<string, string>> pairList = new List<KeyValuePair<string, string>>();
        int cellNumber = 0;
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            foreach (object dc in FD_stringDataGrid.Columns)
            {
                if (dc is BoundColumn)
                {
                    if (((BoundColumn)dc).Visible && ((BoundColumn)dc).DataField != "STRING_ID")
                    {
                        dataField = ((BoundColumn)dc).DataField;
                        cellText = ((TextBox)e.Item.Cells[cellNumber].Controls[0]).Text;
                        pair = new KeyValuePair<string, string>(dataField, cellText);
                        pairList.Add(pair);
                    }
                }
                cellNumber++;
            }           
            dataBlock.stringTable.OpenConnection();
            foreach (KeyValuePair<string, string> onePair in pairList)
            {
                dataBlock.stringTable.UpdateString(stringId, onePair.Value, onePair.Key);
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.stringTable.CloseConnection();
            LoadOrgInfosList();
            LoadVehAndFuelTypeList();
            VehicleTypesUpdatePanel.Update();
            OrgInfoUpdatePanel.Update();
            FD_stringDataGrid_Cancel(s, e);

        }

    }

    protected void AddEnterpriseBtnClick(object sender, EventArgs e)
    {
        try
        {
            DragPanelOrg.Visible = true;
            Session["AddingNewOrg"] = true;
            GeneralTab_CreatingFormsControl1.LoadOrgInfo(-1);
            //OrgUpdatePanel.Update();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }

    protected void EditOrgLinkBtn_Click(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Edit")
            {
                List<int> orgIds = new List<int>();

                orgIds = (List<int>)Session["CreatingForms_OrgId"];
                DragPanelOrg.Visible = true;
                Session["AddingNewOrg"] = false;
                GeneralTab_CreatingFormsControl1.LoadOrgInfo(orgIds[e.Item.ItemIndex]);
            }
            if (e.CommandName == "Delete")
            {
                string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
                DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
                List<int> orgIds = new List<int>();
                orgIds = (List<int>)Session["CreatingForms_OrgId"];
                dataBlock.organizationTable.DeleteOrganization(orgIds[e.Item.ItemIndex]); //Удаление организации
                LoadOrganizationsList();
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }

    protected void EditUserLinkBtn_Click(object sender, DataGridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Edit")
            {
                List<int> usersIds = new List<int>();

                usersIds = (List<int>)Session["CreatingForms_UsersId"];
                DragPanel.Visible = true;
                Session["AddingNewUser"] = false;
                UserEditControl1.LoadUserInfo(usersIds[e.Item.ItemIndex]);
            }
            if (e.CommandName == "Delete")
            {
                string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
                DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
                List<int> usersIds = new List<int>();
                usersIds = (List<int>)Session["CreatingForms_UsersId"];
                dataBlock.usersTable.DeleteUser(usersIds[e.Item.ItemIndex]);
                LoadUsersList();
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }

    protected void AddNewUser(object sender, EventArgs e)
    {
        try
        {
            DragPanel.Visible = true;
            UserEditControl1.LoadUserInfo(-1);
            Session["AddingNewUser"] = true;
            UsersUpdatePanel1.Update();            
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }
    ///////Devices
    private void LoadDeviceList()
    {
        string CurrentLanguage = "STRING_EN";
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, CurrentLanguage);
        List<KeyValuePair<string, int>> allDeviceTypes = new List<KeyValuePair<string, int>>();
        allDeviceTypes = dataBlock.deviceTable.GetAllDeviceTypes();

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("DEVICE_TYPE_ID", typeof(int)));
        dt.Columns.Add(new DataColumn("STRID_DEVICE_TYPE_NAME", typeof(string)));

        foreach (KeyValuePair<string, int> deviceType in allDeviceTypes)
        {
            dr = dt.NewRow();
            dr["DEVICE_TYPE_ID"] = deviceType.Value;
            dr["STRID_DEVICE_TYPE_NAME"] = deviceType.Key;
            dt.Rows.Add(dr);
        }
        DeviceTypeDataGrid.DataSource = dt;
        DeviceTypeDataGrid.DataBind();
    }
    protected void ShowAddDeviceTypeButton_Click(object sender, EventArgs e)
    {
        AddDeviceTypePanel.Visible = true;
    }
    protected void CancelDeviceTypeButtonClick(object sender, EventArgs e)
    {
        AddDeviceTypePanel.Visible = false;
    }
    protected void AddDeviceTypeButtonClick(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            string deviceTypeName = AddDeviceTypeTextBox.Text;
            dataBlock.deviceTable.AddNewDeviceType(deviceTypeName);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            AddDeviceTypePanel.Visible = false;
            LoadStringList();
            LoadDeviceList();
            FdStringUpdatePanel.Update();
            VehicleTypesUpdatePanel.Update();
        }
    }
    protected void DeviceTypeDelLinkBtn_Click(object s, DataGridCommandEventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            if (e.CommandName == "Delete")
            {
                dataBlock.deviceTable.DeleteDeviceType(Convert.ToInt32(e.Item.Cells[0].Text));
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            LoadStringList();
            LoadDeviceList();
            FdStringUpdatePanel.Update();
        }
    }
    ///////Criteria
    private void LoadCriteriaList()
    {
        string CurrentLanguage = "STRING_EN";
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
        CriteriaListDataGrid.DataSource = dt;
        CriteriaListDataGrid.DataBind();
    }
    private void LoadMeasuresToDropDownList()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        dataBlock.criteriaTable.OpenConnection();
        List<int> allMeasures = new List<int>();
        allMeasures = dataBlock.criteriaTable.GetAllMeasuresIds();
        CriteriaMeasureDropDownList.Items.Clear();
        string measureName;
        foreach (int id in allMeasures)
        {
            measureName = dataBlock.criteriaTable.GetMeasureFullName(id) + " - " + dataBlock.criteriaTable.GetMeasureShortName(id);
            CriteriaMeasureDropDownList.Items.Add(new ListItem(measureName, id.ToString()));
        }
        dataBlock.criteriaTable.CloseConnection();
    }
    protected void ShowAddCriteriaButton_Click(object sender, EventArgs e)
    {
        try
        {
            EditNewVehicleTypeButton.Visible = false;
            AddCriteriaButton.Visible = true;
            EditCriteriaButton.Visible = false;
            LoadMeasuresToDropDownList();
            AddNewCriteriaPanel.Visible = true;
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }
    protected void CancelCriteriaButtonClick(object sender, EventArgs e)
    {
        AddNewCriteriaPanel.Visible = false;
    }
    protected void AddCriteriaButtonClick(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            CriteriaTable key = new CriteriaTable();
            key.CriteriaName = CriteriaNameTextBox.Text;
            key.CriteriaNote = CriteriaCommentTextBox.Text;
            key.MeasureId = Convert.ToInt32(CriteriaMeasureDropDownList.SelectedValue);
            key.MinValue = Convert.ToInt32(CriteriaMinValueTextBox.Text);
            key.MaxValue = Convert.ToInt32(CriteriaMaxValueTextBox.Text);

            dataBlock.criteriaTable.AddNewCriteria(key.MeasureId, key.CriteriaName, key.CriteriaNote, key.MinValue, key.MaxValue);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            AddNewCriteriaPanel.Visible = false;
            LoadStringList();
            LoadCriteriaList();
            FdStringUpdatePanel.Update();
            CriteriaListUpdatePanel.Update();
        }
    }
    protected void EditCriteriaButtonClick(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.criteriaTable.OpenConnection();
            CriteriaTable key = new CriteriaTable();
            key.CriteriaName = CriteriaNameTextBox.Text.Trim();
            key.CriteriaNote = CriteriaCommentTextBox.Text.Trim();
            key.MeasureId = Convert.ToInt32(CriteriaMeasureDropDownList.SelectedValue);
            key.MinValue = Convert.ToInt32(CriteriaMinValueTextBox.Text.Trim());
            key.MaxValue = Convert.ToInt32(CriteriaMaxValueTextBox.Text.Trim());
            key.KeyId = (int)Session["CriteriaId"];

            dataBlock.criteriaTable.EditCriteria(key.KeyId, key.MeasureId, key.CriteriaName, key.CriteriaNote, key.MinValue, key.MaxValue);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            AddNewCriteriaPanel.Visible = false;
            dataBlock.criteriaTable.CloseConnection();
            LoadStringList();
            LoadCriteriaList();
            FdStringUpdatePanel.Update();
        }

    }
    protected void CriteriaListDelLinkBtn_Click(object s, DataGridCommandEventArgs e)
    {
        string CurrentLanguage = "STRING_EN";
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.criteriaTable.OpenConnection();
            if (e.CommandName == "Edit")
            {
                AddNewCriteriaPanel.Visible = true;
                EditCriteriaButton.Visible = true;
                AddCriteriaButton.Visible = false;
                LoadMeasuresToDropDownList();
                CriteriaTable key = new CriteriaTable(connectionString, CurrentLanguage, dataBlock.sqlDb);                
                int criteriaId = Convert.ToInt32(e.Item.Cells[0].Text);
                key = dataBlock.criteriaTable.LoadCriteria(criteriaId);
                
                CriteriaNameTextBox.Text = key.CriteriaName;
                CriteriaCommentTextBox.Text = key.CriteriaNote;
                CriteriaMinValueTextBox.Text = key.MinValue.ToString();
                CriteriaMaxValueTextBox.Text = key.MaxValue.ToString();
                Session["CriteriaId"] = criteriaId;
            }
            if (e.CommandName == "Delete")
            {
                dataBlock.criteriaTable.DeleteCriteria(Convert.ToInt32(e.Item.Cells[0].Text));
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.vehiclesTables.CloseConnection();
            LoadStringList();
            LoadCriteriaList();
            // VehicleTypesUpdatePanel.Update();
        }
    }
    ///////Measure
    private void LoadMeasureList()
    {
        string CurrentLanguage = "STRING_EN";
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, CurrentLanguage);
        List<int> allKeys = new List<int>();

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("MEASURE_ID", typeof(int)));
        dt.Columns.Add(new DataColumn("MEASURE_NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("MEASURE_FULL_NAME", typeof(string)));

        dataBlock.criteriaTable.OpenConnection();
        allKeys = dataBlock.criteriaTable.GetAllMeasuresIds();

        foreach (int id in allKeys)
        {
            dr = dt.NewRow();
            dr["MEASURE_ID"] = id;
            dr["MEASURE_NAME"] = dataBlock.criteriaTable.GetMeasureShortName(id);
            dr["MEASURE_FULL_NAME"] = dataBlock.criteriaTable.GetMeasureFullName(id);
            dt.Rows.Add(dr);
        }
        dataBlock.criteriaTable.CloseConnection();
        MeasureListDataGrid.DataSource = dt;
        MeasureListDataGrid.DataBind();
    }
    protected void ShowAddMeasureButton_Click(object sender, EventArgs e)
    {
        AddNewMeasurePanel.Visible = true;
    }
    protected void CancelMeasureButtonClick(object sender, EventArgs e)
    {
        AddNewMeasurePanel.Visible = false;
    }
    protected void AddMeasureButtonClick(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            string shortName = MeasureShortNameTextBox.Text;
            string fullName = MeasureFullNameTextBox.Text;
            dataBlock.criteriaTable.AddNewMeasure(shortName, fullName);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            AddNewMeasurePanel.Visible = false;
            LoadStringList();
            LoadMeasureList();
            FdStringUpdatePanel.Update();
            CriteriaListUpdatePanel.Update();
        }
    }
    protected void MeasureListDelLinkBtn_Click(object s, DataGridCommandEventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.criteriaTable.DeleteMeasure(Convert.ToInt32(e.Item.Cells[0].Text));
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            LoadStringList();
            LoadMeasureList();
            FdStringUpdatePanel.Update();
            CriteriaListUpdatePanel.Update();
        }
    }
    protected void FD_MeasureDataGrid_Edit(Object s, DataGridCommandEventArgs e)
    {
        MeasureListDataGrid.EditItemIndex = e.Item.ItemIndex;
        LoadMeasureList();
    }
    protected void FD_MeasureDataGrid_Cancel(Object s, DataGridCommandEventArgs e)
    {
        MeasureListDataGrid.EditItemIndex = -1;
        LoadMeasureList();
    }
    protected void FD_MeasureDataGrid_Update(Object s, DataGridCommandEventArgs e)
    {
        int gridItemIndex = e.Item.ItemIndex;
        int measureId = Convert.ToInt32(e.Item.Cells[0].Text);
        string dataField = "";
        string cellText = "";
        KeyValuePair<string, string> pair;
        List<KeyValuePair<string, string>> pairList = new List<KeyValuePair<string, string>>();
        int cellNumber = 0;
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            foreach (object dc in MeasureListDataGrid.Columns)
            {
                if (dc is BoundColumn)
                {
                    if (((BoundColumn)dc).Visible && ((BoundColumn)dc).DataField != "MEASURE_ID")
                    {
                        dataField = ((BoundColumn)dc).DataField;
                        cellText = ((TextBox)e.Item.Cells[cellNumber].Controls[0]).Text;
                        pair = new KeyValuePair<string, string>(dataField, cellText);
                        pairList.Add(pair);
                    }
                }
                cellNumber++;
            }           
            dataBlock.criteriaTable.OpenConnection();
            dataBlock.criteriaTable.EditMeasure(measureId, pairList[0].Value, pairList[1].Value);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.criteriaTable.CloseConnection();
            LoadMeasureList();
            LoadCriteriaList();
            CriteriaListUpdatePanel.Update();
            FD_MeasureDataGrid_Cancel(s, e);
            LoadStringList();
            FdStringUpdatePanel.Update();
        }
    }
    ///////Reports
    private void LoadReportList()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        List<int> reportsIds = new List<int>();

        dataBlock.reportsTable.OpenConnection();
        reportsIds = dataBlock.reportsTable.GetAllReportsIds();
        dataBlock.reportsTable.CloseConnection();

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("CODE", typeof(string)));
        dt.Columns.Add(new DataColumn("TYPE", typeof(string)));
        dt.Columns.Add(new DataColumn("NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("PRICE", typeof(string)));
        dt.Columns.Add(new DataColumn("CREATE_DATE", typeof(string)));
        dt.Columns.Add(new DataColumn("UPDATE_DATE", typeof(string)));
        dt.Columns.Add(new DataColumn("NOTE", typeof(string)));

        DateTime date = new DateTime();
        dataBlock.reportsTable.OpenConnection();
        foreach (int reportId in reportsIds)
        {
            dr = dt.NewRow();
            dr["CODE"] = reportId.ToString();
            dr["TYPE"] = dataBlock.reportsTable.GetReportTypeName(reportId, 0);
            dr["NAME"] = dataBlock.reportsTable.GetReportName(reportId);
            dr["PRICE"] = dataBlock.reportsTable.GetReportPrice(reportId);
            dr["CREATE_DATE"] = dataBlock.reportsTable.GetReportDateCreate(reportId).ToShortDateString();
            dr["UPDATE_DATE"] = dataBlock.reportsTable.GetReportDateUpdate(reportId).ToShortDateString();
            dr["NOTE"] = dataBlock.reportsTable.GetUserReport_Note(reportId);
            dt.Rows.Add(dr);
        }

        ReportListDataGrid.DataSource = dt;
        ReportListDataGrid.DataBind();
        dataBlock.reportsTable.CloseConnection();
    }
    private void LoadAllReportsTypesToDropDowns()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        List<KeyValuePair<string, int>> allReportsTypes = new List<KeyValuePair<string, int>>();
        
        dataBlock.reportsTable.OpenConnection();
        allReportsTypes = dataBlock.reportsTable.GetAllReportTypes();
        ReportTypeDropDown.Items.Clear();
        foreach (KeyValuePair<string, int> pair in allReportsTypes)
        {
            ReportTypeDropDown.Items.Add(new ListItem(pair.Key, pair.Value.ToString()));
        }
        dataBlock.reportsTable.CloseConnection();
    }
    protected void ShowAddReportButton_Click(object sender, EventArgs e)
    {
        try
        {
            AddNewReportPanel.Visible = true;
            LoadAllReportsTypesToDropDowns();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }
    protected void CancelReportButtonClick(object sender, EventArgs e)
    {
        AddNewReportPanel.Visible = false;
    }
    protected void AddReportButtonClick(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int reportTypeId = Convert.ToInt32(ReportTypeDropDown.SelectedValue);
            string reportName = ReportNameTextBox.Text;
            int reportPrice = Convert.ToInt32(ReportPriceTextBox.Text);
            string Note = ReportNoteTextBox.Text;

            dataBlock.reportsTable.OpenConnection();
            dataBlock.reportsTable.OpenTransaction();
            dataBlock.reportsTable.AddUserReport(reportTypeId, reportName, reportPrice, Note);
            dataBlock.reportsTable.CommitTransaction();
        }
        catch (Exception ex)
        {
            dataBlock.reportsTable.RollbackConnection();
            Status.Text = ex.Message;
            headerUpdatePanel.Update();
        }
        finally
        {
            dataBlock.reportsTable.CloseConnection();
            AddNewReportPanel.Visible = false;
            LoadStringList();
            LoadReportList();
            FdStringUpdatePanel.Update();
            ReportTypeListUpdatePanel.Update();
        }
    }
    protected void ReportListDelLinkBtn_Click(object s, DataGridCommandEventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.criteriaTable.DeleteMeasure(Convert.ToInt32(e.Item.Cells[0].Text));
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            LoadStringList();
            LoadMeasureList();
            FdStringUpdatePanel.Update();
            CriteriaListUpdatePanel.Update();
        }
    }
    protected void FD_ReportDataGrid_Edit(Object s, DataGridCommandEventArgs e)
    {
        ReportListDataGrid.EditItemIndex = e.Item.ItemIndex;
        LoadReportList();
    }
    protected void FD_ReportDataGrid_Cancel(Object s, DataGridCommandEventArgs e)
    {
        ReportListDataGrid.EditItemIndex = -1;
        LoadReportList();
    }
    protected void FD_ReportDataGrid_Update(Object s, DataGridCommandEventArgs e)
    {
        int gridItemIndex = e.Item.ItemIndex;
        int measureId = Convert.ToInt32(e.Item.Cells[0].Text);
        string dataField = "";
        string cellText = "";
        KeyValuePair<string, string> pair;
        List<KeyValuePair<string, string>> pairList = new List<KeyValuePair<string, string>>();
        int cellNumber = 0;
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            foreach (object dc in MeasureListDataGrid.Columns)
            {
                if (dc is BoundColumn)
                {
                    if (((BoundColumn)dc).Visible && ((BoundColumn)dc).DataField != "MEASURE_ID")
                    {
                        dataField = ((BoundColumn)dc).DataField;
                        cellText = ((TextBox)e.Item.Cells[cellNumber].Controls[0]).Text;
                        pair = new KeyValuePair<string, string>(dataField, cellText);
                        pairList.Add(pair);
                    }
                }
                cellNumber++;
            }
            dataBlock.criteriaTable.OpenConnection();
            dataBlock.criteriaTable.EditMeasure(measureId, pairList[0].Value, pairList[1].Value);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.criteriaTable.CloseConnection();
            LoadMeasureList();
            LoadCriteriaList();
            CriteriaListUpdatePanel.Update();
            FD_MeasureDataGrid_Cancel(s, e);
            LoadStringList();
            FdStringUpdatePanel.Update();
        }
    }
    //ReportsTypes
    private void LoadReportTypesList()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        List<KeyValuePair<string, int>> reportTypesIds = new List<KeyValuePair<string,int>>();

        dataBlock.reportsTable.OpenConnection();
        reportTypesIds = dataBlock.reportsTable.GetAllReportTypes();
        dataBlock.reportsTable.CloseConnection();

        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("CODE", typeof(string)));
        dt.Columns.Add(new DataColumn("NAME", typeof(string)));
        dt.Columns.Add(new DataColumn("SHORTNAME", typeof(string)));
        dt.Columns.Add(new DataColumn("FULLNAME", typeof(string)));
        dt.Columns.Add(new DataColumn("PRINTNAME", typeof(string)));

        DateTime date = new DateTime();
        dataBlock.reportsTable.OpenConnection();
        foreach (KeyValuePair<string, int> reportId in reportTypesIds)
        {
            dr = dt.NewRow();
            dr["CODE"] = reportId.Value.ToString();
            dr["NAME"] = reportId.Key;
            dr["SHORTNAME"] = dataBlock.reportsTable.GetReportType_ReportName(reportId.Value, dataBlock.reportsTable.STRID_REPORT_SHORT_NAME_Ident);
            dr["FULLNAME"] = dataBlock.reportsTable.GetReportType_ReportName(reportId.Value, dataBlock.reportsTable.STRID_REPORT_FULL_NAME_Ident);
            dr["PRINTNAME"] = dataBlock.reportsTable.GetReportType_ReportName(reportId.Value, dataBlock.reportsTable.STRID_REPORT_PRINT_NAME_Ident);
            dt.Rows.Add(dr);
        }

        ReportTypeListDataGrid.DataSource = dt;
        ReportTypeListDataGrid.DataBind();
        dataBlock.reportsTable.CloseConnection();
    }
    protected void ShowAddReportTypeButton_Click(object sender, EventArgs e)
    {
        try
        {
            AddNewReportTypePanel.Visible = true;
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }
    protected void CancelReportTypeButtonClick(object sender, EventArgs e)
    {
        AddNewReportTypePanel.Visible = false;
    }
    protected void AddReportTypeButtonClick(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            string name = ReportTypeNameTextBox.Text;
            string shortName = ReportTypeShortNameTextBox.Text;
            string fullName = ReportTypeFullNameTextBox.Text;
            string printName = ReportTypePrintNameTextBox.Text;
            dataBlock.reportsTable.OpenConnection();
            dataBlock.reportsTable.OpenTransaction();
            dataBlock.reportsTable.AddReportType(name, shortName, fullName, printName);
            dataBlock.reportsTable.CommitTransaction();
        }
        catch (Exception ex)
        {
            dataBlock.reportsTable.RollbackConnection();
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.reportsTable.CloseConnection();
            AddNewReportTypePanel.Visible = false;
            LoadStringList();
            LoadReportTypesList();
            FdStringUpdatePanel.Update();
            ReportTypeListUpdatePanel.Update();
        }
    }
    protected void ReportTypeListDelLinkBtn_Click(object s, DataGridCommandEventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.criteriaTable.DeleteMeasure(Convert.ToInt32(e.Item.Cells[0].Text));
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        finally
        {
            LoadStringList();
            LoadMeasureList();
            FdStringUpdatePanel.Update();
            CriteriaListUpdatePanel.Update();
        }
    }
    protected void FD_ReportTypeDataGrid_Edit(Object s, DataGridCommandEventArgs e)
    {
        ReportTypeListDataGrid.EditItemIndex = e.Item.ItemIndex;
        LoadReportTypesList();
    }
    protected void FD_ReportTypeDataGrid_Cancel(Object s, DataGridCommandEventArgs e)
    {
        ReportTypeListDataGrid.EditItemIndex = -1;
        LoadReportTypesList();
    }
    protected void FD_ReportTypeDataGrid_Update(Object s, DataGridCommandEventArgs e)
    {
        int gridItemIndex = e.Item.ItemIndex;
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        int reportTypeId = Convert.ToInt32(e.Item.Cells[0].Text);
        string name = ((TextBox)e.Item.Cells[1].Controls[0]).Text;
        string shortName = ((TextBox)e.Item.Cells[2].Controls[0]).Text;
        string fullName = ((TextBox)e.Item.Cells[3].Controls[0]).Text;
        string printName = ((TextBox)e.Item.Cells[4].Controls[0]).Text;

        try
        {
            dataBlock.reportsTable.OpenConnection();
            dataBlock.reportsTable.OpenTransaction();
            dataBlock.reportsTable.EditReportType(reportTypeId, name, shortName, fullName, printName);
            dataBlock.reportsTable.CommitTransaction();
        }
        catch (Exception ex)
        {
            dataBlock.reportsTable.RollbackConnection();
            Status.Text = ex.Message;
        }
        finally
        {
            dataBlock.reportsTable.CloseConnection();
            LoadReportTypesList();
            LoadReportList();
            ReportTypeListUpdatePanel.Update();
            FD_ReportTypeDataGrid_Cancel(s, e);
            LoadStringList();
            FdStringUpdatePanel.Update();
        }
    }

    protected override bool OnBubbleEvent(object sender, EventArgs e)
    {
        try
        {
            bool CreateOrgVisible = (bool)Session["CreateOrgVisible"];

            LoadUsersList();
            LoadOrganizationsList();
            Status.Text = Session["UserEditControl_BubbleException"].ToString();
            DragPanelOrg.Visible = CreateOrgVisible;
            //DragPanel.Visible = false;
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            //headerUpdatePanel.Update();
        }
        return true;
    }

}
