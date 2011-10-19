using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuCompanyLocksData
    {
        public int structureSize { get; set; }
        public short noOfLocks { get; set; }
        public List<VuCompanyLocksRecord> vuCompanyLocksRecords { get; set; }

        public VuCompanyLocksData()
        {
            structureSize = 0;
            noOfLocks = 0;
            vuCompanyLocksRecords = new List<VuCompanyLocksRecord>();
        }
        

        public VuCompanyLocksData(byte[] value)
        {
            vuCompanyLocksRecords = new List<VuCompanyLocksRecord>();

            noOfLocks = ConvertionClass.convertIntoUnsigned1ByteInt(value[0]);
            structureSize = 1 + noOfLocks * VuCompanyLocksRecord.structureSize;

            if (noOfLocks != 0)
            {
                for (int i = 0; i < noOfLocks; i++)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, 1 + (i * VuCompanyLocksRecord.structureSize), VuCompanyLocksRecord.structureSize);
                    VuCompanyLocksRecord vclr = new VuCompanyLocksRecord(record);
                    vuCompanyLocksRecords.Add(vclr);
                }
            }
        }
    }
}
