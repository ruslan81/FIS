using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class NoOfCompanyActivityRecords//2bytes
    {
        public int noOfCompanyActivityRecords { get; set; }

        public NoOfCompanyActivityRecords()
        {
            noOfCompanyActivityRecords = 0;
        }

        public NoOfCompanyActivityRecords(byte[] value)
        {
            this.noOfCompanyActivityRecords =ConvertionClass.convertIntoUnsigned2ByteInt(value);
        }

        public NoOfCompanyActivityRecords(string value)
        {
            this.noOfCompanyActivityRecords = Convert.ToInt32(value);
        }

        public override string ToString()
        {
            return noOfCompanyActivityRecords.ToString();
        }
    }
}
