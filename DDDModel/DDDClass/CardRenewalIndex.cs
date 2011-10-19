using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardRenewalIndex//1byte
    {
        public string cardRenewalIndex { get; set; }
        
        public CardRenewalIndex()
        {
            cardRenewalIndex = new string("".ToCharArray());
        }

        public CardRenewalIndex(byte value)
        {
            if (value == 0)
                cardRenewalIndex = " ";
            else
                cardRenewalIndex = ConvertionClass.convertIntoString(value);
        }

        public CardRenewalIndex(string value)
        {
            cardRenewalIndex = value;
        }

        public override string ToString()
        {
            return cardRenewalIndex;
        }
    }
}
