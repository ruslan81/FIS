using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardApprovalNumber//8bytes
    {
        public byte[] cardApprovalNumber { get; set; }

        public CardApprovalNumber()
        {
            cardApprovalNumber = new byte[8];
        }

        public CardApprovalNumber(byte[] value)
        {
            cardApprovalNumber = (ConvertionClass.arrayCopy(value, 0, 8));
        }

        public CardApprovalNumber(string value)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            _bytes = enc.GetBytes(value);
            cardApprovalNumber = _bytes;            
        }

        public override string ToString()
        {
            return ConvertionClass.convertIntoString(cardApprovalNumber).Trim();
        }
    }
}
