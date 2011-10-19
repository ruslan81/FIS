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
        public static string FuelTank1 = "Fuel tank 1";
        public static string FuelTank2 = "Fuel tank 2";
        public static string LoadCarryingCapacity = "Load-carrying capacity";
        public static string MRO1 = "MRO 1";
        public static string MRO2 = "MRO 2";
        public static string NominalTurns = "Nominal turns";
        public static string MaxSpeed = "Maximum speed";
        public static string Manoeuvring = "Manoeuvring";
        public static string Highway = "Highway";
        public static string City = "City";
        public static string NomFuelConsumption = "Nominal fuel consumption";
        public static string ColdStart = "Cold start";
        public static string HotStop = "Hot stop";
        public static string VehiclePhotoAddress = "Vehicle photo address";
        //UserInfo
        public static string UserInfo_Name = "Name";
        public static string UserInfo_Surname = "Surname";
        public static string UserInfo_Patronimic = "Patronimic";
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
