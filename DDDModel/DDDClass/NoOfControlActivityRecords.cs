using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class NoOfControlActivityRecords//2bytes
    {
        public int noOfControlActivityRecords { get; set; }

        public NoOfControlActivityRecords()
        {
            noOfControlActivityRecords = 0;
        }

        public NoOfControlActivityRecords(byte[] value)
        {
            this.noOfControlActivityRecords =ConvertionClass.convertIntoUnsigned2ByteInt(value);
        }

        public NoOfControlActivityRecords(string value)
        {
            this.noOfControlActivityRecords = Convert.ToInt32(value);
        }

        public override string ToString()
        {
            return noOfControlActivityRecords.ToString();
        }
    }
}
