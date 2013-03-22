using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using DB.SQL;
using PLFUnit;

namespace BLL
{
    /// <summary>
    /// Класс отвечает за выборку информации по ДДД файлам.
    /// </summary>
    public class PLFUnitInfo
    {
        /// <summary>
        /// Текущий язык
        /// </summary>
        private string CurrentLanguage;//STRING_RU,STRING_RUG etc.
        /// <summary>
        /// Строка подключения
        /// </summary>
        private string connectionString;
        /// <summary>
        /// Обьект подключения к базе данных
        /// </summary>
        private SQLDB sqlDB { get; set; }

        public int GetedPlfRecords { get; set; }
        public int maxPlfRecords { get; set; }

        public PLFUnitInfo(string connectionsStringTMP, string Current_Language, SQLDB sqlTemp)
        {
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            sqlDB = sqlTemp;
        }

        public string Get_ID_DEVICE(int dataBlockId)
        {
            string ID_Device;
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ID_DEVICE";
            ID_Device = sqldbRecords.Get_ParamValue(dataBlockId, paramName);
            return ID_Device;
        }

        public string Get_VEHICLE(int dataBlockId)
        {
            string vehicle;
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "VEHICLE";
            vehicle = sqldbRecords.Get_ParamValue(dataBlockId, paramName);
            return vehicle;
        }

        public string Get_TIME_STEP(int dataBlockId)
        {
            string TIME_STEP;
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "TIME_STEP";
            TIME_STEP = sqldbRecords.Get_ParamValue(dataBlockId, paramName);
            return TIME_STEP;
        }

        public DateTime Get_START_PERIOD(int dataBlockId)
        {
            string gettedVaL;
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "START_PERIOD";
            gettedVaL = sqldbRecords.Get_ParamValue(dataBlockId, paramName);
            return new PLFSystemTime(gettedVaL).GetSystemTime();
        }

        public DateTime Get_END_PERIOD(int dataBlockId)
        {
            string gettedVaL;
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "END_PERIOD";
            gettedVaL = sqldbRecords.Get_ParamValue(dataBlockId, paramName);
            return new PLFSystemTime(gettedVaL).GetSystemTime();
        }

        private List<PLFRecord> Get_Records(int dataBlockId, PLFRecord sensorsInstalled, Hashtable allParamIds)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            return Get_Records(dataBlockId, new DateTime(), DateTime.Now, sensorsInstalled, allParamIds, sqldbRecords);
        }

