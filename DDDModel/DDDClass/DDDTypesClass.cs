using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;


namespace DDDClass
{
    [Obsolete("Устаревший тип, можно удалить!", true)]
    public class DDDTypesClass
    {       
        
        //-------------------Переменные----------------------------------
        private byte CalibrationPurpose;
        private int CardActivityLengthRange;
        private string CardApprovalNumber;
        private byte[] CardCertificate;//Certificate
        private string CardConsecutiveIndex;
        private byte CardPrivateKey;//RSAKeyPrivateExponent
        private PublicKey CardPublicKey;
        private string CardRenewalIndex;
        private string CardReplacementIndex;
        private short CardSlotNumber;
        private byte CardSlotStatus;
        private byte[] CardStructureVersion;
        private byte[] Certificate;
        private short CompanyActivityType;
        private long CurrentDateTime;//TimeReal
        private byte[] DailyPresenceCounter;
        private int Distance;
        short EntryTypeDailyWorkPeriod;
        short EquipmentType;
        PublicKey EuropeanPublicKey;
        byte EventFaultRecordPurpose;
        byte EventFaultType;
        int K_ConstantOfRecordingEquipment;
        private int L_TyreCircumference;
        private string Language;
        private long LastCardDownload;//TimeReal
        private bool ManualInputFlag;
        private short ManufacturerCode;
        byte[] MemberStateCertificate;//Certificate
        PublicKey MemberStatePublicKey;
        string NationAlpha;
        short NationNumeric;
        short NoOfCalibrationRecords;
        int NoOfCalibrationsSinceDownload;
        short NoOfCardPlaceRecords;
        int NoOfCardVehicleRecords;
        int NoOfCompanyActivityRecords;
        int NoOfControlActivityRecords;
        short NoOfEventsPerType;
        short NoOfFaultsPerType;
        int OdometerShort;
        int OdometerValueMidnight;//OdometerShort
        short OverspeedNumber;
        string RegionAlpha;
        byte RegionNumeric;
        byte RSAKeyModulus;
        byte RSAKeyPrivateExponent;
        byte RSAKeyPublicExponent;
        string SensorApprovalNumber;
        TDesSessionKey SensorInstallationSecData;
        long SensorPairingDate;//TimeReal
        ExtendedSerialNumber SensorSerialNumber;
        byte[] Signature;
        short SimilarEventsNumber;
        short SpecificConditionType;
        short Speed;
        short SpeedAuthorised;//Speed
        short SpeedAverage;//Speed
        short SpeedMax;//Speed
        long TimeReal;
        string TyreSize;
        string VehicleIdentificationNumber;
        string VuApprovalNumber;
        byte[] VuCertificate;//Certificate
        byte[] VuDataBlockCounter;
        Address VuManufacturerAddress;
        Name VuManufacturerName;
        long VuManufacturingDate;//TimeReal
        string VuPartNumber;
        byte VuPrivateKey;//RSAKeyPrivateExponent
        PublicKey VuPublicKey;
        ExtendedSerialNumber VuSerialNumber;
        long VuSoftInstallationDate;//TimeReal
        string VuSoftwareVersion;
        int W_VehicleCharacteristicConstant;


        //-------------------Структуры---------------------------------------
        public struct ActivityChangeInfo
        {
            public byte[] value;
            public bool slot;
            public bool drivingStatus;
            public bool cardStatus;
            public byte activity;
            public int time;
        }     
   
        public struct Address
        {
            public short codePage { get; set; }
            public byte[] address { get; set; }            
        }       

        public struct CardActivityDailyRecord
        {

            public int activityPreviousRecordLength;//CardActivityLengthRange
            public int activityRecordLength;//CardActivityLengthRange
            public long activityRecordDate;//TimeReal
            public byte[] activityDailyPresenceCounter;//DailyPresenceCounter
            public int activityDayDistance;//Distance
            public List<ActivityChangeInfo> activityChangeInfo;
        }

        public struct CardChipIdentification
        {
            public byte[] icSerialNumber;
            public byte[] icManufacturingReferences;
        }      

        public struct CardControlActivityDataRecord
        {
            public ControlType controlType;
            public long controlTime; //TimeReal
            public FullCardNumber controlCardNumber;
            public VehicleRegistrationIdentification controlVehicleRegistration;
            public long controlDownloadPeriodBegin;//TimeReal
            public long controlDownloadPeriodEnd;//TimeReal
        }

        public struct CardCurrentUse
        {
            public long sessionOpenTime;//TimeReal
            public VehicleRegistrationIdentification sessionOpenVehicle;
        }

