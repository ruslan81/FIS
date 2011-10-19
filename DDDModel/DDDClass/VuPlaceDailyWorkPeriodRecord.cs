using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuPlaceDailyWorkPeriodRecord//28bytes
    {
        public static int structureSize = 28;

        public FullCardNumber fullCardNumber { get; set; }
        public PlaceRecord placeRecord { get; set; }

       
        public VuPlaceDailyWorkPeriodRecord()
        {
            fullCardNumber = new FullCardNumber();
            placeRecord = new PlaceRecord();
        }

        public VuPlaceDailyWorkPeriodRecord(byte[] value)
        {            
            fullCardNumber = new FullCardNumber(ConvertionClass.arrayCopy(value, 0, 18));
            placeRecord = new PlaceRecord(ConvertionClass.arrayCopy(value, 18, 10));
        }
    }
}
