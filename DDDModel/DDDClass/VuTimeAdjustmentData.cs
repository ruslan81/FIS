using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuTimeAdjustmentData
    {
        public int size { get; set; }
        public short noOfVuTimeAdjRecords { get; set; }
        public List<VuTimeAdjustmentRecord> vuTimeAdjustmentRecords { get; set; }

        public VuTimeAdjustmentData()
        {
            vuTimeAdjustmentRecords = new List<VuTimeAdjustmentRecord>();
        }

        public VuTimeAdjustmentData(byte[] value)
        {
            vuTimeAdjustmentRecords = new List<VuTimeAdjustmentRecord>();

            noOfVuTimeAdjRecords =ConvertionClass.convertIntoUnsigned1ByteInt(value[0]);
            size = 1 + noOfVuTimeAdjRecords * VuTimeAdjustmentRecord.structureSize;

            if (noOfVuTimeAdjRecords != 0)
            {
                for (int i = 0; i < noOfVuTimeAdjRecords; i++)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, 1 + (i * VuTimeAdjustmentRecord.structureSize), VuTimeAdjustmentRecord.structureSize);
                    VuTimeAdjustmentRecord vtar = new VuTimeAdjustmentRecord(record);
                    vuTimeAdjustmentRecords.Add(vtar);
                }
            }
        }
    }
}
