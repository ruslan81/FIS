using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuPublicKey : PublicKey
    {
        public VuPublicKey()
            : base()
        {
        }

        public VuPublicKey(byte[] value)
            : base(value)
        {
        }
    }
}
