using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class RSAKeyModulus//128bytes
    {
        public byte[] rsaKeyModulus { get; set; }
      
        public RSAKeyModulus()
        {
            rsaKeyModulus = new byte[128];
        }

        public RSAKeyModulus(byte[] value)
        {
            rsaKeyModulus = value;
        }
    }
}
