using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class W_VehicleCharacteristicConstant//2bytes
    {
        public int wVehicleCharacteristicConstant { get; set; }

        public W_VehicleCharacteristicConstant()
        {
            wVehicleCharacteristicConstant = 0;
        }

        public W_VehicleCharacteristicConstant(byte[] value)
        {
            this.wVehicleCharacteristicConstant = ConvertionClass.convertIntoUnsigned2ByteInt(value);
        }
    }
}
