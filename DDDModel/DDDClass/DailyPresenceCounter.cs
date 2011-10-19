using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class DailyPresenceCounter//2байта
    {
        public string dailyPresenceCounter { get; set; }

        public DailyPresenceCounter()
        {
            dailyPresenceCounter = "0000";

        }
       
        public DailyPresenceCounter(byte[] value)
        {
            dailyPresenceCounter = ConvertionClass.ConvertBytesToBCDString(false,value);
        }

        public DailyPresenceCounter(string value)
        {
            dailyPresenceCounter = value;
        }

        public override string ToString()
        {
            return dailyPresenceCounter;
        }
    }
}
