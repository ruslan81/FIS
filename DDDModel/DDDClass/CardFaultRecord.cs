using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardFaultRecord
    {
        public readonly static int structureSize = 24;

        public EventFaultType faultType { get; set; }
        public TimeReal faultBeginTime { get; set; }
        public TimeReal faultEndTime { get; set; }
        public VehicleRegistrationIdentification faultVehicleRegistration { get; set; }

        public CardFaultRecord()
        {
            faultType = new EventFaultType();
            faultBeginTime = new TimeReal();
            faultEndTime = new TimeReal();
            faultVehicleRegistration = new VehicleRegistrationIdentification();
        }

        public CardFaultRecord(byte[] value)
        {
            faultType = new EventFaultType(value[0]);
            long faultBeginTimeTmp = ConvertionClass.convertIntoUnsigned4ByteInt(ConvertionClass.arrayCopy(value, 1, 4));
            faultBeginTime = new TimeReal(ConvertionClass.arrayCopy(value, 1, 4));
            faultEndTime = new TimeReal(ConvertionClass.arrayCopy(value, 5, 4));
            faultVehicleRegistration = new VehicleRegistrationIdentification(ConvertionClass.arrayCopy(value, 9, 15));
        }
    }
}
