using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class NoOfEventsPerType//1byte
    {
        public short noOfEventsPerType { get; set; }

        public NoOfEventsPerType()
        {
            noOfEventsPerType = 0;
        }
       
        public NoOfEventsPerType(byte b)
        {
            noOfEventsPerType =ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }

        public NoOfEventsPerType(string value)
        {
            byte b = Convert.ToByte(value);
            noOfEventsPerType = ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }

        public override string ToString()
        {
            return noOfEventsPerType.ToString();
        }
    }
}
