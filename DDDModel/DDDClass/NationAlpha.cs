using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class NationAlpha//3bytes
    {
        public string nationAlpha { get; set; }

        public NationAlpha()
        {
            nationAlpha = new string("".ToCharArray());
        }

        public NationAlpha(byte[] value)
        {
            nationAlpha = ConvertionClass.convertIntoString(ConvertionClass.arrayCopy(value, 0, 3));
        }
        
        public NationAlpha(string value)
        {
           nationAlpha = value;
        }
    }
}
