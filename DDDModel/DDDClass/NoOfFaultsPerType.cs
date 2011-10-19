using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class NoOfFaultsPerType
    {
        public short noOfFaultsPerType { get; set; }

        public NoOfFaultsPerType()
        {
            noOfFaultsPerType = 0;
        }

        public NoOfFaultsPerType(byte b)
        {
            noOfFaultsPerType =ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }

        public NoOfFaultsPerType(string value)
        {
            byte b = Convert.ToByte(value);
            noOfFaultsPerType = ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }

        public override string ToString()
        {
            return noOfFaultsPerType.ToString();
        }
    }
}
