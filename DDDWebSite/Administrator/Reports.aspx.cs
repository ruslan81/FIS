using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BLL;
using InfoSoftGlobal;
using System.Data;
using System.Reflection;
using System.IO;
using System.Xml;
using DevExpress.XtraReports.UI;

public partial class Administrator_Report : System.Web.UI.Page
{
    int contentPX;

    protected void Page_Load(object sender, EventArgs e)
    {
        /*CalendarApplyButton2.ButtOnClick += new EventHandler(ShowReport);
        Export_OK_Button.ButtOnClick += new EventHandler(ExportButton_Click);
        SendByEmail.ButtOnClick += new EventHandler(SendReportByEmail);*/
        
        /*object postBackObject = Global.GetPostBackControl(this);
        if (postBackObject != null)
        {
            if ("Stimulsoft.Report.Web.StiNextToolButton" == postBackObject.ToString()
                || "Stimulsoft.Report.Web.StiPrevToolButton" == postBackObject.ToString()
                || "Stimulsoft.Report.Web.StiLastToolButton" == postBackObject.ToString()
                || "Stimulsoft.Report.Web.StiFirstToolButton" == postBackObject.ToString()
                || "Stimulsoft.Report.Web.StiZoomList" == postBackObject.ToString()
                || "Stimulsoft.Report.Web.StiViewModeList" == postBackObject.ToString()
                || "Stimulsoft.Report.Web.StiSaveToolButton" == postBackObject.ToString())
            {
                return;
            }
        }*/
        #region "!IsPostBack"
        if (!IsPostBack)
        {           
            try
            {
                /*Session["AnyChartStockSession_1"] = null;
                UserControlsForAll_BlueButton pan = ((UserControlsForAll_BlueButton)Page.Master.FindControl("ReportsMasterButt"));
                pan.Enabled = false;*/


                //Session["maxRecordsCount"] = 1500;

                string connectionString = System.Configuration.ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
                BLL.DataBlock dataBlock = new BLL.DataBlock(connectionString, "STRING_EN");
                dataBlock.OpenConnection();
                int userId = dataBlock.usersTable.Get_UserID_byName(User.Identity.Name);
                //ORG NAME сверху
                string curOrgName = "";
                curOrgName = dataBlock.usersTable.Get_UserOrgName(userId);
                ((Label)Master.FindControl("CompanyHeaderOrgName")).Text = curOrgName;
                //USER NAME сверху
                curOrgName = dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_Surname);
                curOrgName += " " + dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_Name);
                ((Label)Master.FindControl("UserNameHeaderName")).Text = curOrgName;
                ////////////////////////////
                ((Panel)Master.FindControl("AdditionalConditionsPanel")).Visible = false;

                int orgId = dataBlock.usersTable.Get_UserOrgId(userId);
                dataBlock.CloseConnection();
                Session["CURRENT_ORG_ID"] = orgId;

                //выставляем кук, чтобы можно было передать его в метод, вызываемый ч/з ajax
                Response.Cookies["CURRENT_ORG_ID"].Value = Convert.ToString(orgId);
                Response.Cookies["CURRENT_USERNAME"].Value = User.Identity.Name;

                /*Load_Vehicles();
                Load_ReportsList();
                Load_PLFAndDrivers();
                LoadPrintExportsLists();*/
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        //MasterPage m = (MasterPage)Page.Master;
        /*contentPX = 523;
        //m.ResizeReportDiv(contentPX);
        // FCLiteral.Text = "";
        ChartLiteral.Text = "";
        resultActionLabel.Text = "";
        // StiWebViewer1.GetCallbackResult();
        int accordionIndex = -1;
        if (int.TryParse(AccordionSelectedPane.Value, out accordionIndex))
        {
            if (accordionIndex != 4)
            {
                PlfReportsRadioButtonList.Visible = false;
                if (PLFTreeView.SelectedNode != null)
                {
                    PLFTreeView.SelectedNode.Selected = false;
                    PlfFilesList.Text = "";
                    NavigationReportControl1.Visible = false;
                    AnycahrtPanel.Visible = false;
                }
            }
            if (accordionIndex == 4 || accordionIndex == 0 || accordionIndex == 2 || accordionIndex == 1)
            {
                PlfReportsRadioButtonList.Visible = true;
            }
        }
        int hiddenfieldTest = -1;
        if (int.TryParse(HiddenField.Value, out hiddenfieldTest))
        {
            if (hiddenfieldTest >= 0)
            {
                PlfReportsRadioButtonList.Items.Remove(PlfReportsRadioButtonList.Items.FindByValue("no report"));
            }
        }*/
    }


    //AJAX BEGIN


    /// <summary>
    ///Получить элементы дерева в разделе "PLF Файлы"
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<PLFFilesTreeItem> GetPLFFilesTree(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        int orgId = Convert.ToInt32(OrgID);
        List<string> names = new List<string>();
        List<string> numbers = new List<string>();
        List<int> IDs = new List<int>();
        List<int> PLFs = new List<int>();

        dataBlock.OpenConnection();

        IDs = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.driversCardTypeId);
        names = dataBlock.cardsTable.GetCardNames(IDs);
        numbers = dataBlock.cardsTable.GetCardNumbers(IDs);
        List<PLFFilesTreeItem> PLFFilesTreeItems = new List<PLFFilesTreeItem>();
        
        const int CARD_TYPE_DRIVER=0;
        const int CARD_TYPE_PLF=2;

        for (int i = 0; i < names.Count;i++)
        {
            PLFFilesTreeItem treeItem=new PLFFilesTreeItem ();
            treeItem.ID=IDs[i];
            treeItem.Name=names[i];
            treeItem.Number=numbers[i];
            
            PLFFilesTreeItems.Add(treeItem);

            PLFs= dataBlock.cardsTable.GetAllDataBlockIds_byCardId(IDs[i]);
            if (PLFs != null)
            {
                foreach (int plf in PLFs)
                {
                    int cardType; //0 - driver, 2 - plf
                    cardType = dataBlock.GetDataBlock_CardType(plf);

                    if (cardType == CARD_TYPE_PLF)
                    {
                        string vehicle = dataBlock.plfUnitInfo.Get_VEHICLE(plf);
                        string deviceID = dataBlock.plfUnitInfo.Get_ID_DEVICE(plf);
                        string period = "(" + dataBlock.plfUnitInfo.Get_START_PERIOD(plf).ToShortDateString() +
                                        " - " +
                                        dataBlock.plfUnitInfo.Get_END_PERIOD(plf).ToShortDateString() + ")";

                        bool exist=false;
                        foreach(PLFFilesTreeItem item in PLFFilesTreeItems)
                        {
                            foreach(PLFItem PLFItem in item.PLFItems)
                            {
                                if(PLFItem.DeviceID.Equals(vehicle)&&PLFItem.Equals(deviceID)){
                                    PLFItem.PLFs.Add(new MapItem(plf.ToString(), period));
                                    exist=true;
                                    break;
                                }
                            }
                        }
                        if(!exist)
                        {
                            PLFItem PLFItem = new PLFItem();
                            PLFItem.Vehicle = vehicle;
                            PLFItem.DeviceID = deviceID;
                            PLFItem.PLFs.Add(new MapItem(plf.ToString(), period));

                            treeItem.PLFItems.Add(PLFItem);
                        }
                    }
                }
            }
        }
        dataBlock.CloseConnection();

