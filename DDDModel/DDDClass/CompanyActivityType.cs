using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CompanyActivityType
    {
        public short companyActivityType { get; set; }

        public CompanyActivityType()
        {
            companyActivityType = 0;
        }

        public CompanyActivityType(byte value)
        {
            companyActivityType = ConvertionClass.convertIntoUnsigned1ByteInt(value);
        }
    }
}
