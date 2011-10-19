using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuManufacturerAddress : Address
    {
        public VuManufacturerAddress()
            : base()
        { }

        public VuManufacturerAddress(byte[] value)
            : base(value)
        { }
    }
}
