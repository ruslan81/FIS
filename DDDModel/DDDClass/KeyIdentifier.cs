using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class KeyIdentifier//8bytes
    {
        public ExtendedSerialNumber extendedSerialNumber { get; set; }
        public CertificateRequestID certificateRequestID { get; set; }
        public CertificationAuthorityKID certificationAuthorityKID { get; set; }  
        
        private short kidType;
        // Public key of a Vehicle Unit or of a tachograph card
        public static readonly short KIDTYPE_PK_VU_TC = 1;
        // Public key of a Vehicle Unit (in the case the serial number of the
        // Vehicle Unit cannot be known at certificate generation time)
        public static readonly short KIDTYPE_PK_VU = 2;
        // Public key of a Member State	
        public static readonly short KIDTYPE_PK_MS = 3;

        public KeyIdentifier()
        {
            extendedSerialNumber = new ExtendedSerialNumber();
            certificateRequestID = new CertificateRequestID();
            certificationAuthorityKID = new CertificationAuthorityKID();

          
        }

        public KeyIdentifier(byte[] value, short kidType)
        {
            this.kidType = kidType;
            if (kidType == KIDTYPE_PK_VU_TC)
            {
                extendedSerialNumber = new ExtendedSerialNumber(ConvertionClass.arrayCopy(value, 0, 8));
            }
            else if (kidType == KIDTYPE_PK_VU)
            {
                certificateRequestID = new CertificateRequestID(ConvertionClass.arrayCopy(value, 0, 8));
            }
            else
            {
                certificationAuthorityKID = new CertificationAuthorityKID(ConvertionClass.arrayCopy(value, 0, 8));
            }
        }
    }
}
