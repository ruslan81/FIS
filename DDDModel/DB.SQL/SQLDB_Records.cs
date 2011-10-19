using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.Types;
using MySql.Data.MySqlClient;

using DB.Interface;

namespace DB.SQL
{
    /// <summary>
    /// Дополнительные методы работы с БД
    /// в этом классе описаны методы для выборки параметров для отчетов из БД
    /// </summary>
    public class SQLDB_Records:DBI_Records
    {
        private string connectionString;
        private MySqlConnection sqlConnection;
        private MySqlTransaction globTransaction;

        public MySqlConnection GETMYSQLCONNECTION()
        {
            return sqlConnection;
        }

        public SQLDB_Records(string connectionStringTemp)
        {
            connectionString = connectionStringTemp;
            sqlConnection = new MySqlConnection();
            sqlConnection.ConnectionString = connectionString;
        }
        public SQLDB_Records(string connectionStringTemp, MySqlConnection msqlConTemp)
        {
            connectionString = connectionStringTemp;
            sqlConnection = msqlConTemp;
        }

        public void OpenConnection()
        {
            if (sqlConnection.State != System.Data.ConnectionState.Open)
                sqlConnection.Open();
        }
        public void OpenTransaction()
        {
            globTransaction = sqlConnection.BeginTransaction();
        }
        public void CommitConnection()
        {
            globTransaction.Commit();
        }
        public void CloseConnection()
        {
            if (sqlConnection.State != System.Data.ConnectionState.Closed)
                sqlConnection.Close();
        }
        public void RollbackConnection()
        {
            globTransaction.Rollback();
        }
        public bool IsConnectionOpened()
        {
            if (sqlConnection.Ping())
                return true;
            else
                return false;
        }        

        public List<string> Get_DriverNames_ByDataBlockIdList(List<int> dataBlockIds)
        {
            int holderFirstNamesParamId;
            int holderSurnameParamId;
            string holderFirstNames;
            string holderSurname;
            string fullName;
            List<string> driversNamesList = new List<string>();

            if (dataBlockIds.Count == 0)
                return driversNamesList;

            holderFirstNamesParamId = getParamId("ef_identification.driverCardHolderIdentification.cardHolderName.holderFirstNames.name");
            holderSurnameParamId = getParamId("ef_identification.driverCardHolderIdentification.cardHolderName.holderSurname.name");

            OpenConnection();
            foreach(int dataBlockId in dataBlockIds)
            {
                holderFirstNames = Get_ParamValue(dataBlockId, holderFirstNamesParamId);
                holderSurname = Get_ParamValue(dataBlockId, holderSurnameParamId);
                fullName = holderFirstNames + " " + holderSurname;
                driversNamesList.Add(fullName);
            }
            CloseConnection();

            return driversNamesList;
        }

        public List<string> Get_VehicleNumbers_ByDataBlockIdList(List<int> dataBlockIds)
        {
            int vehicleNumberParamId;
            string vehicleNumber;
            List<string> vehicleNumbersList = new List<string>();

            if (dataBlockIds.Count == 0)
                return vehicleNumbersList;

            vehicleNumberParamId = getParamId("vehicleOverview.vehicleIdentificationNumber");

            OpenConnection();
            foreach (int dataBlockId in dataBlockIds)
            {
                vehicleNumber = Get_ParamValue(dataBlockId, vehicleNumberParamId);
                vehicleNumbersList.Add(vehicleNumber);
            }
            CloseConnection();

            return vehicleNumbersList;
        }

        public string Get_ParamValue(int dataBlockId, int paramId)
        {
            string getedValue;

            string sql = "SELECT PARAM_VALUE FROM fn_data_records WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID AND PARAM_ID = @PARAM_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            cmd.Parameters.AddWithValue("@PARAM_ID", paramId);
            try
            {
                getedValue = Convert.ToString(cmd.ExecuteScalar());

                if (getedValue == "")
                    getedValue = " ";
                //!!!!!!!!!!!!Вставить проверку, если не найдено Значение, кинуть эксепшн!
            }
            catch (Exception ex)
            {
                return "Значение не установлено";
            }
            return getedValue;
        }

        public int Get_DataBlockCardType(int dataBlockId) //0-card, 1 - vehicle, 2-plf
        {
            string cardTypeParamName = "cardType";
            int cardType;
            if (!int.TryParse(Get_ParamValue(dataBlockId, cardTypeParamName), out cardType))
                cardType = -1;
            return cardType;
        }

