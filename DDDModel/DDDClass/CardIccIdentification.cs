using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardIccIdentification//25bytes
    {
        public byte clockStop { get; set; }
        public ExtendedSerialNumber cardExtendedSerialNumber { get; set; }
        public CardApprovalNumber cardApprovalNumber { get; set; }
        public byte cardPersonaliserID { get; set; }
        public byte[] embedderIcAssemblerId { get; set; }
        public byte[] icIdentifier { get; set; }

        public CardIccIdentification()
        {
            clockStop = 0;
            cardExtendedSerialNumber = new ExtendedSerialNumber();
            cardApprovalNumber = new CardApprovalNumber();
            cardPersonaliserID = 0;
            embedderIcAssemblerId = new byte[5];
            icIdentifier = new byte[2];
        }

        public CardIccIdentification(byte[] value)
        {
            clockStop = value[0];
            cardExtendedSerialNumber = new ExtendedSerialNumber(ConvertionClass.arrayCopy(value, 1, 8));
            cardApprovalNumber = new CardApprovalNumber(ConvertionClass.arrayCopy(value, 9, 8));
            cardPersonaliserID = value[17];
            embedderIcAssemblerId = ConvertionClass.arrayCopy(value, 18, 5);
            icIdentifier = ConvertionClass.arrayCopy(value, 23, 2);
        }

        public string EmbedderIcAssemblerId_ToString()
        {
            return ConvertionClass.convertIntoString(embedderIcAssemblerId);
        }

        public string IcIdentifier_ToString()
        {
            return ConvertionClass.convertIntoString(icIdentifier);
        }

        public string CardPersonaliserID_ToString()
        {
            return ConvertionClass.convertIntoString(cardPersonaliserID);
        }

        public string ClockStop_ToString()
        {
            string retVal = ConvertionClass.convertIntoString(clockStop);
            if(retVal=="\0")
                retVal = "00";
            return retVal;
        }
    }
}
