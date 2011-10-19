using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
   public class TyreSize
    {
       public string tyreSize { get; set; }

        public TyreSize()
        {
            tyreSize = new string("".ToCharArray());
        }

        public TyreSize(byte[] value)
        {
            tyreSize = ConvertionClass.convertIntoString(value).Trim();
        }

        public TyreSize(string value)
        {
            tyreSize = value;
        }

        public override string ToString()
        {
            return this.tyreSize;
        }
    }
}
