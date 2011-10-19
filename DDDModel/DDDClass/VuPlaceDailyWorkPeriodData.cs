using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuPlaceDailyWorkPeriodData
    {
        public int structureSize { get; set; }

        public short noOfPlaceRecords { get; set; }
        public List<VuPlaceDailyWorkPeriodRecord> vuPlaceDailyWorkPeriodRecords { get; set; }

        public VuPlaceDailyWorkPeriodData()
        {
            vuPlaceDailyWorkPeriodRecords = new List<VuPlaceDailyWorkPeriodRecord>();
        }

        public VuPlaceDailyWorkPeriodData(byte[] value)
        {
            vuPlaceDailyWorkPeriodRecords = new List<VuPlaceDailyWorkPeriodRecord>();

            noOfPlaceRecords = ConvertionClass.convertIntoUnsigned1ByteInt(value[0]);
            structureSize = 1 + noOfPlaceRecords * VuPlaceDailyWorkPeriodRecord.structureSize;

            if (noOfPlaceRecords != 0)
            {
                for (int i = 0; i < noOfPlaceRecords; i++)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, 1 + (i * VuPlaceDailyWorkPeriodRecord.structureSize), VuPlaceDailyWorkPeriodRecord.structureSize);
                    VuPlaceDailyWorkPeriodRecord vpdwpr = new VuPlaceDailyWorkPeriodRecord(record);
                    vuPlaceDailyWorkPeriodRecords.Add(vpdwpr);
                }
            }
        }
    }
}
