using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuEventRecord
    {
        public static int structureSize = 83;

        public EventFaultType eventType { get; set; }
        public EventFaultRecordPurpose eventRecordPurpose { get; set; }
        public TimeReal eventBeginTime { get; set; }
        public TimeReal eventEndTime { get; set; }
        public FullCardNumber cardNumberDriverSlotBegin { get; set; }
        public FullCardNumber cardNumberCodriverSlotBegin { get; set; }
        public FullCardNumber cardNumberDriverSlotEnd { get; set; }
        public FullCardNumber cardNumberCodriverSlotEnd { get; set; }
        public SimilarEventsNumber similarEventsNumber { get; set; }

        public VuEventRecord()
        {
            eventType = new EventFaultType();
            eventRecordPurpose = new EventFaultRecordPurpose();
            eventBeginTime = new TimeReal();
            eventEndTime = new TimeReal();
            cardNumberDriverSlotBegin = new FullCardNumber();
            cardNumberCodriverSlotBegin = new FullCardNumber();
            cardNumberDriverSlotEnd = new FullCardNumber();
            cardNumberCodriverSlotEnd = new FullCardNumber();
            similarEventsNumber = new SimilarEventsNumber();
        }

        public VuEventRecord(byte[] value)
        {
            eventType = new EventFaultType(value[0]);
            eventRecordPurpose = new EventFaultRecordPurpose(value[1]);
            eventBeginTime = new TimeReal(ConvertionClass.arrayCopy(value, 2, 4));
            eventEndTime = new TimeReal(ConvertionClass.arrayCopy(value, 6, 4));
            cardNumberDriverSlotBegin = new FullCardNumber(ConvertionClass.arrayCopy(value, 10, 18));
            cardNumberCodriverSlotBegin = new FullCardNumber(ConvertionClass.arrayCopy(value, 28, 18));
            cardNumberDriverSlotEnd = new FullCardNumber(ConvertionClass.arrayCopy(value, 46, 18));
            cardNumberCodriverSlotEnd = new FullCardNumber(ConvertionClass.arrayCopy(value, 64, 18));
            similarEventsNumber = new SimilarEventsNumber(value[82]);
        }
    }
}

