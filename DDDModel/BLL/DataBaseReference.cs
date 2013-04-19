using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    /// <summary>
    /// Статический класс, в котором содержатся названия дополнительных параметров для различных таблиц.
    /// </summary>
    public static class DataBaseReference
    {
        //VehicleInfo
        public static string Vehicle_FuelTank1 = "Fuel tank 1";
        public static string Vehicle_FuelTank2 = "Fuel tank 2";
        public static string Vehicle_GarageNumber = "Garage number";
        public static string Vehicle_MakeYear = "Production year";
        public static string Vehicle_Type = "Vehicle type";
        public static string Vehicle_FuelType = "Fuel type";
        public static string Vehicle_EquipmentType = "Equipment type";
        public static string Vehicle_SerialNumber = "Serial";
        public static string Vehicle_LastReadDate = "Last read date";
        public static string Vehicle_CalibrReason = "Calibr reason";
        public static string Vehicle_Calibrator = "Calibrator";
        public static string Vehicle_CalibratorCard = "Calibrator card";
        public static string Vehicle_NextCalibrDate = "Next calibr date";
        public static string Vehicle_LoadCarryingCapacity = "Load-carrying capacity";
        public static string Vehicle_MRO1 = "MRO 1";
        public static string Vehicle_MRO2 = "MRO 2";
        public static string Vehicle_NominalTurns = "Nominal turns";
        public static string Vehicle_MaxSpeed = "Maximum speed";
        public static string Vehicle_Manoeuvring = "Manoeuvring";
        public static string Vehicle_Highway = "Highway";
        public static string Vehicle_City = "City";
        public static string Vehicle_NomFuelConsumption = "Nominal fuel consumption";
        public static string Vehicle_ColdStart = "Cold start";
        public static string Vehicle_HotStop = "Hot stop";
        public static string Vehicle_VehiclePhotoAddress = "Vehicle photo address";
        //UserInfo
        public static string UserInfo_Image = "User Image";
        public static string UserInfo_Name = "Name";
        public static string UserInfo_Surname = "Surname";
        public static string UserInfo_Patronimic = "Patronimic";
        public static string UserInfo_License = "License";
        public static string UserInfo_License_Giver = "License Giver";
        public static string UserInfo_Vehicle= "Vehicle";
        public static string UserInfo_Comment = "Comment";
        public static string UserInfo_CardGiver = "Card Giver";
        public static string UserInfo_CardGivenDate = "Card Given Date";
        public static string UserInfo_CardFromDate = "Card From Date";
        public static string UserInfo_CardToDate = "Card To Date";
        public static string UserInfo_DriversCertificate = "Drivers certificate";
        public static string UserInfo_PhoneNumber = "Phone number";
        public static string UserInfo_Fax = "Fax";
        public static string UserInfo_Email = "E-mail";
        public static string UserInfo_AddressOne = "Address(1)";
        public static string UserInfo_AddressTwo = "Address(2)";
        public static string UserInfo_ZIP = "Zip Code";
        public static string UserInfo_Birthday = "Birthday";
        public static string UserInfo_RegDate = "Registration date";
        public static string UserInfo_EndOfRegistrationDate = "End of registration date";
        public static string UserInfo_SiteLang = "Language(screen)";
        public static string UserInfo_ReportsLang = "Language(reports)";
        public static string UserInfo_Country = "Country";
        public static string UserInfo_City = "City";
        public static string UserInfo_ONOFF = "ONOFF";
        public static string UserInfo_DealerId = "DealerId";
        public static string UserInfo_TimeZone = "TimeZone";
        public static string UserInfo_UserPhoto = "UserPhoto";
        //ORG_INFO
        public static string OrgInfo_SiteLang = "Language(screen)";
        public static string OrgInfo_ReportsLang = "Language(reports)";
        public static string OrgInfo_Address = "Address";
        public static string OrgInfo_City = "City";
        public static string OrgInfo_ONOFF = "ONOFF";
        public static string OrgInfo_EndOfRegistrationDate = "End of registration date";
        public static string OrgInfo_RegistrationDate = "Registration date";
        public static string OrgInfo_StorageDataPeriod = "Period of storage of the data";
        public static string OrgInfo_PhoneNumber = "Phone number";
        public static string OrgInfo_Fax = "Fax";
        public static string OrgInfo_Email = "E-mail";
        public static string OrgInfo_AddressOne = "Address(1)";
        public static string OrgInfo_AddressTwo = "Address(2)";
        public static string OrgInfo_ZIP = "Zip Code";
        public static string OrgInfo_TimeZone = "TimeZone";
        //DEALER_INFO (ORG_INFO for Dealers)
        public static string Dealer_Address = "Address";

    }
}
