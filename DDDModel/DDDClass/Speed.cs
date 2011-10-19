using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class Speed//1byte
    {
        public short speed { set; get; }

        public Speed()
        {
            speed = 0;
        }

        public Speed(byte b)
        {
            speed = ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }

        public override string ToString()
        {
            return speed.ToString();
        }
    }
}
