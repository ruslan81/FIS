using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardActivityLengthRange
    {
        public int cardActivityLengthRange { get; set; }

        public CardActivityLengthRange()
        {
            cardActivityLengthRange = 0;
        }

        public CardActivityLengthRange(byte[] value)
        {
            cardActivityLengthRange = ConvertionClass.convertIntoUnsigned2ByteInt(value);
        }

        public CardActivityLengthRange(int value)
        {
            cardActivityLengthRange = value;
        }

        public CardActivityLengthRange(string value)
        {
            cardActivityLengthRange = Convert.ToInt32(value);
        }

        public override string ToString()
        {
            return cardActivityLengthRange.ToString();
        }
    
    }
}
