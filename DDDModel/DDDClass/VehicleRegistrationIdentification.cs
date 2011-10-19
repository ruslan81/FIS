using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VehicleRegistrationIdentification//15bytes
    {
        public NationNumeric vehicleRegistrationNation { get; set; }
        public VehicleRegistrationNumber vehicleRegistrationNumber { get; set; }

        public VehicleRegistrationIdentification()
        {
            vehicleRegistrationNation = new NationNumeric();
            vehicleRegistrationNumber = new VehicleRegistrationNumber();
        }

        public VehicleRegistrationIdentification(byte[] value)
        {
            vehicleRegistrationNation = new NationNumeric(value[0]);
            vehicleRegistrationNumber = new VehicleRegistrationNumber(ConvertionClass.arrayCopy(value, 1, 14));
        }
    }
}
