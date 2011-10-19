using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace VehichleUnit
{
    using HexBytes = ConvertionClass;
    /// <summary>
    /// Общие сведения о транспортном средстве
    /// </summary>
    public class Vehicle_Overview
    {
        public int size;

        public MemberStateCertificate memberStateCertificate { get; set; }
        public VuCertificate vuCertificate { get; set; }
        public VehicleIdentificationNumber vehicleIdentificationNumber { get; set; }
        public VehicleRegistrationIdentification vehicleRegistrationIdentification { get; set; }
        public CurrentDateTime currentDateTime { get; set; }
        public VuDownloadablePeriod vuDownloadablePeriod { get; set; }
        public CardSlotsStatus cardSlotsStatus { get; set; }
        public VuDownloadActivityData vuDownloadActivityData { get; set; }
        public VuCompanyLocksData vuCompanyLocksData { get; set; }
        public VuControlActivityData vuControlActivityData { get; set; }

        public Vehicle_Overview()
        {
            memberStateCertificate = new MemberStateCertificate();
            vuCertificate = new VuCertificate();
            vehicleIdentificationNumber = new VehicleIdentificationNumber();
            vehicleRegistrationIdentification = new VehicleRegistrationIdentification();
            currentDateTime = new CurrentDateTime();
            vuDownloadablePeriod = new VuDownloadablePeriod();
            cardSlotsStatus = new CardSlotsStatus();
            vuDownloadActivityData = new VuDownloadActivityData();
            vuCompanyLocksData = new VuCompanyLocksData();
            vuControlActivityData = new VuControlActivityData();
 
        }

        public Vehicle_Overview(byte[] value)
        {

            int offset1 = 194 + 194 + 17 + 1 + 14 + 4 + 4 + 4 + 1 + 4 + 18 + 36;
            memberStateCertificate = new MemberStateCertificate(HexBytes.arrayCopy(value, 0, 194));
            vuCertificate = new VuCertificate(HexBytes.arrayCopy(value, 194, 194));
            vehicleIdentificationNumber = new VehicleIdentificationNumber(HexBytes.arrayCopy(value, 388, 17));
            vehicleRegistrationIdentification = new VehicleRegistrationIdentification(HexBytes.arrayCopy(value, 405, 15));
            currentDateTime = new CurrentDateTime(HexBytes.arrayCopy(value, 420, 4));
            vuDownloadablePeriod = new VuDownloadablePeriod(HexBytes.arrayCopy(value, 424, 8));
            cardSlotsStatus = new CardSlotsStatus(value[432]);
            vuDownloadActivityData = new VuDownloadActivityData(HexBytes.arrayCopy(value, 433, 58));
            int offset2 = 1 + HexBytes.convertIntoUnsigned1ByteInt(value[offset1]) * VuCompanyLocksRecord.structureSize;
            vuCompanyLocksData = new VuCompanyLocksData(HexBytes.arrayCopy(value, offset1, offset2));
            int offset3 = 1 + HexBytes.convertIntoUnsigned1ByteInt(value[offset1 + offset2]) * VuControlActivityRecord.structureSize;
            vuControlActivityData = new VuControlActivityData(HexBytes.arrayCopy(value, offset1 + offset2, offset3));
            size = offset1 + offset2 + offset3;
        }
    }
}
