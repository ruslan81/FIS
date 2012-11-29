using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.Interface;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// работа с таблицами Транспортного средства.
    /// </summary>
    public class VehiclesTable//для работы с базой данных, таблицами о транспортных средствах
    {
        private string CurrentLanguage;//STRING_RU,STRING_RUG etc.
        private string connectionString;
        private SQLDB sqlDB;

        public void OpenConnection()
        {
            sqlDB.OpenConnection();
        }
        public void CloseConnection()
        {
            sqlDB.CloseConnection();
        }
        public void OpenTransaction()
        {
            sqlDB.OpenTransaction();
        }
        public void CommitTransaction()
        {
            sqlDB.CommitConnection();
        }
        public void RollbackConnection()
        {
            sqlDB.RollbackConnection();
        }

        public VehiclesTable(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            //sqlDB = new SQLDB(connectionString);
            sqlDB = sql;
        }
        //FD_VEHICLE
        public int GetVehicle_byCardId(int CardId)
        {
            int vehId = sqlDB.GetVehicle_byCardId(CardId);
            return vehId;
        }
        public int GetCardId(int vehId)
        {
            return sqlDB.GetVehicleCardId(vehId);
        }
        public string GetVehicleGOSNUM(int vehId)
        {
            string gosNom = sqlDB.GetVehicleGOSNUM(vehId);
            return gosNom;
        }
        public string GetVehicleVin(int vehId)
        {
            string vin = sqlDB.GetVehicleVin(vehId);
            return vin;
        }
        public string GetVehicleMARKA(int vehId)
        {
            int stringId = sqlDB.GetVehicleMARKAStrId(vehId);
            string vehMarka = sqlDB.GetString(stringId, CurrentLanguage);
            return vehMarka;
        }
        public int GetVehicleDeviceId(int vehId)
        {
            return sqlDB.GetVehicleDeviceId(vehId);
        }
        public int GetVehicleTypeId(int vehId)
        {
            return sqlDB.GetVehicleTypeId(vehId);
        }
        public int GetVehiclePriority(int vehId)
        {
           return sqlDB.GetVehiclePriority(vehId);
        }
        public DateTime GetVehicleDateBlocked(int vehId)
        {
            return DateTime.Parse(sqlDB.GetVehicleDateBlocked(vehId));
        }
        public int AddNewVehicle(string GosNomer, string Marka, string VIN, int vehicleTypeId, int deviceId, int cardId, DateTime BLOCKED, int priority)
        {
            int vehId = sqlDB.AddNewVehicle(GosNomer, Marka, VIN, vehicleTypeId, deviceId, cardId, BLOCKED, priority, CurrentLanguage);
            SetAllVehiclesIDS(vehId);
            return vehId;

            //Тут лог не нужен, потому что при создании ТС, создается карта. вот там лог и пишется на ТС
        }
        public void EditVehicle(int VehicleId, string GosNomer, string Marka, string VIN, int vehicleTypeId, int deviceId, DateTime BLOCKED, int priority, int userId)
        {
            sqlDB.EditVehicle(VehicleId, GosNomer, Marka, VIN, vehicleTypeId, deviceId, BLOCKED, priority, CurrentLanguage);
            if (userId > 0)
            {
                HistoryTable log = new HistoryTable(connectionString, CurrentLanguage, sqlDB);
                log.AddHistoryRecord("fn_vehicle", "vehicle_id", VehicleId, userId, log.vehiclesRegDataChanged, " Vehicle number: " + GosNomer + ", VIN: " + VIN + ", code: " + VehicleId, sqlDB);
            }
        }
        public int GetVehicleId_byVinRegNumbers(string vin, string regNumber)
        {
           // sqlDB.OpenConnection();
            int vehId = sqlDB.GetVehicleId_byVinRegNumbers(vin, regNumber);
            //sqlDB.CloseConnection();
            return vehId;
        }
        private void SetAllVehiclesIDS(int vehicleId)
        {
            int keyId = GetCardId(vehicleId);
            sqlDB.SetCardNote(keyId, "Commentary to a vehicle id " + vehicleId.ToString());
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.FuelTank1, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "Fuel tank 1");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.FuelTank2, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "Fuel tank 2");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.LoadCarryingCapacity, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "load-carrying capacity");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.MRO1, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "MRO 1");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.MRO2, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "MRO 2");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.NominalTurns, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "NominalTurns");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.MaxSpeed, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "MaxSpeed");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.Manoeuvring, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "Manoeuvring");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.City, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "City");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.Highway, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "Highway");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.NomFuelConsumption, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "NomFuelConsumption");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.ColdStart, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "ColdStart");
            keyId = sqlDB.GetCriteriaId_byName(DataBaseReference.HotStop, CurrentLanguage);
            AddVehicleKey(vehicleId, keyId, 0, 0, new DateTime(), new DateTime(), "HotStop");
        }
        //FD_VEHICLE_TYPE, FD_FUEL_TYPE
        public string GetVehicleTypeName(int vehId)
        {
           // sqlDB.OpenConnection();
            int vehTypeId = sqlDB.GetVehicleTypeId(vehId);
            int vehNameId = sqlDB.GetVehTypeStrId(vehTypeId);
            string typeName = sqlDB.GetString(vehNameId, CurrentLanguage);
           // sqlDB.CloseConnection();
            return typeName;
        }
        public string GetVehTypeFuelName(int VehTypeId)
        {
            int vehFuelId = sqlDB.GetVehTypeFuelType(VehTypeId);
            int vehFuelStrId = sqlDB.GetVehFuelTypeStrId(vehFuelId);
            return sqlDB.GetString(vehFuelStrId, CurrentLanguage);
        }
        public string GetVehicleTypeName_byVehicleTypeId(int vehTypeId)
        {
            int strid = sqlDB.GetVehTypeStrId(vehTypeId);
            return sqlDB.GetString(strid, CurrentLanguage);
        }
        public void SetVehicleType(int vehId, int vehTypeId)
        {
            sqlDB.SetVehicleTypeId(vehId, vehTypeId);
        }
        public int GetVehTypeFuelId(int VehTypeId)
        {
            int vefFuelId = sqlDB.GetVehTypeFuelType(VehTypeId);
            return vefFuelId;
        }
        public List<KeyValuePair<string, int>> GetAllVehTypes()
        {
            return sqlDB.GetAllVehTypes(CurrentLanguage);
        }
        public List<KeyValuePair<string, int>> GetAllFuelTypes()
        {
            return sqlDB.GetAllVehFuelTypes(CurrentLanguage);
        }
        public int AddNewVehicleType(string Name, int FuelTypeId)
        {
            Exception ex = new Exception("Такой тип транспортных средств уже существует");
            int retVal = -1;
            if (sqlDB.GetVehicleTypeId_byName(Name, CurrentLanguage) == -1)
                retVal = sqlDB.AddNewVehicleType(Name, FuelTypeId);
            else
                throw ex;
            return retVal;
        }
        public void EditVehicleType(int vehTypeId, string Name, int FuelTypeId)
        {
            sqlDB.EditNewVehicleType(vehTypeId, Name, FuelTypeId, CurrentLanguage);
        }
        public void DeleteFuelType(int fuelTypeId)
        {
            sqlDB.DeleteFuelType(fuelTypeId);
        }
        public void DeleteVehicleType(int vehicleTypeId)
        {
            sqlDB.DeleteVehicleType(vehicleTypeId);
        }
        public int AddNewFuelType(string Name)
        {
            Exception ex = new Exception("Такой тип топлива уже существует");
            int retVal = -1;
            if (sqlDB.GetFuelTypeId_byName(Name, CurrentLanguage) == -1)
                retVal = sqlDB.AddNewFuelType(Name);
            else
                throw ex;
            return retVal;
        }
        //FD_VEHICLE_KEY
        List<int> GetAllVehicleKeyIDS(int vehicleId)
        {
            List<int> VehicleKeyIDS = new List<int>();
           // OpenConnection();
            VehicleKeyIDS = sqlDB.GetAllVehicleKeyIDS(vehicleId);
           // CloseConnection();
            return VehicleKeyIDS;
        }
        public int GetVehicleKey_KeyId(int vehicleKeyId)
        {
            return sqlDB.GetVehicleKey_KeyId(vehicleKeyId);
        }
        public int GetVehicleKey_MinVal(int vehicleKeyId)
        {
            return sqlDB.GetVehicleKey_MinVal(vehicleKeyId);
        }
        public int GetVehicleKey_MaxVal(int vehicleKeyId)
        {
            return sqlDB.GetVehicleKey_MaxVal(vehicleKeyId);
        }
        public DateTime GetVehicleKey_BDate(int vehicleKeyId)
        {
            return Convert.ToDateTime(sqlDB.GetVehicleKey_BDate(vehicleKeyId));
        }
        public DateTime GetVehicleKey_EDate(int vehicleKeyId)
        {
            return Convert.ToDateTime(sqlDB.GetVehicleKey_EDate(vehicleKeyId));
        }
        public string GetVehicleKey_Note(int vehicleKeyId)
        {
            object noteObject = sqlDB.GetVehicleKey_NoteId(vehicleKeyId);
            int noteId = Convert.ToInt32(noteObject);
            if (noteId <= 0)
                return "Нет комментария";
            else
            {
                string retVal = sqlDB.GetString(noteId, CurrentLanguage);
                if (retVal == "")
                    return "Нет комментария";
                else
                    return retVal;
            }
        }
        public void AddVehicleKey(int vehicleId, int KeyId, int minVal, int maxVal, DateTime BDate, DateTime EDate, string note)
        {
            sqlDB.AddVehicleKey(vehicleId, KeyId, minVal, maxVal, BDate, EDate, note, CurrentLanguage);
        }
        public void SetVehicleKey(int vehicleKeyId, int minVal, int maxVal, DateTime BDate, DateTime EDate, string note)
        {
            sqlDB.SetVehicleKey(vehicleKeyId, minVal, maxVal, BDate, EDate, note, CurrentLanguage);
        }
        public int GetVehicleKeyId_byKeyNameVehicleId(string KeyName, int VehicleId)
        {
            int vehKeyId = -1;
            int keyNameId = sqlDB.GetStringId(KeyName, "STRING_RU");
            vehKeyId = sqlDB.GetVehicleKeyId_ByVehIdAndKeyId(VehicleId, keyNameId);
            return vehKeyId;
        }
        //FD_USER_INFO_SET and FD_USER_INFO
        public string GetVehicleInfoValue(int VehicleId, int VehicleInfoId)
        {
            int stringId = sqlDB.GetVehicleInfoValueStrId(VehicleId, VehicleInfoId);
            return sqlDB.GetString(stringId, CurrentLanguage);
        }
        public string GetVehicleInfoValue(int VehicleId, string VehicleInfoName)
        {
            int infoNameId = GetVehicleInfoNameId(VehicleInfoName);
            int stringId = sqlDB.GetVehicleInfoValueStrId(VehicleId, infoNameId);
            return sqlDB.GetString(stringId, CurrentLanguage);
        }
        public int GetVehicleInfoNameId(string InfoName)
        {
            int stringId = sqlDB.GetStringId(InfoName);
            int VehicleInfoId = sqlDB.GetVehicleInfoName(stringId);

            if (VehicleInfoId > 0)
                return VehicleInfoId;
            else
                return AddVehicleInfoName(InfoName);
        }
        public List<KeyValuePair<string, int>> GetAllVehicleInfoNames()
        {
            return sqlDB.GetAllVehicleInfoNames(CurrentLanguage);
        }
        public int AddVehicleInfoName(string InfoName)
        {
            return sqlDB.AddVehicleInfoName(InfoName, CurrentLanguage);
        }
        public void AddVehicleInfoValue(int VehicleId, int VehicleInfoId, string value)
        {
            sqlDB.AddVehicleInfoValue(VehicleId, VehicleInfoId, value, CurrentLanguage);
        }
        public void AddVehicleInfoValue(int VehicleId, string VehicleInfoName, string value)
        {
            int infoNameId = GetVehicleInfoNameId(VehicleInfoName);
            if (infoNameId <= 0)
                infoNameId = AddVehicleInfoName(VehicleInfoName);
            sqlDB.AddVehicleInfoValue(VehicleId, infoNameId, value, CurrentLanguage);
        }
        public void EditVehicleInfo(int VehicleId, int VehicleInfoId, string newValue)
        {
            if (sqlDB.GetVehicleInfoValueStrId(VehicleId, VehicleInfoId) > 0)
                sqlDB.EditVehicleInfo(VehicleId, VehicleInfoId, newValue, CurrentLanguage);
            else
                sqlDB.AddVehicleInfoValue(VehicleId, VehicleInfoId, newValue, CurrentLanguage);
        }
    }
}
