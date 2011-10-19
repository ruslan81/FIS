using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuManufacturerName : Name
    {
        public VuManufacturerName()
            : base()
        {}

        public VuManufacturerName(byte[] value)
            : base(value)
        {}
    }
}
