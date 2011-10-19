using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuFaultRecord
    {
        public static int structureSize = 82;

        public EventFaultType faultType { get; set; }
        public EventFaultRecordPurpose faultRecordPurpose { get; set; }
        public TimeReal faultBeginTime { get; set; }
        public TimeReal faultEndTime { get; set; }
        public FullCardNumber cardNumberDriverSlotBegin { get; set; }
        public FullCardNumber cardNumberCodriverSlotBegin { get; set; }
        public FullCardNumber cardNumberDriverSlotEnd { get; set; }
        public FullCardNumber cardNumberCodriverSlotEnd { get; set; }

        public VuFaultRecord()
        {
            faultType = new EventFaultType();
            faultRecordPurpose = new EventFaultRecordPurpose();
            faultBeginTime = new TimeReal();
            faultEndTime = new TimeReal();
            cardNumberDriverSlotBegin = new FullCardNumber();
            cardNumberCodriverSlotBegin = new FullCardNumber();
            cardNumberDriverSlotEnd = new FullCardNumber();
            cardNumberCodriverSlotEnd = new FullCardNumber();
        }

        public VuFaultRecord(byte[] value)
        {
            faultType = new EventFaultType(value[0]);
            faultRecordPurpose = new EventFaultRecordPurpose(value[1]);
            faultBeginTime = new TimeReal(ConvertionClass.arrayCopy(value, 2, 4));
            faultEndTime = new TimeReal(ConvertionClass.arrayCopy(value, 6, 4));
            cardNumberDriverSlotBegin = new FullCardNumber(ConvertionClass.arrayCopy(value, 10, 18));
            cardNumberCodriverSlotBegin = new FullCardNumber(ConvertionClass.arrayCopy(value, 28, 18));
            cardNumberDriverSlotEnd = new FullCardNumber(ConvertionClass.arrayCopy(value, 46, 18));
            cardNumberCodriverSlotEnd = new FullCardNumber(ConvertionClass.arrayCopy(value, 64, 18));

        }
    }
}