        public struct CardDriverActivity
        {
            public int activityPointerOldestDayRecord;
            public int activityPointerNewestRecord;
            public List<CardActivityDailyRecord> activityDailyRecords;
        }

        public struct CardDrivingLicenceInformation
        {
            public Name drivingLicenceIssuingAuthority;
            public short drivingLicenceIssuingNation;//NationNumeric
            public string drivingLicenceNumber;
        }

        public struct CardEventRecords
        {
            public List<List<CardEventRecord>> cardEventRecords;
        }

        public struct CardEventRecord
        {
            public byte eventType;//EventFaultType
            public long eventBeginTime;//TimeReal
            public long eventEndTime;//TimeReal
            public VehicleRegistrationIdentification eventVehicleRegistration;
        }

        public struct CardFaultData
        {
            public List<List<CardFaultRecord>> cardFaultRecords;
        }

        public struct CardFaultRecord
        {
            public byte faultType;//EventFaultType
            public long faultBeginTime;//TimeReal
            public long faultEndTime;//TimeReal
            public VehicleRegistrationIdentification faultVehicleRegistration;
        }

        public struct CardIccIdentification
        {
            public byte clockStop;
            public ExtendedSerialNumber cardExtendedSerialNumber;
            public string cardApprovalNumber;//CardApprovalNumber
            public byte cardPersonaliserID;
            public byte[] embedderIcAssemblerId;
            public byte[] icIdentifier;
        }

        public struct CardIdentification
        {
            public short cardIssuingMemberState;//NationNumeric
            public CardNumber cardNumber;
            public Name cardIssuingAuthorityName;
            public long cardIssueDate;//TimeReal
            public long cardValidityBegin;//TimeReal
            public long cardExpiryDate;//TimeReal
        }

        public struct CardNumber //Это тип для водительских карт, для 
        {                        //мастерских, контрольных и карт компании
                                 //типы другие(закоменчены)
            public string driverIdentification;
            public string cardReplacementIndex;//CardReplacementIndex
            public string cardRenewalIndex;//CardRenewalIndex
            /*
                public string ownerIdentification;
                public CardConsecutiveIndex cardConsecutiveIndex;
                public string cardReplacementIndex; //CardReplacementIndex
                public string cardRenewalIndex;//CardRenewalIndex
            */
        }

        public struct CardPlaceDailyWorkPeriod
        {
            public List<PlaceRecord> placeRecords;
        }       

        public struct CardVehicleRecord
        {
            public int vehicleOdometerBegin;//OdometerShort
            public int vehicleOdometerEnd;//OdometerShort
            public long vehicleFirstUse;//TimeReal
            public long vehicleLastUse;//TimeReal
            public VehicleRegistrationIdentification vehicleRegistration;
            public byte[] vuDataBlockCounter;//VuDataBlockCounter
        }

        public struct CardVehiclesUsed
        {
            public List<CardVehicleRecord> cardVehicleRecords;
        }

        public struct CertificateContent
        {
            public short certificateProfileIdentifier;
            public KeyIdentifier certificationAuthorityReference;
            public CertificateHolderAuthorisation certificateHolderAuthorisation;
            public long certificateEndOfValidity;//TimeReal
            public KeyIdentifier certificateHolderReference;
            public PublicKey publicKey;
        }

        public struct CertificateHolderAuthorisation
        {
            public byte[] tachographApplicationID;
            public short equipmentType;//EquipmentType
        }

        public struct CertificateRequestID
        {
            public long requestSerialNumber;
            public byte[] requestMonthYear;
            public byte crIdentifier;
            public short manufacturerCode;//ManufacturerCode
        }

        public struct CertificationAuthorityKID
        {
            public short nationNumeric;//NationNumeric
            public string nationAlpha;//NationAlpha
            public short keySerialNumber;
            public byte[] additionalInfo;
            public byte caIdentifier;
        }

        public struct CompanyActivityData
        {
            public List<CompanyActivityRecord> companyActivityRecords;
        }

        public struct CompanyActivityRecord
        {
            public short companyActivityType;//CompanyActivityType
            public long companyActivityTime;//TimeReal
            public FullCardNumber cardNumberInformation;
            public VehicleRegistrationIdentification vehicleRegistrationInformation;
            public long downloadPeriodBegin;//TimeReal
            public long downloadPeriodEnd;//TimeReal
        }

        public struct CompanyCardApplicationIdentification
        {
            public short typeOfTachographCardId;//EquipmentType
            public byte[] cardStructureVersion;//CardStructureVersion
            public int noOfCompanyActivityRecords;//NoOfCompanyActivityRecords
        }

