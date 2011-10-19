using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class RSAKeyPrivateExponent//128bytes
    {
        public byte[] rsaKeyPrivateExponent { get; set; }

        public RSAKeyPrivateExponent()
        {
            rsaKeyPrivateExponent = new byte[128];
        }

        public RSAKeyPrivateExponent(byte[] value)
        {
            this.rsaKeyPrivateExponent = value;
        }
    }
}
