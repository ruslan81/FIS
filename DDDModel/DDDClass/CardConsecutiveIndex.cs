using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardConsecutiveIndex//1byte
    {
        public string cardConsecutiveIndex { get; set; }

        public CardConsecutiveIndex()
        {
            cardConsecutiveIndex = new string("".ToCharArray());
        }

        public CardConsecutiveIndex(byte value)
        {
            cardConsecutiveIndex = ConvertionClass.convertIntoString(value);
        }

        public CardConsecutiveIndex(string value)
        {
            cardConsecutiveIndex = value;
        }

        public override string ToString()
        {
            return cardConsecutiveIndex;
        }
    }
}
