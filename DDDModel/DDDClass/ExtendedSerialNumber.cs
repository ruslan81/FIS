using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// расширенный серийный номер
    /// </summary>
    public class ExtendedSerialNumber
    {
        public long serialNumber { get; set; }
        public string monthYear { get; set; }
        
        public byte type { get; set; }
        public ManufacturerCode manufacturerCode { get; set; }

        public ExtendedSerialNumber()
        {
            serialNumber = 0;
            monthYear = "";
            type = 0;
            manufacturerCode = new ManufacturerCode();
        }

        public void setMonthYear (byte[] temp)
        {
            monthYear = ConvertionClass.convertBCDStringIntoString(temp);

        }
        
        public ExtendedSerialNumber(byte[] value)
        {
            serialNumber = ConvertionClass.convertIntoUnsigned4ByteInt(ConvertionClass.arrayCopy(value, 0, 4));
            monthYear = ConvertionClass.convertBCDStringIntoString(ConvertionClass.arrayCopy(value, 4, 2));
            type = value[6];
            manufacturerCode = new ManufacturerCode(value[7]);
        }

        public ExtendedSerialNumber(string value)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            _bytes = enc.GetBytes(value);

            serialNumber = ConvertionClass.convertIntoUnsigned4ByteInt(ConvertionClass.arrayCopy(_bytes, 0, 4));
            monthYear = ConvertionClass.convertBCDStringIntoString(ConvertionClass.arrayCopy(_bytes, 4, 2));
            type = _bytes[6];
            manufacturerCode = new ManufacturerCode(_bytes[7]);
        }
    }
}
