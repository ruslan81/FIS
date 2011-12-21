using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using System.Configuration;

public partial class Administrator_Settings_UserControls_UserVehicleTab : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //LoadVehiclesDataGrid();
        }
    }

    private DataTable createDataSource()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        DataTable newDataTable = new DataTable(Guid.NewGuid().ToString());
        try
        {
            DataRow dr;
            string Col_1 = "#";
            string Col_2 = "Номер ТС";
            string Col_3 = "VIN";
            string Col_4 = "Комментарий";

            newDataTable.Columns.Add(new DataColumn(Col_1, typeof(int)));
            newDataTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
            newDataTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
            newDataTable.Columns.Add(new DataColumn(Col_4, typeof(string)));

            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            dataBlock.OpenConnection();
            List<int> cardsList = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.vehicleCardTypeId);

            int vehicleId;
            int vehicleKeyId;
            for (int i = 0; i < cardsList.Count; i++)
            {
                vehicleId = dataBlock.vehiclesTables.GetVehicle_byCardId(cardsList[i]);
                dr = newDataTable.NewRow();
                dr[Col_1] = vehicleId;
                dr[Col_2] = dataBlock.vehiclesTables.GetVehicleGOSNUM(vehicleId);
                dr[Col_3] = dataBlock.vehiclesTables.GetVehicleVin(vehicleId);
                vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId("Commentary to a vehicle", vehicleId);
                dr[Col_4] = dataBlock.cardsTable.GetCardNote(cardsList[i]);
                newDataTable.Rows.Add(dr);
            }
        }
        catch (Exception ex)
        {
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
        finally
        {
            dataBlock.CloseConnection();

        }
        return newDataTable;
    }

    public void LoadVehiclesDataGrid()
    {
        VehiclesDataGrid.DataSource = createDataSource();
        VehiclesDataGrid.DataBind();
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
            VehiclesDataGridPanel.Visible = !value;
        }
    }

    public bool ShowAddNew
    {
        get
        {
            return EditPanel.Visible;
        }
        set
        {
            if (value == true)
                Load_NewInfo();
            EditPanel.Visible = value;
            VehiclesDataGridPanel.Visible = !value;
        }
    }

    private void Load_EditInfo()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            string selectedIndexString = Selected_VehiclesDataGrid_Index.Value;
            if (selectedIndexString == "")
                throw new Exception("Выберите Транспортное средство для редактирования");
            int selectedVehID = Convert.ToInt32(selectedIndexString);
            int vehicleKeyId;
            int keyId;
            int measureId;
            dataBlock.OpenConnection();
            #region "GeneralTab"
            //RegNumb
            GeneralVehInfo_RegNumbLabel.Text = "Registration number:";
            GeneralVehInfo_RegNumbTextBox.Text = dataBlock.vehiclesTables.GetVehicleGOSNUM(selectedVehID);
            //VIN
            GeneralVehInfo_VinLabel.Text = "Vehicle Identification Number(VIN):";
            GeneralVehInfo_VinTextBox.Text = dataBlock.vehiclesTables.GetVehicleVin(selectedVehID);
            //Date blocked
            GeneralVehInfo_DateBlocked_Label.Text = "Date blocked:";
            GeneralVehInfo_DateBlocked_TextBox.Text = dataBlock.vehiclesTables.GetVehicleDateBlocked(selectedVehID).ToLongDateString();
            //Priority
            GeneralVehInfo_Priority_Label.Text = "Priority:";
            GeneralVehInfo_Priority_TextBox.Text = dataBlock.vehiclesTables.GetVehiclePriority(selectedVehID).ToString();
            //Manufacturer
            GeneralVehInfo_Manufacturer_Label.Text = "Manufacturer:";
            GeneralVehInfo_Manufacturer_TextBox.Text = dataBlock.vehiclesTables.GetVehicleMARKA(selectedVehID);
            //Commentary         
            int cardId = dataBlock.vehiclesTables.GetCardId(selectedVehID);
            GeneralVehInfo_Comment_Label.Text = "Commentary:";
            GeneralVehInfo_Comment_TextBox.Text = dataBlock.cardsTable.GetCardNote(cardId);
            //PHOTO
            int vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.VehiclePhotoAddress);
            string filePath = dataBlock.vehiclesTables.GetVehicleInfoValue(selectedVehID, vehInfoId);
            if (filePath == "")
                filePath = "~/images/unknown_vehicle.jpg";
            VehiclesPhotoImage.ImageUrl = filePath;
            #endregion
            #region "AdditionalTab"
            //Fuel tank 1
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.FuelTank1, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            AdditionalEdit_Bak_1_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            AdditionalEdit_Bak_1_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            AdditionalEdit_Bak_1_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            AdditionalEdit_Bak_1_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //Fuel tank 2
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.FuelTank2, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            AdditionalEdit_Bak_2_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            AdditionalEdit_Bak_2_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            AdditionalEdit_Bak_2_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            AdditionalEdit_Bak_2_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //LoadCcarrying
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.LoadCarryingCapacity, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            AdditionalEdit_LoadCcarrying_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            AdditionalEdit_LoadCcarrying_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            AdditionalEdit_LoadCcarrying_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            AdditionalEdit_LoadCcarrying_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //VehicleType - FuelType
            keyId = dataBlock.vehiclesTables.GetVehicleTypeId(selectedVehID);
            //AdditionalEdit_FuelType_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);//Переводить строку.
            ///AdditionalEdit_VehicleType_Label.Text // "Эту тоже переводить потом.
            List<KeyValuePair<string, int>> vehTypes = dataBlock.vehiclesTables.GetAllVehTypes();
            AdditionalEdit_VehicleType_DropDown.Items.Clear();
            foreach (KeyValuePair<string, int> pair in vehTypes)
            {
                AdditionalEdit_VehicleType_DropDown.Items.Add(new ListItem(pair.Key, pair.Value.ToString()));
                if (pair.Value == keyId)
                    AdditionalEdit_VehicleType_DropDown.SelectedIndex = AdditionalEdit_VehicleType_DropDown.Items.Count - 1;
            }
            AdditionalEdit_FuelType_DropDown.Text = dataBlock.vehiclesTables.GetVehTypeFuelName(keyId);
            //TO1
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.MRO1, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            AdditionalEdit_TO1_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            AdditionalEdit_TO1_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            AdditionalEdit_TO1_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            AdditionalEdit_TO1_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //TO2
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.MRO2, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            AdditionalEdit_TO2_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            AdditionalEdit_TO2_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            AdditionalEdit_TO2_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            AdditionalEdit_TO2_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            #endregion
            #region "HardwareTab"
            int deviceId = dataBlock.vehiclesTables.GetVehicleDeviceId(selectedVehID);
            int deviceTypeId = dataBlock.deviceTable.GetDeviceType(deviceId);

            HardwareEdit_DeviceTypeLabel.Text = "Device type:";
            HardwareEdit_DeviceTypeTextBox.Text = dataBlock.deviceTable.GetDeviceTypeName(deviceTypeId);

            HardwareEdit_DeviceNameLabel.Text = "Device name:";
            HardwareEdit_DeviceNameTextBox.Text = dataBlock.deviceTable.GetDeviceName(deviceId);

            HardwareEdit_DeviceNumberLabel.Text = "Device number:";
            HardwareEdit_DeviceNumberTextBox.Text = dataBlock.deviceTable.GetDeviceNum(deviceId);

            HardwareEdit_DeviceProductionDateLabel.Text = "Device production date:";
            HardwareEdit_DeviceProductionDateTextBox.Text = dataBlock.deviceTable.GetDeviceDateProduction(deviceId).ToLongDateString();

            int firmwareId = dataBlock.deviceTable.GetDeviceFirmwareId(deviceId);
            HardwareEdit_DeviceFirmwareVersionLabel.Text = "Firmaware version:";
            HardwareEdit_DeviceFirmwareVersionTextBox.Text = dataBlock.deviceTable.GetDeviceFirmware_version(firmwareId);

            HardwareEdit_DevicePhoneSimNumberLabel.Text = "Телефонный номер SIM";
            HardwareEdit_DevicePhoneSimNumberTextBox.Text = dataBlock.deviceTable.GetDevicePhoneNumSim(deviceId).ToString();

            int lastVehicleDataBlockId = dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId).Last();
            DDDClass.VuCalibrationRecord calibration = dataBlock.vehicleUnitInfo.Get_VehicleTechnicalData_VuCalibrationData(lastVehicleDataBlockId).Last();

            HardwareEdit_LastCalibPurposeLabel.Text = "Last calibration purpose:";
            HardwareEdit_LastCalibPurposeTextBox.Text = calibration.calibrationPurpose.ToString();

            HardwareEdit_WhoCalibLabel.Text = "Workshop:";
            HardwareEdit_WhoCalibTextBox.Text = calibration.workshopName.ToString();

            HardwareEdit_WhoCalibCardNumberLabel.Text = "Workshop card number:";
            HardwareEdit_WhoCalibCardNumberTextBox.Text = calibration.workshopCardNumber.cardNumber.ToString();

            HardwareEdit_NextCalibDateLabel.Text = "Next calibration date:";
            HardwareEdit_NextCalibDateTextBox.Text = calibration.nextCalibrationDate.getTimeRealDate().ToShortDateString();
            #endregion
            #region "Koef"
            //	Nominal turns
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.NominalTurns, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_NomRPM_Lable.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_NomRPM_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            KoefEditTable_NomRPM_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            KoefEditTable_NomRPM_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //	Maximum speed
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.MaxSpeed, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_MaxSpeed_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_MaxSpeed_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            KoefEditTable_MaxSpeed_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            KoefEditTable_MaxSpeed_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //	Manoeuvring	
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.Manoeuvring, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_Manevr_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_Manevr_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            KoefEditTable_Manevr_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            KoefEditTable_Manevr_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //	City	
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.City, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_City_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_City_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            KoefEditTable_City_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            KoefEditTable_City_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //	HighWay
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.Highway, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_Magistral_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_Magistral_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            KoefEditTable_Magistral_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            KoefEditTable_Magistral_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //  Nominal fuel consumption
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.NomFuelConsumption, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_NomFuelConsumpion_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_NomFuelConsumpion_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            KoefEditTable_NomFuelConsumpion_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            KoefEditTable_NomFuelConsumpion_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //  Cold start
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.ColdStart, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_ColdStart_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_ColdStart_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            KoefEditTable_ColdStart_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            KoefEditTable_ColdStart_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            // 	Hot stop
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.HotStop, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_HotStop_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_HotStop_MinValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MinVal(vehicleKeyId).ToString();
            KoefEditTable_HotStop_MaxValTextBox.Text = dataBlock.vehiclesTables.GetVehicleKey_MaxVal(vehicleKeyId).ToString();
            KoefEditTable_HotStop_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            #endregion

            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.vehiclesTables.CloseConnection();
            dataBlock.criteriaTable.CloseConnection();
            dataBlock.deviceTable.CloseConnection();
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
    }

    private void Load_NewInfo()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            dataBlock.OpenConnection();
            int selectedVehID = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.orgInitCardTypeId)[0];
            int vehicleKeyId;
            int keyId;
            int measureId;
            dataBlock.vehiclesTables.OpenConnection();
            dataBlock.criteriaTable.OpenConnection();
            dataBlock.deviceTable.OpenConnection();
            #region "GeneralTab"
            //RegNumb
            GeneralVehInfo_RegNumbLabel.Text = "Registration number:";
            GeneralVehInfo_RegNumbTextBox.Text = "";
            //VIN
            GeneralVehInfo_VinLabel.Text = "Vehicle Identification Number(VIN):";
            GeneralVehInfo_VinTextBox.Text = "";
            //Date blocked
            GeneralVehInfo_DateBlocked_Label.Text = "Date blocked:";
            GeneralVehInfo_DateBlocked_TextBox.Text = "";
            //Priority
            GeneralVehInfo_Priority_Label.Text = "Priority:";
            GeneralVehInfo_Priority_TextBox.Text = "";
            //Manufacturer
            GeneralVehInfo_Manufacturer_Label.Text = "Manufacturer:";
            GeneralVehInfo_Manufacturer_TextBox.Text = "";
            //Commentary         
            GeneralVehInfo_Comment_Label.Text = "Commentary:";
            GeneralVehInfo_Comment_TextBox.Text = "";
            #endregion
            #region "AdditionalTab"
            //Fuel tank 1
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.FuelTank1, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            AdditionalEdit_Bak_1_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            AdditionalEdit_Bak_1_MinValTextBox.Text = "";
            AdditionalEdit_Bak_1_MaxValTextBox.Text = "";
            AdditionalEdit_Bak_1_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //Fuel tank 2
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.FuelTank2, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            AdditionalEdit_Bak_2_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            AdditionalEdit_Bak_2_MinValTextBox.Text = "";
            AdditionalEdit_Bak_2_MaxValTextBox.Text = "";
            AdditionalEdit_Bak_2_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //LoadCcarrying
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.LoadCarryingCapacity, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            AdditionalEdit_LoadCcarrying_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            AdditionalEdit_LoadCcarrying_MinValTextBox.Text = "";
            AdditionalEdit_LoadCcarrying_MaxValTextBox.Text = "";
            AdditionalEdit_LoadCcarrying_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //VehicleType - FuelType
            //AdditionalEdit_FuelType_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);//Переводить строку.
            ///AdditionalEdit_VehicleType_Label.Text // "Эту тоже переводить потом.
            List<KeyValuePair<string, int>> vehTypes = dataBlock.vehiclesTables.GetAllVehTypes();
            AdditionalEdit_VehicleType_DropDown.Items.Clear();
            int vehTypesCounter = 0;
            foreach (KeyValuePair<string, int> pair in vehTypes)
            {
                AdditionalEdit_VehicleType_DropDown.Items.Add(new ListItem(pair.Key, pair.Value.ToString()));
                vehTypesCounter++;
            }
            if (vehTypesCounter > 0)
                AdditionalEdit_FuelType_DropDown.Text = dataBlock.vehiclesTables.GetVehTypeFuelName(vehTypes[0].Value);
            //TO1
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.MRO1, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            AdditionalEdit_TO1_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            AdditionalEdit_TO1_MinValTextBox.Text = "";
            AdditionalEdit_TO1_MaxValTextBox.Text = "";
            AdditionalEdit_TO1_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //TO2
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.MRO2, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            AdditionalEdit_TO2_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            AdditionalEdit_TO2_MinValTextBox.Text = "";
            AdditionalEdit_TO2_MaxValTextBox.Text = "";
            AdditionalEdit_TO2_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            #endregion
            #region "HardwareTab"
            int deviceId = dataBlock.vehiclesTables.GetVehicleDeviceId(selectedVehID);
            int deviceTypeId = dataBlock.deviceTable.GetDeviceType(deviceId);

            HardwareEdit_DeviceTypeLabel.Text = "Device type:";
            HardwareEdit_DeviceTypeTextBox.Text = "";

            HardwareEdit_DeviceNameLabel.Text = "Device name:";
            HardwareEdit_DeviceNameTextBox.Text = "";

            HardwareEdit_DeviceNumberLabel.Text = "Device number:";
            HardwareEdit_DeviceNumberTextBox.Text = "";

            HardwareEdit_DeviceProductionDateLabel.Text = "Device production date:";
            HardwareEdit_DeviceProductionDateTextBox.Text = "";

            int firmwareId = dataBlock.deviceTable.GetDeviceFirmwareId(deviceId);
            HardwareEdit_DeviceFirmwareVersionLabel.Text = "Firmaware version:";
            HardwareEdit_DeviceFirmwareVersionTextBox.Text = "";

            HardwareEdit_DevicePhoneSimNumberLabel.Text = "Телефонный номер SIM";
            HardwareEdit_DevicePhoneSimNumberTextBox.Text = "";
            #endregion
            #region "Koef"
            //	Nominal turns
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.NominalTurns, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_NomRPM_Lable.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_NomRPM_MinValTextBox.Text = "";
            KoefEditTable_NomRPM_MaxValTextBox.Text = "";
            KoefEditTable_NomRPM_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //	Maximum speed
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.MaxSpeed, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_MaxSpeed_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_MaxSpeed_MinValTextBox.Text = "";
            KoefEditTable_MaxSpeed_MaxValTextBox.Text = "";
            KoefEditTable_MaxSpeed_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //	Manoeuvring	
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.Manoeuvring, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_Manevr_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_Manevr_MinValTextBox.Text = "";
            KoefEditTable_Manevr_MaxValTextBox.Text = "";
            KoefEditTable_Manevr_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //	City	
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.City, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_City_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_City_MinValTextBox.Text = "";
            KoefEditTable_City_MaxValTextBox.Text = "";
            KoefEditTable_City_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //	HighWay
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.Highway, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_Magistral_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_Magistral_MinValTextBox.Text = "";
            KoefEditTable_Magistral_MaxValTextBox.Text = "";
            KoefEditTable_Magistral_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //  Nominal fuel consumption
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.NomFuelConsumption, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_NomFuelConsumpion_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_NomFuelConsumpion_MinValTextBox.Text = "";
            KoefEditTable_NomFuelConsumpion_MaxValTextBox.Text = "";
            KoefEditTable_NomFuelConsumpion_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            //  Cold start
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.ColdStart, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_ColdStart_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_ColdStart_MinValTextBox.Text = "";
            KoefEditTable_ColdStart_MaxValTextBox.Text = "";
            KoefEditTable_ColdStart_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            // 	Hot stop
            vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.HotStop, selectedVehID);
            keyId = dataBlock.vehiclesTables.GetVehicleKey_KeyId(vehicleKeyId);
            measureId = dataBlock.criteriaTable.GetMeasure_byKeyID(keyId);
            KoefEditTable_HotStop_Label.Text = dataBlock.criteriaTable.GetCriteriaName(keyId);
            KoefEditTable_HotStop_MinValTextBox.Text = "";
            KoefEditTable_HotStop_MaxValTextBox.Text = "";
            KoefEditTable_HotStop_MeasureLabel.Text = dataBlock.criteriaTable.GetMeasureFullName(measureId).ToString();
            #endregion
        }
        catch (Exception ex)
        {
            dataBlock.CloseConnection();
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
        finally
        {
            dataBlock.CloseConnection();
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

    protected void VehiclesDataGrid_RadioButton_Checked(object sender, EventArgs e)
    {
       /* foreach (DataGridItem oldrow in VehiclesDataGrid.Items)
        {
            ((RadioButton)oldrow.FindControl("VehiclesDataGrid_RadioButton")).Checked = false;
        }*/

        //Set the new selected row
        RadioButton rb = (RadioButton)sender;
        DataGridItem row = (DataGridItem)rb.NamingContainer;
        ((RadioButton)row.FindControl("VehiclesDataGrid_RadioButton")).Checked = true;
        Selected_VehiclesDataGrid_Index.Value = row.Cells[1].Text;//row.ItemIndex.ToString();  Тут смотреть.
        
    }

    public void SaveAllUpdatedInformation()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            string selectedIndexString = Selected_VehiclesDataGrid_Index.Value;
            if (selectedIndexString == "")
                throw new Exception("Не выбрано транспортное средство для редактирования");
            int selectedVehID = Convert.ToInt32(selectedIndexString);
            
            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            SaveInfo(selectedVehID, dataBlock);
            //comment
            int cardId = dataBlock.vehiclesTables.GetCardId(selectedVehID);
            string commentToAVehicle = GeneralVehInfo_Comment_TextBox.Text;
            dataBlock.cardsTable.SetCardNote(cardId, commentToAVehicle);
            dataBlock.CommitTransaction();
            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.RollbackConnection();
            dataBlock.CloseConnection();
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }        
        Load_EditInfo();
        HideEditWindow();
    }

    public void SaveAllNewInformation()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
         DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            string RegNumber = GeneralVehInfo_RegNumbTextBox.Text;
            string Vin = GeneralVehInfo_VinTextBox.Text;
            DateTime dateBlocked = new DateTime();
            int priority = Convert.ToInt32(GeneralVehInfo_Priority_TextBox.Text);
            string comment = GeneralVehInfo_Comment_TextBox.Text;
            string vehManufacturer = GeneralVehInfo_Manufacturer_TextBox.Text;
            int VehicleTypeId = Convert.ToInt32(AdditionalEdit_VehicleType_DropDown.SelectedValue);

            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
            int newCardId = dataBlock.cardsTable.CreateNewCard(RegNumber, Vin, dataBlock.cardsTable.vehicleCardTypeId, orgId, "Created manually " + RegNumber, curUserId, 1);
           
            int selectedVehId = dataBlock.vehiclesTables.AddNewVehicle(RegNumber, vehManufacturer, Vin, VehicleTypeId, 1, newCardId, new DateTime(), priority);
            SaveInfo(selectedVehId, dataBlock);
            //comment
            int cardId = dataBlock.vehiclesTables.GetCardId(selectedVehId);
            string commentToAVehicle = GeneralVehInfo_Comment_TextBox.Text;
            dataBlock.cardsTable.SetCardNote(cardId, commentToAVehicle);
            dataBlock.CommitTransaction();
            dataBlock.vehiclesTables.CloseConnection();
        }
        catch (Exception ex)
        {
            dataBlock.RollbackConnection();
            dataBlock.CloseConnection();
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }        
        Load_EditInfo();
        HideEditWindow();
    }

    private void SaveInfo(int selectedVehID, DataBlock dataBlock)
    {
        #region "Variables define"
        //GENERAL
        string RegNumber = GeneralVehInfo_RegNumbTextBox.Text;
        string Vin = GeneralVehInfo_VinTextBox.Text;
        DateTime dateBlocked = new DateTime();
        int priority = Convert.ToInt32(GeneralVehInfo_Priority_TextBox.Text);
        string comment = GeneralVehInfo_Comment_TextBox.Text;
        string vehManufacturer = GeneralVehInfo_Manufacturer_TextBox.Text;
        //Additional
        int FuelTank1Min = Convert.ToInt32(AdditionalEdit_Bak_1_MinValTextBox.Text);
        int FuelTank1Max = Convert.ToInt32(AdditionalEdit_Bak_1_MaxValTextBox.Text);
        int FuelTank2Min = Convert.ToInt32(AdditionalEdit_Bak_2_MinValTextBox.Text);
        int FuelTank2Max = Convert.ToInt32(AdditionalEdit_Bak_2_MaxValTextBox.Text);
        int LoadCarryingMin = Convert.ToInt32(AdditionalEdit_LoadCcarrying_MinValTextBox.Text);
        int LoadCarryingMax = Convert.ToInt32(AdditionalEdit_LoadCcarrying_MaxValTextBox.Text);
        int VehicleTypeId = Convert.ToInt32(AdditionalEdit_VehicleType_DropDown.SelectedValue);
        DateTime MRO1Min = new DateTime();
        DateTime MRO1Max = new DateTime();
        DateTime MRO2Min = new DateTime();
        DateTime MRO2Max = new DateTime();
        //Koeff
        int NominalTurnsMin = Convert.ToInt32(KoefEditTable_NomRPM_MinValTextBox.Text);
        int NominalTurnsMax = Convert.ToInt32(KoefEditTable_NomRPM_MaxValTextBox.Text);
        int MaximumSpeedMin = Convert.ToInt32(KoefEditTable_MaxSpeed_MinValTextBox.Text);
        int MaximumSpeedMax = Convert.ToInt32(KoefEditTable_MaxSpeed_MaxValTextBox.Text);
        int ManoeuvringMin = Convert.ToInt32(KoefEditTable_Manevr_MinValTextBox.Text);
        int ManoeuvringMax = Convert.ToInt32(KoefEditTable_Manevr_MaxValTextBox.Text);
        int CityMin = Convert.ToInt32(KoefEditTable_City_MinValTextBox.Text);
        int CityMax = Convert.ToInt32(KoefEditTable_City_MaxValTextBox.Text);
        int HighwayMin = Convert.ToInt32(KoefEditTable_Magistral_MinValTextBox.Text);
        int HighwayMax = Convert.ToInt32(KoefEditTable_Magistral_MinValTextBox.Text);
        int NominalFuelConsumptionMin = Convert.ToInt32(KoefEditTable_NomFuelConsumpion_MinValTextBox.Text);
        int NominalFuelConsumptionMax = Convert.ToInt32(KoefEditTable_NomFuelConsumpion_MaxValTextBox.Text);
        int ColdStartMin = Convert.ToInt32(KoefEditTable_ColdStart_MinValTextBox.Text);
        int ColdStartMax = Convert.ToInt32(KoefEditTable_ColdStart_MaxValTextBox.Text);
        int HotStopMin = Convert.ToInt32(KoefEditTable_HotStop_MinValTextBox.Text);
        int HotStopMax = Convert.ToInt32(KoefEditTable_HotStop_MaxValTextBox.Text);
        #endregion
        int vehicleKeyId;
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);
        //////////////General/////////////////          
       
        //AllGeneralVehicle
        int deviceId = dataBlock.vehiclesTables.GetVehicleDeviceId(selectedVehID);
        dataBlock.vehiclesTables.EditVehicle(selectedVehID, RegNumber, vehManufacturer, Vin, VehicleTypeId, deviceId, dateBlocked, priority, curUserId);
        //////////////Additional//////////////
        //fuel tank1
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.FuelTank1, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, FuelTank1Min, FuelTank1Max, new DateTime(), new DateTime(), "");
        //fuel tank2
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.FuelTank2, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, FuelTank2Min, FuelTank2Max, new DateTime(), new DateTime(), "");
        //LoadCerrying
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.LoadCarryingCapacity, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, LoadCarryingMin, LoadCarryingMax, new DateTime(), new DateTime(), "");
        //MRO1
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.MRO1, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, 0, 0, MRO1Min, MRO1Max, "");
        //MRO2
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.MRO2, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, 0, 0, MRO2Min, MRO2Max, "");
        //////////////////////////KOEF/////////////////////////////////////
        //nominalTurns
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.NominalTurns, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, NominalTurnsMin, NominalTurnsMax, new DateTime(), new DateTime(), "");
        //maxSpeed
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.MaxSpeed, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, MaximumSpeedMin, MaximumSpeedMax, new DateTime(), new DateTime(), "");
        //Manoeuvring
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.Manoeuvring, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, ManoeuvringMin, ManoeuvringMax, new DateTime(), new DateTime(), "");
        //City
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.City, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, CityMin, CityMax, new DateTime(), new DateTime(), "");
        //Highway
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.Highway, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, HighwayMin, HighwayMax, new DateTime(), new DateTime(), "");
        //NominalFuelConsumption
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.NomFuelConsumption, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, NominalFuelConsumptionMin, NominalFuelConsumptionMax, new DateTime(), new DateTime(), "");
        //ColdStart
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.ColdStart, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, ColdStartMin, ColdStartMax, new DateTime(), new DateTime(), "");
        //HotStop
        vehicleKeyId = dataBlock.vehiclesTables.GetVehicleKeyId_byKeyNameVehicleId(DataBaseReference.HotStop, selectedVehID);
        dataBlock.vehiclesTables.SetVehicleKey(vehicleKeyId, HotStopMin, HotStopMax, new DateTime(), new DateTime(), "");
    }

    private void HideEditWindow()
    {
        Session["Settings_GeneralTabException"] = "HideThisWindow";
        RaiseBubbleEvent(null, new EventArgs());
    }

    protected void Upload_Click(object Sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            if (MyFileUpload.PostedFile.FileName == "")
                throw new Exception("Укажите файл для загрузки!");

            string GuidFIleName = Guid.NewGuid().ToString();
            string fileName = Server.MapPath(".\\") + "Photos\\Vehicles\\" + GuidFIleName + "_" + MyFileUpload.PostedFile.FileName;
            string fileNameToDB = "~/Administrator/Photos/Vehicles/" + GuidFIleName + "_" + MyFileUpload.PostedFile.FileName;

           string selectedIndexString = Selected_VehiclesDataGrid_Index.Value;
            if (selectedIndexString == "")
                throw new Exception("Не выбрано транспортное средство для редактирования");
            int selectedVehicleID = Convert.ToInt32(selectedIndexString);

            int vehicleInfoId;

            if (MyFileUpload.PostedFile.ContentType == "image/jpeg" || MyFileUpload.PostedFile.ContentType == "image/png")
            {
                MyFileUpload.PostedFile.SaveAs(fileName);

                dataBlock.OpenConnection();
                dataBlock.OpenTransaction();
                vehicleInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.VehiclePhotoAddress);
                dataBlock.vehiclesTables.EditVehicleInfo(selectedVehicleID, vehicleInfoId, fileNameToDB);
                dataBlock.CommitTransaction();
                dataBlock.CloseConnection();
                VehiclesPhotoImage.ImageUrl = fileNameToDB;
            }
            else
                throw new Exception("Неправильный формат файла");
        }
        catch (Exception ex)
        {
            dataBlock.RollbackConnection();
            dataBlock.CloseConnection();
            Session["Settings_GeneralTabException"] = ex;
            RaiseBubbleEvent(null, new EventArgs());
        }
    }

}
