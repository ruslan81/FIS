using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardEventRecord//24bytes
    {
        public readonly static int structureSize = 24;

        public EventFaultType eventType { get; set; }
        public TimeReal eventBeginTime { get; set; }
        public TimeReal eventEndTime { get; set; }
        public VehicleRegistrationIdentification eventVehicleRegistration { get; set; }

        public CardEventRecord()
        {
            eventType = new EventFaultType();
            eventBeginTime = new TimeReal();
            eventEndTime = new TimeReal();
            eventVehicleRegistration = new VehicleRegistrationIdentification();
        }

        public CardEventRecord(byte[] value)
        {
            eventType = new EventFaultType(value[0]);
            eventBeginTime = new TimeReal(ConvertionClass.arrayCopy(value, 1, 4));
            eventEndTime = new TimeReal(ConvertionClass.arrayCopy(value, 5, 4));
            eventVehicleRegistration = new VehicleRegistrationIdentification(ConvertionClass.arrayCopy(value, 9, 15));
        }
    }
}
