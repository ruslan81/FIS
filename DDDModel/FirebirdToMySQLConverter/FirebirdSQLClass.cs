using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using BLL;
using PLFUnit;
using System.Reflection;
using DB.SQL;
using DB.Interface;

namespace FirebirdToMySQLConverter
{
    /// <summary>
    /// Конвертирует базу из FireBird в MySQL
    /// </summary>
    public class FirebirdSQLClass
    {
        FbConnection fb_con;
        string connectionString;
        string connectionStringMysql;
        string currentLanguage = "STRING_EN";

        public FirebirdSQLClass(string connectionStringMysqlTMP)
        {
            connectionString = "User=SYSDBA;Password=masterkey;Database=C:\\WORK\\!!!!RELEASE_1\\MARKETING.GDB;DataSource=localhost;"
                + "Port=3050;Dialect=3; Charset=NONE;Role=;Connection lifetime=15;Pooling=true; MinPoolSize=0;"
                + "MaxPoolSize=50;Packet Size=8192;ServerType=0;";
            connectionStringMysql = connectionStringMysqlTMP;

            fb_con = new FbConnection(connectionString);
        }

        public void LoadAllInfo()        
        {
            Console.WriteLine("Conerting starts");
            DateTime startLoadTime = DateTime.Now;


            PLFUnit.PLFUnitClass plf;

            CARS cars = new CARS(fb_con);
            cars.LoadAllCars();

            DEPARTMENTS depar = new DEPARTMENTS(fb_con);
            depar.LoadAllDepartments();

            WORKERS workers = new WORKERS(fb_con);
            workers.LoadAllWorkers();

            DEVICES devices = new DEVICES(fb_con);
            devices.LoadAllDevices();

            READCYCLE readcycle = new READCYCLE(fb_con);
            readcycle.LoadAllReadCycles();

            DataBlock datablock = new DataBlock(connectionStringMysql, currentLanguage);
            try
            {
                List<KeyValuePair<int, int>> orgOldNewIds = new List<KeyValuePair<int, int>>();
                List<KeyValuePair<int, int>> workOldNewIds = new List<KeyValuePair<int, int>>();
                List<KeyValuePair<int, int>> carsOldNewIds = new List<KeyValuePair<int, int>>();
                List<KeyValuePair<READCYCLE.readcycle, int>> readCycleAndorgIdList = new List<KeyValuePair<READCYCLE.readcycle, int>>();

                List<int> allIds = new List<int>();
                int newDeviceId = -1;
                int userInfoId;
                KeyValuePair<int, int> tempOldNewIds;
                List<int> addedWorkers = new List<int>();
                datablock.OpenConnection();
                datablock.OpenTransaction();
                //Создание организация, работников и ТС
                foreach (DEPARTMENTS.department department in depar.departments)
                {
                    addedWorkers = new List<int>();
                    int orgId = datablock.organizationTable.AddNewOrganization(department.DEPARTMENT, 1, 1, 1);
                    tempOldNewIds = new KeyValuePair<int, int>(department.ID, orgId);
                    orgOldNewIds.Add(tempOldNewIds);
                    int carId = -1;
                    #region "cars"
                    IEnumerable<CARS.car> carsList =
                       from car in cars.carsArray
                       where car.DEPARTMENTID == department.ID
                       select car;

                    foreach (CARS.car car in carsList)
                    {
                        IEnumerable<int> scoreQuery =
                        from devId in readcycle.cycles
                        where devId.CARID == car.ID
                        select devId.DEVICEID;
                        allIds = scoreQuery.ToList();

                        if (allIds.Count > 0)
                        {
                            IEnumerable<DEVICES.device> oneDevice =
                            from devId in devices.devices
                            where devId.ID == allIds[0]
                            select devId;

                            if (oneDevice.ToList().Count > 0)
                            {
                                DEVICES.device NewDevice = oneDevice.ToList()[0];
                                newDeviceId = datablock.deviceTable.AddNewDevice(1, NewDevice.DEVICE, NewDevice.VERSION, DateTime.Now, 1, 23442324);
                            }
                            else
                                newDeviceId = datablock.deviceTable.AddNewDevice(1, "UndefinedDevice", "UndefinedVersion", DateTime.Now, 1, 23442324);
                        }
                        else
                            newDeviceId = datablock.deviceTable.AddNewDevice(1, "UndefinedDevice", "UndefinedVersion", DateTime.Now, 1, 23442324);

                        if (car.DEPARTMENTID == department.ID)
                        {
                            int cardId = datablock.cardsTable.CreateNewCard(car.REGNUMBER, "Unknown", datablock.cardsTable.vehicleCardTypeId, orgId, "Created from FIREBIRD database", 0, 1);
                            carId = datablock.vehiclesTables.AddNewVehicle(car.REGNUMBER, car.CARMODEL, "Unknown", 1, newDeviceId, cardId, DateTime.Now, 1);
                            tempOldNewIds = new KeyValuePair<int, int>(car.ID, carId);
                            carsOldNewIds.Add(tempOldNewIds);
                        }
                    }
                    #endregion
                    #region "workers"
                    IEnumerable<WORKERS.worker> workersList =
                         from worker in workers.workers
                         where worker.DEPARTMENTID == department.ID
                         select worker;

                    foreach (WORKERS.worker workerForAdd in workersList)
                    {
                        int newWId = datablock.cardsTable.CreateNewCard(workerForAdd.FIRSTNAME + " " + workerForAdd.SURNAME, workerForAdd.WORKERINTID.ToString(), datablock.cardsTable.driversCardTypeId,
                            orgId, "Created from firebird", 0, 1);
                        /*userInfoId = datablock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Patronimic);
                        datablock.usersTable.EditUserInfo(newWId, userInfoId, workerForAdd.MIDDLENAME);
                        userInfoId = datablock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Name);
                        datablock.usersTable.EditUserInfo(newWId, userInfoId, workerForAdd.FIRSTNAME);
                        userInfoId = datablock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Surname);
                        datablock.usersTable.EditUserInfo(newWId, userInfoId, workerForAdd.SURNAME);
                        userInfoId = datablock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_Birthday);
                        datablock.usersTable.EditUserInfo(newWId, userInfoId, workerForAdd.BIRTHDAY.ToShortDateString());
                        userInfoId = datablock.usersTable.GetUserInfoNameId(DataBaseReference.UserInfo_DriversCertificate);
                        datablock.usersTable.EditUserInfo(newWId, userInfoId, workerForAdd.LICENCE);*/
                        addedWorkers.Add(workerForAdd.ID);
                        tempOldNewIds = new KeyValuePair<int, int>(workerForAdd.ID, newWId);
                        workOldNewIds.Add(tempOldNewIds);
                    }
                    #endregion
                    #region "set orgId to READCYCLES"
                    if(addedWorkers.Count>0)
                        for(int i=0;i<readcycle.cycles.Count;i++)
                        {
                            if(addedWorkers.Contains(readcycle.cycles[i].WORKER1ID) || addedWorkers.Contains(readcycle.cycles[i].WORKER2ID))
                            {
                                KeyValuePair<READCYCLE.readcycle, int> readCycleAndorgId = new KeyValuePair<READCYCLE.readcycle, int>(readcycle.cycles[i], orgId);
                                readCycleAndorgIdList.Add(readCycleAndorgId);
                            }
                        }
                    #endregion
                }
                Console.WriteLine("Added workers, departmens, cars");

                if (readCycleAndorgIdList.Count != readcycle.cycles.Count)
                {
                    if (false) //база фигово связана
                        throw new Exception("Куда-то пропала часть инфы");
                }
                //Загрузка файлов ПЛФ в ранее созданные организации.
                foreach (KeyValuePair<READCYCLE.readcycle, int> readedcycle in readCycleAndorgIdList)
                {
                    SQLDB sqlDb = datablock.sqlDb;
                    SQLDB_Records sqlDB_rec = new SQLDB_Records(connectionStringMysql, sqlDb.GETMYSQLCONNECTION());
                    ReflectObjectToTableClass reflectedItemsList;
                    byte[] bytes = Guid.NewGuid().ToByteArray();
                    int orgId = readedcycle.Value;
                    Type type = null;
                    object myParseObject = new object();
                    plf = new PLFUnitClass();
                    plf = LoadLogBook(readedcycle.Key);
                    plf.cardType = 2;

                    IEnumerable<string> selCar =
                            from car in cars.carsArray
                            where car.ID == readedcycle.Key.CARID
                            select car.REGNUMBER;
                    if(selCar.ToList().Count>0)
                        plf.VEHICLE = selCar.ToList()[0];


                    IEnumerable<string> selDeviceName =
                            from dev in devices.devices
                            where dev.ID == readedcycle.Key.DEVICEID
                            select dev.DEVICE;
                    if (selDeviceName.ToList().Count > 0)
                        plf.ID_DEVICE = selDeviceName.ToList()[0];

                   
                    int driverId = -1;
                    foreach (KeyValuePair<int, int> oldNew in workOldNewIds)
                    {
                        if (oldNew.Key == readedcycle.Key.WORKER1ID)
                            driverId = oldNew.Value;
                    }
                    int dataBlockId = datablock.AddPlfTypeData(orgId, bytes, 
                        "AutoGenerated " + plf.VEHICLE + " " + plf.START_PERIOD.ToString() + " - " + plf.END_PERIOD.ToString(),
                        driverId);
                    DataRecords dataRecord = new DataRecords(connectionString, dataBlockId, currentLanguage, sqlDb);

                        //////////////////////устанавливаем PLF карту нужного водителя. Незнаю почему именно здесь, но так получилось.
                    int plfDriversCardType = sqlDB_rec.Get_DataBlockCardType(dataBlockId);
                    sqlDb.SetDataBlock_CardId(dataBlockId, plfDriversCardType);
                       // sqlDB.OpenConnection();
                        int cardTypeParamId = sqlDb.AddParam("cardType", 0, 255);
                        //sqlDB.OpenConnection();
                        sqlDb.DeleteDataRecord(dataBlockId, cardTypeParamId);
                       // sqlDB.CloseConnection();
                        //////////////////////
                        type = plf.GetType();
                        myParseObject = plf;
                   
                    //sqlDB.OpenConnection();
                    SetParseBDate(sqlDb, dataBlockId);
                    //sqlDB.CloseConnection();

                    List<ReflectObjectToTableClass> allRecordsToAdd = new List<ReflectObjectToTableClass>();

                    foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        reflectedItemsList = new ReflectObjectToTableClass();
                        string fieldName = pi.Name;
                        object field2 = pi.GetValue(myParseObject, null);

                        if (field2 != null)
                        {
                            reflectedItemsList.ReflectObjectToTable(fieldName, field2);//Не удалять!
                            allRecordsToAdd.Add(AddRecords(reflectedItemsList, sqlDb));//не удалять
                        }
                    }

                    foreach (ReflectObjectToTableClass recordList in allRecordsToAdd)
                    {
                        dataRecord.AddDataArray(recordList.reflectedItemsList);
                    }
                    
                    //sqlDB.OpenConnection();
                    SetParseEDate(sqlDb,dataBlockId);
                    sqlDb.SetDataBlockState(dataBlockId, 2);
                    int dataBlockParseRecords = sqlDb.SetDataBlockParseRecords(dataBlockId);
                    Console.WriteLine("" + dataBlockParseRecords.ToString()+" records added");
                }

                Console.WriteLine("\n\r" + "Время начала импорта " + startLoadTime.ToShortDateString() + " " + startLoadTime.ToShortTimeString());
                Console.WriteLine("\n\r" + "Время окончания импорта " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                //throw new Exception();
                datablock.CommitTransaction();
                datablock.CloseConnection();

            }
            catch (Exception ex)
            {
                datablock.RollbackConnection();
                datablock.CloseConnection();
                Console.WriteLine(ex.Message);
            }
        }

