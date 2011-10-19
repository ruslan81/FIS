using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuPrivateKey : RSAKeyPrivateExponent
    {
        public VuPrivateKey()
            : base()
        { }

        public VuPrivateKey(byte[] value)
            : base(value)
        { }
    }
}
