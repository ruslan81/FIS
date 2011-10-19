using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class L_TyreCircumference//2byte
    {
        public int lTyreCircumference { get; set; }

        public L_TyreCircumference()
        {
            lTyreCircumference = 0;
        }

        public L_TyreCircumference(byte[] value)
        {
           lTyreCircumference = ConvertionClass.convertIntoUnsigned2ByteInt(value);
        }
    }
}
