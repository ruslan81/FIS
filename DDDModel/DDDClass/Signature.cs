using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class Signature//128 bytes
    {
        public byte[] signature { get; set; }

        public Signature()
        {
            signature = new byte[128];
        }
                
        public Signature(byte[] value)
        {
            signature = value;
        }
    }
}
