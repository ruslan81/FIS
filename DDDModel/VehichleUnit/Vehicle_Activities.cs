using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace VehichleUnit
{
    using HexBytes = ConvertionClass;

    /// <summary>
    /// Описывает активность ТС
    /// </summary>
    public class Vehicle_Activities
    {
        private readonly int structureSize;

        public TimeReal downloadedDayDate { get; set; }
        public OdometerShort odoMeterValueMidnight { get; set; }
        public VuCardIWData vuCardIWData { get; set; }
        public VuActivityDailyData vuActivityDailyData { get; set; }
        public VuPlaceDailyWorkPeriodData vuPlaceDailyWorkPeriodData { get; set; }
        public VuSpecificConditionData vuSpecificConditionData { get; set; }

        public Vehicle_Activities()
        {
            downloadedDayDate = new TimeReal();
            odoMeterValueMidnight = new OdometerShort();
            vuCardIWData = new VuCardIWData();
            vuActivityDailyData = new VuActivityDailyData();
            vuPlaceDailyWorkPeriodData = new VuPlaceDailyWorkPeriodData();
            vuSpecificConditionData = new VuSpecificConditionData();
        }      

        public Vehicle_Activities(byte[] value)
        {

            int offset1 = 7;
            downloadedDayDate = new TimeReal(HexBytes.arrayCopy(value, 0, 4));

            odoMeterValueMidnight = new OdometerShort(HexBytes.arrayCopy(value, 4, 3));

            int offset2 = 2 + HexBytes.convertIntoUnsigned2ByteInt(HexBytes.arrayCopy(value, offset1, 2)) * VuCardIWRecord.structureSize;
            vuCardIWData = new VuCardIWData(HexBytes.arrayCopy(value, offset1, offset2));

            int offset3 = 2 + HexBytes.convertIntoUnsigned2ByteInt(HexBytes.arrayCopy(value, offset1 + offset2, 2)) * ActivityChangeInfo.structureSize;
            vuActivityDailyData = new VuActivityDailyData(HexBytes.arrayCopy(value, offset1 + offset2, offset3), downloadedDayDate);

            int offset4 = 1 + HexBytes.convertIntoUnsigned1ByteInt(value[offset1 + offset2 + offset3]) * VuPlaceDailyWorkPeriodRecord.structureSize;
            vuPlaceDailyWorkPeriodData = new VuPlaceDailyWorkPeriodData(HexBytes.arrayCopy(value, offset1 + offset2 + offset3, offset4));

            int offset5 = 2 + HexBytes.convertIntoUnsigned2ByteInt(HexBytes.arrayCopy(value, offset1 + offset2 + offset3 + offset4, 2)) * SpecificConditionRecord.structureSize;
            vuSpecificConditionData = new VuSpecificConditionData(HexBytes.arrayCopy(value, offset1 + offset2 + offset3 + offset4, offset5));

            structureSize = offset1 + offset2 + offset3 + offset4 + offset5;
        }
    }
}