        private ReflectObjectToTableClass AddRecords(ReflectObjectToTableClass reflectionClass, SQLDB sqlDb)
        {
            Params parameters = new Params();
            ReflectObjectToTableClass myAddedParameters = new ReflectObjectToTableClass();
            //SQLDB sqlDB = new SQLDB(connectionString);
            string lastCashName = "";
            int paramId = -1;

            // sqlDB.OpenConnection();
            //sqlDB.OpenTransaction();
           // Console.WriteLine("Adding Parameters....");
            foreach (int n in reflectionClass.getAllItemsLevels())
            {
                foreach (ReflectionClass item in reflectionClass.GetItemsByLevel(n))
                {
                    /*Новая версия с развитым кешем*/
                    paramId = GetParamIdFromCashe(item.name, -1, true);
                    if (paramId == -1)
                    {
                        paramId = parameters.AddParam(item.name, item.GetParentParamName(), 0, sqlDb);
                        if (paramId == -1)
                        {
                            throw new Exception("Error adding parameters!");
                        }
                        GetParamIdFromCashe(item.name, paramId, false);
                    }
                    /*Новая версия с развитым кешем*/

                    ReflectionClass r = new ReflectionClass();
                    r.name = item.name;
                    r.PARAM_ID = paramId;  //старая версия с одним кешем

                    r.value = item.value;
                    myAddedParameters.reflectedItemsList.Add(r);
                }
            }
            return myAddedParameters;
        }

