using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class SpecificConditionType//1 byte
    {
        public short specificConditionType { get; set; }

        public SpecificConditionType()
        {
            specificConditionType = 0;
        }

        public SpecificConditionType(short value)
        {
            specificConditionType = value;
        }

        public SpecificConditionType(string value)
        {
            specificConditionType = Convert.ToInt16(value);
        }

        public override string ToString()
        {
            if (specificConditionType == 0x00)
            {
                return "RFU";
            }
            if (specificConditionType == 0x01)
            {
                return "Out of scope begin";
            }
            if (specificConditionType == 0x02)
            {
                return "Out of scope end";
            }
            if (specificConditionType == 0x03)
            {
                return "Ferry/Train crossing";
            }

            return "RFU";
        }
    }
}
