using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;
using BLL;
using TotalsClasses;
using MySql.Data;
using MySql.Data.Types;
using MySql.Data.MySqlClient;
using FirebirdToMySQLConverter;

namespace DDDModel
{
    class Program
    {
       
        static void Main(string[] args)
        {
            string connectionString1 = "server=localhost;port=3306;default command timeout=3000;Connection Timeout=6000;User Id=root;password = ;Persist Security Info=True;database=smartfis";
            string connectionString = "server=mysql62.1gb.ru;default command timeout=600;Connection Timeout=600;database=gb_x_smartfis;User Id=gb_x_smartfis;password =5216a0af;";
            string currentLanguage = "STRING_EN";
            DataBlock dataBlock = new DataBlock(connectionString, currentLanguage);
            
            dataBlock.OpenConnection();

            bool ex = false;
            ConsoleKeyInfo ch;

            //MY CODE

            //dataBlock.organizationTable.DeleteOrganization(58);
            //dataBlock.organizationTable.DeleteOrganization(56);

            /*for (int i = 2001; i < 2020; i++)
            {
                System.Console.WriteLine(dataBlock.plfUnitInfo.Statistics_GetYearStatistics(new DateTime(i, 1, 1), 110));
            }
            System.Console.ReadKey();*/


           //SCRIPT TO ADD COMMON GROUP
            /*List<Int32> orgIds = dataBlock.organizationTable.Get_AllOrganizationsId();
            int k = 0;
            foreach (int id in orgIds) 
            {
                List<Int32> groupIds = dataBlock.cardsTable.GetAllGroupIds(id,0);
                if (groupIds.Count == 0) {
                    //k++;
                    dataBlock.cardsTable.CreateDefaultGroup(id);
                }
            }

            System.Console.WriteLine(k);
            System.Console.ReadKey();*/

           return;
   
           while (ex != true)
            {
                Console.Clear();
                Console.WriteLine("Connection string = " + connectionString + "\r\n");
                Console.WriteLine("Data Block Id = " + dataBlock.GET_DATA_BLOCK_ID());
                Console.WriteLine("Data Block STATE : " + dataBlock.GetDataState() + "\r\n");
                Console.WriteLine("1.Change Connection String");
                Console.WriteLine("2.Add History Test");
                Console.WriteLine("3.Get HistoryTest");
                Console.WriteLine("4.Get Firebird test");
                Console.WriteLine("5.Get shedules test");      
          
                Console.WriteLine("v.init Country_Regions tables");
               
                ch = Console.ReadKey(true);
                
                switch (ch.KeyChar)
                {
                    case '1':
                        {
                            string newConnectionString;
                            Console.Clear();
                            Console.WriteLine(connectionString + "\r\n (write exit to cancel)\r\n");
                            Console.WriteLine("New string: \r\n");
                            newConnectionString = Console.ReadLine();
                            if (newConnectionString != "exit")
                                connectionString = newConnectionString;
                        }
                        break;

                    case '2':
                        {
                            HistoryTable history = new HistoryTable(connectionString, currentLanguage, new SQLDB(connectionString));
                            try
                            {
                                history.OpenConnection();
                                history.OpenTransaction();
                                history.AddOrGetAction("Изменение учетных данных пользователя/водителя");
                                history.AddOrGetAction("Загружен блок данных PLF");
                                history.AddOrGetAction("Загружен блок данных карты");
                                history.AddOrGetAction("Загружен блок данных бортового устройства");
                                history.AddOrGetAction("Регистрация нового пользователя в системе");
                                history.AddOrGetAction("Регистрация нового водителя");
                                history.AddOrGetAction("Регистрация нового транспортного средства");
                                history.CommitTransaction();
                                history.CloseConnection();
                            }
                            catch (Exception except)
                            {
                                history.RollbackConnection();
                                history.CloseConnection();
                                Console.WriteLine(except.Message);
                            }

                            Console.ReadKey(false);
                        }
                        break;

                    case '3':
                        {
                            try
                            {
                                SQLDB sql = new SQLDB(connectionString);
                                sql.OpenConnection();
                                sql.CloseConnection();
                            }
                            catch (Exception except)
                            {
                                Console.WriteLine(except.Message);
                            }

                            Console.ReadKey(false);
                        }
                        break;
                    case '4':
                        {
                            try
                            {
                                FirebirdSQLClass fireClass = new FirebirdSQLClass(connectionString);
                                fireClass.LoadAllInfo();
                            }
                            catch (Exception except)
                            {
                                Console.WriteLine(except.Message);
                            }

                            Console.ReadKey(false);
                        }
                        break;
                    case '5':
                        {
                            try
                            {
                                dataBlock.OpenConnection();
                                dataBlock.emailScheduleTable.GetAllEmailShedules_ForSending();
                            }
                            catch (Exception except)
                            {
                                Console.WriteLine(except.Message);
                            }
                            finally
                            {
                                dataBlock.CloseConnection();
                            }

                            Console.ReadKey(false);
                        }
                        break;
                 
                    case 'v':
                        {
                            Console.Clear();
                            Console.WriteLine("type password to init Country_Regions tables");
                            string password = Console.ReadLine();
                            try
                            {
                                dataBlock.organizationTable.FillCountryAndRegionsTable(password,"nothing");                                
                                DataBaseInit(password);
                                Console.WriteLine("Выполнено успешно!");
                            } 
                            catch (Exception except)
                            {
                                Console.WriteLine(except.Message);
                            }
                           
                            Console.ReadKey(false);
                        }
                        break;
                    case '6': ex = true;
                        break;
                }
            }          
        }
        /// <summary>
        /// Иннициализирует базу данных стартовыми значениями.
        /// Это нужно для того, чтобы сразу с разворачивания базы данных можно было начать работать в веб интерфейсе.
        /// </summary>
        /// <param name="password">пароль нужен просто, чтобы из консоли нечайно не тыкнуть. пароль = qqq</param>
        public static void DataBaseInit(string password)
        {
            string CurrentLanguage = "STRING_EN";
            if (password != "qqq")
                throw new Exception("Неправильный пароль");

             string connectionString = "server=localhost;User Id=root;password = 1;Persist Security Info=True;database=fleetnetbase";
             string connectionString1 = "server=mysql62.1gb.ru;default command timeout=600;Connection Timeout=600;database=gb_x_smartfis;User Id=gb_x_smartfis;password =5216a0af;";

            SQLDB sqlDb = new SQLDB(connectionString);
            MySqlConnection sqlConnection = new MySqlConnection(connectionString);
            MySqlCommand cmd = new MySqlCommand();
            int stringId = -1;
            int generatedId = -1;
            string sql;

            sql = "INSERT INTO fd_string "
                    + "(STRING_ID, STRING_EN)"
                    + "VALUES (@STRING_ID, @STRING_EN)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STRING_ID", 0);
            cmd.Parameters.AddWithValue("@STRING_EN", "");

