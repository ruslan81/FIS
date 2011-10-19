using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class SimilarEventsNumber//1byte
    {
        public short similarEventsNumber { get; set; }

        public SimilarEventsNumber()
        {
            similarEventsNumber = 0;
        }

        public SimilarEventsNumber(byte b)
        {
            similarEventsNumber = ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }

        public override string ToString()
        {
            return similarEventsNumber.ToString();
        }
    }
}
