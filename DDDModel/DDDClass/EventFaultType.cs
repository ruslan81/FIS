using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class EventFaultType//1byte
    {
        public byte eventFaultType { get; set; }

        public EventFaultType()
        {
            eventFaultType = 0;
        }

        public EventFaultType(byte value)
        {
            eventFaultType = value;
        }
        /// <summary>
        /// перегруженная строка соответственно документации
        /// </summary>
        /// <returns>строка по документам</returns>
        public override string ToString()
        {
            // invalid event fault types
            if (eventFaultType > 0x4f)  { return "????"; }
            if (eventFaultType == 0x00) { return "no further details"; }
            if (eventFaultType == 0x01) { return "insertion of a non-valid card"; }
            if (eventFaultType == 0x02) { return "card conflict"; }
            if (eventFaultType == 0x03) { return "time overlap"; }
            if (eventFaultType == 0x04) { return "driving without an appropriate card"; }
            if (eventFaultType == 0x05) { return "card insertion while driving"; }
            if (eventFaultType == 0x06) { return "last card session not correctly closed"; }
            if (eventFaultType == 0x07) { return "over speeding"; }
            if (eventFaultType == 0x08) { return "power supply interruption"; }
            if (eventFaultType == 0x09) { return "motion data error"; }
            if (eventFaultType == 0x10) { return "no further details"; }
            if (eventFaultType == 0x11) { return "motion sensor authentication failure"; }
            if (eventFaultType == 0x12) { return "tachograph card authentication failure"; }
            if (eventFaultType == 0x13) { return "unauthorised change of motion sensor"; }
            if (eventFaultType == 0x14) { return "card data input integrity error"; }
            if (eventFaultType == 0x15) { return "stored user data integrity error"; }
            if (eventFaultType == 0x16) { return "internal data transfer error"; }
            if (eventFaultType == 0x17) { return "unauthorised case opening"; }
            if (eventFaultType == 0x18) { return "hardware sabotage"; }
            if (eventFaultType == 0x20) { return "no further details"; }
            if (eventFaultType == 0x21) { return "authentication failure"; }
            if (eventFaultType == 0x22) { return "stored data integrity error"; }
            if (eventFaultType == 0x23) { return "internal data transfer error"; }
            if (eventFaultType == 0x24) { return "unauthorised case opening"; }
            if (eventFaultType == 0x25) { return "hardware sabotage"; }
            if (eventFaultType == 0x30) { return "no further details"; }
            if (eventFaultType == 0x31) { return "VU internal fault"; }
            if (eventFaultType == 0x32) { return "printer fault"; }
            if (eventFaultType == 0x33) { return "display fault"; }
            if (eventFaultType == 0x34) { return "downloading fault"; }
            if (eventFaultType == 0x35) { return "sensor fault"; }
            if (eventFaultType == 0x40) { return "no further details"; }
            return "RFU";//RFU = Reserved for future use
        }

    }
}
