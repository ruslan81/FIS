using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class NoOfCardPlaceRecords
    {
        public short noOfCardPlaceRecords { get; set; }

        public NoOfCardPlaceRecords()
        {
            noOfCardPlaceRecords = 0;
        }
        
        public NoOfCardPlaceRecords(byte b)
        {
            noOfCardPlaceRecords = ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }

        public NoOfCardPlaceRecords(string value)
        {
            byte b = Convert.ToByte(value);
            noOfCardPlaceRecords = ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }

        public override string ToString()
        {
            return noOfCardPlaceRecords.ToString();
        }
    }
}
