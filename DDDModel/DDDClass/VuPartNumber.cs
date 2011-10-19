using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuPartNumber
    {
        public string vuPartNumber { get; set; }

        public VuPartNumber()
        {
            vuPartNumber = new string("".ToCharArray());
        }

        public VuPartNumber(byte[] value)
        {
            vuPartNumber = ConvertionClass.convertIntoString(ConvertionClass.arrayCopy(value, 0, 16));
        }

        public VuPartNumber(string value)
        {
            vuPartNumber = value;
        }

        public override string ToString()
        {
            return vuPartNumber;
        }
    }
}
