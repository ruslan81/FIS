using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    public class DataRecords
    {
        private int DATA_RECORD_ID;
        private int DATA_RECORD_ID_PREVIOUS;
       // private int PARAM_ID;
        private int DATA_BLOCK_ID;
        private int paramId;
        private string CurrentLanguage;//STRING_RU,STRING_ENG etc.
        private SQLDB sqlDB;
        

        private string connectionString;

        public DataRecords(string connectionStringTMP, int dataBlockId, int dataRecordId, string Current_Language, SQLDB sql)//Работа с уже занесенной записью
        {
            sqlDB = sql;
            connectionString = connectionStringTMP;
            DATA_RECORD_ID = dataRecordId;
            DATA_RECORD_ID_PREVIOUS = -1;
            DATA_BLOCK_ID = dataBlockId;
            paramId = -1;
            CurrentLanguage = Current_Language;
        }

        public DataRecords(string connectionStringTMP, int dataBlockId, string Current_Language, SQLDB sql)
        {
            sqlDB = sql;
            connectionString = connectionStringTMP;
            DATA_RECORD_ID = -1;
            DATA_RECORD_ID_PREVIOUS = -1;
            DATA_BLOCK_ID = dataBlockId;
            paramId = -1;
            CurrentLanguage = Current_Language;
        }

        public bool AddData(byte[] dataTemp)
        {
            /*SQLDB sqlDB = new SQLDB(connectionString);
            if (paramId != -1)
            {
                int dataIdTemp = sqlDB.AddDataRecord(dataTemp, DATA_BLOCK_ID, 0, 32);
                if (dataIdTemp == -1)
                {
                    return false;
                }
                else
                {
                    DATA_RECORD_ID_PREVIOUS = DATA_RECORD_ID;
                    DATA_RECORD_ID = dataIdTemp;
                    return true;
                }
            }
            else
                return false;*/
            return true;
        }

        public bool AddData(string name, string value, int paramSize)
        {
            //SQLDB sqlDB = new SQLDB(connectionString);            
            int dataIdTemp = sqlDB.AddDataRecord(name, value, DATA_BLOCK_ID);
            if (dataIdTemp == -1)
            {
                return false;
            }
            else
            {
                DATA_RECORD_ID_PREVIOUS = DATA_RECORD_ID;
                DATA_RECORD_ID = dataIdTemp;
                return true;
            }
        }

        public void AddDataArray(List<ReflectionClass> reflectionClass)
        {//все комменты убрать, если разбор будет плохой в одной транзакции 8.04.2011
            //SQLDB sqlDB = new SQLDB(connectionString);
           //sqlDB.OpenConnection();
           // sqlDB.OpenTransaction();
            foreach (ReflectionClass r in reflectionClass)
            {
                int dataIdTemp = sqlDB.AddDataRecord(r.value, DATA_BLOCK_ID, r.PARAM_ID);
                if (dataIdTemp == -1)
                {
                   // sqlDB.RollbackConnection();
                  //  sqlDB.CloseConnection();
                    throw (new Exception("Troubles with adding records!!!"));
                }                              
            }
           // if (sqlDB.IsConnectionOpened())
           // {
               // sqlDB.CommitConnection();
               // sqlDB.CloseConnection();
           // }
        }
        
        public bool DeleteData()
        {
            //SQLDB sqlDB = new SQLDB(connectionString);
            if (sqlDB.DeleteDataRecord(DATA_RECORD_ID))
                return true;
            else return false;
        }

        public static bool DeleteData(int dataIdTemp, string connectionString)
        {
            SQLDB sqlDB = new SQLDB(connectionString);
            if (sqlDB.DeleteDataRecord(dataIdTemp))
                return true;
            else return false;
        }

        public int DeleteAllData(SQLDB sqlDB)
        {
            return sqlDB.DeleteAllDataRecords(DATA_BLOCK_ID);               
        }

        public static bool DeleteAllData(int dataBlockId, string connectionString)
        {
            SQLDB sqlDB = new SQLDB(connectionString);
            if (sqlDB.DeleteAllDataRecords(dataBlockId) != -1)
                return true;
            else return false;
        }

        public string GetDataState()
        {
            //SQLDB sqlDB = new SQLDB(connectionString);
            int recordStateId;
            string recordStateMessage;

            recordStateId = sqlDB.GetDataRecordState(DATA_RECORD_ID);
            recordStateMessage = sqlDB.GetString(recordStateId, CurrentLanguage);

            return recordStateMessage;
        }

        public string GetDataState(string stringLanguage)
        {
            //SQLDB sqlDB = new SQLDB(connectionString);
            int recordStateId;
            string recordStateMessage;

            recordStateId = sqlDB.GetDataRecordState(DATA_RECORD_ID);
            recordStateMessage = sqlDB.GetString(recordStateId, stringLanguage);

            return recordStateMessage;
        }

      /*  public int AddParam (int parentParamId, int size)
        {
            SQLDB sqlDB = new SQLDB(connectionString);
            paramId = sqlDB.AddParam(parentParamId, size);
            return paramId;
        }

        public void SetParam(int paramIdTemp)
        {            
            paramId = paramIdTemp;
        }*/
    }
}
