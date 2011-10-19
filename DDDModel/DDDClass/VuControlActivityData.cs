using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuControlActivityData
    {
        public int structureSize { get; set; }
        public short noOfControls { get; set; }
        public List<VuControlActivityRecord> vuControlActivityRecords { get; set; }

        public VuControlActivityData()
        { }


        public VuControlActivityData(byte[] value)
        {
            vuControlActivityRecords = new List<VuControlActivityRecord>();

            noOfControls = ConvertionClass.convertIntoUnsigned1ByteInt(value[0]);
            structureSize = 1 + noOfControls * VuControlActivityRecord.structureSize;

            if (noOfControls != 0)
            {
                for (int i = 0; i < noOfControls; i++)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, 1 + (i * VuControlActivityRecord.structureSize), VuControlActivityRecord.structureSize);
                    VuControlActivityRecord vcar = new VuControlActivityRecord(record);
                    vuControlActivityRecords.Add(vcar);
                }
            }
        }
    }
}
