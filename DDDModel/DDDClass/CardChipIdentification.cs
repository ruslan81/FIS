using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardChipIdentification
    {
        public string icSerialNumber { get; set; }
        public string icManufacturingReferences { get; set; }

        public CardChipIdentification()
        {
            icSerialNumber = "";
            icManufacturingReferences = "";
        }

        public void Set_icSerialNumber (string value)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            _bytes = enc.GetBytes(value);
            icSerialNumber = ConvertionClass.convertBCDStringIntoString(_bytes);
        }

        public void Set_icManufacturingReferences(string value)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            _bytes = enc.GetBytes(value);
            icManufacturingReferences = ConvertionClass.convertBCDStringIntoString(_bytes);
        }

        public string Get_icSerialNumberToString()
        {
            string[] splitString = icSerialNumber.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
            byte[] icSerialNumberBytes = new byte[splitString.Length];

            for (int i = 0; i < splitString.Length; i++)
            {
                icSerialNumberBytes[i] = Convert.ToByte(splitString[i]);
            }

            return ConvertionClass.convertIntoString(icSerialNumberBytes).Trim();
        }

        public string Get_icManufacturingReferencesToString()
        {
            return ConvertionClass.ConvertBCDStringToBytes(false, icManufacturingReferences).ToString();
        }

        public CardChipIdentification(byte[] value)
        {
            byte[] icSerialNumberB = ConvertionClass.arrayCopy(value, 0, 4);
            byte[] icManufacturingReferencesB = ConvertionClass.arrayCopy(value, 4, 4);

            icSerialNumber = "";
            icManufacturingReferences = "";
            foreach (byte b in icSerialNumberB)
                icSerialNumber += b.ToString() + "&&";

            foreach (byte b in icManufacturingReferencesB)
                icManufacturingReferences += b.ToString() + "&&";        
        }
    }
}
