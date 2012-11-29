using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// Класс отвечает за выборку информации по ДДД картам.
    /// </summary>
    public class CardUnitInfo
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
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionsStringTMP">Строка подключения</param>
        /// <param name="Current_Language">Текущий язык</param>
        /// <param name="sqlTemp">Обьект подключения к базе данных</param>
        public CardUnitInfo(string connectionsStringTMP, string Current_Language, SQLDB sqlTemp)
        {
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            sqlDB = sqlTemp;
        }

        //EF_Application_Identification
        public DDDClass.DriverCardApplicationIdentification Get_EF_Application_Identification_DriverCardApplicationIdentification(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_application_identification.driverCardApplicationIdentification";
            string currentParamName;
            DDDClass.DriverCardApplicationIdentification driverCardApplicationIdentification = new DDDClass.DriverCardApplicationIdentification();

            string activityStructureLength;
            string cardStructureVersion;
            string noOfCardPlaceRecords;
            string noOfCardVehicleRecords;
            string noOfEventsPerType;
            string noOfFaultsPerType;
            string typeOfTachographCardId;

            currentParamName = paramName + ".activityStructureLength";
            activityStructureLength = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardStructureVersion";
            cardStructureVersion = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".noOfCardPlaceRecords";
            noOfCardPlaceRecords = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".noOfCardVehicleRecords";
            noOfCardVehicleRecords = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".noOfEventsPerType";
            noOfEventsPerType = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".noOfFaultsPerType";
            noOfFaultsPerType = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".typeOfTachographCardId";
            typeOfTachographCardId = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            driverCardApplicationIdentification.activityStructureLength = new DDDClass.CardActivityLengthRange(activityStructureLength);
            driverCardApplicationIdentification.cardStructureVersion = new DDDClass.CardStructureVersion(cardStructureVersion);
            driverCardApplicationIdentification.noOfCardPlaceRecords = new DDDClass.NoOfCardPlaceRecords(noOfCardPlaceRecords);
            driverCardApplicationIdentification.noOfCardVehicleRecords = new DDDClass.NoOfCardVehicleRecords(noOfCardVehicleRecords);
            driverCardApplicationIdentification.noOfEventsPerType = new DDDClass.NoOfEventsPerType(noOfEventsPerType);
            driverCardApplicationIdentification.noOfFaultsPerType = new DDDClass.NoOfFaultsPerType(noOfFaultsPerType);
            driverCardApplicationIdentification.typeOfTachographCardId = new DDDClass.EquipmentType(typeOfTachographCardId);

            return driverCardApplicationIdentification;
        }

        public DDDClass.WorkshopCardApplicationIdentification Get_EF_Application_Identification_WorkshopCardApplicationIdentification(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_application_identification.workshopCardApplicationIdentification";
            string currentParamName;
            DDDClass.WorkshopCardApplicationIdentification worksShopCardApplicationIdentification = new DDDClass.WorkshopCardApplicationIdentification();

            string activityStructureLength;
            string cardStructureVersion;
            string noOfCalibrationRecords;
            string noOfCardPlaceRecords;
            string noOfCardVehicleRecords;
            string noOfEventsPerType;
            string noOfFaultsPerType;
            string typeOfTachographCardId;

            currentParamName = paramName + ".activityStructureLength";
            activityStructureLength = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardStructureVersion";
            cardStructureVersion = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".noOfCalibrationRecords";
            noOfCalibrationRecords = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".noOfCardPlaceRecords";
            noOfCardPlaceRecords = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".noOfCardVehicleRecords";
            noOfCardVehicleRecords = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".noOfEventsPerType";
            noOfEventsPerType = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".noOfFaultsPerType";
            noOfFaultsPerType = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".typeOfTachographCardId";
            typeOfTachographCardId = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            worksShopCardApplicationIdentification.activityStructureLength = new DDDClass.CardActivityLengthRange(activityStructureLength);
            worksShopCardApplicationIdentification.cardStructureVersion = new DDDClass.CardStructureVersion(cardStructureVersion);
            worksShopCardApplicationIdentification.noOfCalibrationRecords = new DDDClass.NoOfCalibrationRecords(noOfCalibrationRecords);
            worksShopCardApplicationIdentification.noOfCardPlaceRecords = new DDDClass.NoOfCardPlaceRecords(noOfCardPlaceRecords);
            worksShopCardApplicationIdentification.noOfCardVehicleRecords = new DDDClass.NoOfCardVehicleRecords(noOfCardVehicleRecords);
            worksShopCardApplicationIdentification.noOfEventsPerType = new DDDClass.NoOfEventsPerType(noOfEventsPerType);
            worksShopCardApplicationIdentification.noOfFaultsPerType = new DDDClass.NoOfFaultsPerType(noOfFaultsPerType);
            worksShopCardApplicationIdentification.typeOfTachographCardId = new DDDClass.EquipmentType(typeOfTachographCardId);

            return worksShopCardApplicationIdentification;
        }

        public DDDClass.ControlCardApplicationIdentification Get_EF_Application_Identification_ControlCardApplicationIdentification(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_application_identification.controlCardApplicationIdentification";
            string currentParamName;
            DDDClass.ControlCardApplicationIdentification controlCardApplicationIdentification = new DDDClass.ControlCardApplicationIdentification();

            string cardStructureVersion;
            string noOfControlActivityRecords;
            string typeOfTachographCardId;

            currentParamName = paramName + ".cardStructureVersion";
            cardStructureVersion = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".noOfControlActivityRecords";
            noOfControlActivityRecords = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".typeOfTachographCardId";
            typeOfTachographCardId = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            controlCardApplicationIdentification.cardStructureVersion = new DDDClass.CardStructureVersion(cardStructureVersion);
            controlCardApplicationIdentification.noOfControlActivityRecords = new DDDClass.NoOfControlActivityRecords(noOfControlActivityRecords);
            controlCardApplicationIdentification.typeOfTachographCardId = new DDDClass.EquipmentType(typeOfTachographCardId);

            return controlCardApplicationIdentification;
        }

        public DDDClass.CompanyCardApplicationIdentification Get_EF_Application_Identification_CompanyCardApplicationIdentification(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_application_identification.companyCardApplicationIdentification";
            string currentParamName;
            DDDClass.CompanyCardApplicationIdentification companyCardApplicationIdentification = new DDDClass.CompanyCardApplicationIdentification();

            string cardStructureVersion;
            string noOfCompanyActivityRecords;
            string typeOfTachographCardId;

            currentParamName = paramName + ".cardStructureVersion";
            cardStructureVersion = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".noOfCompanyActivityRecords";
            noOfCompanyActivityRecords = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".typeOfTachographCardId";
            typeOfTachographCardId = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            companyCardApplicationIdentification.cardStructureVersion = new DDDClass.CardStructureVersion(cardStructureVersion);
            companyCardApplicationIdentification.noOfCompanyActivityRecords = new DDDClass.NoOfCompanyActivityRecords(noOfCompanyActivityRecords);
            companyCardApplicationIdentification.typeOfTachographCardId = new DDDClass.EquipmentType(typeOfTachographCardId);

            return companyCardApplicationIdentification;
        }

        public DDDClass.EquipmentType Get_EF_Application_Identification_CardType(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_application_identification.cardType";
            string currentParamName;

            DDDClass.EquipmentType equipmentType;
            string cardType;

            currentParamName = paramName;
            cardType = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            equipmentType = new DDDClass.EquipmentType(cardType);
            return equipmentType;
        }
        //EF_ICC
        public CardUnit.EF_ICC Get_EF_ICC(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_icc";
            string currentParamName;
            CardUnit.EF_ICC ef_icc = new CardUnit.EF_ICC();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            string cardApprovalNumber;
            List<DDDClass.ExtendedSerialNumber> cardExtendedSerialNumber = new List<DDDClass.ExtendedSerialNumber>();
            string cardPersonaliserID;
            string clockStop;
            string embedderIcAssemblerId;
            string icIdentifier;

            currentParamName = paramName + ".cardApprovalNumber";
            cardApprovalNumber = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardExtendedSerialNumber";
            cardExtendedSerialNumber = GetExtendedSerialNumber(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardPersonaliserID";
            cardPersonaliserID = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".clockStop";
            clockStop = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".embedderIcAssemblerId";
            embedderIcAssemblerId = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".icIdentifier";
            icIdentifier = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            ef_icc.cardIccIdentification.cardApprovalNumber = new DDDClass.CardApprovalNumber(cardApprovalNumber);

            if (cardExtendedSerialNumber.Count > 0)
                ef_icc.cardIccIdentification.cardExtendedSerialNumber = cardExtendedSerialNumber[0];

            ef_icc.cardIccIdentification.cardPersonaliserID = Convert.ToByte(cardPersonaliserID);
            ef_icc.cardIccIdentification.clockStop = Convert.ToByte(clockStop);

            _bytes = enc.GetBytes(embedderIcAssemblerId);
            ef_icc.cardIccIdentification.embedderIcAssemblerId = _bytes;
            _bytes = enc.GetBytes(icIdentifier);
            ef_icc.cardIccIdentification.icIdentifier = _bytes;

            return ef_icc;
        }
        //EF_IC
        public CardUnit.EF_IC Get_EF_IC(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_ic";
            string currentParamName;
            CardUnit.EF_IC ef_ic = new CardUnit.EF_IC();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            string icManufacturingReferences;
            string icSerialNumber;

            currentParamName = paramName + ".icManufacturingReferences";
            icManufacturingReferences = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".icSerialNumber";
            icSerialNumber = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            ef_ic.cardChipIdentification.Set_icSerialNumber(icSerialNumber);
            ef_ic.cardChipIdentification.Set_icManufacturingReferences(icManufacturingReferences);

            return ef_ic;
        }
        //EF_Identification
        public DDDClass.EquipmentType Get_EF_Identification_CardType(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_identification.cardType";
            string currentParamName;

            DDDClass.EquipmentType equipmentType;
            string cardType;

            currentParamName = paramName;
            cardType = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            equipmentType = new DDDClass.EquipmentType(cardType);
            return equipmentType;
        }

        public DDDClass.CardIdentification Get_EF_Identification_CardIdentification(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_identification.cardIdentification";
            string currentParamName;
            DDDClass.CardIdentification cardIdentification = new DDDClass.CardIdentification();

            string cardExpiryDate;
            string cardIssueDate;
            string cardIssuingAuthorityName;
            string cardIssuingMemberState;
            string cardValidityBegin;
            DDDClass.CardNumber cardNumber = new DDDClass.CardNumber();


            currentParamName = paramName + ".cardExpiryDate";
            cardExpiryDate = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardIssueDate";
            cardIssueDate = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardIssuingAuthorityName.name";
            cardIssuingAuthorityName = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardIssuingMemberState";
            cardIssuingMemberState = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardValidityBegin";
            cardValidityBegin = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardNumber";
            cardNumber = GetCardNumber(dataBlockId, currentParamName)[0];//Зайти внутрь попалить

            cardIdentification.cardExpiryDate = new DDDClass.TimeReal(Convert.ToInt64(cardExpiryDate));
            cardIdentification.cardIssueDate = new DDDClass.TimeReal(Convert.ToInt64(cardIssueDate));
            cardIdentification.cardIssuingAuthorityName.setName(cardIssuingAuthorityName);
            cardIdentification.cardIssuingMemberState = new DDDClass.NationNumeric(Convert.ToByte(cardIssuingMemberState));
            cardIdentification.cardValidityBegin = new DDDClass.TimeReal(cardValidityBegin);
            cardIdentification.cardNumber = cardNumber;
            return cardIdentification;
        }

        public DDDClass.DriverCardHolderIdentification Get_EF_Identification_DriverCardHolderIdentification(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            DDDClass.DriverCardHolderIdentification driverCardHolderIdentification = new DDDClass.DriverCardHolderIdentification();
            string paramName = "ef_identification.driverCardHolderIdentification";
            string currentParamName;

            string cardHolderBirthDate_day;
            string cardHolderBirthDate_month;
            string cardHolderBirthDate_year;
            string holderFirstNames;
            string holderSurname;
            string cardHolderPreferredLanguage;


            currentParamName = paramName + ".cardHolderBirthDate.day";
            cardHolderBirthDate_day = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardHolderBirthDate.month";
            cardHolderBirthDate_month = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardHolderBirthDate.year";
            cardHolderBirthDate_year = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardHolderName.holderFirstNames.name";
            holderFirstNames = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardHolderName.holderSurname.name";
            holderSurname = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardHolderPreferredLanguage";
            cardHolderPreferredLanguage = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            driverCardHolderIdentification.cardHolderBirthDate.Set_Day(cardHolderBirthDate_day);
            driverCardHolderIdentification.cardHolderBirthDate.Set_Month(cardHolderBirthDate_month);
            driverCardHolderIdentification.cardHolderBirthDate.Set_Year(cardHolderBirthDate_year);
            driverCardHolderIdentification.cardHolderName.holderFirstNames.setName(holderFirstNames);
            driverCardHolderIdentification.cardHolderName.holderSurname.setName(holderSurname);
            driverCardHolderIdentification.cardHolderPreferredLanguage = new DDDClass.Language(cardHolderPreferredLanguage);

            return driverCardHolderIdentification;
        }

        public DDDClass.WorkshopCardHolderIdentification Get_EF_Identification_WorkshopCardHolderIdentification(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            DDDClass.WorkshopCardHolderIdentification workshopCardHolderIdentification = new DDDClass.WorkshopCardHolderIdentification();
            string paramName = "ef_identification.workshopCardHolderIdentification";
            string currentParamName;

            string holderFirstNames;
            string holderSurname;
            string cardHolderPreferredLanguage;
            string workshopAddress;
            string workshopName;


            currentParamName = paramName + ".holderFirstNames";
            holderFirstNames = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".holderSurname";
            holderSurname = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardHolderPreferredLanguage";
            cardHolderPreferredLanguage = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".workshopAddress.address";
            workshopAddress = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".workshopName.name";
            workshopName = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            workshopCardHolderIdentification.cardHolderName.holderFirstNames.setName(holderFirstNames);
            workshopCardHolderIdentification.cardHolderName.holderSurname.setName(holderSurname);
            workshopCardHolderIdentification.cardHolderPreferredLanguage = new DDDClass.Language(cardHolderPreferredLanguage);
            workshopCardHolderIdentification.workshopAddress.SetAddress(workshopAddress);
            workshopCardHolderIdentification.workshopName.setName(workshopName);

            return workshopCardHolderIdentification;
        }

        public DDDClass.ControlCardHolderIdentification Get_EF_Identification_ControlCardHolderIdentification(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            DDDClass.ControlCardHolderIdentification controlCardHolderIdentification = new DDDClass.ControlCardHolderIdentification();
            string paramName = "ef_identification.controlCardHolderIdentification";
            string currentParamName;

            string holderFirstNames;
            string holderSurname;
            string cardHolderPreferredLanguage;
            string controlBodyAddress;
            string controlBodyName;

            currentParamName = paramName + ".cardHolderName.holderFirstNames.name";
            holderFirstNames = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardHolderName.holderSurname.name";
            holderSurname = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardHolderPreferredLanguage";
            cardHolderPreferredLanguage = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".controlBodyAddress.address";
            controlBodyAddress = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".controlBodyName.name";
            controlBodyName = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            controlCardHolderIdentification.cardHolderName.holderFirstNames = new DDDClass.Name(0, holderFirstNames);
            controlCardHolderIdentification.cardHolderName.holderSurname = new DDDClass.Name(0, holderSurname);
            controlCardHolderIdentification.cardHolderPreferredLanguage = new DDDClass.Language(cardHolderPreferredLanguage);
            controlCardHolderIdentification.controlBodyAddress = new DDDClass.Address(0, controlBodyAddress);
            controlCardHolderIdentification.controlBodyName = new DDDClass.Name(0, controlBodyName);

            return controlCardHolderIdentification;
        }

        public DDDClass.CompanyCardHolderIdentification Get_EF_Identification_CompanyCardHolderIdentification(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            DDDClass.CompanyCardHolderIdentification companyCardHolderIdentification = new DDDClass.CompanyCardHolderIdentification();
            string paramName = "ef_identification.workshopCardHolderIdentification";
            string currentParamName;

            string cardHolderPreferredLanguage;
            string companyAddress;
            string companyName;

            currentParamName = paramName + ".cardHolderPreferredLanguage";
            cardHolderPreferredLanguage = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".companyAddress.address";
            companyAddress = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".companyName.name";
            companyName = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            companyCardHolderIdentification.cardHolderPreferredLanguage = new DDDClass.Language(cardHolderPreferredLanguage);
            companyCardHolderIdentification.companyAddress = new DDDClass.Address(0, companyAddress);
            companyCardHolderIdentification.companyName = new DDDClass.Name(0, companyName);

            return companyCardHolderIdentification;
        }
        //EF_Card_Download
        public CardUnit.EF_Card_Download Get_EF_Card_Download(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            CardUnit.EF_Card_Download efCardDownload = new CardUnit.EF_Card_Download();
            string paramName = "ef_card_download";
            string currentParamName;
            try
            {
                DDDClass.EquipmentType cardType = Get_EF_Identification_CardType(dataBlockId);
                string lastCardDownload;
                string noOfCalibrationsSinceDownload;
                efCardDownload.cardType = cardType.equipmentType;

                switch (cardType.equipmentType)
                {
                    case 1://driver card
                        currentParamName = paramName + ".lastCardDownload";
                        lastCardDownload = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);
                        efCardDownload.lastCardDownload = new DDDClass.LastCardDownload(Convert.ToInt64(lastCardDownload));
                        efCardDownload.cardType = cardType.equipmentType;
                        break;
                    case 2://workshop card
                        currentParamName = paramName + ".noOfCalibrationsSinceDownload";
                        noOfCalibrationsSinceDownload = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);
                        efCardDownload.noOfCalibrationsSinceDownload = new DDDClass.NoOfCalibrationsSinceDownload(noOfCalibrationsSinceDownload);
                        efCardDownload.cardType = cardType.equipmentType;
                        break;
                    default:
                        efCardDownload.cardType = cardType.equipmentType;
                        break;
                }
                return efCardDownload;
            }
            catch (Exception)
            {
                return efCardDownload;
            }
        }
        //EF_Driving_Licence_Info
        public CardUnit.EF_Driving_Licence_Info Get_EF_Driving_Licence_Info(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            CardUnit.EF_Driving_Licence_Info efDrivingLicenceInfo = new CardUnit.EF_Driving_Licence_Info();
            string paramName = "ef_driving_licence_info";
            string currentParamName;
            string drivingLicenceIssuingAuthority;
            string drivingLicenceIssuingNation;
            string drivingLicenceNumber;

            try
            {
                currentParamName = paramName + ".drivingLicenceIssuingAuthority.name";
                drivingLicenceIssuingAuthority = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

                currentParamName = paramName + ".drivingLicenceIssuingNation";
                drivingLicenceIssuingNation = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

                currentParamName = paramName + ".drivingLicenceNumber";
                drivingLicenceNumber = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

                efDrivingLicenceInfo.cardDrivingLicenceInformation.drivingLicenceIssuingAuthority = new DDDClass.Name(0, drivingLicenceIssuingAuthority);
                efDrivingLicenceInfo.cardDrivingLicenceInformation.drivingLicenceIssuingNation = new DDDClass.NationNumeric(drivingLicenceIssuingNation);
                efDrivingLicenceInfo.cardDrivingLicenceInformation.drivingLicenceNumber = drivingLicenceNumber;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                return efDrivingLicenceInfo;
            }
            return efDrivingLicenceInfo;
        }
        //EF_Events_Data
        public List<DDDClass.CardEventRecord> Get_EF_Events_Data(int dataBlockId)
        {
            List<DDDClass.CardEventRecord> cardEventData = new List<DDDClass.CardEventRecord>();
            cardEventData = Get_EF_Events_Data(dataBlockId, new DateTime(), DateTime.Now);
            return cardEventData;
        }

        public List<DDDClass.CardEventRecord> Get_EF_Events_Data(int dataBlockId, DateTime startPeriod, DateTime endPeriod)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            List<DDDClass.CardEventRecord> cardEventData = new List<DDDClass.CardEventRecord>();
            DDDClass.CardEventRecord cardEventRecord = new DDDClass.CardEventRecord();
            string paramName = "ef_events_data";
            string currentParamName;

            List<string> eventBeginTime = new List<string>();
            List<string> eventEndTime = new List<string>();
            List<string> eventType = new List<string>();
            List<DDDClass.VehicleRegistrationIdentification> eventVehicleRegistration = new List<DDDClass.VehicleRegistrationIdentification>();

            currentParamName = paramName + ".eventBeginTime";
            eventBeginTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".eventEndTime";
            eventEndTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".eventType";
            eventType = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".eventVehicleRegistration";
            eventVehicleRegistration = GetVehicleRegistrationIdentification(dataBlockId, currentParamName);

            List<int> existingEventsIndexes = new List<int>();
            if (eventBeginTime.Count == eventEndTime.Count && eventType.Count == eventVehicleRegistration.Count)
            {
                for (int i = 0; i < eventBeginTime.Count; i++)
                    if (eventBeginTime[i] != "0" || eventEndTime[i] != "0")
                        existingEventsIndexes.Add(i);
            }
            else throw new Exception("Ошибка, не могу извлечь информацию из базы");

            List<int> Indexes = new List<int>();

            Indexes = CheckDate(eventBeginTime, eventEndTime, startPeriod, endPeriod);

            foreach (int i in existingEventsIndexes)
            {
                if (Indexes.Contains(i))
                {
                    cardEventRecord = new DDDClass.CardEventRecord();

                    cardEventRecord.eventBeginTime = new DDDClass.TimeReal(eventBeginTime[i]);
                    cardEventRecord.eventEndTime = new DDDClass.TimeReal(eventEndTime[i]);
                    cardEventRecord.eventType = new DDDClass.EventFaultType(Convert.ToByte(eventType[i]));
                    cardEventRecord.eventVehicleRegistration = new DDDClass.VehicleRegistrationIdentification();
                    cardEventRecord.eventVehicleRegistration.vehicleRegistrationNation = eventVehicleRegistration[i].vehicleRegistrationNation;
                    cardEventRecord.eventVehicleRegistration.vehicleRegistrationNumber = eventVehicleRegistration[i].vehicleRegistrationNumber;

                    cardEventData.Add(cardEventRecord);
                }
            }
            cardEventData.Sort(Get_EF_Driver_Events_Data_Comparison);

            return cardEventData;
        }
        //EF_Faults_Data
        public List<DDDClass.CardFaultRecord> Get_EF_Faults_Data(int dataBlockId)
        {
            List<DDDClass.CardFaultRecord> cardFaultRecord = new List<DDDClass.CardFaultRecord>();
            cardFaultRecord = Get_EF_Faults_Data(dataBlockId, new DateTime(), DateTime.Now);
            return cardFaultRecord;
        }

        public List<DDDClass.CardFaultRecord> Get_EF_Faults_Data(int dataBlockId, DateTime startPeriod, DateTime endPeriod)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            List<DDDClass.CardFaultRecord> cardFaultData = new List<DDDClass.CardFaultRecord>();
            DDDClass.CardFaultRecord cardFaultRecord = new DDDClass.CardFaultRecord();
            string paramName = "ef_faults_data";
            string currentParamName;

            List<string> faultBeginTime = new List<string>();
            List<string> faultEndTime = new List<string>();
            List<string> faultType = new List<string>();
            List<DDDClass.VehicleRegistrationIdentification> faultVehicleRegistration = new List<DDDClass.VehicleRegistrationIdentification>();


            currentParamName = paramName + ".faultBeginTime";
            faultBeginTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".faultEndTime";
            faultEndTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".faultType";
            faultType = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".faultVehicleRegistration";
            faultVehicleRegistration = GetVehicleRegistrationIdentification(dataBlockId, currentParamName);

            List<int> existingEventsIndexes = new List<int>();
            if (faultBeginTime.Count == faultEndTime.Count && faultType.Count == faultVehicleRegistration.Count)
            {
                for (int i = 0; i < faultBeginTime.Count; i++)
                    if (faultBeginTime[i] != "0" || faultEndTime[i] != "0")
                        existingEventsIndexes.Add(i);
            }
            else throw new Exception("Ошибка, не могу извлечь информацию из базы");
           
            List<int> Indexes = new List<int>();

            Indexes = CheckDate(faultBeginTime, faultEndTime, startPeriod, endPeriod);

            foreach (int i in existingEventsIndexes)
            {
                if (Indexes.Contains(i))
                {
                    cardFaultRecord = new DDDClass.CardFaultRecord();

                    cardFaultRecord.faultBeginTime = new DDDClass.TimeReal(faultBeginTime[i]);
                    cardFaultRecord.faultEndTime = new DDDClass.TimeReal(faultEndTime[i]);
                    cardFaultRecord.faultType = new DDDClass.EventFaultType(Convert.ToByte(faultType[i]));
                    cardFaultRecord.faultVehicleRegistration = new DDDClass.VehicleRegistrationIdentification();
                    cardFaultRecord.faultVehicleRegistration.vehicleRegistrationNation = faultVehicleRegistration[i].vehicleRegistrationNation;
                    cardFaultRecord.faultVehicleRegistration.vehicleRegistrationNumber = faultVehicleRegistration[i].vehicleRegistrationNumber;

                    cardFaultData.Add(cardFaultRecord);
                }
            }

            return cardFaultData;
        }
        //EF_Driver_Activity_Data
        public DDDClass.CardDriverActivity Get_EF_Driver_Activity_Data(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_driver_activity_data";
            string currentParamName;

            DDDClass.CardDriverActivity driverActivityData = new DDDClass.CardDriverActivity();
            DDDClass.CardActivityDailyRecord cardActivityDailyRecord = new DDDClass.CardActivityDailyRecord();

            string activityPointerNewestRecord;
            string activityPointerOldestDayRecord;

            currentParamName = paramName + ".activityPointerNewestRecord";
            activityPointerNewestRecord = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);
            if (activityPointerNewestRecord == " ")
                throw new Exception("Такой информации не существует...");

            currentParamName = paramName + ".activityPointerOldestDayRecord";
            activityPointerOldestDayRecord = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            driverActivityData.activityPointerNewestRecord = Convert.ToInt32(activityPointerNewestRecord);
            driverActivityData.activityPointerOldestDayRecord = Convert.ToInt32(activityPointerOldestDayRecord);

            List<string> activityChangeInfo = new List<string>();
            List<string> activityDailyPresenceCounter = new List<string>();
            List<string> activityDayDistance = new List<string>();
            List<string> activityPreviousRecordLength = new List<string>();
            List<string> activityRecordDate = new List<string>();
            List<string> activityRecordLength = new List<string>();

            paramName += ".activityDailyRecords";

            currentParamName = paramName + ".activityChangeInfo";
            activityChangeInfo = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityDailyPresenceCounter";
            activityDailyPresenceCounter = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityDayDistance";
            activityDayDistance = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityPreviousRecordLength";
            activityPreviousRecordLength = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityRecordDate";
            activityRecordDate = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityRecordLength";
            activityRecordLength = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            int allActivitiesNumbers = 0;
            int tempActivity;
            List<int> activityChangesCount = new List<int>();
            foreach (string changeNumb in activityRecordLength)
            {
                tempActivity = (Convert.ToInt32(changeNumb) - 12);
                if (tempActivity != 0)
                    tempActivity = tempActivity / 2;
                allActivitiesNumbers += tempActivity;
                activityChangesCount.Add(tempActivity);
            }
            if (allActivitiesNumbers != activityChangeInfo.Count)
                throw new Exception("Активности неправильно разобраны!");

            int noOfActivityChangesInt;
            List<DDDClass.ActivityChangeInfo> dayActivityChangeInfo = new List<DDDClass.ActivityChangeInfo>();
            int number = 0;
            if (activityDailyPresenceCounter.Count == activityRecordDate.Count)
            {
                number = 0;
                for (int i = 0; i < activityRecordDate.Count; i++)
                {
                    cardActivityDailyRecord = new DDDClass.CardActivityDailyRecord();

                    cardActivityDailyRecord.activityDailyPresenceCounter = new DDDClass.DailyPresenceCounter(activityDailyPresenceCounter[i]);
                    cardActivityDailyRecord.activityDayDistance = new DDDClass.Distance(activityDayDistance[i]);
                    cardActivityDailyRecord.activityPreviousRecordLength = new DDDClass.CardActivityLengthRange(activityPreviousRecordLength[i]);
                    cardActivityDailyRecord.activityRecordDate = new DDDClass.TimeReal(activityRecordDate[i]);
                    cardActivityDailyRecord.activityRecordLength = new DDDClass.CardActivityLengthRange(activityRecordLength[i]);

                    noOfActivityChangesInt = cardActivityDailyRecord.activityRecordLength.cardActivityLengthRange;
                    dayActivityChangeInfo = new List<DDDClass.ActivityChangeInfo>();
                    for (int j = 0; j < activityChangesCount[i]; j++)
                    {
                        dayActivityChangeInfo.Add(new DDDClass.ActivityChangeInfo(activityChangeInfo[number]));
                        number++;
                    }
                    cardActivityDailyRecord.activityChangeInfo = dayActivityChangeInfo;

                    driverActivityData.activityDailyRecords.Add(cardActivityDailyRecord);
                }
            }
            else throw new Exception("Ошибка в загрузке активностей водителя");

            return driverActivityData;

        }

        public DDDClass.CardDriverActivity Get_EF_Driver_Activity_Data(int dataBlockId, DateTime startPeriod, DateTime endPeriod)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_driver_activity_data";
            string currentParamName;

            DDDClass.CardDriverActivity driverActivityData = new DDDClass.CardDriverActivity();
            DDDClass.CardActivityDailyRecord cardActivityDailyRecord = new DDDClass.CardActivityDailyRecord();

            string activityPointerNewestRecord;
            string activityPointerOldestDayRecord;

            currentParamName = paramName + ".activityPointerNewestRecord";
            activityPointerNewestRecord = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityPointerOldestDayRecord";
            activityPointerOldestDayRecord = sqldbRecords.Get_ParamValue(dataBlockId, currentParamName);

            driverActivityData.activityPointerNewestRecord = Convert.ToInt32(activityPointerNewestRecord);
            driverActivityData.activityPointerOldestDayRecord = Convert.ToInt32(activityPointerOldestDayRecord);

            List<string> activityChangeInfo = new List<string>();
            List<string> activityDailyPresenceCounter = new List<string>();
            List<string> activityDayDistance = new List<string>();
            List<string> activityPreviousRecordLength = new List<string>();
            List<string> activityRecordDate = new List<string>();
            List<string> activityRecordLength = new List<string>();

            paramName += ".activityDailyRecords";

            currentParamName = paramName + ".activityChangeInfo";
            activityChangeInfo = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityDailyPresenceCounter";
            activityDailyPresenceCounter = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityDayDistance";
            activityDayDistance = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityPreviousRecordLength";
            activityPreviousRecordLength = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityRecordDate";
            activityRecordDate = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".activityRecordLength";
            activityRecordLength = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            int allActivitiesNumbers = 0;
            int tempActivity;
            List<int> activityChangesCount = new List<int>();
            foreach (string changeNumb in activityRecordLength)
            {
                tempActivity = (Convert.ToInt32(changeNumb) - 12);
                if (tempActivity != 0)
                    tempActivity = tempActivity / 2;
                allActivitiesNumbers += tempActivity;
                activityChangesCount.Add(tempActivity);
            }
            if (allActivitiesNumbers != activityChangeInfo.Count)
                throw new Exception("Активности неправильно разобраны!");

            List<List<string>> ActivityChangeInfoSorted = GetActivityInfosStartsFromIndex(activityChangeInfo, activityChangesCount);

            int noOfActivityChangesInt;
            List<DDDClass.ActivityChangeInfo> dayActivityChangeInfo = new List<DDDClass.ActivityChangeInfo>();
            if (activityDailyPresenceCounter.Count == activityRecordDate.Count)
            {
                List<int> Indexes = new List<int>();

                Indexes = CheckDate(activityRecordDate, activityRecordDate, startPeriod, endPeriod);

                foreach(int i in Indexes)
                {
                    cardActivityDailyRecord = new DDDClass.CardActivityDailyRecord();

                    cardActivityDailyRecord.activityDailyPresenceCounter = new DDDClass.DailyPresenceCounter(activityDailyPresenceCounter[i]);
                    cardActivityDailyRecord.activityDayDistance = new DDDClass.Distance(activityDayDistance[i]);
                    cardActivityDailyRecord.activityPreviousRecordLength = new DDDClass.CardActivityLengthRange(activityPreviousRecordLength[i]);
                    cardActivityDailyRecord.activityRecordDate = new DDDClass.TimeReal(activityRecordDate[i]);
                    cardActivityDailyRecord.activityRecordLength = new DDDClass.CardActivityLengthRange(activityRecordLength[i]);

                    dayActivityChangeInfo = new List<DDDClass.ActivityChangeInfo>();
                    for (int j = 0; j < ActivityChangeInfoSorted[i].Count; j++)
                    {
                        dayActivityChangeInfo.Add(new DDDClass.ActivityChangeInfo(ActivityChangeInfoSorted[i][j]));
                    }
                    dayActivityChangeInfo.Sort(Get_EF_Driver_Activity_Data_Comparison);
                    cardActivityDailyRecord.activityChangeInfo = dayActivityChangeInfo;

                    driverActivityData.activityDailyRecords.Add(cardActivityDailyRecord);
                }
            }
            else throw new Exception("Ошибка в загрузке активностей водителя");

            return driverActivityData;

        }

        public List<DDDClass.CardDriverActivity> Get_EF_Driver_Activity_Data_byWeeks(int dataBlockId)
        {
            DDDClass.CardDriverActivity driverActivity = Get_EF_Driver_Activity_Data(dataBlockId);
            return Get_EF_Driver_Activity_Data_byWeeks_weeksCalculation(driverActivity);
        }

        public List<DDDClass.CardDriverActivity> Get_EF_Driver_Activity_Data_byWeeks(int dataBlockId, DateTime startPeriod, DateTime endPeriod)
        {
            DDDClass.CardDriverActivity driverActivity = Get_EF_Driver_Activity_Data(dataBlockId, startPeriod, endPeriod);
            return Get_EF_Driver_Activity_Data_byWeeks_weeksCalculation(driverActivity);
        }

        private List<DDDClass.CardDriverActivity> Get_EF_Driver_Activity_Data_byWeeks_weeksCalculation(DDDClass.CardDriverActivity driverActivity)
        {
            DDDClass.CardActivityDailyRecord oneActivity = new DDDClass.CardActivityDailyRecord();
            List<DDDClass.CardActivityDailyRecord> activities = new List<DDDClass.CardActivityDailyRecord>();
            DDDClass.CardDriverActivity weekActivityList = new DDDClass.CardDriverActivity();
            List<DDDClass.CardDriverActivity> weeksList = new List<DDDClass.CardDriverActivity>();

            if (driverActivity.activityDailyRecords.Count <= 0)
                return weeksList;

            //система такая. Если предудыщий день больше настроящего, то новая неделя
            /*      DayOfWeek currentDay = new DayOfWeek();
                  DayOfWeek previosDayOfWeek = new DayOfWeek();
                  previosDayOfWeek = driverActivity.activityDailyRecords[0].activityRecordDate.getTimeRealDate().DayOfWeek;
                  //currentDay = driverActivity.activityDailyRecords[0].activityRecordDate.getTimeRealDate().DayOfWeek;
                  weekActivityList.activityDailyRecords.Add(driverActivity.activityDailyRecords[0]);
                  driverActivity.activityDailyRecords.RemoveAt(0);
                  bool nextweek = false;
                  foreach (DDDClass.CardActivityDailyRecord record in driverActivity.activityDailyRecords)
                  {
                      currentDay = record.activityRecordDate.getTimeRealDate().DayOfWeek;

                      if (previosDayOfWeek >= currentDay)
                      {
                          nextweek = true;                    
                      }
                      previosDayOfWeek = currentDay;
                      if (nextweek == true)
                      {                   
                          weeksList.Add(weekActivityList);
                          weekActivityList = new DDDClass.CardDriverActivity();
                          nextweek = false;
                      }
                      weekActivityList.activityDailyRecords.Add(record);
                  }
                  */
            /////////////////////test///]
            for (int i = 0; i < driverActivity.activityDailyRecords.Count;)
            {
                weekActivityList = new DDDClass.CardDriverActivity();
                try
                {
                    if (driverActivity.activityDailyRecords[i].activityRecordDate.getTimeRealDate().DayOfWeek == DayOfWeek.Monday)
                    {
                        weekActivityList.activityDailyRecords.Add(driverActivity.activityDailyRecords[i]);
                        i++;
                    }
                    if (driverActivity.activityDailyRecords[i].activityRecordDate.getTimeRealDate().DayOfWeek == DayOfWeek.Tuesday)
                    {
                        weekActivityList.activityDailyRecords.Add(driverActivity.activityDailyRecords[i]);
                        i++;
                    }
                    if (driverActivity.activityDailyRecords[i].activityRecordDate.getTimeRealDate().DayOfWeek == DayOfWeek.Wednesday)
                    {
                        weekActivityList.activityDailyRecords.Add(driverActivity.activityDailyRecords[i]);
                        i++;
                    }
                    if (driverActivity.activityDailyRecords[i].activityRecordDate.getTimeRealDate().DayOfWeek == DayOfWeek.Thursday)
                    {
                        weekActivityList.activityDailyRecords.Add(driverActivity.activityDailyRecords[i]);
                        i++;
                    }
                    if (driverActivity.activityDailyRecords[i].activityRecordDate.getTimeRealDate().DayOfWeek == DayOfWeek.Friday)
                    {
                        weekActivityList.activityDailyRecords.Add(driverActivity.activityDailyRecords[i]);
                        i++;
                    }
                    if (driverActivity.activityDailyRecords[i].activityRecordDate.getTimeRealDate().DayOfWeek == DayOfWeek.Saturday)
                    {
                        weekActivityList.activityDailyRecords.Add(driverActivity.activityDailyRecords[i]);
                        i++;
                    }
                    if (driverActivity.activityDailyRecords[i].activityRecordDate.getTimeRealDate().DayOfWeek == DayOfWeek.Sunday)
                    {
                        weekActivityList.activityDailyRecords.Add(driverActivity.activityDailyRecords[i]);
                        i++;
                    }
                }
                catch
                { }
                weeksList.Add(weekActivityList);
            }
            ///////////////////////////
            return weeksList;
        }
        //EF_Vehicles_Used
        public List<DDDClass.CardVehicleRecord> Get_EF_Vehicles_Used(int dataBlockId)
        {
            List<int> dataBlockIds = new List<int>();
            dataBlockIds.Add(dataBlockId);
            return Get_EF_Vehicles_Used(dataBlockIds, new DateTime(), DateTime.Now);
        }

        public List<DDDClass.CardVehicleRecord> Get_EF_Vehicles_Used(List<int> dataBlockIds)
        {
            return Get_EF_Vehicles_Used(dataBlockIds, new DateTime(), DateTime.Now);
        }

        public List<DDDClass.CardVehicleRecord> Get_EF_Vehicles_Used(List<int> dataBlockIds, DateTime startPeriod, DateTime endPeriod)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_vehicles_used";
            string curentParam;
            int dataBlockId = dataBlockIds[0];

            List<DDDClass.CardVehicleRecord> ef_cardVehicle_used = new List<DDDClass.CardVehicleRecord>();
            List<string> vehicleOdometerBegin = new List<string>();
            List<string> vehicleOdometerEnd = new List<string>();
            List<string> vehicleFirstUse = new List<string>();
            List<string> vehicleLastUse = new List<string>();
            List<string> vehicleRegistrationNation = new List<string>();
            List<string> vehicleRegNumber = new List<string>();


            curentParam = paramName + ".vehicleOdometerBegin";
            vehicleOdometerBegin = sqldbRecords.Get_AllParamsArray(dataBlockId, curentParam);

            curentParam = paramName + ".vehicleOdometerEnd";
            vehicleOdometerEnd = sqldbRecords.Get_AllParamsArray(dataBlockId, curentParam);

            curentParam = paramName + ".vehicleFirstUse";
            vehicleFirstUse = sqldbRecords.Get_AllParamsArray(dataBlockId, curentParam);

            curentParam = paramName + ".vehicleLastUse";
            vehicleLastUse = sqldbRecords.Get_AllParamsArray(dataBlockId, curentParam);

            curentParam = paramName + ".vehicleRegistration.vehicleRegistrationNation";
            vehicleRegistrationNation = sqldbRecords.Get_AllParamsArray(dataBlockId, curentParam);

            curentParam = paramName + ".vehicleRegistration.vehicleRegistrationNumber.vehicleRegNumber";
            vehicleRegNumber = sqldbRecords.Get_AllParamsArray(dataBlockId, curentParam);

            DDDClass.CardVehicleRecord cardVehiclesUsed;
            byte[] _bytes;

            List<int> Indexes = new List<int>();

            Indexes = CheckDate(vehicleFirstUse, vehicleLastUse, startPeriod, endPeriod);

            if (vehicleOdometerBegin.Count == vehicleOdometerEnd.Count && vehicleLastUse.Count == vehicleRegNumber.Count)
            {
                foreach (int i in Indexes)
                {
                    cardVehiclesUsed = new DDDClass.CardVehicleRecord();
                    cardVehiclesUsed.vehicleFirstUse = new DDDClass.TimeReal(Convert.ToUInt32(vehicleFirstUse[i]));
                    cardVehiclesUsed.vehicleLastUse = new DDDClass.TimeReal(Convert.ToUInt32(vehicleLastUse[i]));
                    cardVehiclesUsed.vehicleOdometerBegin = new DDDClass.OdometerShort();
                    cardVehiclesUsed.vehicleOdometerEnd = new DDDClass.OdometerShort();
                    cardVehiclesUsed.vehicleOdometerBegin.odometerShort = Convert.ToInt32(vehicleOdometerBegin[i]);
                    cardVehiclesUsed.vehicleOdometerEnd.odometerShort = Convert.ToInt32(vehicleOdometerEnd[i]);
                    cardVehiclesUsed.vehicleRegistration = new DDDClass.VehicleRegistrationIdentification();
                    cardVehiclesUsed.vehicleRegistration.vehicleRegistrationNation.nationNumeric = short.Parse(vehicleRegistrationNation[i]);

                    System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                    _bytes = enc.GetBytes(vehicleRegNumber[i]);
                    cardVehiclesUsed.vehicleRegistration.vehicleRegistrationNumber.vehicleRegNumber = _bytes;

                    ef_cardVehicle_used.Add(cardVehiclesUsed);
                }
            }
            else throw new Exception("Нельзя извлечь информацию об использовании ТС");

            return ef_cardVehicle_used;
        }
        //EF_Places
        public DDDClass.CardPlaceDailyWorkPeriod Get_EF_Places(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_places";
            string currentParam;

            DDDClass.CardPlaceDailyWorkPeriod cardPlaceDailyWorkPeriod = new DDDClass.CardPlaceDailyWorkPeriod();
            DDDClass.PlaceRecord placeRecord = new DDDClass.PlaceRecord();

            List<string> dailyWorkPeriodCountry = new List<string>();
            List<string> dailyWorkPeriodRegion = new List<string>();
            List<string> entryTime = new List<string>();
            List<string> entryTypeDailyWorkPeriod = new List<string>();
            List<string> vehicleOdometerValue = new List<string>();

            currentParam = paramName + ".dailyWorkPeriodCountry";
            dailyWorkPeriodCountry = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParam);

            currentParam = paramName + ".dailyWorkPeriodRegion";
            dailyWorkPeriodRegion = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParam);

            currentParam = paramName + ".entryTime";
            entryTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParam);

            currentParam = paramName + ".entryTypeDailyWorkPeriod";
            entryTypeDailyWorkPeriod = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParam);

            currentParam = paramName + ".vehicleOdometerValue";
            vehicleOdometerValue = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParam);


            if (dailyWorkPeriodCountry.Count == dailyWorkPeriodRegion.Count && entryTime.Count == entryTypeDailyWorkPeriod.Count)
            {
                for (int i = 0; i < dailyWorkPeriodCountry.Count; i++)
                {
                    placeRecord = new DDDClass.PlaceRecord();

                    placeRecord.dailyWorkPeriodCountry = new DDDClass.NationNumeric(dailyWorkPeriodCountry[i]);
                    placeRecord.dailyWorkPeriodRegion = new DDDClass.RegionNumeric(dailyWorkPeriodRegion[i]);
                    placeRecord.entryTime = new DDDClass.TimeReal(entryTime[i]);
                    placeRecord.entryTypeDailyWorkPeriod = new DDDClass.EntryTypeDailyWorkPeriod(entryTypeDailyWorkPeriod[i]);
                    placeRecord.vehicleOdometerValue = new DDDClass.OdometerShort(vehicleOdometerValue[i]);

                    cardPlaceDailyWorkPeriod.placeRecords.Add(placeRecord);
                }
            }

            return cardPlaceDailyWorkPeriod;
        }
        //EF_Current_Usage
        public DDDClass.CardCurrentUse Get_EF_Current_Usage(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_current_usage";
            string currentParam;

            DDDClass.CardCurrentUse cardCurrentUse = new DDDClass.CardCurrentUse();

            string sessionOpenTime;
            List<DDDClass.VehicleRegistrationIdentification> sessionOpenVehicle = new List<DDDClass.VehicleRegistrationIdentification>();

            currentParam = paramName + ".sessionOpenTime";
            sessionOpenTime = sqldbRecords.Get_ParamValue(dataBlockId, currentParam);

            currentParam = paramName + ".sessionOpenVehicle";
            sessionOpenVehicle = GetVehicleRegistrationIdentification(dataBlockId, currentParam);

            if (sessionOpenVehicle.Count != 0)
                cardCurrentUse.sessionOpenVehicle = sessionOpenVehicle[0];
            else
                cardCurrentUse.sessionOpenVehicle = new DDDClass.VehicleRegistrationIdentification();
            cardCurrentUse.sessionOpenTime = new DDDClass.TimeReal(sessionOpenTime);

            return cardCurrentUse;
        }
        //EF_Control_Activity_Data
        public DDDClass.CardControlActivityDataRecord Get_EF_Control_Activity_Data(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_control_activity_data";
            string currentParam;

            DDDClass.CardControlActivityDataRecord cardControlActivityDataRecord = new DDDClass.CardControlActivityDataRecord();

            List<DDDClass.FullCardNumber> controlCardNumber = new List<DDDClass.FullCardNumber>();
            string controlDownloadPeriodBegin;
            string controlDownloadPeriodEnd;
            string controlTime;
            string controlType;
            List<DDDClass.VehicleRegistrationIdentification> controlVehicleRegistration = new List<DDDClass.VehicleRegistrationIdentification>();

            currentParam = paramName + ".controlCardNumber";
            controlCardNumber = GetCardFullNumber(dataBlockId, currentParam);

            currentParam = paramName + ".controlVehicleRegistration";
            controlVehicleRegistration = GetVehicleRegistrationIdentification(dataBlockId, currentParam);

            currentParam = paramName + ".controlDownloadPeriodBegin";
            controlDownloadPeriodBegin = sqldbRecords.Get_ParamValue(dataBlockId, currentParam);

            currentParam = paramName + ".controlDownloadPeriodEnd";
            controlDownloadPeriodEnd = sqldbRecords.Get_ParamValue(dataBlockId, currentParam);

            currentParam = paramName + ".controlTime";
            controlTime = sqldbRecords.Get_ParamValue(dataBlockId, currentParam);

            currentParam = paramName + ".controlType";
            controlType = sqldbRecords.Get_ParamValue(dataBlockId, currentParam);

            if (controlCardNumber.Count != 0 && controlVehicleRegistration.Count != 0)
            {
                cardControlActivityDataRecord.controlCardNumber = controlCardNumber[0];
                cardControlActivityDataRecord.controlVehicleRegistration = controlVehicleRegistration[0];
            }
            else
            {
                cardControlActivityDataRecord.controlCardNumber = new DDDClass.FullCardNumber();
                cardControlActivityDataRecord.controlVehicleRegistration = new DDDClass.VehicleRegistrationIdentification();
            }
            cardControlActivityDataRecord.controlDownloadPeriodBegin = new DDDClass.TimeReal(controlDownloadPeriodBegin);
            cardControlActivityDataRecord.controlDownloadPeriodEnd = new DDDClass.TimeReal(controlDownloadPeriodEnd);
            cardControlActivityDataRecord.controlTime = new DDDClass.TimeReal(controlTime);
            cardControlActivityDataRecord.controlType = new DDDClass.ControlType(controlType);

            return cardControlActivityDataRecord;
        }
        //EF_Specific_Conditions
        public List<DDDClass.SpecificConditionRecord> Get_EF_Specific_Conditions(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_specific_conditions";
            string currentParam;

            List<DDDClass.SpecificConditionRecord> specificConditionData = new List<DDDClass.SpecificConditionRecord>();
            DDDClass.SpecificConditionRecord specificConditionRecord = new DDDClass.SpecificConditionRecord();

            List<string> entryTime = new List<string>();
            List<string> specificConditionType = new List<string>();

            currentParam = paramName + ".entryTime";
            entryTime = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParam);

            currentParam = paramName + ".specificConditionType";
            specificConditionType = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParam);

            if (entryTime.Count == specificConditionType.Count)
            {
                for (int i = 0; i < entryTime.Count; i++)
                {
                    specificConditionRecord = new DDDClass.SpecificConditionRecord();

                    specificConditionRecord.entryTime = new DDDClass.TimeReal(entryTime[i]);
                    specificConditionRecord.specificConditionType = new DDDClass.SpecificConditionType(specificConditionType[i]);

                    specificConditionData.Add(specificConditionRecord);
                }
            }
            else throw new Exception("Ошибка, извлечение из БД информации об особых состояниях неуспешно!");

            return specificConditionData;
        }
        //Это не вся информация. Есть еще, смотреть CardUnit.CardUnitClass - нет информации в картах для отладки.

        public List<DateTime> Get_StartEndPeriod(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string paramName = "ef_driver_activity_data.activityDailyRecords";
            string currentParamName;
            List<DateTime> returnPeriod = new List<DateTime>();
            List<string> activityRecordDate = new List<string>();

            currentParamName = paramName + ".activityRecordDate";
            activityRecordDate = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);
            DateTime date;
            if (activityRecordDate.Count > 0)
            {
                date = new DateTime();
                date = new DDDClass.TimeReal(activityRecordDate[0]).getTimeRealDate();
                returnPeriod.Add(date);
                date = new DDDClass.TimeReal(activityRecordDate[activityRecordDate.Count - 1]).getTimeRealDate();
                returnPeriod.Add(date);
            }
            else
            {
                date = new DateTime();
                returnPeriod.Add(date);
                date = new DateTime();
                returnPeriod.Add(date);
            }
            return returnPeriod;
        }

        public CardUnit.CardUnitClass GetAllCardUnitClass_parsingDataBlock(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string fileName = "";
            PARSER.DDDParser dddParser = new PARSER.DDDParser();

            fileName = sqldbRecords.Get_ParamValue(dataBlockId, "DataBlock_FileName");
            byte[] blockDataBlob = sqlDB.GetDataBlock(dataBlockId);
            blockDataBlob = UnZipBytes(blockDataBlob);//АнЗипим файл!
            dddParser.ParseFile(blockDataBlob, fileName);

            return dddParser.cardUnitClass;
        }
        private byte[] UnZipBytes(byte[] _bytes)
        {
            byte[] bar = Compressor.Compressor.Decompress(_bytes);
            return bar;
        }
        //Расчет значений процентного содержания информации от всех дней в году/месяце/дне
        /// <summary>
        /// Получает проценты информации в карточке относительно минут в дне
        /// </summary>
        /// <param name="date">дата(год, месяц, день)</param>
        /// <param name="datablockId">ID файла, для которого нужно подсчитать</param>
        /// <returns>double - проценты</returns>
        public double Statistics_GetDayStatistics(DateTime date, int datablockId)//Проверить функции activities.GetTotalTime() - писалась давно, может не точно подсчитывать!
        {
            double stat = 0;
            int minutesInDay = 1440;
            DDDClass.CardDriverActivity activities = new DDDClass.CardDriverActivity();
            activities = Get_EF_Driver_Activity_Data(datablockId, date.Date, date);
            
            stat = (activities.GetTotalTime().TotalMinutes/minutesInDay)*100;
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
            double stat = 0;
            int minutesInDay = 1440;
            int dayInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            DDDClass.CardDriverActivity activities = new DDDClass.CardDriverActivity();
            activities = Get_EF_Driver_Activity_Data(datablockId, new DateTime(date.Year, date.Month, 1), new DateTime(date.Year, date.Month, dayInMonth));

            stat = (activities.GetTotalTime().TotalMinutes / (minutesInDay*dayInMonth)) * 100;
            return stat;
        }
        /// <summary>
        /// Получает проценты информации в карточке относительно дней в году
        /// </summary>
        /// <param name="date">Дата(год)</param>
        /// <param name="datablockId">ID файла, для которого нужно подсчитать</param>
        /// <returns>double - проценты</returns>
        public double Statistics_GetYearStatistics(DateTime date, int datablockId)//Проверить функции activities.GetTotalTime() - писалась давно, может не точно подсчитывать!
        {
            double stat = 0;
            int minutesInDay = 1440;
            int dayInYear = GetDaysInAYear(date.Year);
            DDDClass.CardDriverActivity activities = new DDDClass.CardDriverActivity();
            activities = Get_EF_Driver_Activity_Data(datablockId, new DateTime(date.Year, 1, 1), new DateTime(date.Year, 12, DateTime.DaysInMonth(date.Year, 12)));

            stat = (activities.GetTotalTime().TotalMinutes / (minutesInDay * dayInYear)) * 100;
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

        //-------------------------------------------
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

        private List<DDDClass.CardNumber> GetCardNumber(int dataBlockId, string paramName)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDB.GETMYSQLCONNECTION());
            string currentParamName;
            List<DDDClass.CardNumber> cardNumberList = new List<DDDClass.CardNumber>();
            DDDClass.CardNumber cardNumber = new DDDClass.CardNumber();

            List<string> cardConsecutiveIndex = new List<string>();
            List<string> cardRenewalIndex = new List<string>();
            List<string> cardReplacementIndex = new List<string>();
            List<string> driverIdentification = new List<string>();
            List<string> ownerIdentification = new List<string>();

            currentParamName = paramName + ".cardConsecutiveIndex";
            cardConsecutiveIndex = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardRenewalIndex";
            cardRenewalIndex = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".cardReplacementIndex";
            cardReplacementIndex = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".driverIdentification";
            driverIdentification = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            currentParamName = paramName + ".ownerIdentification";
            ownerIdentification = sqldbRecords.Get_AllParamsArray(dataBlockId, currentParamName);

            if (cardRenewalIndex.Count == ownerIdentification.Count && driverIdentification.Count == cardConsecutiveIndex.Count)
            {
                for (int i = 0; i < cardRenewalIndex.Count; i++)
                {
                    cardNumber = new DDDClass.CardNumber();

                    cardNumber.cardConsecutiveIndex = new DDDClass.CardConsecutiveIndex(cardConsecutiveIndex[i]);
                    cardNumber.cardRenewalIndex = new DDDClass.CardRenewalIndex(cardRenewalIndex[i]);
                    cardNumber.cardReplacementIndex = new DDDClass.CardReplacementIndex(cardReplacementIndex[i]);
                    cardNumber.driverIdentification = driverIdentification[i];
                    cardNumber.ownerIdentification = ownerIdentification[i];

                    cardNumberList.Add(cardNumber);
                }
            }
            return cardNumberList;
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

        private List<List<string>> GetActivityInfosStartsFromIndex(List<string> ActivityChangeInfo, List<int> activityChangesCount)
        {
            List<List<string>> activityReturn = new List<List<string>>(); ;

            List<string> dayActivities = new List<string>();
            int number = 0;
            for (int i = 0; i < activityChangesCount.Count; i++)
            {
                dayActivities = new List<string>();
                for (int j = 0; j < activityChangesCount[i]; j++)
                {
                    dayActivities.Add(ActivityChangeInfo[number]);
                    number++;
                }
                activityReturn.Add(dayActivities);
            }
            return activityReturn;
        }
       //comparison Functions!
        private int Get_EF_Driver_Activity_Data_Comparison(DDDClass.ActivityChangeInfo x, DDDClass.ActivityChangeInfo y)
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

        private int Get_EF_Driver_Events_Data_Comparison(DDDClass.CardEventRecord x, DDDClass.CardEventRecord y)
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
    }
}
