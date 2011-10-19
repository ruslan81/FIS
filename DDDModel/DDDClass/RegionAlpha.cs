using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class RegionAlpha//3bytes
    {
        public string regionAlpha { get; set; }

        public RegionAlpha()
        {
            regionAlpha = new string("".ToCharArray());
        }

        public RegionAlpha(byte[] value)
        {
            regionAlpha = ConvertionClass.convertIntoString(ConvertionClass.arrayCopy(value, 0, 3));
        }

        public RegionAlpha(string value)
        {
            regionAlpha = value;
        }
    }
}
