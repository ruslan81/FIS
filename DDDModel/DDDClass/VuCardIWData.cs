using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuCardIWData
    {
        public int structureSize { get; set; }
        public int noOfIWRecords { get; set; }
        public List<VuCardIWRecord> vuCardIWRecords { get; set; }

        public VuCardIWData()
        {
            vuCardIWRecords = new List<VuCardIWRecord>();
        }

        public VuCardIWData(byte[] value)
        {
            vuCardIWRecords = new List<VuCardIWRecord>();

            noOfIWRecords = ConvertionClass.convertIntoUnsigned2ByteInt(ConvertionClass.arrayCopy(value, 0, 2));
            structureSize = 2 + noOfIWRecords * VuCardIWRecord.structureSize;

            if (noOfIWRecords != 0)
            {
                for (int i = 0; i < noOfIWRecords; i++)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, 2 + (i * VuCardIWRecord.structureSize), VuCardIWRecord.structureSize);
                    VuCardIWRecord vciwr = new VuCardIWRecord(record);
                    vuCardIWRecords.Add(vciwr);
                }
            }
        }

        public override string ToString()
        {
            return vuCardIWRecords.Count.ToString();
        }
    }
}
