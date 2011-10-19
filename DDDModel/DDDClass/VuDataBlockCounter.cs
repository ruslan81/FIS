using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuDataBlockCounter//2bytes
    {
        public byte[] vuDataBlockCounter { get; set; }

        public VuDataBlockCounter()
        {
            vuDataBlockCounter = new byte[2];
        }

        public VuDataBlockCounter(byte[] value)
        {
            this.vuDataBlockCounter = ConvertionClass.arrayCopy(value, 0, 2);
        }
    }
}
