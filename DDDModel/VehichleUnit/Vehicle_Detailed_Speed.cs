using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace VehichleUnit
{
    using HexBytes = ConvertionClass;
    /// <summary>
    /// Детальная скорость ТС
    /// </summary>
    public class Vehicle_Detailed_Speed
    {
        public readonly int size;

        public VuDetailedSpeedData vuDetailedSpeedData { get; set; }

        public Vehicle_Detailed_Speed()
        { 
        }

        public Vehicle_Detailed_Speed(byte[] value)
        {
            int offset1 = HexBytes.convertIntoUnsigned2ByteInt(HexBytes.arrayCopy(value, 0, 2)) * VuDetailedSpeedBlock.structureSize;
            vuDetailedSpeedData = new VuDetailedSpeedData(value);

            size = offset1;
        }
    }
}
