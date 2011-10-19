using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuFaultData
    {
        public int structureSize { get; set; }
        public short noOfVuFaults { get; set; }
        public List<VuFaultRecord> vuFaultRecords { get; set; }

        public VuFaultData()
        {
            vuFaultRecords = new List<VuFaultRecord>();
        }

        public VuFaultData(byte[] value)
        {
            vuFaultRecords = new List<VuFaultRecord>();

            noOfVuFaults = ConvertionClass.convertIntoUnsigned1ByteInt(value[0]);
            structureSize = 1 + noOfVuFaults * VuFaultRecord.structureSize;

            if (noOfVuFaults != 0)
            {
                for (int i = 0; i < noOfVuFaults; i++)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, 1 + (i * VuFaultRecord.structureSize), VuFaultRecord.structureSize);
                    VuFaultRecord vfr = new VuFaultRecord(record);
                    vuFaultRecords.Add(vfr);
                }
            }
        }
    }
}
