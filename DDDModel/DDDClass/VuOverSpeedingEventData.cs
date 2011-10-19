using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuOverSpeedingEventData
    {
        public int structureSize { get; set; }

        public short noOfOverSpeedingEvents { get; set; }
        public List<VuOverSpeedingEventRecord> vuOverSpeedingEventRecords { get; set; }

        public VuOverSpeedingEventData()
        { }

        public VuOverSpeedingEventData(byte[] value)
        {
            vuOverSpeedingEventRecords = new List<VuOverSpeedingEventRecord>();

            noOfOverSpeedingEvents = ConvertionClass.convertIntoUnsigned1ByteInt(value[0]);
            structureSize = 1 + noOfOverSpeedingEvents * VuOverSpeedingEventRecord.structureSize;

            if (noOfOverSpeedingEvents != 0)
            {
                for (int i = 0; i < noOfOverSpeedingEvents; i++)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, 1 + (i * VuOverSpeedingEventRecord.structureSize), VuOverSpeedingEventRecord.structureSize);
                    VuOverSpeedingEventRecord voser = new VuOverSpeedingEventRecord(record);
                    vuOverSpeedingEventRecords.Add(voser);
                }
            }
        }
    }
}
