using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class ControlCardControlActivityData
    {
        public readonly int structureSize;
        public List<CardControlActivityDataRecord> controlActivityRecords { get; set; }

        public ControlCardControlActivityData()
        {
            controlActivityRecords = new List<CardControlActivityDataRecord>();
        }
        
        public ControlCardControlActivityData(byte[] value, int noOfControlActivityRecords)
        {
            int noOfValidControlActivityRecords = 0;

            controlActivityRecords = new List<CardControlActivityDataRecord>();

            for (int i = 0; i < noOfControlActivityRecords; i += 1)
            {
                byte[] record =ConvertionClass.arrayCopy(value, 2 + (i * CardControlActivityDataRecord.structureSize), CardControlActivityDataRecord.structureSize);

                CardControlActivityDataRecord ccadr = new CardControlActivityDataRecord(record);

                // only add entries with non-default values, i.e. skip empty entries
                if (ccadr.controlTime.timereal != 0)
                {
                    controlActivityRecords.Add(ccadr);

                    noOfValidControlActivityRecords += 1;
                }
            }

            structureSize = 2 + noOfValidControlActivityRecords * CardControlActivityDataRecord.structureSize;
        }
    }
}
