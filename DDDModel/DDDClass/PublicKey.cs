using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class PublicKey
    {
        public RSAKeyModulus rsaKeyModulus { get; set; }
        public RSAKeyPublicExponent rsaKeyPublicExponent { get; set; }//RSAKeyPublicExponent

        public PublicKey()
        {
            rsaKeyModulus = new RSAKeyModulus();
            rsaKeyPublicExponent = new RSAKeyPublicExponent();
        }

        public PublicKey(byte[] value)
        {
            rsaKeyModulus = new RSAKeyModulus(ConvertionClass.arrayCopy(value, 0, 128));
            rsaKeyPublicExponent = new RSAKeyPublicExponent(ConvertionClass.arrayCopy(value, 128, 8));

        }
    }
}
