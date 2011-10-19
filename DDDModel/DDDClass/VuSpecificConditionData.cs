using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuSpecificConditionData
    {
        public int structureSize { get; set; }
        public int noOfSpecificConditionRecords { get; set; }
        public List<SpecificConditionRecord> specificConditionRecords { get; set; }

        public VuSpecificConditionData()
        {
            specificConditionRecords = new List<SpecificConditionRecord>();
        }

        public VuSpecificConditionData(byte[] value)
        {
            specificConditionRecords = new List<SpecificConditionRecord>();

            noOfSpecificConditionRecords = ConvertionClass.convertIntoUnsigned2ByteInt(ConvertionClass.arrayCopy(value, 0, 2));
            structureSize = 2 + noOfSpecificConditionRecords * SpecificConditionRecord.structureSize;

            if (noOfSpecificConditionRecords != 0)
            {
                for (int i = 0; i < noOfSpecificConditionRecords; i++)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, 2 + (i * SpecificConditionRecord.structureSize), SpecificConditionRecord.structureSize);
                    SpecificConditionRecord scr = new SpecificConditionRecord(record);
                    specificConditionRecords.Add(scr);
                }
            }
        }
    }
}