        public string Get_DriversNumber(int dataBlockId)
        {
            string driversNumber;
            string paramName = "ef_identification.cardIdentification.cardNumber.driverIdentification";

            driversNumber = Get_ParamValue(dataBlockId, paramName);

            return driversNumber;
        }

        public List<int> Get_DataBlockIdByDriversName(string name, string surname)
        {
            List<int> dataBlockIdList = new List<int>();
            List<int> dataBlockIdName = new List<int>();
            List<int> dataBlockIdSurName = new List<int>();

            string sql = "SELECT DATA_BLOCK_ID FROM fn_data_records WHERE PARAM_VALUE=@NAME ";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@NAME", name);
            OpenConnection();
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                dataBlockIdName.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            sql = "SELECT DATA_BLOCK_ID FROM fn_data_records WHERE PARAM_VALUE=@SURNAME ";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@SURNAME", surname);
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                dataBlockIdSurName.Add(sdr.GetInt32(0));
            }
            CloseConnection();
            
            foreach(int nameId in dataBlockIdName)//Возможны неполадки!!! Проверить при совпадении нескольких!
            {
                foreach (int surNameId in dataBlockIdSurName)
                {
                    if (nameId == surNameId)
                        dataBlockIdList.Add(nameId);
                }
            }

            return dataBlockIdList;
        }

        public List<int> Get_DataBlockIdByVehicleNumber(string number)
        {
            string paramName = "vehicleOverview.vehicleIdentificationNumber";
            List<int> dataBlockIdNumber = new List<int>();
            int paramId = getParamId(paramName);

            string sql = "SELECT DATA_BLOCK_ID FROM fn_data_records WHERE PARAM_VALUE=@NUMBER AND PARAM_ID=@PARAM_ID ";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@NUMBER", number);
            cmd.Parameters.AddWithValue("@PARAM_ID", paramId);

            OpenConnection();
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                dataBlockIdNumber.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            CloseConnection();

            return dataBlockIdNumber;
        }

        public string Get_DriversCardIssuingMemberState(int dataBlockId)
        {            
            string cardIssuingMemberState;
            string paramName = "ef_identification.cardIdentification.cardIssuingMemberState";

            cardIssuingMemberState = Get_ParamValue(dataBlockId, paramName);

            return cardIssuingMemberState;
        }

        public string Get_DriversNameOrVehiclesNumberByBlockId(int dataBlockId)
        {
            int cardType;
            int paramIdFirst = -1;
            int paramIdSecond = -1;
            string returnValue;
            SQLDB sqlDb = new SQLDB(connectionString);

            cardType = Get_DataBlockCardType(dataBlockId);

            if (cardType == 0)//driver
            {

                paramIdFirst = getParamId("ef_identification.driverCardHolderIdentification.cardHolderName.holderFirstNames.name");
                paramIdSecond = getParamId("ef_identification.driverCardHolderIdentification.cardHolderName.holderSurname.name");
            }
            else
                if (cardType == 1)//vehicle
                {
                    paramIdFirst = getParamId("vehicleOverview.vehicleIdentificationNumber");
                }
                else
                    if (cardType == 2)//plf
                    {
                        paramIdFirst = getParamId("VEHICLE");
                        paramIdSecond = getParamId("ID_DEVICE");
                    }
                    else
                        throw new Exception("Не поддерживаемый тип данных!");

            OpenConnection();
            returnValue = Get_ParamValue(dataBlockId, paramIdFirst);
            if (cardType == 0 || cardType == 2)
                returnValue += " " + Get_ParamValue(dataBlockId, paramIdSecond);
            CloseConnection();

            return returnValue;
        }

        public int Get_DataBlock_RecordsCount(int dataBlockId)
        {
            int recorsCount;
            string sql = "SELECT PARSE_RECORDS FROM fn_data_block WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            recorsCount = Convert.ToInt32(cmd.ExecuteScalar());
            return recorsCount;
        }

        public string Get_DataBlock_EDate(int dataBlockId)
        {
            string sql = "SELECT PARSE_EDATE FROM fn_data_block WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            string date = cmd.ExecuteScalar().ToString();

            return date;
        }

        public List<int> Get_DataBlockIdByRecordsCount(int recordsCount)
        {
            List<int> dataBlockIdNumber = new List<int>();

            string sql = "SELECT DATA_BLOCK_ID FROM fn_data_block WHERE PARSE_RECORDS = @PARSE_RECORDS";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@PARSE_RECORDS", recordsCount);

            OpenConnection();
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                dataBlockIdNumber.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            CloseConnection();

            return dataBlockIdNumber;
        }