            sqlConnection.Open();
            sqlDb.OpenConnection();
            

            cmd.ExecuteNonQuery();

            //dataBlockStates
            InitTable_ID_String("fd_data_block_state", "DATA_BLOCK_STATE_ID", "STRID_DATA_BLOCK_STATE_NAME", "Ideal", sqlDb, sqlConnection);
            InitTable_ID_String("fd_data_block_state", "DATA_BLOCK_STATE_ID", "STRID_DATA_BLOCK_STATE_NAME", "Parsed", sqlDb, sqlConnection);
            InitTable_ID_String("fd_data_block_state", "DATA_BLOCK_STATE_ID", "STRID_DATA_BLOCK_STATE_NAME", "Not parsed", sqlDb, sqlConnection);
            InitTable_ID_String("fd_data_block_state", "DATA_BLOCK_STATE_ID", "STRID_DATA_BLOCK_STATE_NAME", "Not supported", sqlDb, sqlConnection);

            //dataRecordsState
            InitTable_ID_String("fd_data_record_state", "DATA_RECORD_STATE_ID", "STRID_DATA_RECORD_STATE_NAME", "Ideal", sqlDb, sqlConnection);
            InitTable_ID_String("fd_data_record_state", "DATA_RECORD_STATE_ID", "STRID_DATA_RECORD_STATE_NAME", "Parsed", sqlDb, sqlConnection);
            InitTable_ID_String("fd_data_record_state", "DATA_RECORD_STATE_ID", "STRID_DATA_RECORD_STATE_NAME", "Not parsed", sqlDb, sqlConnection);

            //Card_TYPE
            stringId = sqlDb.AddOrGetString("Card Type: Driver");
            generatedId = sqlDb.generateId("fd_card_type", "CARD_TYPE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate CARD_TYPE_ID"));

