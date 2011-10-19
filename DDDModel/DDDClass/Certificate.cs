using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class Certificate
    {
        //Certificate :== OCTET STRING(SIZE(194)), 194 bytes
        public byte[] certificate { get; set; }        

        public void setCertificate(byte[] value)
        {
            certificate =ConvertionClass.arrayCopy(value, 0, 194);
        }

        public Certificate()
        {
            certificate = new byte[194];
        }

        public Certificate(byte[] certificateTmp)
        {
            setCertificate(certificateTmp);
        }

    /**    public override string ToString()
        {
            return ConvertionClass.convertIntoString(certificate);
        }*/
    }
}