        return PLFFilesTreeItems;
    }

    /// <summary>
    ///Получить типы отчетов для генерации из plf файлов
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static string[] GetPLFReportTypes()
    {
        //load needed template
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/templates_plf"));
        string[] fileNames = new string[filePaths.Length];
        for (int i = 0; i < filePaths.Length; i++)
        {
            fileNames[i] = Path.GetFileNameWithoutExtension(filePaths[i]);
        }
        return fileNames;
    }

     /// <summary>
    ///Получить отчет в разделе "PLF Файлы"
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static Report GetPLFReport(String CardID, String PLFID, String UserName, String ReportType)
    {
        int dataBlockId = int.Parse(PLFID);
        List<int> dataBlockIDS = new List<int>();
        dataBlockIDS.Add(dataBlockId);
        int cardID = int.Parse(CardID);

        string connectionString = System.Configuration.ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        BLL.DataBlock dataBlock = new BLL.DataBlock(connectionString, "STRING_EN");
        dataBlock.OpenConnection();

        DateTime from = new DateTime();
        from = dataBlock.plfUnitInfo.Get_START_PERIOD(dataBlockId);

        DateTime to = new DateTime();
        to = dataBlock.plfUnitInfo.Get_END_PERIOD(dataBlockId);

        DataSet dataset = new DataSet();

        int userId = dataBlock.usersTable.Get_UserID_byName(UserName);

        List<PLFUnit.PLFRecord> records = new List<PLFUnit.PLFRecord>();
        dataset = ReportDataSetLoader.Get_PLF_ALLData(dataBlockIDS,
            new DateTime(from.Year, from.Month, from.Day), new DateTime(to.Year, to.Month, to.Day),
            cardID, userId, ref records);

        dataBlock.CloseConnection();

        //load needed template
        string path = HttpContext.Current.Server.MapPath("~/templates_plf") + "\\";
        XtraReport report = new XtraReport();
        if (string.IsNullOrEmpty(ReportType))
        {
            ReportType = "Полный отчет";
        }
        report.LoadLayout(path + ReportType+".repx");
        report.DataSource = dataset;
        MemoryStream reportStream = new MemoryStream();
        report.ExportToHtml(reportStream);
        reportStream.Seek(0, SeekOrigin.Begin);

        // convert stream to string
        StreamReader reader = new StreamReader(reportStream);
        string textReport = reader.ReadToEnd();

        Report r = new Report();
        r.report = textReport;
        r.time = new double[records.Count];
        r.speed = new double[records.Count];
        r.voltage = new double[records.Count];
        r.rpm = new double[records.Count];
        r.fuel = new double[records.Count];
        r.lat= new double[records.Count];
        r.lng = new double[records.Count];
        for (int i=0;i<records.Count;i++)
        {
            double t=(records[i].SYSTEM_TIME.GetSystemTime()-new DateTime(1970,1,1,0,0,0)).TotalMilliseconds;
            r.time[i] = t;
            r.speed[i]=double.Parse(records[i].SPEED, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            r.voltage[i] = double.Parse(records[i].VOLTAGE, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            r.rpm[i] = double.Parse(records[i].ENGINE_RPM, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            r.fuel[i] = Math.Round(double.Parse(records[i].FUEL_VOLUME1),1);
            if (records[i].LATITUDE != null && records[i].LONGITUDE != null)
            {
                r.lat[i] = double.Parse(records[i].LATITUDE, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                r.lng[i] = double.Parse(records[i].LONGITUDE,System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            }
            else
            {
                r.lat[i] = 0;
                r.lng[i] = 0;
            }
        }
        r.period = new DateTime(from.Year, from.Month, from.Day).ToShortDateString() + " - " + new DateTime(to.Year, to.Month, to.Day).ToShortDateString();
        
        return r;
    }

    /// <summary>
    ///Получить элементы дерева в разделе "Транспортные средства"
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<VehiclesTreeItem> GetVehiclesTree(String OrgID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        int orgId = Convert.ToInt32(OrgID);

        dataBlock.OpenConnection();

        List<int> cardsList = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.vehicleCardTypeId);
        List<VehiclesTreeItem> vehicles = new List<VehiclesTreeItem>();

        foreach (int cardId in cardsList)
        {
            string name = dataBlock.cardsTable.GetCardName(cardId);
            int vehicleId = dataBlock.vehiclesTables.GetVehicle_byCardId(cardId);
            string vehicleVin = dataBlock.vehiclesTables.GetVehicleVin(vehicleId);

            VehiclesTreeItem vehiclesTreeItem = new VehiclesTreeItem();
            vehiclesTreeItem.Name=name;
            vehiclesTreeItem.VehicleID = vehicleId;
            vehiclesTreeItem.VehicleVin = vehicleVin;
            vehiclesTreeItem.Files = new List<VehiclesTreeFileItem>();

            List<int> vehDataBlocks = dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId);
            foreach (int dataBlockId in vehDataBlocks)
            {
                List<DateTime> vehsCardPeriod = dataBlock.vehicleUnitInfo.Get_StartEndPeriod(dataBlockId);
                string fileName=dataBlock.GetDataBlock_FileName(dataBlockId);

                VehiclesTreeFileItem vehiclesTreeFileItem = new VehiclesTreeFileItem();
                vehiclesTreeFileItem.DataBlockID = dataBlockId;
                vehiclesTreeFileItem.FileName= dataBlock.GetDataBlock_FileName(dataBlockId);
                vehiclesTreeFileItem.VehicleCardPeriodBegin = vehsCardPeriod[0].ToShortDateString();
                vehiclesTreeFileItem.VehicleCardPeriodEnd = vehsCardPeriod[1].ToShortDateString();

                vehiclesTreeItem.Files.Add(vehiclesTreeFileItem);
            }

            vehicles.Add(vehiclesTreeItem);
        }

        dataBlock.vehiclesTables.CloseConnection();


        return vehicles;
    }

    /// <summary>
    ///Получить типы отчетов для генерации из plf файлов
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static string[] GetDDDReportTypes()
    {
        //load needed template
        string[] filePaths = Directory.GetFiles(HttpContext.Current.Server.MapPath("~/templates_ddd"));
        string[] fileNames = new string[filePaths.Length];
        for (int i = 0; i < filePaths.Length; i++)
        {
            fileNames[i] = Path.GetFileNameWithoutExtension(filePaths[i]);
        }
        return fileNames;
    }

    /// <summary>
    ///Получить отчет в разделе "Транспортные средства"
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static String GetDDDReport(String DataBlockID, String UserName, String ReportType)
    {
        int dataBlockId = int.Parse(DataBlockID);
        List<int> dataBlockIDS = new List<int>();
        dataBlockIDS.Add(dataBlockId);

        string connectionString = System.Configuration.ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        dataBlock.OpenConnection();

        DataSet dataset = new DataSet();

        string VIN = dataBlock.vehicleUnitInfo.Get_VehicleOverview_IdentificationNumber(dataBlockId).ToString();
        string RegNumb = dataBlock.vehicleUnitInfo.Get_VehicleOverview_RegistrationIdentification(dataBlockId).vehicleRegistrationNumber.ToString();
        int vehicleId = dataBlock.vehiclesTables.GetVehicleId_byVinRegNumbers(VIN, RegNumb);

        List<DateTime> vehsCardPeriod = dataBlock.vehicleUnitInfo.Get_StartEndPeriod(dataBlockId);

        int userId = dataBlock.usersTable.Get_UserID_byName(UserName);

        dataset = ReportDataSetLoader.Get_Vehicle_ALLDate(vehicleId,
            dataBlockIDS, vehsCardPeriod[0], vehsCardPeriod[1], userId);

        dataBlock.CloseConnection();

        //load needed template
        string path = HttpContext.Current.Server.MapPath("~/templates_ddd") + "\\";
        XtraReport report = new XtraReport();
        if (string.IsNullOrEmpty(ReportType))
        {
            ReportType = "Полный отчет";
        }
        report.LoadLayout(path + ReportType + ".repx");
        report.DataSource = dataset;
        MemoryStream reportStream = new MemoryStream();
        report.ExportToHtml(reportStream);
        reportStream.Seek(0, SeekOrigin.Begin);

        // convert stream to string
        StreamReader reader = new StreamReader(reportStream);
        string textReport = reader.ReadToEnd();

        return textReport;
    }


    //AJAX END


    /*DataTable CreateDataSource(List<string> data, int header) //0 - Водители, 1 - ТС, 2 - plf
    {
        DataTable dt = new DataTable();
        DataRow dr;

        if (header == 0)
            dt.Columns.Add(new DataColumn("Имя водителя", typeof(string)));
        if(header == 1)
            dt.Columns.Add(new DataColumn("Номер ТС", typeof(string)));
        if(header ==2)
            dt.Columns.Add(new DataColumn("Номер ТС - Номер устройства", typeof(string)));


        foreach (string item in data)
        {
            dr = dt.NewRow();
            dr[0] = item;
            dt.Rows.Add(dr);
        }
        return dt;
    }*/

    /*private void Load_Vehicles()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        dataBlock.OpenConnection();
        int userID = dataBlock.usersTable.Get_UserID_byName(User.Identity.Name);
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);

        List<int> cardsList = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.vehicleCardTypeId);
        List<int> vehDataBlocks = new List<int>(); 
        List<DateTime> vehsCardPeriod = new List<DateTime>();
        int vehicleId = -1;
        string VehName;
        foreach (int cardId in cardsList)
        {
            vehicleId = dataBlock.vehiclesTables.GetVehicle_byCardId(cardId);
            VehName = dataBlock.vehiclesTables.GetVehicleVin(vehicleId) + " " + dataBlock.vehiclesTables.GetVehicleGOSNUM(vehicleId);
            MultiVehicles.Items.Add( new ListItem(VehName, cardId.ToString()));
            VehiclesTreeView.Nodes.Add(new TreeNode(VehName, ""));
            vehDataBlocks = dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId);
            foreach (int dataBlockId in vehDataBlocks)
            {
                vehsCardPeriod = dataBlock.vehicleUnitInfo.Get_StartEndPeriod(dataBlockId);
                VehiclesTreeView.Nodes[VehiclesTreeView.Nodes.Count - 1].Value += dataBlockId.ToString() + " ";
                VehiclesTreeView.Nodes[VehiclesTreeView.Nodes.Count - 1].ChildNodes.Add(new TreeNode("(" + vehsCardPeriod[0].ToShortDateString() + " - " + vehsCardPeriod[1].ToShortDateString() + ")", dataBlockId.ToString()));
            }
        }
        dataBlock.vehiclesTables.CloseConnection();
        ///Все убрал, пока не разберусь с подключениями
        //VehiclesTreeView.CollapseAll();
    }*/

    /*private void Load_PLFAndDrivers()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        List<string> names = new List<string>();
        List<string> numbers = new List<string>();
        List<int> namesIds = new List<int>();
        List<int> plfIds = new List<int>();
        List<string> plfVehNames = new List<string>();
        dataBlock.OpenConnection();
        namesIds = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.driversCardTypeId);
        names = dataBlock.cardsTable.GetCardNames(namesIds);
        numbers = dataBlock.cardsTable.GetCardNumbers(namesIds);
        string name_deviceId;
        string startEndPeriod;
        int cardType; //0-driver, 2 - plf
        List<DateTime> driversCardPeriod = new List<DateTime>();

        int index;

        for (int i = 0; i < names.Count;i++)
        {
            drSearch.Items.Add(names[i]);
            plfDrSearch.Items.Add(names[i]);
            plfIds = new List<int>();
            PLFTreeView.Nodes.Add(new TreeNode(names[i] + " " + numbers[i], namesIds[i].ToString()));
            DriversTreeView.Nodes.Add(new TreeNode(names[i] + " " + numbers[i], ""));
            MultiDrivers.Items.Add(new ListItem(names[i] + " " + numbers[i], ""));
            plfIds = dataBlock.cardsTable.GetAllDataBlockIds_byCardId(namesIds[i]);
            plfVehNames = new List<string>();
            foreach (int plf in plfIds)
            {
                cardType = dataBlock.GetDataBlock_CardType(plf);
                if (cardType == 0)
                {
                    driversCardPeriod = new List<DateTime>();
                    driversCardPeriod = dataBlock.cardUnitInfo.Get_StartEndPeriod(plf);
                    name_deviceId = dataBlock.GetDriversNameOrVehiclesNumberByBlockId(plf);
                    startEndPeriod = "(" + driversCardPeriod[0].ToShortDateString() + " - " + driversCardPeriod[1].ToShortDateString() + ")";
                    DriversTreeView.Nodes[DriversTreeView.Nodes.Count - 1].ChildNodes.Add(new TreeNode(startEndPeriod, plf.ToString()));
                    DriversTreeView.Nodes[DriversTreeView.Nodes.Count - 1].Value+= " " + plf.ToString();
                    MultiDrivers.Items[MultiDrivers.Items.Count - 1].Value += " " + plf.ToString();
                }
                if (cardType == 2)
                {
                    name_deviceId = dataBlock.plfUnitInfo.Get_VEHICLE(plf) + " " + dataBlock.plfUnitInfo.Get_ID_DEVICE(plf);
                    startEndPeriod = "(" + dataBlock.plfUnitInfo.Get_START_PERIOD(plf).ToShortDateString() + " - " + dataBlock.plfUnitInfo.Get_END_PERIOD(plf).ToShortDateString() + ")";
                    if (plfVehNames.Contains(name_deviceId))
                    {
                        index = plfVehNames.IndexOf(name_deviceId);
                        PLFTreeView.Nodes[PLFTreeView.Nodes.Count - 1].ChildNodes[index].Value += " " + plf.ToString();//новая версия
                        PLFTreeView.Nodes[PLFTreeView.Nodes.Count - 1].ChildNodes[index].ChildNodes.Add(new TreeNode(startEndPeriod, plf.ToString()));//Убрать чуть что
                    }
                    else
                    {
                        PLFTreeView.Nodes[PLFTreeView.Nodes.Count - 1].ChildNodes.Add(new TreeNode(name_deviceId, plf.ToString()));
                        plfVehNames.Add(name_deviceId);
                        index = plfVehNames.IndexOf(name_deviceId);
                        PLFTreeView.Nodes[PLFTreeView.Nodes.Count - 1].ChildNodes[index].ChildNodes.Add(new TreeNode(startEndPeriod, plf.ToString()));//Убрать чуть что
                    }
                }
            }
        }
        dataBlock.CloseConnection();
        DriversTreeView.CollapseAll();
        PLFTreeView.CollapseAll();
    }*/

    /*private void Load_ReportsList()
    {
        PlfReportsPickUpdatePanel1.Update();
    }*/

    /*private void LoadPlfReports()
    {
        PlfReportsRadioButtonList.Items.Clear();
        //PlfReportsRadioButtonList.RepeatColumns = 3;
        PlfReportsRadioButtonList.Items.Add(new ListItem("Выберите отчет...", "no report"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("Полный отчет за календарный период(тест)", "Полный отчет за календарный период"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("ТС по дням","ТС по дням"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("Эффективность использования автопарка", "Эффективность использования автопарка"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("Отчет за период", "Отчет за период"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("Не показывать таблицы", "Не показывать таблицы"));
        PlfReportsPickUpdatePanel1.Update();
    }*/

    /*private void LoadDriversReports()
    {
        PlfReportsRadioButtonList.Items.Clear();
        //PlfReportsRadioButtonList.RepeatColumns = 3;
        PlfReportsRadioButtonList.Items.Add(new ListItem("Выберите отчет...", "no report"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("Ежедневный протокол активности", "Driver_DriverDailyActivityData"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("Еженедельный протокол активности", "Driver_DriverWeeklyActivityProtocol"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("Использование ТС", "Driver_VehicleUsed"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("Отчет о нарушениях", "Driver_InfringementReport"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("События", "Driver_EventsReport"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("Ошибки", "Driver_FaultsReport"));
        // PlfReportsPickUpdatePanel1.Update();

        int panelHeight = 95;
        MasterPage m = (MasterPage)Page.Master;
        m.ResizeAdditionalConditionsDiv(panelHeight);

    }*/

    /*private void LoadMultiDriversReports()
    {
        PlfReportsRadioButtonList.Items.Clear();
        //PlfReportsRadioButtonList.RepeatColumns = 4;
        PlfReportsRadioButtonList.Items.Add(new ListItem("Итоги по активности(Activity Summary)", "MultiDrivers_ActivitySummary"));
    }*/

    /*private void LoadVehiclesReports()
    {
        PlfReportsRadioButtonList.Items.Clear();
        //PlfReportsRadioButtonList.RepeatColumns = 4;
        PlfReportsRadioButtonList.Items.Add(new ListItem("Выберите отчет...", "no report"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("Превышение скорости", "Vehicle_DriverOverSpeeding"));
        PlfReportsRadioButtonList.Items.Add(new ListItem("Активность ТС", "Vehicle_DailyVehicleActivityProtocol"));
        PlfReportsPickUpdatePanel1.Update();
    }*/

    /*private void LoadPrintExportsLists()
    {
        ReportsExport_RadioButtonList.Items.Clear();
        ReportsExport_RadioButtonList.Items.Add(new ListItem("PDF", "PDF"));
        ReportsExport_RadioButtonList.Items.Add(new ListItem("CSV", "CSV"));
        ReportsExport_RadioButtonList.Items.Add(new ListItem("Excel", "Excel"));
        ReportsExport_RadioButtonList.Items.Add(new ListItem("Rtf", "Rtf"));
        ReportsExport_RadioButtonList.Items.Add(new ListItem("Текст (.txt)", "Txt"));
        ReportsExport_RadioButtonList.Items.Add(new ListItem("Изображение (.jpg)", "Jpg"));
        ReportsExport_RadioButtonList.Items[0].Selected = true;
    }*/

    /*protected void DriversTreeView_SelectedNodeChanged(Object sender, EventArgs e)
    {
        resultActionLabel.Text = "";
        ChartsCheckBoxList.Visible = false;
        AnycahrtPanel.Visible = false;
        NavigationReportControl1.Visible = false;

        //Делаем неактивными нижние кнопки, если отчет невидимый
        Export_Button.Enabled = false;
        Email_Button.Enabled = false;
        Print_Button.Enabled = false;

        DecisionUpdatePanel.Update();
        ///////////////////////////////////

        PLFTreeView.CollapseAll();
        VehiclesTreeView.CollapseAll();

        try
        {
            AccordionSelectedPane.Value = "0";

            HiddenField.Value = DriversTreeView.SelectedNode.Value;
            if (DriversTreeView.SelectedNode.Value.Trim() == "")
                throw new Exception("Нет данных!");

            LoadDriversReports();
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            DataTable dataTable = new DataTable();

            ReportNameLabel.Text = "Выберите отчет";
            DriversNameSourceLabel.Text = "Водитель: ";
            DriversNameLabel.Text = "";
            if (DriversTreeView.SelectedNode.Parent == null)
                DriversNameLabel.Text += DriversTreeView.SelectedNode.Text;
            if (DriversTreeView.SelectedNode.ChildNodes.Count == 0)
                DriversNameLabel.Text += DriversTreeView.SelectedNode.Parent.Text;

            List<int> driverIds = new List<int>();
            driverIds = getIdList();
            List<DateTime> startDateList = new List<DateTime>();
            List<DateTime> endDateList = new List<DateTime>();
            List<DateTime> period = new List<DateTime>();
            //   PlfFilesList.Text = "Загруженные файлы:" + Environment.NewLine;
            dataBlock.OpenConnection();
            foreach (int id in driverIds)
            {
                period = dataBlock.cardUnitInfo.Get_StartEndPeriod(id);
                startDateList.Add(period[0]);
                endDateList.Add(period[1]);
                //       PlfFilesList.Text += "(" + startDateList[startDateList.Count - 1] + " - "
                //           + endDateList[startDateList.Count - 1] + ")" + Environment.NewLine;
            }
            dataBlock.CloseConnection();
            if (startDateList.Count == 0 || endDateList.Count == 0)
                throw new Exception("Ни одной карты не загружено!");

            startDateList.Sort();
            endDateList.Sort();

            CalendarFromExtender.SelectedDate = startDateList[0];
            CalendarToExtender.SelectedDate = endDateList[endDateList.Count - 1];
        }
        catch (Exception ex)
        {
            resultActionLabel.Text = "Произошла ошибка: " + ex.Message;
        }
        finally
        {
            HiddenFieldUpdatePanel.Update();
            ChoisesUpdatePanel.Update();
            ChartsCheckBoxListUpdatePanel.Update();
            //ReportUpdatePanel.Update();
           OutputUpdatePanel.Update();
        }
    }*/

    /*protected void ShowReport(object sender, EventArgs e)
    {
        resultActionLabel.Text = "";
        Exception noSelectedDriver = new Exception("Не выбран объект для отчета!");
        Exception noReportException = new Exception("Этот отчет находится в разработке");
       // StiWebViewer1.Visible = true;
        DateTime from = new DateTime();
        DateTime to = new DateTime();
        DataSet dataset = new DataSet();
        string connectionString = System.Configuration.ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        BLL.DataBlock dataBlock = new BLL.DataBlock(connectionString, "STRING_EN");
        int dataBlockId = -1;
        bool charts = false;
        try
        {
            from = getDate("fromCalendarDate");
            to = getDate("toCalendarDate");
            TreeNode node = new TreeNode();
            int accordionIndex = Convert.ToInt32(AccordionSelectedPane.Value);
            if (accordionIndex == 0)//drivers
            {
                node.Value = "DriverReport";
            }
            if (accordionIndex == 1)//vehicles
            {
                node.Value = "VehiclesReport";
            }
            if (accordionIndex == 2)//multiDrivers
            {
                node.Value = "MultiDriversReport";
            }
            if (accordionIndex == 3)//multiVehicles
            {
                node.Value = "MultiVehiclesReport";
            }
            if (accordionIndex == 4)//PLF
            {
               // node = new TreeNode();
                node.Value = "PLFReport";
            }

            if (node.ChildNodes.Count == 0)
            {
                dataBlock.OpenConnection();
                int userId = dataBlock.usersTable.Get_UserID_byName(User.Identity.Name);
                dataBlock.CloseConnection();

                switch (node.Value)
                {
                    #region "Drivers Reports"
                    case "DriverReport":
                        {
                            List<int> dataBlockIDS = new List<int>();
                            dataBlock.OpenConnection();
                            int cardId = dataBlock.cardsTable.GetCardId(DriversNameLabel.Text, dataBlock.cardsTable.driversCardTypeId);
                            dataBlock.CloseConnection();
                            if (Selected_PlfReportsDataGrid_Index.Value == null)
                                throw new Exception("Выберите тип отчета!");
                            string DriversReportType = Selected_PlfReportsDataGrid_Index.Value;
                            dataBlockIDS = getIdList();
                            dataBlockId = dataBlockIDS[0]; 


                            switch (DriversReportType)
                            {
                                case "Driver_DriverDailyActivityData":
                                    {
                                        dataset = ReportDataSetLoader.Get_Driver_DriverDailyActivityData(dataBlockIDS, from, to, cardId, userId);
                                        LoadStiReport(dataset, "Reports/DriversReports/DailyDriverActivityProtocol.mrt");
                                    } break;
                                case "Driver_DriverWeeklyActivityProtocol":
                                    {
                                        dataset = ReportDataSetLoader.Get_Driver_DriverWeeklyActivityData(dataBlockIDS, from, to, cardId, userId);
                                        LoadStiReport(dataset, "Reports/DriversReports/WeeklyDriverActivityProtocol.mrt");
                                    } break;
                                case "Driver_VehicleUsed":
                                    {
                                        dataset = ReportDataSetLoader.Get_Driver_VehicleUsed(dataBlockIDS, from, to, cardId, userId);
                                        LoadStiReport(dataset, "Reports/DriversReports/DriverVehicleUsing.mrt");
                                    } break;
                                case "Driver_InfringementReport":
                                    {
                                        dataset = ReportDataSetLoader.Get_Driver_InfringementReport(dataBlockId, from, to);
                                    } break;
                                case "Driver_EventsReport":
                                    {
                                        dataset = ReportDataSetLoader.Get_Driver_EventsReport(dataBlockId, from, to);
                                    } break;
                                case "Driver_FaultsReport":
                                    {
                                        dataset = ReportDataSetLoader.Get_Driver_FaultsReport(dataBlockId, from, to);
                                    } break;
                            }
                        } break;
                    #endregion

                    #region "Vehicle Reports"
                    case "VehiclesReport":
                        {
                            List<int> dataBlockIDS = new List<int>();
                            if (Selected_PlfReportsDataGrid_Index.Value == null)
                                throw new Exception("Выберите тип отчета!");
                            string VehiclesReportType = Selected_PlfReportsDataGrid_Index.Value;
                            dataBlockIDS = getIdList();
                            dataBlockId = dataBlockIDS[0];
                            dataBlock.OpenConnection();
                            string VIN = dataBlock.vehicleUnitInfo.Get_VehicleOverview_IdentificationNumber(dataBlockId).ToString();
                            string RegNumb = dataBlock.vehicleUnitInfo.Get_VehicleOverview_RegistrationIdentification(dataBlockId).vehicleRegistrationNumber.ToString();
                            int vehicleId = dataBlock.vehiclesTables.GetVehicleId_byVinRegNumbers(VIN, RegNumb);
                            int cardId = dataBlock.cardsTable.GetCardId(RegNumb, VIN, dataBlock.cardsTable.vehicleCardTypeId);
                            dataBlock.CloseConnection();
                            switch (VehiclesReportType)
                            {
                                case "Vehicle_DriverOverSpeeding":
                                    {
                                        dataset = ReportDataSetLoader.Get_VehicleEventsAndFaults_VuOverSpeedingEventData(vehicleId, dataBlockIDS, from, to, userId);
                                        LoadStiReport(dataset, "Reports/VehiclesReports/VehiclesOverSpeeding.mrt");
                                        return;
                                    } break;
                                case "Vehicle_DailyVehicleActivityProtocol":
                                    {
                                        dataset = ReportDataSetLoader.Get_Vehicle_ActivityReport(vehicleId, dataBlockIDS, from, to, userId);
                                        LoadStiReport(dataset, "Reports/VehiclesReports/DailyVehicleActivityProtocol.mrt");
                                        return;
                                    } break;
                            }
                        } break;
                  
                    #endregion

                    #region "MultiDriversReport"
                    case "MultiDriversReport":
                        {
                            List<int> dataBlockIDS = new List<int>();
                            //int cardId = dataBlock.cardsTable.GetCardId(DriversNameLabel.Text);
                            if (Selected_PlfReportsDataGrid_Index.Value == null)
                                throw new Exception("Выберите тип отчета!");
                            string DriversReportType = Selected_PlfReportsDataGrid_Index.Value;
                            dataBlockIDS = new List<int>();
                            List<int> DriversCardsIds = new List<int>();
                            List<int> geteedIds = new List<int>();
                            foreach (ListItem item in MultiDrivers.Items)
                            {
                                geteedIds = getIdList(item.Value);
                                if (geteedIds != null)
                                {
                                    dataBlockIDS.AddRange(getIdList(item.Value));
                                    DriversCardsIds.Add(dataBlock.cardsTable.GetCardId(item.Text, dataBlock.cardsTable.driversCardTypeId));
                                }
                            }
                            //dataBlockId = dataBlockIDS[0]; 


                            switch (DriversReportType)
                            {
                                case "MultiDrivers_ActivitySummary":
                                    {
                                        dataset = ReportDataSetLoader.Get_MultiDrivers_ActivitySummary(dataBlockIDS, DriversCardsIds, from, to, userId);
                                        LoadStiReport(dataset, "Reports/MultiDriversReports/DriverActivitySummary.mrt");
                                    } break;
                            }
                        } break;
                    #endregion                   

                    #region "PLFReport"
                    case "PLFReport":
                        {

                            List<PLFUnit.PLFRecord> records = new List<PLFUnit.PLFRecord>();
                            List<int> dataBlockIDS = new List<int>();
                            int cardId = -1;
                            string PlfReportType;
                            if (Selected_PlfReportsDataGrid_Index.Value == null)
                                // throw new Exception("Выберите тип отчета!");
                                PlfReportType = "Выберите тип отчета";
                            else
                                PlfReportType = Selected_PlfReportsDataGrid_Index.Value;
                            
                            dataBlockIDS = getIdList();
                            if (PLFTreeView.SelectedNode.Parent == null)
                                throw noSelectedDriver;
                            else
                                if (PLFTreeView.SelectedNode.Parent.Parent == null)
                                    cardId = Convert.ToInt32(PLFTreeView.SelectedNode.Parent.Value);
                                else
                                    if (PLFTreeView.SelectedNode.Parent.Parent.Parent == null)
                                        cardId = Convert.ToInt32(PLFTreeView.SelectedNode.Parent.Parent.Value);

                            switch (PlfReportType)
                            {
                                case "Полный отчет за календарный период":
                                    {
                                        dataset = ReportDataSetLoader.Get_PLF_ALLData(dataBlockIDS, from, to, cardId, userId, ref records);
                                        DataTable dt = dataset.Tables[0];

                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            string value=dr["С"].ToString();
                                        }

                                        // step 1: creation of a document-object
                                        iTextSharp.text.Document document = new iTextSharp.text.Document();

                                        // step 2:
                                        // we create a writer that listens to the document
                                        // and directs a PDF-stream to a file
                                        iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream("Chap0101.pdf", FileMode.Create));

                                        // step 3: we open the document
                                        document.Open();

                                        // step 4: we add a paragraph to the document
                                        document.Add(new iTextSharp.text.Paragraph("Hello World"));

                                        // step 5: we close the document
                                        document.Close();

                                        LoadStiReport(dataset, "Reports/PLFReports/PlfFullCalendarReport.mrt");
                                    } break;
                                case "ТС по дням":
                                    {
                                        string filePath;
                                        //PHOTO
                                        //int vehInfoId = dataBlock.vehiclesTables.GetVehicleInfoNameId(DataBaseReference.VehiclePhotoAddress);
                                        //string filePath = dataBlock.vehiclesTables.GetVehicleInfoValue(selectedVehID, vehInfoId);
                                        //if (filePath == "")
                                        filePath = "~/images/unknown_vehicle.jpg";
                                        dataset = ReportDataSetLoader.Get_PlfUnitsByDays(dataBlockIDS, from, to, cardId, userId, ref records, filePath);
                                        LoadStiReport(dataset, "Reports/PLFReports/PlfUnitsByDays.mrt");
                                    }
                                    break;
                                case "Эффективность использования автопарка":
                                    {
                                        dataset = ReportDataSetLoader.Get_PlfUnitsEficienty(dataBlockIDS, from, to, cardId, userId, ref records);
                                        LoadStiReport(dataset, "Reports/PLFReports/PlfUnitsEficienty.mrt");
                                    }
                                    break;
                                case "Отчет за период":
                                    {
                                        throw noReportException;
                                    } break;
                                case "Не показывать таблицы":
                                    {
                                        dataset = ReportDataSetLoader.Get_PLF_ALLData(dataBlockIDS, from, to, cardId, userId, ref records);
                                    } break;
                                default:
                                    {
                                        dataset = ReportDataSetLoader.Get_PLF_ALLData(dataBlockIDS, from, to, cardId, userId, ref records);
                                    } break;
                            }
                            ChartLiteral.Text = "";
                            if (records.Count != 0)
                            {
                                //  ShowGraphClick(records);
                                if (ChartsCheckBoxList.SelectedValue != null && ChartsCheckBoxList.SelectedValue != "")
                                {
                                    //Для теста приближения, не забыть убрать потом!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                    //for (int i = 0; i < records.Count; i++)
                                    //{
                                    //    records[i].FUEL_COUNTER = records[i].FUEL_VOLUME1;
                                    //}
                                    //PLFUnit.PLFUnitClass.FFT_Fuel(ref records);
                                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                    AnycahrtPanel.Visible = true;
                                    string xmlName = "";
                                    string mapPath = Server.MapPath("");
                                    xmlName = AnyChartStockXml.WriteToServersTempFile(AnyChartShow(records), mapPath);

                                    MapPath_HiddenFieldForDeleteingXml.Value = mapPath;
                                    AnyChartHiddenField.Value = xmlName;
                                    //ChartUpdatePanel.Update();
                                    charts = true;
                                }
                                else
                                    AnycahrtPanel.Visible = false;
                            }
                        } break;
                    #endregion

                    default: throw new Exception("Выберите тип отчета!");
                }
            }
            Export_Button.Enabled = true;
            Email_Button.Enabled = true;
            Print_Button.Enabled = true;

            DecisionUpdatePanel.Update();
            OutputUpdatePanel.Update();  
        }
        catch (Exception ex)
        {
            resultActionLabel.Text = ex.Message;
            //StateFullPanel.Visible = false;
        }
        finally
        {            
            ModalPopupExtender1.Hide();
        }
    }*/

    /*private void LoadStiReport(DataSet dataset, string Path)
    {
        NavigationReportControl1.Visible = true;
        NavigationReportControl1.SetReportData(dataset, Path);
    }*/

    /*private Stimulsoft.Report.StiExportFormat getExportFormat()
    {
        Stimulsoft.Report.StiExportFormat exportFormat;
        switch (ReportsExport_RadioButtonList_SelectedValHiddenField.Value)
        {
            case "PDF":
                { exportFormat = Stimulsoft.Report.StiExportFormat.Pdf; } break;
            case "CSV":
                { exportFormat = Stimulsoft.Report.StiExportFormat.Csv; } break;
            case "Excel":
                { exportFormat = Stimulsoft.Report.StiExportFormat.Excel; } break;
            case "Rtf":
                { exportFormat = Stimulsoft.Report.StiExportFormat.Rtf; } break;
            case "Txt":
                { exportFormat = Stimulsoft.Report.StiExportFormat.Text; } break;
            case "Jpg":
                { exportFormat = Stimulsoft.Report.StiExportFormat.ImageJpeg; } break;
            default:
                { exportFormat = Stimulsoft.Report.StiExportFormat.Text; } break;
        }
        return exportFormat;
    }*/

    /*protected void ExportButton_Click(object s, EventArgs e)
    {
        try
        {

            Stimulsoft.Report.StiExportFormat exportFormat;
            exportFormat = getExportFormat();
            Stimulsoft.Report.StiPagesRange pagesRange = GetPagesRange(NavigationReportControl1.GetCurrentPage(), 100);
            NavigationReportControl1.ExportReport(exportFormat, pagesRange);
        }
        catch (Exception ex)
        {
            resultActionLabel.Text = ex.Message;
            HiddenFieldUpdatePanel.Update();
        }
    }*/

    /*public Stimulsoft.Report.StiPagesRange GetPagesRange(int reportCurrentPage, int reportPagesCount)
    {
        Stimulsoft.Report.StiPagesRange pagesRange = new Stimulsoft.Report.StiPagesRange();
        bool shit = CustomPages_RadioButton.Checked;
        if (PagesExportsOption_hiddenField.Value == "AllPages")
        {
            pagesRange = Stimulsoft.Report.StiPagesRange.All;
        }
        else
            if (PagesExportsOption_hiddenField.Value == "CurrentPage")
            {
                pagesRange = new Stimulsoft.Report.StiPagesRange(reportCurrentPage);
            }
            else
                if (PagesExportsOption_hiddenField.Value == "CustomPages")
                {
                    pagesRange = new Stimulsoft.Report.StiPagesRange(CustomPagesValue_hiddenField.Value);
                }

        return pagesRange;
    }*/

    /*protected void SendReportByEmail(object sender, EventArgs e)
    {
        try
        {
            DateTime from = new DateTime();
            DateTime to = new DateTime();
            from = getDate("fromCalendarDate");
            to = getDate("toCalendarDate");
            string mailTO = EmailAddress_HiddenField.Value;
            string mailSubject = from.ToShortDateString() + " - " + to.ToShortDateString() + "  " + DriversNameLabel.Text;
            string mailBody = "Файл в приложении. Smartfis.ru";
            string attachmentName = "(" + from.ToShortDateString() + "-" + to.ToShortDateString() + ")" + DriversNameLabel.Text.Replace(" ", "");

            //mailTO = "twoblackniggaz@mail.ru";

            Stimulsoft.Report.StiExportFormat exportFormat;
            exportFormat = getExportFormat();
            resultActionLabel.Text = NavigationReportControl1.SendReportByEmail(exportFormat, mailTO, mailSubject, mailBody, attachmentName);
            HiddenFieldUpdatePanel.Update();

        }
        catch (Exception ex)
        {
            resultActionLabel.Text = ex.Message;
            HiddenFieldUpdatePanel.Update();
        }
    }*/

    /*protected void VehTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        resultActionLabel.Text = "";
        ChartsCheckBoxList.Visible = false;
        AnycahrtPanel.Visible = false;
        NavigationReportControl1.Visible = false;

        //Делаем неактивными нижние кнопки, если отчет невидимый
        Export_Button.Enabled = false;
        Email_Button.Enabled = false;
        Print_Button.Enabled = false;

        DecisionUpdatePanel.Update();
        ///////////////////////////////////

        PLFTreeView.CollapseAll();
        DriversTreeView.CollapseAll();

        try
        {
            AccordionSelectedPane.Value = "1";
            LoadVehiclesReports();

            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            DataTable dataTable = new DataTable();

            ReportNameLabel.Text = "Выберите отчет";
            DriversNameSourceLabel.Text = "ТС: ";
            HiddenField.Value = VehiclesTreeView.SelectedNode.Value;
            if (VehiclesTreeView.SelectedNode.Parent == null)
                DriversNameLabel.Text = VehiclesTreeView.SelectedNode.Text;
            if (VehiclesTreeView.SelectedNode.ChildNodes.Count == 0)
                DriversNameLabel.Text = VehiclesTreeView.SelectedNode.Parent.Text;

            List<int> vehiclesIds = new List<int>();
            vehiclesIds = getIdList();
            List<DateTime> startDateList = new List<DateTime>();
            List<DateTime> endDateList = new List<DateTime>();
            List<DateTime> period = new List<DateTime>();
            //   PlfFilesList.Text = "Загруженные файлы:" + Environment.NewLine;

            foreach (int id in vehiclesIds)
            {
                period = dataBlock.vehicleUnitInfo.Get_StartEndPeriod(id);
                startDateList.Add(period[0]);
                endDateList.Add(period[1]);
                //       PlfFilesList.Text += "(" + startDateList[startDateList.Count - 1] + " - "
                //           + endDateList[startDateList.Count - 1] + ")" + Environment.NewLine;
            }

            if (startDateList.Count == 0 || endDateList.Count == 0)
                throw new Exception("Ни одной карты не загружено!");

            startDateList.Sort();
            endDateList.Sort();

            CalendarFromExtender.SelectedDate = startDateList[0];
            CalendarToExtender.SelectedDate = endDateList[endDateList.Count - 1];
         
        }
        catch (Exception ex)
        {
            resultActionLabel.Text = "Произошла ошибка: " + ex.Message;
        }
        finally
        {
            HiddenFieldUpdatePanel.Update();
            ChoisesUpdatePanel.Update();
            ChartsCheckBoxListUpdatePanel.Update();
            //ReportUpdatePanel.Update();
            OutputUpdatePanel.Update();
        }
    }*/

    /*protected void PLFTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        ChartsCheckBoxList.Visible = false;
        resultActionLabel.Text = "";

        //Делаем неактивными нижние кнопки, если отчет невидимый
        Export_Button.Enabled = false;
        Email_Button.Enabled = false;
        Print_Button.Enabled = false;

        DecisionUpdatePanel.Update();
        ///////////////////////////////////

        VehiclesTreeView.CollapseAll();
        DriversTreeView.CollapseAll();

        try
        {
            if (PLFTreeView.SelectedNode.Parent != null)
            {
                LoadPlfReports();
                AccordionSelectedPane.Value = "4";
                DriversNameSourceLabel.Text = "Датчики: ";

                string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
                DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
                HiddenField.Value = PLFTreeView.SelectedNode.Value;
                if (PLFTreeView.SelectedNode.ChildNodes.Count != 0)
                    DriversNameLabel.Text = PLFTreeView.SelectedNode.Text;
                else
                {
                    DriversNameLabel.Text = PLFTreeView.SelectedNode.Parent.Text + " - " + PLFTreeView.SelectedNode.Text;
                }


                List<int> plfIds = new List<int>();
                plfIds = getIdList();
                List<DateTime> startDateList = new List<DateTime>();
                List<DateTime> endDateList = new List<DateTime>();
                PlfFilesList.Text = "<b>Загруженные файлы:</b>";
                dataBlock.OpenConnection();
                foreach (int id in plfIds)
                {
                    startDateList.Add(dataBlock.plfUnitInfo.Get_START_PERIOD(id));
                    endDateList.Add(dataBlock.plfUnitInfo.Get_END_PERIOD(id));
                    PlfFilesList.Text += "<p>(" + startDateList[startDateList.Count - 1] + " - "
                        + endDateList[startDateList.Count - 1] + ")</p>";
                }

                if (startDateList.Count == 0 || endDateList.Count == 0)
                    throw new Exception("В этой карточке нет ниодной записи!");

                startDateList.Sort();
                endDateList.Sort();

                CalendarFromExtender.SelectedDate = startDateList[0];
                CalendarToExtender.SelectedDate = endDateList[endDateList.Count - 1];                
                //ReportNameLabel.Text = "Общие сведения";

                ChartsCheckBoxList.Visible = true;
                ChartsCheckBoxList.Items.Clear();

                PLFUnit.PLFRecord installedSensors = new PLFUnit.PLFRecord();
                installedSensors = dataBlock.plfUnitInfo.Get_InstalledSensors(plfIds[0]);
                dataBlock.CloseConnection();

                if (installedSensors.FUEL_VOLUME1 != null)
                    ChartsCheckBoxList.Items.Add(new ListItem("Уровень топлива 1", "FuelVolume"));
                if (installedSensors.VOLTAGE != null)
                    ChartsCheckBoxList.Items.Add(new ListItem("Напряжение", "Voltage"));
                if (installedSensors.ENGINE_RPM != null)
                    ChartsCheckBoxList.Items.Add(new ListItem("RPM", "ENGINE_RPM"));
                if (installedSensors.SPEED != null)
                    ChartsCheckBoxList.Items.Add(new ListItem("Скорость", "SPEED"));
                if (installedSensors.TEMPERATURE1 != null)
                    ChartsCheckBoxList.Items.Add(new ListItem("Температура 1", "Temperature1"));
                if (installedSensors.FUEL_CONSUMPTION != null)
                    ChartsCheckBoxList.Items.Add(new ListItem("Расход топлива", "FuelConsumption"));
                if (installedSensors.FUEL_COUNTER != null)
                    ChartsCheckBoxList.Items.Add(new ListItem("Счетчик топлива", "FuelCounter"));
                if (installedSensors.WEIGHT1 != null)
                    ChartsCheckBoxList.Items.Add(new ListItem("Вес 1", "Weight1"));
                AnycahrtPanel.Visible = false;
                NavigationReportControl1.Visible = false;

                Selected_PlfReportsDataGrid_Index.Value = "";
                ReportNameLabel.Text = "Выберите отчет";

                HiddenFieldUpdatePanel.Update();
                ChoisesUpdatePanel.Update();
                ChartsCheckBoxListUpdatePanel.Update();
                //ReportUpdatePanel.Update();
                OutputUpdatePanel.Update();
            }
        }
        catch (Exception ex)
        {
            resultActionLabel.Text = ex.Message;
        }
    }*/

    /*private string MultiDriversCharts(List<List<int>> data, List<string> driverNames, string ChartName)
    {
        List<int> dataBlockIds = new List<int>();
        dataBlockIds = getIdList();

        ChartColors colors = new ChartColors();
        string strXML;
        strXML = "<graph caption='" + ChartName + "' xAxisName='Водители' yAxisName='Часы' canvasBgColor='F6DFD9' canvasBaseColor='FE6E54' hovercapbgColor='FFECAA' hovercapborder='F47E00' divlinecolor='F47E00' limitsDecimalPrecision='0' divLineDecimalPrecision='0'>";
        string color;
        strXML += "<categories>"
            + "<category name='Время вождения'/>"
            + "<category name='Время активности'/>"
            + "<category name='Время пассивности'/>"
            + "<category name='Время отдыха'/>"
            + "</categories>";

        int i = 0;
        string driverName;
        foreach (List<int> driver in data)
        {
            color = colors.getFCColor_Iteration();
            driverName = driverNames[i++];
            strXML += "<dataset seriesname='" + driverName + "' color='" + color + "' showValue='1'>";
            foreach (int record in driver)
            {
                strXML += "<set value='" + record.ToString() + "' />";
            }
            strXML += "</dataset>";            
        }

        //Close <graph> element
        strXML += "</graph>";
        string chartId = "product" + new Random().Next() + new Random().Next();
        //Create the chart - Column 3D Chart with data contained in strXML
        return FusionCharts.RenderChart("../FusionCharts/FCF_MSColumn3D.swf", "", strXML, chartId, "800", "400", false, false);
    }*/

    /*private void compareMultiDates(ref DateTime from, ref DateTime to, DateTime fromComp, DateTime toComp)
    {
        try
        {
            if (fromComp < from)
                from = fromComp;
            if (toComp > to)
                to = toComp;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }*/

    /*private List<int> getIdList()
    {
        List<int> dataBlockIds = new List<int>();
        try
        {
            if (HiddenField.Value == null || HiddenField.Value == "")
                return null;

            string[] splitedStr = HiddenField.Value.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in splitedStr)
                dataBlockIds.Add(Convert.ToInt32(str));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataBlockIds;
    }*/

    /*private List<int> getIdList(string input)
    {
        List<int> dataBlockIds = new List<int>();
        try
        {
            if (input == null || input == "")
                return null;

            string[] splitedStr = input.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in splitedStr)
                dataBlockIds.Add(Convert.ToInt32(str));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dataBlockIds;
    }*/

    /*protected void MultiDrivers_SelectedIndexChanged(object sender, EventArgs e)
    {
        resultActionLabel.Text = "";
        ChartsCheckBoxList.Visible = false;
        AnycahrtPanel.Visible = false;
        NavigationReportControl1.Visible = false;

        //Делаем неактивными нижние кнопки, если отчет невидимый
        Export_Button.Enabled = false;
        Email_Button.Enabled = false;
        Print_Button.Enabled = false;

        DecisionUpdatePanel.Update();
        ///////////////////////////////////

        try
        {
            AccordionSelectedPane.Value = "2";
            LoadMultiDriversReports();

            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            DataTable dataTable = new DataTable();
            HiddenField.Value = "";
            foreach (ListItem item in MultiDrivers.Items)
            {
                if (item.Selected == true)
                    HiddenField.Value += item.Value + " ";
            }
            DriversNameLabel.Text = "Группа водителей";

            List<int> driverIds = new List<int>();
            driverIds = getIdList();
            if (driverIds == null)
                throw new Exception("");
            List<DateTime> startDateList = new List<DateTime>();
            List<DateTime> endDateList = new List<DateTime>();
            List<DateTime> period = new List<DateTime>();
            //   PlfFilesList.Text = "Загруженные файлы:" + Environment.NewLine;
            foreach (int id in driverIds)
            {
                period = dataBlock.cardUnitInfo.Get_StartEndPeriod(id);
                startDateList.Add(period[0]);
                endDateList.Add(period[1]);
            }

            if (startDateList.Count == 0 || endDateList.Count == 0)
                throw new Exception("Ни одной карты не загружено!");

            startDateList.Sort();
            endDateList.Sort();

            CalendarFromExtender.SelectedDate = startDateList[0];
            CalendarToExtender.SelectedDate = endDateList[endDateList.Count - 1];
        }
        catch (Exception ex)
        {
            resultActionLabel.Text = ex.Message;
        }
        finally
        {
            HiddenFieldUpdatePanel.Update();
            ChoisesUpdatePanel.Update();
            ChartsCheckBoxListUpdatePanel.Update();
            ReportUpdatePanel.Update();
        }
    }*/

    /*protected void SelectAllMultiDrivers(object sender, EventArgs e)
    {
        bool setSelected = true;

        if (MultiDriversSelectAllDrivers.Checked == false)
            setSelected = false;

        for (int i = 0; i < MultiDrivers.Items.Count; i++)
        {
            MultiDrivers.Items[i].Selected = setSelected;
        }

        MultiDrivers_SelectedIndexChanged(sender, e);
    }*/

    /*protected void DriverSearchMade(object sender, EventArgs e)
    {
        DriversTreeView.CollapseAll();
        if (FindNodeByValue(DriversTreeView.Nodes, drSearch.SelectedItem.Text))
        {
            DriversTreeView_SelectedNodeChanged(null, null);
        }
    }*/

    /*private bool FindNodeByValue(TreeNodeCollection n, string name)
    {

        for (int i = 0; i < n.Count; i++)
        {
            if (n[i].Text.Contains(name))
            {
                n[i].Select();
                n[i].Expand();
                return true;
            }
        }
        return false;
    }*/

    /*protected void PlfDriverSearchMade(object sender, EventArgs e)
    {
        PLFTreeView.CollapseAll();
        if (FindNodeByValue(PLFTreeView.Nodes, plfDrSearch.SelectedItem.Text))
        {
            PLFTree_SelectedNodeChanged(null, null);
        }
    }*/

    /*protected void ChartsCheckBoxList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (false)
        {
            foreach (ListItem item in ChartsCheckBoxList.Items)
            {
                item.Enabled = true;
            }
            for (int i = 0; i < ChartsCheckBoxList.Items.Count; i++)
                if (ChartsCheckBoxList.Items[i].Selected)
                {
                    switch (ChartsCheckBoxList.Items[i].Value)
                    {
                        case "Voltage_RPM":
                            {
                                ChartsCheckBoxList.Items.FindByValue("ENGINE_RPM").Enabled = false;
                                ChartsCheckBoxList.Items.FindByValue("Voltage").Enabled = false;
                            } break;
                        case "Speed_RPM":
                            {
                                ChartsCheckBoxList.Items.FindByValue("SPEED").Enabled = false;
                                ChartsCheckBoxList.Items.FindByValue("ENGINE_RPM").Enabled = false;
                            } break;
                        case "Speed_RPM_Voltage":
                            {
                                ChartsCheckBoxList.Items.FindByValue("SPEED").Enabled = false;
                                ChartsCheckBoxList.Items.FindByValue("ENGINE_RPM").Enabled = false;
                                ChartsCheckBoxList.Items.FindByValue("Voltage").Enabled = false;
                                ChartsCheckBoxList.Items.FindByValue("Voltage_RPM").Enabled = false;
                                ChartsCheckBoxList.Items.FindByValue("Speed_RPM").Enabled = false;
                            } break;
                    }
                }
        }
    }*/

    /*private DateTime getDate(string textBoxName)
    {
        DateTime createdDateTime = new DateTime();
        try
        {
            string textDate = ((TextBox)periodCalendarPanel.FindControl(textBoxName)).Text;
            createdDateTime = DateTime.Parse(textDate);
           // createdDateTime = createdDateTime.AddMinutes(1);
        }
        catch
        {
            throw new Exception("Укажите временной интервал");
        }
        return createdDateTime;
    }*/

    /*void ShowGraphClick(List<PLFUnit.PLFRecord> plfTable)
    {
        try
        {
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

            int graphCount = 0;
            int drawedAllreadyCount = 0;
            int width = 850;
            for (int i = 0; i < ChartsCheckBoxList.Items.Count; i++)
                if (ChartsCheckBoxList.Items[i].Selected && ChartsCheckBoxList.Items[i].Enabled == true)
                    graphCount++;

            ChartLiteral.Text = "<table>";
            if (graphCount > 1)
            {
                width = width / 2;                
            }         
           // resultActionLabel.Text = "Максимальное колл-во точек на график - " + (Session["maxRecordsCount"]).ToString()+Environment.NewLine;
            
            ChartColors colors = new ChartColors();
            List<DateTime> date = new List<DateTime>();
            for (int i = 0; i < ChartsCheckBoxList.Items.Count; i++)
            {
                if (ChartsCheckBoxList.Items[i].Selected && ChartsCheckBoxList.Items[i].Enabled == true)
                {
                    #region "FuelVolume"
                    if (ChartsCheckBoxList.Items[i].Value == "FuelVolume")
                    {
                        List<int> FuelLevel = new List<int>();
                        date = new List<DateTime>();
                        foreach (PLFUnit.PLFRecord record in plfTable)
                        {
                            FuelLevel.Add(Convert.ToInt32(record.FUEL_VOLUME1.Split('.')[0]));
                            date.Add(record.SYSTEM_TIME.GetSystemTime());
                        }
                        if (drawedAllreadyCount % 2 == 0)
                            ChartLiteral.Text += "<tr>";
                        ChartLiteral.Text += "<td>";
                        ChartLiteral.Text += " " + CreateChart(FuelLevel, date, ChartsCheckBoxList.Items[i].Text, "Литры", "", width, colors.getFCColor_Iteration());
                        //ChartLiteral.Text += " " + CreateAnyChart(FuelLevel, date, ChartsCheckBoxList.Items[i].Text, "Литры", "", width, colors.getFCColor_Iteration());
                        ChartLiteral.Text += "</td>";                       
                        if (drawedAllreadyCount % 2 != 0)
                            ChartLiteral.Text += "</tr>";
                        drawedAllreadyCount++;
                    }
                    #endregion
                    #region "Voltage"
                    if (ChartsCheckBoxList.Items[i].Value == "Voltage")
                    {
                        List<int> Voltage = new List<int>();
                        date = new List<DateTime>();
                        foreach (PLFUnit.PLFRecord record in plfTable)
                        {
                            Voltage.Add(Convert.ToInt32(record.VOLTAGE.Split('.')[0]));
                            date.Add(record.SYSTEM_TIME.GetSystemTime());
                        }
                        if (drawedAllreadyCount % 2 == 0)
                            ChartLiteral.Text += "<tr>";
                        ChartLiteral.Text += "<td>";
                        ChartLiteral.Text += " " + CreateChart(Voltage, date, ChartsCheckBoxList.Items[i].Text, "Вольты", "", width, colors.getFCColor_Iteration());
                        ChartLiteral.Text += "</td>";
                        if (drawedAllreadyCount % 2 != 0)
                            ChartLiteral.Text += "</tr>";
                        drawedAllreadyCount++;
                    }
                    #endregion
                    #region "ENGINE_RPM"
                    if (ChartsCheckBoxList.Items[i].Value == "ENGINE_RPM")
                    {
                        List<int> ENGINE_RPM = new List<int>();
                        date = new List<DateTime>();
                        foreach (PLFUnit.PLFRecord record in plfTable)
                        {
                            ENGINE_RPM.Add(Convert.ToInt32(record.ENGINE_RPM.Split('.')[0]));
                            date.Add(record.SYSTEM_TIME.GetSystemTime());
                        }
                        if (drawedAllreadyCount % 2 == 0)
                            ChartLiteral.Text += "<tr>";
                        ChartLiteral.Text += "<td>";
                        ChartLiteral.Text += " " + CreateChart(ENGINE_RPM, date, ChartsCheckBoxList.Items[i].Text, "Об/мин", "", width, colors.getFCColor_Iteration());
                        ChartLiteral.Text += "</td>";
                        if (drawedAllreadyCount % 2 != 0)
                            ChartLiteral.Text += "</tr>";
                        drawedAllreadyCount++;
                    }
                    #endregion
                    #region "SPEED"
                    if (ChartsCheckBoxList.Items[i].Value == "SPEED")
                    {
                        List<int> SPEED = new List<int>();
                        date = new List<DateTime>();
                        foreach (PLFUnit.PLFRecord record in plfTable)
                        {
                            SPEED.Add(Convert.ToInt32(record.SPEED.Split('.')[0]));
                            date.Add(record.SYSTEM_TIME.GetSystemTime());
                        }
                        if (drawedAllreadyCount % 2 == 0)
                            ChartLiteral.Text += "<tr>";
                        ChartLiteral.Text += "<td>";
                        ChartLiteral.Text += " " + CreateChart(SPEED, date, ChartsCheckBoxList.Items[i].Text, "Км/ч", "", width, colors.getFCColor_Iteration());
                        ChartLiteral.Text += "</td>";
                        if (drawedAllreadyCount % 2 != 0)
                            ChartLiteral.Text += "</tr>";
                        drawedAllreadyCount++;
                    }
                    #endregion
                    #region "Voltage_RPM"
                    if (ChartsCheckBoxList.Items[i].Value == "Voltage_RPM")
                    {
                        List<int> VOLTAGE = new List<int>();
                        List<int> RPM = new List<int>();
                        List<string> yNames = new List<string>();
                        List<string> colorsList = new List<string>();
                        List<List<int>> parameters = new List<List<int>>();
                        date = new List<DateTime>();
                        foreach (PLFUnit.PLFRecord record in plfTable)
                        {
                            VOLTAGE.Add(Convert.ToInt32(record.VOLTAGE.Split('.')[0]));
                            RPM.Add(Convert.ToInt32(record.ENGINE_RPM.Split('.')[0]));
                            date.Add(record.SYSTEM_TIME.GetSystemTime());
                        }
                        parameters.Add(VOLTAGE);
                        parameters.Add(RPM);
                        yNames.Add("Вольты");
                        yNames.Add("RPM");
                        colorsList.Add(colors.getFCColor_Iteration());
                        colorsList.Add(colors.getFCColor_Iteration());
                        if (drawedAllreadyCount % 2 == 0)
                            ChartLiteral.Text += "<tr>";
                        ChartLiteral.Text += "<td>";
                        ChartLiteral.Text += " " + MultiAxisChart(parameters, date, ChartsCheckBoxList.Items[i].Text, yNames, "", width, colorsList);
                        ChartLiteral.Text += "</td>";
                        if (drawedAllreadyCount % 2 != 0)
                            ChartLiteral.Text += "</tr>";
                        drawedAllreadyCount++;
                    }
                    #endregion
                    #region "Voltage_RPM"
                    if (ChartsCheckBoxList.Items[i].Value == "Speed_RPM")
                    {
                        List<int> SPEED = new List<int>();
                        List<int> RPM = new List<int>();
                        List<string> yNames = new List<string>();
                        List<string> colorsList = new List<string>();
                        List<List<int>> parameters = new List<List<int>>();
                        date = new List<DateTime>();
                        foreach (PLFUnit.PLFRecord record in plfTable)
                        {
                            SPEED.Add(Convert.ToInt32(record.SPEED.Split('.')[0]));
                            RPM.Add(Convert.ToInt32(record.ENGINE_RPM.Split('.')[0]));
                            date.Add(record.SYSTEM_TIME.GetSystemTime());
                        }
                        parameters.Add(SPEED);
                        parameters.Add(RPM);
                        yNames.Add("КМ/Ч");
                        yNames.Add("RPM");
                        colorsList.Add(colors.getFCColor_Iteration());
                        colorsList.Add(colors.getFCColor_Iteration());
                        if (drawedAllreadyCount % 2 == 0)
                            ChartLiteral.Text += "<tr>";
                        ChartLiteral.Text += "<td>";
                        ChartLiteral.Text += " " + MultiAxisChart(parameters, date, ChartsCheckBoxList.Items[i].Text, yNames, "", width, colorsList);
                        ChartLiteral.Text += "</td>";
                        if (drawedAllreadyCount % 2 != 0)
                            ChartLiteral.Text += "</tr>";
                        drawedAllreadyCount++;
                    }
                    #endregion
                    #region "Speed_RPM_Voltage"
                    if (ChartsCheckBoxList.Items[i].Value == "Speed_RPM_Voltage")
                    {
                        List<int> VOLTAGE = new List<int>();
                        List<int> RPM = new List<int>();
                        List<int> SPEED = new List<int>();
                        List<string> yNames = new List<string>();
                        List<string> colorsList = new List<string>();
                        List<List<int>> parameters = new List<List<int>>();
                        date = new List<DateTime>();
                        foreach (PLFUnit.PLFRecord record in plfTable)
                        {
                            VOLTAGE.Add(Convert.ToInt32(record.VOLTAGE.Split('.')[0]));
                            RPM.Add(Convert.ToInt32(record.ENGINE_RPM.Split('.')[0]));
                            SPEED.Add(Convert.ToInt32(record.SPEED.Split('.')[0]));
                            date.Add(record.SYSTEM_TIME.GetSystemTime());
                        }
                        parameters.Add(RPM);
                        parameters.Add(VOLTAGE);
                        parameters.Add(SPEED);
                        yNames.Add("RPM");
                        yNames.Add("Вольты");
                        yNames.Add("КМ/Ч");
                        colorsList.Add(colors.getFCColor_Iteration());
                        colorsList.Add(colors.getFCColor_Iteration());
                        colorsList.Add(colors.getFCColor_Iteration());
                        if (drawedAllreadyCount % 2 == 0)
                            ChartLiteral.Text += "<tr>";
                        ChartLiteral.Text += "<td>";
                        ChartLiteral.Text += " " + MultiAxisChart(parameters, date, ChartsCheckBoxList.Items[i].Text, yNames, "", width, colorsList);
                        ChartLiteral.Text += "</td>";
                        if (drawedAllreadyCount % 2 != 0)
                            ChartLiteral.Text += "</tr>";
                        drawedAllreadyCount++;
                    }
                    #endregion
                }
            }
            if (drawedAllreadyCount % 2 != 0)
                ChartLiteral.Text += "</tr>";
            ChartLiteral.Text += "</table>";
        }
        catch (Exception ex)
        {
            resultActionLabel.Text = ex.Message;
        }
    }*/

    /*private string AnyChartShow(List<PLFUnit.PLFRecord> plfTable)
    {
        List<string> selectedCharts = new List<string>();
        for (int i = 0; i < ChartsCheckBoxList.Items.Count; i++)
        {
            if (ChartsCheckBoxList.Items[i].Selected && ChartsCheckBoxList.Items[i].Enabled == true)
            {
                selectedCharts.Add(ChartsCheckBoxList.Items[i].Value);
            }
        }
        return AnyChartStockXml.AnyChartMultiYChart(plfTable, selectedCharts);
    }*/

    /*private string CreateChart(List<int> arrayInput, List<DateTime> date, string name, string xName, string yName, int width, string color )
    {
        List<int> array = new List<int>();
        
        int maxRecordsCount = Convert.ToInt32(Session["maxRecordsCount"]);
        if (maxRecordsCount <= 0)
        {
            maxRecordsCount = 1500;
            Session["maxRecordsCount"] = maxRecordsCount;
        }
        int step = -1;
        if (arrayInput.Count > maxRecordsCount)
        {
            step = arrayInput.Count / maxRecordsCount;
            array = ApllyStepsToArray(arrayInput, step);
        }
        else
            array = arrayInput;
        if (step > 0)
            date = ApllyStepsToArray(date, step);
        string strXML;
        int i = 0;
        int maxdateCount = 20;
        if (width < 600)
            maxdateCount = 11;
        int labelStep = date.Count / maxdateCount;//maxDateCount
        int j = 0;
        bool hasValues = false;
        foreach (int a in array)
        {
            if (a > 0)
            {
                hasValues = true;
                break;
            }
        }
        string anchorAlpha = "0";
        //if (EveryDayPlf.Checked == true)
        //    anchorAlpha = "100";
        if (hasValues)
        {
            strXML = "<graph caption='" + name + "' labelStep='" + labelStep.ToString() + "' lineColor='" + color + "' anchorAlpha='" + anchorAlpha + "' lineThickness='2' yAxisName='" + xName + "' xAxisName='" + yName + "' numberPrefix='' showValues='0' rotateNames='1' formatNumberScale='0' decimalPrecision='0'>";
            foreach (int element in array)
            {
                    strXML += "<set name='" + date[i].ToShortDateString() + ' ' + date[i++].ToShortTimeString() + "' value='" + element + "'/>";
            }
            //Close <graph> element
            strXML += "</graph>";
            string chartId = Guid.NewGuid().ToString().Substring(0, 8);
            //Create the chart - Column 3D Chart with data contained in strXML
            var xxx = FusionCharts.RenderChartHTML("../FusionCharts/Line.swf", "", strXML, chartId, width.ToString(), "400", false, false, true);

            //resultActionLabel.Text += "Колл-во точек в графике - " + array.Count.ToString() + Environment.NewLine;


            return xxx;
        }
        else 
            return "<b>Нет значений для графика!</b>";
    }*/

    /*private string CreateAnyChart(List<int> arrayInput, List<DateTime> date, string name, string xName, string yName, int width, string color)
    {
        List<int> array = new List<int>();

        int maxRecordsCount = Convert.ToInt32(Session["maxRecordsCount"]);
        int step = -1;
        if (arrayInput.Count > maxRecordsCount)
        {
            step = arrayInput.Count / maxRecordsCount;
            array = ApllyStepsToArray(arrayInput, step);
        }
        else
            array = arrayInput;
        if (step > 0)
            date = ApllyStepsToArray(date, step);
        string strXML;
        int i = 0;
        int maxdateCount = 20;
        if (width < 600)
            maxdateCount = 11;
        int labelStep = date.Count / maxdateCount;//maxDateCount
        int j = 0;
        bool hasValues = false;
        foreach (int a in array)
        {
            if (a > 0)
            {
                hasValues = true;
                break;
            }
        }

        if (hasValues)
        {
            strXML = "<anychart><charts><chart><chart_settings><title><text>" + name+"</text></title>";
            strXML += " <axes><x_axis><title><text>"+xName+"</text></title></x_axis>";
            strXML += " <y_axis><title><text>"+yName+"</text></title></y_axis></axes></chart_settings>";
            strXML += @"<data><series name="""+name+@""" type=""Line"">";
            foreach (int element in array)
            {
                strXML += @"<point name=""" + date[i].ToShortDateString() + ' ' + date[i++].ToShortTimeString() + @""" y=""" + element + @"""/>";
            }
            //Close <graph> element
            strXML += "</series></data></chart></charts></anychart>";
            string chartId = Guid.NewGuid().ToString().Substring(0, 8);
            //Create the chart - Column 3D Chart with data contained in strXML
            var xxx = FusionCharts.RenderChartHTML("../anychart_files/swf/AnyChart.swf", "", strXML, chartId, width.ToString(), "400", false, false, true);

            //resultActionLabel.Text += "Колл-во точек в графике - " + array.Count.ToString() + Environment.NewLine;
            return xxx;
        }
        else
            return "<b>Нет значений для графика!</b>";
    }

    private string MultiAxisChart(List<List<int>> parameter, List<DateTime> date, string chartName, List<string> yNames, string xName, int width, List<string> colors)
    {
        int maxRecordsCount = Convert.ToInt32(Session["maxRecordsCount"]);
        int step = -1;
        List<List<int>> arrayList = new List<List<int>>();
        foreach (List<int> arrayInput in parameter)
        {
            if (arrayInput.Count > maxRecordsCount)
            {
                step = arrayInput.Count / maxRecordsCount;
                arrayList.Add(ApllyStepsToArray(arrayInput, step));
            }
            else
                arrayList.Add(arrayInput);
        }
        if (step > 0)
            date = ApllyStepsToArray(date, step);
        string strXML;
        int i = 0;
        int maxdateCount = 20;
        if (width < 600)
            maxdateCount = 11;
        int labelStep = date.Count / maxdateCount;//maxDateCount

        strXML = "<graph caption='" + chartName + "' showShadow='0' labelStep='" + labelStep.ToString() + "' anchorAlpha='0' lineThickness='2' xAxisName='" + xName + "' showValues='0' rotateNames='1' formatNumberScale='0' decimalPrecision='0'>";
        strXML += " <categories>";
        foreach (DateTime categ in date)
            strXML += "<category label='" + categ.ToShortDateString() + " " + categ.ToShortTimeString() + "' />";
        strXML += " </categories>";
        Random r = new Random();
        int onLeft = 1;
        int osj = 1;
        foreach (List<int> array in arrayList)
        {
            strXML += "<axis title='" + yNames[i] + "' titlePos='left' tickWidth='7' axisOnLeft='" + onLeft + "'>";
            strXML += "<dataset seriesName='"+ yNames[i++] +"' >";
            foreach (int element in array)
            {
                strXML += "<set value='" + element + "'/>";
            }
            strXML += "</dataset>";
            strXML += "</axis>";

            if (onLeft == 0)
                onLeft = 1;
            else
                onLeft = 0;
            resultActionLabel.Text += "Колл-во точек в оси " + osj++.ToString()+" - " + array.Count.ToString() + Environment.NewLine;
        }

        strXML += "</graph>";
        string chartId = Guid.NewGuid().ToString().Substring(0, 8);
        var xxx = FusionCharts.RenderChartHTML("../FusionCharts/MultiAxisLine.swf", "", strXML, chartId, width.ToString(), "400", false, false, true);
        return xxx;
    }*/

    /*private List<int> ApllyStepsToArray(List<int> array, int step)
    {
        List<int> returnArray = new List<int>();
        int i = step;
        foreach (int a in array)
        {
            if (i >= step)
            {
                returnArray.Add(a);
                i = 0;
            }
            else
                i++;
        }
        return returnArray;
    }*/

    /*private List<DateTime> ApllyStepsToArray(List<DateTime> array, int step)
    {
        List<DateTime> returnArray = new List<DateTime>();
        int i = step;
        foreach (DateTime a in array)
        {
            if (i >= step)
            {
                returnArray.Add(a);
                i = 0;
            }
            else
                i++;
        }
        return returnArray;
    }*/

    /*protected void MakeMax(object sender, EventArgs e)
    {
        Session["maxRecordsCount"] = 1500;
    }*/

    /*protected void PlfReportsDataGrid_RadioButton_Checked(object sender, EventArgs e)
    {
        Selected_PlfReportsDataGrid_Index.Value = PlfReportsRadioButtonList.SelectedValue;
        ReportNameLabel.Text = PlfReportsRadioButtonList.SelectedItem.Text;
        ChoisesUpdatePanel.Update();
        PlfReportsPickUpdatePanel1.Update();
       // ReportUpdatePanel.Update();
    }*/

    /*[System.Web.Services.WebMethod]
    public static void DeleteMethod(string objectValue)
    {
        try
        {           
            if(System.IO.File.Exists(objectValue))
            {
                System.IO.File.Delete(objectValue);
            }
        }
        catch (Exception ex)
        {
           // resultActionLabel.Text = ex.Message;
        }
    }*/
}