        private List<string> namesForCashe;
        private List<int> idsForCashe;
        private int GetParamIdFromCashe(string name, int id, bool checkOnly)//при переполнении сделать выталкивание а не обнуление
        {
            int MaxCasheCount = 30;
            KeyValuePair<string, int> thisItem = new KeyValuePair<string, int>(name, id);
            if (namesForCashe == null)
            {
                namesForCashe = new List<string>();
                idsForCashe = new List<int>();
            }
            int index = namesForCashe.IndexOf(thisItem.Key);
            if (index > -1)
            {
                return idsForCashe[index];
            }
            else
            {
                if (checkOnly)
                    return -1;
                if (namesForCashe.Count > MaxCasheCount)
                {
                    namesForCashe = new List<string>();
                    idsForCashe = new List<int>();
                }
                namesForCashe.Add(thisItem.Key);
                idsForCashe.Add(thisItem.Value);
            }
            return -1;
        }

        private void SetParseBDate(SQLDB sqlDb, int dataBlockId)
        {
            DateTime dt = sqlDb.SetCurrentTime("fn_data_block", "DATA_BLOCK_ID", dataBlockId, "PARSE_BDATE");
            Console.WriteLine("\r\nParse Begin Time " + dt.ToString("dd-MM-yyyy HH:mm:ss"));
        }

