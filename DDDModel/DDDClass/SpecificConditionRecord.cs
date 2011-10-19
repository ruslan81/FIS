using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class SpecificConditionRecord//5 bytes
    {

        public static int structureSize = 5;
        public TimeReal entryTime { get; set; }
        public SpecificConditionType specificConditionType { get; set; }

        public SpecificConditionRecord()
        {
            entryTime = new TimeReal();
            specificConditionType = new SpecificConditionType();
        }

        public SpecificConditionRecord(byte[] value)
        {
            entryTime = new TimeReal(ConvertionClass.arrayCopy(value, 0, 4));
            specificConditionType = new SpecificConditionType(value[4]);
        }

    }
}
