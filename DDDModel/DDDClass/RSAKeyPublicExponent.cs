using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class RSAKeyPublicExponent//8 bytes
    {
        public byte[] rsaKeyPublicExponent { get; set; }

        public RSAKeyPublicExponent()
        {
            rsaKeyPublicExponent = new byte[8];
        }

        public RSAKeyPublicExponent(byte[] value)
        {
            rsaKeyPublicExponent = value;
        }
    }
}
