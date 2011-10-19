using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardReplacementIndex//1byte
    {
        public string cardReplacementIndex { get; set; }

        public CardReplacementIndex()
        {
            cardReplacementIndex = new string("".ToCharArray());
        }

        public CardReplacementIndex(byte value)
        {
            if (value == 0)
                cardReplacementIndex = " ";
            else
                cardReplacementIndex = ConvertionClass.convertIntoString(value);
        }

        public CardReplacementIndex(string value)
        {
            cardReplacementIndex = value;
        }

        public override string ToString()
        {
            return cardReplacementIndex;
        }
    }
}
