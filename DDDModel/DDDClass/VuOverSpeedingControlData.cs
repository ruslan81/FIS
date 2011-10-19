using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuOverSpeedingControlData//9byte
    {
        public TimeReal lastOverspeedControlTime { get; set; }
        public TimeReal firstOverspeedSince { get; set; }
        public OverspeedNumber numberOfOverspeedSince { get; set; }

        public VuOverSpeedingControlData()
        {
            lastOverspeedControlTime = new TimeReal();
            firstOverspeedSince = new TimeReal();
            numberOfOverspeedSince = new OverspeedNumber();
        }

        public VuOverSpeedingControlData(byte[] value)
        {
            lastOverspeedControlTime = new TimeReal(ConvertionClass.arrayCopy(value, 0, 4));
            firstOverspeedSince = new TimeReal(ConvertionClass.arrayCopy(value, 4, 4));
            numberOfOverspeedSince = new OverspeedNumber(value[8]);
        }

    }
}
