using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class Language
    {
        public string language { get; set; }

        public Language()
        {
            language = "";
        }

        public Language(byte[] value)
        {
            language = ConvertionClass.convertIntoString(ConvertionClass.arrayCopy(value, 0, 2));
        }

        public Language(string value)
        {
            language = value;
        }

        public override string ToString()
        {
            return language.ToString();
        }
    }
}