        private void SetParseEDate(SQLDB sqlDb, int dataBlockId)
        {
            DateTime dt = sqlDb.SetCurrentTime("fn_data_block", "DATA_BLOCK_ID", dataBlockId, "PARSE_EDATE");
            Console.WriteLine("Parse End Time " + dt.ToString("dd-MM-yyyy HH:mm:ss"));
        }      

        private PLFUnitClass LoadLogBook(READCYCLE.readcycle readcycle)
        {
            PLFUnitClass plf = new PLFUnitClass();
            plf.START_PERIOD.systemTime = readcycle.TIMEFROM.ToString("yy:MM:dd HH:mm:ss");
            plf.END_PERIOD.systemTime = readcycle.TIMETO.ToString("yy:MM:dd HH:mm:ss");
            plf.TIME_STEP = readcycle.TIMESTEP.ToString();
            plf.installedSensors = readcycle.installedSensors;

            fb_con.Open();
            string sql = "SELECT * FROM LOGBOOK WHERE READCYCLEID=@READCYCLEID ORDER BY ID";
            FbCommand cmd = new FbCommand(sql, fb_con);
            cmd.Parameters.AddWithValue("@READCYCLEID", readcycle.ID);
            int i = 0;
            PLFRecord onerecord = new PLFRecord();
            DateTime previewDate = new DateTime();
            string gettedValue;
            using (FbDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    onerecord = new PLFRecord();

                    if (plf.installedSensors.SYSTEM_TIME.systemTime != null)
                    {
                        gettedValue = r["READTIME"].ToString();
                        if (previewDate == new DateTime())
                        {
                            onerecord.SYSTEM_TIME.systemTime = DateTime.Parse(gettedValue).ToString("yy:MM:dd HH:mm:ss");
                            previewDate = DateTime.Parse(gettedValue);
                        }
                        else
                        {
                            if (DateTime.Parse(gettedValue) == previewDate)
                            {
                                onerecord.SYSTEM_TIME.systemTime = (DateTime.Parse(gettedValue).AddSeconds(Convert.ToInt32(plf.TIME_STEP))).ToString("yy:MM:dd HH:mm:ss");
                                previewDate = DateTime.Parse(gettedValue);
                            }
                            else
                            {
                                onerecord.SYSTEM_TIME.systemTime = DateTime.Parse(gettedValue).ToString("yy:MM:dd HH:mm:ss");
                                previewDate = DateTime.Parse(gettedValue);
                            }
                        }

                    }
                    if (plf.installedSensors.FUEL_CONSUMPTION != null)
                        onerecord.FUEL_CONSUMPTION = r["FUEL0"].ToString();
                    if (plf.installedSensors.FUEL_VOLUME1 != null)
                        onerecord.FUEL_VOLUME1 = r["FUEL1"].ToString();
                    if (plf.installedSensors.FUEL_VOLUME2 != null)
                        onerecord.FUEL_VOLUME2 = r["FUEL2"].ToString();
                    //FUELPH
                    if (plf.installedSensors.VOLTAGE != null)
                        onerecord.VOLTAGE = r["BATTERY"].ToString();
                    if (plf.installedSensors.ENGINE_RPM != null)
                        onerecord.ENGINE_RPM = r["ENGINERPM"].ToString();
                    if (plf.installedSensors.TEMPERATURE1 != null)
                        onerecord.TEMPERATURE1 = r["TEMPERATURE1"].ToString();
                    if (plf.installedSensors.TEMPERATURE2 != null)
                        onerecord.TEMPERATURE2 = r["TEMPERATURE2"].ToString();
                    if (plf.installedSensors.WEIGHT1 != null)
                        onerecord.WEIGHT1 = r["WEIGHT1"].ToString();
                    if (plf.installedSensors.WEIGHT2 != null)
                        onerecord.WEIGHT2 = r["WEIGHT2"].ToString();
                    if (plf.installedSensors.WEIGHT3 != null)
                        onerecord.WEIGHT3 = r["WEIGHT3"].ToString();
                    if (plf.installedSensors.WEIGHT4 != null)
                        onerecord.WEIGHT4 = r["WEIGHT4"].ToString();
                    if (plf.installedSensors.WEIGHT5 != null)
                        onerecord.WEIGHT5 = r["WEIGHT5"].ToString();
                    if (plf.installedSensors.ADDITIONAL_SENSORS != null)
                        onerecord.ADDITIONAL_SENSORS = r["SENSOR1"].ToString();
                    if (plf.installedSensors.RESERVED_3 != null)
                        onerecord.RESERVED_3 = r["SENSOR3"].ToString();
                    if (plf.installedSensors.RESERVED_4 != null)
                        onerecord.RESERVED_4 = r["SENSOR4"].ToString();
                    if (plf.installedSensors.RESERVED_5 != null)
                        onerecord.RESERVED_5 = r["SENSOR5"].ToString();
                    //IGNITION
                    //ENGWORKTIME
                    //REFUEL
                    //FUELDELTA
                    //REFUELTIME
                    //REFUELTIME
                    if (plf.installedSensors.SPEED != null)
                        onerecord.SPEED = r["SPEED"].ToString();
                    if (plf.installedSensors.DISTANCE_COUNTER != null)
                        onerecord.DISTANCE_COUNTER = r["RACE"].ToString();
                    //MOVETIME
                    //IDLETIME
                    if (plf.installedSensors.LATITUDE != null)
                        onerecord.LATITUDE = r["LATITUDE"].ToString();
                    if (plf.installedSensors.LONGITUDE != null)
                        onerecord.LONGITUDE = r["LONGITUDE"].ToString();
                    if (plf.installedSensors.ALTITUDE != null)
                        onerecord.ALTITUDE = r["ALTITUDE"].ToString();
                    if (plf.installedSensors.FUEL_COUNTER != null)
                        onerecord.FUEL_COUNTER = r["FUEL"].ToString();

                    plf.Records.Add(onerecord);
                }
            }
            fb_con.Close();
            return plf;
        }
    }

    public class CARS
    {
        public struct car
        {
            public int ID{get;set;}
            public string REGNUMBER { get; set; }
            public string CARMODEL { get; set; }
            public string CHECKUPDATE { get; set; }//Дата следующего ТО
            public byte[] PHOTO { get; set; }
            public string NOTE { get; set; }
            public string CREATEWHO { get; set; }//Кем занесена
            public string CREATEWHEN { get; set; }
            public string CHANGEWHO { get; set; }//Кем последний раз изменена 
            public string CHANGEWHEN { get; set; }//Кем последний раз изменена 
            public int DEPARTMENTID { get; set; }//Указатель на подразделение
            public int TANK1 { get; set; }//Объем бака 1
            public int TANK2 { get; set; }
            public int MAXCONSUMPTPH { get; set; } //Максимальный расход топлива
            public int CSTABILITYTIME { get; set; }//Время устойчивости
            public int NOMINALRPM { get; set; }//Номинальные обороты
        }

        public List<car> carsArray{get;set;}
        FbConnection fb_con;

        public CARS(FbConnection fb_con_temp)
        {
            carsArray = new List<car>();
            fb_con = fb_con_temp;
        }

        public void LoadAllCars()
        {
            fb_con.Open();
            string sql = "SELECT * FROM CARS ORDER BY ID";
            FbCommand cmd = new FbCommand(sql, fb_con);
            car onecar;
            int tempInt;
            using (FbDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    onecar = new car();
                    onecar.ID = r.GetInt32(0);
                    onecar.REGNUMBER = r.GetString(1);
                    onecar.CARMODEL = r.GetString(2);
                    onecar.CHECKUPDATE = r.GetString(3);
                    //BLOB READING
                    onecar.PHOTO = new byte[(r.GetBytes(4, 0, null, 0, int.MaxValue))];
                    if(onecar.PHOTO.Length>0)
                        r.GetBytes(0, 0, onecar.PHOTO, 0, onecar.PHOTO.Length);
                    /////////////////////
                    onecar.NOTE = r.GetString(5);
                    onecar.CREATEWHO = r.GetString(6);
                    onecar.CREATEWHEN = r.GetString(7);
                    onecar.CHANGEWHO = r.GetString(8);
                    onecar.CHANGEWHEN= r.GetString(9);
                    onecar.DEPARTMENTID = r.GetInt32(10);
                    if (int.TryParse(r.GetString(11), out tempInt))
                        onecar.TANK1 = tempInt;
                    if (int.TryParse(r.GetString(12), out tempInt))
                        onecar.TANK2 = tempInt;
                    if (int.TryParse(r.GetString(13), out tempInt))
                        onecar.MAXCONSUMPTPH = tempInt;
                    if (int.TryParse(r.GetString(14), out tempInt))
                        onecar.CSTABILITYTIME = tempInt;
                    if (int.TryParse(r.GetString(15), out tempInt))
                        onecar.NOMINALRPM = tempInt;
                    carsArray.Add(onecar);
                }
            }
            fb_con.Close();
        }
    }

    public class DEPARTMENTS
    {
        public struct department
        {
            public int ID { get; set; }
            public string DEPARTMENT { get; set; }//Название подразделения
            public string NOTES { get; set; }
            public string CREATEWHO { get; set; }
            public string CREATEWHEN { get; set; }
            public string CHANGEWHO { get; set; }
            public string CHANGEWHEN { get; set; }
        }

        public List<department> departments{get;set;}
        FbConnection fb_con;

        public DEPARTMENTS(FbConnection fb_con_temp)
        {
            departments = new List<department>();
            fb_con = fb_con_temp;
        }

        public void LoadAllDepartments()
        {
            fb_con.Open();
            string sql = "SELECT * FROM DEPARTMENTS ORDER BY ID";
            FbCommand cmd = new FbCommand(sql, fb_con);
            department onedep;
            using (FbDataReader r = cmd.ExecuteReader())
            {
                int i;
                while (r.Read())
                {
                    i=0;
                    onedep = new department();
                    onedep.ID = r.GetInt32(i++);
                    onedep.DEPARTMENT = r.GetString(i++);
                    onedep.NOTES = r.GetString(i++);
                    onedep.CREATEWHO = r.GetString(i++);
                    onedep.CREATEWHEN = r.GetString(i++);
                    onedep.CHANGEWHO = r.GetString(i++);
                    onedep.CHANGEWHEN = r.GetString(i++);
                    departments.Add(onedep);
                }
            }
            fb_con.Close();
        }
    }

    public class WORKERS
    {
        public struct worker
        {
            public int ID { get; set; }
            public int WORKERINTID { get; set; }//Личный номер
            public int DEPARTMENTID { get; set; }//Указатель на подразделение
            public string FIRSTNAME { get; set; }//Имя
            public string SURNAME { get; set; }
            public string MIDDLENAME { get; set; }//Отчество
            public DateTime BIRTHDAY { get; set; }//Дата рождения
            public string PASSPORT { get; set; }//Паспорт
            public string LICENCE { get; set; }//Права
            public DateTime DATEDUELICENCE { get; set; }//Срок действия прав
            public string CATHEGORIES { get; set; }//Категория
            public string MEDICALCERTIFICATE { get; set; }//Мед. Справка
            public DateTime DATEDUEMEDICAL { get; set; }//Срок действия Мед. Справки 
            public byte[] PHOTO { get; set; }
            public string NOTES { get; set; }
            public string CREATEWHO { get; set; }//Кем занесена
            public string CREATEWHEN { get; set; }
            public string CHANGEWHO { get; set; }//Кем последний раз изменена 
            public string CHANGEWHEN { get; set; }//Кем последний раз изменена 
        }

        public List<worker> workers { get; set; }
        FbConnection fb_con;

        public WORKERS(FbConnection fb_con_temp)
        {
            workers = new List<worker>();
            fb_con = fb_con_temp;
        }

        public void LoadAllWorkers()
        {
            fb_con.Open();
            string sql = "SELECT * FROM WORKERS ORDER BY ID";
            FbCommand cmd = new FbCommand(sql, fb_con);
            worker oneworker;
            DateTime dateTemp = new DateTime();
            using (FbDataReader r = cmd.ExecuteReader())
            {
                int i;
                while (r.Read())
                {
                    i = 0;
                    oneworker = new worker();
                    oneworker.ID = r.GetInt32(i++);
                    oneworker.WORKERINTID = r.GetInt32(i++);
                    oneworker.DEPARTMENTID = r.GetInt32(i++);
                    oneworker.FIRSTNAME = r.GetString(i++);
                    oneworker.SURNAME = r.GetString(i++);
                    oneworker.MIDDLENAME = r.GetString(i++);
                    if (DateTime.TryParse(r.GetString(i++), out dateTemp))
                        oneworker.BIRTHDAY = dateTemp;
                    oneworker.PASSPORT = r.GetString(i++);
                    oneworker.LICENCE = r.GetString(i++);
                    if (DateTime.TryParse(r.GetString(i++), out dateTemp))
                        oneworker.DATEDUELICENCE = dateTemp;
                    oneworker.CATHEGORIES = r.GetString(i++);
                    oneworker.MEDICALCERTIFICATE = r.GetString(i++);
                    if (DateTime.TryParse(r.GetString(i++), out dateTemp))
                        oneworker.DATEDUEMEDICAL = dateTemp;
                    //BLOB READING
                    oneworker.PHOTO = new byte[(r.GetBytes(i++, 0, null, 0, int.MaxValue))];
                    if (oneworker.PHOTO.Length > 0)
                        r.GetBytes(0, 0, oneworker.PHOTO, 0, oneworker.PHOTO.Length);
                    /////////////////////
                    oneworker.NOTES = r.GetString(i++);
                    oneworker.CREATEWHO = r.GetString(i++);
                    oneworker.CREATEWHEN = r.GetString(i++);
                    oneworker.CHANGEWHO = r.GetString(i++);
                    oneworker.CHANGEWHEN = r.GetString(i++);
                    workers.Add(oneworker);
                }
            }
            fb_con.Close();
        }
    }

    public class DEVICES
    {
        public struct device
        {
            public int ID { get; set; }
            public string DEVICE { get; set; }//Номер электронного блока
            public string VERSION { get; set; }//Версия Электронного блока
            public DateTime LASTREAD { get; set; }//Дата последнего считывания данных
            public string NOTES { get; set; }
            public string CREATEWHO { get; set; }//Кем занесена
            public string CREATEWHEN { get; set; }
            public string CHANGEWHO { get; set; }//Кем последний раз изменена 
            public string CHANGEWHEN { get; set; }//Кем последний раз изменена 
            public string LASTREADWHO { get; set; }//Кто произвел последнее считывание данных
        }

        public List<device> devices { get; set; }
        FbConnection fb_con;

        public DEVICES(FbConnection fb_con_temp)
        {
            devices = new List<device>();
            fb_con = fb_con_temp;
        }

        public void LoadAllDevices()
        {
            fb_con.Open();
            string sql = "SELECT * FROM DEVICES ORDER BY ID";
            FbCommand cmd = new FbCommand(sql, fb_con);
            device onedevice;
            DateTime datetemp = new DateTime();
            using (FbDataReader r = cmd.ExecuteReader())
            {
                int i;
                while (r.Read())
                {
                    i = 0;
                    onedevice = new device();
                    onedevice.ID = r.GetInt32(i++);
                    onedevice.DEVICE = r.GetString(i++);
                    onedevice.VERSION = r.GetString(i++);
                    if(DateTime.TryParse(r.GetString(i++), out datetemp))
                        onedevice.LASTREAD = datetemp;
                    onedevice.CREATEWHO = r.GetString(i++);
                    onedevice.CREATEWHEN = r.GetString(i++);
                    onedevice.CHANGEWHO = r.GetString(i++);
                    onedevice.CHANGEWHEN = r.GetString(i++);
                    onedevice.NOTES = r.GetString(i++);
                    onedevice.LASTREADWHO = r.GetString(i++);
                    devices.Add(onedevice);
                }
            }
            fb_con.Close();
        }
    }

    public class READCYCLE 
    {
        public struct readcycle
        {
            public int ID { get; set; }
            public int DEVICEID { get; set; }
            public int CARID { get; set; }
            public int WORKER1ID { get; set; }
            public int WORKER2ID { get; set; }
            public string CREATEWHO { get; set; }//Кем занесена
            public string CREATEWHEN { get; set; }
            public string CHANGEWHO { get; set; }//Кем последний раз изменена 
            public string CHANGEWHEN { get; set; }//Кем последний раз изменена 
            public DateTime TIMEFROM { get; set; }
            public DateTime TIMETO { get; set; }
            public int CMAXCONSUMPTPH { get; set; }
            public int CSTABILITYTIME { get; set; }
            public PLFUnit.PLFRecord installedSensors { get; set; }
            public int TIMESTEP { get; set; }
            /// <summary>
            /// SET Manually ONLY!
            /// </summary>
            public int orgid { get; set; }
        }

        public List<readcycle> cycles { get; set; }
        FbConnection fb_con;

        public READCYCLE(FbConnection fb_con_temp)
        {
            cycles = new List<readcycle>();
            fb_con = fb_con_temp;             
        }

        public void LoadAllReadCycles()
        {
            fb_con.Open();
            string sql = "SELECT * FROM READCYCLE ORDER BY ID";
            FbCommand cmd = new FbCommand(sql, fb_con);
            readcycle onecycle;
            int intTemp;
            DateTime datetemp = new DateTime();
            using (FbDataReader r = cmd.ExecuteReader())
            {
                int i;
                while (r.Read())
                {
                    if (cycles.Count == 121)
                        i = 0;
                    i = 0;
                    onecycle = new readcycle();
                    onecycle.ID = r.GetInt32(i++);
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        onecycle.DEVICEID = intTemp;
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        onecycle.CARID = intTemp;
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        onecycle.WORKER1ID = intTemp;
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        onecycle.WORKER2ID = intTemp;
                    onecycle.CREATEWHO = r.GetString(i++);
                    onecycle.CREATEWHEN = r.GetString(i++);
                    onecycle.CHANGEWHO = r.GetString(i++);
                    onecycle.CHANGEWHEN = r.GetString(i++);
                    if (DateTime.TryParse(r.GetString(i++), out datetemp))
                        onecycle.TIMEFROM = datetemp;
                    if (DateTime.TryParse(r.GetString(i++), out datetemp))
                        onecycle.TIMETO = datetemp;
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        onecycle.CMAXCONSUMPTPH = intTemp;
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        onecycle.CSTABILITYTIME = intTemp;
                    //sensors
                    onecycle.installedSensors = new PLFUnit.PLFRecord();
                    onecycle.installedSensors.SetNForAllParams();
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                        {
                            onecycle.installedSensors.SYSTEM_TIME = new PLFUnit.PLFSystemTime();
                            onecycle.installedSensors.SYSTEM_TIME.systemTime = "Y";
                        }
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.FUEL_VOLUME1 = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.FUEL_VOLUME2 = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.FUEL_COUNTER = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.FUEL_CONSUMPTION = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.DISTANCE_COUNTER = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.VOLTAGE = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.ENGINE_RPM = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.LATITUDE = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.LONGITUDE = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.ALTITUDE = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.TEMPERATURE1 = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.TEMPERATURE2 = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.WEIGHT1 = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.WEIGHT2 = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.WEIGHT3 = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.WEIGHT4 = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.WEIGHT5 = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.ADDITIONAL_SENSORS = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.RESERVED_3 = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.RESERVED_4 = "Y";
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.RESERVED_5 = "Y";
                    i++;//ADD_SENS5
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        onecycle.TIMESTEP = intTemp;
                    if (int.TryParse(r.GetString(i++), out intTemp))
                        if (intTemp == 1)
                            onecycle.installedSensors.SPEED = "Y";
                    //
                    cycles.Add(onecycle);
                }
            }
            fb_con.Close();
        }
    }
}