        public struct CompanyCardHolderIdentification
        {
            public Name companyName;
            public Address companyAddress;
            public string cardHolderPreferredLanguage;//Language
        }

        public struct ControlCardApplicationIdentification
        {
            public short typeOfTachographCardId;//EquipmentType
            public byte[] cardStructureVersion;//CardStructureVersion
            public int noOfControlActivityRecords;//NoOfControlActivityRecords
        }

        public struct ControlCardControlActivityData
        {
            public List<ControlActivityRecord> controlActivityRecords;
        }

        public struct ControlActivityRecord
        {
            public ControlType controlType;
            public long controlTime;//TimeReal
            public FullCardNumber controlledCardNumber;
            public VehicleRegistrationIdentification controlledVehicleRegistration;
            public long controlDownloadPeriodBegin;//TimeReal
            public long controlDownloadPeriodEnd;//TimeReal
        }

        public struct ControlCardHolderIdentification
        {
            public Name controlBodyName;
            public Address controlBodyAddress;
            public HolderName cardHolderName;
            public string cardHolderPreferredLanguage;//Language
        }

        public struct ControlType
        {
            public byte value;
            public bool card_downloading;
            public bool vu_downloading;
            public bool display;
            public bool printing;
        }

        public struct Datef
        {
            public byte[] year;
            public byte month;
            public byte day;
        }       

        public struct DriverCardApplicationIdentification
        {
            public short typeOfTachographCardId;//EquipmentType
            public byte[] cardStructureVersion;//CardStructureVersion
            public short noOfEventsPerType;//NoOfEventsPerType
            public short noOfFaultsPerType;//NoOfFaultsPerType
            public int activityStructureLength; //CardActivityLengthRange
            public int noOfCardVehicleRecords;//NoOfCardVehicleRecords
            public short noOfCardPlaceRecords;//NoOfCardPlaceRecords
        }

        public struct DriverCardHolderIdentification
        {
            public HolderName cardHolderName;
            public Datef cardHolderBirthDate;
            public string cardHolderPreferredLanguage;//Language
        }

        public struct ExtendedSerialNumber
        {
            public long serialNumber;
            public byte[] monthYear;
            public byte type;
            public short manufacturerCode;//ManufacturerCode
        }

        public struct FullCardNumber
        {
            public short cardType;//EquipmentType
            public short cardIssuingMemberState;//NationNumeric
            public CardNumber cardNumber;
        }

        public struct HolderName
        {
            public Name holderSurname;
            public Name holderFirstName;
        }       

        public struct KeyIdentifier
        {
            public ExtendedSerialNumber extendedSerialNumber;
            public CertificateRequestID certificateRequestID;
            public CertificationAuthorityKID certificationAuthorityKID;
        }       

        public struct Name
        {
            public short codePage;
            public byte[] name;
        }   

        public struct PlaceRecord
        {
            public long entryTime;//TimeReal
            public short entryTypeDailyWorkPeriod;//EntryTypeDailyWorkPeriod
            public short dailyWorkPeriodCountry;//NationNumeric
            public byte dailyWorkPeriodRegion;//RegionNumeric
            public int vehicleOdometerValue;//OdometerShort
        }

        public struct PreviousVehicleInfo
        {
            public VehicleRegistrationIdentification vehicleRegistrationIdentification;
            public long cardWithdrawalTime;//TimeReal
        }

        public struct PublicKey
        {
            public byte rsaKeyModulus;//RSAKeyModulus
            public byte rsaKeyPublicExponent;//RSAKeyPublicExponent
        }       

        public struct SensorPaired
        {
            public ExtendedSerialNumber sensorSerialNumber;//SensorSerialNumber
            public string sensorApprovalNumber;//SensorApprovalNumber
            public long sensorPairingDateFirst;//SensorPairingDate
        }             

        public struct SpecificConditionRecord
        {
            public long entryTime;//TimeReal
            public short specificConditionType;//SpecificConditionType
        }

        public struct TDesSessionKey
        {
            public byte[] tDesKeyA;
            public byte[] tDesKeyB;
        }

        public struct VehicleRegistrationIdentification
        {
            public short vehicleRegistrationNation;//NationNumeric
            public VehicleRegistrationNumber vehicleRegistrationNumber;
        }

        public struct VehicleRegistrationNumber
        {
            public short codePage;
            public byte[] vehicleRegNumber;
        }

        public struct VuActivityDailyData
        {
            public int noOfActivityChanges;
            public List<ActivityChangeInfo> ActivityChangeInfos;
        }

