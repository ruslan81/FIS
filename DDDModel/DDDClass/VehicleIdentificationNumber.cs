using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VehicleIdentificationNumber//17 bytes
    {
        public string vehicleIdentificationNumber { get; set; }

        public VehicleIdentificationNumber()
        {
            vehicleIdentificationNumber = new string("".ToCharArray());
        }

        public VehicleIdentificationNumber(byte[] value)
        {
            vehicleIdentificationNumber = ConvertionClass.convertIntoString(ConvertionClass.arrayCopy(value, 0, 17)).Trim();            
        }

        public override string ToString()
        {
            return vehicleIdentificationNumber;
        }
    }
}