        public List<int> Get_DataBlockIdByFileNameAndBytesCount(string fileName, int bytesCount)
        {
            List<int> dataBlockIdList = new List<int>();
            List<int> dataBlockIdFileName = new List<int>();
            List<int> dataBlockIdBytesCount = new List<int>();

            string sql = "SELECT DATA_BLOCK_ID FROM fn_data_records WHERE PARAM_VALUE=@NAME ";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@NAME", fileName);
            OpenConnection();
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                dataBlockIdFileName.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            sql = "SELECT DATA_BLOCK_ID FROM fn_data_records WHERE PARAM_VALUE=@BYTESCOUNT ";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@BYTESCOUNT", bytesCount);
            sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                dataBlockIdBytesCount.Add(sdr.GetInt32(0));
            }
            CloseConnection();

            foreach (int nameId in dataBlockIdFileName)//Возможны неполадки!!! Проверить при совпадении нескольких!
            {
                foreach (int countId in dataBlockIdBytesCount)
                {
                    if (nameId == countId)
                        dataBlockIdList.Add(nameId);
                }
            }

            return dataBlockIdList;
        }

        public List<string> Get_AllParamsArray(int dataBlockId, string paramName)
        {
            int paramId = getParamId(paramName);

            return Get_AllParamsArray(dataBlockId, paramId);
        }

        public List<string> Get_AllParamsArray(int dataBlockId, int paramId)
        {
            List<string> allParams = new List<string>();

            string sql = "SELECT PARAM_VALUE FROM fn_data_records WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID AND PARAM_ID = @PARAM_ID ORDER BY DATA_RECORD_ID ASC";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            cmd.Parameters.AddWithValue("@PARAM_ID", paramId);

            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                allParams.Add(sdr.GetString(0));
            }
            sdr.Close();

            return allParams;
        }
   
        //vehicle_overwiew_________________________________________________________________________
        public byte Get_VOverview_CardSlotsStatus(int dataBlockId)
        {
            byte SlotsStatus;
            string paramName = "vehicleOverview.cardSlotsStatus";

            SlotsStatus = Convert.ToByte(Get_ParamValue(dataBlockId, paramName));

            return SlotsStatus;
        }

        public long Get_VOverview_CurrentDateTime(int dataBlockId)
        {
            long CurrentDateTime;
            string paramName = "vehicleOverview.currentDateTime";

            CurrentDateTime = Convert.ToUInt32(Get_ParamValue(dataBlockId, paramName));

            return CurrentDateTime;
        }

        public string Get_VOverview_IdentificationNumber(int dataBlockId)
        {
            string IdentificationNumber;
            string paramName = "vehicleOverview.vehicleIdentificationNumber";
            IdentificationNumber = Get_ParamValue(dataBlockId, paramName);

            return IdentificationNumber;
        }

        public string Get_VOverview_RegistrationNumber(int dataBlockId)
        {
            string RegistrationNumber;
            string paramName = "vehicleOverview.vehicleRegistrationIdentification.vehicleRegistrationNumber.vehicleRegNumber";
            RegistrationNumber = Get_ParamValue(dataBlockId, paramName);

            return RegistrationNumber;
        }

        public short Get_VOverview_RegistrationNation(int dataBlockId)
        {
            short vehicleRegistrationNation;
            string paramName = "vehicleOverview.vehicleRegistrationIdentification.vehicleRegistrationNation";
            vehicleRegistrationNation = Convert.ToInt16(Get_ParamValue(dataBlockId, paramName));

            return vehicleRegistrationNation;
        }
        //_________________________________________________________________________________________
        private int getParamId(string paramName)
        {
            int paramId;
            SQLDB sqlDb = new SQLDB(connectionString, GETMYSQLCONNECTION());
            paramId = sqlDb.getParamId(paramName);
            return paramId;
        }

        public string Get_ParamValue(int dataBlockId, string paramName)
        {
            int paramId;
            string returnString;

            paramId = getParamId(paramName);
            if (paramId == -1)
                throw new Exception("Не найден параметр " + paramName);
            else
            {
                //OpenConnection();
                returnString = Get_ParamValue(dataBlockId, paramId);
                ////CloseConnection();
                return returnString;
            }
        }


        ////////////////////////////
        public void AddCounter()
        {
            OpenConnection();
            MySqlCommand cmd = new MySqlCommand();
            string sql = "INSERT INTO TEST_COUNTER "
                + "(NUMBER)"
                + "VALUES (@NUMBER)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@NUMBER", 1);
            cmd.ExecuteNonQuery();
            CloseConnection();
        }
    }
}