        public struct VuCalibrationData
        {
            public short noOfVuCalibrationRecords;
            public List<VuCalibrationRecord> vuCalibrationRecords;
        }

        public struct VuCalibrationRecord
       {
           public byte calibrationPurpose;//CalibrationPurpose
           public Name workshopName;
           public Address workshopAddress;
           public FullCardNumber workshopCardNumber;
           public long workshopCardExpiryDate;//TimeReal
           public string vehicleIdentificationNumber;//VehicleIdentificationNumber
           public VehicleRegistrationIdentification vehicleRegistrationIdentification;
           public int wVehicleCharacteristicConstant;//W_VehicleCharacteristicConstant
           public int kConstantOfRecordingEquipment;//K_ConstantOfRecordingEquipment
           public int lTyreCircumference;//L_TyreCircumference
           public string tyreSize;//TyreSize
           public short authorisedSpeed;//SpeedAuthorised
           public int oldOdometerValue; //OdometerShort
           public int newOdometerValue;//OdometerShort
           public long oldTimeValue;//TimeReal
           public long newTimeValue;//TimeReal
           public long nextCalibrationDate;//TimeReal
       
       }

        public struct VuCardIWData
       {
           public int noOfIWRecords;
           public List<VuCardIWRecord> vuCardIWRecords;
       }

        public struct VuCardIWRecord
       {
           public HolderName cardHolderName;
           public FullCardNumber fullCardNumber;
           public long cardExpiryDate;//TimeReal
           public long cardInsertionTime;//TimeReal
           public int vehicleOdometerValueAtInsertion;//OdometerShort
           public short cardSlotNumber;//CardSlotNumber
           public long cardWithdrawalTime;//TimeReal
           public int vehicleOdometerValueAtWithdrawal;//OdometerShort
           public PreviousVehicleInfo previousVehicleInfo;
           public bool manualInputFlag;//ManualInputFlag
       }       

        public struct VuCompanyLocksData
       {
           public short noOfLocks;
           public List<VuCompanyLocksRecord> vuCompanyLocksRecords;
       }

        public struct VuCompanyLocksRecord
       {
           public long lockInTime;//TimeReal
           public long lockOutTime;//TimeReal
           public Name companyName;
           public Address companyAddress;
           public FullCardNumber companyCardNumber;
       }

        public struct VuControlActivityData
       {
           public short noOfControls;
           public List<VuControlActivityRecord> vuControlActivityRecords;
       }

        public struct VuControlActivityRecord
       {
           public ControlType controlType;
           public long controlTime;//TimeReal
           public FullCardNumber controlCardNumber;
           public long downloadPeriodBeginTime;//TimeReal
           public long downloadPeriodEndTime;//TimeReal
       }

        public struct VuDetailedSpeedBlock
       {
           public long speedBlockBeginDate;//TimeReal
           public short[] speedsPerSecond;//Speed
       }

        public struct VuDetailedSpeedData
       {
           public int noOfSpeedBlocks;
           public List<VuDetailedSpeedBlock> vuDetailedSpeedBlocks;
       }

        public struct VuDownloadablePeriod
       {
           public long minDownloadableTime;//TimeReal
           public long maxDownloadableTime;//TimeReal
       }

        public struct VuDownloadActivityData
       {
           public long downloadingTime;//TimeReal
           public FullCardNumber fullCardNumber;
           public Name companyOrWorkShopName;
       }

        public struct VuEventData
       {
           public short noOfVuEvents;
           public List<VuEventRecord> vuEventRecords;
       }

        public struct VuEventRecord
       {
           public byte eventType;//EventFaultType
           public byte eventRecordPurpose;//EventFaultRecordPurpose
           public long eventBeginTime;//TimeReal
           public long eventEndTime;//TimeReal
           public FullCardNumber cardNumberDriverSlotBegin;
           public FullCardNumber cardNumberCodriverSlotBegin;
           public FullCardNumber cardNumberDriverSlotEnd;
           public FullCardNumber cardNumberCodriverSlotEnd;
           public short similarEventsNumber;//SimilarEventsNumber
       }

        public struct VuFaultData
       {
           public short noOfVuFaults;
           public List<VuFaultRecord> vuFaultRecords;
       }

        public struct VuFaultRecord
       {
           public byte faultType;//EventFaultType
           public byte faultRecordPurpose;//EventFaultRecordPurpose
           public long faultBeginTime;//TimeReal
           public long faultEndTime;//TimeReal
           public FullCardNumber cardNumberDriverSlotBegin;
           public FullCardNumber cardNumberCodriverSlotBegin;
           public FullCardNumber cardNumberDriverSlotEnd;
           public FullCardNumber cardNumberCodriverSlotEnd;
       }

