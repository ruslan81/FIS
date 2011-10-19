using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class OverspeedNumber//1byte
    {
        public short overspeedNumber { get; set; }

        public OverspeedNumber()
        {
            overspeedNumber = 0;
        }

        public OverspeedNumber(byte b)
        {
            overspeedNumber =ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }

        public override string ToString()
        {
            return overspeedNumber.ToString();
        }

    }
}
