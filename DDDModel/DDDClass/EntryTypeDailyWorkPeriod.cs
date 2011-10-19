using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class EntryTypeDailyWorkPeriod//1byte
    {
        public short entryTypeDailyWorkPeriod { get; set; }

        public EntryTypeDailyWorkPeriod()
        {
            entryTypeDailyWorkPeriod = 0;
        }

        public EntryTypeDailyWorkPeriod(byte value)
        {
            entryTypeDailyWorkPeriod = ConvertionClass.convertIntoUnsigned1ByteInt(value);
        }

        public EntryTypeDailyWorkPeriod(string value)
        {
            entryTypeDailyWorkPeriod = Convert.ToInt16(value);
        }
        /// <summary>
        /// перегруженная строка соответственно документации
        /// </summary>
        /// <returns>строка по документам</returns>
        public override string ToString()
        {
            switch (entryTypeDailyWorkPeriod)
            {
                case 0:
                    return "Begin, related time = card insertion time or time of entry";
                case 1:
                    return "End, related time = card withdrawal time or time of entry";
                case 2:
                    return "Begin, related time manually entered (start time)";
                case 3:
                    return "End, related time manually entered (end of work period)";
                case 4:
                    return "Begin, related time assumed by VU";
                case 5:
                    return "End, related time assumed by VU";               
                default:
                    return "unknown";
            }

        }
    }
}