        public struct VuIdentification
       {
           public Name vuManufacturerName;//VuManufacturerName
           public Address vuManufacturerAddress;//VuManufacturerAddress
           public string vuPartNumber;//VuPartNumber
           public ExtendedSerialNumber vuSerialNumber;//VuSerialNumber
           public VuSoftwareIdentification vuSoftwareIdentification;
           public long vuManufacturingDate;//VuManufacturingDate
           public string vuApprovalNumber;//VuApprovalNumber
       }

        public struct VuOverSpeedingControlData
       {
           public long lastOverspeedControlTime;//TimeReal
           public long firstOverspeedSince;//TimeReal
           public short numberOfOverspeedSince;//OverspeedNumber
       }

        public struct VuOverSpeedingEventData
       {
           public short noOfOverSpeedingEvents;
           public List<VuOverSpeedingEventRecord> vuOverSpeedingEventRecords;
       }

        public struct VuOverSpeedingEventRecord
       {

           public byte eventType;//EventFaultType
           public byte eventRecordPurpose;//EventFaultRecordPurpose
           public long eventBeginTime;//TimeReal
           public long eventEndTime;//TimeReal
           public short maxSpeedValue;//SpeedMax
           public short averageSpeedValue;//SpeedAverage
           public FullCardNumber cardNumberDriverSlotBegin;
           public short similarEventsNumber;//SimilarEventsNumber
       }      

        public struct VuPlaceDailyWorkPeriodData
       {
           public short noOfPlaceRecords;
           public List<VuOverSpeedingEventRecord> vuOverSpeedingEventRecords;
       }

        public struct VuPlaceDailyWorkPeriodRecord
       {
           public FullCardNumber fullCardNumber;
           public PlaceRecord placeRecord;
       }

        public struct VuSoftwareIdentification
       {
           public string vuSoftwareVersion;//VuSoftwareVersion
           public long vuSoftInstallationDate;//VuSoftInstallationDate
       }      

        public struct VuSpecificConditionData
       {
           public int noOfSpecificConditionsRecords;
           public List<SpecificConditionRecord> specificConditionRecords;
       }

        public struct VuTimeAdjustmentData
       {
           public short noOfVuTimeAdjRecords;
           public List<VuTimeAdjustmentRecord> vuTimeAdjustmentRecords;
       }

        public struct VuTimeAdjustmentRecord
       {
           public long oldTimeValue;//TimeReal
           public long newTimeValue;//TimeReal
           public Name workshopName;
           public Address workshopAddress;
           public FullCardNumber workshopCardNumber;
       }      

        public struct WorkshopCardApplicationIdentification
       {
           public short typeOfTachographCardId;//EquipmentType
           public byte[] cardStructureVersion;//CardStructureVersion
           public short noOfEventsPerType;//NoOfEventsPerType
           public short noOfFaultsPerType;//NoOfFaultsPerType
           public int activityStructureLength; //CardActivityLengthRange
           public int noOfCardVehicleRecords;//NoOfCardVehicleRecords
           public short noOfCardPlaceRecords;//NoOfCardPlaceRecords
           public short noOfCalibrationRecords;//NoOfCalibrationRecords
       }

        public struct WorkshopCardCalibrationData
       {
           public int calibrationTotalNumber;
           public short calibrationPointerNewestRecord;
           public List<WorkshopCardCalibrationRecord> calibrationRecords;
       }

        public struct WorkshopCardCalibrationRecord
       {
           public byte calibrationPurpose;//CalibrationPurpose
           public string vehicleIdentificationNumber;//VehicleIdentificationNumber
           public VehicleRegistrationIdentification vehicleRegistration;
           public int wVehicleCharacteristicConstant;//W_VehicleCharacteristicConstant
           public int lTyreCircumference;//L_TyreCircumference
           public string tyreSize;//TyreSize
           public short authorisedSpeed;//SpeedAuthorised
           public int oldOdometerValue;//OdometerShort
           public int newOdometerValue;//OdometerShort
           public long oldTimeValue;//TimeReal
           public long newTimeValue;//TimeReal
           public long nextCalibrationDate;//TimeReal
           public string vuPartNumber;//VuPartNumber
           public ExtendedSerialNumber vuSerialNumber;//VuSerialNumber
           public ExtendedSerialNumber sensorSerialNumber;//SensorSerialNumber
       }

        public struct WorkshopCardHolderIdentification
       {
           public Name workshopName;
           public Address workshopAddress;
           public HolderName cardHolderName;
           public string cardHolderPreferredLanguage;//Language
       } 
    }            
}

