using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardCurrentUse//19bytes
    {
        public readonly static int structureSize = 19;

        public TimeReal sessionOpenTime { get; set; }
        public VehicleRegistrationIdentification sessionOpenVehicle { get; set; }

        public CardCurrentUse()
        {
            sessionOpenTime = new TimeReal();
            sessionOpenVehicle = new VehicleRegistrationIdentification();
        }

        public CardCurrentUse(byte[] value)
        {
            sessionOpenTime = new TimeReal(ConvertionClass.arrayCopy(value, 0, 4));
            sessionOpenVehicle = new VehicleRegistrationIdentification(ConvertionClass.arrayCopy(value, 4, 15));
        }
    }
}
