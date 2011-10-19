using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuOverSpeedingEventRecord
    {
        public readonly static int structureSize = 31;

        public EventFaultType eventType { get; set; }
        public EventFaultRecordPurpose eventRecordPurpose { get; set; }
        public TimeReal eventBeginTime { get; set; }
        public TimeReal eventEndTime { get; set; }
        public SpeedMax maxSpeedValue { get; set; }
        public SpeedAverage averageSpeedValue { get; set; }
        public FullCardNumber cardNumberDriverSlotBegin { get; set; }
        public SimilarEventsNumber similarEventsNumber { get; set; }

        public VuOverSpeedingEventRecord()
        {
            eventType = new EventFaultType();
            eventRecordPurpose = new EventFaultRecordPurpose();
            eventBeginTime = new TimeReal();
            eventEndTime = new TimeReal();
            maxSpeedValue = new SpeedMax();
            averageSpeedValue = new SpeedAverage();
            cardNumberDriverSlotBegin = new FullCardNumber();
            similarEventsNumber = new SimilarEventsNumber();
        }

        public VuOverSpeedingEventRecord(byte[] value)
        {
            eventType = new EventFaultType(value[0]);
            eventRecordPurpose = new EventFaultRecordPurpose(value[1]);
            eventBeginTime = new TimeReal(ConvertionClass.arrayCopy(value, 2, 4));
            eventEndTime = new TimeReal(ConvertionClass.arrayCopy(value, 6, 4));
            maxSpeedValue = new SpeedMax(value[10]);
            averageSpeedValue = new SpeedAverage(value[11]);
            cardNumberDriverSlotBegin = new FullCardNumber(ConvertionClass.arrayCopy(value, 12, 18));
            similarEventsNumber = new SimilarEventsNumber(value[30]);
        }
    }
}
