using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class Distance//2bytes
    {
        public int distance { get; set; }

        public Distance()
        {
            distance = 0;
        }
        public Distance(byte[] value)
        {
            distance = ConvertionClass.convertIntoUnsigned2ByteInt(value); ;
        }

        public Distance(string value)
        {
            distance = Convert.ToInt32(value); ;
        }

        public override string ToString()
        {
            return distance.ToString();
        }
    }
}
