using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using System.Configuration;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

public partial class Administrator_Data : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Upload_Button.ButtOnClick += new EventHandler(Upload_Click);
        Parse_Button.ButtOnClick += new EventHandler(Parse_Click);

        Aplication_Identification_btn.ButtOnClick += new EventHandler(Driver_AplicationIdentification);
        ICC_btn.ButtOnClick += new EventHandler(Driver_ICC);
        IC_btn.ButtOnClick += new EventHandler(Driver_IC);
        CardIdentification_btn.ButtOnClick += new EventHandler(Driver_Identification);
        CardDownload_btn.ButtOnClick += new EventHandler(Driver_CardDownload);
        DrivingLicenceInfo_btn.ButtOnClick += new EventHandler(Driver_DrivingLicenceInfo);
        Events_btn.ButtOnClick += new EventHandler(Driver_EventsData);
        Faults_btn.ButtOnClick += new EventHandler(Driver_FaultsData);
        Places_btn.ButtOnClick += new EventHandler(Driver_Places);
        Current_Usage_btn.ButtOnClick += new EventHandler(Driver_CurrentUsage);
        ControlActivityData_btn.ButtOnClick += new EventHandler(Driver_ControlActivityData);
        Specific_Conditions_btn.ButtOnClick += new EventHandler(Driver_SpecificConditions);
        DriverFileContent_btn.ButtOnClick += new EventHandler(Driver_FileContents);

        VehicleFileContent_btn.ButtOnClick += new EventHandler(Vehicles_FileContents);
        VehicleIdentification_btn.ButtOnClick += new EventHandler(Vehicles_Identification);
        VehicleCurrentDateTime_btn.ButtOnClick += new EventHandler(Vehicles_CurrentDateTime);
        VehicleDownloadablePeriod_btn.ButtOnClick += new EventHandler(Vehicles_DownloadablePeriod);
        VehicleInsertedCardType.ButtOnClick += new EventHandler(Vehicles_InsertedCardType);
        VehicleDownloadActivityData_btn.ButtOnClick += new EventHandler(Vehicles_DownloadActivityData);
        VehicleCompanyLocksData_btn.ButtOnClick += new EventHandler(Vehicles_CompanyLocksData);
        VehicleControlActivityData_btn.ButtOnClick += new EventHandler(Vehicles_ControlActivityData);
        VehicleEventData_btn.ButtOnClick += new EventHandler(Vehicles_EventData);
        VehicleFaultData_btn.ButtOnClick += new EventHandler(Vehicles_FaultData);
        VehicleOverSpeedingControlData_btn.ButtOnClick += new EventHandler(Vehicles_OverSpeedingControlData);
        VehicleOverSpeedingEventData_btn.ButtOnClick += new EventHandler(Vehicles_OverSpeedingEventData);
        VehicleTimeAdjustmentData_btn.ButtOnClick += new EventHandler(Vehicles_TimeAdjustmentData);
        VehicleDetailedSpeedData_btn.ButtOnClick += new EventHandler(Vehicles_DetailedSpeedData);
        VehicleFullIdentification_btn.ButtOnClick += new EventHandler(Vehicles_FullIdentificationData);
        VehicleSensorPaired_btn.ButtOnClick += new EventHandler(Vehicles_SensorPaired);
        VehicleCalibrationData_btn.ButtOnClick += new EventHandler(Vehicles_CalibrationData);

        ShowStatistics_btn.ButtOnClick += new EventHandler(ShowDriversStatisticsForOneFile);
        ShowGroupContent.ButtOnClick += new EventHandler(ShowDriversStatistics);
        ShowVehiclesGroupContent.ButtOnClick += new EventHandler(ShowVehiclesStatistics);

        TextBoxTest.Text = "";
        AddGridPanel.Visible = true;

        if (!IsPostBack)
        {
           // ((LinkButton)Page.Master.FindControl("DataMasterButt")).Enabled = false;
            UserControlsForAll_BlueButton pan = ((UserControlsForAll_BlueButton)Page.Master.FindControl("DataMasterButt"));
            pan.Enabled = false;
            Session["DeleteCommmand"] = "RemoveUnparsedData";          

            string connectionString = System.Configuration.ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            BLL.DataBlock dataBlock = new BLL.DataBlock(connectionString, "STRING_EN");
            dataBlock.OpenConnection();
            int userId = dataBlock.usersTable.Get_UserID_byName(User.Identity.Name);
            int orgId = dataBlock.usersTable.Get_UserOrgId(userId);
            Session["CURRENT_ORG_ID"] = orgId;

            //выставляем кук, чтобы можно было передать его в метод, вызываемый ч/з ajax
            Response.Cookies["CURRENT_ORG_ID"].Value = Convert.ToString(orgId);
            
            //ORG NAME сверху
            string curOrgName = "";
            curOrgName = dataBlock.usersTable.Get_UserOrgName(userId);
            ((Label)Master.FindControl("CompanyHeaderOrgName")).Text = curOrgName;
            //USER NAME сверху
            curOrgName = dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_Surname);
            curOrgName += " " + dataBlock.usersTable.GetUserInfoValue(userId, DataBaseReference.UserInfo_Name);
            ((Label)Master.FindControl("UserNameHeaderName")).Text = curOrgName;
            ////////////////////////////
            
            ((Panel)Master.FindControl("MainConditionsPanel")).Visible = false;
            ((Panel)Master.FindControl("AdditionalConditionsPanel")).Visible = false;

            dataBlock.usersTable.CloseConnection();
            LoadAllLists();
            LoadDriversEveryWhere(true);
            LoadVehiclesEveryWhere();
            driversCardEditPanelVisible(false);
        }
      /*  if (DataAccordion.SelectedIndex != 2 && DriversSelectTree.SelectedNode != null)
            DriversSelectTree.SelectedNode.Selected = false;
        if (DataAccordion.SelectedIndex != 4 && ManagmentLoadedInfo.SelectedNode != null)
        {
            ManagmentLoadedInfo.SelectedNode.Selected = false;
            driversCardEditPanelVisible(false);          
        }
        if (DataAccordion.SelectedIndex != 4)
            driversCardEditPanelVisible(false);*/
    }

    //AJAX BEGIN

    /// <summary>
    ///Получить элементы дерева Водителей в разделе "Восстановить у пользователя"
    /// </summary>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetRecoverUserDriversTree(String OrgID)
    {
        List<MapItem> data;
        try
        {
            string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            int orgId = Convert.ToInt32(OrgID);
            List<int> namesIds = new List<int>();
            List<string> cardNames = new List<string>();
            dataBlock.OpenConnection();
            namesIds = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.driversCardTypeId);
            cardNames = dataBlock.cardsTable.GetCardNames(namesIds);

            dataBlock.CloseConnection();
            data=new List<MapItem>();
            for (int i = 0; i < namesIds.Count; i++)
            {
                data.Add(new MapItem(namesIds[i].ToString(),cardNames[i]));
            }
        }
        catch (Exception ex)
        {
            return null;
        }
        return data;
    }

    /// <summary>
    ///Получить элементы дерева Транспортные средства в разделе "Восстановить у пользователя"
    /// </summary>
    /// <param name="CardID"></param>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<MapItem> GetRecoverUserTransportTree(String OrgID)
    {
        List<MapItem> data;
        try
        {
            string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            dataBlock.OpenConnection();

            int orgID = Convert.ToInt32(OrgID);
            List<int> vehicles = dataBlock.cardsTable.GetAllCardIds(orgID, dataBlock.cardsTable.vehicleCardTypeId);
            data=new List<MapItem>();
            for (int i = 0; i < vehicles.Count; i++)
            {
                string value = dataBlock.cardsTable.GetCardName(vehicles[i]);
                string key = vehicles[i].ToString();
                data.Add(new MapItem(key,value));
            }
            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            return null;
        }
        return data;
    }

    /// <summary>
    /// Получить данные для выбранного элемента дерева в разделе "Восстановить у пользователя"
    /// </summary>
    /// <param name="CardID"></param>
    /// <returns></returns>
    [System.Web.Services.WebMethod]
    public static List<Driver> GetRecoverUserNodeData(String CardID, String OrgID)
    {
        const string DRIVER="Drivers";
        const string TRANSPORT="Transport";

        List<Driver> drivers=null;
        //выбран общий узел "Водители"
        if (CardID.Equals(DRIVER))
        {
            List<MapItem> items=GetRecoverUserDriversTree(OrgID); 
            foreach(MapItem item in items){
                int id = Convert.ToInt32(item.Key);
                if (drivers == null)
                {
                    drivers = LoadAllDriversLists(id);
                }
                else
                {
                    drivers.AddRange(LoadAllDriversLists(id));
                }
            }
            //устанавливаем правильные номера
            setNumbers(drivers);
            return drivers;
        }
        //выбран общий узел "Транспортные средства"
        if (CardID.Equals(TRANSPORT))
        {
            List<MapItem> items = GetRecoverUserTransportTree(OrgID);
            foreach (MapItem item in items)
            {
                int id = Convert.ToInt32(item.Key);
                if (drivers == null)
                {
                    drivers = LoadAllDriversLists(id);
                }
                else
                {
                    drivers.AddRange(LoadAllDriversLists(id));
                }
            }
            //устанавливаем правильные номера
            setNumbers(drivers);
            return drivers;
        }

        //выбран обычный узел
        int cardId = Convert.ToInt32(CardID);
        drivers = LoadAllDriversLists(cardId);
        return drivers;
    }
    /// <summary>
    /// Sets numbers of items, see GetRecoverUserNodeData 
    /// </summary>
    /// <param name="drivers"></param>
    private static void setNumbers(List<Driver> drivers)
    {
        //устанавливаем правильные номера
        if (drivers != null)
        {
            int i = 1;
            foreach (Driver driver in drivers)
            {
                driver.Number = i;
                i++;
            }
        }
    }

    /// <summary>
    ///Получить элементы дерева в разделе "Просмотреть(Водитель)"
    /// </summary>
    /// <returns>String</returns>
    [System.Web.Services.WebMethod]
    public static GroupTree GetOverlookDriversTree(String OrgID)
    {
        GroupTree drivTree = new GroupTree();
        try
        {
            string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            int orgId = Convert.ToInt32(OrgID);
            dataBlock.OpenConnection();
            String name=dataBlock.organizationTable.GetOrganizationName(orgId);
            
            drivTree.OrgName = name;

            List<int> groupIds = dataBlock.cardsTable.GetAllGroupIds(orgId, dataBlock.cardsTable.driversCardTypeId);
            for (int i = 0; i < groupIds.Count; i++)
            {
                TreeGroup gr = new TreeGroup();
                string grName = dataBlock.cardsTable.GetGroupNameById(groupIds[i]);
                gr.GroupName = grName;
                List<int> values = dataBlock.cardsTable.GetAllCardIdsByGroupId(orgId, dataBlock.cardsTable.driversCardTypeId, groupIds[i]);

                for (int j = 0; j < values.Count; j++)
                {
                    String dr_name = dataBlock.cardsTable.GetCardHolderNameByCardId(values[j]);
                    gr.addValue(values[j].ToString(),dr_name);
                }
                drivTree.addGroup(gr);
            }

            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            drivTree.OrgName = ex.Message;
            return drivTree;
        }
        return drivTree;
    }

    /// <summary>
    ///Получить элементы дерева в разделе "Просмотреть(ТС)"
    /// </summary>
    /// <returns>String</returns>
    [System.Web.Services.WebMethod]
    public static GroupTree GetOverlookVehiclesTree(String OrgID)
    {
        GroupTree vehTree = new GroupTree();
        try
        {
            string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            int orgId = Convert.ToInt32(OrgID);
            dataBlock.OpenConnection();
            String name = dataBlock.organizationTable.GetOrganizationName(orgId);

            vehTree.OrgName = name;

            List<int> groupIds = dataBlock.cardsTable.GetAllGroupIds(orgId, dataBlock.cardsTable.vehicleCardTypeId);
            for (int i = 0; i < groupIds.Count; i++)
            {
                TreeGroup gr = new TreeGroup();
                string grName = dataBlock.cardsTable.GetGroupNameById(groupIds[i]);
                gr.GroupName = grName;
                List<int> values = dataBlock.cardsTable.GetAllCardIdsByGroupId(orgId, dataBlock.cardsTable.vehicleCardTypeId, groupIds[i]);

                for (int j = 0; j < values.Count; j++)
                {
                    String veh_name = dataBlock.cardsTable.GetCardHolderNameByCardId(values[j]);
                    gr.addValue(values[j].ToString(), veh_name);
                }
                vehTree.addGroup(gr);
            }
            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            return null;
        }
        return vehTree;
    }

    private static List<int> getYearsList()
    {
        List<int> years = new List<int>();
        years.Add(2011);
        return years;
    }

    private static string getMonthName(int id){
        switch(id){
            case 1: return "Январь";
            case 2: return "Февраль";
            case 3: return "Март";
            case 4: return "Апрель";
            case 5: return "Май";
            case 6: return "Июнь";
            case 7: return "Июль";
            case 8: return "Август";
            case 9: return "Сентябрь";
            case 10: return "Октябрь";
            case 11: return "Ноябрь";
            case 12: return "Декабрь";
            default: return "Месяц?";
        }

    }

    private static List<int> getMonthsList(int year)
    {
        List<int> months = new List<int>();
        //for (int i=1;i<2;i++){
            //months.Add(i);
        //}
        months.Add(5);
        return months;
    }

    private static List<int> getDaysList(int year, int month)
    {
        List<int> days = new List<int>();        
        //for (int i = 1; i < DateTime.DaysInMonth(year,month)/2;i++)
        //{
            //days.Add(i);
        //}
        days.Add(4);
        return days;
    }

    /// <summary>
    ///Получить значения в таблице "Просмотреть()"
    /// </summary>
    /// <returns>String</returns>
    [System.Web.Services.WebMethod]
    public static List<YearData> GetOverlookDriverNodeData(String CardID, String OrgID)
    {
        List<YearData> result = new List<YearData>();
        try
        {
            string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            int orgId = Convert.ToInt32(OrgID);
            int cardId = Convert.ToInt32(CardID);
            dataBlock.OpenConnection();

            List<int> years = getYearsList();

            bool yearFlag = true;
            bool monthFlag = true;
            int count = 0;
            foreach (int year in years)
            {
                yearFlag = true;
                List<int> months = getMonthsList(year);
                foreach (int month in months)
                {
                    monthFlag = true;
                    List<int> days = getDaysList(year, month);
                    foreach (int day in days)
                    {
                        YearData data= new YearData();
                        if (yearFlag)
                        {
                            data.YearName = year.ToString();
                            yearFlag = false;
                        }
                        else {
                            data.YearName = " ";
                        }
                        if (monthFlag)
                        {
                            data.MonthName = getMonthName(month);
                            monthFlag = false;
                        }
                        else
                        {
                            data.MonthName = " ";
                        }
                        data.DayName = day.ToString();
                        data.Percent = dataBlock.plfUnitInfo.Statistics_GetDayStatistics(new DateTime(year, month, day), cardId).ToString();
                        //data.Percent = "100";
                        data.key = count;
                        result.Add(data);
                        count++;
                    }
                }
            }

            /*YearData d = new YearData();
            d.YearName = CardID;
            d.MonthName = "November";
            d.DayName = "1";
            d.Percent = "37";
            d.key = 0;
            years.Add(d);
            d = new YearData();
            d.DayName = "2";
            d.Percent = "46";
            d.key = 1;
            years.Add(d);*/

            dataBlock.CloseConnection();
        }
        catch (Exception ex)
        {
            YearData d = new YearData();
            d.YearName = ex.Message;
            result.Add(d);
            return result;
        }
        return result;
    }

    
    //AJAX END



    struct AddStructure
    {
        public string name { get; set; }
        public string state { get; set; }
        public int number { get; set; }
        public int byteSize { get; set; }
        public int dataBlockId { get; set; }
        //  public int recordsCount { get; set; }
    }
    protected void Upload_Click(object Sender, EventArgs e)
    {
        int DataBlockId = -1;
        string fileName = MyFileUpload.PostedFile.FileName;
        Status.Text = "";
        int userID = 0;

        try
        {
            int bytesLenght = Convert.ToInt32(MyFileUpload.PostedFile.InputStream.Length);
            byte[] _bytes = new byte[bytesLenght];
            int bytesSize = 0;
            if (MyFileUpload.PostedFile.ContentType != null)
            {
                bytesSize = MyFileUpload.PostedFile.InputStream.Read(_bytes, 0, bytesLenght);
                if (fileName.Substring(fileName.Length - 4, 4).ToLower() == ".zip")
                {
                    ZipFile zip = new ZipFile(MyFileUpload.PostedFile.InputStream);
                    UploadZipFile(zip);
                    LoadAllLists();
                    return;
                }
                else
                    if (fileName.Substring(fileName.Length - 4, 4).ToLower() == ".plf")
                    {
                        if (SelectPLFDriver.Visible == false)
                        {
                            List<filenameAndBytesStruct> listOfPLFS = new List<filenameAndBytesStruct>();
                            filenameAndBytesStruct onePlf = new filenameAndBytesStruct();
                            onePlf.filename = fileName;
                            onePlf._bytes = _bytes;
                            listOfPLFS.Add(onePlf);
                            Session["FileUpload"] = listOfPLFS;
                            SelectDriverForPLF();
                            return;
                        }
                    }
                    else
                        if (BLL.DataBlock.checkDataBlock(_bytes))
                        {
                            UploadFileToBase(fileName, _bytes);
                            LoadAllLists();
                        }
                        else
                            throw new Exception("Неправильный формат файла");
            }
        }
        catch (Exception ex)
        {
            LoadAllLists();
            Status.Text = "Ошибка:" + ex.Message;
        }
    }
    private void SelectDriverForPLF()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        List<string> cardNamesList = new List<string>();
        List<int> CardIdsList = new List<int>();
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        dataBlock.OpenConnection();
        CardIdsList = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.driversCardTypeId);
        cardNamesList = dataBlock.cardsTable.GetCardNames(CardIdsList);
        dataBlock.CloseConnection();
        cardNamesList.Insert(0, "Выберите водителя...");
        cardNamesList.Add("Добавить...");
        cardNamesList.Add("Отмена");
        CardIdsList.Insert(0, -1);
        CardIdsList.Add(-1);
        CardIdsList.Add(-1);
        Session.Add("CardIdsList", CardIdsList);
        SelectPLFDriver.DataSource = cardNamesList;
        SelectPLFDriver.DataBind();
        SelectPLFDriver.Visible = true;
    }
    protected void CreateDriverClick(object Sender, EventArgs e)
    {
        Exception notAllFields = new Exception("Не все поля заполнены!");
        try
        {
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            string name = CreateDriversName.Text;
            string surName = CreateDriversSurname.Text;
            string number = CreateDriversNumber.Text;
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            dataBlock.OpenConnection();
            int curUserId = dataBlock.usersTable.Get_UserID_byName(Page.User.Identity.Name);

            if (name == "")
            {
                CreateDriversName.Text = "Введите значение!";
                throw notAllFields;
            }
            if (surName == "")
            {
                CreateDriversName.Text = "Введите значение!";
                throw notAllFields;
            }
            if (number == "")
            {
                CreateDriversName.Text = "Введите значение!";
                throw notAllFields;
            }

            int cardId = dataBlock.cardsTable.CreateNewCard(name + " " + surName, number, dataBlock.cardsTable.driversCardTypeId, orgId, "Init DataBlockId = NONE", curUserId);

            List<filenameAndBytesStruct> fileUpload = new List<filenameAndBytesStruct>();
            fileUpload = (List<filenameAndBytesStruct>)Session["FileUpload"];

            foreach (filenameAndBytesStruct curFile in fileUpload)
            {
                if (curFile.isPLF)
                    dataBlock.AddPlfTypeData(orgId, curFile._bytes, curFile.filename, cardId);
                else
                    dataBlock.AddData(curFile._bytes, curFile.filename);
            }
            dataBlock.CloseConnection();
            createDriverPanel.Visible = false;
            LoadAllLists();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }
    protected void Upload_PLFFile(object Sender, EventArgs e)
    {
        Status.Text = "";
        int userID = 0;
        SelectPLFDriver.Visible = false;
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            if (SelectPLFDriver.SelectedIndex == 0)
                return;
            if (SelectPLFDriver.SelectedIndex == SelectPLFDriver.Items.Count - 1)
            {
                throw new Exception("Отменено...");
            }
            else
                if (SelectPLFDriver.SelectedIndex == SelectPLFDriver.Items.Count - 2)//Создать
                {
                    createDriverPanel.Visible = true;
                }
                else
                {
                    List<filenameAndBytesStruct> fileUpload = new List<filenameAndBytesStruct>();
                    fileUpload = (List<filenameAndBytesStruct>)Session["FileUpload"];
                    List<int> CardIdsList = new List<int>();
                    CardIdsList = (List<int>)Session["CardIdsList"];
                   
                    foreach (filenameAndBytesStruct curFile in fileUpload)
                    {
                        if (curFile.isPLF)
                            dataBlock.AddPlfTypeData(orgId, curFile._bytes, curFile.filename, CardIdsList[SelectPLFDriver.SelectedIndex]);
                        else
                            dataBlock.AddData(orgId, curFile._bytes, curFile.filename);
                    }
                    
                    Session["FileUpload"] = "";
                }
            dataBlock.CommitTransaction();
            dataBlock.CloseConnection();
            LoadAllLists();
        }
        catch (Exception ex)
        {
            dataBlock.RollbackConnection();
            dataBlock.CloseConnection();
            Status.Text = ex.Message;
        }
        finally
        {
            SelectPLFDriver.Visible = false;
            StatusUpdatePanel.Update();
        }
    }
    struct filenameAndBytesStruct
    {
        public byte[] _bytes;
        public string filename;

        public bool isPLF
        {
            get
            {
                if (filename.Substring(filename.Length - 4, 4).ToLower() == ".plf")
                    return true;
                else
                    return false;
            }
        }
    }
    private void UploadZipFile(ZipFile zip)
    {
        byte[] _bytes;
        List<filenameAndBytesStruct> plfFiles = new List<filenameAndBytesStruct>();
        filenameAndBytesStruct oneFile;
        foreach (ZipEntry fileEntry in zip)
        {
            if (fileEntry.IsFile)
            {
                Stream zipStr = zip.GetInputStream(fileEntry);
                _bytes = new byte[fileEntry.Size];
                zipStr.Read(_bytes, 0, (int)fileEntry.Size);
                if (BLL.DataBlock.checkDataBlock(_bytes))
                {
                    UploadFileToBase(fileEntry.Name, _bytes);
                }
                if (fileEntry.Name.Substring(fileEntry.Name.Length - 4, 4).ToLower() == ".plf")
                {
                    oneFile = new filenameAndBytesStruct();
                    oneFile.filename = fileEntry.Name;
                    oneFile._bytes = _bytes;
                    plfFiles.Add(oneFile);
                }
            }
        }
        if (plfFiles.Count != 0)
            SelectDriverForPLF();
        Session["FileUpload"] = plfFiles;
    }
    private int UploadFileToBase(string fileName, byte[] _bytes)
    {
        int DataBlockId = -1;
        try
        {
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            dataBlock.OpenConnection();
            dataBlock.AddData(orgId, _bytes, fileName);
            dataBlock.CloseConnection();
            DataBlockId = dataBlock.GET_DATA_BLOCK_ID();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return DataBlockId;
    }
    private List<AddStructure> LoadAllLists()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            List<AddStructure> addStructureList = new List<AddStructure>();
            int i = 0;

            AddStructure addStructureTemp;
            List<int> dataBlockIds = new List<int>();

            dataBlock.OpenConnection();
            dataBlockIds = dataBlock.GetAllUnparsedDataBlockIDs(orgId);
            foreach (int dataBlockId in dataBlockIds)
            {
                i++;
                addStructureTemp = new AddStructure();
                addStructureTemp.dataBlockId = dataBlockId;
                addStructureTemp.number = i;
                addStructureTemp.name = dataBlock.GetDataBlock_FileName(dataBlockId);
                addStructureTemp.state = dataBlock.GetDataBlockState(dataBlockId);
                addStructureTemp.byteSize = Convert.ToInt32(dataBlock.GetDataBlock_BytesCount(dataBlockId));

                addStructureList.Add(addStructureTemp);
            }
            if (dataBlockIds.Count == 0)
            {
                Parse_Button.Enabled = false;
                Status.Text = "Нет записей для отображения";
                //UpdatePanel2.Update();
            }
            else
            {
                Parse_Button.Enabled = true;
                Status.Text = "";
            }

            AddGrid.DataSource = CreateDataSource(addStructureList);
            AddGrid.DataBind();
            SetDelColVisible(true);
            setParseButtonVisible(true);
            dataBlock.CloseConnection();
            return addStructureList;
        }
        catch (Exception ex)
        {
            dataBlock.CloseConnection();
            Status.Text = "Произошла ошибка: " + ex.Message;
            return null;
        }
        finally
        {
            StatusUpdatePanel.Update();
        }
    }
    DataTable CreateDataSource(List<AddStructure> data) //0 - Водители, 1 - ТС
    {
        DataTable dt = new DataTable();
        DataRow dr;

        dt.Columns.Add(new DataColumn("#", typeof(int)));
        dt.Columns.Add(new DataColumn("Имя файла", typeof(string)));
        dt.Columns.Add(new DataColumn("Размер(в байтах)", typeof(int)));
        dt.Columns.Add(new DataColumn("Состояние", typeof(string)));

        foreach (AddStructure item in data)
        {
            dr = dt.NewRow();
            dr["#"] = item.number;
            dr["Имя файла"] = item.name;
            dr["Размер(в байтах)"] = item.byteSize;
            dr["Состояние"] = item.state;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    protected void AddGridCommand(object sender, DataGridCommandEventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        List<int> dataBlockIds = new List<int>();
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
        Status.Text = "";
        DriverPreviewButtonsPanel.Visible = false;
        VehiclePreviewButtonsPanel.Visible = false;
        FilesPreviewDataGrid.DataSource = null;
        FilesPreviewDataGrid.DataBind();
        try
        {
            if (e.CommandName == "ViewData")
            {
                if (Session["DeleteCommmand"] == "ViewDriversFile")
                {
                    int tableIndex = Convert.ToInt32(e.Item.Cells[2].Text) - 1;
                    int cardId = Convert.ToInt32(DriversSelectTree.SelectedValue);
                    List<Driver> driversStructureList = new List<Driver>();
                    dataBlock.OpenConnection();
                    driversStructureList = LoadAllDriversLists(cardId);
                    dataBlockIds.Add(driversStructureList[tableIndex].DataBlockId);
                    onlyForInternal.Value = driversStructureList[tableIndex].DataBlockId.ToString();

                    if (driversStructureList[tableIndex].CardTypeName != "Plf")
                    {
                        DriverPreviewButtonsPanel.Visible = true;
                        Driver_FileContents(null, null);
                    }
                    else
                    {
                        PLFUnit.PLFUnitClass plf = new PLFUnit.PLFUnitClass();
                        plf.Records = dataBlock.plfUnitInfo.Get_Records(dataBlockIds, cardId);
                        FilesPreviewDataGrid.DataSource = PlfReportsDataTables.PLF_Data(plf.Records);
                        FilesPreviewDataGrid.DataBind();
                        FilesPreviewPanel.Visible = true;
                    }
                    ShowDriversStatisticsForOneFile(sender, e);
                    dataBlock.CloseConnection();
                }
                if (Session["DeleteCommmand"] == "ViewVehiclesFile")
                {
                    int tableIndex = Convert.ToInt32(e.Item.Cells[2].Text) - 1;
                    int cardId = Convert.ToInt32(VehiclesSelectTree.SelectedValue);
                    List<int> cardDataBlockIds = new List<int>();
                    dataBlock.OpenConnection();
                    cardDataBlockIds = dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId);
                    dataBlockIds.Add(cardDataBlockIds[tableIndex]);
                    onlyForInternal.Value = cardDataBlockIds[tableIndex].ToString();

                    dataBlock.CloseConnection();
                    VehiclePreviewButtonsPanel.Visible = true;
                    Vehicles_FileContents(null, null);
                    ShowVehiclesStatistics(null, null);
                }
            }
            else
                if (Session["DeleteCommmand"].ToString() == "RemoveParsedData")
                {
                    DelGridCommand(sender, e);
                }
                else
                    if (Session["DeleteCommmand"].ToString() == "RemoveUnparsedData")
                    {
                        int number = Convert.ToInt32(e.Item.Cells[2].Text);
                        dataBlockIds = dataBlock.GetAllUnparsedDataBlockIDs(orgId);
                        int dataBlockId = dataBlockIds[number - 1];
                        dataBlock = new DataBlock(connectionString, dataBlockId, "STRING_EN");
                        dataBlock.DeleteDataBlockAndRecords();
                        LoadAllLists();
                    }
                    else
                        if (Session["DeleteCommmand"].ToString() == "RemoveDriversFile")
                        {
                            int rightIdToDelete = -1;
                            string name = e.Item.Cells[3].Text;
                            int recordsCount = int.Parse(e.Item.Cells[7].Text);
                            dataBlockIds = dataBlock.GetDataBlockIdByRecordsCount(recordsCount);

                            if (dataBlockIds.Count == 1)
                                rightIdToDelete = dataBlockIds[0];
                            else
                                if (dataBlockIds.Count > 1)
                                {
                                    string nameTemp;
                                    foreach (int blockId in dataBlockIds)
                                    {
                                        nameTemp = dataBlock.GetDriversNameOrVehiclesNumberByBlockId(blockId);
                                        if (nameTemp == name)
                                        {
                                            rightIdToDelete = blockId;
                                            break;
                                        }
                                    }
                                }

                                else
                                    throw new Exception("Нет доступа к информации(или нет данных для удаления)");
                            if (rightIdToDelete != -1)
                            {
                                dataBlock = new DataBlock(connectionString, rightIdToDelete, "STRING_EN");
                                dataBlock.DeleteDataBlockAndRecords();
                                ManagmentLoadedInfo_SelectedNodeChanged(null, null);
                            }
                        }
                        else
                            if (Session["DeleteCommmand"].ToString() == "RecoverDriversFile")
                            {
                                int tableIndex = Convert.ToInt32(e.Item.Cells[2].Text) - 1;
                                int cardId = Convert.ToInt32(UserFileRecover_TreeView.SelectedValue);
                                List<int> filesIds = new List<int>();
                                dataBlock.OpenConnection();
                                filesIds = dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId);
                                int dataBlockId = filesIds[tableIndex];
                                onlyForInternal.Value = dataBlockId.ToString();
                                byte[] fileBytes = dataBlock.GetDataBlock_BytesArray(dataBlockId);
                                string fileName = dataBlock.GetDataBlock_FileName(dataBlockId);
                                dataBlock.CloseConnection();

                                Response.Clear();
                                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                                Response.AddHeader("Content-Length", fileBytes.Length.ToString());
                                Response.ContentType = "application/octet-stream";
                                Response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
                                Response.End();
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "submit", "javascript:document.form1.submit()", true);
                            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            ModalPopupExtender1.Hide();
        }

    }
    protected void Parse_Click(object Sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        PLFUnit.PLFUnitClass plfForXml = new PLFUnit.PLFUnitClass();
        List<int> dataBlockIDs = new List<int>();
        int cardType = 0;
        int filesCounter = 0;
        object parsedObject;

        try
        {
            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            int userId = dataBlock.usersTable.Get_UserID_byName(User.Identity.Name);
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            dataBlockIDs = dataBlock.GetAllUnparsedDataBlockIDs(orgId);
            if (dataBlockIDs.Count == 0)
                throw new Exception("Отсутствуют данные для разбора.");

            string output = "";
            foreach (int blockId in dataBlockIDs)
            {
                dataBlock.SetDataBlockIdForParse(blockId);
                dataBlock.SetOrgIdForParse(orgId);
                //dataBlock = new DataBlock(connectionString, blockId, "STRING_EN", orgId);
                if (dataBlock.GetDataBlockState(blockId) == "Not parsed")
                {
                    output = Server.MapPath("~/XML_PLF") + "\\";
                    parsedObject = dataBlock.ParseRecords(true, output, userId);
                    filesCounter++;                   
                }
                else
                    continue;

            }
            dataBlock.CommitTransaction();
            dataBlock.CloseConnection();
            LoadAllLists();
            if (filesCounter > 0)
                Status.Text = "Файлы разобраны успешно! Разобрано " + filesCounter.ToString() + " файлов.";
            else
                Status.Text = "Не разобрано ни одного файла.";
        }
        catch (Exception ex)
        {
            dataBlock.RollbackConnection();
            dataBlock.CloseConnection();
            LoadAllLists();
            Status.Text = "Ошибка: " + ex.Message;
        }
        finally
        {
            StatusUpdatePanel.Update();
            ModalPopupExtender1.Hide();
        }
    }
    private List<TableRow> TableRowCollectionToList(TableRowCollection tRC)
    {
        List<TableRow> tableRowList = new List<TableRow>();
        for (int i = 0; i < tRC.Count; i++)
        {
            tableRowList.Add(tRC[i]);
        }
        return tableRowList;
    }
    protected void CancelCreateDriverClick(object Sender, EventArgs e)
    {
        createDriverPanel.Visible = false;
    }
    protected void AccordionHeader1_Click(object sender, EventArgs e)
    {
        try
        {
            int currentAccordionPane = Convert.ToInt32(AccordionCurrentTabIndex_HiddenField.Value);
           /* FilesPreviewDataGrid.DataSource = null;
            FilesPreviewDataGrid.DataBind();
            FilesPreviewPanel.Visible = false;*/

            switch (currentAccordionPane)
            {
                case 0:
                    {
                        LoadAllLists();
                        if(DriversSelectTree.SelectedNode!=null)
                            DriversSelectTree.SelectedNode.Selected = false;
                        if (ManagmentLoadedInfo.SelectedNode != null)
                            ManagmentLoadedInfo.SelectedNode.Selected = false;
                        if (UserFileRecover_TreeView.SelectedNode != null)
                            UserFileRecover_TreeView.SelectedNode.Selected = false;
                        FilesPreviewDataGrid.DataSource = null;
                        FilesPreviewDataGrid.DataBind();
                        FilesPreviewPanel.Visible = false;
                        VehiclePreviewButtonsPanel.Visible = false;
                        DriverPreviewButtonsPanel.Visible = false;
                        //FilesPreviewPanel.Visible = false;
                        setParseButtonVisible(true);
                        setStatistiscPanelVisible(false);
                    } break;
                case 1:
                    {
                        if (DriversSelectTree.SelectedNode != null)
                            DriversSelectTree.SelectedNode.Selected = false;
                        if (ManagmentLoadedInfo.SelectedNode != null)
                            ManagmentLoadedInfo.SelectedNode.Selected = false;
                        FilesPreviewDataGrid.DataSource = null;
                        FilesPreviewDataGrid.DataBind();
                        FilesPreviewPanel.Visible = false;
                        VehiclePreviewButtonsPanel.Visible = false;
                        DriverPreviewButtonsPanel.Visible = false;
                       // FilesPreviewPanel.Visible = false;
                        setParseButtonVisible(false);
                        setStatistiscPanelVisible(false);
                    } break;
                case 2:
                    {
                        if (ManagmentLoadedInfo.SelectedNode != null)
                            ManagmentLoadedInfo.SelectedNode.Selected = false;
                        if (UserFileRecover_TreeView.SelectedNode != null)
                            UserFileRecover_TreeView.SelectedNode.Selected = false;
                        FilesPreviewDataGrid.DataSource = null;
                        FilesPreviewDataGrid.DataBind();
                        FilesPreviewPanel.Visible = false;
                        VehiclePreviewButtonsPanel.Visible = false;
                        DriverPreviewButtonsPanel.Visible = false;
                        //FilesPreviewDataGrid.Visible = true;
                        setParseButtonVisible(false);
                        setStatistiscPanelVisible(true);
                    } break;
                case 3:
                    {
                        if (DriversSelectTree.SelectedNode != null)
                            DriversSelectTree.SelectedNode.Selected = false;
                        if (ManagmentLoadedInfo.SelectedNode != null)
                            ManagmentLoadedInfo.SelectedNode.Selected = false;
                        if (UserFileRecover_TreeView.SelectedNode != null)
                            UserFileRecover_TreeView.SelectedNode.Selected = false;
                        FilesPreviewDataGrid.DataSource = null;
                        FilesPreviewDataGrid.DataBind();
                        FilesPreviewPanel.Visible = false;
                        VehiclePreviewButtonsPanel.Visible = false;
                        DriverPreviewButtonsPanel.Visible = false;
                        //FilesPreviewDataGrid.Visible = true;
                        setParseButtonVisible(false);
                        setStatistiscPanelVisible(true);
                    } break;
                case 4:
                    {
                        if (DriversSelectTree.SelectedNode != null)
                            DriversSelectTree.SelectedNode.Selected = false;
                        if (UserFileRecover_TreeView.SelectedNode != null)
                            UserFileRecover_TreeView.SelectedNode.Selected = false;
                        FilesPreviewDataGrid.DataSource = null;
                        FilesPreviewDataGrid.DataBind();
                        FilesPreviewPanel.Visible = false;
                        VehiclePreviewButtonsPanel.Visible = false;
                        DriverPreviewButtonsPanel.Visible = false;
                       // FilesPreviewPanel.Visible = false;
                    } break;
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            StatusUpdatePanel.Update();
        }
    }
    ////////////////////drivers tab!!!!!!!!!!!/////////////////////////////
    private void LoadDriversEveryWhere(bool managmentTo)
    {
        try
        {
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);
            List<int> namesIds = new List<int>();
            List<string> cardNames = new List<string>();
            dataBlock.OpenConnection();
            namesIds = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.driversCardTypeId);
            cardNames = dataBlock.cardsTable.GetCardNames(namesIds);
            string orgName = dataBlock.organizationTable.GetOrganizationName(orgId);
            dataBlock.CloseConnection();
            if (managmentTo)
            {
                ManagmentLoadedInfo.Nodes.Add(new TreeNode("Водители", "-1"));
                UserFileRecover_TreeView.Nodes.Add(new TreeNode("Водители", "-1"));
            }
            DriversSelectTree.Nodes.Add(new TreeNode(orgName, ""));
            DriversSelectTree.Nodes[0].ChildNodes.Add(new TreeNode("Группа 1", ""));

            for (int i = 0; i < namesIds.Count; i++)
            {
                DriversSelectTree.Nodes[0].Value += namesIds[i].ToString() + ", ";
                DriversSelectTree.Nodes[0].ChildNodes[0].Value += namesIds[i].ToString() + ", ";
                DriversSelectTree.Nodes[0].ChildNodes[0].ChildNodes.Add(new TreeNode(cardNames[i], namesIds[i].ToString()));
                if (managmentTo)
                {
                    ManagmentLoadedInfo.Nodes[ManagmentLoadedInfo.Nodes.Count - 1].ChildNodes.Add(new TreeNode(cardNames[i], namesIds[i].ToString()));
                    UserFileRecover_TreeView.Nodes[UserFileRecover_TreeView.Nodes.Count - 1].ChildNodes.Add(new TreeNode(cardNames[i], namesIds[i].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            StatusUpdatePanel.Update();
        }
    }
    protected void DriversSelectTree_NodeChanged(object sender, EventArgs e)
    {
        try
        {
            Session["DeleteCommmand"] = "ViewDriversFile";
            List<int> cardIds = new List<int>();
            cardIds = getIdList(DriversSelectTree.SelectedValue);

            if (cardIds.Count == 1)
            {
                int cardId = Convert.ToInt32(cardIds[0]);
                List<Driver> driversStructureList = new List<Driver>();
                driversStructureList = LoadAllDriversLists(cardId);
                AddGrid.DataSource = CreateDriversInfoSource(driversStructureList);
                AddGrid.DataBind();
            }
            if (cardIds.Count > 1)
            {
                AddGridPanel.Visible = false;
            }
            SetDelColVisible(false);
            setParseButtonVisible(false);
            VehiclePreviewButtonsPanel.Visible = false;
            DriverPreviewButtonsPanel.Visible = false;
            FilesPreviewPanel.Visible = false;
            ShowDriversStatistics(sender, e);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            StatusUpdatePanel.Update();
        }
    }

    private static List<Driver> LoadAllDriversLists(int cardId)
    {

        List<Driver> driversStructureList = new List<Driver>();
        int number = 0;
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        Driver driversStructureTemp;
        List<int> dataBlockIds = new List<int>();
        try
        {
            dataBlock.OpenConnection();
            dataBlockIds = dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId);
            int cardType;
            List<DateTime> startEnd = new List<DateTime>();
            foreach (int dataBlockId in dataBlockIds)
            {
                number++;
                driversStructureTemp = new Driver();
                driversStructureTemp.DataBlockId = dataBlockId;
                driversStructureTemp.Number = number;
                driversStructureTemp.Name = dataBlock.GetDataBlock_FileName(dataBlockId);
                driversStructureTemp.RecordsCount = dataBlock.GetDataBlock_RecorsCount(dataBlockId);
                driversStructureTemp.CreationTime = dataBlock.GetDataBlock_EDate(dataBlockId);
                cardType = dataBlock.GetDataBlock_CardType(dataBlockId);
                driversStructureTemp.CardTypeName = dataBlock.GetCardTypeName(dataBlockId);
                if (cardType == 0)
                {
                    startEnd = dataBlock.cardUnitInfo.Get_StartEndPeriod(dataBlockId);
                    driversStructureTemp.setFromDate(startEnd[0]);
                    driversStructureTemp.setToDate(startEnd[1]);
                }
                else
                    if (cardType == 2)
                    {
                        driversStructureTemp.setFromDate(dataBlock.plfUnitInfo.Get_START_PERIOD(dataBlockId));
                        driversStructureTemp.setToDate(dataBlock.plfUnitInfo.Get_END_PERIOD(dataBlockId));
                    }
                    else
                    {
                        driversStructureTemp.setFromDate(new DateTime());
                        driversStructureTemp.setToDate(new DateTime()) ;
                    }

                driversStructureList.Add(driversStructureTemp);
            }
            dataBlock.CloseConnection();

            return driversStructureList;
        }
        catch (Exception ex)
        {
            dataBlock.CloseConnection();
            return null;
        }
    }
    DataTable CreateDriversInfoSource(List<Driver> data)
    {
        DataTable dt = new DataTable();
        DataRow dr;

        dt.Columns.Add(new DataColumn("#", typeof(int)));
        dt.Columns.Add(new DataColumn("Имя файла", typeof(string)));
        dt.Columns.Add(new DataColumn("Тип Файла", typeof(string)));
        dt.Columns.Add(new DataColumn("Период С", typeof(string)));
        dt.Columns.Add(new DataColumn("По", typeof(string)));
        dt.Columns.Add(new DataColumn("Кол-во записей", typeof(int)));
        dt.Columns.Add(new DataColumn("Дата разбора", typeof(string)));

        foreach (Driver item in data)
        {
            dr = dt.NewRow();
            dr["#"] = item.Number;
            dr["имя файла"] = item.Name;
            dr["Тип файла"] = item.CardTypeName;
            dr["Период С"] = item.FromDate;
            dr["По"] = item.ToDate;
            dr["Кол-во записей"] = item.RecordsCount;
            dr["Дата разбора"] = item.CreationTime;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    ///driversPreview
    protected void Driver_FileContents(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        DriverFileContent_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();
            TextBoxTest.Text = "";
            CardUnit.CardUnitClass driversCard = new CardUnit.CardUnitClass();

            driversCard = dataBlock.cardUnitInfo.GetAllCardUnitClass_parsingDataBlock(dataBlockId);            

            FilesPreviewDataGrid.DataSource = DriverPreviewDataTable.DriverPreview_FileContents(driversCard);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_AplicationIdentification(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        Aplication_Identification_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            TextBoxTest.Attributes.Add("style", "overflow :hidden");
            int userID = 0;
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
        

            dataBlock.OpenConnection();
            TextBoxTest.Text = "";
            DDDClass.EquipmentType cardType = dataBlock.cardUnitInfo.Get_EF_Application_Identification_CardType(dataBlockId);

            string textResult = "";
            switch (cardType.equipmentType)
            {
                case 1:
                    {
                        DDDClass.DriverCardApplicationIdentification driversCard = dataBlock.cardUnitInfo.Get_EF_Application_Identification_DriverCardApplicationIdentification(dataBlockId);
/*
                        textResult += "cardType: " + cardType.ToString() + Environment.NewLine+"<br/>";
                        textResult += "activityStructureLength:   " + driversCard.activityStructureLength.ToString() + Environment.NewLine + "<br/>";
                        textResult += "cardStructureVersion:      " + driversCard.cardStructureVersion.ToString() + Environment.NewLine + "<br/>";
                        textResult += "noOfCardPlaceRecords:      " + driversCard.noOfCardPlaceRecords.ToString() + Environment.NewLine + "<br/>";
                        textResult += "noOfCardVehicleRecords:    " + driversCard.noOfCardVehicleRecords.ToString() + Environment.NewLine + "<br/>";
                        textResult += "noOfEventsPerType:         " + driversCard.noOfEventsPerType.ToString() + Environment.NewLine + "<br/>";
                        textResult += "noOfFaultsPerType:         " + driversCard.noOfFaultsPerType.ToString() + Environment.NewLine + "<br/>";
                        textResult += "typeOfTachographCardId:    " + driversCard.typeOfTachographCardId.ToString() + Environment.NewLine + "<br/>";
                                   
                        TextBoxTest.Text = textResult;
                        //TextBoxTest.Columns = 8;
                        //TextBoxTest.Height = 130;*/
                        FilesPreviewDataGrid.DataSource = DriverPreviewDataTable.DriverPreview_AplicationIdentification(driversCard, cardType);
                        FilesPreviewDataGrid.DataBind();
                    }
                    break;
                case 2:
                    {
                        DDDClass.WorkshopCardApplicationIdentification workshopCard = dataBlock.cardUnitInfo.Get_EF_Application_Identification_WorkshopCardApplicationIdentification(dataBlockId);

                      /*  TextBoxTest.Text += "cardType: " + cardType.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "activityStructureLength:   " + workshopCard.activityStructureLength.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "cardStructureVersion:      " + workshopCard.cardStructureVersion.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "noOfCardPlaceRecords:      " + workshopCard.noOfCardPlaceRecords.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "noOfCardVehicleRecords:    " + workshopCard.noOfCardVehicleRecords.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "noOfEventsPerType:         " + workshopCard.noOfEventsPerType.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "noOfFaultsPerType:         " + workshopCard.noOfFaultsPerType.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "typeOfTachographCardId:    " + workshopCard.typeOfTachographCardId.ToString() + Environment.NewLine + "<br/>";*/

                        FilesPreviewDataGrid.DataSource = DriverPreviewDataTable.DriverPreview_AplicationIdentification(workshopCard, cardType);
                        FilesPreviewDataGrid.DataBind();
                    }
                    break;
                case 3:
                    {
                        DDDClass.ControlCardApplicationIdentification controlCard = dataBlock.cardUnitInfo.Get_EF_Application_Identification_ControlCardApplicationIdentification(dataBlockId);

                        TextBoxTest.Text += "cardType: " + cardType.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "cardStructureVersion:      " + controlCard.cardStructureVersion.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "noOfControlActivityRecords:" + controlCard.noOfControlActivityRecords.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "typeOfTachographCardId:    " + controlCard.typeOfTachographCardId.ToString() + Environment.NewLine + "<br/>";
                    }
                    break;
                case 4:
                    {
                        DDDClass.CompanyCardApplicationIdentification companyCard = dataBlock.cardUnitInfo.Get_EF_Application_Identification_CompanyCardApplicationIdentification(dataBlockId);

                        TextBoxTest.Text += "cardType: " + cardType.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "cardStructureVersion:      " + companyCard.cardStructureVersion.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "noOfCompanyActivityRecords:" + companyCard.noOfCompanyActivityRecords.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "typeOfTachographCardId:    " + companyCard.typeOfTachographCardId.ToString() + Environment.NewLine + "<br/>";
                    }
                    break;
                default:
                    throw new Exception("Неизвестный тип карты!");
            }
            TextBoxTest.Text += Environment.NewLine;
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_ICC(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        ICC_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        int userID = 0;
        int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        TextBoxTest.Text = "";
        dataBlock.OpenConnection();
        try
        {
            DDDClass.CardIccIdentification data = new DDDClass.CardIccIdentification();
            data = dataBlock.cardUnitInfo.Get_EF_ICC(dataBlockId).cardIccIdentification;

      /*      TextBoxTest.Text += "cardApprovalNumber:" + data.cardApprovalNumber.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "cardExtendedSerialNumber: " + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "    manufacturerCode:" + data.cardExtendedSerialNumber.manufacturerCode.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "    monthYear:      " + data.cardExtendedSerialNumber.monthYear.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "    serialNumber:   " + data.cardExtendedSerialNumber.serialNumber.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "    type:           " + data.cardExtendedSerialNumber.type.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "cardPersonaliserID: " + data.CardPersonaliserID_ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "clockStop:          " + data.ClockStop_ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "embedderIcAssemblerId:" + data.EmbedderIcAssemblerId_ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "icIdentifier:       " + data.IcIdentifier_ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += Environment.NewLine + "<br/>";*/

            FilesPreviewDataGrid.DataSource = DriverPreviewDataTable.DriverPreview_ICC(data);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception exc)
        {
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            Status.Text = exc.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_IC(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        IC_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        TextBoxTest.Text = "";
        dataBlock.OpenConnection();
        try
        {
            DDDClass.CardChipIdentification data = new DDDClass.CardChipIdentification();
            data = dataBlock.cardUnitInfo.Get_EF_IC(dataBlockId).cardChipIdentification;

          /*  TextBoxTest.Text += "icManufacturingReferences:" + data.icManufacturingReferences.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "icSerialNumber: " + data.icSerialNumber.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += Environment.NewLine + "<br/>";*/
            FilesPreviewDataGrid.DataSource = DriverPreviewDataTable.DriverPreview_IC(data);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception exc)
        {
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;

            Status.Text = exc.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_Identification(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        CardIdentification_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        int userID = 0;
        int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        dataBlock.OpenConnection();
        TextBoxTest.Text = "";

        try
        {
            DDDClass.CardIdentification data = new DDDClass.CardIdentification();
            data = dataBlock.cardUnitInfo.Get_EF_Identification_CardIdentification(dataBlockId);
           /* TextBoxTest.Text += "card Identification:    " + Environment.NewLine + "<br/>";

            TextBoxTest.Text += "cardExpiryDate:    " + data.cardExpiryDate.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "cardIssueDate:     " + data.cardIssueDate.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "cardIssuingAuthorityName:  " + data.cardIssuingAuthorityName.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "cardIssuingMemberState:    " + data.cardIssuingMemberState.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "cardValidityBegin:         " + data.cardValidityBegin.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "cardNumber:    " + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   driverIdentificationNumber:    " + data.cardNumber.driverIdentificationNumber() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   ownerIdentificationNumber:    " + data.cardNumber.ownerIdentificationNumber() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += Environment.NewLine + "<br/>";
            */
            DDDClass.EquipmentType cardType = dataBlock.cardUnitInfo.Get_EF_Identification_CardType(dataBlockId);

            switch (cardType.equipmentType)
            {
                case 1:
                    {
                        DDDClass.DriverCardHolderIdentification driverCard = new DDDClass.DriverCardHolderIdentification();
                        driverCard = dataBlock.cardUnitInfo.Get_EF_Identification_DriverCardHolderIdentification(dataBlockId);
/*
                        TextBoxTest.Text += "cardType:  " + cardType.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "cardHolderBirthDate:   " + driverCard.cardHolderBirthDate.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "cardHolderName:        " + driverCard.cardHolderName.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "cardHolderName:        " + driverCard.cardHolderPreferredLanguage.ToString() + Environment.NewLine + "<br/>";*/

                        FilesPreviewDataGrid.DataSource = DriverPreviewDataTable.DriverPreview_CardIdentification(data, driverCard, cardType);
                        FilesPreviewDataGrid.DataBind();
                    }
                    break;
                case 2:
                    {
                        DDDClass.WorkshopCardHolderIdentification workshopCard = new DDDClass.WorkshopCardHolderIdentification();
                        workshopCard = dataBlock.cardUnitInfo.Get_EF_Identification_WorkshopCardHolderIdentification(dataBlockId);

                     /*   TextBoxTest.Text += "cardType:  " + cardType.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "workshopName:      " + workshopCard.workshopName.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "workshopAddress:   " + workshopCard.workshopAddress.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "cardHolderName:    " + workshopCard.cardHolderName.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "cardHolderPreferredLanguage: " + workshopCard.cardHolderPreferredLanguage.ToString() + Environment.NewLine + "<br/>";*/

                        FilesPreviewDataGrid.DataSource = DriverPreviewDataTable.DriverPreview_CardIdentification(data, workshopCard, cardType);
                        FilesPreviewDataGrid.DataBind();
                    }
                    break;
                case 3:
                    {
                        DDDClass.ControlCardHolderIdentification controlCard = new DDDClass.ControlCardHolderIdentification();
                        controlCard = dataBlock.cardUnitInfo.Get_EF_Identification_ControlCardHolderIdentification(dataBlockId);

                        TextBoxTest.Text += "cardType:  " + cardType.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "cardHolderName:   " + controlCard.cardHolderName.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "cardHolderPreferredLanguage: " + controlCard.cardHolderPreferredLanguage.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "controlBodyAddress: " + controlCard.controlBodyAddress.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "controlBodyName:    " + controlCard.controlBodyName.ToString() + Environment.NewLine + "<br/>";
                    }
                    break;
                case 4:
                    {
                        DDDClass.CompanyCardHolderIdentification companyCard = new DDDClass.CompanyCardHolderIdentification();
                        companyCard = dataBlock.cardUnitInfo.Get_EF_Identification_CompanyCardHolderIdentification(dataBlockId);

                        TextBoxTest.Text += "cardType:  " + cardType.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "cardHolderPreferredLanguage: " + companyCard.cardHolderPreferredLanguage.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "companyAddress:    " + companyCard.companyAddress.ToString() + Environment.NewLine + "<br/>";
                        TextBoxTest.Text += "companyName:       " + companyCard.companyName.ToString() + Environment.NewLine + "<br/>";
                    }
                    break;
                default:
                    throw new Exception("Неизвестный тип карты!");
            }
        }
        catch (Exception exc)
        {
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            Status.Text = exc.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_CardDownload(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        CardDownload_btn.Enabled = false;

        int userID = 0;
        int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        dataBlock.OpenConnection();
        TextBoxTest.Text = "";

        try
        {
            CardUnit.EF_Card_Download data = new CardUnit.EF_Card_Download();
            data = dataBlock.cardUnitInfo.Get_EF_Card_Download(dataBlockId);
            TextBoxTest.Text += "CardDownload:    " + Environment.NewLine + "<br/>";

            DDDClass.EquipmentType cardType = new DDDClass.EquipmentType(Convert.ToByte(data.cardType));

            switch (cardType.equipmentType)
            {
                case 1:
                    {
                        TextBoxTest.Text += "cardType:  " + cardType.ToString() + Environment.NewLine + "<br/>";
                        DDDClass.LastCardDownload lastDownload = data.lastCardDownload;
                        if (lastDownload != null)
                            TextBoxTest.Text += "lastCardDownload:  " + lastDownload.ToString() + Environment.NewLine + "<br/>";
                        else
                            TextBoxTest.Text += "lastCardDownload:  нет информации" + Environment.NewLine + "<br/>";
                    }
                    break;
                case 2:
                    {
                        TextBoxTest.Text += "cardType:  " + cardType.ToString() + Environment.NewLine + "<br/>";
                        DDDClass.NoOfCalibrationsSinceDownload noofcalib = data.noOfCalibrationsSinceDownload;
                        if (noofcalib != null)
                            TextBoxTest.Text += "noOfCalibrationsSinceDownload: " + noofcalib.ToString() + Environment.NewLine + "<br/>";
                        else
                            TextBoxTest.Text += "noOfCalibrationsSinceDownload: нет информации" + Environment.NewLine + "<br/>";
                    }
                    break;
                case 3:
                    {
                        TextBoxTest.Text += "cardType:  " + cardType.ToString() + Environment.NewLine + "<br/>";
                    }
                    break;
                case 4:
                    {
                        TextBoxTest.Text += "cardType:  " + cardType.ToString() + Environment.NewLine + "<br/>";
                    }
                    break;
                default:
                    throw new Exception("Неизвестный тип карты!");
            }
        }
        catch (Exception exc)
        {
            Status.Text += exc.Message;
            TextBoxTest.Text = "";
            StatusUpdatePanel.Update();
            //TextBoxTest.Visible = false;
            //ErrorLabel.Text = exc.Message;
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_DrivingLicenceInfo(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        DrivingLicenceInfo_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        int userID = 0;
        int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        dataBlock.OpenConnection();
        TextBoxTest.Text = "";

        try
        {
            CardUnit.EF_Driving_Licence_Info data = new CardUnit.EF_Driving_Licence_Info();
            data = dataBlock.cardUnitInfo.Get_EF_Driving_Licence_Info(dataBlockId);
          /*  TextBoxTest.Text += "Driving Licence Info:    " + Environment.NewLine + Environment.NewLine + "<br/>" + "<br/>";

            TextBoxTest.Text += "drivingLicenceIssuingAuthority:    " + data.cardDrivingLicenceInformation.drivingLicenceIssuingAuthority.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "drivingLicenceIssuingNation:       " + data.cardDrivingLicenceInformation.drivingLicenceIssuingNation.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "drivingLicenceNumber:              " + data.cardDrivingLicenceInformation.drivingLicenceNumber.ToString() + Environment.NewLine + "<br/>";*/

            FilesPreviewDataGrid.DataSource = DriverPreviewDataTable.DriverPreview_DrivingLicenceInfo(data);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception exc)
        {
            TextBoxTest.Text = "";
            Status.Text = exc.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_EventsData(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        Events_btn.Enabled = false;

        List<DDDClass.CardEventRecord> eventsData = new List<DDDClass.CardEventRecord>();
        int userID = 0;
        int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        dataBlock.OpenConnection();
        TextBoxTest.Text = "";

        try
        {
            eventsData = dataBlock.cardUnitInfo.Get_EF_Events_Data(dataBlockId);
            int number = 1;
            DateTime dateTime = new DateTime();
            TextBoxTest.Text = "Events Data:" + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   COUNT:" + eventsData.Count + Environment.NewLine + "<br/>";

            if (eventsData.Count == 0)
                TextBoxTest.Text += "Нет записей!";
            else
                foreach (DDDClass.CardEventRecord record in eventsData)
                {
                    TextBoxTest.Text += Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "#" + number + Environment.NewLine + "<br/>";
                    dateTime = record.eventBeginTime.getTimeRealDate();
                    TextBoxTest.Text += "   eventBeginTime: " + dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString() + Environment.NewLine + "<br/>";
                    dateTime = record.eventEndTime.getTimeRealDate();
                    TextBoxTest.Text += "   eventEndTime:   " + dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString() + Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "   eventType:      " + record.eventType.ToString() + Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "   eventVehicleRegistration:      " + Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "       vehicleRegistrationNation:  " + record.eventVehicleRegistration.vehicleRegistrationNation.ToString() + Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "       vehicleRegistrationNumber:  " + record.eventVehicleRegistration.vehicleRegistrationNumber.ToString() + Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "-----------------------------------------";
                    number++;
                }
        }
        catch (Exception exc)
        {
            TextBoxTest.Text = "";
            Status.Text = exc.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_FaultsData(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        Faults_btn.Enabled = false;

        List<DDDClass.CardFaultRecord> faultsData = new List<DDDClass.CardFaultRecord>();
        int userID = 0;
        int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        dataBlock.OpenConnection();
        TextBoxTest.Text = "";

        try
        {
            faultsData = dataBlock.cardUnitInfo.Get_EF_Faults_Data(dataBlockId);
            int number = 1;
            DateTime dateTime = new DateTime();
            TextBoxTest.Text = "Faults Data:" + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   COUNT:" + faultsData.Count + Environment.NewLine + "<br/>";

            if (faultsData.Count == 0)
                TextBoxTest.Text += "Нет записей!";
            else
                foreach (DDDClass.CardFaultRecord record in faultsData)
                {
                    TextBoxTest.Text += Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "#" + number + Environment.NewLine + "<br/>";
                    dateTime = record.faultBeginTime.getTimeRealDate();
                    TextBoxTest.Text += "   faultBeginTime: " + dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString() + Environment.NewLine + "<br/>";
                    dateTime = record.faultEndTime.getTimeRealDate();
                    TextBoxTest.Text += "   faultEndTime:   " + dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString() + Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "   faultType:      " + record.faultType.ToString() + Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "   faultVehicleRegistration:      " + Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "       vehicleRegistrationNation:  " + record.faultVehicleRegistration.vehicleRegistrationNation.ToString() + Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "       vehicleRegistrationNumber:  " + record.faultVehicleRegistration.vehicleRegistrationNumber.ToString() + Environment.NewLine + "<br/>";
                    TextBoxTest.Text += "-----------------------------------------";
                    number++;
                }
        }
        catch (Exception exc)
        {
            TextBoxTest.Text = "";
            Status.Text = exc.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_Places(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        Places_btn.Enabled = false;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            TextBoxTest.Text = "";

            DDDClass.CardPlaceDailyWorkPeriod cardPlaceDailyWorkPeriod = new DDDClass.CardPlaceDailyWorkPeriod();

            int userID = 0;
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            cardPlaceDailyWorkPeriod = dataBlock.cardUnitInfo.Get_EF_Places(dataBlockId);

           /* int number = 1;
            TextBoxTest.Text = "Place Daily Work Period records COUNT: " + cardPlaceDailyWorkPeriod.placeRecords.Count + Environment.NewLine + "<br/>";
            */
            if (cardPlaceDailyWorkPeriod.placeRecords.Count == 0)
                TextBoxTest.Text += "Нет записей!";
            else
                FilesPreviewPanel.Visible = true;

          /*  foreach (DDDClass.PlaceRecord record in cardPlaceDailyWorkPeriod.placeRecords)
            {
                TextBoxTest.Text += Environment.NewLine + "<br/>";
                TextBoxTest.Text += "#" + number + Environment.NewLine + "<br/>";
                TextBoxTest.Text += "   dailyWorkPeriodCountry:\t" + record.dailyWorkPeriodCountry.ToString() + Environment.NewLine + "<br/>";
                TextBoxTest.Text += "   dailyWorkPeriodRegion:\t   " + record.dailyWorkPeriodRegion.ToString() + Environment.NewLine + "<br/>";
                TextBoxTest.Text += "   entryTime:               " + record.entryTime.ToString() + Environment.NewLine + "<br/>";
                TextBoxTest.Text += "   entryTypeDailyWorkPeriod: " + record.entryTypeDailyWorkPeriod.ToString() + Environment.NewLine + "<br/>";
                TextBoxTest.Text += "   vehicleOdometerValue:    " + record.vehicleOdometerValue.ToString() + Environment.NewLine + "<br/>";
                TextBoxTest.Text += "-----------------------------------------";
                number++;
            }*/

            FilesPreviewDataGrid.DataSource = DriverPreviewDataTable.DriverPreview_DrivingLicenceInfo(cardPlaceDailyWorkPeriod);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception exc)
        {
            TextBoxTest.Text = "";
            Status.Text = exc.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_CurrentUsage(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        Current_Usage_btn.Enabled = false;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            dataBlock.OpenConnection();
            TextBoxTest.Text = "";

            DDDClass.CardCurrentUse cardCurrentUse = new DDDClass.CardCurrentUse();

            int userID = 0;
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
          

            cardCurrentUse = dataBlock.cardUnitInfo.Get_EF_Current_Usage(dataBlockId);

            TextBoxTest.Text += Environment.NewLine;
            TextBoxTest.Text += "sessionOpenTime:       " + cardCurrentUse.sessionOpenTime.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "sessionOpenVehicle:    " + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   vehicleRegistrationNation:  " + cardCurrentUse.sessionOpenVehicle.vehicleRegistrationNation.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   vehicleRegistrationNumber:  " + cardCurrentUse.sessionOpenVehicle.vehicleRegistrationNumber.ToString() + Environment.NewLine + "<br/>";
        }
        catch (Exception exc)
        {
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            Status.Text = exc.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_ControlActivityData(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        ControlActivityData_btn.Enabled = false;
        
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            dataBlock.OpenConnection();
            TextBoxTest.Text = "";

            DDDClass.CardControlActivityDataRecord cardControlActivityDataRecord = new DDDClass.CardControlActivityDataRecord();

            int userID = 0;
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
           

            cardControlActivityDataRecord = dataBlock.cardUnitInfo.Get_EF_Control_Activity_Data(dataBlockId);
            TextBoxTest.Text = "Control Activity Data Record: " + Environment.NewLine + "<br/>";

            TextBoxTest.Text += Environment.NewLine + "<br/>";
            TextBoxTest.Text += "controlCardNumber:  " + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   cardIssuingMemberState: " + cardControlActivityDataRecord.controlCardNumber.cardIssuingMemberState.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   cardType:               " + cardControlActivityDataRecord.controlCardNumber.cardType.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   driverIdentificationNumber: " + cardControlActivityDataRecord.controlCardNumber.cardNumber.driverIdentificationNumber() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   ownerIdentificationNumber:  " + cardControlActivityDataRecord.controlCardNumber.cardNumber.ownerIdentificationNumber() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "controlDownloadPeriodBegin:    " + cardControlActivityDataRecord.controlDownloadPeriodBegin.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "controlDownloadPeriodEnd:      " + cardControlActivityDataRecord.controlDownloadPeriodEnd.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "controlTime:                   " + cardControlActivityDataRecord.controlTime.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "controlType:                   " + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   Card_Downloading:           " + cardControlActivityDataRecord.controlType.Card_Downloading() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   Display_Activity:           " + cardControlActivityDataRecord.controlType.Display_Activity() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   Printing_Activity:          " + cardControlActivityDataRecord.controlType.Printing_Activity() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   Vu_Downloading:             " + cardControlActivityDataRecord.controlType.Vu_Downloading() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "controlVehicleRegistration:    " + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   vehicleRegistrationNation:  " + cardControlActivityDataRecord.controlVehicleRegistration.vehicleRegistrationNation.ToString() + Environment.NewLine + "<br/>";
            TextBoxTest.Text += "   vehicleRegistrationNumber:  " + cardControlActivityDataRecord.controlVehicleRegistration.vehicleRegistrationNumber.ToString() + Environment.NewLine + "<br/>";
        }
        catch (Exception exc)
        {
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            Status.Text = exc.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Driver_SpecificConditions(object sender, EventArgs e)
    {
        EnableAllDriversPreviewButtons();
        Specific_Conditions_btn.Enabled = false;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");

        try
        {
            dataBlock.OpenConnection();
            TextBoxTest.Text = "";

            List<DDDClass.SpecificConditionRecord> SpecificConditionData = new List<DDDClass.SpecificConditionRecord>();

            int userID = 0;
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
         
            SpecificConditionData = dataBlock.cardUnitInfo.Get_EF_Specific_Conditions(dataBlockId);

            int number = 1;
            TextBoxTest.Text = "Specific ConditionData COUNT: " + SpecificConditionData.Count + Environment.NewLine + "<br/>";

            if (SpecificConditionData.Count == 0)
                TextBoxTest.Text += "Нет записей!";
            else
            {

                DateTime dateTime = new DateTime();

                TextBoxTest.Text += Environment.NewLine + "<br/>";
                TextBoxTest.Text += "#\tentryTime\t\t\tspecificConditionType" + Environment.NewLine + "<br/>";
                foreach (DDDClass.SpecificConditionRecord record in SpecificConditionData)
                {
                    dateTime = record.entryTime.getTimeRealDate();
                    TextBoxTest.Text += Environment.NewLine + "<br/>";
                    TextBoxTest.Text += number + "\t";
                    TextBoxTest.Text += dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString() + "\t";
                    TextBoxTest.Text += record.specificConditionType.ToString();
                    number++;
                }
            }
        }
        catch (Exception exc)
        {
            TextBoxTest.Text = "";
            Status.Text = exc.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }

    private void EnableAllDriversPreviewButtons()
    {
        Aplication_Identification_btn.Enabled = true;
        ICC_btn.Enabled = true;
        IC_btn.Enabled = true;
        CardIdentification_btn.Enabled = true;
        CardDownload_btn.Enabled = true;
        DrivingLicenceInfo_btn.Enabled = true;
        Events_btn.Enabled = true;
        Faults_btn.Enabled = true;
        Places_btn.Enabled = true;
        Current_Usage_btn.Enabled = true;
        ControlActivityData_btn.Enabled = true;
        Specific_Conditions_btn.Enabled = true;
        DriverFileContent_btn.Enabled = true;

        TextBoxTest.Text = "";
        FilesPreviewDataGrid.DataSource = null;
        FilesPreviewDataGrid.DataBind();
        FilesPreviewPanel.Visible = false;
    }
    ///
    ////////////////////vehicles tab///////////////////////////
    private void LoadVehiclesEveryWhere()
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        int orgId = Convert.ToInt32(Session["CURRENT_ORG_ID"]);

        try
        {
            List<int> vehicles = new List<int>();
            dataBlock.OpenConnection();
            string orgName = dataBlock.organizationTable.GetOrganizationName(orgId);

            vehicles = dataBlock.cardsTable.GetAllCardIds(orgId, dataBlock.cardsTable.vehicleCardTypeId);
            string vehicleName;
            ManagmentLoadedInfo.Nodes.Add(new TreeNode("Транспортные средства", "-1"));
            UserFileRecover_TreeView.Nodes.Add(new TreeNode("Транспортные средства", "-1"));
            VehiclesSelectTree.Nodes.Add(new TreeNode(orgName, ""));
            VehiclesSelectTree.Nodes[0].ChildNodes.Add(new TreeNode("Группа 1", ""));
            for (int i = 0; i < vehicles.Count; i++)
            {
                vehicleName = dataBlock.cardsTable.GetCardName(vehicles[i]);
                ManagmentLoadedInfo.Nodes[ManagmentLoadedInfo.Nodes.Count - 1].ChildNodes.Add(new TreeNode(vehicleName, vehicles[i].ToString()));
                UserFileRecover_TreeView.Nodes[UserFileRecover_TreeView.Nodes.Count - 1].ChildNodes.Add(new TreeNode(vehicleName, vehicles[i].ToString()));

                VehiclesSelectTree.Nodes[0].Value += vehicles[i].ToString() + ", ";
                VehiclesSelectTree.Nodes[0].ChildNodes[0].Value += vehicles[i].ToString() + ", ";
                VehiclesSelectTree.Nodes[0].ChildNodes[0].ChildNodes.Add(new TreeNode(vehicleName, vehicles[i].ToString()));
               
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }
    protected void VehiclesSelectTree_NodeChanged(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {            
            Session["DeleteCommmand"] = "ViewVehiclesFile";
            List<int> cardIds = new List<int>();
            cardIds = getIdList(VehiclesSelectTree.SelectedValue);
            dataBlock.OpenConnection();
            if (cardIds.Count == 1)
            {
                List<int> dataBlocksIds = dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardIds[0]);
                AddGrid.DataSource = CreateVehiclesInfoSource(dataBlocksIds, dataBlock);
                AddGrid.DataBind();
            }
            if (cardIds.Count > 1)
            {
                AddGridPanel.Visible = false;
            }

            SetDelColVisible(false);
            setParseButtonVisible(false);
            VehiclePreviewButtonsPanel.Visible = false;
            DriverPreviewButtonsPanel.Visible = false;
            FilesPreviewPanel.Visible = false;

            ShowVehiclesStatistics(sender, e);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
        }
    }
    DataTable CreateVehiclesInfoSource(List<int> dataBlocksIds, DataBlock dataBlock)
    {
        DataTable dt = new DataTable();
        DataRow dr;

        dt.Columns.Add(new DataColumn("#", typeof(int)));
        dt.Columns.Add(new DataColumn("Имя файла", typeof(string)));
        dt.Columns.Add(new DataColumn("Тип карты", typeof(string)));
        dt.Columns.Add(new DataColumn("Период С", typeof(string)));
        dt.Columns.Add(new DataColumn("По", typeof(string)));
        dt.Columns.Add(new DataColumn("Кол-во записей", typeof(int)));
        dt.Columns.Add(new DataColumn("Дата разбора", typeof(string)));
        int i = 1;
        List<DateTime> startEnd = new List<DateTime>();
        foreach (int id in dataBlocksIds)
        {
            dr = dt.NewRow();
            dr["#"] = i++;
            dr["имя файла"] = dataBlock.GetDataBlock_FileName(id);
            dr["Тип карты"] = dataBlock.GetCardTypeName(id);
            startEnd = dataBlock.vehicleUnitInfo.Get_StartEndPeriod(id);
            dr["Период С"] = startEnd[0].ToLongDateString();
            dr["По"] = startEnd[1].ToLongDateString();
            dr["Кол-во записей"] = dataBlock.GetDataBlock_RecorsCount(id);
            dr["Дата разбора"] = dataBlock.GetDataBlock_EDate(id);
            dt.Rows.Add(dr);
        }
        return dt;
    }
    ///vehiclesPreview
    protected void Vehicles_FileContents(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleFileContent_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();
            VehichleUnit.VehicleUnitClass vehicleCard = new VehichleUnit.VehicleUnitClass();

            vehicleCard = dataBlock.vehicleUnitInfo.GetAllVehicleUnitClass_parsingDataBlock(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_FileContents(vehicleCard);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_Identification(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleIdentification_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();
            VehichleUnit.Vehicle_Overview vehicleCard = new VehichleUnit.Vehicle_Overview();

            vehicleCard.vehicleIdentificationNumber.vehicleIdentificationNumber = dataBlock.vehicleUnitInfo.Get_VehicleOverview_IdentificationNumber(dataBlockId);
            vehicleCard.vehicleRegistrationIdentification = dataBlock.vehicleUnitInfo.Get_VehicleOverview_RegistrationIdentification(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_Identification(vehicleCard);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_CurrentDateTime(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleCurrentDateTime_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();
            DDDClass.CurrentDateTime time = new DDDClass.CurrentDateTime(dataBlock.vehicleUnitInfo.Get_VehicleOverview_CurrentDateTime(dataBlockId).timereal);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_CurrentDateTime(time);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_DownloadablePeriod(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleDownloadablePeriod_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();
            DDDClass.VuDownloadablePeriod period = new DDDClass.VuDownloadablePeriod();
            period = dataBlock.vehicleUnitInfo.Get_VehicleOverview_VuDownloadablePeriod(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_DownloadablePeriod(period);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_InsertedCardType(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleInsertedCardType.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();

            DDDClass.CardSlotsStatus slotsStatus = new DDDClass.CardSlotsStatus();
            slotsStatus = dataBlock.vehicleUnitInfo.Get_VehicleOverview_CardSlotsStatus(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_InsertedCardType(slotsStatus);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_DownloadActivityData(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleDownloadActivityData_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();
            DDDClass.VuDownloadActivityData data = new DDDClass.VuDownloadActivityData();
            data = dataBlock.vehicleUnitInfo.Get_VehicleOverview_VuDownloadActivityData(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_DownloadActivityData(data);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_CompanyLocksData(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleCompanyLocksData_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();

            List<DDDClass.VuCompanyLocksRecord> locks = new List<DDDClass.VuCompanyLocksRecord>();
            locks = dataBlock.vehicleUnitInfo.Get_VehicleOverview_VuCompanyLocksData(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_CompanyLocksData(locks);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_ControlActivityData(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleControlActivityData_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();

            List<DDDClass.VuControlActivityRecord> records = new List<DDDClass.VuControlActivityRecord>();
            records = dataBlock.vehicleUnitInfo.Get_VehicleOverview_VuControlActivityData(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_ControlActivityData(records);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_EventData(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleEventData_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();

            List<DDDClass.VuEventRecord> events = new List<DDDClass.VuEventRecord>();
            events = dataBlock.vehicleUnitInfo.Get_VehicleEventsAndFaults_VuEventData(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_EventData(events);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_FaultData(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleFaultData_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();

            List<DDDClass.VuFaultRecord> faults = new List<DDDClass.VuFaultRecord>();
            faults = dataBlock.vehicleUnitInfo.Get_VehicleEventsAndFaults_VuFaultData(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_FaultData(faults);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_OverSpeedingControlData(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleOverSpeedingControlData_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();
            DDDClass.VuOverSpeedingControlData overSpeed = dataBlock.vehicleUnitInfo.Get_VehicleEventsAndFaults_VuOverSpeedingControlData(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_OverspeedingControlData(overSpeed);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_OverSpeedingEventData(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleOverSpeedingEventData_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();
            List<DDDClass.VuOverSpeedingEventRecord> events = dataBlock.vehicleUnitInfo.Get_VehicleEventsAndFaults_VuOverSpeedingEventData(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_OverspeedingEventData(events);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_TimeAdjustmentData(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleTimeAdjustmentData_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
            dataBlock.OpenConnection();
            List<DDDClass.VuTimeAdjustmentRecord> records = dataBlock.vehicleUnitInfo.Get_VehicleEventsAndFaults_VuTimeAdjustmentData(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_TimeAdjustmentData(records);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_DetailedSpeedData(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleDetailedSpeedData_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
            dataBlock.OpenConnection();
            List<DDDClass.VuDetailedSpeedBlock> records = dataBlock.vehicleUnitInfo.Get_VehicleDetailedSpeed_VuDetailedSpeedData(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_DetailedSpeedData(records);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_FullIdentificationData(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleFullIdentification_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
            dataBlock.OpenConnection();
            DDDClass.VuIdentification ident = dataBlock.vehicleUnitInfo.Get_VehicleTechnicalData_VuIdentification(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_VuIdentification(ident);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_SensorPaired(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleSensorPaired_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);
            dataBlock.OpenConnection();

            DDDClass.SensorPaired sensorPaired = dataBlock.vehicleUnitInfo.Get_VehicleTechnicalData_SensorPaired(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_SensorPaired(sensorPaired);
            FilesPreviewDataGrid.DataBind();
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    protected void Vehicles_CalibrationData(object sender, EventArgs e)
    {
        EnableAllVehiclesPreviewButtons();
        VehicleCalibrationData_btn.Enabled = false;
        FilesPreviewPanel.Visible = true;

        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            int dataBlockId = Convert.ToInt32(onlyForInternal.Value);

            dataBlock.OpenConnection();
            List<DDDClass.VuCalibrationRecord> records = dataBlock.vehicleUnitInfo.Get_VehicleTechnicalData_VuCalibrationData(dataBlockId);

            FilesPreviewDataGrid.DataSource = VehiclePreviewDataTable.VehiclePreview_CalibrationData(records);
            FilesPreviewDataGrid.DataBind();
            FilesPreviewPanel.Style.Add("width", "2500px");
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            TextBoxTest.Text = "";
            TextBoxTest.Visible = false;
            StatusUpdatePanel.Update();
        }
        finally
        {
            dataBlock.CloseConnection();
            ModalPopupExtender1.Hide();
        }
    }
    private void EnableAllVehiclesPreviewButtons()
    {
        FilesPreviewPanel.Style.Add("width", "100%");

        VehicleFileContent_btn.Enabled = true;
        VehicleIdentification_btn.Enabled = true;
        VehicleCurrentDateTime_btn.Enabled = true;
        VehicleDownloadablePeriod_btn.Enabled = true;
        VehicleInsertedCardType.Enabled = true;
        VehicleDownloadActivityData_btn.Enabled = true;
        VehicleCompanyLocksData_btn.Enabled = true;
        ControlActivityData_btn.Enabled = true;
        VehicleFaultData_btn.Enabled = true;
        VehicleEventData_btn.Enabled = true;
        VehicleOverSpeedingControlData_btn.Enabled = true;
        VehicleOverSpeedingEventData_btn.Enabled = true;
        VehicleTimeAdjustmentData_btn.Enabled = true;
        VehicleDetailedSpeedData_btn.Enabled = true;
        VehicleFullIdentification_btn.Enabled = true;
        VehicleSensorPaired_btn.Enabled = true;
        VehicleControlActivityData_btn.Enabled = true;
        VehicleCalibrationData_btn.Enabled = true;

        TextBoxTest.Text = "";
        FilesPreviewDataGrid.DataSource = null;
        FilesPreviewDataGrid.DataBind();
        FilesPreviewPanel.Visible = false;
    }

    ////////////////////managment tab!!!!!!!!!!!!/////////////////////////////
    protected void LoadDeleteTable(object sender, EventArgs e)
    {
        try
        {
            Status.Text = "";
            LoadAllDeleteLists();           
            Session["DeleteCommmand"] = "RemoveParsedData";
            SetDelColVisible(true);
            setParseButtonVisible(false);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }
    private struct DeleteStructure
    {
        public int dataBlockId { get; set; }
        public int number { get; set; }
        public string name { get; set; }
        public string cardTypeName { get; set; }
        public string createTime { get; set; }
        public int recordsCount { get; set; }
        public string dataBlockState { get; set; }
    }
    private List<DeleteStructure> LoadAllDeleteLists()
    {
        try
        {
            int userId = 0;
            List<DeleteStructure> deleteStructureList = new List<DeleteStructure>();
            int i = 0;
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            DeleteStructure deleteStructureTemp;
            List<int> dataBlockIds = new List<int>();
            dataBlock.OpenConnection();
            dataBlockIds = dataBlock.GetAllParsedDataBlockIDs(userId);
            foreach (int dataBlockId in dataBlockIds)
            {
                i++;
                deleteStructureTemp = new DeleteStructure();
                deleteStructureTemp.dataBlockId = dataBlockId;
                deleteStructureTemp.number = i;
                deleteStructureTemp.name = dataBlock.GetDriversNameOrVehiclesNumberByBlockId(dataBlockId);
                deleteStructureTemp.cardTypeName = dataBlock.GetCardTypeName(dataBlockId);
                deleteStructureTemp.recordsCount = dataBlock.GetDataBlock_RecorsCount(dataBlockId);
                deleteStructureTemp.createTime = dataBlock.GetDataBlock_EDate(dataBlockId);
                deleteStructureTemp.dataBlockState = dataBlock.GetDataBlockState(dataBlockId);

                deleteStructureList.Add(deleteStructureTemp);
            }
            if (dataBlockIds.Count == 0)
                Status.Text = "Нет записей для отображения";
            
            AddGrid.DataSource = CreateDeleteDataSource(deleteStructureList);
            AddGrid.DataBind();
            dataBlock.CloseConnection();
            return deleteStructureList;
        }
        catch (Exception ex)
        {
            Status.Text = "Произошла ошибка: " + ex.Message + " Возможо просто нету данных в БД. Исправление в след. версии";
            return null;
        }
        finally
        {
        }
    }
    DataTable CreateDeleteDataSource(List<DeleteStructure> data) //0 - Водители, 1 - ТС
    {
        DataTable dt = new DataTable();
        DataRow dr;

        dt.Columns.Add(new DataColumn("#", typeof(int)));
        dt.Columns.Add(new DataColumn("Имя водителя/Номер ТС", typeof(string)));
        dt.Columns.Add(new DataColumn("Тип карты", typeof(string)));
        dt.Columns.Add(new DataColumn("Колл-во записей", typeof(int)));
        dt.Columns.Add(new DataColumn("Дата разбора", typeof(string)));
        dt.Columns.Add(new DataColumn("Состояние", typeof(string)));

        foreach (DeleteStructure item in data)
        {
            dr = dt.NewRow();
            dr["#"] = item.number;
            dr["Имя водителя/Номер ТС"] = item.name;
            dr["Тип карты"] = item.cardTypeName;
            dr["Колл-во записей"] = item.recordsCount;
            dr["Дата разбора"] = item.createTime;
            dr["Состояние"] = item.dataBlockState;
            dt.Rows.Add(dr);
        }
        return dt;
    }
    protected void DelGridCommand(Object sender, DataGridCommandEventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        List<int> dataBlockIds = new List<int>();
        int cardType;
        Status.Text = "";
        try
        {
            if (((ImageButton)e.CommandSource).CommandName == "RemoveData")
            {
                DeleteStructure delStr = new DeleteStructure();
                int rightIdToDelete = -1;
                delStr.name = e.Item.Cells[3].Text;
                delStr.createTime = e.Item.Cells[6].Text;
                delStr.recordsCount = int.Parse(e.Item.Cells[5].Text);
                dataBlockIds = dataBlock.GetDataBlockIdByRecordsCount(delStr.recordsCount);

                if (dataBlockIds.Count == 1)
                {
                    rightIdToDelete = dataBlockIds[0];
                }
                else
                    if (dataBlockIds.Count > 1)
                    {
                        string nameTemp;
                        foreach (int blockId in dataBlockIds)
                        {
                            nameTemp = dataBlock.GetDriversNameOrVehiclesNumberByBlockId(blockId);
                            if (nameTemp == delStr.name)
                            {
                                rightIdToDelete = blockId;
                                break;
                            }
                        }
                        //Обязательно сделать проверку, по дате добавления...(будет не лишним по-любому!)
                    }
                    else
                        throw new Exception("Нет доступа к информации(или нет данных для удаления)");
                if (rightIdToDelete != -1)
                {
                    dataBlock = new DataBlock(connectionString, rightIdToDelete, "STRING_EN");
                    dataBlock.DeleteDataBlockAndRecords();
                    LoadAllDeleteLists();
                }
                else throw new Exception("Не найдена запись для удаления");
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }
    protected void AddGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (AccordionCurrentTabIndex_HiddenField.Value == "1")
        {
            //  if (e.Item.ItemType.Equals(ListItemType.Item))
            // {
            ImageButton ImageButton1 = (ImageButton)e.Item.FindControl("RemoveDataId");
            if (ImageButton1 != null)
                ImageButton1.ImageUrl = "../images/icons/save.png";
            //  }
        }
    }
    protected void ManagmentLoadedInfo_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            if (ManagmentLoadedInfo.SelectedNode.ChildNodes.Count == 0)
            {
                driversCardEditPanelVisible(true);
                Session["DeleteCommmand"] = "RemoveDriversFile";
                int cardId = Convert.ToInt32(ManagmentLoadedInfo.SelectedValue);
                SelectedManagmentDriver.Text = ManagmentLoadedInfo.SelectedNode.Text;
                List<Driver> driversStructureList = new List<Driver>();
                driversStructureList = LoadAllDriversLists(cardId);
                AddGrid.DataSource = CreateDriversInfoSource(driversStructureList);
                AddGrid.DataBind();
                SetDelColVisible(true);
                setParseButtonVisible(false);
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            StatusUpdatePanel.Update();
        }
    }
    protected void UserFileRecoverTree_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            setParseButtonVisible(false);
            if (UserFileRecover_TreeView.SelectedNode.ChildNodes.Count == 0)
            {
                driversCardEditPanelVisible(true);
                Session["DeleteCommmand"] = "RecoverDriversFile";
                int cardId = Convert.ToInt32(UserFileRecover_TreeView.SelectedValue);
                //SelectedManagmentDriver.Text = ManagmentLoadedInfo.SelectedNode.Text;
                List<Driver> driversStructureList = new List<Driver>();
                driversStructureList = LoadAllDriversLists(cardId);
                AddGrid.DataSource = CreateDriversInfoSource(driversStructureList);
                AddGrid.DataBind();
                SetDelColVisible(true);
            }
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
            StatusUpdatePanel.Update();
        }
    }
    protected void ChangeDriversNameClick(object sender, EventArgs e)
    {
        try
        {
            setNameChangeVisible(true);
           
            string name = ManagmentLoadedInfo.SelectedNode.Text.Split(' ')[0];
            string surName = ManagmentLoadedInfo.SelectedNode.Text.Split(' ')[1];
            new_DriversName.Text = name;
            new_DriversSurName.Text = surName;
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }
    protected void ChangeDriversNumberClick(object sender, EventArgs e)
    {
        try
        {
            setNameChangeVisible(false);

            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            int cardId = Convert.ToInt32(ManagmentLoadedInfo.SelectedValue);
            List<int> arrayIds = new List<int>();
            arrayIds.Add(cardId);
            string number = dataBlock.cardsTable.GetCardNumbers(arrayIds)[0];

            new_DriversCardNumber.Text = number;
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
    }
    protected void makeChangeDriversNameAndSurName(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        int cardId = Convert.ToInt32(ManagmentLoadedInfo.SelectedValue);
        string newName = new_DriversName.Text;
        string newSurName = new_DriversSurName.Text;

        dataBlock.cardsTable.ChangeCardName(newName + " " + newSurName, cardId);

        DriversSelectTree.Nodes.Clear();
        ManagmentLoadedInfo.SelectedNode.Text = newName + " " + newSurName;
        SelectedManagmentDriver.Text = ManagmentLoadedInfo.SelectedNode.Text;
        LoadDriversEveryWhere(false);
        makeChangeDriversInfo_cancel(null, null);
    }
    protected void makeChangeDriversNumber(object sender, EventArgs e)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        int cardId = Convert.ToInt32(ManagmentLoadedInfo.SelectedValue);

        string newNumber = new_DriversCardNumber.Text;
        int userId = dataBlock.cardsTable.GetCardUserId(cardId);

        dataBlock.cardsTable.ChangeCardNumber(newNumber, cardId, userId);
        makeChangeDriversInfo_cancel(null, null);
    }
    protected void makeChangeDriversInfo_cancel(object sender, EventArgs e)
    {
        driversCardEditPanelVisible(false);
        driversCardEditPanelVisible(true);
    }
    protected void DeleteDriversCardWithAllInfo(object sender, EventArgs e)
    {
        try
        {
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
            int cardId = Convert.ToInt32(ManagmentLoadedInfo.SelectedValue);

            dataBlock.cardsTable.DeleteCardAndAllFiles(cardId);
        }
        catch (Exception ex)
        {
            Status.Text = ex.Message;
        }
        
    }
    ////////////////////////FORALL!!!!!!!!!!!!!!///////////////////////////////////////
    protected void ShowDriversStatistics(object sender, EventArgs e)
    {
        List<int> cardIds = getIdList(DriversSelectTree.SelectedValue);
        List<int> dataBlockIds = new List<int>();
        FilesPreviewPanel.Visible = false;
        //AddGridPanel.Visible=false;
        //AddGrid.DataSource = null;
        //AddGrid.DataBind();
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        dataBlock.OpenConnection();
        foreach (int id in cardIds)
        {
            dataBlockIds.AddRange(dataBlock.cardsTable.GetAllDataBlockIds_byCardId(id));
        }
        dataBlock.CloseConnection();
        LoadDataStatistics1.CheckPlfFiles(ViewPLF.Checked);
        LoadDataStatistics1.LoadAllDriversDateStatistics(dataBlockIds);
    }
    protected void ShowDriversStatisticsForOneFile(object sender, EventArgs e)
    {
        List<int> dataBlockIds = new List<int>();
        FilesPreviewPanel.Visible = false;
        //AddGridPanel.Visible = false;
        //AddGrid.DataSource = null;
        //AddGrid.DataBind();
        dataBlockIds.Add(Convert.ToInt32(onlyForInternal.Value));
        LoadDataStatistics1.CheckPlfFiles(false);
        LoadDataStatistics1.LoadAllDriversDateStatistics(dataBlockIds);
    }
    protected void ShowVehiclesStatistics(object sender, EventArgs e)
    {
        List<int> cardIds = getIdList(VehiclesSelectTree.SelectedValue);
        FilesPreviewPanel.Visible = false;
        //AddGridPanel.Visible = false;
        //AddGrid.DataSource = null;
        //AddGrid.DataBind();
        List<int> dataBlockIds = new List<int>();
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        dataBlock.OpenConnection();
        foreach (int id in cardIds)
        {
            dataBlockIds.AddRange(dataBlock.cardsTable.GetAllDataBlockIds_byCardId(id));
        }
        dataBlock.CloseConnection();
        LoadDataStatistics1.CheckPlfFiles(false);
        LoadDataStatistics1.LoadAllVehiclesDateStatistics(dataBlockIds);
    }
//----------------------------------------------------------------------------------
    private List<int> getIdList(string input)
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
    }

    private void SetDelColVisible(bool YES)
    {
        if(YES)
        {
            AddGrid.Columns[0].Visible = true;
            AddGrid.Columns[1].Visible = false;
        }
        else
        {
            AddGrid.Columns[0].Visible = false;
            AddGrid.Columns[1].Visible = true;
        }
            
    }

    private void setParseButtonVisible(bool YES)
    {
        if (YES)
        {
            Parse_Button.Visible = true;
        }
        else
        {
            Parse_Button.Visible = false;
        }
    }
    private void setStatistiscPanelVisible(bool YES)
    {
        if (YES)
        {
            LoadDataStatistics1.Visible = true;
        }
        else
        {
            LoadDataStatistics1.ClearAllData();
            LoadDataStatistics1.Visible = false;
        }
        DataStatisticsUpdatePanel.Update();
    }

    private void setNameChangeVisible(bool YES)
    {
        if (YES)
        {
            DriverCardEditUpdatePanel_NameEdit.Visible = true;
            DriverCardEditUpdatePanel_NumberEdit.Visible = false;
            DriversNameChangeButton.Enabled = false;
            DriversNumberChangeButton.Enabled = true;
        }
        else
        {
            DriverCardEditUpdatePanel_NameEdit.Visible = false;
            DriverCardEditUpdatePanel_NumberEdit.Visible = true;
            DriversNameChangeButton.Enabled = true;
            DriversNumberChangeButton.Enabled = false;
        }
    }

    private void driversCardEditPanelVisible(bool YES)
    {
        if(YES)
        {
            DriversCardEditButtonsPanel.Visible = true;
            DriverCardEditFormsPanel.Visible = true;
        }
        else
        {
            DriversCardEditButtonsPanel.Visible = false;
            DriverCardEditFormsPanel.Visible = false;
            DriversNameChangeButton.Enabled = true;
            DriversNumberChangeButton.Enabled = true;
            DriverCardEditUpdatePanel_NumberEdit.Visible = false;
            DriverCardEditUpdatePanel_NameEdit.Visible = false;
        }
    }

}
