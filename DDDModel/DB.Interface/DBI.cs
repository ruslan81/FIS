using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DB.Interface
{
    /// <summary>
    /// Интерфейс для SQLDB.cs
    /// </summary>
    public interface DBI
    {
        void OpenConnection();
        void OpenTransaction();
        void CommitConnection();
        void CloseConnection();
        void RollbackConnection();
        bool IsConnectionOpened();
        //-----------------------fn_data_block and fd_data_block_state ----
        int AddDataBlock(byte[] dataBlock);
        void DeleteDataBlock(int dataBlockId);
        int GetSTRIdDataBlockStateName(int dataBlockStateId);
        int GetDataBlockState(int BlockStateId);
        void SetDataBlockState(int dataBlockId, int dataBlockStateId);
        int SetDataBlockParseRecords(int dataBlockId);
        List<int> GetAllDataBlocksId(int UserId); // Доделать этот пункт, когда будет несколько пользователей!!!!
       // List<int> GetAllUnparsedDataBlockIDs(int UserId);
        List<int> GetAllParsedDataBlockIDs(int UserId);
        byte[] GetDataBlock(int dataBlockId);
        void SetDataBlock_CardId(int dataBlockId, int cardId);
        List<int> GetDataBlockIdsByCardId(int cardID);
        //-----------------------------------------------------------------
        //----------------------fn_data_record and fd_data_record_state ---
        int AddDataRecord(string name, string value, int dataBlockId);
        int AddDataRecord(string value, int dataBlockId, int paramId);
        bool DeleteDataRecord(int dataRecordId);
        void DeleteDataRecord(int dataBlockId, int paramId);
        int DeleteAllDataRecords(int dataBlockId);
        int DeleteAllDataRecordsFast(int dataBlockId);
        int GetDataRecordState(int dataRecordStateId);
        //-----------------------------------------------------------------
        //----------------------fd_param ---
        int AddParam(string name, int parentParamId, int paramSize);//создает новый или возвращает ID если уже существующего с таким именем.
        //byte[] GetParamIds(string paramName); //Берет по заданному имени параметра PARAM_ID и PARENT_PARAM_ID, если их нету - возврашает -1 
        int getParamId(string name);
        bool DeleteParam(int paramId);
        bool IsParamInDataBlock( int dataBlockId, string paramName);
        //-----------------------------------------------------------------
        //----------------------fd_string---
        string GetString(int stringId, string Language);//STRING_RU, STRING_RU
        int GetStringId(string stringValue, string Language, int owner);
        int GetStringId(string stringValue, int owner);
        int AddOrGetString(string EN_STRING, int owner);
        int AddOrGetString(string stringValue, string Language, int owner);
        int AddString(string stringValue, string Language, int owner);
        int AddString(string stringValue, int stringId, string Language, int owner);
        void TranslateString(string stringValue, string Language, int stringId);
        void DeleteString(int stringId);
        //int EditAnySTRIDValue(string newValue, string STRID_NAME, string Language, string tableName, string primaryName, int primaryValue);
        //----------------------------------
        int checkTableExistence(string tableName, string tablePrimaryKeyName, int tablePrimaryKeyId);
        DateTime SetCurrentTime(string tableName, string tablePrimaryKeyName, int tablePrimaryKeyId, string dateRowName);
        //---------------------------------User authetification and User TABLES---
        int GetUserAndPasswordCount(string userName, string password);
        int GetUserAndPasswordCount(string userName, string password, int orgId);
        string GetUserPassword(int userId);
        string GetUserName(int userId);
        int GetUserId_byUserName(string userName);
        int GetUserOrgId(int userId);
        string GetDateConnect(int userId);
        int GetUserTypeId(int userId);
        List<int> GetAllUsersId();
        List<int> GetAllUsersId(int orgId);
        List<int> GetAllUsersId(int orgId, int UserTypeId);
        int getUserRoleId(int userId);//fn_user_rights
        int GetUserTypeNameId(int UserTypeId);//fd_user_type
        int GetUserRoleNameId(int UserRoleId);//fd_user_role
        int GetUserTypeId_byUserTypeNameId(int UserTypeNameId);
        int GetUserRoleId_byUserRoleNameId(int UserRoleNameId);
        int GetUserRightsNameId(int UserRightsId);//fd_user_role
        int GetUserRightsId(int UserRoleId);//fn_role_rights
        List<KeyValuePair<string, int>> GetUserTypes(string Language);
        List<KeyValuePair<string, int>> GetUserRoles(string Language);
        int AddNewUser(string name, string pass, int userTypeId, int userRoleId, int orgId, string oldName, string oldPass);
        void DeleteUser(int UserId);
        //FD_USER_INFO_SET and FD_USER_INFO
        int GetUserInfoValueStrId(int userId, int UserInfoId);
        int GetUserInfoName(int InfoNameId);
        List<KeyValuePair<string, int>> GetAllUserInfoNames(string Language);
        int AddUserInfoName(string InfoName, string Language);
        void AddUserInfoValue(int userId, int UserInfoId, string value, string Language);
        void EditUserInfo(int userId, int UserInfoId, string newValue, string Language);
        int GetUserInfoUserId(int valueStrId);
        //---------------------------------FD_ORG and OrganizationTables------------------
        int GetOrgNameId(int ORG_ID);
        int GetOrgId_byOrgNameStr(string name, string language);
        void SetOrgName(string name, int ORG_ID, string Language);
        int GetOrgTypeNameId(int orgTypeId);
        int GetOrgCountryId(int orgId);
        int GetOrgRegionId(int orgId);
        int GetOrgTypeId(int orgId);
        int GetOrgId_byOrgNameId(int orgNameId);
        int GetOrgInfoId_bySTRID(int irgInfoStringId);
        void SetOrgCountryAndRegion(int orgId, int countryId, int regionId);
        List<int> GetAllOrganizationsId();
        List<int> GetAllOrganizationsId(int parentOrgId, int dealerTypeId, int subDealerId, int preDealerId);
        List<int> GetAllDealersId(int parentOrgId, int dealerTypeId, int subDealerId);
        List<KeyValuePair<string, int>> GetOrganizationNames(string Language);
        List<KeyValuePair<string, int>> GetOrgTypes(string Language);
        int AddNewOrganization(string newName, int orgTypeId, int orgCountryId, int orgRegionId, string oldName, string language);
        void SetParentOrganization(int orgId, int ParentOrgId);
        int GetOrgParentOrganization(int orgId);
        void AddAdditionalOrgInfo(int orgId, int ORG_INFO_ID, string value, string language);
        int GetOrgInfoName(int InfoNameId);
        int GetAdditionalOrgInfoValueId(int orgId, int ORG_INFO_ID);
        List<KeyValuePair<string, int>> GetAllOrgInfoIds(string Language);
        void DeleteOrgInfo(int orgInfoId);
        int AddNewOrgInfo(string Name);
        //---------------------------------COUNTRY AND REGION----------
        void AddCountryAndRegion(int countryNameId, int countryABBRId, byte[] countryFlagPic, List<KeyValuePair<int, int>> regionNames_short_long);
        int GetRegionLongNameId(int regionId);
        int GetRegionShortNameId(int regionId);
        int GetCountryId_byRegionId(int regionId);
        List<int> GetAllRegions_byCountryId(int countryId);
        int GetCountryABBRId(int countryId);
        int GetCountryNameId(int countryId);
        List<KeyValuePair<string, int>> GetAllCountry(string Language);
        List<KeyValuePair<string, int>> GetAllRegions(int countryId, string Language);
        byte[] GetCountryFlag(int countryId);
        //---------------------------------FD_VEHICLE and other vehicle Tables--------
        int AddNewVehicle(string GosNomer, string Marka, string VIN, int vehicleTypeId, int deviceId, int cardId, DateTime BLOCKED, int priority, string Language);
        void EditVehicle(int VehicleId, string GosNomer, string Marka, string VIN, int vehicleTypeId, int deviceId, DateTime BLOCKED, int priority, string Language);
        int GetVehicle_byCardId(int CardId);
        int GetVehicleCardId(int vehId);
        List<int> GetAllVehicleDataBlocks_byVehId(int vehId);
        string GetVehicleGOSNUM(int vehId);
        string GetVehicleVin(int vehId);
        int GetVehicleMARKAStrId(int vehId);
        int GetVehicleDeviceId(int vehId);
        int GetVehicleTypeId(int vehId);
        int GetVehiclePriority(int vehId);
        string GetVehicleDateBlocked(int vehId);
        void SetVehicleTypeId(int vehId, int vehTypeId);
        //vehType
        int GetVehTypeStrId(int VehTypeId);
        int GetVehTypeFuelType(int VehTypeId);
        List<KeyValuePair<string, int>> GetAllVehTypes(string Language);
        List<KeyValuePair<string, int>> GetAllVehFuelTypes(string Language);
        int GetVehFuelTypeStrId(int FuelTypeId);
        void DeleteFuelType(int fuelTypeId);
        void DeleteVehicleType(int VehicleTypeId);
        int AddNewFuelType(string Name);
        int GetFuelTypeId_byName(string Name, string Language);
        int AddNewVehicleType(string Name, int FuelTypeId);
        void EditNewVehicleType(int VehTypeId, string Name, int FuelTypeId, string Language);
        int GetVehicleTypeId_byName(string Name, string Language);
        int GetVehicleId_byVinRegNumbers(string vin, string regNumber);
        //criteria - measures
        int AddNewCriteria(int MeasureId, string Name, string Note, int minValue, int maxValue);
        int GetCriteriaId_byNameAndMeasureId(string Name, int MeasureId, string Language);
        int GetCriteriaId_byName(string Name, string Language);
        void EditCriteria(int keyId, int MeasureId, string Name, string Note, int minValue, int maxValue, string Language);
        int GetCriteriaNameId(int keyId);
        int GetCriteriaNoteId(int keyId);
        int GetCriteriaMinValue(int keyId);
        int GetCriteriaMaxValue(int keyId);
        int GetCriteriaMeasureId(int keyId);
        List<KeyValuePair<string, int>> GetAllCriteria_Name_n_Id(string Language);
        void DeleteCriteria(int keyId);
        int GetMeasureShortNameId(int MeasureId);
        int GetMeasureFullNameId(int MeasureId);
        List<int> GetAllMeasuresIds();
        int GetMeasureId_byFullName(string fullName, string Language);
        int AddNewMeasure(string shortName, string fullName);
        void DeleteMeasure(int measureId);
        void EditMeasure(int MeasureId, string shortName, string fullName, string Language);        
        //FD_VEHICLE_KEY
        List<int> GetAllVehicleKeyIDS(int vehicleId);
        int GetVehicleKey_KeyId(int vehicleKeyId);
        int GetVehicleKey_MinVal(int vehicleKeyId);
        int GetVehicleKey_MaxVal(int vehicleKeyId);
        string GetVehicleKey_BDate(int vehicleKeyId);
        string GetVehicleKey_EDate(int vehicleKeyId);
        int GetVehicleKey_NoteId(int vehicleKeyId);
        int AddVehicleKey(int vehicleId, int KeyId, int minVal, int maxVal, DateTime BDate, DateTime EDate, string note, string Language);
        void SetVehicleKey(int vehicleKeyId, int minVal, int maxVal, DateTime BDate, DateTime EDate, string note, string Language);
        int GetVehicleKeyId_ByVehIdAndKeyId(int VehId, int KeyNameId);
        //FD_VEHICLE_INFO_SET and FD_VEHICLE_INFO
        int GetVehicleInfoValueStrId(int vehicleId, int vehicleInfoId);
        int GetVehicleInfoName(int InfoNameId);
        List<KeyValuePair<string, int>> GetAllVehicleInfoNames(string Language);
        int AddVehicleInfoName(string InfoName, string Language);
        void AddVehicleInfoValue(int vehicleId, int vehicleInfoId, string value, string Language);
        void EditVehicleInfo(int vehicleId, int vehicleInfoId, string newValue, string Language);
        //-------------------------------Device tables------------
        //FD_DEVICE_TYPE
        List<KeyValuePair<string, int>> GetAllDeviceTypes(string Language);
        int GetDeviceTypeStrId(int DeviceTypeId);
        int AddNewDeviceType(string Name);
        int GetDeviceTypeId_byName(string Name, string Language);
        void DeleteDeviceType(int deviceTypeId);
        //FD_DEVICE
        int GetDeviceType(int deviceId);
        int GetDeviceNameId(int deviceId);
        string GetDeviceNum(int deviceId);
        string GetDeviceDateProduction(int deviceId);
        int GetDeviceFirmwareId(int deviceId);
        int GetDevicePhoneNumSim(int deviceId);
        int AddNewDevice(int deviceTypeId, string deviceName, string deviceNum, DateTime dateProduction, int firmwareId, int phoneNumSim);
        //FD_DEVICE_FIRMWARE
        int AddNewDeviceFirmware(string deviceModel, DateTime productionDate, string version, byte[] firmWare);
        string GetDeviceFirmware_deviceModel(int firmwareId);
        string GetDeviceFirmware_dateProduction(int firmwareId);
        string GetDeviceFirmware_version(int firmwareId);
        //---------------------------------FD_CARD--------
        int GetCardId(string cardHolderName, string cardNumber, int cardTypeId);
        int CreateNewCard(string cardHolderName, string cardNumber, int cardTypeId, int orgId, string CardNote, int groupID);
        int CreateNewCard(string cardHolderName, string cardNumber, int cardTypeId, int orgId, string CardNote, int UserId, int groupID);//При создании еще указывается ссылка на пользователя. Используется при создании водителей.
        List<int> GetAllCardIds(int orgId, int cardTypeId);
        string GetCardNumber(int cardId);
        string GetCardName(int cardId);
        void ChangeCardHolderName(string newName, int cardId);
        void ChangeCardNumber(string newNumber, int cardId);
        void DeleteCard(int cardId);
        string GetCardNote(int cardId);
        void SetCardNote(int cardId, string Note);
        int GetCardUserId(int cardId);
        //------------------------------HISTORY TABLES----------
        //FN_HISTORY
        DateTime AddHistoryRecord(int tableId, int TABLE_KEYFIELD_VALUE, int userId, int actionId, DateTime actionDate, string Note);
        List<List<KeyValuePair<string, string>>> GetHistoryActionIdAndTableId(int UserId, DateTime from, DateTime to, int actionId);
        string GetHistoryDate(int userId, int actionId, int tableId);
        string GetHistoryNote(int userId, int actionId, int tableId, DateTime historyDate);
        //FD_TABLE
        int GetTableId(string tableName);
        string GetTableName(int tableId);
        string GetTableKeyFieldName(int tableId);
        int GetTableNoteId(int tableId);
        int AddTable(string TableName, string TABLE_KEYFIELD_NAME, string TableNote, string Language); 
        //FD_ACTION
        List<KeyValuePair<string, int>> GetAllActions(string Language);
        int GetActionSrId(int actionId);
        int GetActionId(int actionStrId);
        int AddAction(string actionString, string Language);
        //-------------------------------INVOICE TABLES---------------
        //FD_INVOICE_STATUS
        int GetInvoiceStatusNameStrId(int invoiceStatusId);
        int GetInvoiceStatusId(int statusSTRID);
        int AddInvoiceStatus(string invoiceStatusName, string Language);
        //FD_INVOICE_TYPE
        int GetInvoiceTypeNameStrId(int invoiceTypeId);
        int GetInvoiceTypeId(int typeSTRID);
        int AddInvoiceType(string invoiceTypeName, string Language);
        //FN_INVOICE
        List<int> GetAllInvoices(int orgId);
        int AddInvoice(int invoiceTypeId, int invoiceStatusId, int orgId, string BillName, DateTime dateInvoice, DateTime datePaymentTerm, DateTime datePayment, string Language);
        void UpdateInvoice(int invoiceId, int statusId, DateTime payDate);//Если не оплачен - передавать просто new DateTime()
        int GetInvoice_TypeId(int invoiceId);
        int GetInvoice_StatusId(int invoiceId);
        int GetInvoice_OrgId(int invoiceId);
        int GetInvoice_BillNameStrId(int invoiceId);
        string GetInvoice_Date(int invoiceId);
        string GetInvoice_DatePaymentTerm(int invoiceId);
        string GetInvoice_DatePayment(int invoiceId);
        //-------------------------------REPORTS TABLES---------------------
        //FD_REPORT_TYPE
        int AddReportType(string reportName, string reportShortName, string reportFullName, string reportPrintName, string Language);
        List<KeyValuePair<string, int>> GetAllReportTypes(string language);
        int GetReportType_ReportNameStrId(int reportTypeId);
        int GetReportType_ReportShortNameStrId(int reportTypeId);
        int GetReportType_ReportFullNameStrId(int reportTypeId);
        int GetReportType_ReportPrintNameStrId(int reportTypeId);
        void EditReportType(int reportTypeId, string reportName, string reportShortName, string reportFullName, string reportPrintName, string Language);
        //FN_REPORT_USER
        int AddUserReport(int ReportTypeId, string reportUserName, DateTime dateCreate, DateTime dateUpdate, int Price, string Note, string Language);
        List<int> GetAllUserReportsIds();
        List<int> GetAllUserReportsIds(int reportTypeId);
        int GetUserReport_ReportTypeId(int reportUserId);
        int GetUserReport_ReportUserNameStrId(int reportUserId);
        string GetUserReport_DateCreate(int reportUserId);
        string GetUserReport_DateUpdate(int reportUserId);
        int GetUserReport_PRICE(int reportUserId);
        string GetUserReport_Note(int reportUserId);
        string GetUserReport_TemplateName(int reportUserId);
        //FN_REPORT_USER_ROLES
        List<int> GetAllUserRolesReportsId(int userRole, int orgId);
        string GetReportUserRoles_BDATE(int reportsUserId, int orgId, int userRole);
        string GetReportUserRoles_EDATE(int reportsUserId, int orgId, int userRole);
        string GetReportUserRoles_SETDATE(int reportsUserId, int orgId, int userRole);
        void AddOrSetReportUserRoles_SETDATE(int reportsUserId, int orgId, int userRole, bool IsActive);
        //FN_REPORT_USER_ORG
        List<int> GetAllUserOrgReportsId(int orgId);
        string GetReportUserOrg_BDATE(int reportsUserId, int orgId);
        string GetReportUserOrg_EDATE(int reportsUserId, int orgId);
        string GetReportUserOrg_SETDATE(int reportsUserId, int orgId);
        void AddOrSetReportUserOrg_SETDATE(int reportsUserId, int orgId, bool IsActive);
        //------------------------------Email Schedule------------------------------------
        int AddEmailSchedule(int orgId, int userId, int reportId, int cardId, int periodType, int formatType, int period, string emailAddress);
        void EditEmailSchedule(int sheduleId, int orgId, int userId, int reportId, int cardId, int period, int periodType, int formatType, string emailAddress);
        List<int> GetAllEmailScheduleIds();
        List<int> GetAllEmailScheduleIds(int orgId, int userId);
        /// <summary>
        /// Получает список дат последней отправки и период для каждого ID
        /// </summary>
        /// <param name="emailScheduleId">список ID записей отправки отчета на почту</param>
        /// <returns>список пар значений - Последняя дата отправки/(тип периода/период отправки)</returns>
        List<KeyValuePair<DateTime, KeyValuePair<int, int>>> GetEmailScheduleTimes(List<int> emailScheduleIds);
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
        List<object> GetAllEmailScheduleTable(int scheduleId);
        void DeleteEmailShedule(int sheduleId);
        void SetEmailSheduleLastSendDate(int sheduleId);
        //Email_Format
        //List<int> GetAllEmailExportFormat();
        /*int GetEmailExportFormatNameId();
        int AddEmailExportFormat(string formatName);*/
        //-----------------------------FORALL-------------------
        object GetOneParameter(int primaryId, string primaryKeyName, string tableName, string paramName);
        //INITALLBASE
        void DataBaseInit();
    }
}
