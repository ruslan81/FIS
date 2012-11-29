using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.Interface;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// работа с таблицами, описывающими установленное бортовое оборудование
    /// </summary>
    public class DeviceTable
    {
        /// <summary>
        /// Текущия язык
        /// </summary>
        private string CurrentLanguage;//STRING_RU,STRING_RUG etc.
        /// <summary>
        /// Строка подключения(в большинстве не нужна)
        /// </summary>
        private string connectionString;
        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        private SQLDB sqlDB;
        /// <summary>
        /// открывает подключение(лучше не использовать, а пользовать в DataBlock)
        /// </summary>
        public void OpenConnection()
        {
            sqlDB.OpenConnection();
        }
        /// <summary>
        /// Закрывает подключение
        /// </summary>
        public void CloseConnection()
        {
            sqlDB.CloseConnection();
        }
        //deviceTypes
        public DeviceTable(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            sqlDB = sql;
        }
        /// <summary>
        /// Получает все типы устройств
        /// </summary>
        /// <returns>Массив пар (название типа устройств, ID типа устройств)</returns>
        public List<KeyValuePair<string, int>> GetAllDeviceTypes()
        {
            sqlDB.OpenConnection();
            List<KeyValuePair<string, int>> allDeviceTypes = sqlDB.GetAllDeviceTypes(CurrentLanguage);
            sqlDB.CloseConnection();
            return allDeviceTypes;
        }
        /// <summary>
        /// Получить название типа устройств
        /// </summary>
        /// <param name="deviceTypeId">ID типа устройств</param>
        /// <returns>Имя типа устройств</returns>
        public string GetDeviceTypeName(int deviceTypeId)
        {
            int strId = sqlDB.GetDeviceTypeStrId(deviceTypeId);
            string returnValue = sqlDB.GetString(strId, CurrentLanguage);
            return returnValue;
        }
        /// <summary>
        /// Добавить новый тип устройств
        /// </summary>
        /// <param name="DeviceName">Имя нового типа устройств</param>
        /// <returns>ID типа устройств</returns>
        public int AddNewDeviceType(string DeviceName)
        {
            sqlDB.OpenConnection();
            int newDeviceId = sqlDB.AddNewDeviceType(DeviceName);
            sqlDB.CloseConnection();
            return newDeviceId;
        }
        /// <summary>
        /// Удалить тип устройств
        /// </summary>
        /// <param name="deviceTypeId">ID типа устройств</param>
        public void DeleteDeviceType(int deviceTypeId)
        {
            sqlDB.OpenConnection();
            sqlDB.DeleteDeviceType(deviceTypeId);
            sqlDB.CloseConnection();
        }

        //Devices
        /// <summary>
        /// Получить ID типа устройства
        /// </summary>
        /// <param name="deviceId">ID устройства</param>
        /// <returns>ID типа устройств</returns>
        public int GetDeviceType(int deviceId)
        {
           return sqlDB.GetDeviceType(deviceId);
        }
        /// <summary>
        /// Получить имя устройства
        /// </summary>
        /// <param name="deviceId">ID устройства</param>
        /// <returns>Имя устройства</returns>
        public string GetDeviceName(int deviceId)
        {
            int deviceNameId = sqlDB.GetDeviceNameId(deviceId);
            return sqlDB.GetString(deviceNameId, CurrentLanguage);
        }
        /// <summary>
        /// Получить номер устройства
        /// </summary>
        /// <param name="deviceId">ID устройства</param>
        /// <returns>Номер устройства</returns>
        public string GetDeviceNum(int deviceId)
        {
            return sqlDB.GetDeviceNum(deviceId);
        }
        /// <summary>
        /// Получить дату изготовления устройства
        /// </summary>
        /// <param name="deviceId">ID устройства</param>
        /// <returns>Дата изготовления устройства</returns>
        public DateTime GetDeviceDateProduction(int deviceId)
        {
            return DateTime.Parse(sqlDB.GetDeviceDateProduction(deviceId));
        }
        /// <summary>
        /// Получить ID ПО(прошивки) устройства
        /// </summary>
        /// <param name="deviceId">ID устройства</param>
        /// <returns>ID ПО(прошивки) устройства</returns>
        public int GetDeviceFirmwareId(int deviceId)
        {
            return sqlDB.GetDeviceFirmwareId(deviceId);
        }
        /// <summary>
        /// Получить номер сим-карты в устройстве
        /// </summary>
        /// <param name="deviceId">ID устройства</param>
        /// <returns>Номер сим-карты в устройстве</returns>
        public int GetDevicePhoneNumSim(int deviceId)
        {
            return sqlDB.GetDevicePhoneNumSim(deviceId);
        }
        /// <summary>
        /// Добавить новое устройство
        /// </summary>
        /// <param name="deviceTypeId">ID типа устройств</param>
        /// <param name="deviceName">Имя устройства</param>
        /// <param name="deviceNum">Номер устройства</param>
        /// <param name="dateProduction">Дата изготовления устройства</param>
        /// <param name="firmwareId">ID ПО(прошивки) устройства</param>
        /// <param name="phoneNumSim">Номер сим-карты в устройстве</param>
        /// <returns>ID устройства</returns>
        public int AddNewDevice(int deviceTypeId, string deviceName, string deviceNum, DateTime dateProduction, int firmwareId, int phoneNumSim)
        {
            int deviceId = sqlDB.AddNewDevice(deviceTypeId, deviceName, deviceNum, dateProduction, firmwareId, phoneNumSim);
            return deviceId;
        }

        //Firmware
        /// <summary>
        /// Добавить новую версию ПО(прошивки) для устройства
        /// </summary>
        /// <param name="deviceModel">Модель устройства</param>
        /// <param name="productionDate">Дата</param>
        /// <param name="version">Версия</param>
        /// <param name="firmWare">Битовый массив самой прошивки(он тоже сохраняется в базе данных)</param>
        /// <returns>ID ПО(прошивки)</returns>
        public int AddNewFirmware(string deviceModel, DateTime productionDate, string version, byte[] firmWare)
        {
            int firmwareId = sqlDB.AddNewDeviceFirmware(deviceModel, productionDate, version, firmWare);
            return firmwareId;
        }
        /// <summary>
        /// Получить модель устройства
        /// </summary>
        /// <param name="firmwareId"></param>
        /// <returns>Модель устройства</returns>
        public string GetDeviceFirmware_deviceModel(int firmwareId)
        {
            return sqlDB.GetDeviceFirmware_deviceModel(firmwareId);
        }
        /// <summary>
        /// Получить Дату производства прошивки
        /// </summary>
        /// <param name="firmwareId">ID ПО(прошивки)</param>
        /// <returns>Дата</returns>
        public DateTime GetDeviceFirmware_dateProduction(int firmwareId)
        {
            string date = sqlDB.GetDeviceFirmware_dateProduction(firmwareId);
            return DateTime.Parse(date);
        }
        /// <summary>
        /// Версия ПО(прошивки)
        /// </summary>
        /// <param name="firmwareId">ID ПО(прошивки)</param>
        /// <returns>Версия ПО(прошивки)</returns>
        public string GetDeviceFirmware_version(int firmwareId)
        {
           return sqlDB.GetDeviceFirmware_version(firmwareId);
        }
    }
}
