using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class OdometerShort//3bytes
    {
        public int odometerShort { get; set; }

        public OdometerShort()
        {
            odometerShort = 0;
        }

        public OdometerShort(byte[] value)
        {
            odometerShort = ConvertionClass.convertIntoUnsigned3ByteInt(value);
        }

        public OdometerShort(string value)
        {
            odometerShort = Convert.ToInt32(value);
        }

        public override string ToString()
        {
            return odometerShort.ToString();
        }
    }
}
