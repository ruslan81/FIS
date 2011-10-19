using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardControlActivityDataRecord//46bytes
    {
        public readonly static int structureSize = 46;

        public ControlType controlType { get; set; }
        public TimeReal controlTime { get; set; }
        public FullCardNumber controlCardNumber { get; set; }
        public VehicleRegistrationIdentification controlVehicleRegistration { get; set; }
        public TimeReal controlDownloadPeriodBegin { get; set; }
        public TimeReal controlDownloadPeriodEnd { get; set; }

        public CardControlActivityDataRecord()
        {
            controlType = new ControlType();
            controlTime = new TimeReal();
            controlCardNumber = new FullCardNumber();
            controlVehicleRegistration = new VehicleRegistrationIdentification();
            controlDownloadPeriodBegin = new TimeReal();
            controlDownloadPeriodEnd = new TimeReal();
        }

        public CardControlActivityDataRecord(byte[] value)
        {
            controlType = new ControlType(value[0]);
            controlTime = new TimeReal(ConvertionClass.arrayCopy(value, 1, 4));
            controlCardNumber = new FullCardNumber(ConvertionClass.arrayCopy(value, 5, 18));
            controlVehicleRegistration = new VehicleRegistrationIdentification(ConvertionClass.arrayCopy(value, 23, 15));
            controlDownloadPeriodBegin = new TimeReal(ConvertionClass.arrayCopy(value, 38, 4));
            controlDownloadPeriodEnd = new TimeReal(ConvertionClass.arrayCopy(value, 42, 4));
        }

    }
}
