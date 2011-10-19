using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class EventFaultRecordPurpose//1byte
    {
        public byte eventFaultRecordPurpose{ get; set; }
        
        public EventFaultRecordPurpose()
        {
            eventFaultRecordPurpose = 0;
        }

        public EventFaultRecordPurpose(byte value)
        {
            eventFaultRecordPurpose = value;
        }
        /// <summary>
        /// перегруженная строка соответственно документации
        /// </summary>
        /// <returns>строка по документам</returns>
        public override string ToString()
        {
            if (eventFaultRecordPurpose == 0x00) { return "one of the 10 most recent (or last) events or faults"; }
            if (eventFaultRecordPurpose == 0x01) { return "the longest event for one of the last 10 days of occurrence"; }
            if (eventFaultRecordPurpose == 0x02) { return "one of the 5 longest events over the last 365 days"; }
            if (eventFaultRecordPurpose == 0x03) { return "the last event for one of the last 10 days of occurrence"; }
            if (eventFaultRecordPurpose == 0x04) { return "the most serious event for one of the last 10 days of occurrence"; }
            if (eventFaultRecordPurpose == 0x05) { return "one of the 5 most serious events over the last 365 days"; }
            if (eventFaultRecordPurpose == 0x06) { return "the first event or fault having occurred after the last calibration"; }
            if (eventFaultRecordPurpose == 0x07) { return "an active/on-going event or fault"; }
            if ((eventFaultRecordPurpose >= 0x08) && (eventFaultRecordPurpose <= 0x7f)) { return "RFU"; }
            if ((eventFaultRecordPurpose >= 0x80) && (eventFaultRecordPurpose <= 0xff)) { return "manufacturer specific"; }
            return "????";
        }
    }
}
