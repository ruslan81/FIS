using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuSerialNumber : ExtendedSerialNumber
    {

        public VuSerialNumber()
            : base()
        { }


        public VuSerialNumber(byte[] value)
            : base(value)
        { }
    }
}