            sql = "INSERT INTO fd_card_type "
               + "(CARD_TYPE_ID, STRID_CARD_TYPE_NAME, STRID_CARD_TYPE_SHORT_NAME, STRID_CARD_TYPE_PRINT_NAME)"
               + "VALUES (@CARD_TYPE_ID, @STRID_CARD_TYPE_NAME, @STRID_CARD_TYPE_SHORT_NAME, @STRID_CARD_TYPE_PRINT_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_TYPE_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_CARD_TYPE_NAME", stringId);
            cmd.Parameters.AddWithValue("@STRID_CARD_TYPE_SHORT_NAME", stringId);
            cmd.Parameters.AddWithValue("@STRID_CARD_TYPE_PRINT_NAME", stringId);
            cmd.ExecuteNonQuery();

            stringId = sqlDb.AddOrGetString("Card Type: Vehicle");
            generatedId = sqlDb.generateId("fd_card_type", "CARD_TYPE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate CARD_TYPE_ID"));

            sql = "INSERT INTO fd_card_type "
               + "(CARD_TYPE_ID, STRID_CARD_TYPE_NAME, STRID_CARD_TYPE_SHORT_NAME, STRID_CARD_TYPE_PRINT_NAME)"
               + "VALUES (@CARD_TYPE_ID, @STRID_CARD_TYPE_NAME, @STRID_CARD_TYPE_SHORT_NAME, @STRID_CARD_TYPE_PRINT_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_TYPE_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_CARD_TYPE_NAME", stringId);
            cmd.Parameters.AddWithValue("@STRID_CARD_TYPE_SHORT_NAME", stringId);
            cmd.Parameters.AddWithValue("@STRID_CARD_TYPE_PRINT_NAME", stringId);
            cmd.ExecuteNonQuery();

            stringId = sqlDb.AddOrGetString("Card Type: Organization Init Card");
            generatedId = sqlDb.generateId("fd_card_type", "CARD_TYPE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate CARD_TYPE_ID"));

            sql = "INSERT INTO fd_card_type "
               + "(CARD_TYPE_ID, STRID_CARD_TYPE_NAME, STRID_CARD_TYPE_SHORT_NAME, STRID_CARD_TYPE_PRINT_NAME)"
               + "VALUES (@CARD_TYPE_ID, @STRID_CARD_TYPE_NAME, @STRID_CARD_TYPE_SHORT_NAME, @STRID_CARD_TYPE_PRINT_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_TYPE_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_CARD_TYPE_NAME", stringId);
            cmd.Parameters.AddWithValue("@STRID_CARD_TYPE_SHORT_NAME", stringId);
            cmd.Parameters.AddWithValue("@STRID_CARD_TYPE_PRINT_NAME", stringId);
            cmd.ExecuteNonQuery();
            int orgInitCardTypeId = generatedId;

            //fd_deviceFirmware
            int deviceFirmWareId = sqlDb.AddNewDeviceFirmware("onboard device by default", DateTime.Now, "0.0.0.0", new byte[1]);

            //fd_device
            int deviceTypeId = sqlDb.AddNewDeviceType("DefaultDeviceType");
            sqlDb.AddNewDevice(deviceTypeId, "The onboard device by default", "00000", DateTime.Now, deviceFirmWareId, 222222);
            //InitTable_ID_String("fd_device", "DEVICE_ID", "STRID_DEVICE_NAME", "");

            //fd_object
            InitTable_ID_String("fd_object", "OBJECT_ID", "STRID_OBJECT_NAME", "Object by default", sqlDb, sqlConnection);

            //FD_ORG_TYPE
            InitTable_ID_String("fd_org_type", "ORG_TYPE_ID", "STRID_ORG_TYPE_NAME", "Cargo transportation", sqlDb, sqlConnection);
            InitTable_ID_String("fd_org_type", "ORG_TYPE_ID", "STRID_ORG_TYPE_NAME", "Passengers transportation", sqlDb, sqlConnection);
            InitTable_ID_String("fd_org_type", "ORG_TYPE_ID", "STRID_ORG_TYPE_NAME", "Anything else transportation", sqlDb, sqlConnection);
            InitTable_ID_String("fd_org_type", "ORG_TYPE_ID", "STRID_ORG_TYPE_NAME", "Dealer", sqlDb, sqlConnection);
            InitTable_ID_String("fd_org_type", "ORG_TYPE_ID", "STRID_ORG_TYPE_NAME", "Subdealer", sqlDb, sqlConnection);
            InitTable_ID_String("fd_org_type", "ORG_TYPE_ID", "STRID_ORG_TYPE_NAME", "Predealer", sqlDb, sqlConnection);
            
            //FD_ORG
            OrganizationTable orgTable = new OrganizationTable(connectionString, CurrentLanguage, sqlDb);
            orgTable.AddNewOrganization("Init Organization", 1, 1, 1, 6);

            //fd_param

            sql = "INSERT INTO fd_param "
               + "(PARAM_ID, PARENT_PARAM_ID, PARAM_NAME, PARAM_SIZE)"
               + "VALUES (@PARAM_ID, @PARENT_PARAM_ID, @PARAM_NAME, @PARAM_SIZE)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@PARAM_ID", 0);
            cmd.Parameters.AddWithValue("@PARENT_PARAM_ID", 0);
            cmd.Parameters.AddWithValue("@PARAM_NAME", "Parent_param");
            cmd.Parameters.AddWithValue("@PARAM_SIZE", 0);
            cmd.ExecuteNonQuery();

            //fd_user_info_set and fd_user_info
            sqlDb.AddUserInfoName("Surname", "STRING_EN");
            sqlDb.AddUserInfoName("Name", "STRING_EN");
            sqlDb.AddUserInfoName("Patronimic", "STRING_EN");
            sqlDb.AddUserInfoName("Drivers certificate", "STRING_EN");
            sqlDb.AddUserInfoName("Card number", "STRING_EN");
            sqlDb.AddUserInfoName("Phone number", "STRING_EN");
            sqlDb.AddUserInfoName("Birthday", "STRING_EN");


            //fd_user_rights
            InitTable_ID_String("fd_user_rights", "USER_RIGHTS_ID", "STRID_USER_RIGHTS_NAME", "Administrator", sqlDb, sqlConnection);
            InitTable_ID_String("fd_user_rights", "USER_RIGHTS_ID", "STRID_USER_RIGHTS_NAME", "SuperAdministrator", sqlDb, sqlConnection);

            //fd_user_role
            InitTable_ID_String("fd_user_role", "USER_ROLE_ID", "STRID_USER_ROLE_NAME", "Administrator", sqlDb, sqlConnection);
            InitTable_ID_String("fd_user_role", "USER_ROLE_ID", "STRID_USER_ROLE_NAME", "SuperAdministrator", sqlDb, sqlConnection);

            //fd_user_type
            InitTable_ID_String("fd_user_type", "USER_TYPE_ID", "STRID_USER_TYPE_NAME", "Driver", sqlDb, sqlConnection);
            InitTable_ID_String("fd_user_type", "USER_TYPE_ID", "STRID_USER_TYPE_NAME", "Manager", sqlDb, sqlConnection);
            InitTable_ID_String("fd_user_type", "USER_TYPE_ID", "STRID_USER_TYPE_NAME", "Administrator", sqlDb, sqlConnection);
            InitTable_ID_String("fd_user_type", "USER_TYPE_ID", "STRID_USER_TYPE_NAME", "DealerUser", sqlDb, sqlConnection);

            //fd_user
            UsersTables usersTable = new UsersTables(connectionString, "STRING_EN", sqlDb);
            usersTable.OpenConnection();
            usersTable.OpenTransaction();
            UserFromTable userFromTable = new UserFromTable("admin", "123", "3", "1", DateTime.Now, "org");
            usersTable.AddNewUser(userFromTable, 3, 1, 1, 0);
            userFromTable = new UserFromTable("admin2", "123", "3", "1", DateTime.Now, "org");
            usersTable.AddNewUser(userFromTable, 3, 2, 1, 0);
            usersTable.CommitTransaction();
            usersTable.CloseConnection();

            //fd_measure
            int kgId = sqlDb.AddNewMeasure("Kg", "Kilograms");
            int kmId = sqlDb.AddNewMeasure("Km", "Kilometers");
            int m3Id = sqlDb.AddNewMeasure("M3", "Cubic metres");
            int dateId = sqlDb.AddNewMeasure("Date", "DateTime");
            int RPMId = sqlDb.AddNewMeasure("Rpm", "Revolutions per minute");
            int KmphId = sqlDb.AddNewMeasure("Km/h", "Kilometers per hour");
            int PercentsId = sqlDb.AddNewMeasure("%", "Percents");
            int FConsumption = sqlDb.AddNewMeasure("L/h", "Liters per hour");

            //fd_key
            sqlDb.AddNewCriteria(kgId, "Commentary to a vehicle", "Строка для Комментария к ТС", 0, 0);
            sqlDb.AddNewCriteria(kgId, "Load-carrying capacity", "Грузоподьемность", 5000, 25000);
            sqlDb.AddNewCriteria(m3Id, "Fuel tank 1", "Коментарий к Топливному баку 1", 1, 250);
            sqlDb.AddNewCriteria(m3Id, "Fuel tank 2", "Коментарий к Топливному баку 1", 1, 250);
            sqlDb.AddNewCriteria(dateId, "MRO 1", "Дата ТО 1", 1, 1);
            sqlDb.AddNewCriteria(dateId, "MRO 2", "Дата ТО 2", 1, 1);
            sqlDb.AddNewCriteria(RPMId, "Nominal turns", "критерий для номинальных оборотов", 1, 1);
            sqlDb.AddNewCriteria(KmphId, "Maximum speed", "критерий для максимальной скорости", 1, 1);
            sqlDb.AddNewCriteria(KmphId, "Manoeuvring", "критерий для маневрирования", 1, 1);
            sqlDb.AddNewCriteria(PercentsId, "City", "критерий для города", 1, 1);
            sqlDb.AddNewCriteria(PercentsId, "Highway", "	критерий для магистрали", 1, 1);
            sqlDb.AddNewCriteria(FConsumption, "Nominal fuel consumption", "критерий для номинального расхода топлива", 1, 1);
            sqlDb.AddNewCriteria(RPMId, "Cold start", "критерий для холодного старта", 1, 1);
            sqlDb.AddNewCriteria(RPMId, "Hot stop", "критерий для горячего стопа", 1, 1);

            //fd_fuel_type
            stringId = sqlDb.AddOrGetString("Unknown fuel type");

            sql = "INSERT INTO fd_fuel_type "
               + "(FUEL_TYPE_ID, STRID_FUEL_TYPE_NAME)"
               + "VALUES (@FUEL_TYPE_ID, @STRID_FUEL_TYPE_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@FUEL_TYPE_ID", 0);
            cmd.Parameters.AddWithValue("@STRID_FUEL_TYPE_NAME", stringId);
            cmd.ExecuteNonQuery();

            stringId = sqlDb.AddOrGetString("Undefined");

            sql = "INSERT INTO fd_vehicle_type "
               + "(VEHICLE_TYPE_ID, STRID_VEHICLE_TYPE_NAME, FUEL_TYPE_ID)"
               + "VALUES (@VEHICLE_TYPE_ID, @STRID_VEHICLE_TYPE_NAME, @FUEL_TYPE_ID)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_TYPE_ID", 0);
            cmd.Parameters.AddWithValue("@STRID_VEHICLE_TYPE_NAME", stringId);
            cmd.Parameters.AddWithValue("@FUEL_TYPE_ID", 0);
            cmd.ExecuteNonQuery();

            int fuelTypeID = sqlDb.AddNewFuelType("FuelType 1");
            sqlDb.AddNewVehicleType("Veh type 1", fuelTypeID);


            //HISTORY
          /*  HistoryTable history = new HistoryTable(connectionString, CurrentLanguage);
            history.AddOrGetAction("Изменение учетных данных");*/
            //

            sqlConnection.Close();
            sqlDb.CloseConnection();
        }

        private static int InitTable_ID_String(string tableName, string primaryKeyName, string stringName, string stringValue, SQLDB sqlDb, MySqlConnection sqlConnection)
        {
            MySqlCommand cmd = new MySqlCommand();
            int stringId = sqlDb.AddOrGetString(stringValue);
            int generatedId = sqlDb.generateId(tableName, primaryKeyName);
            if (generatedId == -1)
                throw (new Exception("Can't generate " + primaryKeyName));

            string sql = "INSERT INTO " + tableName + " "
               + "(" + primaryKeyName + "," + stringName + ")"
               + "VALUES (@" + primaryKeyName + ",@" + stringName + ")";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@" + primaryKeyName, generatedId);
            cmd.Parameters.AddWithValue("@" + stringName, stringId);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
    }
}
