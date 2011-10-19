using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class PlaceRecord//10bytes
    {
        public readonly static int structureSize = 10;

        public TimeReal entryTime { get; set; }
        public EntryTypeDailyWorkPeriod entryTypeDailyWorkPeriod { get; set; }
        public NationNumeric dailyWorkPeriodCountry { get; set; }
        public RegionNumeric dailyWorkPeriodRegion { get; set; }
        public OdometerShort vehicleOdometerValue { get; set; }

        public PlaceRecord()
        {
            entryTime = new TimeReal();
            entryTypeDailyWorkPeriod = new EntryTypeDailyWorkPeriod();
            dailyWorkPeriodCountry = new NationNumeric();
            dailyWorkPeriodRegion = new RegionNumeric();
            vehicleOdometerValue = new OdometerShort();
        }

        public PlaceRecord(byte[] record)
        {
            entryTime = new TimeReal(ConvertionClass.arrayCopy(record, 0, 4));
            entryTypeDailyWorkPeriod = new EntryTypeDailyWorkPeriod(record[4]); 
            dailyWorkPeriodCountry = new NationNumeric(record[5]);
            dailyWorkPeriodRegion = new RegionNumeric(record[6]);
            vehicleOdometerValue = new OdometerShort(ConvertionClass.arrayCopy(record, 7, 3));
        }

    }
}
