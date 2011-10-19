using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class Address//36bytes
    {
        public short codePage { get; set; }
        public byte[] address { get; set; }

        public Address()
        {
            codePage = 0;
            address = new byte[35];
        }

        public Address(byte[] value)
        {
            byte codePageTmp = value[0];
            byte[] addressTemp = ConvertionClass.arrayCopy(value, 1, 35);

            codePage = codePageTmp;
            address = addressTemp;
        }

        public Address(byte codePageTmp, byte[] addressTemp)
        {
            codePage = codePageTmp;
            address = addressTemp;
        }

        public Address(byte codePageTmp, string addressTemp)
        {
            codePage = codePageTmp;
            SetAddress(addressTemp);
        }

        public void SetAddress(string value)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes = enc.GetBytes(value);

            codePage = 0;
            address = _bytes;
        }


        public override string ToString()
        {
            return ConvertionClass.convertIntoString(address);
        }
    }
}
