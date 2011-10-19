using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VehicleRegistrationNumber//14bytes
    {
        public short codePage { get; set; }
        public byte[] vehicleRegNumber { get; set; }
        
        public VehicleRegistrationNumber()
        {
            codePage = 0;
            vehicleRegNumber = new byte[13];
        }

        public VehicleRegistrationNumber(byte[] value)
        {
            byte codePageTemp = value[0];
            byte[] vehicleRegNumberTemp = ConvertionClass.arrayCopy(value, 1, value.Length - 1);

            codePage = ConvertionClass.convertIntoUnsigned1ByteInt(codePageTemp);
            vehicleRegNumber = ConvertionClass.arrayCopy(vehicleRegNumberTemp, 0, 13);
        }

        public VehicleRegistrationNumber(byte codePageTemp, byte[] vehicleRegNumberTemp)
        {
            codePage =ConvertionClass.convertIntoUnsigned1ByteInt(codePageTemp);
            vehicleRegNumber = ConvertionClass.arrayCopy(vehicleRegNumberTemp, 0, 13);
        }

        public override string ToString()
        {
            return ConvertionClass.convertIntoString(vehicleRegNumber).Trim();
        }
    }
}