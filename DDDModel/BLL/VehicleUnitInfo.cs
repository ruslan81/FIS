using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// выгрузка ДДД данных для транспортного средства.
    /// </summary>
    public class VehicleUnitInfo
    {
        private string CurrentLanguage;//STRING_RU,STRING_RUG etc.
        private string connectionString;
        private SQLDB sqlDB { get; set; }

        public VehicleUnitInfo(string connectionsStringTMP, string Current_Language, SQLDB sqlTemp)
        {
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            sqlDB = sqlTemp;
        }
        //Vehicle_Overview
        public DDDClass.CardSlotsStatus Get_VehicleOverview_CardSlotsStatus(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            byte vehicleCardSlotsStatus_byte = Convert.ToByte(sqldbRecords.Get_VOverview_CardSlotsStatus(dataBlockId));
            DDDClass.CardSlotsStatus vehicleCardSlotsStatus = new DDDClass.CardSlotsStatus(vehicleCardSlotsStatus_byte);

            return vehicleCardSlotsStatus;
        }

        public DDDClass.TimeReal Get_VehicleOverview_CurrentDateTime(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            long timeRealSeconds = sqldbRecords.Get_VOverview_CurrentDateTime(dataBlockId);
            DDDClass.TimeReal CurrentDateTime = new DDDClass.TimeReal(timeRealSeconds);

            return CurrentDateTime;
        }

        public string Get_VehicleOverview_IdentificationNumber(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string IdentificationNumber = sqldbRecords.Get_VOverview_IdentificationNumber(dataBlockId);

            return IdentificationNumber;
        }

        private DDDClass.NationNumeric Get_VehicleOverview_RegistrationNation(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            DDDClass.NationNumeric RegistrationNation = new DDDClass.NationNumeric(sqldbRecords.Get_VOverview_RegistrationNation(dataBlockId));
            return RegistrationNation;
        }

        public DDDClass.VehicleRegistrationIdentification Get_VehicleOverview_RegistrationIdentification(int dataBlockId)
        {
            DDDClass.VehicleRegistrationIdentification vehicleRegistrationIdentification = new DDDClass.VehicleRegistrationIdentification();

            vehicleRegistrationIdentification.vehicleRegistrationNation = Get_VehicleOverview_RegistrationNation(dataBlockId);
            vehicleRegistrationIdentification.vehicleRegistrationNumber = Get_VehicleOverview_RegistrationNumber(dataBlockId);

            return vehicleRegistrationIdentification;
        }

        private DDDClass.VehicleRegistrationNumber Get_VehicleOverview_RegistrationNumber(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;
            DDDClass.VehicleRegistrationNumber vehicleRegistrationNumber = new DDDClass.VehicleRegistrationNumber();
            string RegistrationNumber = sqldbRecords.Get_VOverview_RegistrationNumber(dataBlockId);

            _bytes = enc.GetBytes(RegistrationNumber);
            vehicleRegistrationNumber.vehicleRegNumber = _bytes;

            return vehicleRegistrationNumber;
        }

        public List<DDDClass.VuControlActivityRecord> Get_VehicleOverview_VuControlActivityData(int dataBlockId)
        {//Тут еще надо дополнить инфо о карточке проверяющего!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName;
            byte[] _bytes;
            List<DDDClass.VuControlActivityRecord> vehicleOverview_VuControlActivityData = new List<DDDClass.VuControlActivityRecord>();

            List<string> controlTime = new List<string>();
            List<string> controlType = new List<string>();
            List<string> downloadPeriodBeginTime = new List<string>();
            List<string> downloadPeriodEndTime = new List<string>();

            paramName = "vehicleOverview.vuControlActivityData.vuControlActivityRecords.controlTime";
            controlTime = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleOverview.vuControlActivityData.vuControlActivityRecords.controlType";
            controlType = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleOverview.vuControlActivityData.vuControlActivityRecords.downloadPeriodBeginTime";
            downloadPeriodBeginTime = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleOverview.vuControlActivityData.vuControlActivityRecords.downloadPeriodEndTime";
            downloadPeriodEndTime = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            DDDClass.VuControlActivityRecord vuControlActivityRecord = new DDDClass.VuControlActivityRecord();

            if (controlTime.Count == controlType.Count && downloadPeriodEndTime.Count == downloadPeriodBeginTime.Count)
            {
                List<DDDClass.FullCardNumber> cardNumbers = new List<DDDClass.FullCardNumber>();
                cardNumbers = GetCardFullNumber(dataBlockId, "vehicleOverview.vuControlActivityData.vuControlActivityRecords.controlCardNumber");

                for (int i = 0; i < controlTime.Count; i++)
                {
                    vuControlActivityRecord = new DDDClass.VuControlActivityRecord();

                    vuControlActivityRecord.controlTime = new DDDClass.TimeReal(Convert.ToUInt32(controlTime[i]));
                    vuControlActivityRecord.downloadPeriodBeginTime = new DDDClass.TimeReal(Convert.ToUInt32(downloadPeriodBeginTime[i]));
                    vuControlActivityRecord.downloadPeriodEndTime = new DDDClass.TimeReal(Convert.ToUInt32(downloadPeriodEndTime[i]));

                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                    _bytes = enc.GetBytes(controlType[i]);
                    vuControlActivityRecord.controlType = new DDDClass.ControlType(_bytes);

                    vuControlActivityRecord.controlCardNumber = cardNumbers[i];

                    vehicleOverview_VuControlActivityData.Add(vuControlActivityRecord);
                }
            }
            return vehicleOverview_VuControlActivityData;
        }

        public DDDClass.VuDownloadablePeriod Get_VehicleOverview_VuDownloadablePeriod(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName;
            DDDClass.VuDownloadablePeriod vuDownloadablePeriod = new DDDClass.VuDownloadablePeriod();

            paramName = "vehicleOverview.vuDownloadablePeriod.minDownloadableTime";
            string minDownloadableTime = sqldbRecords.Get_ParamValue(dataBlockId, paramName);

            paramName = "vehicleOverview.vuDownloadablePeriod.maxDownloadableTime";
            string maxDownloadableTime = sqldbRecords.Get_ParamValue(dataBlockId, paramName);

            vuDownloadablePeriod.minDownloadableTime = new DDDClass.TimeReal(Convert.ToUInt32(minDownloadableTime));
            vuDownloadablePeriod.maxDownloadableTime = new DDDClass.TimeReal(Convert.ToUInt32(maxDownloadableTime));

            return vuDownloadablePeriod;
        }

        public DDDClass.VuDownloadActivityData Get_VehicleOverview_VuDownloadActivityData(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName;
            DDDClass.VuDownloadActivityData vuDownloadActivityData = new DDDClass.VuDownloadActivityData();

            paramName = "vehicleOverview.vuDownloadActivityData.companyOrWorkshopName.name";
            string companyOrWorkshopName = sqldbRecords.Get_ParamValue(dataBlockId, paramName);

            paramName = "vehicleOverview.vuDownloadActivityData.downloadingTime";
            string downloadingTime = sqldbRecords.Get_ParamValue(dataBlockId, paramName);

            DDDClass.FullCardNumber cardNumber = new DDDClass.FullCardNumber();
            cardNumber = GetCardFullNumber(dataBlockId, "vehicleOverview.vuDownloadActivityData.fullCardNumber")[0];


            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes = enc.GetBytes(companyOrWorkshopName);
            vuDownloadActivityData.companyOrWorkshopName.name = _bytes;

            vuDownloadActivityData.downloadingTime = new DDDClass.TimeReal(Convert.ToUInt32(downloadingTime));

            vuDownloadActivityData.fullCardNumber = cardNumber;

            return vuDownloadActivityData;
        }

        public List<DDDClass.VuCompanyLocksRecord> Get_VehicleOverview_VuCompanyLocksData(int dataBlockId)
        {
            DateTime startPeriod = new DateTime();
            DateTime endPeriod = new DateTime(2999, 1, 1);

            return Get_VehicleOverview_VuCompanyLocksData(dataBlockId, startPeriod, endPeriod);
        }

        public List<DDDClass.VuCompanyLocksRecord> Get_VehicleOverview_VuCompanyLocksData(int dataBlockId, DateTime startPeriod, DateTime endPeriod)
        {
            //Тут еще надо дополнить инфо о карточке Компании!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName;
            byte[] _bytes;
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            List<DDDClass.VuCompanyLocksRecord> vehicleOverview_VuCompanyLocksRecord = new List<DDDClass.VuCompanyLocksRecord>();
            DDDClass.VuCompanyLocksRecord vuCompanyLocksRecord = new DDDClass.VuCompanyLocksRecord();

            List<string> lockInTime = new List<string>();
            List<string> lockOutTime = new List<string>();
            List<string> companyName = new List<string>();
            List<string> companyAddress = new List<string>();

            paramName = "vehicleOverview.vuCompanyLocksData.vuCompanyLocksRecords.lockInTime";
            lockInTime = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleOverview.vuCompanyLocksData.vuCompanyLocksRecords.lockOutTime";
            lockOutTime = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleOverview.vuCompanyLocksData.vuCompanyLocksRecords.companyName.name";
            companyName = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleOverview.vuCompanyLocksData.vuCompanyLocksRecords.companyAddress.address";
            companyAddress = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            if (lockInTime.Count == lockOutTime.Count && companyName.Count == companyAddress.Count)
            {
                List<DDDClass.FullCardNumber> cardNumbers = new List<DDDClass.FullCardNumber>();
                cardNumbers = GetCardFullNumber(dataBlockId, "vehicleOverview.vuCompanyLocksData.vuCompanyLocksRecords.companyCardNumber");

                List<int> Indexes = new List<int>();                
                Indexes = CheckDate(lockInTime, lockOutTime, startPeriod, endPeriod);

                foreach (int i in Indexes)
                {
                    vuCompanyLocksRecord = new DDDClass.VuCompanyLocksRecord();

                    vuCompanyLocksRecord.lockInTime = new DDDClass.TimeReal(Convert.ToUInt32(lockInTime[i]));
                    vuCompanyLocksRecord.lockOutTime = new DDDClass.TimeReal(Convert.ToUInt32(lockOutTime[i]));

                    _bytes = enc.GetBytes(companyName[i]);
                    vuCompanyLocksRecord.companyName.name = _bytes;

                    _bytes = enc.GetBytes(companyAddress[i]);
                    vuCompanyLocksRecord.companyAddress.address = _bytes;

                    vuCompanyLocksRecord.companyCardNumber = cardNumbers[i];

                    vehicleOverview_VuCompanyLocksRecord.Add(vuCompanyLocksRecord);
                }
            }
            return vehicleOverview_VuCompanyLocksRecord;
        }

        //Vehicle_Technical_data
        public DDDClass.SensorPaired Get_VehicleTechnicalData_SensorPaired(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            DDDClass.SensorPaired sensorPaired = new DDDClass.SensorPaired();
            string paramName;

            paramName = "vehicleTechnicalData.sensorPaired.sensorApprovalNumber";
            string sensorApprovalNumber = sqldbRecords.Get_ParamValue(dataBlockId, paramName);

            paramName = "vehicleTechnicalData.sensorPaired.sensorPairingDateFirst";
            string sensorPairingDateFirst = sqldbRecords.Get_ParamValue(dataBlockId, paramName);

            List<DDDClass.ExtendedSerialNumber> sensorSerialNumber = new List<DDDClass.ExtendedSerialNumber>();
            sensorSerialNumber = GetExtendedSerialNumber(dataBlockId, "vehicleTechnicalData.sensorPaired.sensorSerialNumber");

            if (sensorSerialNumber.Count > 1)
                throw new Exception("Ошибка в VehicleTechnicalData! Несколько записей!");

            if (sensorSerialNumber.Count == 0)
                throw new Exception("Нет данных!");

            sensorPaired.sensorApprovalNumber = new DDDClass.SensorApprovalNumber(sensorApprovalNumber);
            sensorPaired.sensorPairingDateFirst = new DDDClass.SensorPairingDate(Convert.ToInt64(sensorPairingDateFirst));

            sensorPaired.sensorSerialNumber.manufacturerCode = sensorSerialNumber[0].manufacturerCode;
            sensorPaired.sensorSerialNumber.monthYear = sensorSerialNumber[0].monthYear;
            sensorPaired.sensorSerialNumber.serialNumber = sensorSerialNumber[0].serialNumber;
            sensorPaired.sensorSerialNumber.type = sensorSerialNumber[0].type;

            return sensorPaired;
        }

        public List<DDDClass.VuCalibrationRecord> Get_VehicleTechnicalData_VuCalibrationData(int dataBlockId)
        {
            DDDClass.VuCalibrationRecord vuCalibrationRecord = new DDDClass.VuCalibrationRecord();
            List<DDDClass.VuCalibrationRecord> VuCalibrationDataList = new List<DDDClass.VuCalibrationRecord>();
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string currentParamName;
            string paramName = "vehicleTechnicalData.vuCalibrationData.vuCalibrationRecords";
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            List<string> authorisedSpeed = new List<string>();
            List<string> calibrationPurpose = new List<string>();
            List<string> kConstantOfRecordingEquipment = new List<string>();
            List<string> lTyreCircumference = new List<string>();
            List<string> newOdometerValue = new List<string>();
            List<string> newTimeValue = new List<string>();
            List<string> nextCalibrationDate = new List<string>();
            List<string> oldOdometerValue = new List<string>();
            List<string> oldTimeValue = new List<string>();
            List<string> tyreSize = new List<string>();
            List<string> vehicleIdentificationNumber = new List<string>();
            List<DDDClass.VehicleRegistrationIdentification> vehicleRegistrationIdentification = new List<DDDClass.VehicleRegistrationIdentification>();
            List<string> workshopAddress = new List<string>();
            List<string> workshopCardExpiryDate = new List<string>();
            List<DDDClass.FullCardNumber> workshopCardNumber = new List<DDDClass.FullCardNumber>();
            List<string> workshopName = new List<string>();
            List<string> wVehicleCharacteristicConstant = new List<string>();

            currentParamName = paramName + ".authorisedSpeed";
            authorisedSpeed = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".calibrationPurpose";
            calibrationPurpose = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".kConstantOfRecordingEquipment";
            kConstantOfRecordingEquipment = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".lTyreCircumference";
            lTyreCircumference = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".newOdometerValue";
            newOdometerValue = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".newTimeValue";
            newTimeValue = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".nextCalibrationDate";
            nextCalibrationDate = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".oldOdometerValue";
            oldOdometerValue = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".oldTimeValue";
            oldTimeValue = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".tyreSize";
            tyreSize = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".vehicleIdentificationNumber";
            vehicleIdentificationNumber = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            vehicleRegistrationIdentification = GetVehicleRegistrationIdentification(dataBlockId, paramName + ".vehicleRegistrationIdentification");

            currentParamName = paramName + ".workshopAddress.address";
            workshopAddress = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".workshopCardExpiryDate";
            workshopCardExpiryDate = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            workshopCardNumber = GetCardFullNumber(dataBlockId, paramName + ".workshopCardNumber");

            currentParamName = paramName + ".workshopName.name";
            workshopName = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".wVehicleCharacteristicConstant";
            wVehicleCharacteristicConstant = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            if (wVehicleCharacteristicConstant.Count == workshopAddress.Count && oldTimeValue.Count == kConstantOfRecordingEquipment.Count)
            {
                for (int i = 0; i < calibrationPurpose.Count; i++)
                {
                    vuCalibrationRecord = new DDDClass.VuCalibrationRecord();

                    vuCalibrationRecord.authorisedSpeed.speed = Convert.ToInt16(authorisedSpeed[i]);
                    vuCalibrationRecord.calibrationPurpose = new DDDClass.CalibrationPurpose(Convert.ToByte(calibrationPurpose[i]));
                    vuCalibrationRecord.kConstantOfRecordingEquipment.kConstantOfRecordingEquipment = Convert.ToInt32(kConstantOfRecordingEquipment[i]);
                    vuCalibrationRecord.lTyreCircumference.lTyreCircumference = Convert.ToInt32(lTyreCircumference[i]);
                    vuCalibrationRecord.newOdometerValue.odometerShort = Convert.ToInt32(newOdometerValue[i]);
                    vuCalibrationRecord.newTimeValue = new DDDClass.TimeReal(Convert.ToInt64(newTimeValue[i]));
                    vuCalibrationRecord.nextCalibrationDate = new DDDClass.TimeReal(Convert.ToInt64(nextCalibrationDate[i]));
                    vuCalibrationRecord.oldOdometerValue.odometerShort = Convert.ToInt32(oldOdometerValue[i]);
                    vuCalibrationRecord.oldTimeValue = new DDDClass.TimeReal(Convert.ToInt64(oldTimeValue[i]));
                    vuCalibrationRecord.tyreSize = new DDDClass.TyreSize(tyreSize[i]);
                    vuCalibrationRecord.vehicleIdentificationNumber.vehicleIdentificationNumber = vehicleIdentificationNumber[i];
                    vuCalibrationRecord.vehicleRegistrationIdentification = vehicleRegistrationIdentification[i];

                    _bytes = enc.GetBytes(workshopAddress[i]);
                    vuCalibrationRecord.workshopAddress.address = _bytes;

                    vuCalibrationRecord.workshopCardExpiryDate = new DDDClass.TimeReal(Convert.ToInt64(workshopCardExpiryDate[i]));
                    vuCalibrationRecord.workshopCardNumber = workshopCardNumber[i];

                    _bytes = enc.GetBytes(workshopName[i]);
                    vuCalibrationRecord.workshopName.name = _bytes;

                    vuCalibrationRecord.wVehicleCharacteristicConstant.wVehicleCharacteristicConstant = Convert.ToInt32(wVehicleCharacteristicConstant[i]);

                    VuCalibrationDataList.Add(vuCalibrationRecord);
                }
            }
            return VuCalibrationDataList;
        }

        public DDDClass.VuIdentification Get_VehicleTechnicalData_VuIdentification(int dataBlockId)
        {
            DDDClass.VuIdentification vuIdentification = new DDDClass.VuIdentification();
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "vehicleTechnicalData.vuIdentification";
            string currentParamName;
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            currentParamName = paramName + ".vuApprovalNumber";
            string vuApprovalNumber = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".vuManufacturerAddress.address";
            string vuManufacturerAddress = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".vuManufacturerName.name";
            string vuManufacturerName = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".vuManufacturingDate";
            string vuManufacturingDate = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".vuPartNumber";
            string vuPartNumber = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            List<DDDClass.ExtendedSerialNumber> vuSerialNumber = new List<DDDClass.ExtendedSerialNumber>();
            vuSerialNumber = GetExtendedSerialNumber(dataBlockId, paramName + ".vuSerialNumber");

            currentParamName = paramName + ".vuSoftwareIdentification.vuSoftInstallationDate";
            string vuSoftInstallationDate = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".vuSoftwareIdentification.vuSoftwareVersion";
            string vuSoftwareVersion = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            if (vuSerialNumber.Count > 1)
                throw new Exception("Ошибка в VehicleTechnicalData! Несколько записей вместо одной!");

            if (vuSerialNumber.Count == 0)
                throw new Exception("Нет данных!");


            vuIdentification.vuApprovalNumber.vuApprovalNumber = vuApprovalNumber;

            _bytes = enc.GetBytes(vuManufacturerAddress);
            vuIdentification.vuManufacturerAddress.address = _bytes;

            _bytes = enc.GetBytes(vuManufacturerName);
            vuIdentification.vuManufacturerName.name = _bytes;

            vuIdentification.vuManufacturingDate.timereal = Convert.ToInt64(vuManufacturingDate);
            vuIdentification.vuPartNumber = new DDDClass.VuPartNumber(vuPartNumber);

            vuIdentification.vuSerialNumber.manufacturerCode = vuSerialNumber[0].manufacturerCode;
            vuIdentification.vuSerialNumber.monthYear = vuSerialNumber[0].monthYear;
            vuIdentification.vuSerialNumber.serialNumber = vuSerialNumber[0].serialNumber;
            vuIdentification.vuSerialNumber.type = vuSerialNumber[0].type;

            vuIdentification.vuSoftwareIdentification.vuSoftInstallationDate = new DDDClass.VuSoftInstallationDate(Convert.ToInt64(vuSoftInstallationDate));
            vuIdentification.vuSoftwareIdentification.vuSoftwareVersion.vuSoftwareVersion = vuSoftwareVersion;

            return vuIdentification;
        }
        //Vehicle_Events_And_Faults
        public List<DDDClass.VuOverSpeedingEventRecord> Get_VehicleEventsAndFaults_VuOverSpeedingEventData(int dataBlockId)
        {
            DateTime startPeriod = new DateTime();
            DateTime endPeriod = new DateTime(2999, 1, 1);

            return Get_VehicleEventsAndFaults_VuOverSpeedingEventData(dataBlockId, startPeriod, endPeriod);
        }
        private List<DDDClass.VuOverSpeedingEventRecord> Get_VehicleEventsAndFaults_VuOverSpeedingEventData(int dataBlockId, DateTime startPeriod, DateTime endPeriod)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName;

            List<DDDClass.VuOverSpeedingEventRecord> vuOverSpeedingEvents = new List<DDDClass.VuOverSpeedingEventRecord>();
            List<string> averageSpeedValue = new List<string>();
            List<string> eventBeginTime = new List<string>();
            List<string> eventEndTime = new List<string>();
            List<string> eventRecordPurpose = new List<string>();
            List<string> maxSpeedValue = new List<string>();
            List<string> eventType = new List<string>();
            List<DDDClass.FullCardNumber> cardNumberDriverSlotBegin = new List<DDDClass.FullCardNumber>();

            paramName = "vehicleEventsAndFaults.vuOverSpeedingEventData.vuOverSpeedingEventRecords.averageSpeedValue";
            averageSpeedValue = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleEventsAndFaults.vuOverSpeedingEventData.vuOverSpeedingEventRecords.eventBeginTime";
            eventBeginTime = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleEventsAndFaults.vuOverSpeedingEventData.vuOverSpeedingEventRecords.eventEndTime";
            eventEndTime = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleEventsAndFaults.vuOverSpeedingEventData.vuOverSpeedingEventRecords.eventRecordPurpose";
            eventRecordPurpose = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleEventsAndFaults.vuOverSpeedingEventData.vuOverSpeedingEventRecords.eventType";
            eventType = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleEventsAndFaults.vuOverSpeedingEventData.vuOverSpeedingEventRecords.maxSpeedValue";
            maxSpeedValue = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);

            paramName = "vehicleEventsAndFaults.vuOverSpeedingEventData.vuOverSpeedingEventRecords.cardNumberDriverSlotBegin";
            cardNumberDriverSlotBegin = GetCardFullNumber(dataBlockId, paramName);

            DDDClass.VuOverSpeedingEventRecord eventRecord;

            if (averageSpeedValue.Count == maxSpeedValue.Count && cardNumberDriverSlotBegin.Count == eventRecordPurpose.Count)
            {
                List<DDDClass.FullCardNumber> cardNumbers = new List<DDDClass.FullCardNumber>();
                cardNumbers = GetCardFullNumber(dataBlockId, "vehicleEventsAndFaults.vuOverSpeedingEventData.vuOverSpeedingEventRecords.cardNumberDriverSlotBegin");

                List<int> Indexes = new List<int>();
                Indexes = CheckDate(eventBeginTime, eventEndTime, startPeriod, endPeriod);

                foreach (int i in Indexes)
                {
                    eventRecord = new DDDClass.VuOverSpeedingEventRecord();
                    eventRecord.averageSpeedValue.speed = Convert.ToInt16(averageSpeedValue[i]);
                    eventRecord.eventBeginTime = new DDDClass.TimeReal(Convert.ToUInt32(eventBeginTime[i]));
                    eventRecord.eventEndTime = new DDDClass.TimeReal(Convert.ToUInt32(eventEndTime[i]));
                    eventRecord.eventRecordPurpose.eventFaultRecordPurpose = Convert.ToByte(eventRecordPurpose[i]);
                    eventRecord.eventType.eventFaultType = Convert.ToByte(eventType[i]);
                    eventRecord.maxSpeedValue.speed = Convert.ToInt16(maxSpeedValue[i]);
                    eventRecord.cardNumberDriverSlotBegin = cardNumbers[i];

                    eventRecord.cardNumberDriverSlotBegin.cardIssuingMemberState = cardNumberDriverSlotBegin[i].cardIssuingMemberState;
                    eventRecord.cardNumberDriverSlotBegin.cardNumber = cardNumberDriverSlotBegin[i].cardNumber;
                    eventRecord.cardNumberDriverSlotBegin.cardType = cardNumberDriverSlotBegin[i].cardType;

                    vuOverSpeedingEvents.Add(eventRecord);
                }
            }
            else throw new Exception("Ошибка, не могу извлечь информацию из базы");
            vuOverSpeedingEvents.Sort(VuOverSpeedingEventDataComparison);
            return vuOverSpeedingEvents;
        }
        public List<DDDClass.VuOverSpeedingEventRecord> Get_VehicleEventsAndFaults_VuOverSpeedingEventData(List<int> dataBlockIDS, DateTime startPeriod, DateTime endPeriod)
        {
            List<DDDClass.VuOverSpeedingEventRecord> records = new List<DDDClass.VuOverSpeedingEventRecord>();
            List<int> dataBlockIdsToGet = new List<int>();
            DateTime fromTemp = new DateTime();
            DateTime toTemp = new DateTime();
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            List<DateTime> startEndPeriod = new List<DateTime>();

            foreach (int dataBlock in dataBlockIDS)
            {
                startEndPeriod = Get_StartEndPeriod(dataBlock);
                fromTemp = startEndPeriod[0];
                toTemp = startEndPeriod[1];
                if (fromTemp.Date >= startPeriod && fromTemp.Date <= endPeriod)
                {
                    dataBlockIdsToGet.Add(dataBlock);
                    continue;
                }
                if (toTemp.Date >= startPeriod && toTemp.Date <= endPeriod)
                {
                    dataBlockIdsToGet.Add(dataBlock);
                    continue;
                }
                if (startPeriod >= fromTemp.Date && endPeriod <= toTemp.Date)
                {
                    dataBlockIdsToGet.Add(dataBlock);
                    continue;
                }
            }

            foreach (int id in dataBlockIdsToGet)
            {
                records.AddRange(Get_VehicleEventsAndFaults_VuOverSpeedingEventData(id, startPeriod, endPeriod));
            }

            records.Sort(VuOverSpeedingEventDataComparison);
            return records;
        }

        public List<DDDClass.VuFaultRecord> Get_VehicleEventsAndFaults_VuFaultData(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "vehicleEventsAndFaults.vuFaultData.vuFaultRecords";
            string currentParamName;

            List<DDDClass.VuFaultRecord> VuFaultData = new List<DDDClass.VuFaultRecord>();
            DDDClass.VuFaultRecord vuFaultRecord = new DDDClass.VuFaultRecord();

            List<DDDClass.FullCardNumber> cardNumberCodriverSlotBegin = new List<DDDClass.FullCardNumber>();
            List<DDDClass.FullCardNumber> cardNumberCodriverSlotEnd = new List<DDDClass.FullCardNumber>();
            List<DDDClass.FullCardNumber> cardNumberDriverSlotBegin = new List<DDDClass.FullCardNumber>();
            List<DDDClass.FullCardNumber> cardNumberDriverSlotEnd = new List<DDDClass.FullCardNumber>();
            List<string> faultBeginTime = new List<string>();
            List<string> faultEndTime = new List<string>();
            List<string> faultRecordPurpose = new List<string>();
            List<string> faultType = new List<string>();

            currentParamName = paramName + ".cardNumberCodriverSlotBegin";
            cardNumberCodriverSlotBegin = GetCardFullNumber(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumberCodriverSlotEnd";
            cardNumberCodriverSlotEnd = GetCardFullNumber(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumberDriverSlotBegin";
            cardNumberDriverSlotBegin = GetCardFullNumber(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumberDriverSlotEnd";
            cardNumberDriverSlotEnd = GetCardFullNumber(dataBlockId, currentParamName);

            currentParamName = paramName + ".faultBeginTime";
            faultBeginTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".faultEndTime";
            faultEndTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".faultRecordPurpose";
            faultRecordPurpose = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".faultType";
            faultType = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            if (faultEndTime.Count == faultType.Count && cardNumberCodriverSlotBegin.Count == faultBeginTime.Count)
            {
                for (int i = 0; i < faultEndTime.Count; i++)
                {
                    vuFaultRecord = new DDDClass.VuFaultRecord();

                    vuFaultRecord.cardNumberCodriverSlotBegin.cardIssuingMemberState = cardNumberCodriverSlotBegin[i].cardIssuingMemberState;
                    vuFaultRecord.cardNumberCodriverSlotBegin.cardNumber = cardNumberCodriverSlotBegin[i].cardNumber;
                    vuFaultRecord.cardNumberCodriverSlotBegin.cardType = cardNumberCodriverSlotBegin[i].cardType;

                    vuFaultRecord.cardNumberCodriverSlotEnd.cardIssuingMemberState = cardNumberCodriverSlotEnd[i].cardIssuingMemberState;
                    vuFaultRecord.cardNumberCodriverSlotEnd.cardNumber = cardNumberCodriverSlotEnd[i].cardNumber;
                    vuFaultRecord.cardNumberCodriverSlotEnd.cardType = cardNumberCodriverSlotEnd[i].cardType;

                    vuFaultRecord.cardNumberDriverSlotBegin.cardIssuingMemberState = cardNumberDriverSlotBegin[i].cardIssuingMemberState;
                    vuFaultRecord.cardNumberDriverSlotBegin.cardNumber = cardNumberDriverSlotBegin[i].cardNumber;
                    vuFaultRecord.cardNumberDriverSlotBegin.cardType = cardNumberDriverSlotBegin[i].cardType;

                    vuFaultRecord.cardNumberDriverSlotEnd.cardIssuingMemberState = cardNumberDriverSlotEnd[i].cardIssuingMemberState;
                    vuFaultRecord.cardNumberDriverSlotEnd.cardNumber = cardNumberDriverSlotEnd[i].cardNumber;
                    vuFaultRecord.cardNumberDriverSlotEnd.cardType = cardNumberDriverSlotEnd[i].cardType;

                    vuFaultRecord.faultBeginTime = new DDDClass.TimeReal(Convert.ToInt64(faultBeginTime[i]));
                    vuFaultRecord.faultEndTime = new DDDClass.TimeReal(Convert.ToInt64(faultEndTime[i]));
                    vuFaultRecord.faultRecordPurpose = new DDDClass.EventFaultRecordPurpose(Convert.ToByte(faultRecordPurpose[i]));
                    vuFaultRecord.faultType = new DDDClass.EventFaultType(Convert.ToByte(faultType[i]));

                    VuFaultData.Add(vuFaultRecord);
                }
            }
            else throw new Exception("Ошибка, не могу извлечь информацию из базы");

            return VuFaultData;
        }

        public List<DDDClass.VuEventRecord> Get_VehicleEventsAndFaults_VuEventData(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "vehicleEventsAndFaults.vuEventData.vuEventRecords";
            string currentParamName;
            List<DDDClass.VuEventRecord> vuEventData = new List<DDDClass.VuEventRecord>();
            DDDClass.VuEventRecord vuEventRecord = new DDDClass.VuEventRecord();

            List<DDDClass.FullCardNumber> cardNumberCodriverSlotBegin = new List<DDDClass.FullCardNumber>();
            List<DDDClass.FullCardNumber> cardNumberCodriverSlotEnd = new List<DDDClass.FullCardNumber>();
            List<DDDClass.FullCardNumber> cardNumberDriverSlotBegin = new List<DDDClass.FullCardNumber>();
            List<DDDClass.FullCardNumber> cardNumberDriverSlotEnd = new List<DDDClass.FullCardNumber>();
            List<string> eventBeginTime = new List<string>();
            List<string> eventEndTime = new List<string>();
            List<string> eventRecordPurpose = new List<string>();
            List<string> eventType = new List<string>();
            List<string> similarEventsNumber = new List<string>();

            currentParamName = paramName + ".cardNumberCodriverSlotBegin";
            cardNumberCodriverSlotBegin = GetCardFullNumber(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumberCodriverSlotEnd";
            cardNumberCodriverSlotEnd = GetCardFullNumber(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumberDriverSlotBegin";
            cardNumberDriverSlotBegin = GetCardFullNumber(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumberDriverSlotEnd";
            cardNumberDriverSlotEnd = GetCardFullNumber(dataBlockId, currentParamName);

            currentParamName = paramName + ".eventBeginTime";
            eventBeginTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".eventEndTime";
            eventEndTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".eventRecordPurpose";
            eventRecordPurpose = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".eventType";
            eventType = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".similarEventsNumber";
            similarEventsNumber = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            if (eventBeginTime.Count == eventRecordPurpose.Count && cardNumberCodriverSlotBegin.Count == cardNumberCodriverSlotEnd.Count)
            {
                for (int i = 0; i < eventBeginTime.Count; i++)
                {
                    vuEventRecord = new DDDClass.VuEventRecord();

                    vuEventRecord.cardNumberCodriverSlotBegin.cardIssuingMemberState = cardNumberCodriverSlotBegin[i].cardIssuingMemberState;
                    vuEventRecord.cardNumberCodriverSlotBegin.cardNumber = cardNumberCodriverSlotBegin[i].cardNumber;
                    vuEventRecord.cardNumberCodriverSlotBegin.cardType = cardNumberCodriverSlotBegin[i].cardType;

                    vuEventRecord.cardNumberCodriverSlotEnd.cardIssuingMemberState = cardNumberCodriverSlotEnd[i].cardIssuingMemberState;
                    vuEventRecord.cardNumberCodriverSlotEnd.cardNumber = cardNumberCodriverSlotEnd[i].cardNumber;
                    vuEventRecord.cardNumberCodriverSlotEnd.cardType = cardNumberCodriverSlotEnd[i].cardType;

                    vuEventRecord.cardNumberDriverSlotBegin.cardIssuingMemberState = cardNumberDriverSlotBegin[i].cardIssuingMemberState;
                    vuEventRecord.cardNumberDriverSlotBegin.cardNumber = cardNumberDriverSlotBegin[i].cardNumber;
                    vuEventRecord.cardNumberDriverSlotBegin.cardType = cardNumberDriverSlotBegin[i].cardType;

                    vuEventRecord.cardNumberDriverSlotEnd.cardIssuingMemberState = cardNumberDriverSlotEnd[i].cardIssuingMemberState;
                    vuEventRecord.cardNumberDriverSlotEnd.cardNumber = cardNumberDriverSlotEnd[i].cardNumber;
                    vuEventRecord.cardNumberDriverSlotEnd.cardType = cardNumberDriverSlotEnd[i].cardType;

                    vuEventRecord.eventBeginTime = new DDDClass.TimeReal(Convert.ToInt64(eventBeginTime[i]));
                    vuEventRecord.eventEndTime = new DDDClass.TimeReal(Convert.ToInt64(eventEndTime[i]));
                    vuEventRecord.eventRecordPurpose = new DDDClass.EventFaultRecordPurpose(Convert.ToByte(eventRecordPurpose[i]));
                    vuEventRecord.eventType = new DDDClass.EventFaultType(Convert.ToByte(eventType[i]));
                    vuEventRecord.similarEventsNumber = new DDDClass.SimilarEventsNumber(Convert.ToByte(similarEventsNumber[i]));

                    vuEventData.Add(vuEventRecord);
                }
            }
            else throw new Exception("Ошибка, не могу извлечь информацию из базы");

            return vuEventData;
        }

        public DDDClass.VuOverSpeedingControlData Get_VehicleEventsAndFaults_VuOverSpeedingControlData(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "vehicleEventsAndFaults.vuOverSpeedingControlData";
            string currentParamName;
            DDDClass.VuOverSpeedingControlData vuOverSpeedingControlData = new DDDClass.VuOverSpeedingControlData();

            string firstOverspeedSince;
            string lastOverspeedControlTime;
            string numberOfOverspeedSince;

            currentParamName = paramName + ".firstOverspeedSince";
            firstOverspeedSince = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".lastOverspeedControlTime";
            lastOverspeedControlTime = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".numberOfOverspeedSince";
            numberOfOverspeedSince = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);


            vuOverSpeedingControlData.firstOverspeedSince = new DDDClass.TimeReal(Convert.ToInt64(firstOverspeedSince));
            vuOverSpeedingControlData.lastOverspeedControlTime = new DDDClass.TimeReal(Convert.ToInt64(lastOverspeedControlTime));
            vuOverSpeedingControlData.numberOfOverspeedSince = new DDDClass.OverspeedNumber(Convert.ToByte(numberOfOverspeedSince));

            return vuOverSpeedingControlData;
        }

        public List<DDDClass.VuTimeAdjustmentRecord> Get_VehicleEventsAndFaults_VuTimeAdjustmentData(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "vehicleEventsAndFaults.vuTimeAdjustmentData.vuTimeAdjustmentRecords";
            string currentParamName;
            List<DDDClass.VuTimeAdjustmentRecord> vuTimeAdjustmentData = new List<DDDClass.VuTimeAdjustmentRecord>();
            DDDClass.VuTimeAdjustmentRecord vuTimeAdjustmentRecord = new DDDClass.VuTimeAdjustmentRecord();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            List<string> newTimeValue = new List<string>();
            List<string> oldTimeValue = new List<string>();
            List<string> workshopAddress = new List<string>();
            List<DDDClass.FullCardNumber> workshopCardNumber = new List<DDDClass.FullCardNumber>();
            List<string> workshopName = new List<string>();

            currentParamName = paramName + ".newTimeValue";
            newTimeValue = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".oldTimeValue";
            oldTimeValue = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".workshopAddress.address";
            workshopAddress = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".workshopName.name";
            workshopName = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".workshopCardNumber";
            workshopCardNumber = GetCardFullNumber(dataBlockId, currentParamName);

            if (newTimeValue.Count == oldTimeValue.Count && workshopName.Count == workshopCardNumber.Count)
            {
                for (int i = 0; i < newTimeValue.Count; i++)
                {
                    vuTimeAdjustmentRecord = new DDDClass.VuTimeAdjustmentRecord();

                    vuTimeAdjustmentRecord.workshopCardNumber.cardIssuingMemberState = workshopCardNumber[i].cardIssuingMemberState;
                    vuTimeAdjustmentRecord.workshopCardNumber.cardNumber = workshopCardNumber[i].cardNumber;
                    vuTimeAdjustmentRecord.workshopCardNumber.cardType = workshopCardNumber[i].cardType;

                    vuTimeAdjustmentRecord.newTimeValue = new DDDClass.TimeReal(Convert.ToInt64(newTimeValue[i]));
                    vuTimeAdjustmentRecord.oldTimeValue = new DDDClass.TimeReal(Convert.ToInt64(oldTimeValue[i]));

                    _bytes = enc.GetBytes(workshopAddress[i]);
                    vuTimeAdjustmentRecord.workshopAddress.address = _bytes;

                    _bytes = enc.GetBytes(workshopName[i]);
                    vuTimeAdjustmentRecord.workshopName.name = _bytes;

                    vuTimeAdjustmentData.Add(vuTimeAdjustmentRecord);
                }
            }
            else throw new Exception("Ошибка, не могу извлечь информацию из базы");

            return vuTimeAdjustmentData;
        }
        //Vehicle_Detailed_Speed
        public List<DDDClass.VuDetailedSpeedBlock> Get_VehicleDetailedSpeed_VuDetailedSpeedData(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "vehicleDetailedSpeed.vuDetailedSpeedBlocks";
            string currentParamName;

            List<DDDClass.VuDetailedSpeedBlock> vuDetailedSpeedData = new List<DDDClass.VuDetailedSpeedBlock>();
            DDDClass.VuDetailedSpeedBlock vuDetailedSpeedBlock = new DDDClass.VuDetailedSpeedBlock();

            List<string> speedBlockBeginDate = new List<string>();
            List<string> speedsPerSecond = new List<string>();
          
            currentParamName = paramName + ".speedBlockBeginDate";
            speedBlockBeginDate = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".speedsPerSecond";
            speedsPerSecond = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            int speedIndex = 0;

            if (speedBlockBeginDate.Count == (speedsPerSecond.Count / 60))
            {
                for (int i = 0; i < speedBlockBeginDate.Count; i++)
                {
                    vuDetailedSpeedBlock = new DDDClass.VuDetailedSpeedBlock();

                    vuDetailedSpeedBlock.speedBlockBeginDate = new DDDClass.TimeReal(Convert.ToInt64(speedBlockBeginDate[i]));

                    for (int j = 0; j < 60; j++)
                    {
                        vuDetailedSpeedBlock.speedsPerSecond[j] = new DDDClass.Speed();
                        vuDetailedSpeedBlock.speedsPerSecond[j].speed = Convert.ToInt16(speedsPerSecond[speedIndex]);
                        speedIndex++;
                    }
                    vuDetailedSpeedData.Add(vuDetailedSpeedBlock);
                }
            }
            else throw new Exception("Ошибка, извлечение скорости ТС прошло с ошибками!");

            return vuDetailedSpeedData;
        }

        public List<DDDClass.VuDetailedSpeedBlock> Get_VehicleDetailedSpeed_VuDetailedSpeedData(int dataBlockId, DateTime startPeriod, DateTime endPeriod)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "vehicleDetailedSpeed.vuDetailedSpeedBlocks";
            string currentParamName;

            List<DDDClass.VuDetailedSpeedBlock> vuDetailedSpeedData = new List<DDDClass.VuDetailedSpeedBlock>();
            DDDClass.VuDetailedSpeedBlock vuDetailedSpeedBlock = new DDDClass.VuDetailedSpeedBlock();

            List<string> speedBlockBeginDate = new List<string>();
            List<string> speedsPerSecond = new List<string>();           

            currentParamName = paramName + ".speedBlockBeginDate";
            speedBlockBeginDate = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".speedsPerSecond";
            speedsPerSecond = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            int speedIndex = 0;

            if (speedBlockBeginDate.Count == (speedsPerSecond.Count / 60) && speedBlockBeginDate.Count!=0)
            {
                List<int> Indexes = new List<int>();
                
                Indexes = CheckDate(speedBlockBeginDate, speedBlockBeginDate, startPeriod, endPeriod);

                foreach (int i in Indexes)
                {
                    vuDetailedSpeedBlock = new DDDClass.VuDetailedSpeedBlock();

                    vuDetailedSpeedBlock.speedBlockBeginDate = new DDDClass.TimeReal(Convert.ToInt64(speedBlockBeginDate[i]));

                    for (int j = 0; j < 60; j++)
                    {
                        vuDetailedSpeedBlock.speedsPerSecond[j] = new DDDClass.Speed();
                        vuDetailedSpeedBlock.speedsPerSecond[j].speed = Convert.ToInt16(speedsPerSecond[speedIndex]);
                        speedIndex++;
                    }
                    vuDetailedSpeedData.Add(vuDetailedSpeedBlock);
                }
            }
            else throw new Exception("Ошибка, извлечение скорости ТС прошло с ошибками!");

            return vuDetailedSpeedData;
        }
        //Vehicle_Activities
        public List<DDDClass.TimeReal> Get_VehicleActivities_DownloadedDayDate(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName;
            DDDClass.TimeReal downloadedDayDate = new DDDClass.TimeReal();
            List<DDDClass.TimeReal> downloadedDayDateList = new List<DDDClass.TimeReal>();
            List<string> downloadedDayDateStringList = new List<string>();

            paramName = "vehicleActivities.downloadedDayDate";
            downloadedDayDateStringList = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);
          
            for (int i = 0; i < downloadedDayDateStringList.Count; i++)
            {
                downloadedDayDate = new DDDClass.TimeReal(Convert.ToInt64(downloadedDayDateStringList[i]));
                downloadedDayDateList.Add(downloadedDayDate);
            }

            return downloadedDayDateList;
        }

        public List<DDDClass.OdometerShort> Get_VehicleActivities_OdoMeterValueMidnight(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName;
            DDDClass.OdometerShort odoMeterValueMidnight = new DDDClass.OdometerShort();
            List<DDDClass.OdometerShort> odoMeterValueMidnightList = new List<DDDClass.OdometerShort>();
            List<string> odoMeterValueMidnightStringList = new List<string>();

            paramName = "vehicleActivities.odoMeterValueMidnight";
            odoMeterValueMidnightStringList = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);
           
            for (int i = 0; i < odoMeterValueMidnightStringList.Count; i++)
            {
                odoMeterValueMidnight = new DDDClass.OdometerShort();
                odoMeterValueMidnight.odometerShort = Convert.ToInt32(odoMeterValueMidnightStringList[i]);
                odoMeterValueMidnightList.Add(odoMeterValueMidnight);
            }

            return odoMeterValueMidnightList;
        }

        public List<DDDClass.VuCardIWRecord> Get_VehicleActivities_VuCardIWData(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "vehicleActivities.vuCardIWData.vuCardIWRecords";
            string currentParamName;
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            DDDClass.VuCardIWRecord vuCardIWRecord = new DDDClass.VuCardIWRecord();
            List<DDDClass.VuCardIWRecord> vuCardIWData = new List<DDDClass.VuCardIWRecord>();

            List<string> cardExpiryDate = new List<string>();
            List<string> holderFirstNames = new List<string>();
            List<string> holderSurname = new List<string>();
            List<string> cardInsertionTime = new List<string>();
            List<string> cardSlotNumber = new List<string>();
            List<string> cardWithdrawalTime = new List<string>();
            List<DDDClass.FullCardNumber> fullCardNumber = new List<DDDClass.FullCardNumber>();
            List<string> manualInputFlag = new List<string>();
            List<string> previousVehicleInfo_cardWithdrawalTime = new List<string>();
            List<DDDClass.VehicleRegistrationIdentification> previousVehicleInfo_vehicleRegistrationIdentification = new List<DDDClass.VehicleRegistrationIdentification>();
            List<string> vehicleOdometerValueAtInsertion = new List<string>();
            List<string> vehicleOdometerValueAtWithdrawal = new List<string>();

            currentParamName = paramName + ".cardExpiryDate";
            cardExpiryDate = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardHolderName.holderFirstNames.name";
            holderFirstNames = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardHolderName.holderSurname.name";
            holderSurname = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardInsertionTime";
            cardInsertionTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardSlotNumber";
            cardSlotNumber = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardWithdrawalTime";
            cardWithdrawalTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".manualInputFlag";
            manualInputFlag = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".previousVehicleInfo.cardWithdrawalTime";
            previousVehicleInfo_cardWithdrawalTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".vehicleOdometerValueAtInsertion";
            vehicleOdometerValueAtInsertion = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".vehicleOdometerValueAtWithdrawal";
            vehicleOdometerValueAtWithdrawal = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            fullCardNumber = GetCardFullNumber(dataBlockId, paramName + ".fullCardNumber");
            previousVehicleInfo_vehicleRegistrationIdentification = GetVehicleRegistrationIdentification(dataBlockId, paramName + ".previousVehicleInfo.vehicleRegistrationIdentification");

            if (manualInputFlag.Count == cardSlotNumber.Count && cardExpiryDate.Count == fullCardNumber.Count && holderFirstNames.Count == holderSurname.Count)
            {
                for (int i = 0; i < holderFirstNames.Count; i++)
                {
                    vuCardIWRecord = new DDDClass.VuCardIWRecord();

                    vuCardIWRecord.cardExpiryDate = new DDDClass.TimeReal(Convert.ToInt64(cardExpiryDate[i]));

                    _bytes = enc.GetBytes(holderFirstNames[i]);
                    vuCardIWRecord.cardHolderName.holderFirstNames.name = _bytes;

                    _bytes = enc.GetBytes(holderSurname[i]);
                    vuCardIWRecord.cardHolderName.holderSurname.name = _bytes;

                    vuCardIWRecord.cardInsertionTime = new DDDClass.TimeReal(Convert.ToInt64(cardInsertionTime[i]));
                    vuCardIWRecord.cardSlotNumber = new DDDClass.CardSlotNumber(Convert.ToByte(cardSlotNumber[i]));
                    vuCardIWRecord.cardWithdrawalTime = new DDDClass.TimeReal(Convert.ToInt64(cardWithdrawalTime[i]));
                    vuCardIWRecord.fullCardNumber = fullCardNumber[i];
                    if (manualInputFlag[i] == "true")
                        vuCardIWRecord.manualInputFlag.manualInputFlag = true;
                    else
                        vuCardIWRecord.manualInputFlag.manualInputFlag = false;

                    vuCardIWRecord.previousVehicleInfo.cardWithdrawalTime = new DDDClass.TimeReal(Convert.ToInt64(previousVehicleInfo_cardWithdrawalTime[i]));
                    vuCardIWRecord.previousVehicleInfo.vehicleRegistrationIdentification = previousVehicleInfo_vehicleRegistrationIdentification[i];

                    vuCardIWRecord.vehicleOdometerValueAtInsertion.odometerShort = Convert.ToInt32(vehicleOdometerValueAtInsertion[i]);
                    vuCardIWRecord.vehicleOdometerValueAtWithdrawal.odometerShort = Convert.ToInt32(vehicleOdometerValueAtWithdrawal[i]);

                    vuCardIWData.Add(vuCardIWRecord);
                }
            }
            else throw new Exception("Ошибка, не могу извлечь информацию из базы");

            return vuCardIWData;
        }

        public List<DDDClass.VuActivityDailyData> Get_VehicleActivities_VuActivityDailyData(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "vehicleActivities.vuActivityDailyData";
            string currentParamName;
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            DDDClass.VuActivityDailyData activityChangeInfo = new DDDClass.VuActivityDailyData();
            List<DDDClass.VuActivityDailyData> ActivityChangeInfoData = new List<DDDClass.VuActivityDailyData>();

            List<string> activityChangeInfos = new List<string>();
            List<string> noOfActivityChanges = new List<string>();

            currentParamName = paramName + ".noOfActivityChanges";
            noOfActivityChanges = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityChangeInfo";
            activityChangeInfos = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            int allActivitiesNumbers = 0;
            foreach (string changeNumb in noOfActivityChanges)
            {
                allActivitiesNumbers += Convert.ToInt32(changeNumb);
            }
            if (allActivitiesNumbers != activityChangeInfos.Count)
                throw new Exception("Активности неправильно разобраны!");

            int noOfActivityChangesInt;
            int number = 0;
            for (int i = 0; i < noOfActivityChanges.Count; i++)
            {
                noOfActivityChangesInt = Convert.ToInt32(noOfActivityChanges[i]);
                activityChangeInfo = new DDDClass.VuActivityDailyData();
                activityChangeInfo.noOfActivityChanges = noOfActivityChangesInt;
                activityChangeInfo.activityChangeInfo = new List<DDDClass.ActivityChangeInfo>();

                for (int j = 0; j < noOfActivityChangesInt; j++)
                {
                    activityChangeInfo.activityChangeInfo.Add(new DDDClass.ActivityChangeInfo(activityChangeInfos[number]));
                    number++;
                }
/*Новое тестировать!!!!*/
                if (activityChangeInfo.activityChangeInfo[activityChangeInfo.activityChangeInfo.Count - 1].time == 0)
                    activityChangeInfo.activityChangeInfo.RemoveAt(activityChangeInfo.activityChangeInfo.Count - 1);

                activityChangeInfo.activityChangeInfo.Sort(ActivityChangeInfoDataComparison);
/***********************/
                ActivityChangeInfoData.Add(activityChangeInfo);
            }
            return ActivityChangeInfoData;
        }

        public List<DDDClass.VuPlaceDailyWorkPeriodRecord> Get_VehicleActivities_VuPlaceDailyWorkPeriodData(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "vehicleActivities.vuPlaceDailyWorkPeriodData.vuPlaceDailyWorkPeriodRecords";
            string currentParamName;
            List<DDDClass.VuPlaceDailyWorkPeriodRecord> vuPlaceDailyWorkPeriodData = new List<DDDClass.VuPlaceDailyWorkPeriodRecord>();
            DDDClass.VuPlaceDailyWorkPeriodRecord vuPlaceDailyWorkPeriodRecord = new DDDClass.VuPlaceDailyWorkPeriodRecord();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            List<DDDClass.FullCardNumber> fullCardNumber = new List<DDDClass.FullCardNumber>();
            List<string> dailyWorkPeriodCountry = new List<string>();
            List<string> dailyWorkPeriodRegion = new List<string>();
            List<string> entryTime = new List<string>();
            List<string> entryTypeDailyWorkPeriod = new List<string>();
            List<string> vehicleOdometerValue = new List<string>();


            currentParamName = paramName + ".fullCardNumber";
            fullCardNumber = GetCardFullNumber(dataBlockId, currentParamName);

            currentParamName = paramName + ".placeRecord.dailyWorkPeriodCountry";
            dailyWorkPeriodCountry = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".placeRecord.dailyWorkPeriodRegion";
            dailyWorkPeriodRegion = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".placeRecord.entryTime";
            entryTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".placeRecord.entryTypeDailyWorkPeriod";
            entryTypeDailyWorkPeriod = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".placeRecord.vehicleOdometerValue";
            vehicleOdometerValue = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            if (dailyWorkPeriodCountry.Count == dailyWorkPeriodRegion.Count && entryTypeDailyWorkPeriod.Count == fullCardNumber.Count)
            {
                for (int i = 0; i < entryTime.Count; i++)
                {
                    vuPlaceDailyWorkPeriodRecord = new DDDClass.VuPlaceDailyWorkPeriodRecord();

                    vuPlaceDailyWorkPeriodRecord.fullCardNumber = fullCardNumber[i];
                    vuPlaceDailyWorkPeriodRecord.placeRecord.dailyWorkPeriodCountry = new DDDClass.NationNumeric(Convert.ToInt16(dailyWorkPeriodCountry[i]));
                    vuPlaceDailyWorkPeriodRecord.placeRecord.dailyWorkPeriodRegion = new DDDClass.RegionNumeric(Convert.ToByte(dailyWorkPeriodRegion[i]));
                    vuPlaceDailyWorkPeriodRecord.placeRecord.entryTime = new DDDClass.TimeReal(Convert.ToInt64(entryTime[i]));
                    vuPlaceDailyWorkPeriodRecord.placeRecord.entryTypeDailyWorkPeriod = new DDDClass.EntryTypeDailyWorkPeriod(Convert.ToByte(entryTypeDailyWorkPeriod[i]));
                    vuPlaceDailyWorkPeriodRecord.placeRecord.vehicleOdometerValue.odometerShort = Convert.ToInt32(vehicleOdometerValue[i]);

                    vuPlaceDailyWorkPeriodData.Add(vuPlaceDailyWorkPeriodRecord);
                }
            }
            else throw new Exception("Ошибка, не могу извлечь информацию из базы");

            return vuPlaceDailyWorkPeriodData;
        }

        public List<DDDClass.SpecificConditionRecord> Get_VehicleActivities_VuSpecificConditionData(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "vehicleActivities.vuSpecificConditionData.noOfSpecificConditionRecords";
            string currentParamName;
            List<DDDClass.SpecificConditionRecord> vuSpecificConditionData = new List<DDDClass.SpecificConditionRecord>();
            DDDClass.SpecificConditionRecord vuSpecificConditionRecord = new DDDClass.SpecificConditionRecord();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            List<string> entryTime = new List<string>();
            List<string> specificConditionType = new List<string>();

            currentParamName = paramName + ".entryTime";
            entryTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".specificConditionType";
            specificConditionType = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            if (entryTime.Count == specificConditionType.Count)
            {
                for (int i = 0; i < entryTime.Count; i++)
                {
                    vuSpecificConditionRecord = new DDDClass.SpecificConditionRecord();

                    vuSpecificConditionRecord.entryTime = new DDDClass.TimeReal(Convert.ToInt64(entryTime[i]));
                    vuSpecificConditionRecord.specificConditionType = new DDDClass.SpecificConditionType(Convert.ToInt16(specificConditionType[i]));

                    vuSpecificConditionData.Add(vuSpecificConditionRecord);
                }
            }
            else throw new Exception("Ошибка, не могу извлечь информацию из базы");

            return vuSpecificConditionData;
        }

        private List<VehichleUnit.Vehicle_Activities> Get_VehicleActivities_AllInOne(int dataBlockId)
        {
            List<VehichleUnit.Vehicle_Activities> vehicleActivities = new List<VehichleUnit.Vehicle_Activities>();
            vehicleActivities = Get_VehicleActivities_AllInOne(dataBlockId, new DateTime(), DateTime.Now);
            return vehicleActivities;
        }
        private List<VehichleUnit.Vehicle_Activities> Get_VehicleActivities_AllInOne(int dataBlockId, DateTime fromDate, DateTime toDate)
        {
            VehichleUnit.Vehicle_Activities vehicleActivity;
            List<VehichleUnit.Vehicle_Activities> vehicleActivitiesList = new List<VehichleUnit.Vehicle_Activities>();

            List<DDDClass.TimeReal> downloadedDayDate = new List<DDDClass.TimeReal>();
            List<DDDClass.OdometerShort> odoMeterValueMidnight = new List<DDDClass.OdometerShort>();
            List<DDDClass.VuCardIWRecord> vuCardIWRecord = new List<DDDClass.VuCardIWRecord>();
            List<DDDClass.VuActivityDailyData> vuActivityDailyData = new List<DDDClass.VuActivityDailyData>();

            int userID = 0;

            vuCardIWRecord = Get_VehicleActivities_VuCardIWData(dataBlockId);
            downloadedDayDate = Get_VehicleActivities_DownloadedDayDate(dataBlockId);
            odoMeterValueMidnight = Get_VehicleActivities_OdoMeterValueMidnight(dataBlockId);
            vuActivityDailyData = Get_VehicleActivities_VuActivityDailyData(dataBlockId);

            string downloadedDayDateRecord;
            string cardInsertionTimeRecord;
            string cardWithdrawalTimeRecord;

            List<int> Indexes = new List<int>();
            Indexes = CheckDate(downloadedDayDate, downloadedDayDate, fromDate, toDate);

            for (int i = 0; i < downloadedDayDate.Count; i++)
            {
                if (Indexes.Contains(i))
                {
                    vehicleActivity = new VehichleUnit.Vehicle_Activities();
                    vehicleActivity.downloadedDayDate = downloadedDayDate[i];
                    vehicleActivity.odoMeterValueMidnight = odoMeterValueMidnight[i];
                    vehicleActivity.vuActivityDailyData = vuActivityDailyData[i];

                    downloadedDayDateRecord = vehicleActivity.downloadedDayDate.getTimeRealDate().ToLongDateString();

                    for (int j = 0; j < vuCardIWRecord.Count; j++)
                    {
                        cardInsertionTimeRecord = vuCardIWRecord[j].cardInsertionTime.getTimeRealDate().ToLongDateString();
                        cardWithdrawalTimeRecord = vuCardIWRecord[j].cardWithdrawalTime.getTimeRealDate().ToLongDateString();

                        if (downloadedDayDateRecord == cardInsertionTimeRecord || downloadedDayDateRecord == cardWithdrawalTimeRecord)
                        {
                            vehicleActivity.vuCardIWData.vuCardIWRecords.Add(vuCardIWRecord[j]);
                            // vuCardIWRecord.RemoveAt(j); // убрать коммент, если не надо повторов как в Tachograph file viewer
                        }
                    }
                    vehicleActivitiesList.Add(vehicleActivity);
                }
            }           

            return vehicleActivitiesList;
        }
        public List<VehichleUnit.Vehicle_Activities> Get_VehicleActivities_AllInOne(List<int> dataBlockIDS, DateTime startPeriod, DateTime endPeriod)
        {
            List<VehichleUnit.Vehicle_Activities> records = new List<VehichleUnit.Vehicle_Activities>();
            List<int> dataBlockIdsToGet = new List<int>();
            DateTime fromTemp = new DateTime();
            DateTime toTemp = new DateTime();
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            List<DateTime> startEndPeriod = new List<DateTime>();

            foreach (int dataBlock in dataBlockIDS)
            {
                startEndPeriod = Get_StartEndPeriod(dataBlock);
                fromTemp = startEndPeriod[0];
                toTemp = startEndPeriod[1];
                if (fromTemp.Date >= startPeriod && fromTemp.Date <= endPeriod)
                {
                    dataBlockIdsToGet.Add(dataBlock);
                    continue;
                }
                if (toTemp.Date >= startPeriod && toTemp.Date <= endPeriod)
                {
                    dataBlockIdsToGet.Add(dataBlock);
                    continue;
                }
                if (startPeriod >= fromTemp.Date && endPeriod <= toTemp.Date)
                {
                    dataBlockIdsToGet.Add(dataBlock);
                    continue;
                }
            }

            foreach (int id in dataBlockIdsToGet)
            {
                records.AddRange(Get_VehicleActivities_AllInOne(id, startPeriod, endPeriod));
            }           
            records.Sort(VehicleActivitiesDataComparison);
            return records;
        }

        public VehichleUnit.VehicleUnitClass GetAllVehicleUnitClass_parsingDataBlock(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string fileName = "";
            PARSER.DDDParser dddParser = new PARSER.DDDParser();

            fileName = sqldbRecords.Get_ParamValue(dataBlockId, "DataBlock_FileName");
            byte[] blockDataBlob = sqlDB.GetDataBlock(dataBlockId);
            blockDataBlob = UnZipBytes(blockDataBlob);//АнЗипим файл!
            dddParser.ParseFile(blockDataBlob, fileName);

            return dddParser.vehicleUnitClass;
        }
        private byte[] UnZipBytes(byte[] _bytes)
        {
            byte[] bar = Compressor.Compressor.Decompress(_bytes);
            return bar;
        }

        //------------------------private functions!!!
        private List<DDDClass.FullCardNumber> GetCardFullNumber(int dataBlockId, string paramName)
        {
            DDDClass.FullCardNumber fullCardNumber = new DDDClass.FullCardNumber();
            List<DDDClass.FullCardNumber> fullCardNumberList = new List<DDDClass.FullCardNumber>();
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string currentParamName;

            List<string> cardIssuingMemberState = new List<string>();
            List<string> cardType = new List<string>();
            List<string> cardConsecutiveIndex = new List<string>();
            List<string> cardRenewalIndex = new List<string>();
            List<string> cardReplacementIndex = new List<string>();
            List<string> driverIdentification = new List<string>();
            List<string> ownerIdentification = new List<string>();

            currentParamName = paramName + ".cardIssuingMemberState";
            cardIssuingMemberState = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardType";
            cardType = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumber.cardConsecutiveIndex";
            cardConsecutiveIndex = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumber.cardRenewalIndex";
            cardRenewalIndex = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumber.cardReplacementIndex";
            cardReplacementIndex = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumber.driverIdentification";
            driverIdentification = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumber.ownerIdentification";
            ownerIdentification = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            if (cardRenewalIndex.Count == ownerIdentification.Count && cardType.Count == cardIssuingMemberState.Count)
            {
                for (int i = 0; i < cardRenewalIndex.Count; i++)
                {
                    fullCardNumber = new DDDClass.FullCardNumber();

                    fullCardNumber.cardIssuingMemberState = new DDDClass.NationNumeric(Convert.ToInt16(cardIssuingMemberState[i]));

                    fullCardNumber.cardType = new DDDClass.EquipmentType(Convert.ToByte(cardType[i]));

                    fullCardNumber.cardNumber.cardConsecutiveIndex = new DDDClass.CardConsecutiveIndex(cardConsecutiveIndex[i]);
                    fullCardNumber.cardNumber.cardRenewalIndex = new DDDClass.CardRenewalIndex(cardRenewalIndex[i]);
                    fullCardNumber.cardNumber.cardReplacementIndex = new DDDClass.CardReplacementIndex(cardReplacementIndex[i]);
                    fullCardNumber.cardNumber.driverIdentification = driverIdentification[i];
                    fullCardNumber.cardNumber.ownerIdentification = ownerIdentification[i];

                    fullCardNumberList.Add(fullCardNumber);
                }
            }
            return fullCardNumberList;
        }

        private List<DDDClass.VehicleRegistrationIdentification> GetVehicleRegistrationIdentification(int dataBlockId, string paramName)
        {
            DDDClass.VehicleRegistrationIdentification vehicleRegistrationIdentification = new DDDClass.VehicleRegistrationIdentification();
            List<DDDClass.VehicleRegistrationIdentification> vehicleRegistrationIdentificationList = new List<DDDClass.VehicleRegistrationIdentification>();
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string currentParamName;
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;
            List<string> vehicleRegistrationNumber = new List<string>();
            List<string> vehicleRegistrationNation = new List<string>();

            currentParamName = paramName + ".vehicleRegistrationNumber.vehicleRegNumber";
            vehicleRegistrationNumber = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".vehicleRegistrationNation";
            vehicleRegistrationNation = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            if (vehicleRegistrationNumber.Count == vehicleRegistrationNation.Count)
            {
                for (int i = 0; i < vehicleRegistrationNumber.Count; i++)
                {
                    vehicleRegistrationIdentification = new DDDClass.VehicleRegistrationIdentification();

                    vehicleRegistrationIdentification.vehicleRegistrationNation = new DDDClass.NationNumeric(Convert.ToInt16(vehicleRegistrationNation[i]));

                    _bytes = enc.GetBytes(vehicleRegistrationNumber[i]);
                    vehicleRegistrationIdentification.vehicleRegistrationNumber.vehicleRegNumber = _bytes;

                    vehicleRegistrationIdentificationList.Add(vehicleRegistrationIdentification);
                }
            }
            return vehicleRegistrationIdentificationList;
        }

        private List<DDDClass.ExtendedSerialNumber> GetExtendedSerialNumber(int dataBlockId, string paramName)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            DDDClass.ExtendedSerialNumber extendedSerialNumber = new DDDClass.ExtendedSerialNumber();
            List<DDDClass.ExtendedSerialNumber> extendedSerialNumberList = new List<DDDClass.ExtendedSerialNumber>();
            string currentParamName;

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            List<string> manufacturerCode = new List<string>();
            List<string> monthYear = new List<string>();
            List<string> serialNumber = new List<string>();
            List<string> type = new List<string>();

            currentParamName = paramName + ".manufacturerCode";
            manufacturerCode = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".monthYear";
            monthYear = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".serialNumber";
            serialNumber = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".type";
            type = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            if (manufacturerCode.Count == monthYear.Count && type.Count == serialNumber.Count)
            {
                for (int i = 0; i < manufacturerCode.Count; i++)
                {
                    extendedSerialNumber = new DDDClass.ExtendedSerialNumber();
                    extendedSerialNumber.manufacturerCode = new DDDClass.ManufacturerCode(Convert.ToInt16(manufacturerCode[i]));

                    extendedSerialNumber.monthYear = monthYear[i];

                    extendedSerialNumber.serialNumber = Convert.ToUInt32(serialNumber[i]);
                    extendedSerialNumber.type = Convert.ToByte(type[i]);

                    extendedSerialNumberList.Add(extendedSerialNumber);
                }
            }
            return extendedSerialNumberList;
        }

        private List<int> CheckDate(List<string> dateArray, DateTime periodDate, bool reverseDirection)//true <=, false >=
        {
            List<int> returnArray = new List<int>();
            DateTime dateTime = new DateTime();
            DDDClass.TimeReal timeReal;
            int index = 0;

            foreach (string record in dateArray)
            {
                timeReal = new DDDClass.TimeReal(record);
                dateTime = timeReal.getTimeRealDate();

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

            foreach (int fromIndex in fromIndexes)
            {
                foreach (int toIndex in toIndexes)
                    if (fromIndex == toIndex)
                        Indexes.Add(fromIndex);
            }
            return Indexes;
        }

        private List<int> CheckDate(List<DDDClass.TimeReal> beginDateArray, List<DDDClass.TimeReal> endDateArray, DateTime startPeriodDate, DateTime endPeriodDate)
        {
            List<int> fromIndexes = new List<int>();
            List<int> toIndexes = new List<int>();
            List<int> Indexes = new List<int>();
            List<string> timereal = new List<string>();
            foreach(DDDClass.TimeReal time in beginDateArray)
                timereal.Add(time.timereal.ToString());
            fromIndexes = CheckDate(timereal, startPeriodDate, false);

            timereal = new List<string>();
            foreach (DDDClass.TimeReal time in endDateArray)
                timereal.Add(time.timereal.ToString());
            toIndexes = CheckDate(timereal, endPeriodDate, true);

            foreach (int fromIndex in fromIndexes)
            {
                foreach (int toIndex in toIndexes)
                    if (fromIndex == toIndex)
                        Indexes.Add(fromIndex);
            }
            return Indexes;
        }
        //----------------list comparison functions
        private int VuOverSpeedingEventDataComparison(DDDClass.VuOverSpeedingEventRecord x, DDDClass.VuOverSpeedingEventRecord y)
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
                    int retval = x.eventBeginTime.timereal.CompareTo(y.eventBeginTime.timereal);
                    return retval;
                }
            }
        }

        private int ActivityChangeInfoDataComparison(DDDClass.ActivityChangeInfo x, DDDClass.ActivityChangeInfo y)
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
                    int retval = x.time.CompareTo(y.time);
                    return retval;
                }
            }
        }

        private int VehicleActivitiesDataComparison(VehichleUnit.Vehicle_Activities x, VehichleUnit.Vehicle_Activities y)
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
                    int retval = x.downloadedDayDate.timereal.CompareTo(y.downloadedDayDate.timereal);
                    return retval;
                }
            }
        }

        public List<DateTime> Get_StartEndPeriod(int dataBlockId)
        {
            List<DateTime> returnPeriod = new List<DateTime>();
            DDDClass.VuDownloadablePeriod period = new DDDClass.VuDownloadablePeriod();
            period = Get_VehicleOverview_VuDownloadablePeriod(dataBlockId);

            returnPeriod.Add(period.minDownloadableTime.getTimeRealDate());
            returnPeriod.Add(period.maxDownloadableTime.getTimeRealDate());

            return returnPeriod;
        }
        //////////////////////Statistics////////////////////////////
        public double Statistics_GetYearStatistics(DateTime date, int datablockId)//Проверить функции
        {
            double stat = 0;
            int minutesInDay = 1440;
            int dayInYear = GetDaysInAYear(date.Year);

            VehichleUnit.VehicleUnitClass vehicleUnitClass = new VehichleUnit.VehicleUnitClass();
            vehicleUnitClass.vehicleActivities = Get_VehicleActivities_AllInOne(datablockId, new DateTime(date.Year, 1, 1), new DateTime(date.Year, 12, DateTime.DaysInMonth(date.Year, 12)));

            stat = (vehicleUnitClass.GetTotalVehicleActivitiesTime().TotalMinutes / (minutesInDay * dayInYear)) * 100;
            return stat;
        }
        public double Statistics_GetMonthStatistics(DateTime date, int datablockId)//Проверить функции activities.GetTotalTime() - писалась давно, может не точно подсчитывать!
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            double stat = 0;
            int minutesInDay = 1440;
            int dayInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            VehichleUnit.VehicleUnitClass vehicleUnitClass = new VehichleUnit.VehicleUnitClass();
            vehicleUnitClass.vehicleActivities = Get_VehicleActivities_AllInOne(datablockId, new DateTime(date.Year, date.Month, 1), new DateTime(date.Year, date.Month, dayInMonth));

            stat = (vehicleUnitClass.GetTotalVehicleActivitiesTime().TotalMinutes / (minutesInDay * dayInMonth)) * 100;
            return stat;
        }
        public double Statistics_GetDayStatistics(DateTime date, int datablockId)//Проверить функции activities.GetTotalTime() - писалась давно, может не точно подсчитывать!
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            double stat = 0;
            int minutesInDay = 1440;

            VehichleUnit.VehicleUnitClass vehicleUnitClass = new VehichleUnit.VehicleUnitClass();
            //DateTime nextDay = new DateTime(date.Year,date.Month,date.Day+1);
            vehicleUnitClass.vehicleActivities = Get_VehicleActivities_AllInOne(datablockId, date.Date, date);
            stat = (vehicleUnitClass.GetTotalVehicleActivitiesTime().TotalMinutes / minutesInDay) * 100;
            return stat;
        }
        private int GetDaysInAYear(int year)
        {

            int days = 0;
            for (int i = 1; i <= 12; i++)
            {
                days += DateTime.DaysInMonth(year, i);
            }
            return days;
        }
        /////////////////////////////////////////////////////
    }
}
