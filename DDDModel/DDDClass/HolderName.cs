using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class HolderName//72bytes
    {
        public Name holderSurname { get; set; }
        public Name holderFirstNames { get; set; }

        public HolderName()
        {
            holderSurname = new Name();
            holderFirstNames = new Name();
        }

        public HolderName(byte[] value)
        {
            holderSurname = new Name(ConvertionClass.arrayCopy(value, 0, 36));
            holderFirstNames = new Name(ConvertionClass.arrayCopy(value, 36, 36));
        }

        public override string ToString()
        {
            return holderFirstNames.ToString() + " " + holderSurname.ToString();
        }

    }
}
