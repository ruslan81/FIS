using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuSoftwareVersion//4bytes
    {
        public string vuSoftwareVersion { get; set; }

        public VuSoftwareVersion()
        {
            vuSoftwareVersion = new string("".ToCharArray());
        }

        public VuSoftwareVersion(byte[] value)
        {
            vuSoftwareVersion = ConvertionClass.convertIntoString(ConvertionClass.arrayCopy(value, 0, 4));
        }

        public VuSoftwareVersion(string value)
        {
            vuSoftwareVersion = value;
        }

        public override string ToString()
        {
            return this.vuSoftwareVersion;
        }
    }
}
