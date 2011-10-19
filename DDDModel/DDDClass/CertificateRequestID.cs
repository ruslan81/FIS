using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CertificateRequestID
    {
        public long requestSerialNumber { get; set; }
        public byte[] requestMonthYear { get; set; }
        public byte crIdentifier { get; set; }
        public ManufacturerCode manufacturerCode { get; set; }

        public CertificateRequestID()
        {
            requestSerialNumber = 0;
            requestMonthYear = new byte[2];
            crIdentifier = 0;
            manufacturerCode = new ManufacturerCode();
        }

        public CertificateRequestID(byte[] value)
        {
            requestSerialNumber = ConvertionClass.convertIntoUnsigned4ByteInt(ConvertionClass.arrayCopy(value, 0, 4));
            requestMonthYear = ConvertionClass.arrayCopy(value, 4, 2);
            crIdentifier = value[6];
            manufacturerCode = new ManufacturerCode(value[7]);
        }
    }
}
