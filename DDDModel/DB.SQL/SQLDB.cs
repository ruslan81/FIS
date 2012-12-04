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
    /// Клаас образованный от интерфейса DBI, описывает почти все взаимодействие с БД
    /// </summary>
    public class SQLDB : DBI
    {
        private string connectionString;
        private MySqlConnection sqlConnection;
        private MySqlTransaction globTransaction;

        public MySqlConnection GETMYSQLCONNECTION()
        {
            return sqlConnection;
        }

        public SQLDB(string connectionStringTemp, MySqlConnection sqlConnectionTemp)
        {
            connectionString = connectionStringTemp;
            sqlConnection = sqlConnectionTemp;
        }
        public SQLDB(string connectionStringTemp)
        {
            connectionString = connectionStringTemp;
            sqlConnection = new MySqlConnection();
            sqlConnection.ConnectionString = connectionString;
        }

        public void OpenConnection()
        {
            if (sqlConnection.State != System.Data.ConnectionState.Open)
                sqlConnection.Open();
        }
        public void OpenTransaction()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
                globTransaction = sqlConnection.BeginTransaction();
        }
        public void CommitConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
                globTransaction.Commit();
        }
        public void CloseConnection()
        {
            if (sqlConnection.State != System.Data.ConnectionState.Closed)
                sqlConnection.Close();
        }
        public void RollbackConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
                globTransaction.Rollback();
        }
        public bool IsConnectionOpened()
        {
            if (sqlConnection.Ping())
                return true;
            else
                return false;
        }
        //-----------------------fn_data_block and fd_data_block_state ----
        //принимает битовый массив, раскладывает его в другую таблицу fn_data_records
        public int AddDataBlock(byte[] dataBlockTemp)
        {
            int generatedId;
            // MySqlTransaction trans = null;
            int blockStateId;
            //sqlConnection.Open();
            //trans = sqlConnection.BeginTransaction();

            generatedId = generateId("fn_data_block", "DATA_BLOCK_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate DATA_BLOCK_ID"));

            blockStateId = 3;

            MySqlCommand cmd = new MySqlCommand();
            string sql = "INSERT INTO fn_data_block "
                + "(DATA_BLOCK_ID, DEVICE_ID, DATA_BLOCK_STATE_ID, DATA, PARSE_BDATE, STRID_PARSE_MESSAGE)"
                + "VALUES (@DATA_BLOCK_ID, '1', @DATA_BLOCK_STATE_ID, @dataBlockTemp,@dateTemp, '1')";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@dataBlockTemp", dataBlockTemp);
            cmd.Parameters.AddWithValue("@dateTemp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@DATA_BLOCK_STATE_ID", blockStateId);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", generatedId);

            cmd.ExecuteNonQuery();
            //trans.Commit();
            Console.WriteLine("Successefully added!");
            return generatedId;
        }
        private int AddDataBlockState()
        {
            MySqlCommand cmd = new MySqlCommand();
            int generatedId;

            try
            {
                generatedId = generateId("fd_data_block_state", "DATA_BLOCK_STATE_ID");
                if (generatedId == -1)
                    throw (new Exception("Can't generate DATA_BLOCK_STATE_ID"));

                string sql = "INSERT INTO fd_data_block_state "
                    + "(DATA_BLOCK_STATE_ID, STRID_DATA_BLOCK_STATE_NAME)"
                    + "VALUES (@DATA_BLOCK_STATE_ID, '1')";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@DATA_BLOCK_STATE_ID", generatedId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
            Console.WriteLine("Successefully added!");
            return generatedId;
        }
        public void DeleteDataBlock(int dataBlockId)
        {
            string sql = "DELETE FROM fn_data_block WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            cmd.ExecuteNonQuery();
        }
        private bool DeleteDataBlockState(int dataBlockStateId)
        {
            try
            {
                string sql = "DELETE FROM fd_data_block_state WHERE DATA_BLOCK_STATE_ID = @DATA_BLOCK_STATE_ID";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@DATA_BLOCK_STATE_ID", dataBlockStateId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            Console.WriteLine("Successefully deleted!");
            return true;
        }
        public byte[] GetDataBlock(int dataBlockId)
        {
            byte[] returnValue = null;
            try
            {
                string sql = "SELECT DATA FROM fn_data_block WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);

                MySqlDataReader sdr = cmd.ExecuteReader();
                sdr.Read();

                returnValue = new Byte[(sdr.GetBytes(0, 0, null, 0, int.MaxValue))];
                sdr.GetBytes(0, 0, returnValue, 0, returnValue.Length);
                sdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return returnValue;
            }
            Console.WriteLine(("Successefully Geted Data from Data_Block with DATA_BLOCK_ID = " + dataBlockId.ToString()));
            return returnValue;
        }
        public int GetSTRIdDataBlockStateName(int dataBlockStateId)
        {
            int dataBlockState = -1;

            string sql = "SELECT STRID_DATA_BLOCK_STATE_NAME FROM fd_data_block_state WHERE DATA_BLOCK_STATE_ID = @DATA_BLOCK_STATE_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_STATE_ID", dataBlockStateId);
            dataBlockState = Convert.ToInt32(cmd.ExecuteScalar());
            return dataBlockState;

        }
        public int GetDataBlockState(int dataBlockId)
        {
            int dataBlockState = -1;

            string sql = "SELECT DATA_BLOCK_STATE_ID FROM fn_data_block WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            dataBlockState = Convert.ToInt32(cmd.ExecuteScalar());
            return dataBlockState;
        }
        public void SetDataBlockState(int dataBlockId, int dataBlockStateId)
        {
            MySqlCommand cmd;
            string sql = "UPDATE fn_data_block SET DATA_BLOCK_STATE_ID =@DATA_BLOCK_STATE_ID WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_STATE_ID", dataBlockStateId);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            cmd.ExecuteNonQuery();
        }
        public int SetDataBlockParseRecords(int dataBlockId)
        {
            int recordsCount;
            MySqlCommand cmd;
            string sql = "SELECT COUNT(*) FROM fn_data_records WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            recordsCount = Convert.ToInt32(cmd.ExecuteScalar());

            sql = "UPDATE fn_data_block SET PARSE_RECORDS =@PARSE_RECORDS WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            cmd.Parameters.AddWithValue("@PARSE_RECORDS", recordsCount);
            cmd.ExecuteNonQuery();

            return recordsCount;
        }
        public List<int> GetAllDataBlocksId(int UserId)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT DATA_BLOCK_ID FROM fn_data_block ORDER BY DATA_BLOCK_ID";// WHERE DATA_BLOCK_USER_ID = @UserId или типа того! Доделать
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);

            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public List<int> GetAllDataBlocksId_byCRC32(int UserId, uint crc)
        {
            List<int> gettedId = new List<int>();
            int dataBlockCrc32ParamId = getParamId("DataBlock_CRC32");
            if (dataBlockCrc32ParamId != -1)
            {
                string sql = "SELECT DATA_BLOCK_ID FROM fn_data_records WHERE PARAM_ID=@PARAM_ID AND PARAM_VALUE=@PARAM_VALUE ORDER BY DATA_BLOCK_ID";// WHERE DATA_BLOCK_USER_ID = @UserId или типа того! Доделать
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@PARAM_ID", dataBlockCrc32ParamId);
                cmd.Parameters.AddWithValue("@PARAM_VALUE", crc.ToString());

                MySqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    gettedId.Add(sdr.GetInt32(0));
                }
                sdr.Close();
            }
            return gettedId;
        }
        public List<int> GetAllDataBlocksId_byCardType(int UserId, int cardType)
        {
            List<int> gettedId = new List<int>();
            int cardTypeParamId = getParamId("cardType");
            if (cardTypeParamId != -1)
            {
                string sql = "SELECT DATA_BLOCK_ID FROM fn_data_records WHERE PARAM_ID=@PARAM_ID AND PARAM_VALUE=@PARAM_VALUE ORDER BY DATA_BLOCK_ID";// WHERE DATA_BLOCK_USER_ID = @UserId или типа того! Доделать
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@PARAM_ID", cardTypeParamId);
                cmd.Parameters.AddWithValue("@PARAM_VALUE", cardType.ToString());

                MySqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    gettedId.Add(sdr.GetInt32(0));
                }
                sdr.Close();
            }
            return gettedId;
        }
        /*  public List<int> GetAllUnparsedDataBlockIDs(int UserId)
          {
              List<int> gettedId = new List<int>();

              string sql = "SELECT DATA_BLOCK_ID FROM fn_data_block WHERE DATA_BLOCK_STATE_ID = 3 ORDER BY DATA_BLOCK_ID";// WHERE DATA_BLOCK_USER_ID = @UserId или типа того! Доделать
              MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);

              MySqlDataReader sdr = cmd.ExecuteReader();
              while (sdr.Read())
              {
                  gettedId.Add(sdr.GetInt32(0));
              }
              sdr.Close();
              return gettedId;
          }*/
        public List<int> GetAllParsedDataBlockIDs(int UserId)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT DATA_BLOCK_ID FROM fn_data_block WHERE DATA_BLOCK_STATE_ID = 2 ORDER BY DATA_BLOCK_ID";// WHERE DATA_BLOCK_USER_ID = @UserId или типа того! Доделать
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);

            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public void SetDataBlock_CardId(int dataBlockId, int cardId)
        {
            string sql = "UPDATE fn_data_block SET CARD_ID=@CARD_ID WHERE DATA_BLOCK_ID=@DATA_BLOCK_ID";
            MySqlCommand cmd = new MySqlCommand();
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            cmd.ExecuteNonQuery();
        }
        public List<int> GetDataBlockIdsByCardId(int cardID)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT DATA_BLOCK_ID FROM fn_data_block WHERE CARD_ID = @CARD_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardID);

            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        //-----------------------------------------------------------------
        //----------------------fn_data_record and fd_data_record_state ---
        public int AddDataRecord(string paramName, string value, int dataBlockId)
        {
            int generatedId;
            int recordStateId = -1;
            int paramId = -1;

            if (dataBlockId == -1)
                throw new Exception("Файл не может быть добавлен, поэтому не может быть сохранена сопутствующая информация.");
            /*  generatedId = generateId("fn_data_records", "DATA_RECORD_ID");Убрал так как автоинкремент включен. ПОтом можно будет выключить...
              if (generatedId == -1)
                  throw (new Exception("Can't generate DATA_RECORD_ID"));*/

            recordStateId = 1;

            paramId = getParamId(paramName);
            if (paramId == -1)
                throw (new Exception("check sqldb addDataRecord paramID"));

            MySqlCommand cmd = new MySqlCommand();

            string sql = "INSERT INTO fn_data_records "
                + "(DATA_BLOCK_ID, DATA_RECORD_STATE_ID, PARAM_ID, DATE_RECORD, PARAM_VALUE)"
                + "VALUES (@DATA_BLOCK_ID, @DATA_RECORD_STATE_ID, @PARAM_ID, @DATE_RECORD, @PARAM_VALUE)";
            cmd = new MySqlCommand(sql, sqlConnection);
            // cmd.Parameters.AddWithValue("@DATA_RECORD_ID", generatedId);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            cmd.Parameters.AddWithValue("@DATA_RECORD_STATE_ID", recordStateId);
            cmd.Parameters.AddWithValue("@PARAM_ID", paramId);
            cmd.Parameters.AddWithValue("@DATE_RECORD", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            cmd.Parameters.AddWithValue("@PARAM_VALUE", value);

            cmd.ExecuteNonQuery();

            return 0;
        }
        public int AddDataRecord(string value, int dataBlockId, int paramId)
        {
            //int generatedId;
            int recordStateId = 1;
            if (dataBlockId == -1)
                throw new Exception("Файл не может быть добавлен, поэтому не может быть сохранена сопутствующая информация.");

            /*  generatedId = generateId("fn_data_records", "DATA_RECORD_ID");
              if (generatedId == -1)
                  throw (new Exception("Can't generate DATA_RECORD_ID"));            DATA_RECORD_ID,   @DATA_RECORD_ID,
              */
            //recordStateId = 1;

            MySqlCommand cmd = new MySqlCommand();

            string sql = "INSERT INTO fn_data_records "
                + "( DATA_BLOCK_ID, DATA_RECORD_STATE_ID, PARAM_ID, DATE_RECORD, PARAM_VALUE)"
                + "VALUES ( @DATA_BLOCK_ID, @DATA_RECORD_STATE_ID, @PARAM_ID, @DATE_RECORD, @PARAM_VALUE)";
            cmd = new MySqlCommand(sql, sqlConnection);
            // cmd.Parameters.AddWithValue("@DATA_RECORD_ID", generatedId);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            cmd.Parameters.AddWithValue("@DATA_RECORD_STATE_ID", recordStateId);
            cmd.Parameters.AddWithValue("@PARAM_ID", paramId);
            cmd.Parameters.AddWithValue("@DATE_RECORD", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@PARAM_VALUE", value);

            return cmd.ExecuteNonQuery();

            //return generatedId;
        }
        private int AddDataRecordState()
        {
            MySqlCommand cmd = new MySqlCommand();
            int generatedId;

            try
            {
                generatedId = generateId("fd_data_record_state", "DATA_RECORD_STATE_ID");
                if (generatedId == -1)
                    throw (new Exception("Can't generate DATA_RECORD_STATE_ID"));

                string sql = "INSERT INTO fd_data_record_state "
                    + "(DATA_RECORD_STATE_ID, STRID_DATA_RECORD_STATE_NAME)"
                    + "VALUES (@DATA_RECORD_STATE_ID, '1')";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@DATA_RECORD_STATE_ID", generatedId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
            // Console.WriteLine("Successefully added!");
            return generatedId;
        }
        public bool DeleteDataRecord(int dataRecordId)
        {
            MySqlTransaction trans = null;
            try
            {
                sqlConnection.Open();
                trans = sqlConnection.BeginTransaction();
                if (ProcessDeleting(dataRecordId) == false)
                    throw (new Exception("Error in deleting record!"));
                trans.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (trans != null) trans.Rollback();
                return false;
            }
            finally
            {
                sqlConnection.Close();
            }
            Console.WriteLine("Successefully deleted!");
            return true;
        }
        public void DeleteDataRecord(int dataBlockId, int paramId)
        {
            string sql = "DELETE FROM fn_data_records WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID AND PARAM_ID = @PARAM_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            cmd.Parameters.AddWithValue("@PARAM_ID", paramId);
            cmd.ExecuteNonQuery();
        }
        public int DeleteAllDataRecords(int dataBlockId)
        {
            int deletedRecordsCount = 0;

            string sql;
            int dataRecordsCount;
            int dataRecordId;
            MySqlCommand cmd;

            sql = "SELECT Count(DATA_RECORD_ID) FROM fn_data_records WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            dataRecordsCount = Convert.ToInt32(cmd.ExecuteScalar());

            for (int i = 1; i <= dataRecordsCount; i++)
            {
                sql = "SELECT DATA_RECORD_ID FROM fn_data_records WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
                dataRecordId = Convert.ToInt32(cmd.ExecuteScalar());
                if (ProcessDeleting(dataRecordId) == false)
                    throw (new Exception("Error in deleting DataRecord"));
                else
                    deletedRecordsCount++;
            }

            return deletedRecordsCount;
        }
        public int DeleteAllDataRecordsFast(int dataBlockId)
        {
            try
            {
                string sql = "DELETE FROM fn_data_records WHERE DATA_BLOCK_ID = @DATA_BLOCK_ID";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw (new Exception("Deleting error! Can't delete record with dataBlockid = " + dataBlockId.ToString()));
            }
            return 0;
        }
        private bool ProcessDeleting(int dataRecordId)
        {
            try
            {
                string sql = "DELETE FROM fn_data_records WHERE DATA_RECORD_ID = @DATA_RECORD_ID";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@DATA_RECORD_ID", dataRecordId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw (new Exception("Deleting error! Can't delete record id = " + dataRecordId.ToString()));
            }
            return true;
        }
        public int GetDataRecordState(int dataRecordStateId)
        {
            // MySqlConnection sqlConnection = new MySqlConnection();
            int dataBlockState = -1;
            // sqlConnection.ConnectionString = connectionString;
            try
            {
                string sql = "SELECT STRID_DATA_RECORD_STATE_NAME FROM fd_data_record_state WHERE DATA_RECORD_STATE_ID = @DATA_RECORD_STATE_ID";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@DATA_RECORD_STATE_ID", dataRecordStateId);
                sqlConnection.Open();
                dataBlockState = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
            finally
            {
                sqlConnection.Close();
            }
            if (dataBlockState == 0)
                Console.WriteLine(("no such state record(no record block)"));
            else
                Console.WriteLine(("dataRecordState = " + dataBlockState.ToString()));
            return dataBlockState;
        }
        private bool DeleteDataRecordState(int dataRecordStateId)
        {
            try
            {
                string sql = "DELETE FROM fd_data_record_state WHERE DATA_RECORD_STATE_ID = @DATA_RECORD_STATE_ID";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@DATA_RECORD_STATE_ID", dataRecordStateId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            Console.WriteLine("Successefully deleted!");
            return true;
        }
        //-----------------------------------------------------------------
        //----------------------fd_param ----------------------------------
        public int AddParam(string name, int parentParamId, int paramSize)
        {
            int generatedId = 0;
            MySqlCommand cmd = new MySqlCommand();
            string sql = "";

            sql = "SELECT PARAM_ID FROM fd_param WHERE PARAM_NAME = @PARAM_NAME";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@PARAM_NAME", name);
            generatedId = Convert.ToInt32(cmd.ExecuteScalar());
            if (generatedId == 0)
            {
                generatedId = generateId("fd_param", "PARAM_ID");
                if (generatedId == -1)
                    throw (new Exception("Can't generate PARAM_ID"));

                sql = "INSERT INTO fd_param "
                    + "(PARAM_ID, PARENT_PARAM_ID, PARAM_NAME, PARAM_SIZE)"
                    + "VALUES (@PARAM_ID, @PARENT_PARAM_ID, @PARAM_NAME, @PARAM_SIZE)";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@PARAM_ID", generatedId);
                cmd.Parameters.AddWithValue("@PARENT_PARAM_ID", parentParamId);
                cmd.Parameters.AddWithValue("@PARAM_NAME", name);
                cmd.Parameters.AddWithValue("@PARAM_SIZE", paramSize);
                cmd.ExecuteNonQuery();
            }
            else return generatedId;
            // Console.WriteLine("Successefully added!");
            return generatedId;
        }
        public int getParamId(string name)
        {
            int getedId = 0;
            MySqlCommand cmd = new MySqlCommand();
            string sql = "";

            sql = "SELECT PARAM_ID FROM fd_param WHERE PARAM_NAME = @PARAM_NAME";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@PARAM_NAME", name);
            getedId = Convert.ToInt32(cmd.ExecuteScalar());
            if (getedId == 0)
            {
                return -1;
            }
            else return getedId;
        }
        public bool DeleteParam(int paramId)
        {
            try
            {
                string sql = "DELETE FROM fd_param WHERE PARAM_ID = @PARAM_ID";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@PARAM_ID", paramId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            Console.WriteLine("Successefully deleted!");
            return true;
        }
        public bool IsParamInDataBlock(int dataBlockId, string paramName)
        {
            int getedCount = 0;
            MySqlCommand cmd = new MySqlCommand();
            string sql = "";
            int paramId = getParamId(paramName);
            if (paramId <= 0)
                return false;

            sql = "SELECT COUNT(*) FROM fn_data_records WHERE DATA_BLOCK_ID=@DATA_BLOCK_ID AND PARAM_ID = @PARAM_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DATA_BLOCK_ID", dataBlockId);
            cmd.Parameters.AddWithValue("@PARAM_ID", paramId);
            getedCount = Convert.ToInt32(cmd.ExecuteScalar());
            if (getedCount <= 0)
            {
                return false;
            }
            else return true;
        }
        //------------------------fd_string
        public string GetString(int stringId, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            string returnValue;

            string sql = "SELECT " + Language + " FROM fd_string WHERE STRING_ID = " + stringId.ToString();
            cmd = new MySqlCommand(sql, sqlConnection);
            returnValue = Convert.ToString(cmd.ExecuteScalar());

            return returnValue;
        }
        public int GetStringId(string stringValue, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;

            string sql = "SELECT STRING_ID FROM fd_string WHERE " + Language + " =@" + Language;
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@" + Language, stringValue);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());

            return returnValue;
        }
        public int GetStringId(string stringValue)
        {
            return GetStringId(stringValue, "STRING_RU");
        }
        public int AddOrGetString(string STRING_RU)
        {
            int stringId = AddOrGetString(STRING_RU, "STRING_RU");
            return stringId;
        }
        public int AddOrGetString(string stringValue, string Language)
        {
            int stringId = 0;
            MySqlCommand cmd = new MySqlCommand();
            string sql = "";

            sql = "SELECT STRING_ID FROM fd_string WHERE " + Language + "=" + "@" + Language;
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@" + Language, stringValue);
            stringId = Convert.ToInt32(cmd.ExecuteScalar());
            if (stringId == 0)
            {
                stringId = AddString(stringValue, Language);
            }
            return stringId;
        }
        public int AddString(string stringValue, string Language)
        {
            int stringId = 0;
            stringId = generateId("fd_string", "STRING_ID");
            if (stringId == -1)
                throw (new Exception("Can't generate STRING_ID"));

            return AddString(stringValue, stringId, Language);
        }
        public int AddString(string stringValue, int stringId, string Language)//используется для редактирования строк. 
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql = "";

            //ENGLISH FIRST
            sql = "INSERT INTO fd_string "
               + "(STRING_ID, STRING_RU )"
               + "VALUES (@STRING_ID, @STRING_RU )";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STRING_ID", stringId);
            cmd.Parameters.AddWithValue("@STRING_RU", stringValue);
            cmd.ExecuteNonQuery();
            ///////////////Other Language
            if (Language != "STRING_RU")
            {
                TranslateString(stringValue, Language, stringId);
            }
            return stringId;
        }

        public void TranslateString(string stringValue, string Language, int stringId)
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql = "";

            sql = "UPDATE fd_string SET " + Language + "=@" + Language + " WHERE STRING_ID=@STRING_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STRING_ID", stringId);
            cmd.Parameters.AddWithValue("@" + Language, stringValue);
            cmd.ExecuteNonQuery();
        }
        public void DeleteString(int stringId)
        {
            string sql = "DELETE FROM fd_string WHERE STRING_ID = @STRING_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STRING_ID", stringId);
            cmd.ExecuteNonQuery();
        }
        public int EditAnySTRIDValue(string newValue, string STRID_NAME, string Language, string tableName, string primaryName, int primaryValue)
        {
            bool toTranslate = true;
            int oldStringId = Convert.ToInt32(GetOneParameter(primaryValue, primaryName, tableName, STRID_NAME));
            int newStringId = GetStringId(newValue, Language);
            if (oldStringId == 0)
                oldStringId = -1;
            if (newStringId != 0)
                toTranslate = false;
            MySqlCommand cmd = new MySqlCommand();
            string sql;

            if (oldStringId != newStringId)//поправить после теста на !=
            {
                //выборка всей строки со всеми языками из таблицы строк
                System.Data.DataTable dTable = new System.Data.DataTable();
                string query = "SELECT * FROM fd_string WHERE STRING_ID=" + oldStringId.ToString();
                MySqlDataAdapter dAdapter = new MySqlDataAdapter(query, connectionString);
                MySqlCommandBuilder cBuilder = new MySqlCommandBuilder(dAdapter);
                dAdapter.Fill(dTable);
                //------------------------------------------------------
                //ставим значение в dbNull
                sql = "UPDATE " + tableName + " SET " + STRID_NAME + "=@" + STRID_NAME + " WHERE " + primaryName + "=@" + primaryName;
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@" + primaryName, primaryValue);
                cmd.Parameters.AddWithValue("@" + STRID_NAME, DBNull.Value);
                cmd.ExecuteNonQuery();
                //удаляем
                //если удалилось - поставить новую строку на тоже место! Если не удалилось - создать новую.
                bool deleted = true;
                try { DeleteString(oldStringId); }
                catch { deleted = false; }
                //добавляем строку
                if (deleted)
                {
                    //newStringId = GetStringId(newValue, Language); *звездочки - убрать коменты, если newstring надо будет спецом еще инициализировать.
                    if (newStringId == 0)
                        newStringId = AddString(newValue, oldStringId, Language);
                    //else *
                    //    newStringId = AddOrGetString(newValue, Language); *
                }
                else
                {
                    if (newStringId == 0)
                        newStringId = AddOrGetString(newValue, Language);
                    //тут тоже самое. внимательно просмотреть при проблемах!
                }
                //Перевод всех строк
                if (toTranslate)
                {
                    System.Collections.Generic.KeyValuePair<string, string> LangValue;

                    List<string> allValues = new List<string>();
                    foreach (object obj in dTable.Rows[0].ItemArray.ToList())
                    {
                        allValues.Add(obj.ToString());
                    }
                    string colName;
                    int i = 0;
                    foreach (System.Data.DataColumn column in dTable.Columns)
                    {
                        colName = column.ColumnName;

                        if (colName == "STRING_ID" || colName == Language)
                        {
                            i++;
                            continue;
                        }
                        TranslateString(allValues[i], colName, newStringId);
                        i++;
                    }
                }
                //Ставим новое сначение STRID таблице
                sql = "UPDATE " + tableName + " SET " + STRID_NAME + "=@" + STRID_NAME + " WHERE " + primaryName + "=@" + primaryName;
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@" + primaryName, primaryValue);
                cmd.Parameters.AddWithValue("@" + STRID_NAME, newStringId);
                cmd.ExecuteNonQuery();
            }
            return newStringId;
        }
        /// <summary>
        /// Этот метод для таблиц - расширенная информация пользователе(или тс). Все таблицы с составным первичным ключем(айди пользователя и айди параметра например).
        /// </summary>
        public int EditAnySTRIDValue(string newValue, string STRID_NAME, string Language, string tableName,
            string primaryNameOne, int primaryValueOne, string primaryNameTwo, int primaryValueTwo)
        {
            bool toTranslate = true;
            int oldStringId = Convert.ToInt32(GetOneParameter(primaryValueOne, primaryNameOne, primaryValueTwo, primaryNameTwo, tableName, STRID_NAME));
            int newStringId = GetStringId(newValue, Language);
            if (oldStringId == 0)
                oldStringId = -1;
            if (newStringId != 0)
                toTranslate = false;
            MySqlCommand cmd = new MySqlCommand();
            string sql;

            if (oldStringId != newStringId)
            {
                //выборка всей строки со всеми языками из таблицы строк
                System.Data.DataTable dTable = new System.Data.DataTable();
                string query = "SELECT * FROM fd_string WHERE STRING_ID=" + oldStringId.ToString();
                MySqlDataAdapter dAdapter = new MySqlDataAdapter(query, connectionString);
                MySqlCommandBuilder cBuilder = new MySqlCommandBuilder(dAdapter);
                dAdapter.Fill(dTable);
                //------------------------------------------------------
                //ставим значение в dbNull
                sql = "UPDATE " + tableName + " SET " + STRID_NAME + "=@" + STRID_NAME + " WHERE "
                    + primaryNameOne + "=@" + primaryNameOne + " AND " + primaryNameTwo + "=@" + primaryNameTwo;
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@" + primaryNameOne, primaryValueOne);
                cmd.Parameters.AddWithValue("@" + primaryNameTwo, primaryValueTwo);
                cmd.Parameters.AddWithValue("@" + STRID_NAME, DBNull.Value);
                cmd.ExecuteNonQuery();
                //удаляем
                //если удалилось - поставить новую строку на тоже место! Если не удалилось - создать новую.
                bool deleted = true;
                try { DeleteString(oldStringId); }
                catch { deleted = false; }
                //добавляем строку
                if (deleted)
                {
                    //newStringId = GetStringId(newValue, Language); *звездочки - убрать коменты, если newstring надо будет спецом еще инициализировать.
                    if (newStringId == 0)
                        newStringId = AddString(newValue, oldStringId, Language);
                    //else *
                    //    newStringId = AddOrGetString(newValue, Language); *
                }
                else
                {
                    if (newStringId == 0)
                        newStringId = AddOrGetString(newValue, Language);
                    //тут тоже самое. внимательно просмотреть при проблемах!
                }
                //Перевод всех строк
                if (toTranslate)
                {
                    System.Collections.Generic.KeyValuePair<string, string> LangValue;

                    List<string> allValues = new List<string>();
                    foreach (object obj in dTable.Rows[0].ItemArray.ToList())
                    {
                        allValues.Add(obj.ToString());
                    }
                    string colName;
                    int i = 0;
                    foreach (System.Data.DataColumn column in dTable.Columns)
                    {
                        colName = column.ColumnName;

                        if (colName == "STRING_ID" || colName == Language)
                        {
                            i++;
                            continue;
                        }
                        TranslateString(allValues[i], colName, newStringId);
                        i++;
                    }
                }
                //Ставим новое сначение STRID таблице
                sql = "UPDATE " + tableName + " SET " + STRID_NAME + "=@" + STRID_NAME + " WHERE "
                + primaryNameOne + "=@" + primaryNameOne + " AND " + primaryNameTwo + "=@" + primaryNameTwo;
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@" + primaryNameOne, primaryValueOne);
                cmd.Parameters.AddWithValue("@" + primaryNameTwo, primaryValueTwo);
                cmd.Parameters.AddWithValue("@" + STRID_NAME, newStringId);
                cmd.ExecuteNonQuery();
            }
            return newStringId;
        }
        //---------------------------------User authetification---
        #region "userTables"
        //fd_user
        public int GetUserAndPasswordCount(string userName, string password)
        {
            int count = 0;
            try
            {
                string sql = "Select COUNT(*) FROM fd_user WHERE USER_LOGIN=@UserName AND USER_PASSWORD=@Password";


                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);

                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            return count;
        }
        public int GetUserAndPasswordCount(string userName, string password, int orgId)
        {
            int count = 0;
            try
            {
                string sql = "Select COUNT(*) FROM fd_user WHERE USER_LOGIN=@UserName AND USER_PASSWORD=@Password AND ORG_ID=@ORG_ID";

                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@UserName", userName);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@ORG_ID", orgId);

                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
            return count;
        }
        public string GetUserPassword(int userId)
        {
            MySqlCommand cmd = new MySqlCommand();
            string returnValue;
            string sql = "SELECT USER_PASSWORD FROM fd_user WHERE USER_ID=@USER_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            returnValue = Convert.ToString(cmd.ExecuteScalar());
            return returnValue;
        }
        public string GetUserName(int userId)
        {
            MySqlCommand cmd = new MySqlCommand();
            string returnValue;
            string sql = "SELECT USER_LOGIN FROM fd_user WHERE USER_ID=@USER_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            returnValue = Convert.ToString(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetUserId_byUserName(string userName)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT USER_ID FROM fd_user WHERE USER_LOGIN=@USER_LOGIN";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_LOGIN", userName);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetUserOrgId(int userId)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                int returnValue;
                string sql = "SELECT ORG_ID FROM fd_user WHERE USER_ID=@USER_ID";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@USER_ID", userId);
                returnValue = Convert.ToInt32(cmd.ExecuteScalar());
                return returnValue;
            }
            catch
            {
                return -1;
            }
        }
        public string GetDateConnect(int userId)
        {
            MySqlCommand cmd = new MySqlCommand();
            string returnValue;
            string sql = "SELECT DATE_CONNECT FROM fd_user WHERE USER_ID=@USER_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            returnValue = Convert.ToString(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetUserTypeId(int userId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT USER_TYPE_ID FROM fd_user WHERE USER_ID=@USER_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public List<int> GetAllUsersId()
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT USER_ID FROM fd_user WHERE USER_DELETED IS NULL ORDER BY USER_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public List<int> GetAllUsersId(int orgId)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT USER_ID FROM fd_user WHERE USER_DELETED IS NULL AND ORG_ID=@ORG_ID ORDER BY USER_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public List<int> GetAllUsersId(int orgId, int UserTypeId)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT USER_ID FROM fd_user WHERE USER_DELETED IS NULL AND ORG_ID=@ORG_ID AND USER_TYPE_ID=@USER_TYPE_ID ORDER BY USER_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@USER_TYPE_ID", UserTypeId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        //
        public int getUserRoleId(int userId)//fn_user_rights
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT USER_ROLE_ID FROM fn_user_rights WHERE USER_ID=@USER_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetUserTypeNameId(int UserTypeId)//fd_user_type
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT STRID_USER_TYPE_NAME FROM fd_user_type WHERE USER_TYPE_ID=@USER_TYPE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_TYPE_ID", UserTypeId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetUserRoleNameId(int UserRoleId)//fd_user_role
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT STRID_USER_ROLE_NAME FROM fd_user_role WHERE USER_ROLE_ID=@USER_ROLE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ROLE_ID", UserRoleId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetUserTypeId_byUserTypeNameId(int UserTypeNameId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT USER_TYPE_ID FROM fd_user_type WHERE STRID_USER_TYPE_NAME=@STRID_USER_TYPE_NAME";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STRID_USER_TYPE_NAME", UserTypeNameId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetUserRoleId_byUserRoleNameId(int UserRoleNameId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT USER_ROLE_ID FROM fd_user_role WHERE STRID_USER_ROLE_NAME=@STRID_USER_ROLE_NAME";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STRID_USER_ROLE_NAME", UserRoleNameId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetUserRightsNameId(int UserRightsId)//fd_user_role
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT STRID_USER_RIGHTS_NAME FROM fd_user_rights WHERE USER_RIGHTS_ID=@USER_RIGHTS_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_RIGHTS_ID", UserRightsId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetUserRightsId(int UserRoleId)//fn_role_rights
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT USER_RIGHTS_ID FROM fn_role_rights WHERE USER_ROLE_ID=@USER_ROLE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ROLE_ID", UserRoleId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public List<KeyValuePair<string, int>> GetUserRoles(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();

            List<int> allUserRollesIds = new List<int>();

            string sql = "SELECT USER_ROLE_ID FROM fd_user_role ORDER BY USER_ROLE_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                allUserRollesIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            KeyValuePair<string, int> hash;
            int userRoleNameId;
            foreach (int id in allUserRollesIds)
            {
                userRoleNameId = GetUserRoleNameId(id);
                hash = new KeyValuePair<string, int>(GetString(userRoleNameId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public List<KeyValuePair<string, int>> GetUserTypes(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();

            List<int> allUserTypesIds = new List<int>();

            string sql = "SELECT USER_TYPE_ID FROM fd_user_type ORDER BY USER_TYPE_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                allUserTypesIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            KeyValuePair<string, int> hash;
            int userTypeNameId;
            foreach (int id in allUserTypesIds)
            {
                userTypeNameId = GetUserTypeNameId(id);
                hash = new KeyValuePair<string, int>(GetString(userTypeNameId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public List<int> GetAllCountries()
        {
            List<int> result = new List<int>();
            string sql = "SELECT country_id FROM fd_country ORDER BY STRID_COUNTRY_NAME";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                result.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return result;
        }
        public string GetCountryName(int id)
        {
            string sql = "SELECT STRID_COUNTRY_NAME FROM fd_country WHERE country_id=@ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ID", id);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                int cid=sdr.GetInt32(0);
                sdr.Close();
                string result = GetString(cid, "STRING_RU");
                return result;
            }
            sdr.Close();
            return "";
        }
        public List<int> GetAllCities(int countryId)
        {
            List<int> result = new List<int>();
            string sql = "SELECT city_id FROM fd_city WHERE country_id=@COUNTRY_ID ORDER BY STRID_CITY_NAME";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@COUNTRY_ID", countryId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                result.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return result;
        }
        public string GetCityName(int id)
        {
            string sql = "SELECT STRID_CITY_NAME FROM fd_city WHERE city_id=@ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ID", id);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                int cid = sdr.GetInt32(0);
                sdr.Close();
                string result = GetString(cid, "STRING_RU");
                return result;
            }
            sdr.Close();
            return "";
        }
        public int AddNewUser(string name, string pass, int userTypeId, int userRoleId, int orgId, string oldName, string oldPass)
        {
            int generatedId = 0;
            string sql = "";
            string findName = name;
            if (name != oldName)
                findName = oldName;
            string findPass = pass;
            if (pass != oldPass)
                findPass = oldPass;

            sql = "SELECT USER_ID FROM fd_user WHERE USER_LOGIN = @USER_LOGIN AND USER_PASSWORD=@USER_PASSWORD";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_LOGIN", findName);
            cmd.Parameters.AddWithValue("@USER_PASSWORD", findPass);
            generatedId = Convert.ToInt32(cmd.ExecuteScalar());
            if (generatedId == 0)
            {
                generatedId = generateId("fd_user", "USER_ID");
                if (generatedId == -1)
                    throw (new Exception("Can't generate USER_ID"));

                sql = "INSERT INTO fd_user "
                    + "(USER_ID, USER_LOGIN, USER_PASSWORD, USER_TYPE_ID, ORG_ID)"
                    + "VALUES (@USER_ID, @USER_LOGIN, @USER_PASSWORD, @USER_TYPE_ID, @ORG_ID)";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@USER_ID", generatedId);
                cmd.Parameters.AddWithValue("@USER_LOGIN", name);
                cmd.Parameters.AddWithValue("@USER_PASSWORD", pass);
                cmd.Parameters.AddWithValue("@USER_TYPE_ID", userTypeId);
                cmd.Parameters.AddWithValue("@ORG_ID", orgId);
                cmd.ExecuteNonQuery();

                sql = "INSERT INTO fn_user_rights (USER_ID, USER_ROLE_ID) VALUES (@USER_ID, @USER_ROLE_ID)";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@USER_ID", generatedId);
                cmd.Parameters.AddWithValue("@USER_ROLE_ID", userRoleId);
                cmd.ExecuteNonQuery();
            }
            else
            {
                sql = "UPDATE fd_user SET USER_LOGIN=@USER_LOGIN, USER_PASSWORD=@USER_PASSWORD, USER_TYPE_ID=@USER_TYPE_ID, ORG_ID=@ORG_ID WHERE USER_ID=@USER_ID";//, ORG_ID=@ORG_ID  добавить вконец, когда будут организации.
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@USER_ID", generatedId);
                cmd.Parameters.AddWithValue("@USER_LOGIN", name.ToString());
                cmd.Parameters.AddWithValue("@USER_PASSWORD", pass);
                cmd.Parameters.AddWithValue("@USER_TYPE_ID", userTypeId);
                cmd.Parameters.AddWithValue("@ORG_ID", orgId);
                cmd.ExecuteNonQuery();

                sql = "UPDATE fn_user_rights SET USER_ROLE_ID=@USER_ROLE_ID WHERE USER_ID=@USER_ID";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@USER_ID", generatedId);
                cmd.Parameters.AddWithValue("@USER_ROLE_ID", userRoleId);
                cmd.ExecuteNonQuery();
            }
            return generatedId;
        }
        public void EditUserPassword(int curUserId, string pass)
        {
            string sql = "UPDATE fd_user SET USER_PASSWORD=@USER_PASSWORD WHERE USER_ID=@USER_ID";//, ORG_ID=@ORG_ID  добавить вконец, когда будут организации.
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", curUserId);
            cmd.Parameters.AddWithValue("@USER_PASSWORD", pass);
            cmd.ExecuteNonQuery();
        }

        public void EditUserLogin(int curUserId, string login)
        {
            string sql = "UPDATE fd_user SET USER_LOGIN=@USER_LOGIN WHERE USER_ID=@USER_ID";//, ORG_ID=@ORG_ID  добавить вконец, когда будут организации.
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", curUserId);
            cmd.Parameters.AddWithValue("@USER_LOGIN", login);
            cmd.ExecuteNonQuery();
        }

        public void EditUserType(int curUserId, int type)
        {
            string sql = "UPDATE fd_user SET USER_TYPE_ID=@USER_TYPE WHERE USER_ID=@USER_ID";//, ORG_ID=@ORG_ID  добавить вконец, когда будут организации.
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", curUserId);
            cmd.Parameters.AddWithValue("@USER_TYPE", type);
            cmd.ExecuteNonQuery();
        }
        
        public void DeleteUser(int UserId)
        {
            string sql = "DELETE FROM fn_user_rights WHERE USER_ID = @USER_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", UserId);
            cmd.ExecuteNonQuery();

            sql = "DELETE FROM fd_user WHERE USER_ID = @USER_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", UserId);
            cmd.ExecuteNonQuery();
        }
        public void DeleteUserSoft(int UserId)
        {
            string sql = "UPDATE fd_user SET USER_DELETED=@DATE WHERE USER_ID = @USER_ID ";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", UserId);
            cmd.Parameters.AddWithValue("@DATE", DateTime.Now);
            cmd.ExecuteNonQuery();

            sql = "SELECT USER_LOGIN FROM fd_user WHERE USER_ID=@ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ID", UserId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            string value = "";
            while (sdr.Read())
            {
                value = sdr.GetString(0);
            }
            sdr.Close();

            sql = "UPDATE fd_user SET USER_LOGIN = @STR WHERE USER_ID=@ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STR", "###" + value);
            cmd.Parameters.AddWithValue("@ID", UserId);
            cmd.ExecuteNonQuery();
        }
        //FD_USER_INFO_SET and FD_USER_INFO
        public int GetUserInfoValueStrId(int userId, int UserInfoId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT STRID_USER_INFO_VALUE FROM fd_user_info_set WHERE USER_ID=@USER_ID AND USER_INFO_ID=@USER_INFO_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            cmd.Parameters.AddWithValue("@USER_INFO_ID", UserInfoId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetUserInfoName(int InfoNameId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(InfoNameId, "STRID_USER_INFO_NAME", "fd_user_info", "USER_INFO_ID"));
            return returnValue;
        }
        public List<KeyValuePair<string, int>> GetAllUserInfoNames(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();

            List<int> AllUserInfoNames = new List<int>();

            string sql = "SELECT USER_INFO_ID FROM fd_user_info ORDER BY USER_INFO_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                AllUserInfoNames.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            KeyValuePair<string, int> hash;
            int userInfoNameId;
            foreach (int id in AllUserInfoNames)
            {
                userInfoNameId = GetUserInfoName(id);
                hash = new KeyValuePair<string, int>(GetString(userInfoNameId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public int AddUserInfoName(string InfoName, string Language)
        {
            int stringId = AddOrGetString(InfoName);
            // TranslateString(InfoName, Language, stringId);

            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_user_info", "USER_INFO_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate USER_INFO_ID"));

            string sql = "INSERT INTO fd_user_info "
                + "(USER_INFO_ID, STRID_USER_INFO_NAME)"
                + "VALUES (@USER_INFO_ID, @STRID_USER_INFO_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_INFO_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_USER_INFO_NAME", stringId);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public void AddUserInfoValue(int userId, int UserInfoId, string value, string Language)
        {
            int stringId = AddOrGetString(value, Language);
            MySqlCommand cmd = new MySqlCommand();
            string sql = "INSERT INTO fd_user_info_set "
                + "(USER_ID, USER_INFO_ID, STRID_USER_INFO_VALUE)"
                + "VALUES (@USER_ID, @USER_INFO_ID, @STRID_USER_INFO_VALUE)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            cmd.Parameters.AddWithValue("@USER_INFO_ID", UserInfoId);
            cmd.Parameters.AddWithValue("@STRID_USER_INFO_VALUE", stringId);
            cmd.ExecuteNonQuery();
        }
        public void EditUserInfo(int userId, int UserInfoId, string newValue, string Language)
        {
            EditAnySTRIDValue(newValue, "STRID_USER_INFO_VALUE", Language, "fd_user_info_set", "USER_INFO_ID", UserInfoId, "USER_ID", userId);
        }
        public int GetUserInfoUserId(int valueStrId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(valueStrId, "STRID_USER_INFO_VALUE", "fd_user_info_set", "USER_ID"));
            return returnValue;
        }
        public List<int> GetAllMessagesIds(int userId)
        {
            List<int> result = new List<int>();
            string sql = "SELECT MESSAGE_ID FROM fn_message WHERE USER_ID=@USER_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                result.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return result;
        }
        public void DeleteMessage(int messageId)
        {
            string sql = "DELETE FROM fn_message WHERE MESSAGE_ID=@MESS_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@MESS_ID", messageId);
            cmd.ExecuteNonQuery();
        }
        public string GetMessageTopic(int messageId)
        {
            string returnValue = Convert.ToString(GetOneParameter(messageId, "MESSAGE_ID", "fn_message", "MESSAGE_TOPIC"));
            return returnValue;
        }
        public string GetMessageSender(int messageId)
        {
            string returnValue = Convert.ToString(GetOneParameter(messageId, "MESSAGE_ID", "fn_message", "MESSAGE_SENDER"));
            return returnValue;
        }
        public DateTime GetMessageDate(int messageId)
        {
            DateTime returnValue = Convert.ToDateTime(GetOneParameter(messageId, "MESSAGE_ID", "fn_message", "MESSAGE_DATE"));
            return returnValue;
        }
        public DateTime GetMessageEndDate(int messageId)
        {
            DateTime returnValue = Convert.ToDateTime(GetOneParameter(messageId, "MESSAGE_ID", "fn_message", "MESSAGE_END_DATE"));
            return returnValue;
        }
        #endregion
        //---------------------------------FD_ORG and OrganizationTables------------------
        #region "ORG"
        public int GetOrgNameId(int ORG_ID)
        {
            int getedValue;

            string sql = "SELECT STRID_ORG_NAME FROM fd_org WHERE ORG_ID = @ORG_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", ORG_ID);
            getedValue = Convert.ToInt32(cmd.ExecuteScalar());
            return getedValue;
        }
        public int GetOrgId_byOrgNameStr(string name, string language)
        {
            OpenConnection();
            int nameId = GetStringId(name, language);
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT ORG_ID FROM fd_org WHERE STRID_ORG_NAME=@STRID_ORG_NAME";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STRID_ORG_NAME", nameId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public void SetOrgName(string name, int ORG_ID, string Language)
        {
            EditAnySTRIDValue(name, "STRID_ORG_NAME", Language, "fd_org", "ORG_ID", ORG_ID);
        }
        public int GetOrgTypeNameId(int orgTypeId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT STRID_ORG_TYPE_NAME FROM fd_org_type WHERE ORG_TYPE_ID=@ORG_TYPE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_TYPE_ID", orgTypeId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetOrgCountryId(int orgId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT COUNTRY_ID FROM fd_org WHERE ORG_ID=@ORG_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetOrgRegionId(int orgId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT REGION_ID FROM fd_org WHERE ORG_ID=@ORG_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetOrgTypeId(int orgId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT ORG_TYPE_ID FROM fd_org WHERE ORG_ID=@ORG_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetOrgId_byOrgNameId(int orgNameId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT ORG_ID FROM fd_org WHERE STRID_ORG_NAME=@STRID_ORG_NAME";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STRID_ORG_NAME", orgNameId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetOrgInfoId_bySTRID(int irgInfoStringId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(irgInfoStringId, "STRID_ORG_INFO_NAME", "fd_org_info", "ORG_INFO_ID"));
            return returnValue;
        }
        public void SetOrgCountryAndRegion(int orgId, int countryId, int regionId)
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql = "UPDATE fd_org SET COUNTRY_ID=@COUNTRY_ID, REGION_ID=@REGION_ID WHERE ORG_ID=@ORG_ID";//, ORG_ID=@ORG_ID  добавить вконец, когда будут организации.
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@COUNTRY_ID", countryId);
            cmd.Parameters.AddWithValue("@REGION_ID", regionId);
            cmd.ExecuteNonQuery();
        }
        public List<int> GetAllOrganizationsId()
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT ORG_ID FROM fd_org ORDER BY ORG_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public List<int> GetAllOrganizationsId(int parentOrgId, int dealerTypeId, int subDealerId, int preDealerId)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT ORG_ID FROM fd_org WHERE DATE_DELETE IS NULL AND "
                + "ORG_TYPE_ID NOT IN (" + dealerTypeId.ToString() + "," + subDealerId.ToString() + "," + preDealerId.ToString() + ") "
                + "AND PARENT_ORG_ID=@PARENT_ORG_ID ORDER BY ORG_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@PARENT_ORG_ID", parentOrgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public List<int> GetAllDealersId(int parentOrgId, int dealerTypeId, int subDealerId)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT ORG_ID FROM fd_org WHERE "
                + "ORG_TYPE_ID IN (" + dealerTypeId.ToString() + "," + subDealerId.ToString() + ") "
                + "AND PARENT_ORG_ID=@PARENT_ORG_ID AND DATE_DELETE IS NULL ORDER BY ORG_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@PARENT_ORG_ID", parentOrgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public List<KeyValuePair<string, int>> GetOrganizationNames(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> hash;
            List<int> allOrganizationIds = new List<int>();
            allOrganizationIds = GetAllOrganizationsId();
            string sql = "SELECT STRID_ORG_NAME FROM fd_org WHERE DATE_DELETE IS NULL AND ORG_ID=@ORG_ID";
            MySqlCommand cmd;
            int stringId;
            foreach (int id in allOrganizationIds)
            {
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@ORG_ID", id);
                stringId = Convert.ToInt32(cmd.ExecuteScalar());
                hash = new KeyValuePair<string, int>(GetString(stringId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public List<KeyValuePair<string, int>> GetOrgTypes(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> hash;
            List<int> allOrgTypesIds = new List<int>();
            string sql = "SELECT ORG_TYPE_ID FROM fd_org_type";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                allOrgTypesIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            sql = "SELECT STRID_ORG_TYPE_NAME FROM fd_org_type WHERE ORG_TYPE_ID=@ORG_TYPE_ID";
            int stringId;
            foreach (int id in allOrgTypesIds)
            {
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@ORG_TYPE_ID", id);
                stringId = Convert.ToInt32(cmd.ExecuteScalar());
                hash = new KeyValuePair<string, int>(GetString(stringId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public int AddNewOrganization(string newName, int orgTypeId, int orgCountryId, int orgRegionId, string oldName, string language)
        {
            int generatedId = 0;
            string sql = "";
            string findName = newName;
            if (newName != oldName)
                findName = oldName;
            int name = GetStringId(findName, language);
            sql = "SELECT ORG_ID FROM fd_org WHERE STRID_ORG_NAME=@STRID_ORG_NAME";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STRID_ORG_NAME", name);
            generatedId = Convert.ToInt32(cmd.ExecuteScalar());
            if (generatedId == 0)
            {
                generatedId = generateId("fd_org", "ORG_ID");
                if (generatedId == -1)
                    throw (new Exception("Can't generate ORG_ID"));
                name = AddOrGetString(findName);
                sql = "INSERT INTO fd_org "
                    + "(ORG_ID, OBJECT_ID, ORG_TYPE_ID, COUNTRY_ID, REGION_ID, STRID_ORG_NAME)"
                    + "VALUES (@ORG_ID, @OBJECT_ID, @ORG_TYPE_ID, @COUNTRY_ID, @REGION_ID, @STRID_ORG_NAME)";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@ORG_ID", generatedId);
                cmd.Parameters.AddWithValue("@OBJECT_ID", 1);
                cmd.Parameters.AddWithValue("@ORG_TYPE_ID", orgTypeId);
                cmd.Parameters.AddWithValue("@COUNTRY_ID", orgCountryId);
                cmd.Parameters.AddWithValue("@REGION_ID", orgRegionId);
                cmd.Parameters.AddWithValue("@STRID_ORG_NAME", name);
                cmd.ExecuteNonQuery();

                sql = "INSERT INTO fn_groups "
                    + " (GROUP_NAME, GROUP_COMMENT, ORG_ID, CARD_TYPE_ID) "
                    + "VALUES (@GROUP_NAME, @GROUP_COMMENT, @ORG_ID, @CARD_TYPE_ID) ";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@GROUP_NAME", "Общая группа");
                cmd.Parameters.AddWithValue("@GROUP_COMMENT", "Группа по умолчанию");
                cmd.Parameters.AddWithValue("@ORG_ID", generatedId);
                cmd.Parameters.AddWithValue("@CARD_TYPE_ID", 0);
                cmd.ExecuteNonQuery();

            }
            else
            {
                if (name <= 0)
                    throw (new Exception("Проблемы с базой данных, нет необходимых данных"));
                sql = "UPDATE fd_org SET ORG_TYPE_ID=@ORG_TYPE_ID, COUNTRY_ID=@COUNTRY_ID, REGION_ID=@REGION_ID WHERE ORG_ID=@ORG_ID";//, ORG_ID=@ORG_ID  добавить вконец, когда будут организации.
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@ORG_ID", generatedId);
                cmd.Parameters.AddWithValue("@ORG_TYPE_ID", orgTypeId);
                cmd.Parameters.AddWithValue("@COUNTRY_ID", orgCountryId);
                cmd.Parameters.AddWithValue("@REGION_ID", orgRegionId);
                cmd.ExecuteNonQuery();

                SetOrgName(newName, generatedId, language);//Меняем название организации.
            }
            return generatedId;
        }
        public void SetParentOrganization(int orgId, int ParentOrgId)
        {
            MySqlCommand cmd;
            string sql = "UPDATE fd_org SET PARENT_ORG_ID=@PARENT_ORG_ID WHERE ORG_ID=@ORG_ID";

            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@PARENT_ORG_ID", ParentOrgId);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.ExecuteNonQuery();
        }
        public int GetOrgParentOrganization(int orgId)
        {
            int retVal = -1;
            if (int.TryParse(GetOneParameter(orgId, "ORG_ID", "fd_org", "PARENT_ORG_ID").ToString(), out retVal))
                return retVal;
            else
                return -1;
        }
        public void AddAdditionalOrgInfo(int orgId, int ORG_INFO_ID, string value, string language)
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql;
            int stringId = GetAdditionalOrgInfoValueId(orgId, ORG_INFO_ID);

            if (stringId > 0)
            {
                EditAnySTRIDValue(value, "STRID_ORG_INFO_VALUE", language, "fd_org_info_set", "ORG_ID", orgId, "ORG_INFO_ID", ORG_INFO_ID);
            }
            else
            {
                int newStringInfo = AddOrGetString(value, language);

                sql = "INSERT INTO fd_org_info_set "
                    + "(ORG_ID, ORG_INFO_ID, STRID_ORG_INFO_VALUE)"
                    + "VALUES (@ORG_ID, @ORG_INFO_ID, @STRID_ORG_INFO_VALUE)";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@ORG_INFO_ID", ORG_INFO_ID);
                cmd.Parameters.AddWithValue("@ORG_ID", orgId);
                cmd.Parameters.AddWithValue("@STRID_ORG_INFO_VALUE", newStringInfo);
                cmd.ExecuteNonQuery();
            }
        }
        public int GetOrgInfoName(int InfoNameId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(InfoNameId, "STRID_ORG_INFO_NAME", "fd_org_info", "ORG_INFO_ID"));
            return returnValue;
        }
        public int GetAdditionalOrgInfoValueId(int orgId, int ORG_INFO_ID)
        {
            return Convert.ToInt32(GetOneParameter(ORG_INFO_ID, "ORG_INFO_ID", orgId, "ORG_ID", "fd_org_info_set", "STRID_ORG_INFO_VALUE"));
        }
        public List<KeyValuePair<string, int>> GetAllOrgInfoIds(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> hash;
            List<int> allOrgInfosIds = new List<int>();
            string sql = "SELECT ORG_INFO_ID FROM fd_org_info";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                allOrgInfosIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            //sql = "SELECT STRID_ORG_INFO_NAME FROM fd_org_info WHERE ORG_INFO_ID=@ORG_INFO_ID";
            int stringId;
            foreach (int id in allOrgInfosIds)
            {
                stringId = Convert.ToInt32(GetOneParameter(id, "ORG_INFO_ID", "fd_org_info", "STRID_ORG_INFO_NAME"));
                //  if (stringId == 0) stringId = -1;
                hash = new KeyValuePair<string, int>(GetString(stringId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public void DeleteOrgInfo(int orgInfoId)
        {
            string sql;
            MySqlCommand cmd;

            int stringId;
            stringId = Convert.ToInt32(GetOneParameter(orgInfoId, "ORG_INFO_ID", "fd_org_info", "STRID_ORG_INFO_NAME"));

            sql = "DELETE FROM fd_org_info WHERE ORG_INFO_ID = @ORG_INFO_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_INFO_ID", orgInfoId);
            cmd.ExecuteNonQuery();

            try { DeleteString(stringId); }
            catch
            { }
        }
        public void DeleteOrganization(int orgId)
        {
            string sql;
            MySqlCommand cmd;
            sql = "UPDATE fd_org SET DATE_DELETE = @DATE WHERE ORG_ID=@ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ID", orgId);
            cmd.Parameters.AddWithValue("@DATE", DateTime.Now);
            cmd.ExecuteNonQuery();

            sql = "SELECT STRID_ORG_NAME FROM fd_org WHERE ORG_ID=@ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ID", orgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            int strId=-1;
            while (sdr.Read())
            {
                strId=sdr.GetInt32(0);
            }
            sdr.Close();

            sql = "SELECT STRING_RU FROM fd_string WHERE STRING_ID=@ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ID", strId);
            sdr = cmd.ExecuteReader();
            string value = "";
            while (sdr.Read())
            {
                value = sdr.GetString(0);
            }
            sdr.Close();

            sql = "UPDATE fd_string SET STRING_RU = @STR WHERE STRING_ID=@ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STR", "###" + value);
            cmd.Parameters.AddWithValue("@ID", strId);
            cmd.ExecuteNonQuery();

            sql = "UPDATE fn_card SET CARD_HOLDER_NAME = @STR2 WHERE CARD_HOLDER_NAME=@STR1";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STR1", value + "ORG");
            cmd.Parameters.AddWithValue("@STR2", "###" + value + "ORG");
            cmd.ExecuteNonQuery();
        }
        public int AddNewOrgInfo(string Name)
        {
            int stringId = AddOrGetString(Name);

            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_org_info", "ORG_INFO_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate ORG_INFO_ID"));

            string sql = "INSERT INTO fd_org_info "
                + "(ORG_INFO_ID, STRID_ORG_INFO_NAME)"
                + "VALUES (@ORG_INFO_ID, @STRID_ORG_INFO_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_INFO_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_ORG_INFO_NAME", stringId);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        #endregion
        //---------------------------------COUNTRY AND REGION----------
        public void AddCountryAndRegion(int countryNameId, int countryABBRId, byte[] countryFlagPic, List<KeyValuePair<int, int>> regionNames_short_long)
        {
            int generatedId;

            generatedId = generateId("fd_country", "COUNTRY_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate COUNTRY_ID"));

            MySqlCommand cmd = new MySqlCommand();
            string sql = "INSERT INTO fd_country "
                + "(COUNTRY_ID, STRID_COUNTRY_NAME, STRID_COUNTRY_ABBR, COUNTRY_FLAG)"
                + "VALUES ( @COUNTRY_ID, @STRID_COUNTRY_NAME, @STRID_COUNTRY_ABBR, @COUNTRY_FLAG)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@COUNTRY_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_COUNTRY_NAME", countryNameId);
            cmd.Parameters.AddWithValue("@STRID_COUNTRY_ABBR", countryABBRId);
            cmd.Parameters.AddWithValue("@COUNTRY_FLAG", countryFlagPic);
            cmd.ExecuteNonQuery();

            int regionGeneratedId = -1;
            foreach (KeyValuePair<int, int> region in regionNames_short_long)
            {
                regionGeneratedId = generateId("fd_region", "REGION_ID");
                if (regionGeneratedId == -1)
                    throw (new Exception("Can't generate REGION_ID"));

                cmd = new MySqlCommand();
                sql = "INSERT INTO fd_region "
                    + "(REGION_ID, COUNTRY_ID, STRID_REGION_NAME, STRID_REGION_SHORT_NAME)"
                    + "VALUES ( @REGION_ID, @COUNTRY_ID, @STRID_REGION_NAME, @STRID_REGION_SHORT_NAME)";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@REGION_ID", regionGeneratedId);
                cmd.Parameters.AddWithValue("@COUNTRY_ID", generatedId);
                cmd.Parameters.AddWithValue("@STRID_REGION_NAME", region.Key);
                cmd.Parameters.AddWithValue("@STRID_REGION_SHORT_NAME", region.Value);
                cmd.ExecuteNonQuery();
            }
        }
        public int GetRegionLongNameId(int regionId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(regionId, "REGION_ID", "fd_region", "STRID_REGION_NAME"));
            return returnValue;
        }
        public int GetRegionShortNameId(int regionId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(regionId, "REGION_ID", "fd_region", "STRID_REGION_SHORT_NAME"));
            return returnValue;
        }
        public int GetCountryId_byRegionId(int regionId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(regionId, "REGION_ID", "fd_region", "COUNTRY_ID"));
            return returnValue;
        }
        public List<int> GetAllRegions_byCountryId(int countryId)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT REGION_ID FROM fd_region ORDER BY REGION_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public int GetCountryABBRId(int countryId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(countryId, "COUNTRY_ID", "fd_country", "STRID_COUNTRY_ABBR"));
            return returnValue;
        }
        public int GetCountryNameId(int countryId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(countryId, "COUNTRY_ID", "fd_country", "STRID_COUNTRY_NAME"));
            return returnValue;
        }
        public List<KeyValuePair<string, int>> GetAllCountry(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> hash;
            List<int> allCountryIds = new List<int>();
            string sql = "SELECT COUNTRY_ID FROM fd_country";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                allCountryIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            sql = "SELECT STRID_COUNTRY_NAME FROM fd_country WHERE COUNTRY_ID=@COUNTRY_ID";
            int stringId;
            foreach (int id in allCountryIds)
            {
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@COUNTRY_ID", id);
                stringId = Convert.ToInt32(cmd.ExecuteScalar());
                hash = new KeyValuePair<string, int>(GetString(stringId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public List<KeyValuePair<string, int>> GetAllRegions(int countryId, string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> hash;
            List<int> regionIds = new List<int>();
            string sql = "SELECT REGION_ID FROM fd_region WHERE COUNTRY_ID=@COUNTRY_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@COUNTRY_ID", countryId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                regionIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            sql = "SELECT STRID_REGION_NAME FROM fd_region WHERE REGION_ID=@REGION_ID";
            int stringId;
            foreach (int id in regionIds)
            {
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@REGION_ID", id);
                stringId = Convert.ToInt32(cmd.ExecuteScalar());
                hash = new KeyValuePair<string, int>(GetString(stringId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public byte[] GetCountryFlag(int countryId)
        {
            byte[] returnValue = null;
            string sql = "SELECT COUNTRY_FLAG FROM fd_country WHERE COUNTRY_ID = @COUNTRY_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@COUNTRY_ID", countryId);

            MySqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();

            returnValue = new Byte[(sdr.GetBytes(0, 0, null, 0, int.MaxValue))];
            sdr.GetBytes(0, 0, returnValue, 0, returnValue.Length);
            sdr.Close();
            return returnValue;
        }
        //---------------------------------FD_VEHICLE and other vehicle Tables--------
        #region "VEHICLES"
        public int AddNewVehicle(string GosNomer, string Marka, string VIN, int vehicleTypeId, int deviceId, int cardId, DateTime BLOCKED, int priority, string Language)
        {
            int generatedId;
            int markaStrId;
            generatedId = generateId("fd_vehicle", "VEHICLE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate VEHICLE_ID"));

            markaStrId = AddOrGetString(Marka, Language);

            MySqlCommand cmd = new MySqlCommand();
            string sql = "INSERT INTO fd_vehicle "
                + "(VEHICLE_ID, VEHICLE_TYPE_ID, DEVICE_ID, VIN, GOS_NUM, STRID_MARKA, PRIORITY, DATE_BLOCKED, CARD_ID)"
                + "VALUES ( @VEHICLE_ID, @VEHICLE_TYPE_ID, @DEVICE_ID, @VIN, @GOS_NUM, @STRID_MARKA, @PRIORITY, @DATE_BLOCKED, @CARD_ID)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_ID", generatedId);
            cmd.Parameters.AddWithValue("@VEHICLE_TYPE_ID", vehicleTypeId);
            cmd.Parameters.AddWithValue("@DEVICE_ID", deviceId);
            cmd.Parameters.AddWithValue("@VIN", VIN);
            cmd.Parameters.AddWithValue("@GOS_NUM", GosNomer);
            cmd.Parameters.AddWithValue("@STRID_MARKA", markaStrId);
            cmd.Parameters.AddWithValue("@PRIORITY", priority);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            if (BLOCKED == null)
                cmd.Parameters.AddWithValue("@DATE_BLOCKED", BLOCKED);
            else
                cmd.Parameters.AddWithValue("@DATE_BLOCKED", BLOCKED.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();

            return generatedId;
        }
        public void EditVehicle(int VehicleId, string GosNomer, string Marka, string VIN, int vehicleTypeId, int deviceId, DateTime BLOCKED, int priority, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql;
            int newMarkaId = EditAnySTRIDValue(Marka, "STRID_MARKA", Language, "fd_vehicle", "VEHICLE_ID", VehicleId);

            cmd = new MySqlCommand();
            sql = "UPDATE fd_vehicle SET VEHICLE_TYPE_ID=@VEHICLE_TYPE_ID,"
                + "DEVICE_ID=@DEVICE_ID, VIN=@VIN, GOS_NUM=@GOS_NUM, STRID_MARKA=@STRID_MARKA, STRID_MARKA=@STRID_MARKA, PRIORITY=@PRIORITY, DATE_BLOCKED=@DATE_BLOCKED "
                + "WHERE VEHICLE_ID=@VEHICLE_ID";//, ORG_ID=@ORG_ID  добавить вконец, когда будут организации.
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_ID", VehicleId);
            cmd.Parameters.AddWithValue("@VEHICLE_TYPE_ID", vehicleTypeId);
            cmd.Parameters.AddWithValue("@DEVICE_ID", deviceId);
            cmd.Parameters.AddWithValue("@VIN", VIN);
            cmd.Parameters.AddWithValue("@GOS_NUM", GosNomer);
            cmd.Parameters.AddWithValue("@PRIORITY", priority);
            cmd.Parameters.AddWithValue("@STRID_MARKA", newMarkaId);
            cmd.Parameters.AddWithValue("@DATE_BLOCKED", BLOCKED.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
        }
        public int GetVehicle_byCardId(int CardId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(CardId, "CARD_ID", "fd_vehicle", "VEHICLE_ID"));
            return returnValue;
        }
        public int GetVehicleCardId(int vehId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(vehId, "VEHICLE_ID", "fd_vehicle", "CARD_ID"));
            return returnValue;
        }
        public List<int> GetAllVehicleDataBlocks_byVehId(int vehId)
        {
            List<int> gettedId = new List<int>();
            string sql = "SELECT DATA_BLOCK_ID FROM fn_data_block WHERE VEHICLE_ID=@VEHICLE_ID ORDER BY DATA_BLOCK_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_ID", vehId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
                gettedId.Add(sdr.GetInt32(0));
            sdr.Close();
            return gettedId;
        }
        public string GetVehicleGOSNUM(int vehId)
        {
            string name = "";
            name = Convert.ToString(GetOneParameter(vehId, "VEHICLE_ID", "fd_vehicle", "GOS_NUM"));
            if (name == "")
                name = "Нет значения!";
            return name;
        }
        public string GetVehicleVin(int vehId)
        {
            string name = "";
            name = Convert.ToString(GetOneParameter(vehId, "VEHICLE_ID", "fd_vehicle", "VIN"));
            if (name == "")
                name = "Нет значения!";
            return name;
        }
        public int GetVehicleMARKAStrId(int vehId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(vehId, "VEHICLE_ID", "fd_vehicle", "STRID_MARKA"));
            return returnValue;
        }
        public int GetVehicleTypeId(int vehId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(vehId, "VEHICLE_ID", "fd_vehicle", "VEHICLE_TYPE_ID"));
            return returnValue;
        }
        public int GetVehicleDeviceId(int vehId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(vehId, "VEHICLE_ID", "fd_vehicle", "DEVICE_ID"));
            return returnValue;
        }
        public void SetVehicleTypeId(int vehId, int vehTypeId)
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql = "UPDATE fd_vehicle SET VEHICLE_TYPE_ID=@VEHICLE_TYPE_ID WHERE VEHICLE_ID=@VEHICLE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_TYPE_ID", vehTypeId);
            cmd.Parameters.AddWithValue("@VEHICLE_ID", vehId);
            cmd.ExecuteNonQuery();
        }
        public int GetVehiclePriority(int vehId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(vehId, "VEHICLE_ID", "fd_vehicle", "PRIORITY"));
            return returnValue;
        }
        public string GetVehicleDateBlocked(int vehId)
        {
            string returnValue = Convert.ToString(GetOneParameter(vehId, "VEHICLE_ID", "fd_vehicle", "DATE_BLOCKED"));
            return returnValue;
        }
        public int GetVehTypeStrId(int VehTypeId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(VehTypeId, "VEHICLE_TYPE_ID", "fd_vehicle_type", "STRID_VEHICLE_TYPE_NAME"));
            return returnValue;
        }
        public int GetVehTypeFuelType(int VehTypeId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(VehTypeId, "VEHICLE_TYPE_ID", "fd_vehicle_type", "FUEL_TYPE_ID"));
            return returnValue;
        }
        public List<KeyValuePair<string, int>> GetAllVehTypes(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> hash;
            List<int> allIds = new List<int>();
            string sql = "SELECT VEHICLE_TYPE_ID FROM fd_vehicle_type";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                allIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            int stringId;
            foreach (int id in allIds)
            {
                stringId = Convert.ToInt32(GetOneParameter(id, "VEHICLE_TYPE_ID", "fd_vehicle_type", "STRID_VEHICLE_TYPE_NAME"));
                hash = new KeyValuePair<string, int>(GetString(stringId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public List<KeyValuePair<string, int>> GetAllVehFuelTypes(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> hash;
            List<int> allIds = new List<int>();
            string sql = "SELECT FUEL_TYPE_ID FROM fd_fuel_type";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                allIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            int stringId;
            foreach (int id in allIds)
            {
                stringId = Convert.ToInt32(GetOneParameter(id, "FUEL_TYPE_ID", "fd_fuel_type", "STRID_FUEL_TYPE_NAME"));
                hash = new KeyValuePair<string, int>(GetString(stringId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public int GetVehFuelTypeStrId(int FuelTypeId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(FuelTypeId, "FUEL_TYPE_ID", "fd_fuel_type", "STRID_FUEL_TYPE_NAME"));
            return returnValue;
        }
        public void DeleteFuelType(int fuelTypeId)
        {
            string sql;
            MySqlCommand cmd;

            int stringId;
            stringId = Convert.ToInt32(GetOneParameter(fuelTypeId, "FUEL_TYPE_ID", "fd_fuel_type", "STRID_FUEL_TYPE_NAME"));

            sql = "DELETE FROM fd_fuel_type WHERE FUEL_TYPE_ID = @FUEL_TYPE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@FUEL_TYPE_ID", fuelTypeId);
            try { cmd.ExecuteNonQuery(); }
            catch
            { throw new Exception("Can't delete Fuel Type, because it is used in some Vehicle Types"); }

            try { DeleteString(stringId); }
            catch
            { }
        }
        public int AddNewFuelType(string Name)
        {
            int stringId = AddOrGetString(Name);

            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_fuel_type", "FUEL_TYPE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate FUEL_TYPE_ID"));

            string sql = "INSERT INTO fd_fuel_type "
                + "(FUEL_TYPE_ID, STRID_FUEL_TYPE_NAME)"
                + "VALUES (@FUEL_TYPE_ID, @STRID_FUEL_TYPE_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@FUEL_TYPE_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_FUEL_TYPE_NAME", stringId);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public int GetFuelTypeId_byName(string Name, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue = -1;
            int gettedStringVal = GetStringId(Name, Language);
            if (gettedStringVal > 0)
            {
                returnValue = Convert.ToInt32(GetOneParameter(gettedStringVal, "STRID_FUEL_TYPE_NAME", "fd_fuel_type", "FUEL_TYPE_ID"));
            }
            return returnValue;
        }
        public int AddNewVehicleType(string Name, int FuelTypeId)
        {
            int stringId = AddOrGetString(Name);

            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_vehicle_type", "VEHICLE_TYPE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate VEHICLE_TYPE_ID"));

            string sql = "INSERT INTO fd_vehicle_type "
                + "(VEHICLE_TYPE_ID, STRID_VEHICLE_TYPE_NAME, FUEL_TYPE_ID)"
                + "VALUES (@VEHICLE_TYPE_ID, @STRID_VEHICLE_TYPE_NAME, @FUEL_TYPE_ID)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_TYPE_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_VEHICLE_TYPE_NAME", stringId);
            cmd.Parameters.AddWithValue("@FUEL_TYPE_ID", FuelTypeId);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public void EditNewVehicleType(int VehTypeId, string Name, int FuelTypeId, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql = "UPDATE fd_vehicle_type SET FUEL_TYPE_ID=@FUEL_TYPE_ID WHERE VEHICLE_TYPE_ID=@VEHICLE_TYPE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@FUEL_TYPE_ID", FuelTypeId);
            cmd.Parameters.AddWithValue("@VEHICLE_TYPE_ID", VehTypeId);
            cmd.ExecuteNonQuery();

            //   int stringId = GetVehTypeStrId(VehTypeId);
            EditAnySTRIDValue(Name, "STRID_VEHICLE_TYPE_NAME", Language, "fd_vehicle_type", "VEHICLE_TYPE_ID", VehTypeId);
        }
        public int GetVehicleTypeId_byName(string Name, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue = -1;
            int gettedStringVal = GetStringId(Name, Language);
            if (gettedStringVal > 0)
            {
                returnValue = Convert.ToInt32(GetOneParameter(gettedStringVal, "STRID_VEHICLE_TYPE_NAME", "fd_vehicle_type", "VEHICLE_TYPE_ID"));
            }
            return returnValue;
        }
        public void DeleteVehicleType(int VehicleTypeId)
        {
            string sql;
            MySqlCommand cmd;

            int stringId;
            stringId = Convert.ToInt32(GetOneParameter(VehicleTypeId, "VEHICLE_TYPE_ID", "fd_vehicle_type", "STRID_VEHICLE_TYPE_NAME"));

            sql = "DELETE FROM fd_vehicle_type WHERE VEHICLE_TYPE_ID = @VEHICLE_TYPE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_TYPE_ID", VehicleTypeId);
            try { cmd.ExecuteNonQuery(); }
            catch
            { throw new Exception("Can't delete Vehicle Type, because it is used in some Vehicles"); }

            try { DeleteString(stringId); }
            catch
            { }
        }
        public int GetVehicleId_byVinRegNumbers(string vin, string regNumber)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue = -1;
            string sql = "SELECT VEHICLE_ID FROM fd_vehicle WHERE VIN=@VIN AND GOS_NUM=@GOS_NUM";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VIN", vin);
            cmd.Parameters.AddWithValue("@GOS_NUM", regNumber);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        //criteria
        public int AddNewCriteria(int MeasureId, string Name, string Note, int minValue, int maxValue)
        {
            int NameId = AddOrGetString(Name);
            int NoteId = AddOrGetString(Note);

            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_key", "KEY_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate KEY_ID"));

            string sql = "INSERT INTO fd_key "
                + "(KEY_ID, MEASURE_ID, STRID_KEY_NAME, STRID_KEY_NOTE, KEY_VALUE_MIN, KEY_VALUE_MAX)"
                + "VALUES (@KEY_ID, @MEASURE_ID, @STRID_KEY_NAME, @STRID_KEY_NOTE, @KEY_VALUE_MIN, @KEY_VALUE_MAX)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@KEY_ID", generatedId);
            cmd.Parameters.AddWithValue("@MEASURE_ID", MeasureId);
            cmd.Parameters.AddWithValue("@STRID_KEY_NAME", NameId);
            cmd.Parameters.AddWithValue("@STRID_KEY_NOTE", NoteId);
            cmd.Parameters.AddWithValue("@KEY_VALUE_MIN", minValue);
            cmd.Parameters.AddWithValue("@KEY_VALUE_MAX", maxValue);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public int GetCriteriaId_byNameAndMeasureId(string Name, int MeasureId, string Language)
        {
            int stringId = GetStringId(Name, Language);
            int returnValue = -1;
            MySqlCommand cmd = new MySqlCommand();

            if (stringId > 0)
            {
                string sql = "SELECT KEY_ID FROM fd_key WHERE STRID_KEY_NAME=@STRID_KEY_NAME AND MEASURE_ID=@MEASURE_ID";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@STRID_KEY_NAME", stringId);
                cmd.Parameters.AddWithValue("@MEASURE_ID", MeasureId);
                returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return returnValue;
        }
        public int GetCriteriaId_byName(string Name, string Language)// Возможно несколько вариантов(сделать позже, если будет ошибка возвращение массивом List<int>
        {
            int stringId = GetStringId(Name, Language);
            int returnValue = -1;
            MySqlCommand cmd = new MySqlCommand();

            if (stringId > 0)
            {
                string sql = "SELECT KEY_ID FROM fd_key WHERE STRID_KEY_NAME=@STRID_KEY_NAME";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@STRID_KEY_NAME", stringId);
                returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            }
            return returnValue;
        }
        public void EditCriteria(int keyId, int MeasureId, string Name, string Note, int minValue, int maxValue, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql = "UPDATE fd_key SET MEASURE_ID=@MEASURE_ID, KEY_VALUE_MIN=@KEY_VALUE_MIN, KEY_VALUE_MAX=@KEY_VALUE_MAX  WHERE KEY_ID=@KEY_ID";//, ORG_ID=@ORG_ID  добавить вконец, когда будут организации.
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@KEY_ID", keyId);
            cmd.Parameters.AddWithValue("@MEASURE_ID", MeasureId);
            cmd.Parameters.AddWithValue("@KEY_VALUE_MIN", minValue);
            cmd.Parameters.AddWithValue("@KEY_VALUE_MAX", maxValue);
            cmd.ExecuteNonQuery();

            EditAnySTRIDValue(Name, "STRID_KEY_NAME", Language, "fd_key", "KEY_ID", keyId);
            EditAnySTRIDValue(Note, "STRID_KEY_NOTE", Language, "fd_key", "KEY_ID", keyId);
        }
        public int GetCriteriaNameId(int keyId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(keyId, "KEY_ID", "fd_key", "STRID_KEY_NAME"));
            return returnValue;
        }
        public int GetCriteriaNoteId(int keyId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(keyId, "KEY_ID", "fd_key", "STRID_KEY_NOTE"));
            return returnValue;
        }
        public int GetCriteriaMinValue(int keyId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(keyId, "KEY_ID", "fd_key", "KEY_VALUE_MIN"));
            return returnValue;
        }
        public int GetCriteriaMaxValue(int keyId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(keyId, "KEY_ID", "fd_key", "KEY_VALUE_MAX"));
            return returnValue;
        }
        public int GetCriteriaMeasureId(int keyId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(keyId, "KEY_ID", "fd_key", "MEASURE_ID"));
            return returnValue;
        }
        public List<KeyValuePair<string, int>> GetAllCriteria_Name_n_Id(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> hash;
            List<int> allIds = new List<int>();
            string sql = "SELECT KEY_ID FROM fd_key";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                allIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            sql = "SELECT STRID_KEY_NAME FROM fd_key WHERE KEY_ID=@KEY_ID";
            int stringId;
            foreach (int id in allIds)
            {
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@KEY_ID", id);
                stringId = Convert.ToInt32(cmd.ExecuteScalar());
                hash = new KeyValuePair<string, int>(GetString(stringId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public void DeleteCriteria(int keyId)
        {
            string sql;
            MySqlCommand cmd;

            int nameId = GetCriteriaNameId(keyId);
            int noteId = GetCriteriaNoteId(keyId);

            sql = "DELETE FROM fd_key WHERE KEY_ID = @KEY_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@KEY_ID", keyId);
            try { cmd.ExecuteNonQuery(); }
            catch
            { throw new Exception("Can't delete criterion, because it is used in some other Tables"); }

            try { DeleteString(nameId); }
            catch { }
            try { DeleteString(noteId); }
            catch { }
        }
        //measures
        public int GetMeasureShortNameId(int MeasureId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(MeasureId, "MEASURE_ID", "fd_measure", "STRID_MEASURE_NAME"));
            return returnValue;
        }
        public int GetMeasureFullNameId(int MeasureId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(MeasureId, "MEASURE_ID", "fd_measure", "STRID_MEASURE_FULL_NAME"));
            return returnValue;
        }
        public List<int> GetAllMeasuresIds()
        {
            List<int> gettedId = new List<int>();
            string sql = "SELECT MEASURE_ID FROM fd_measure";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
                gettedId.Add(sdr.GetInt32(0));
            sdr.Close();
            return gettedId;
        }
        public int GetMeasureId_byFullName(string fullName, string Language)
        {
            int stringId = GetStringId(fullName, Language);
            int returnValue = -1;
            MySqlCommand cmd = new MySqlCommand();
            if (stringId > 0)
                returnValue = Convert.ToInt32(GetOneParameter(stringId, "MEASURE_ID", "fd_measure", "STRID_MEASURE_FULL_NAME"));
            return returnValue;
        }
        public int AddNewMeasure(string shortName, string fullName)
        {
            int shortNameId = AddOrGetString(shortName);
            int fullNameId = AddOrGetString(fullName);

            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_measure", "MEASURE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate MEASURE_ID"));

            string sql = "INSERT INTO fd_measure "
                + "(MEASURE_ID, STRID_MEASURE_NAME, STRID_MEASURE_FULL_NAME)"
                + "VALUES (@MEASURE_ID, @STRID_MEASURE_NAME, @STRID_MEASURE_FULL_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@MEASURE_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_MEASURE_NAME", shortNameId);
            cmd.Parameters.AddWithValue("@STRID_MEASURE_FULL_NAME", fullNameId);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public void DeleteMeasure(int measureId)
        {
            string sql;
            MySqlCommand cmd;

            int shortNameId = GetMeasureShortNameId(measureId);
            int fullNameId = GetMeasureFullNameId(measureId);

            sql = "DELETE FROM fd_measure WHERE MEASURE_ID = @MEASURE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@MEASURE_ID", measureId);
            try { cmd.ExecuteNonQuery(); }
            catch
            { throw new Exception("Can't delete measure, because it is used in some other Tables"); }

            try { DeleteString(shortNameId); }
            catch { }
            try { DeleteString(fullNameId); }
            catch { }
        }
        public void EditMeasure(int MeasureId, string shortName, string fullName, string Language)
        {
            EditAnySTRIDValue(shortName, "STRID_MEASURE_NAME", Language, "fd_measure", "MEASURE_ID", MeasureId);
            EditAnySTRIDValue(fullName, "STRID_MEASURE_FULL_NAME", Language, "fd_measure", "MEASURE_ID", MeasureId);
        }
        //FD_VEHICLE_KEY
        public List<int> GetAllVehicleKeyIDS(int vehicleId)
        {
            List<int> gettedId = new List<int>();
            string sql = "SELECT VEHICLE_KEY_ID FROM fd_vehicle_key WHERE VEHICLE_ID=@VEHICLE_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_ID", vehicleId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
                gettedId.Add(sdr.GetInt32(0));
            sdr.Close();
            return gettedId;
        }
        public int GetVehicleKey_KeyId(int vehicleKeyId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(vehicleKeyId, "VEHICLE_KEY_ID", "fd_vehicle_key", "KEY_ID"));
            return returnValue;
        }
        public int GetVehicleKey_MinVal(int vehicleKeyId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(vehicleKeyId, "VEHICLE_KEY_ID", "fd_vehicle_key", "KEY_VALUE_MIN"));
            return returnValue;
        }
        public int GetVehicleKey_MaxVal(int vehicleKeyId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(vehicleKeyId, "VEHICLE_KEY_ID", "fd_vehicle_key", "KEY_VALUE_MAX"));
            return returnValue;
        }
        public string GetVehicleKey_BDate(int vehicleKeyId)
        {
            string returnValue = Convert.ToString(GetOneParameter(vehicleKeyId, "VEHICLE_KEY_ID", "fd_vehicle_key", "BDATE"));
            return returnValue;
        }
        public string GetVehicleKey_EDate(int vehicleKeyId)
        {
            string returnValue = Convert.ToString(GetOneParameter(vehicleKeyId, "VEHICLE_KEY_ID", "fd_vehicle_key", "EDATE"));
            return returnValue;
        }
        public int GetVehicleKey_NoteId(int vehicleKeyId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(vehicleKeyId, "VEHICLE_KEY_ID", "fd_vehicle_key", "STRID_NOTE"));
            return returnValue;
        }
        public int AddVehicleKey(int vehicleId, int KeyId, int minVal, int maxVal, DateTime BDate, DateTime EDate, string note, string Language)
        {
            int NoteId = AddOrGetString(note, Language);

            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_vehicle_key", "VEHICLE_KEY_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate VEHICLE_KEY_ID"));

            string sql = "INSERT INTO fd_vehicle_key "
                + "(VEHICLE_KEY_ID, VEHICLE_ID, KEY_ID, KEY_VALUE_MIN, KEY_VALUE_MAX, BDATE, EDATE, STRID_NOTE)"
                + "VALUES (@VEHICLE_KEY_ID, @VEHICLE_ID, @KEY_ID, @KEY_VALUE_MIN, @KEY_VALUE_MAX, @BDATE, @EDATE, @STRID_NOTE)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_KEY_ID", generatedId);
            cmd.Parameters.AddWithValue("@VEHICLE_ID", vehicleId);
            cmd.Parameters.AddWithValue("@KEY_ID", KeyId);
            cmd.Parameters.AddWithValue("@KEY_VALUE_MIN", minVal);
            cmd.Parameters.AddWithValue("@KEY_VALUE_MAX", maxVal);
            cmd.Parameters.AddWithValue("@BDATE", BDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@EDATE", EDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@STRID_NOTE", NoteId);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public void SetVehicleKey(int vehicleKeyId, int minVal, int maxVal, DateTime BDate, DateTime EDate, string note, string Language)
        {
            if (note != "")
            {
                EditAnySTRIDValue("note", "STRID_NOTE", Language, "fd_vehicle_key", "VEHICLE_KEY_ID", vehicleKeyId);
            }

            MySqlCommand cmd = new MySqlCommand();
            string sql = "UPDATE fd_vehicle_key SET KEY_VALUE_MIN=@KEY_VALUE_MIN, KEY_VALUE_MAX=@KEY_VALUE_MAX, BDATE=@BDATE, EDATE=@EDATE WHERE VEHICLE_KEY_ID=@VEHICLE_KEY_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@KEY_VALUE_MIN", minVal);
            cmd.Parameters.AddWithValue("@KEY_VALUE_MAX", maxVal);
            cmd.Parameters.AddWithValue("@BDATE", BDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@EDATE", EDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@VEHICLE_KEY_ID", vehicleKeyId);
            cmd.ExecuteNonQuery();
        }
        public int GetVehicleKeyId_ByVehIdAndKeyId(int VehId, int KeyNameId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue = -1;
            string sql = "SELECT VEHICLE_KEY_ID FROM fd_vehicle_key WHERE VEHICLE_ID=@VEHICLE_ID AND KEY_ID=(SELECT KEY_ID FROM fd_key WHERE STRID_KEY_NAME=@STRID_KEY_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_ID", VehId);
            cmd.Parameters.AddWithValue("@STRID_KEY_NAME", KeyNameId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        //FD_VEHICLE_INFO_SET and FD_VEHICLE_INFO
        public int GetVehicleInfoValueStrId(int vehicleId, int vehicleInfoId)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT STRID_VEHICLE_INFO_VALUE FROM fd_vehicle_info_set WHERE VEHICLE_ID=@VEHICLE_ID AND VEHICLE_INFO_ID=@VEHICLE_INFO_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_ID", vehicleId);
            cmd.Parameters.AddWithValue("@VEHICLE_INFO_ID", vehicleInfoId);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public int GetVehicleInfoName(int InfoNameId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(InfoNameId, "STRID_VEHICLE_INFO_NAME", "fd_vehicle_info", "VEHICLE_INFO_ID"));
            return returnValue;
        }
        public List<KeyValuePair<string, int>> GetAllVehicleInfoNames(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            List<int> AllVehicleInfoNames = new List<int>();

            string sql = "SELECT VEHICLE_INFO_ID FROM fd_vehicle_info ORDER BY VEHICLE_INFO_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                AllVehicleInfoNames.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            KeyValuePair<string, int> hash;
            int vehicleInfoNameId;
            foreach (int id in AllVehicleInfoNames)
            {
                vehicleInfoNameId = GetUserInfoName(id);
                hash = new KeyValuePair<string, int>(GetString(vehicleInfoNameId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public int AddVehicleInfoName(string InfoName, string Language)
        {
            int stringId = AddOrGetString(InfoName);
            // TranslateString(InfoName, Language, stringId);

            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_vehicle_info", "VEHICLE_INFO_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate VEHICLE_INFO_ID"));

            string sql = "INSERT INTO fd_vehicle_info "
                + "(VEHICLE_INFO_ID, STRID_VEHICLE_INFO_NAME)"
                + "VALUES (@VEHICLE_INFO_ID, @STRID_VEHICLE_INFO_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_INFO_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_VEHICLE_INFO_NAME", stringId);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public void AddVehicleInfoValue(int vehicleId, int vehicleInfoId, string value, string Language)
        {
            int stringId = AddOrGetString(value, Language);
            MySqlCommand cmd = new MySqlCommand();
            string sql = "INSERT INTO fd_vehicle_info_set "
                + "(VEHICLE_ID, VEHICLE_INFO_ID, STRID_VEHICLE_INFO_VALUE)"
                + "VALUES (@VEHICLE_ID, @VEHICLE_INFO_ID, @STRID_VEHICLE_INFO_VALUE)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_ID", vehicleId);
            cmd.Parameters.AddWithValue("@VEHICLE_INFO_ID", vehicleInfoId);
            cmd.Parameters.AddWithValue("@STRID_VEHICLE_INFO_VALUE", stringId);
            cmd.ExecuteNonQuery();
        }
        public void EditVehicleInfo(int vehicleId, int vehicleInfoId, string newValue, string Language)
        {
            EditAnySTRIDValue(newValue, "STRID_VEHICLE_INFO_VALUE", Language, "fd_vehicle_info_set", "VEHICLE_INFO_ID", vehicleInfoId, "VEHICLE_ID", vehicleId);
        }
        #endregion
        //-------------------------------Device tables------------
        #region "DeviceTables"
        //FD_DEVICE_TYPE
        public List<KeyValuePair<string, int>> GetAllDeviceTypes(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> hash;
            List<int> allIds = new List<int>();
            string sql = "SELECT DEVICE_TYPE_ID FROM fd_device_type";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                allIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            sql = "SELECT STRID_DEVICE_TYPE_NAME FROM fd_device_type WHERE DEVICE_TYPE_ID=@DEVICE_TYPE_ID";
            int stringId;
            foreach (int id in allIds)
            {
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@DEVICE_TYPE_ID", id);
                stringId = Convert.ToInt32(cmd.ExecuteScalar());
                hash = new KeyValuePair<string, int>(GetString(stringId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public int GetDeviceTypeStrId(int DeviceTypeId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(DeviceTypeId, "DEVICE_TYPE_ID", "fd_device_type", "STRID_DEVICE_TYPE_NAME"));
            return returnValue;
        }
        public int AddNewDeviceType(string Name)
        {
            int stringId = AddOrGetString(Name);

            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_device_type", "DEVICE_TYPE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate DEVICE_TYPE_ID"));

            string sql = "INSERT INTO fd_device_type "
                + "(DEVICE_TYPE_ID, STRID_DEVICE_TYPE_NAME)"
                + "VALUES (@DEVICE_TYPE_ID, @STRID_DEVICE_TYPE_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DEVICE_TYPE_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_DEVICE_TYPE_NAME", stringId);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public int GetDeviceTypeId_byName(string Name, string Language)
        {
            return 0;
        }
        public void DeleteDeviceType(int deviceTypeId)
        {
            string sql;
            MySqlCommand cmd;

            int strId = GetDeviceTypeStrId(deviceTypeId);

            sql = "DELETE FROM fd_device_type WHERE DEVICE_TYPE_ID = @DEVICE_TYPE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DEVICE_TYPE_ID", deviceTypeId);
            try { cmd.ExecuteNonQuery(); }
            catch
            { throw new Exception("Can't delete device type, because it is used in some other Tables"); }

            try { DeleteString(strId); }
            catch { }
        }
        //FD_DEVICE
        public int GetDeviceType(int deviceId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(deviceId, "DEVICE_ID", "fd_device", "DEVICE_TYPE_ID"));
            return returnValue;
        }
        public int GetDeviceNameId(int deviceId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(deviceId, "DEVICE_ID", "fd_device", "STRID_DEVICE_NAME"));
            return returnValue;
        }
        public string GetDeviceNum(int deviceId)
        {
            string returnValue = Convert.ToString(GetOneParameter(deviceId, "DEVICE_ID", "fd_device", "DEVICE_NUM"));
            return returnValue;
        }
        public string GetDeviceDateProduction(int deviceId)
        {
            string returnValue = Convert.ToString(GetOneParameter(deviceId, "DEVICE_ID", "fd_device", "DATE_PRODUCTION"));
            return returnValue;
        }
        public int GetDeviceFirmwareId(int deviceId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(deviceId, "DEVICE_ID", "fd_device", "DEVICE_FIRMWARE_ID"));
            return returnValue;
        }
        public int GetDevicePhoneNumSim(int deviceId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(deviceId, "DEVICE_ID", "fd_device", "PHONE_NUM_SIM"));
            return returnValue;
        }
        public int AddNewDevice(int deviceTypeId, string deviceName, string deviceNum, DateTime dateProduction, int firmwareId, int phoneNumSim)
        {
            int deviceNameId = AddOrGetString(deviceName);

            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_device", "DEVICE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate DEVICE_ID"));

            string sql = "INSERT INTO fd_device "
                + "(DEVICE_ID, DEVICE_TYPE_ID, STRID_DEVICE_NAME, DEVICE_NUM, DATE_PRODUCTION, DEVICE_FIRMWARE_ID, PHONE_NUM_SIM)"
                + "VALUES (@DEVICE_ID, @DEVICE_TYPE_ID, @STRID_DEVICE_NAME, @DEVICE_NUM, @DATE_PRODUCTION, @DEVICE_FIRMWARE_ID, @PHONE_NUM_SIM)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DEVICE_ID", generatedId);
            cmd.Parameters.AddWithValue("@DEVICE_TYPE_ID", deviceTypeId);
            cmd.Parameters.AddWithValue("@STRID_DEVICE_NAME", deviceNameId);
            cmd.Parameters.AddWithValue("@DEVICE_NUM", deviceNum);
            cmd.Parameters.AddWithValue("@DATE_PRODUCTION", dateProduction.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@DEVICE_FIRMWARE_ID", firmwareId);
            cmd.Parameters.AddWithValue("@PHONE_NUM_SIM", phoneNumSim);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        //FD_DEVICE_FIRMWARE
        public int AddNewDeviceFirmware(string deviceModel, DateTime productionDate, string version, byte[] firmWare)
        {
            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_device_firmware", "DEVICE_FIRMWARE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate DEVICE_FIRMWARE_ID"));

            string sql = "INSERT INTO fd_device_firmware "
                + "(DEVICE_FIRMWARE_ID, DEVICE_MODEL, DATE_PRODUCTION, VERSION, FIRMWARE) "
                + "VALUES (@DEVICE_FIRMWARE_ID, @DEVICE_MODEL, @DATE_PRODUCTION, @VERSION, @FIRMWARE)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@DEVICE_FIRMWARE_ID", generatedId);
            cmd.Parameters.AddWithValue("@DEVICE_MODEL", deviceModel);
            cmd.Parameters.AddWithValue("@DATE_PRODUCTION", productionDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@VERSION", version);
            cmd.Parameters.AddWithValue("@FIRMWARE", firmWare);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public string GetDeviceFirmware_deviceModel(int firmwareId)
        {
            string returnValue = Convert.ToString(GetOneParameter(firmwareId, "DEVICE_FIRMWARE_ID", "fd_device_firmware", "DEVICE_MODEL"));
            return returnValue;
        }
        public string GetDeviceFirmware_dateProduction(int firmwareId)
        {
            string returnValue = Convert.ToString(GetOneParameter(firmwareId, "DEVICE_FIRMWARE_ID", "fd_device_firmware", "DATE_PRODUCTION"));
            return returnValue;
        }
        public string GetDeviceFirmware_version(int firmwareId)
        {
            string returnValue = Convert.ToString(GetOneParameter(firmwareId, "DEVICE_FIRMWARE_ID", "fd_device_firmware", "VERSION"));
            return returnValue;
        }
        #endregion
        //--------------------FD_CARD
        #region "fd_card"
        public int GetCardId(string cardHolderName, string cardNumber, int cardTypeId)
        {
            int cardId = 0;
            try
            {
                string sql = "Select CARD_ID FROM fn_card WHERE CARD_HOLDER_NAME = @CARD_HOLDER_NAME AND CARD_NUMBER=@CARD_NUMBER AND CARD_TYPE_ID=@CARD_TYPE_ID";
                MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@CARD_HOLDER_NAME", cardHolderName);
                cmd.Parameters.AddWithValue("@CARD_NUMBER", cardNumber);
                cmd.Parameters.AddWithValue("@CARD_TYPE_ID", cardTypeId);

                cardId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Нет такой карты! Необходимо добавить карту.");
            }
            return cardId;
        }
        public int CreateNewCard(string cardHolderName, string cardNumber, int cardTypeId, int orgId, string CardNote, int groupID)
        {
            int generatedId = 0;
            MySqlCommand cmd = new MySqlCommand();
            string sql = "";

            if (GetCardId(cardHolderName, cardNumber, cardTypeId) != 0)
                throw new Exception("Эта карта уже существует");

            generatedId = generateId("fn_card", "CARD_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate CARD_ID"));

            sql = "INSERT INTO fn_card "
                + "(CARD_ID, CARD_TYPE_ID, CARD_HOLDER_NAME, CARD_NUMBER, ORG_ID, CARD_NOTE, GROUP_ID)"
                + "VALUES (@CARD_ID, @CARD_TYPE_ID, @CARD_HOLDER_NAME, @CARD_NUMBER, @ORG_ID, @CARD_NOTE, @GR_ID)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", generatedId);
            cmd.Parameters.AddWithValue("@CARD_TYPE_ID", cardTypeId);
            cmd.Parameters.AddWithValue("@CARD_HOLDER_NAME", cardHolderName);
            cmd.Parameters.AddWithValue("@CARD_NUMBER", cardNumber);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@CARD_NOTE", CardNote);
            cmd.Parameters.AddWithValue("@GR_ID", groupID);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public int CreateNewCard(string cardHolderName, string cardNumber, int cardTypeId, int orgId, string CardNote, int UserId, int groupID)//При создании еще указывается ссылка на пользователя. Используется при создании водителей.
        {
            int generatedId = 0;
            MySqlCommand cmd = new MySqlCommand();
            string sql = "";

            if (GetCardId(cardHolderName, cardNumber, cardTypeId) != 0)
                throw new Exception("Эта карта уже существует");

            generatedId = generateId("fn_card", "CARD_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate CARD_ID"));

            sql = "INSERT INTO fn_card "
                + "(CARD_ID, CARD_TYPE_ID, CARD_HOLDER_NAME, CARD_NUMBER, ORG_ID, CARD_NOTE, USER_ID, GROUP_ID)"
                + "VALUES (@CARD_ID, @CARD_TYPE_ID, @CARD_HOLDER_NAME, @CARD_NUMBER, @ORG_ID, @CARD_NOTE, @USER_ID, @GR_ID)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", generatedId);
            cmd.Parameters.AddWithValue("@CARD_TYPE_ID", cardTypeId);
            cmd.Parameters.AddWithValue("@CARD_HOLDER_NAME", cardHolderName);
            cmd.Parameters.AddWithValue("@CARD_NUMBER", cardNumber);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@CARD_NOTE", CardNote);
            cmd.Parameters.AddWithValue("@USER_ID", UserId);
            cmd.Parameters.AddWithValue("@GR_ID", groupID);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public List<int> GetAllCardIds(int orgId, int cardTypeId)
        {
            List<int> gettedId = new List<int>();
            string sql = "SELECT CARD_ID FROM fn_card WHERE CARD_TYPE_ID=@CARD_TYPE_ID AND ORG_ID=@ORG_ID ORDER BY CARD_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_TYPE_ID", cardTypeId);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
                gettedId.Add(sdr.GetInt32(0));
            sdr.Close();
            return gettedId;
        }
        public List<int> GetAllGroupIds(int orgId)
        {
            List<int> gettedNames = new List<int>();
            string sql = "SELECT GROUP_ID FROM fn_groups WHERE ORG_ID=@ORG_ID ORDER BY CARD_TYPE_ID, GROUP_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                int id = sdr.GetInt32(0);
                if (!gettedNames.Contains(id))
                {
                    gettedNames.Add(id);
                }
            }
            sdr.Close();
            return gettedNames;
        }
        public List<int> GetAllGroupIds(int orgId, int cardTypeId)
        {
            List<int> gettedNames = new List<int>();
            string sql = "SELECT GROUP_ID FROM fn_groups WHERE (CARD_TYPE_ID=@CARD_TYPE_ID OR CARD_TYPE_ID=0) AND ORG_ID=@ORG_ID ORDER BY CARD_TYPE_ID, GROUP_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_TYPE_ID", cardTypeId);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                int id = sdr.GetInt32(0);
                if (!gettedNames.Contains(id))
                {
                    gettedNames.Add(id);
                }
            }
            sdr.Close();
            return gettedNames;
        }
        public string GetGroupNameById(int groupId)
        {
            string sql = "SELECT GROUP_NAME FROM fn_groups WHERE GROUP_ID=@GR_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@GR_ID", groupId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                string s = sdr.GetString(0);
                sdr.Close();
                return s;
            }
            else
            {
                sdr.Close();
                return null;
            }
        }
        public string GetGroupCommentById(int groupId)
        {
            string sql = "SELECT GROUP_COMMENT FROM fn_groups WHERE GROUP_ID=@GR_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@GR_ID", groupId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                string s = sdr.GetString(0);
                sdr.Close();
                return s;
            }
            else
            {
                sdr.Close();
                return null;
            }
        }
        public int GetGroupCardTypeById(int groupId)
        {
            string sql = "SELECT CARD_TYPE_ID FROM fn_groups WHERE GROUP_ID=@GR_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@GR_ID", groupId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                int s = Convert.ToInt32(sdr.GetDecimal(0));
                sdr.Close();
                return s;
            }
            else
            {
                sdr.Close();
                return 0;
            }
        }
        public void DeleteGroup(int orgId, int groupId)
        {
            string sql0 = "SELECT GROUP_ID FROM fn_groups WHERE ORG_ID=@ORG_ID AND CARD_TYPE_ID=0";
            MySqlCommand cmd0 = new MySqlCommand(sql0, sqlConnection);
            cmd0.Parameters.AddWithValue("@ORG_ID", orgId);
            MySqlDataReader sdr0 = cmd0.ExecuteReader();
            sdr0.Read();
            int newGroupId = sdr0.GetInt32(0);
            sdr0.Close();

            string sql = "UPDATE fn_card SET GROUP_ID=@NEW_GROUP_ID WHERE GROUP_ID=@GROUP_ID AND ORG_ID=@ORG_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@NEW_GROUP_ID", newGroupId);
            cmd.Parameters.AddWithValue("@GROUP_ID", groupId);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            sdr.Close();

            string sql1 = "DELETE FROM fn_groups WHERE GROUP_ID=@GROUP_ID";
            MySqlCommand cmd1 = new MySqlCommand(sql1, sqlConnection);
            cmd1.Parameters.AddWithValue("@GROUP_ID", groupId);
            MySqlDataReader sdr1 = cmd1.ExecuteReader();
            sdr1.Close();
        }
        public void UpdateGroup(int groupId, String name, String comment, int cardType)
        {
            string sql = "UPDATE fn_groups SET GROUP_NAME=@GR_NAME, GROUP_COMMENT=@GR_COMM, CARD_TYPE_ID=@C_T_I WHERE GROUP_ID=@GROUP_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@GROUP_ID", groupId);
            cmd.Parameters.AddWithValue("@GR_NAME", name);
            cmd.Parameters.AddWithValue("@GR_COMM", comment);
            cmd.Parameters.AddWithValue("@C_T_I", cardType);
            MySqlDataReader sdr = cmd.ExecuteReader();
            sdr.Close();
        }
        public void CreateGroup(int orgID, String name, String comment, int cardType)
        {
            string sql = "INSERT INTO fn_groups (GROUP_NAME, GROUP_COMMENT, ORG_ID, CARD_TYPE_ID) VALUES (@GR_NAME, @GR_COMM, @ORG_ID, @C_T_I)";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgID);
            cmd.Parameters.AddWithValue("@GR_NAME", name);
            cmd.Parameters.AddWithValue("@GR_COMM", comment);
            cmd.Parameters.AddWithValue("@C_T_I", cardType);
            MySqlDataReader sdr = cmd.ExecuteReader();
            sdr.Close();
        }
        //FOR PRIVATE USE ONLY!
        public void CreateDefaultGroup(int orgID)
        {
            string sql = "INSERT INTO fn_groups (GROUP_ID,GROUP_NAME, GROUP_COMMENT, ORG_ID, CARD_TYPE_ID) VALUES (@GR_ID,@GR_NAME, @GR_COMM, @ORG_ID, @C_T_I)";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@GR_ID", 0);
            cmd.Parameters.AddWithValue("@ORG_ID", orgID);
            cmd.Parameters.AddWithValue("@GR_NAME", "Общая группа");
            cmd.Parameters.AddWithValue("@GR_COMM", "Группа по умолчанию");
            cmd.Parameters.AddWithValue("@C_T_I", 0);
            MySqlDataReader sdr = cmd.ExecuteReader();
            sdr.Close();
        }
        public String GetCardHolderNameByCardId(int cardId)
        {
            String name = "";
            string sql = "SELECT CARD_HOLDER_NAME FROM fn_card WHERE CARD_ID=@CARD_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                name = sdr.GetString(0);
            }
            sdr.Close();
            return name;
        }
        public List<int> GetAllCardIdsByGroupId(int orgId, int cardTypeId, int groupId)
        {
            List<int> gettedIds = new List<int>();
            string sql = "SELECT CARD_ID FROM fn_card WHERE CARD_TYPE_ID=@CARD_TYPE_ID AND ORG_ID=@ORG_ID AND GROUP_ID=@GR_ID ORDER BY CARD_HOLDER_NAME";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_TYPE_ID", cardTypeId);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@GR_ID", groupId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                int d = sdr.GetInt32(0);
                gettedIds.Add(d);
            }
            sdr.Close();
            return gettedIds;
        }
        public string GetCardName(int cardId)
        {
            string name = "";
            string sql = "Select CARD_HOLDER_NAME FROM fn_card WHERE CARD_ID = @CARD_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            name = Convert.ToString(cmd.ExecuteScalar());
            if (name == "")
                throw new Exception("Этого водителя не существует!");
            return name;
        }
        public string GetCardNumber(int cardId)
        {
            string name = "";
            string sql = "Select CARD_NUMBER FROM fn_card WHERE CARD_ID = @CARD_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            name = Convert.ToString(cmd.ExecuteScalar());
            if (name == "")
                throw new Exception("Этого водителя не существует!");
            return name;
        }
        public int GetCardGroupID(int cardId)
        {
            //string name = "";
            string sql = "Select GROUP_ID FROM fn_card WHERE CARD_ID = @CARD_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            return Convert.ToInt32(cmd.ExecuteScalar());
            //return name;
        }
        public void ChangeCardHolderName(string newName, int cardId)
        {
            string sql = "UPDATE fn_card SET CARD_HOLDER_NAME=@CARD_HOLDER_NAME WHERE CARD_ID=@CARD_ID";
            MySqlCommand cmd = new MySqlCommand();
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            cmd.Parameters.AddWithValue("@CARD_HOLDER_NAME", newName);
            cmd.ExecuteNonQuery();
        }
        public void ChangeCardNumber(string newNumber, int cardId)
        {
            string sql = "UPDATE fn_card SET CARD_NUMBER=@CARD_NUMBER WHERE CARD_ID=@CARD_ID";
            MySqlCommand cmd = new MySqlCommand();
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            cmd.Parameters.AddWithValue("@CARD_NUMBER", newNumber);
            cmd.ExecuteNonQuery();
        }
        public void ChangeCardComment(string newComment, int cardId)
        {
            string sql = "UPDATE fn_card SET CARD_NOTE=@CARD_COMMENT WHERE CARD_ID=@CARD_ID";
            MySqlCommand cmd = new MySqlCommand();
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            cmd.Parameters.AddWithValue("@CARD_COMMENT", newComment);
            cmd.ExecuteNonQuery();
        }
        public void ChangeCardGroup(int groupId, int cardId)
        {
            string sql = "UPDATE fn_card SET GROUP_ID=@GROUP_ID WHERE CARD_ID=@CARD_ID";
            MySqlCommand cmd = new MySqlCommand();
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            cmd.Parameters.AddWithValue("@GROUP_ID", groupId);
            cmd.ExecuteNonQuery();
        }
        public void DeleteCard(int cardId)
        {
            string sql = "DELETE FROM fn_card WHERE CARD_ID = @CARD_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            cmd.ExecuteNonQuery();
        }
        public string GetCardNote(int cardId)
        {
            string returnValue = Convert.ToString(GetOneParameter(cardId, "CARD_ID", "fn_card", "CARD_NOTE"));
            return returnValue;
        }
        public void SetCardNote(int cardId, string Note)
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql = "UPDATE fn_card SET CARD_NOTE=@CARD_NOTE WHERE CARD_ID=@CARD_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            cmd.Parameters.AddWithValue("@CARD_NOTE", Note);
            cmd.ExecuteNonQuery();

        }
        public int GetCardUserId(int cardId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(cardId, "CARD_ID", "fn_card", "USER_ID"));
            return returnValue;
        }

        #endregion
        //------------------------------REMIND TABLES----------
        #region "RemindTables"
        public void CreateNewRemind(int orgId, bool remindActive, int userId, int sourceType, int sourceId, int period, DateTime lastDate, int remindType)
        {
            string sql = "INSERT INTO fn_remind (ORG_ID, REMIND_ACTIVE, REMIND_USER, REMIND_SOURCE_TYPE, REMIND_SOURCE, REMIND_PERIOD, REMIND_LAST_DATE, REMIND_TYPE) VALUES (@ORG_ID, @REM_ACTIVE, @REM_USER, @REM_SOURCE_TYPE, @REM_SOURCE, @REM_PERIOD, @REM_LAST_DATE, @REM_TYPE)";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@REM_ACTIVE", remindActive ? 1 : 0);
            cmd.Parameters.AddWithValue("@REM_USER", userId);
            cmd.Parameters.AddWithValue("@REM_SOURCE_TYPE", sourceType);
            cmd.Parameters.AddWithValue("@REM_SOURCE", sourceId);
            cmd.Parameters.AddWithValue("@REM_PERIOD", period);
            cmd.Parameters.AddWithValue("@REM_LAST_DATE", lastDate);
            cmd.Parameters.AddWithValue("@REM_TYPE", remindType);
            cmd.ExecuteNonQuery();
        }
        public List<int> GetAllRemindIds(int orgId)
        {
            List<int> gettedNames = new List<int>();
            string sql = "SELECT REMIND_ID FROM fn_remind WHERE ORG_ID=@ORG_ID ORDER BY REMIND_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                int id = sdr.GetInt32(0);
                if (!gettedNames.Contains(id))
                {
                    gettedNames.Add(id);
                }
            }
            sdr.Close();
            return gettedNames;
        }
        public List<int> GetAllHourRemindIds()
        {
            List<int> gettedNames = new List<int>();
            string sql = "SELECT REMIND_ID FROM fn_remind WHERE REMIND_PERIOD=2 AND REMIND_ACTIVE=1 ORDER BY REMIND_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                int id = sdr.GetInt32(0);
                if (!gettedNames.Contains(id))
                {
                    gettedNames.Add(id);
                }
            }
            sdr.Close();
            return gettedNames;
        }
        public List<int> GetAllDayRemindIds()
        {
            List<int> gettedNames = new List<int>();
            string sql = "SELECT REMIND_ID FROM fn_remind WHERE REMIND_PERIOD=3 AND REMIND_ACTIVE=1 ORDER BY REMIND_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                int id = sdr.GetInt32(0);
                if (!gettedNames.Contains(id))
                {
                    gettedNames.Add(id);
                }
            }
            sdr.Close();
            return gettedNames;
        }
        public List<int> GetAllMonthRemindIds()
        {
            List<int> gettedNames = new List<int>();
            string sql = "SELECT REMIND_ID FROM fn_remind WHERE REMIND_PERIOD=4 AND REMIND_ACTIVE=1 ORDER BY REMIND_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                int id = sdr.GetInt32(0);
                if (!gettedNames.Contains(id))
                {
                    gettedNames.Add(id);
                }
            }
            sdr.Close();
            return gettedNames;
        }
        public void UpdateRemind(int remindId, bool remindActive, int userId, int sourceType, int sourceId, int period, DateTime lastDate, int remindType)
        {
            string sql = "UPDATE fn_remind SET REMIND_ACTIVE=@REM_ACTIVE, REMIND_USER=@REM_USER, REMIND_SOURCE_TYPE=@REM_SOURCE_TYPE, REMIND_SOURCE=@REM_SOURCE, REMIND_PERIOD=@REM_PERIOD, REMIND_LAST_DATE=@REM_LAST_DATE, REMIND_TYPE=@REM_TYPE WHERE REMIND_ID=@REM_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@REM_ID", remindId);
            cmd.Parameters.AddWithValue("@REM_ACTIVE", remindActive ? 1 : 0);
            cmd.Parameters.AddWithValue("@REM_USER", userId);
            cmd.Parameters.AddWithValue("@REM_SOURCE_TYPE", sourceType);
            cmd.Parameters.AddWithValue("@REM_SOURCE", sourceId);
            cmd.Parameters.AddWithValue("@REM_PERIOD", period);
            cmd.Parameters.AddWithValue("@REM_LAST_DATE", lastDate);
            cmd.Parameters.AddWithValue("@REM_TYPE", remindType);
            cmd.ExecuteNonQuery();
        }
        public void UpdateRemind(int remindId, DateTime lastDate)
        {
            string sql = "UPDATE fn_remind SET REMIND_LAST_DATE=@REM_LAST_DATE WHERE REMIND_ID=@REM_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@REM_ID", remindId);
            cmd.Parameters.AddWithValue("@REM_LAST_DATE", lastDate);
            cmd.ExecuteNonQuery();
        }
        public void DeleteRemind(int remindId)
        {
            string sql = "DELETE FROM fn_remind WHERE REMIND_ID = @REMIND_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@REMIND_ID", remindId);
            cmd.ExecuteNonQuery();
        }
        public bool GetRemindActive(int remindId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(remindId, "REMIND_ID", "fn_remind", "REMIND_ACTIVE"));
            return returnValue == 0 ? false : true;
        }
        public int GetRemindUser(int remindId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(remindId, "REMIND_ID", "fn_remind", "REMIND_USER"));
            return returnValue;
        }
        public int GetRemindSourceType(int remindId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(remindId, "REMIND_ID", "fn_remind", "REMIND_SOURCE_TYPE"));
            return returnValue;
        }
        public int GetRemindSource(int remindId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(remindId, "REMIND_ID", "fn_remind", "REMIND_SOURCE"));
            return returnValue;
        }
        public int GetRemindPeriod(int remindId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(remindId, "REMIND_ID", "fn_remind", "REMIND_PERIOD"));
            return returnValue;
        }
        public int GetRemindType(int remindId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(remindId, "REMIND_ID", "fn_remind", "REMIND_TYPE"));
            return returnValue;
        }
        public int GetRemindOrgId(int remindId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(remindId, "REMIND_ID", "fn_remind", "ORG_ID"));
            return returnValue;
        }
        public string GetRemindTypeName(int remindTypeId)
        {
            string returnValue = Convert.ToString(GetOneParameter(remindTypeId, "REMIND_TYPE", "fn_remind_type", "REMIND_TYPE_NAME"));
            return returnValue;
        }
        public string GetRemindPeriodName(int remindPeriodId)
        {
            string returnValue = Convert.ToString(GetOneParameter(remindPeriodId, "REMIND_PERIOD", "fn_remind_period", "REMIND_PERIOD_NAME"));
            return returnValue;
        }
        public DateTime GetRemindLastDate(int remindId)
        {
            DateTime returnValue = Convert.ToDateTime(GetOneParameter(remindId, "REMIND_ID", "fn_remind", "REMIND_LAST_DATE"));
            return returnValue;
        }
        #endregion
        //------------------------------HISTORY TABLES----------
        #region "HistoryTables"
        //FN_HISTORY
        /// <summary>
        /// Добавляет запись в таблицы журнала
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="TABLE_KEYFIELD_VALUE"></param>
        /// <param name="userId"></param>
        /// <param name="actionId"></param>
        /// <param name="actionDate"></param>
        /// <param name="Note"></param>
        /// <returns>Возвращает время действия. если действие происходит несколько раз в секунду, нужно увеличить на одну секунду actionDate</returns>
        public DateTime AddHistoryRecord(int tableId, int TABLE_KEYFIELD_VALUE, int userId, int actionId, DateTime actionDate, string Note)
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql = "INSERT INTO fn_history "
                + "(TABLE_ID, TABLE_KEYFIELD_VALUE, USER_ID, ACTION_ID, ACTION_DATE, NOTE) "
                + "VALUES (@TABLE_ID, @TABLE_KEYFIELD_VALUE, @USER_ID, @ACTION_ID, @ACTION_DATE, @NOTE)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@TABLE_ID", tableId);
            cmd.Parameters.AddWithValue("@TABLE_KEYFIELD_VALUE", TABLE_KEYFIELD_VALUE);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            cmd.Parameters.AddWithValue("@ACTION_ID", actionId);
            cmd.Parameters.AddWithValue("@ACTION_DATE", actionDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@NOTE", Note);
            cmd.ExecuteNonQuery();
            return actionDate;
        }
        public List<List<KeyValuePair<string, string>>> GetHistoryActionIdAndTableId(int UserId, DateTime from, DateTime to, int actionId)
        {
            List<List<KeyValuePair<string, string>>> returnValue = new List<List<KeyValuePair<string, string>>>();
            KeyValuePair<string, string> oneValue;
            List<KeyValuePair<string, string>> oneFullValue;
            string actionString = "";
            if (actionId > 0)
                actionString = " AND ACTION_ID=@ACTION_ID";

            string sql = "SELECT ACTION_ID, TABLE_ID, ACTION_DATE FROM fn_history "
                + "WHERE USER_ID=@USER_ID" + actionString + " AND ACTION_DATE BETWEEN @FROMACTIONDATE AND @TOACTIONDATE ORDER BY ACTION_DATE DESC";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ID", UserId);
            if (actionId > 0)
                cmd.Parameters.AddWithValue("@ACTION_ID", actionId);
            cmd.Parameters.AddWithValue("@FROMACTIONDATE", from.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@TOACTIONDATE", to.ToString("yyyy-MM-dd HH:mm:ss"));
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                oneFullValue = new List<KeyValuePair<string, string>>();
                oneValue = new KeyValuePair<string, string>("ACTION_ID", sdr.GetInt32("ACTION_ID").ToString());
                oneFullValue.Add(oneValue);
                oneValue = new KeyValuePair<string, string>("TABLE_ID", sdr.GetInt32("TABLE_ID").ToString());
                oneFullValue.Add(oneValue);
                oneValue = new KeyValuePair<string, string>("USER_ID", UserId.ToString());
                oneFullValue.Add(oneValue);
                oneValue = new KeyValuePair<string, string>("ACTION_DATE", sdr.GetString("ACTION_DATE"));
                oneFullValue.Add(oneValue);

                returnValue.Add(oneFullValue);
            }
            sdr.Close();
            return returnValue;
        }
        public string GetHistoryDate(int userId, int actionId, int tableId)
        {
            MySqlCommand cmd = new MySqlCommand();
            string returnValue;
            string sql = "SELECT ACTION_DATE FROM fn_history WHERE TABLE_ID=@TABLE_ID AND USER_ID=@USER_ID AND ACTION_ID=@ACTION_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@TABLE_ID", tableId);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            cmd.Parameters.AddWithValue("@ACTION_ID", actionId);
            returnValue = Convert.ToString(cmd.ExecuteScalar());
            return returnValue;
        }
        public string GetHistoryNote(int userId, int actionId, int tableId, DateTime historyDate)
        {
            MySqlCommand cmd = new MySqlCommand();
            string returnValue;
            string sql = "SELECT NOTE FROM fn_history WHERE TABLE_ID=@TABLE_ID AND USER_ID=@USER_ID AND ACTION_ID=@ACTION_ID AND ACTION_DATE=@ACTION_DATE";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@TABLE_ID", tableId);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            cmd.Parameters.AddWithValue("@ACTION_ID", actionId);
            cmd.Parameters.AddWithValue("@ACTION_DATE", historyDate.ToString("yyyy-MM-dd HH:mm:ss"));
            returnValue = Convert.ToString(cmd.ExecuteScalar());
            return returnValue;
        }
        //FD_TABLE
        public int GetTableId(string tableName)
        {
            MySqlCommand cmd = new MySqlCommand();
            int returnValue;
            string sql = "SELECT TABLE_ID FROM fd_table WHERE TABLE_NAME=@TABLE_NAME";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@TABLE_NAME", tableName);
            returnValue = Convert.ToInt32(cmd.ExecuteScalar());
            return returnValue;
        }
        public string GetTableName(int tableId)
        {
            string returnValue = Convert.ToString(GetOneParameter(tableId, "TABLE_ID", "fd_table", "TABLE_NAME"));
            return returnValue;
        }
        public string GetTableKeyFieldName(int tableId)
        {
            string returnValue = Convert.ToString(GetOneParameter(tableId, "TABLE_ID", "fd_table", "TABLE_KEYFIELD_NAME"));
            return returnValue;
        }
        public int GetTableNoteId(int tableId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(tableId, "TABLE_ID", "fd_table", "STRID_TABLE_NOTE"));
            return returnValue;
        }
        public int AddTable(string TableName, string TABLE_KEYFIELD_NAME, string TableNote, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_table", "TABLE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate TABLE_ID"));

            int noteSTRID = AddOrGetString(TableNote, Language);

            string sql = "INSERT INTO fd_table "
                + "(TABLE_ID, TABLE_NAME, TABLE_KEYFIELD_NAME, STRID_TABLE_NOTE) "
                + "VALUES (@TABLE_ID, @TABLE_NAME, @TABLE_KEYFIELD_NAME, @STRID_TABLE_NOTE)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@TABLE_ID", generatedId);
            cmd.Parameters.AddWithValue("@TABLE_NAME", TableName);
            cmd.Parameters.AddWithValue("@TABLE_KEYFIELD_NAME", TABLE_KEYFIELD_NAME);
            cmd.Parameters.AddWithValue("@STRID_TABLE_NOTE", noteSTRID);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        //FD_ACTION
        public List<KeyValuePair<string, int>> GetAllActions(string Language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> hash;
            List<int> actionIds = new List<int>();
            string sql = "SELECT ACTION_ID FROM fd_action";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                actionIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            sql = "SELECT STRID_ACTION_NAME FROM fd_action WHERE ACTION_ID=@ACTION_ID";
            int stringId;
            foreach (int id in actionIds)
            {
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@ACTION_ID", id);
                stringId = Convert.ToInt32(cmd.ExecuteScalar());
                hash = new KeyValuePair<string, int>(GetString(stringId, Language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public int GetActionSrId(int actionId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(actionId, "ACTION_ID", "fd_action", "STRID_ACTION_NAME"));
            return returnValue;
        }
        public int GetActionId(int actionStrId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(actionStrId, "STRID_ACTION_NAME", "fd_action", "ACTION_ID"));
            return returnValue;
        }
        public int AddAction(string actionString, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_action", "ACTION_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate ACTION_ID"));

            int noteSTRID = AddOrGetString(actionString, Language);

            string sql = "INSERT INTO fd_action "
                + "(ACTION_ID, STRID_ACTION_NAME) "
                + "VALUES (@ACTION_ID, @STRID_ACTION_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ACTION_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_ACTION_NAME", noteSTRID);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        #endregion
        //-------------------------------INVOICE TABLES---------------
        #region "InvoiceTables"
        //FD_INVOICE_STATUS
        public int GetInvoiceStatusId(int statusSTRID)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(statusSTRID, "STRID_INVOICE_STATUS_NAME", "fd_invoice_status", "INVOICE_STATUS_ID"));
            return returnValue;
        }
        public int GetInvoiceStatusNameStrId(int invoiceStatusId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(invoiceStatusId, "INVOICE_STATUS_ID", "fd_invoice_status", "STRID_INVOICE_STATUS_NAME"));
            return returnValue;
        }
        public int AddInvoiceStatus(string invoiceStatusName, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_invoice_status", "INVOICE_STATUS_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate INVOICE_STATUS_ID"));

            int noteSTRID = AddOrGetString(invoiceStatusName, Language);

            string sql = "INSERT INTO fd_invoice_status "
                + "(INVOICE_STATUS_ID, STRID_INVOICE_STATUS_NAME) "
                + "VALUES (@INVOICE_STATUS_ID, @STRID_INVOICE_STATUS_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@INVOICE_STATUS_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_INVOICE_STATUS_NAME", noteSTRID);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        //FD_INVOICE_TYPE
        public int GetInvoiceTypeId(int typeSTRID)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(typeSTRID, "STRID_INVOICE_TYPE_NAME", "fd_invoice_type", "INVOICE_TYPE_ID"));
            return returnValue;
        }
        public int GetInvoiceTypeNameStrId(int invoiceTypeId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(invoiceTypeId, "INVOICE_TYPE_ID", "fd_invoice_type", "STRID_INVOICE_TYPE_NAME"));
            return returnValue;
        }
        public int AddInvoiceType(string invoiceTypeName, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_invoice_type", "INVOICE_TYPE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate INVOICE_TYPE_ID"));

            int noteSTRID = AddOrGetString(invoiceTypeName, Language);

            string sql = "INSERT INTO fd_invoice_type "
                + "(INVOICE_TYPE_ID, STRID_INVOICE_TYPE_NAME) "
                + "VALUES (@INVOICE_TYPE_ID, @STRID_INVOICE_TYPE_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@INVOICE_TYPE_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_INVOICE_TYPE_NAME", noteSTRID);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        //FN_INVOICE
        public void UpdateInvoice(int invoiceId, int statusId, DateTime payDate)//Если не оплачен - передавать просто new DateTime()
        {
            string sql = "UPDATE fn_invoice SET DATE_PAYMENT=@DATE_PAYMENT, INVOICE_STATUS_ID=@INVOICE_STATUS_ID WHERE INVOICE_ID=@INVOICE_ID";
            MySqlCommand cmd = new MySqlCommand();
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@INVOICE_ID", invoiceId);
            cmd.Parameters.AddWithValue("@INVOICE_STATUS_ID", statusId);
            cmd.Parameters.AddWithValue("@DATE_PAYMENT", payDate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
        }
        public List<int> GetAllInvoices(int orgId)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT INVOICE_ID FROM fn_invoice WHERE ORG_ID=@ORG_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public List<int> GetAllInvoiceStatuses()
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT INVOICE_STATUS_ID FROM fd_invoice_status";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public int AddInvoice(int invoiceTypeId, int invoiceStatusId, int orgId, string BillName,
            DateTime dateInvoice, DateTime datePaymentTerm, DateTime datePayment, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fn_invoice", "INVOICE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate INVOICE_ID"));

            int billNameSTRID = AddOrGetString(BillName, Language);

            string sql = "INSERT INTO fn_invoice "
                + "(INVOICE_ID, INVOICE_TYPE_ID, INVOICE_STATUS_ID, ORG_ID, BILL_NAME_STRID, DATE_INVOICE, DATE_PAYMENT_TERM, DATE_PAYMENT) "
                + "VALUES (@INVOICE_ID, @INVOICE_TYPE_ID, @INVOICE_STATUS_ID, @ORG_ID, @BILL_NAME_STRID, @DATE_INVOICE, @DATE_PAYMENT_TERM, @DATE_PAYMENT)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@INVOICE_ID", generatedId);
            cmd.Parameters.AddWithValue("@INVOICE_TYPE_ID", invoiceTypeId);
            cmd.Parameters.AddWithValue("@INVOICE_STATUS_ID", invoiceStatusId);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@BILL_NAME_STRID", billNameSTRID);
            cmd.Parameters.AddWithValue("@DATE_INVOICE", dateInvoice.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@DATE_PAYMENT_TERM", datePaymentTerm.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@DATE_PAYMENT", datePayment.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public int GetInvoice_TypeId(int invoiceId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(invoiceId, "INVOICE_ID", "fn_invoice", "INVOICE_TYPE_ID"));
            return returnValue;
        }
        public int GetInvoice_StatusId(int invoiceId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(invoiceId, "INVOICE_ID", "fn_invoice", "INVOICE_STATUS_ID"));
            return returnValue;
        }
        public string GetInvoiceStatusName(int statusId)
        {
            string returnValue = Convert.ToString(GetOneParameter(statusId, "INVOICE_STATUS_ID", "fd_invoice_status", "INVOICE_STATUS_NAME"));
            return returnValue;
        }
        public int GetInvoice_OrgId(int invoiceId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(invoiceId, "INVOICE_ID", "fn_invoice", "ORG_ID"));
            return returnValue;
        }
        public int GetInvoice_BillNameStrId(int invoiceId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(invoiceId, "INVOICE_ID", "fn_invoice", "BILL_NAME_STRID"));
            return returnValue;
        }
        public string GetInvoice_Date(int invoiceId)
        {
            string returnValue = Convert.ToString(GetOneParameter(invoiceId, "INVOICE_ID", "fn_invoice", "DATE_INVOICE"));
            return returnValue;
        }
        public string GetInvoice_DatePaymentTerm(int invoiceId)
        {
            string returnValue = Convert.ToString(GetOneParameter(invoiceId, "INVOICE_ID", "fn_invoice", "DATE_PAYMENT_TERM"));
            return returnValue;
        }
        public string GetInvoice_DatePayment(int invoiceId)
        {
            string returnValue = Convert.ToString(GetOneParameter(invoiceId, "INVOICE_ID", "fn_invoice", "DATE_PAYMENT"));
            return returnValue;
        }
        #endregion
        //-------------------------------REPORTS TABLES---------------------
        #region "ReportsTable"
        //FD_REPORT_TYPE
        public int AddReportType(string reportName, string reportShortName, string reportFullName, string reportPrintName, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fd_report_type", "REPORT_TYPE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate REPORT_TYPE_ID"));

            int nameSTRID = AddOrGetString(reportName, Language);
            int shortNameSTRID = AddOrGetString(reportShortName, Language);
            int fullNameSTRID = AddOrGetString(reportFullName, Language);
            int printNameSTRID = AddOrGetString(reportPrintName, Language);

            string sql = "INSERT INTO fd_report_type "
                + "(REPORT_TYPE_ID, STRID_REPORT_NAME, STRID_REPORT_SHORT_NAME, STRID_REPORT_FULL_NAME, STRID_REPORT_PRINT_NAME) "
                + "VALUES (@REPORT_TYPE_ID, @STRID_REPORT_NAME, @STRID_REPORT_SHORT_NAME, @STRID_REPORT_FULL_NAME, @STRID_REPORT_PRINT_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@REPORT_TYPE_ID", generatedId);
            cmd.Parameters.AddWithValue("@STRID_REPORT_NAME", nameSTRID);
            cmd.Parameters.AddWithValue("@STRID_REPORT_SHORT_NAME", shortNameSTRID);
            cmd.Parameters.AddWithValue("@STRID_REPORT_FULL_NAME", fullNameSTRID);
            cmd.Parameters.AddWithValue("@STRID_REPORT_PRINT_NAME", printNameSTRID);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public List<KeyValuePair<string, int>> GetAllReportTypes(string language)
        {
            List<KeyValuePair<string, int>> returnArray = new List<KeyValuePair<string, int>>();
            KeyValuePair<string, int> hash;
            List<int> allTypesIds = new List<int>();
            string sql = "SELECT REPORT_TYPE_ID FROM fd_report_type";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                allTypesIds.Add(sdr.GetInt32(0));
            }
            sdr.Close();

            sql = "SELECT STRID_REPORT_NAME FROM fd_report_type WHERE REPORT_TYPE_ID=@REPORT_TYPE_ID";
            int stringId;
            foreach (int id in allTypesIds)
            {
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@REPORT_TYPE_ID", id);
                stringId = Convert.ToInt32(cmd.ExecuteScalar());
                hash = new KeyValuePair<string, int>(GetString(stringId, language), id);
                returnArray.Add(hash);
            }
            return returnArray;
        }
        public int GetReportType_ReportNameStrId(int reportTypeId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(reportTypeId, "REPORT_TYPE_ID", "fd_report_type", "STRID_REPORT_NAME"));
            return returnValue;
        }
        public int GetReportType_ReportShortNameStrId(int reportTypeId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(reportTypeId, "REPORT_TYPE_ID", "fd_report_type", "STRID_REPORT_SHORT_NAME"));
            return returnValue;
        }
        public int GetReportType_ReportFullNameStrId(int reportTypeId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(reportTypeId, "REPORT_TYPE_ID", "fd_report_type", "STRID_REPORT_FULL_NAME"));
            return returnValue;
        }
        public int GetReportType_ReportPrintNameStrId(int reportTypeId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(reportTypeId, "REPORT_TYPE_ID", "fd_report_type", "STRID_REPORT_PRINT_NAME"));
            return returnValue;
        }
        public void EditReportType(int reportTypeId, string reportName, string reportShortName, string reportFullName, string reportPrintName, string Language)
        {
            EditAnySTRIDValue(reportName, "STRID_REPORT_NAME", Language, "fd_report_type", "REPORT_TYPE_ID", reportTypeId);
            EditAnySTRIDValue(reportShortName, "STRID_REPORT_SHORT_NAME", Language, "fd_report_type", "REPORT_TYPE_ID", reportTypeId);
            EditAnySTRIDValue(reportFullName, "STRID_REPORT_FULL_NAME", Language, "fd_report_type", "REPORT_TYPE_ID", reportTypeId);
            EditAnySTRIDValue(reportPrintName, "STRID_REPORT_PRINT_NAME", Language, "fd_report_type", "REPORT_TYPE_ID", reportTypeId);
        }
        //FN_REPORT_USER
        public int AddUserReport(int ReportTypeId, string reportUserName, DateTime dateCreate, DateTime dateUpdate, int Price, string Note, string Language)
        {
            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("fn_report_user", "REPORT_USER_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate REPORT_USER_ID"));

            int userNameSTRID = AddOrGetString(reportUserName, Language);

            string sql = "INSERT INTO fn_report_user "
                + "(REPORT_USER_ID, REPORT_TYPE_ID, STRID_REPORT_USER_NAME, DATE_CREATE, DATE_UPDATE, PRICE, NOTE) "
                + "VALUES (@REPORT_USER_ID, @REPORT_TYPE_ID, @STRID_REPORT_USER_NAME, @DATE_CREATE, @DATE_UPDATE, @PRICE, @NOTE)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@REPORT_USER_ID", generatedId);
            cmd.Parameters.AddWithValue("@REPORT_TYPE_ID", ReportTypeId);
            cmd.Parameters.AddWithValue("@STRID_REPORT_USER_NAME", userNameSTRID);
            cmd.Parameters.AddWithValue("@DATE_CREATE", dateCreate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@DATE_UPDATE", dateUpdate.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@PRICE", Price);
            cmd.Parameters.AddWithValue("@NOTE", Note);
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public List<int> GetAllUserReportsIds()
        {
            List<int> gettedId = new List<int>();
            string sql = "SELECT REPORT_USER_ID FROM fn_report_user ORDER BY REPORT_USER_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public List<int> GetAllUserReportsIds(int reportTypeId)
        {
            List<int> gettedId = new List<int>();
            string sql = "SELECT REPORT_USER_ID FROM fn_report_user WHERE REPORT_TYPE_ID=@REPORT_TYPE_ID ORDER BY REPORT_USER_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@REPORT_TYPE_ID", reportTypeId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }

        public int GetUserReport_ReportTypeId(int reportUserId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(reportUserId, "REPORT_USER_ID", "fn_report_user", "REPORT_TYPE_ID"));
            return returnValue;
        }
        public int GetUserReport_ReportUserNameStrId(int reportUserId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(reportUserId, "REPORT_USER_ID", "fn_report_user", "STRID_REPORT_USER_NAME"));
            return returnValue;
        }
        public string GetUserReport_DateCreate(int reportUserId)
        {
            string returnValue = Convert.ToString(GetOneParameter(reportUserId, "REPORT_USER_ID", "fn_report_user", "DATE_CREATE"));
            return returnValue;
        }
        public string GetUserReport_DateUpdate(int reportUserId)
        {
            string returnValue = Convert.ToString(GetOneParameter(reportUserId, "REPORT_USER_ID", "fn_report_user", "DATE_UPDATE"));
            return returnValue;
        }
        public int GetUserReport_PRICE(int reportUserId)
        {
            int returnValue = Convert.ToInt32(GetOneParameter(reportUserId, "REPORT_USER_ID", "fn_report_user", "PRICE"));
            return returnValue;
        }
        public string GetUserReport_Note(int reportUserId)
        {
            string returnValue = Convert.ToString(GetOneParameter(reportUserId, "REPORT_USER_ID", "fn_report_user", "NOTE"));
            return returnValue;
        }
        public string GetUserReport_TemplateName(int reportUserId)
        {
            string returnValue = Convert.ToString(GetOneParameter(reportUserId, "REPORT_USER_ID", "fn_report_user", "TEMPLATE_NAME"));
            return returnValue;
        }
        //FN_REPORT_USER_ROLES
        public List<int> GetAllUserRolesReportsId(int userRole, int orgId)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT REPORT_USER_ID FROM fn_report_user_roles WHERE ORG_ID=@ORG_ID AND USER_ROLE_ID=@USER_ROLE_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@USER_ROLE_ID", userRole);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public string GetReportUserRoles_BDATE(int reportsUserId, int orgId, int userRole)
        {
            MySqlCommand cmd = new MySqlCommand();
            string returnValue;
            string sql = "SELECT BDATE FROM fn_report_user_roles WHERE REPORT_USER_ID=@REPORT_USER_ID AND ORG_ID=@ORG_ID AND USER_ROLE_ID=@USER_ROLE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@REPORT_USER_ID", reportsUserId);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@USER_ROLE_ID", userRole);
            returnValue = Convert.ToString(cmd.ExecuteScalar());
            return returnValue;
        }
        public string GetReportUserRoles_EDATE(int reportsUserId, int orgId, int userRole)
        {
            MySqlCommand cmd = new MySqlCommand();
            string returnValue;
            string sql = "SELECT EDATE FROM fn_report_user_roles WHERE REPORT_USER_ID=@REPORT_USER_ID AND ORG_ID=@ORG_ID AND USER_ROLE_ID=@USER_ROLE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@REPORT_USER_ID", reportsUserId);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@USER_ROLE_ID", userRole);
            returnValue = Convert.ToString(cmd.ExecuteScalar());
            return returnValue;
        }
        public string GetReportUserRoles_SETDATE(int reportsUserId, int orgId, int userRole)
        {
            MySqlCommand cmd = new MySqlCommand();
            string returnValue;
            string sql = "SELECT DATE_SET FROM fn_report_user_roles WHERE REPORT_USER_ID=@REPORT_USER_ID AND ORG_ID=@ORG_ID AND USER_ROLE_ID=@USER_ROLE_ID";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@REPORT_USER_ID", reportsUserId);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@USER_ROLE_ID", userRole);
            returnValue = Convert.ToString(cmd.ExecuteScalar());
            return returnValue;
        }
        public void AddOrSetReportUserRoles_SETDATE(int reportsUserId, int orgId, int userRole, bool IsActive)
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql;
            DateTime setValue;

            if (IsActive)
                setValue = DateTime.Now;
            else
                setValue = new DateTime();

            if (GetReportUserRoles_SETDATE(reportsUserId, orgId, userRole).Trim() == "")//ADD
            {
                sql = "INSERT INTO fn_report_user_roles "
                    + "(DATE_SET, REPORT_USER_ID, ORG_ID, USER_ROLE_ID) "
                    + "VALUES (@DATE_SET, @REPORT_USER_ID, @ORG_ID, @USER_ROLE_ID)";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@REPORT_USER_ID", reportsUserId);
                cmd.Parameters.AddWithValue("@ORG_ID", orgId);
                cmd.Parameters.AddWithValue("@USER_ROLE_ID", userRole);
                cmd.Parameters.AddWithValue("@DATE_SET", setValue.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            else//EDIT/SET
            {
                sql = "UPDATE fn_report_user_roles SET DATE_SET=@DATE_SET WHERE REPORT_USER_ID=@REPORT_USER_ID AND ORG_ID=@ORG_ID AND USER_ROLE_ID=@USER_ROLE_ID";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@REPORT_USER_ID", reportsUserId);
                cmd.Parameters.AddWithValue("@ORG_ID", orgId);
                cmd.Parameters.AddWithValue("@USER_ROLE_ID", userRole);
                cmd.Parameters.AddWithValue("@DATE_SET", setValue.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
        }
        //FN_REPORT_USER_ORG
        public List<int> GetAllUserOrgReportsId(int orgId)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT REPORT_USER_ID FROM fn_report_user_org WHERE ORG_ID=@ORG_ID AND USER_ROLE_ID=@USER_ROLE_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public string GetReportUserOrg_BDATE(int reportsUserId, int orgId)
        {
            string returnValue = Convert.ToString(GetOneParameter(reportsUserId, "REPORT_USER_ID", orgId, "ORG_ID", "fn_report_user_org", "BDATE"));
            return returnValue;
        }
        public string GetReportUserOrg_EDATE(int reportsUserId, int orgId)
        {
            string returnValue = Convert.ToString(GetOneParameter(reportsUserId, "REPORT_USER_ID", orgId, "ORG_ID", "fn_report_user_org", "EDATE"));
            return returnValue;
        }
        public string GetReportUserOrg_SETDATE(int reportsUserId, int orgId)
        {
            string returnValue = Convert.ToString(GetOneParameter(reportsUserId, "REPORT_USER_ID", orgId, "ORG_ID", "fn_report_user_org", "DATE_SET"));
            return returnValue;
        }
        public void AddOrSetReportUserOrg_SETDATE(int reportsUserId, int orgId, bool IsActive)
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql;
            DateTime bdateValue;
            DateTime edateValue;

            if (IsActive)
            {
                bdateValue = DateTime.Now;
                edateValue = bdateValue.AddMonths(6);
            }
            else
            {
                bdateValue = new DateTime();
                edateValue = new DateTime();
            }

            if (GetReportUserOrg_BDATE(reportsUserId, orgId).Trim() == "")//ADD
            {
                sql = "INSERT INTO fn_report_user_org "
                    + "(REPORT_USER_ID, ORG_ID, BDATE, EDATE) "
                    + "VALUES (@REPORT_USER_ID, @ORG_ID, @BDATE, @EDATE)";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@REPORT_USER_ID", reportsUserId);
                cmd.Parameters.AddWithValue("@ORG_ID", orgId);
                cmd.Parameters.AddWithValue("@BDATE", bdateValue.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EDATE", edateValue.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
            else//EDIT/SET
            {
                sql = "UPDATE fn_report_user_org SET BDATE=@BDATE, EDATE=@EDATE  WHERE REPORT_USER_ID=@REPORT_USER_ID AND ORG_ID=@ORG_ID";
                cmd = new MySqlCommand(sql, sqlConnection);
                cmd.Parameters.AddWithValue("@REPORT_USER_ID", reportsUserId);
                cmd.Parameters.AddWithValue("@ORG_ID", orgId);
                cmd.Parameters.AddWithValue("@BDATE", bdateValue.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.Parameters.AddWithValue("@EDATE", edateValue.ToString("yyyy-MM-dd HH:mm:ss"));
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
        //------------------------------Email Schedule------------------------------------
        public int AddEmailSchedule(int orgId, int userId, int reportId, int cardId, int periodType, int period, string emailAddress)
        {
            MySqlCommand cmd = new MySqlCommand();
            int generatedId;
            generatedId = generateId("email_schedule", "EMAIL_SCHEDULE_ID");
            if (generatedId == -1)
                throw (new Exception("Can't generate EMAIL_SCHEDULE_ID"));

            string sql = "INSERT INTO email_schedule "
                + "(EMAIL_SCHEDULE_ID, ORG_ID, USER_ID, REPORT_ID, CARD_ID, RERIOD, PERIOD_TYPE, LAST_SEND_DATE, EMAIL_ADDRESS) "
                + "VALUES (@EMAIL_SCHEDULE_ID, @ORG_ID, @USER_ID, @REPORT_ID, @CARD_ID, @RERIOD, @PERIOD_TYPE, @LAST_SEND_DATE, @EMAIL_ADDRESS)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@EMAIL_SCHEDULE_ID", generatedId);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            cmd.Parameters.AddWithValue("@REPORT_ID", reportId);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            cmd.Parameters.AddWithValue("@RERIOD", period);
            cmd.Parameters.AddWithValue("@PERIOD_TYPE", periodType);//0-Минуты, 1-дни, 2-месяцы, 3-годы
            cmd.Parameters.AddWithValue("@EMAIL_ADDRESS", emailAddress);
            cmd.Parameters.AddWithValue("@LAST_SEND_DATE", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
            return generatedId;
        }
        public void EditEmailSchedule(int sheduleId, int orgId, int userId, int reportId, int cardId, int period, int periodType, string emailAddress)
        {
            string sql = "UPDATE email_schedule SET ORG_ID=@ORG_ID, USER_ID=@USER_ID, REPORT_ID=@REPORT_ID, CARD_ID=@CARD_ID, "
                + "RERIOD=@RERIOD, PERIOD_TYPE=@PERIOD_TYPE, EMAIL_ADDRESS=@EMAIL_ADDRESS "
                + "WHERE EMAIL_SCHEDULE_ID=@EMAIL_SCHEDULE_ID";
            MySqlCommand cmd = new MySqlCommand();
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@EMAIL_SCHEDULE_ID", sheduleId);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            cmd.Parameters.AddWithValue("@REPORT_ID", reportId);
            cmd.Parameters.AddWithValue("@CARD_ID", cardId);
            cmd.Parameters.AddWithValue("@RERIOD", period);
            cmd.Parameters.AddWithValue("@PERIOD_TYPE", periodType);//0-Минуты, 1-дни, 2-месяцы, 3-годы
            cmd.Parameters.AddWithValue("@EMAIL_ADDRESS", emailAddress);
            cmd.ExecuteNonQuery();
        }
        public List<int> GetAllEmailScheduleIds()
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT EMAIL_SCHEDULE_ID FROM email_schedule";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        public List<int> GetAllEmailScheduleIds(int orgId, int userId)
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT EMAIL_SCHEDULE_ID FROM email_schedule WHERE ORG_ID=@ORG_ID AND USER_ID=@USER_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@ORG_ID", orgId);
            cmd.Parameters.AddWithValue("@USER_ID", userId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        /// <summary>
        /// Получает список дат последней отправки и период для каждого ID
        /// </summary>
        /// <param name="emailScheduleId">список ID записей отправки отчета на почту</param>
        /// <returns>список пар значений - Последняя дата отправки/(тип периода/период отправки)</returns>
        public List<KeyValuePair<DateTime, KeyValuePair<int, int>>> GetEmailScheduleTimes(List<int> emailScheduleIds)
        {
            StringBuilder IdIN = new StringBuilder();
            foreach (int ID in emailScheduleIds)
            {
                IdIN.Append(ID.ToString());
                IdIN.Append(", ");
            }
            IdIN.Remove(IdIN.Length - 2, 2);
            List<KeyValuePair<DateTime, KeyValuePair<int, int>>> returnArray = new List<KeyValuePair<DateTime, KeyValuePair<int, int>>>();
            int periodType = 0;
            int period = 0;
            DateTime lastSendDate = new DateTime();
            string sql = "SELECT PERIOD_TYPE, RERIOD, LAST_SEND_DATE FROM email_schedule WHERE EMAIL_SCHEDULE_ID IN (" + IdIN + ") ORDER BY EMAIL_SCHEDULE_ID";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                periodType = sdr.GetInt32("PERIOD_TYPE");//0-Минуты, 1-дни, 2-месяцы, 3-годы
                period = sdr.GetInt32("RERIOD");
                lastSendDate = DateTime.Parse(sdr.GetString("LAST_SEND_DATE"));
                returnArray.Add(new KeyValuePair<DateTime, KeyValuePair<int, int>>(lastSendDate, new KeyValuePair<int, int>(periodType, period)));
            }
            sdr.Close();
            return returnArray;
        }
        /// <summary>
        /// Получает все поля в таблице emailSchedule
        /// </summary>
        /// <param name="scheduleId">ID записи отправки отчета на почту</param>
        /// <returns>
        /// Возвращает список объектов, которые надо привести к нужному типу по очереди(смотреть класс SingleEmailSchedule в пространстве имен BLL)
        /// Если ничего не поменялось, то список такой:
        /// int EMAIL_SCHEDULE_ID;
        /// int ORG_ID;
        /// int USER_ID;
        /// int REPORT_ID;
        /// int CARD_ID;
        /// int PERIOD;
        /// int PERIOD_TYPE;
        /// DateTime LAST_SEND_DATE;(использовать для приведения DateTime.Parse(object.toString()) для правильного приведения;
        /// string EMAIL_ADDRESS;
        /// </returns>
        public List<object> GetAllEmailScheduleTable(int scheduleId)
        {
            List<object> returnList = new List<object>();
            string sql = "SELECT * FROM email_schedule WHERE EMAIL_SCHEDULE_ID=@EMAIL_SCHEDULE_ID ";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@EMAIL_SCHEDULE_ID", scheduleId);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                returnList.Add(sdr.GetInt32("EMAIL_SCHEDULE_ID"));
                returnList.Add(sdr.GetInt32("ORG_ID"));
                returnList.Add(sdr.GetInt32("USER_ID"));
                returnList.Add(sdr.GetInt32("REPORT_ID"));
                returnList.Add(sdr.GetInt32("CARD_ID"));
                returnList.Add(sdr.GetInt32("RERIOD"));
                returnList.Add(sdr.GetInt32("PERIOD_TYPE"));
                returnList.Add(sdr.GetString("LAST_SEND_DATE"));
                returnList.Add(sdr.GetString("EMAIL_ADDRESS"));
            }
            sdr.Close();
            return returnList;
        }
        public void DeleteEmailShedule(int sheduleId)
        {
            string sql;
            MySqlCommand cmd;
            sql = "DELETE FROM email_schedule WHERE EMAIL_SCHEDULE_ID = @EMAIL_SCHEDULE_ID ";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@EMAIL_SCHEDULE_ID", sheduleId);
            cmd.ExecuteNonQuery();
        }
        public void SetEmailSheduleLastSendDate(int sheduleId)
        {
            SetCurrentTime("email_schedule", "EMAIL_SCHEDULE_ID", sheduleId, "LAST_SEND_DATE");
        }
        //Email_Format
        public List<int> GetAllEmailExportFormat()
        {
            List<int> gettedId = new List<int>();

            string sql = "SELECT FORMAT_ID FROM email_export_format";
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                gettedId.Add(sdr.GetInt32(0));
            }
            sdr.Close();
            return gettedId;
        }
        /*  public int GetEmailExportFormatNameId()
          {
          }
          public int AddEmailExportFormat(string formatName)
          {
          }*/
        //-------------------for all
        public DateTime SetCurrentTime(string tableName, string tablePrimaryKeyName, int tablePrimaryKeyId, string dateRowName)
        {
            DateTime returnValue = new DateTime();
            returnValue = DateTime.Now;
            string currentDate = returnValue.ToString("yyyy-MM-dd HH:mm:ss");
            MySqlCommand cmd;
            string sql = "UPDATE " + tableName + " SET " + dateRowName + "=@" + dateRowName + " WHERE " + tablePrimaryKeyName + "=" + tablePrimaryKeyId.ToString();

            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@" + dateRowName, currentDate);

            cmd.ExecuteNonQuery();
            return returnValue;
        }
        public int checkTableExistence(string tableName, string tablePrimaryKeyName, int tablePrimaryKeyId)
        {
            int gettedPrimaryKey;
            MySqlCommand cmd;
            sqlConnection.Open();
            string sql = "SELECT COUNT(*) " + " FROM " + tableName + " WHERE " + tablePrimaryKeyName + " = @" + tablePrimaryKeyName;

            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@" + tablePrimaryKeyName, tablePrimaryKeyId);

            gettedPrimaryKey = Convert.ToInt32(cmd.ExecuteScalar());
            sqlConnection.Close();
            return gettedPrimaryKey;
        }
        private string SetConnectionString(string connectionStringTemp)
        {
            connectionString = connectionStringTemp;
            return connectionString;
        }
        private int getLastId()
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection sqlConnection = new MySqlConnection();
            sqlConnection.ConnectionString = connectionString;
            string sql = "SELECT LAST_INSERT_ID()";
            int lastId = 0;

            try
            {
                cmd = new MySqlCommand(sql, sqlConnection);
                sqlConnection.Open();
                lastId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
            finally
            {
                sqlConnection.Close();
            }
            string lastIdPrint = "Last Id = !" + lastId.ToString();
            Console.WriteLine(lastIdPrint);
            return lastId;
        }
        public int generateId(string tableName, string PRIMARY_KEY)
        {
            MySqlCommand cmd = new MySqlCommand();

            string sqlCount = "SELECT COUNT(" + PRIMARY_KEY + ") FROM " + tableName;
            string sqlMax = "SELECT max(" + PRIMARY_KEY + ") FROM " + tableName;
            int highestId = -1;
            try
            {
                cmd = new MySqlCommand(sqlCount, sqlConnection);
                highestId = Convert.ToInt32(cmd.ExecuteScalar());

                if (highestId == 0)
                    highestId = 0;
                else
                {
                    cmd = new MySqlCommand(sqlMax, sqlConnection);
                    highestId = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
            highestId++;
            // string lastIdPrint = "Generated " + PRIMARY_KEY + " for " + tableName + " = " + highestId.ToString();
            // Console.WriteLine(lastIdPrint);          
            return highestId;

        }
        public object GetOneParameter(int primaryId, string primaryKeyName, string tableName, string paramName)
        {
            string sql = "Select " + paramName + " FROM " + tableName + " WHERE " + primaryKeyName + " = @" + primaryKeyName;
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@" + primaryKeyName, primaryId);
            object o = cmd.ExecuteScalar();
            if (o == null)
                return null;
            if (o.GetType() == DBNull.Value.GetType())
                return null;
            else
                return o;
        }
        /// <summary>
        /// метод для получения записи из таблицы с составным ключем или другим условием.
        /// </summary>
        public object GetOneParameter(int primaryOneId, string primaryOneName, int primaryTwoId, string primaryTwoName, string tableName, string paramName)
        {
            string sql = "Select " + paramName + " FROM " + tableName + " WHERE " + primaryOneName + " = @" + primaryOneName
                + " AND " + primaryTwoName + " = @" + primaryTwoName;
            MySqlCommand cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@" + primaryOneName, primaryOneId);
            cmd.Parameters.AddWithValue("@" + primaryTwoName, primaryTwoId);
            object o = cmd.ExecuteScalar();
            if (o == null)
                return null;
            if (o.GetType() == DBNull.Value.GetType())
                return null;
            else
                return o;
        }

        public void DataBaseInit()
        {
            MySqlCommand cmd = new MySqlCommand();
            string sql = "INSERT INTO fd_string "
                    + "(STRING_ID, STRING_RU)"
                    + "VALUES (@STRING_ID, @STRING_RU)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@STRING_ID", 0);
            cmd.Parameters.AddWithValue("@STRING_RU", "");
            cmd.ExecuteNonQuery();

            //dataBlockStates
            InitTable_ID_String("fd_data_block_state", "DATA_BLOCK_STATE_ID", "STRID_DATA_BLOCK_STATE_NAME", "Ideal");
            InitTable_ID_String("fd_data_block_state", "DATA_BLOCK_STATE_ID", "STRID_DATA_BLOCK_STATE_NAME", "Parsed");
            InitTable_ID_String("fd_data_block_state", "DATA_BLOCK_STATE_ID", "STRID_DATA_BLOCK_STATE_NAME", "Not parsed");

            //dataRecordsState
            InitTable_ID_String("fd_data_record_state", "DATA_RECORD_STATE_ID", "STRID_DATA_RECORD_STATE_NAME", "Ideal");
            InitTable_ID_String("fd_data_record_state", "DATA_RECORD_STATE_ID", "STRID_DATA_RECORD_STATE_NAME", "Parsed");
            InitTable_ID_String("fd_data_record_state", "DATA_RECORD_STATE_ID", "STRID_DATA_RECORD_STATE_NAME", "Not parsed");

            //Card_TYPE
            int stringId = AddOrGetString("Card Type: Driver");
            int generatedId = generateId("fd_card_type", "CARD_TYPE_ID");
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

            stringId = AddOrGetString("Card Type: Vehicle");
            generatedId = generateId("fd_card_type", "CARD_TYPE_ID");
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

            stringId = AddOrGetString("Card Type: Organization Init Card");
            generatedId = generateId("fd_card_type", "CARD_TYPE_ID");
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
            int deviceFirmWareId = AddNewDeviceFirmware("onboard device by default", DateTime.Now, "0.0.0.0", new byte[1]);

            //fd_device
            int deviceTypeId = AddNewDeviceType("DefaultDeviceType");
            AddNewDevice(deviceTypeId, "The onboard device by default", "00000", DateTime.Now, deviceFirmWareId, 222222);
            //InitTable_ID_String("fd_device", "DEVICE_ID", "STRID_DEVICE_NAME", "");

            //fd_object
            InitTable_ID_String("fd_object", "OBJECT_ID", "STRID_OBJECT_NAME", "Object by default");

            //FD_ORG_TYPE
            InitTable_ID_String("fd_org_type", "ORG_TYPE_ID", "STRID_ORG_TYPE_NAME", "Cargo transportation");
            InitTable_ID_String("fd_org_type", "ORG_TYPE_ID", "STRID_ORG_TYPE_NAME", "Passengers transportation");
            InitTable_ID_String("fd_org_type", "ORG_TYPE_ID", "STRID_ORG_TYPE_NAME", "Anything else transportation");

            //FD_ORG
            generatedId = AddNewOrganization("Init Organization", 1, 1, 1, "Init Organization", "STRING_RU");
            CreateNewCard("Init Organization ORG", "000", orgInitCardTypeId, generatedId, "Карта организации " + "Init Organization" + " для неразобранных блоков данных", 1);

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
            AddUserInfoName("Surname", "STRING_RU");
            AddUserInfoName("Name", "STRING_RU");
            AddUserInfoName("Patronimic", "STRING_RU");
            AddUserInfoName("Drivers certificate", "STRING_RU");
            AddUserInfoName("Card number", "STRING_RU");
            AddUserInfoName("Phone number", "STRING_RU");
            AddUserInfoName("Birthday", "STRING_RU");


            //fd_user_rights
            InitTable_ID_String("fd_user_rights", "USER_RIGHTS_ID", "STRID_USER_RIGHTS_NAME", "Administrator");
            InitTable_ID_String("fd_user_rights", "USER_RIGHTS_ID", "STRID_USER_RIGHTS_NAME", "SuperAdministrator");

            //fd_user_role
            InitTable_ID_String("fd_user_role", "USER_ROLE_ID", "STRID_USER_ROLE_NAME", "Administrator");
            InitTable_ID_String("fd_user_role", "USER_ROLE_ID", "STRID_USER_ROLE_NAME", "SuperAdministrator");

            //fd_user_type
            InitTable_ID_String("fd_user_type", "USER_TYPE_ID", "STRID_USER_TYPE_NAME", "Driver");
            InitTable_ID_String("fd_user_type", "USER_TYPE_ID", "STRID_USER_TYPE_NAME", "Manager");
            InitTable_ID_String("fd_user_type", "USER_TYPE_ID", "STRID_USER_TYPE_NAME", "Managing director");

            //fd_user

            AddNewUser("admin", "123", 3, 1, 1, "admin", "123");
            AddNewUser("admin2", "123", 3, 2, 1, "admin2", "123");

            //fd_measure
            int kgId = AddNewMeasure("Kg", "Kilograms");
            int kmId = AddNewMeasure("Km", "Kilometers");
            int m3Id = AddNewMeasure("M3", "Cubic metres");
            int dateId = AddNewMeasure("Date", "DateTime");
            int RPMId = AddNewMeasure("Rpm", "Revolutions per minute");
            int KmphId = AddNewMeasure("Km/h", "Kilometers per hour");
            int PercentsId = AddNewMeasure("%", "Percents");
            int FConsumption = AddNewMeasure("L/h", "Liters per hour");

            //fd_key
            AddNewCriteria(kgId, "Commentary to a vehicle", "Строка для Комментария к ТС", 0, 0);
            AddNewCriteria(kgId, "Load-carrying capacity", "Грузоподьемность", 5000, 25000);
            AddNewCriteria(m3Id, "Fuel tank 1", "Коментарий к Топливному баку 1", 1, 250);
            AddNewCriteria(m3Id, "Fuel tank 2", "Коментарий к Топливному баку 1", 1, 250);
            AddNewCriteria(dateId, "MRO 1", "Дата ТО 1", 1, 1);
            AddNewCriteria(dateId, "MRO 2", "Дата ТО 2", 1, 1);
            AddNewCriteria(RPMId, "Nominal turns", "критерий для номинальных оборотов", 1, 1);
            AddNewCriteria(KmphId, "Maximum speed", "критерий для максимальной скорости", 1, 1);
            AddNewCriteria(KmphId, "Manoeuvring", "критерий для маневрирования", 1, 1);
            AddNewCriteria(PercentsId, "City", "критерий для города", 1, 1);
            AddNewCriteria(PercentsId, "Highway", "	критерий для магистрали", 1, 1);
            AddNewCriteria(FConsumption, "Nominal fuel consumption", "критерий для номинального расхода топлива", 1, 1);
            AddNewCriteria(RPMId, "Cold start", "критерий для холодного старта", 1, 1);
            AddNewCriteria(RPMId, "Hot stop", "критерий для горячего стопа", 1, 1);

            //fd_fuel_type
            stringId = AddOrGetString("Unknown fuel type");

            sql = "INSERT INTO fd_fuel_type "
               + "(FUEL_TYPE_ID, STRID_FUEL_TYPE_NAME)"
               + "VALUES (@FUEL_TYPE_ID, @STRID_FUEL_TYPE_NAME)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@FUEL_TYPE_ID", 0);
            cmd.Parameters.AddWithValue("@STRID_FUEL_TYPE_NAME", stringId);
            cmd.ExecuteNonQuery();

            stringId = AddOrGetString("Undefined");

            sql = "INSERT INTO fd_vehicle_type "
               + "(VEHICLE_TYPE_ID, STRID_VEHICLE_TYPE_NAME, FUEL_TYPE_ID)"
               + "VALUES (@VEHICLE_TYPE_ID, @STRID_VEHICLE_TYPE_NAME, @FUEL_TYPE_ID)";
            cmd = new MySqlCommand(sql, sqlConnection);
            cmd.Parameters.AddWithValue("@VEHICLE_TYPE_ID", 0);
            cmd.Parameters.AddWithValue("@STRID_VEHICLE_TYPE_NAME", stringId);
            cmd.Parameters.AddWithValue("@FUEL_TYPE_ID", 0);
            cmd.ExecuteNonQuery();

            int fuelTypeID = AddNewFuelType("FuelType 1");
            AddNewVehicleType("Veh type 1", fuelTypeID);

        }

        private int InitTable_ID_String(string tableName, string primaryKeyName, string stringName, string stringValue)
        {
            MySqlCommand cmd = new MySqlCommand();
            int stringId = AddOrGetString(stringValue);
            int generatedId = generateId(tableName, primaryKeyName);
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