        private List<PLFRecord> Get_Records(int dataBlockId, DateTime startPeriod, DateTime endPeriod, PLFRecord sensorsInstalled, Hashtable allParamIds, SQLDB_Records sqldbRecords)
        {
            List<PLFRecord> records = new List<PLFRecord>();
            PLFRecord plfRecord = new PLFRecord();
            string paramName = "Records";
            string currentParamName;

            #region "Variables"
            List<string> ADDITIONAL_SENSORS = new List<string>();
            List<string> ALTITUDE = new List<string>();
            List<string> DISTANCE_COUNTER = new List<string>();
            List<string> ENGINE_RPM = new List<string>();
            List<string> FUEL_CONSUMPTION = new List<string>();
            List<string> FUEL_COUNTER = new List<string>();
            List<string> FUEL_VOLUME1 = new List<string>();
            List<string> FUEL_VOLUME2 = new List<string>();
            List<string> LATITUDE = new List<string>();
            List<string> LONGITUDE = new List<string>();
            List<string> MAIN_STATES = new List<string>();
            List<string> RESERVED_3 = new List<string>();
            List<string> RESERVED_4 = new List<string>();
            List<string> RESERVED_5 = new List<string>();
            List<string> SPEED = new List<string>();
            List<string> SYSTEM_TIME = new List<string>();
            List<string> TEMPERATURE1 = new List<string>();
            List<string> TEMPERATURE2 = new List<string>();
            List<string> VOLTAGE = new List<string>();
            List<string> WEIGHT1 = new List<string>();
            List<string> WEIGHT2 = new List<string>();
            List<string> WEIGHT3 = new List<string>();
            List<string> WEIGHT4 = new List<string>();
            List<string> WEIGHT5 = new List<string>();
            #endregion

            #region "Load From DataBase"
            if (sensorsInstalled.ADDITIONAL_SENSORS == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".ADDITIONAL_SENSORS";
                ADDITIONAL_SENSORS = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["ADDITIONAL_SENSORS"]);
                GetedPlfRecords += ADDITIONAL_SENSORS.Count;
            }
            if (sensorsInstalled.ALTITUDE == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".ALTITUDE";
                ALTITUDE = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["ALTITUDE"]);
                GetedPlfRecords += ALTITUDE.Count;
            }
            if (sensorsInstalled.DISTANCE_COUNTER == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".DISTANCE_COUNTER";
                DISTANCE_COUNTER = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["DISTANCE_COUNTER"]);
                GetedPlfRecords += DISTANCE_COUNTER.Count;
            }
            if (sensorsInstalled.ENGINE_RPM == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".ENGINE_RPM";
                ENGINE_RPM = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["ENGINE_RPM"]);
                GetedPlfRecords += ENGINE_RPM.Count;
            }
            if (sensorsInstalled.FUEL_CONSUMPTION == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".FUEL_CONSUMPTION";
                FUEL_CONSUMPTION = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["FUEL_CONSUMPTION"]);
                GetedPlfRecords += FUEL_CONSUMPTION.Count;
            }
            if (sensorsInstalled.FUEL_COUNTER == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".FUEL_COUNTER";
                FUEL_COUNTER = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["FUEL_COUNTER"]);
                GetedPlfRecords += FUEL_COUNTER.Count;
            }
            if (sensorsInstalled.FUEL_VOLUME1 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".FUEL_VOLUME1";
                FUEL_VOLUME1 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["FUEL_VOLUME1"]);
                GetedPlfRecords += FUEL_VOLUME1.Count;
            }
            if (sensorsInstalled.FUEL_VOLUME2 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".FUEL_VOLUME2";
                FUEL_VOLUME2 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["FUEL_VOLUME2"]);
                GetedPlfRecords += FUEL_VOLUME2.Count;
            }
            if (sensorsInstalled.LATITUDE == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".LATITUDE";
                LATITUDE = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["LATITUDE"]);
                GetedPlfRecords += LATITUDE.Count;
            }
            if (sensorsInstalled.LONGITUDE == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".LONGITUDE";
                LONGITUDE = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["LONGITUDE"]);
                GetedPlfRecords += LONGITUDE.Count;
            }
            if (sensorsInstalled.MAIN_STATES == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".MAIN_STATES";
                MAIN_STATES = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["MAIN_STATES"]);
                GetedPlfRecords += MAIN_STATES.Count;
            }
            if (sensorsInstalled.RESERVED_3 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".RESERVED_3";
                RESERVED_3 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["RESERVED_3"]);
                GetedPlfRecords += RESERVED_3.Count;
            }
            if (sensorsInstalled.RESERVED_4 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".RESERVED_4";
                RESERVED_4 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["RESERVED_4"]);
                GetedPlfRecords += RESERVED_4.Count;
            }
            if (sensorsInstalled.RESERVED_5 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".RESERVED_5";
                RESERVED_5 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["RESERVED_5"]);
                GetedPlfRecords += RESERVED_5.Count;
            }
            if (sensorsInstalled.SPEED == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".SPEED";
                SPEED = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["SPEED"]);
                GetedPlfRecords += SPEED.Count;
            }
            if (sensorsInstalled.SYSTEM_TIME.systemTime == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".SYSTEM_TIME";
                SYSTEM_TIME = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["SYSTEM_TIME"]);
                GetedPlfRecords += SYSTEM_TIME.Count;
            }
            if (sensorsInstalled.TEMPERATURE1 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".TEMPERATURE1";
                TEMPERATURE1 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["TEMPERATURE1"]);
                GetedPlfRecords += TEMPERATURE1.Count;
            }
            if (sensorsInstalled.TEMPERATURE2 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".TEMPERATURE2";
                TEMPERATURE2 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["TEMPERATURE2"]);
                GetedPlfRecords += TEMPERATURE2.Count;
            }
            if (sensorsInstalled.VOLTAGE == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".VOLTAGE";
                VOLTAGE = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["VOLTAGE"]);
                GetedPlfRecords += VOLTAGE.Count;
            }
            if (sensorsInstalled.WEIGHT1 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".WEIGHT1";
                WEIGHT1 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["WEIGHT1"]);
                GetedPlfRecords += WEIGHT1.Count;
            }
            if (sensorsInstalled.WEIGHT2 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".WEIGHT2";
                WEIGHT2 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["WEIGHT2"]);
                GetedPlfRecords += WEIGHT2.Count;
            }
            if (sensorsInstalled.WEIGHT3 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".WEIGHT3";
                WEIGHT3 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["WEIGHT3"]);
                GetedPlfRecords += WEIGHT3.Count;
            }
            if (sensorsInstalled.WEIGHT4 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".WEIGHT4";
                WEIGHT4 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["WEIGHT4"]);
                GetedPlfRecords += WEIGHT4.Count;
            }
            if (sensorsInstalled.WEIGHT5 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".WEIGHT5";
                WEIGHT5 = sqldbRecords.Get_AllParamsArray(dataBlockId, (int)allParamIds["WEIGHT5"]);
                GetedPlfRecords += WEIGHT5.Count;
            }
            #endregion

            if (SYSTEM_TIME.Count == 0)
            {
                return records;
                throw new Exception("Файл содержит ошибки либо неправильно разобран");
            }
            
            List<int> Indexes = new List<int>();
            Indexes = CheckDate(SYSTEM_TIME, SYSTEM_TIME, startPeriod, endPeriod);
            foreach(int i in Indexes)
            {
                plfRecord = new PLFRecord();
                #region "List Initialization"

                if (ADDITIONAL_SENSORS.Count != 0)
                    plfRecord.ADDITIONAL_SENSORS = ADDITIONAL_SENSORS[i];

                if (ALTITUDE.Count != 0)
                    plfRecord.ALTITUDE = ALTITUDE[i];

                if (DISTANCE_COUNTER.Count != 0)
                    plfRecord.DISTANCE_COUNTER = DISTANCE_COUNTER[i];

                if (ENGINE_RPM.Count != 0)
                    plfRecord.ENGINE_RPM = ENGINE_RPM[i];

                if (FUEL_CONSUMPTION.Count != 0)
                    plfRecord.FUEL_CONSUMPTION = FUEL_CONSUMPTION[i];

                if (FUEL_COUNTER.Count != 0)
                    plfRecord.FUEL_COUNTER = FUEL_COUNTER[i];

                if (FUEL_VOLUME1.Count != 0)
                    plfRecord.FUEL_VOLUME1 = FUEL_VOLUME1[i];

                if (FUEL_VOLUME2.Count != 0)
                    plfRecord.FUEL_VOLUME2 = FUEL_VOLUME2[i];

                if (LATITUDE.Count != 0)
                    plfRecord.LATITUDE = LATITUDE[i];

                if (LONGITUDE.Count != 0)
                    plfRecord.LONGITUDE = LONGITUDE[i];

                if (MAIN_STATES.Count != 0)
                    plfRecord.MAIN_STATES = MAIN_STATES[i];

                if (RESERVED_3.Count != 0)
                    plfRecord.RESERVED_3 = RESERVED_3[i];

                if (RESERVED_4.Count != 0)
                    plfRecord.RESERVED_4 = RESERVED_4[i];

                if (RESERVED_5.Count != 0)
                    plfRecord.RESERVED_5 = RESERVED_5[i];

                if (SPEED.Count != 0)
                    plfRecord.SPEED = SPEED[i];

                if (SYSTEM_TIME.Count != 0)
                    plfRecord.SYSTEM_TIME = new PLFSystemTime(SYSTEM_TIME[i]);

                if (TEMPERATURE1.Count != 0)
                    plfRecord.TEMPERATURE1 = TEMPERATURE1[i];

                if (TEMPERATURE2.Count != 0)
                    plfRecord.TEMPERATURE2 = TEMPERATURE2[i];

                if (VOLTAGE.Count != 0)
                    plfRecord.VOLTAGE = VOLTAGE[i];

                if (WEIGHT1.Count != 0)
                    plfRecord.WEIGHT1 = WEIGHT1[i];

                if (WEIGHT2.Count != 0)
                    plfRecord.WEIGHT2 = WEIGHT2[i];

                if (WEIGHT3.Count != 0)
                    plfRecord.WEIGHT3 = WEIGHT3[i];

                if (WEIGHT4.Count != 0)
                    plfRecord.WEIGHT4 = WEIGHT4[i];

                if (WEIGHT5.Count != 0)
                    plfRecord.WEIGHT5 = WEIGHT5[i];
                #endregion
                records.Add(plfRecord);
            }
            return records;
        }

        public PLFRecord Get_InstalledSensors(int dataBlockId)
        {
            //SQLDB sqldb = new SQLDB(connectionString);
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            PLFRecord sensorsInstalled = new PLFRecord();
            string paramName = "installedSensors";
            string currentParamName;

            #region "Variables"
            bool ADDITIONAL_SENSORS;
            bool ALTITUDE;
            bool DISTANCE_COUNTER;
            bool ENGINE_RPM;
            bool FUEL_CONSUMPTION;
            bool FUEL_COUNTER;
            bool FUEL_VOLUME1;
            bool FUEL_VOLUME2;
            bool LATITUDE;
            bool LONGITUDE;
            bool MAIN_STATES;
            bool RESERVED_3;
            bool RESERVED_4;
            bool RESERVED_5;
            bool SPEED;
            bool SYSTEM_TIME;
            bool TEMPERATURE1;
            bool TEMPERATURE2;
            bool VOLTAGE;
            bool WEIGHT1;
            bool WEIGHT2;
            bool WEIGHT3;
            bool WEIGHT4;
            bool WEIGHT5;
            #endregion

            #region "Load From DataBase"
            currentParamName = paramName + ".ADDITIONAL_SENSORS";
            ADDITIONAL_SENSORS = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));

            currentParamName = paramName + ".ALTITUDE";
            ALTITUDE = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".DISTANCE_COUNTER";
            DISTANCE_COUNTER = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".ENGINE_RPM";
            ENGINE_RPM = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".FUEL_CONSUMPTION";
            FUEL_CONSUMPTION = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".FUEL_COUNTER";
            FUEL_COUNTER = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".FUEL_VOLUME1";
            FUEL_VOLUME1 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".FUEL_VOLUME2";
            FUEL_VOLUME2 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".FUEL_VOLUME2";
            FUEL_VOLUME2 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".LATITUDE";
            LATITUDE = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".LONGITUDE";
            LONGITUDE = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".MAIN_STATES";
            MAIN_STATES = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".RESERVED_3";
            RESERVED_3 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".RESERVED_4";
            RESERVED_4 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".RESERVED_5";
            RESERVED_5 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".SPEED";
            SPEED = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".SYSTEM_TIME";
            SYSTEM_TIME = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".TEMPERATURE1";
            TEMPERATURE1 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".TEMPERATURE2";
            TEMPERATURE2 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".VOLTAGE";
            VOLTAGE = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".WEIGHT1";
            WEIGHT1 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".WEIGHT2";
            WEIGHT2 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".WEIGHT3";
            WEIGHT3 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".WEIGHT4";
            WEIGHT4 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            currentParamName = paramName + ".WEIGHT5";
            WEIGHT5 = If_Y_retTrue(sqldbRecords.Get_ParamValue(dataBlockId, currentParamName));
            #endregion

            #region "List Initialization" 
            //Тут еще имеется пространство для оптимизации
            if (ADDITIONAL_SENSORS)
                sensorsInstalled.ADDITIONAL_SENSORS = "Y";
            if (ALTITUDE)
                sensorsInstalled.ALTITUDE = "Y";
            if (DISTANCE_COUNTER)
                sensorsInstalled.DISTANCE_COUNTER = "Y";
            if (ENGINE_RPM)
                sensorsInstalled.ENGINE_RPM = "Y";
            if (FUEL_CONSUMPTION)
                sensorsInstalled.FUEL_CONSUMPTION = "Y";
            if (FUEL_COUNTER)
                sensorsInstalled.FUEL_COUNTER = "Y";
            if (FUEL_VOLUME1)
                sensorsInstalled.FUEL_VOLUME1 = "Y";
            if (FUEL_VOLUME2)
                sensorsInstalled.FUEL_VOLUME2 = "Y";
            if (LATITUDE)
                sensorsInstalled.LATITUDE = "Y";
            if (LONGITUDE)
                sensorsInstalled.LONGITUDE = "Y";
            if (MAIN_STATES)
                sensorsInstalled.MAIN_STATES = "Y";
            if (RESERVED_3)
                sensorsInstalled.RESERVED_3 = "Y";
            if (RESERVED_4)
                sensorsInstalled.RESERVED_4 = "Y";
            if (RESERVED_5)
                sensorsInstalled.RESERVED_5 = "Y";
            if (SPEED)
                sensorsInstalled.SPEED = "Y";
            if (SYSTEM_TIME)
                sensorsInstalled.SYSTEM_TIME.systemTime = "Y";
            if (TEMPERATURE1)
                sensorsInstalled.TEMPERATURE1 = "Y";
            if (TEMPERATURE2)
                sensorsInstalled.TEMPERATURE2 = "Y";
            if (VOLTAGE)
                sensorsInstalled.VOLTAGE = "Y";
            if (WEIGHT1)
                sensorsInstalled.WEIGHT1 = "Y";
            if (WEIGHT2)
                sensorsInstalled.WEIGHT2 = "Y";
            if (WEIGHT3)
                sensorsInstalled.WEIGHT3 = "Y";
            if (WEIGHT4)
                sensorsInstalled.WEIGHT4 = "Y";
            if (WEIGHT5)
                sensorsInstalled.WEIGHT5 = "Y";
            #endregion

            return sensorsInstalled;
        }

        private Hashtable Get_AllParamsSensorsId(PLFRecord sensorsInstalled)
        {
            Hashtable allParamsIds = new Hashtable();
           
            string paramName = "Records";
            string currentParamName;

            #region "GetAllParams"
            if (sensorsInstalled.ADDITIONAL_SENSORS == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".ADDITIONAL_SENSORS";
                allParamsIds.Add("ADDITIONAL_SENSORS",sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.ALTITUDE == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".ALTITUDE";
                allParamsIds.Add("ALTITUDE", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.DISTANCE_COUNTER == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".DISTANCE_COUNTER";
                allParamsIds.Add("DISTANCE_COUNTER", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.ENGINE_RPM == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".ENGINE_RPM";
                allParamsIds.Add("ENGINE_RPM", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.FUEL_CONSUMPTION == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".FUEL_CONSUMPTION";
                allParamsIds.Add("FUEL_CONSUMPTION", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.FUEL_COUNTER == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".FUEL_COUNTER";
                allParamsIds.Add("FUEL_COUNTER", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.FUEL_VOLUME1 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".FUEL_VOLUME1";
                allParamsIds.Add("FUEL_VOLUME1", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.FUEL_VOLUME2 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".FUEL_VOLUME2";
                allParamsIds.Add("FUEL_VOLUME2", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.LATITUDE == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".LATITUDE";
                allParamsIds.Add("LATITUDE", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.LONGITUDE == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".LONGITUDE";
                allParamsIds.Add("LONGITUDE", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.MAIN_STATES == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".MAIN_STATES";
                allParamsIds.Add("MAIN_STATES", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.RESERVED_3 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".RESERVED_3";
                allParamsIds.Add("RESERVED_3", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.RESERVED_4 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".RESERVED_4";
                allParamsIds.Add("RESERVED_4", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.RESERVED_5 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".RESERVED_5";
                allParamsIds.Add("RESERVED_5", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.SPEED == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".SPEED";
                allParamsIds.Add("SPEED", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.SYSTEM_TIME.systemTime == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".SYSTEM_TIME";
                allParamsIds.Add("SYSTEM_TIME", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.TEMPERATURE1 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".TEMPERATURE1";
                allParamsIds.Add("TEMPERATURE1", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.TEMPERATURE2 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".TEMPERATURE2";
                allParamsIds.Add("TEMPERATURE2", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.VOLTAGE == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".VOLTAGE";
                allParamsIds.Add("VOLTAGE", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.WEIGHT1 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".WEIGHT1";
                allParamsIds.Add("WEIGHT1", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.WEIGHT2 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".WEIGHT2";
                allParamsIds.Add("WEIGHT2", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.WEIGHT3 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".WEIGHT3";
                allParamsIds.Add("WEIGHT3", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.WEIGHT4 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".WEIGHT4";
                allParamsIds.Add("WEIGHT4", sqlDB.getParamId(currentParamName));
            }
            if (sensorsInstalled.WEIGHT5 == sensorsInstalled.YesString)
            {
                currentParamName = paramName + ".WEIGHT5";
                allParamsIds.Add("WEIGHT5", sqlDB.getParamId(currentParamName));
            }
            #endregion
            return allParamsIds;
        }

        private bool If_Y_retTrue(string word)
        {
            if (word == "Y")
                return true;
            else
                if(word == "N" || word.Trim()=="") //word.Trim()=="" - добавил для импорта из файрберд, остальное и так работает. если будут проблемы - смотреть тут.
                    return false;
                else
                    throw new Exception("Неправильно разобран файл, нельзя прочитать установленные сенсоры");
        }

        private List<int> CheckDate(List<string> dateArray, DateTime periodDate, bool reverseDirection)//true <=, false >=
        {
            List<int> returnArray = new List<int>();
            DateTime dateTime = new DateTime();
            PLFSystemTime timeReal;
            int index = 0;

            foreach (string record in dateArray)
            {
                timeReal = new PLFSystemTime(record);
                dateTime = timeReal.GetSystemTime();

                if (reverseDirection == false)
                {
                    if (dateTime.Date >= periodDate.Date)
                        returnArray.Add(index);
                }
                else
                {
                    if (dateTime.Date <= periodDate.Date)
                        returnArray.Add(index);
                }
                index++;
            }
            return returnArray;
        }

        private List<int> CheckDate(List<string> beginDateArray, List<string> endDateArray, DateTime startPeriodDate, DateTime endPeriodDate)
        {
            List<int> fromIndexes = new List<int>();
            List<int> toIndexes = new List<int>();
            List<int> Indexes = new List<int>();
            fromIndexes = CheckDate(beginDateArray, startPeriodDate, false);
            toIndexes = CheckDate(endDateArray, endPeriodDate, true);

           /* foreach (int fromIndex in fromIndexes)
            {
                foreach (int toIndex in toIndexes)
                    if (fromIndex == toIndex)
                        Indexes.Add(fromIndex);
            }*/

            if (fromIndexes.Count != 0)
            {
                if (toIndexes.Count != 0)
                {
                    Indexes = (((IEnumerable<int>)fromIndexes).Intersect<int>(((IEnumerable<int>)toIndexes))).ToList();
                }
            }
            return Indexes;
        }

        public List<PLFRecord> Get_Records(List<int> dataBlockIDS, int driversCardId)
        {
            return Get_Records(dataBlockIDS, new DateTime(), DateTime.Now, driversCardId);
        }

        public List<PLFRecord> Get_Records(List<int> dataBlockIDS, DateTime startPeriod, DateTime endPeriod, int driversCardId)
        {
            List<PLFRecord> records = new List<PLFRecord>();
            List<int> dataBlockIdsToGet = new List<int>();
            DateTime fromTemp = new DateTime();
            DateTime toTemp = new DateTime();
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            maxPlfRecords = 0;
            GetedPlfRecords = 0;

            foreach (int dataBlock in dataBlockIDS)
            {
                fromTemp = Get_START_PERIOD(dataBlock);
                toTemp = Get_END_PERIOD(dataBlock);
                if (fromTemp.Date >= startPeriod && fromTemp.Date <= endPeriod)
                {
                    dataBlockIdsToGet.Add(dataBlock);
                    maxPlfRecords += sqldbRecords.Get_DataBlock_RecordsCount(dataBlock);
                    continue;
                }
                if (toTemp.Date >= startPeriod && toTemp.Date <= endPeriod)
                {
                    dataBlockIdsToGet.Add(dataBlock);
                    maxPlfRecords += sqldbRecords.Get_DataBlock_RecordsCount(dataBlock);
                    continue;
                }
                if(startPeriod >= fromTemp.Date && endPeriod <= toTemp.Date)
                {
                    dataBlockIdsToGet.Add(dataBlock);
                    maxPlfRecords += sqldbRecords.Get_DataBlock_RecordsCount(dataBlock);
                    continue;
                }
            }

            List<PLFRecord> plfUnit = new List<PLFRecord>();
            PLFUnitClass plfUnitClassTemp = new PLFUnitClass();
            PLFRecord sensorsInstalled = new PLFRecord();
            Hashtable allSensorsParamIds = new Hashtable();

            /*if (dataBlockIdsToGet.Count > 0)
            {
                sensorsInstalled = Get_InstalledSensors(dataBlockIdsToGet[0]);
                allSensorsParamIds = Get_AllParamsSensorsId(sensorsInstalled);
            }*/

            foreach (int id in dataBlockIdsToGet)
            {
                sensorsInstalled = Get_InstalledSensors(id);
                allSensorsParamIds = Get_AllParamsSensorsId(sensorsInstalled);
                plfUnit.AddRange(Get_Records(id, startPeriod, endPeriod, sensorsInstalled, allSensorsParamIds, sqldbRecords));
            }

            plfUnit.Sort(PlfRecordsByTimeComparison);
            return plfUnit;
        }

        private int PlfRecordsByTimeComparison(PLFRecord x, PLFRecord y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    int retval = x.SYSTEM_TIME.GetSystemTime().CompareTo(y.SYSTEM_TIME.GetSystemTime());
                    return retval;
                }
            }
        }
        /////Statistics//////////////////////
        /// <summary>
        /// Получает проценты информации в карточке относительно дней в году
        /// </summary>
        /// <param name="date">Дата(год)</param>
        /// <param name="datablockId">ID файла, для которого нужно подсчитать</param>
        /// <returns>double - проценты</returns>
        public double Statistics_GetYearStatistics(DateTime date, int datablockId)//Проверить функции
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            
            double stat = 0;
            int minutesInDay = 1440;
            int dayInYear = GetDaysInAYear(date.Year);

            PLFUnitClass plfUnitClassTemp = new PLFUnitClass();
            PLFRecord sensorsInstalled = new PLFRecord();
            Hashtable allSensorsParamIds = new Hashtable();

            sensorsInstalled.SYSTEM_TIME.systemTime = "Y";
            allSensorsParamIds = Get_AllParamsSensorsId(sensorsInstalled);
            //plfUnitClassTemp.Records = Get_Records(datablockId, sensorsInstalled, allSensorsParamIds);

            int dayInMonth = DateTime.DaysInMonth(date.Year, 12);
            plfUnitClassTemp.Records = Get_Records(datablockId, new DateTime(date.Year, 1, 1), new DateTime(date.Year, 12, dayInMonth), sensorsInstalled, allSensorsParamIds, sqldbRecords);
            
            plfUnitClassTemp.TIME_STEP = Get_TIME_STEP(datablockId);
            if (plfUnitClassTemp.TIME_STEP == " ")
                return 0;
            double temp = plfUnitClassTemp.Get_AllWorkingTime().TotalMinutes;
            stat = (plfUnitClassTemp.Get_AllWorkingTime().TotalMinutes / (minutesInDay * dayInYear)) * 100;
            return stat;
        }
        /// <summary>
        /// Получает проценты информации в карточке относительно дней в месяце
        /// </summary>
        /// <param name="date">дата(год, месяц)</param>
        /// <param name="datablockId">ID файла, для которого нужно подсчитать</param>
        /// <returns>double - проценты</returns>
        public double Statistics_GetMonthStatistics(DateTime date, int datablockId)//Проверить функции activities.GetTotalTime() - писалась давно, может не точно подсчитывать!
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            double stat = 0;
            int minutesInDay = 1440;
            int dayInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            PLFUnitClass plfUnitClassTemp = new PLFUnitClass();
            PLFRecord sensorsInstalled = new PLFRecord();
            Hashtable allSensorsParamIds = new Hashtable();

            sensorsInstalled.SYSTEM_TIME.systemTime = "Y";
            allSensorsParamIds = Get_AllParamsSensorsId(sensorsInstalled);
            plfUnitClassTemp.Records = Get_Records(datablockId, new DateTime(date.Year, date.Month, 1), new DateTime(date.Year, date.Month, dayInMonth), sensorsInstalled, allSensorsParamIds, sqldbRecords);
            plfUnitClassTemp.TIME_STEP = Get_TIME_STEP(datablockId);
            if (plfUnitClassTemp.TIME_STEP == " ")
                return 0;
            stat = (plfUnitClassTemp.Get_AllWorkingTime().TotalMinutes / (minutesInDay * dayInMonth)) * 100;
            return stat;
        }
        /// <summary>
        /// Получает проценты информации в карточке относительно минут в дне
        /// </summary>
        /// <param name="date">дата(год, месяц, день)</param>
        /// <param name="datablockId">ID файла, для которого нужно подсчитать</param>
        /// <returns>double - проценты</returns>
        public double Statistics_GetDayStatistics(DateTime date, int datablockId)//Проверить функции activities.GetTotalTime() - писалась давно, может не точно подсчитывать!
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            double stat = 0;
            int minutesInDay = 1440;

            PLFUnitClass plfUnitClassTemp = new PLFUnitClass();
            PLFRecord sensorsInstalled = new PLFRecord();
            Hashtable allSensorsParamIds = new Hashtable();

            sensorsInstalled.SYSTEM_TIME.systemTime = "Y";
            allSensorsParamIds = Get_AllParamsSensorsId(sensorsInstalled);
            plfUnitClassTemp.Records = Get_Records(datablockId, date.Date, date, sensorsInstalled, allSensorsParamIds, sqldbRecords);
            plfUnitClassTemp.TIME_STEP = Get_TIME_STEP(datablockId);
            if (plfUnitClassTemp.TIME_STEP == " ")
                return 0;
            stat = (plfUnitClassTemp.Get_AllWorkingTime().TotalMinutes / minutesInDay) * 100;
            return stat;
        }
        /// <summary>
        /// получает сколько дней в году
        /// </summary>
        /// <param name="year">номер года(2007, 2008 и тд..)</param>
        /// <returns>количество дней в году</returns>
        private int GetDaysInAYear(int year)
        {

            int days = 0;
            for (int i = 1; i <= 12; i++)
            {
                days += DateTime.DaysInMonth(year, i);
            }
            return days;
        }

    }
}
