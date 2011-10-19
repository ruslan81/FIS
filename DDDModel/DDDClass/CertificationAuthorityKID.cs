using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CertificationAuthorityKID
    {
        public NationNumeric nationNumeric { get; set; }
        public NationAlpha nationAlpha { get; set; }
        public short keySerialNumber { get; set; }
        public byte[] additionalInfo { get; set; }
        public byte caIdentifier { get; set; }

        public CertificationAuthorityKID()
        {
            nationNumeric = new NationNumeric();
            nationAlpha = new NationAlpha();
            keySerialNumber = 0;
            additionalInfo = new byte[2];
            caIdentifier = 0;
        }

        public CertificationAuthorityKID(byte[] value)
        {
            nationNumeric = new NationNumeric(value[0]);
            nationAlpha = new NationAlpha(ConvertionClass.arrayCopy(value, 1, 3));
            keySerialNumber = value[4];
            additionalInfo = ConvertionClass.arrayCopy(value, 5, 2);
            caIdentifier = value[7];
        }
    }
}
