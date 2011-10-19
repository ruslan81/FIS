using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuControlActivityRecord//31byte
    {
        public readonly static int structureSize = 31;

        public ControlType controlType { get; set; }
        public TimeReal controlTime { get; set; }
        public FullCardNumber controlCardNumber { get; set; }
        public TimeReal downloadPeriodBeginTime { get; set; }
        public TimeReal downloadPeriodEndTime { get; set; }

        public VuControlActivityRecord()
        {
            controlType = new ControlType();
            controlTime = new TimeReal();
            controlCardNumber = new FullCardNumber();
            downloadPeriodBeginTime = new TimeReal();
            downloadPeriodEndTime = new TimeReal();
        }

        public VuControlActivityRecord(byte[] value)
        {
            controlType = new ControlType(value[0]);
            controlTime = new TimeReal(ConvertionClass.arrayCopy(value, 1, 4));
            controlCardNumber = new FullCardNumber(ConvertionClass.arrayCopy(value, 5, 18));
            downloadPeriodBeginTime = new TimeReal(ConvertionClass.arrayCopy(value, 23, 4));
            downloadPeriodEndTime = new TimeReal(ConvertionClass.arrayCopy(value, 27, 4));
        }
    }
}
