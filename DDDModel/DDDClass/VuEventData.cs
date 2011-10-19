using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuEventData
    {
        public int structureSize { get; set; }
        public short noOfVuEvents { get; set; }
        public List<VuEventRecord> vuEventRecords { get; set; }

        public VuEventData()
        {
            vuEventRecords = new List<VuEventRecord>();
        }

        public VuEventData(byte[] value)
        {
            vuEventRecords = new List<VuEventRecord>();

            noOfVuEvents = ConvertionClass.convertIntoUnsigned1ByteInt(value[0]);
            structureSize = 1 + noOfVuEvents * VuEventRecord.structureSize;

            if (noOfVuEvents != 0)
            {
                for (int i = 0; i < noOfVuEvents; i++)
                {
                    byte[] record =ConvertionClass.arrayCopy(value, 1 + (i * VuEventRecord.structureSize), VuEventRecord.structureSize);
                    VuEventRecord ver = new VuEventRecord(record);
                    vuEventRecords.Add(ver);
                }
            }
        }
    }
}
