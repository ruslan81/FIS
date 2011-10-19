using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CertificateContent
    {
        public short certificateProfileIdentifier { get; set; }
        public KeyIdentifier certificationAuthorityReference { get; set; }
        public CertificateHolderAuthorisation certificateHolderAuthorisation { get; set; }
        public TimeReal certificateEndOfValidity { get; set; }
        public KeyIdentifier certificateHolderReference { get; set; }
        public PublicKey publicKey { get; set; }

        public CertificateContent()
        {
            certificateProfileIdentifier = 0;
            certificationAuthorityReference = new KeyIdentifier();
            certificateHolderAuthorisation = new CertificateHolderAuthorisation();
            certificateEndOfValidity = new TimeReal();
            certificateHolderReference = new KeyIdentifier();
            publicKey = new PublicKey();
        }

        public CertificateContent(byte[] value, short kidType)
        {
            certificateProfileIdentifier = ConvertionClass.convertIntoUnsigned1ByteInt(value[0]);
            certificationAuthorityReference = new KeyIdentifier(ConvertionClass.arrayCopy(value, 1, 8), kidType);
            certificateHolderAuthorisation = new CertificateHolderAuthorisation(ConvertionClass.arrayCopy(value, 9, 7));
            certificateEndOfValidity = new TimeReal(ConvertionClass.arrayCopy(value, 16, 4));
            certificateHolderReference = new KeyIdentifier(ConvertionClass.arrayCopy(value, 20, 8), kidType);
            publicKey = new PublicKey(ConvertionClass.arrayCopy(value, 28, 136));
        }
    }
}
