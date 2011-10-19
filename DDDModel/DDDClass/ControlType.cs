using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class ControlType //1 byte
    {
        public byte value { get; set; }
        public bool card_downloading;
        public bool vu_downloading;
        public bool display;
        public bool printing;

        public ControlType(byte[] value)
        {
            this.setControlType(value[0]);
        }

        public ControlType(byte controlType)
        {
            this.setControlType(controlType);
        }

        public ControlType(string controlType)
        {
            this.setControlType(Convert.ToByte(controlType));
        }

        public ControlType()
        {
            value = 0;
        }

        public void setControlType(byte value)
        {
            this.value = value;

            card_downloading = ((value & 0x80) == 0x80);//CARD_DOWNLOADING
            vu_downloading = ((value & 0x40) == 0x40);  //VU_DOWNLOADING
            printing = ((value & 0x20) == 0x20);        //PRINTING
            display = ((value & 0x10) == 0x10);         //DISPLAY
        }

        public string Card_Downloading()
        {
            if (card_downloading == true)
                return "card downloaded during this control activity";
            else
                return "card not downloaded during this control activity";
        }

        public string Vu_Downloading()
        {
            if (vu_downloading == true)
                return "VU downloaded during this control activity";
            else
                return "VU not downloaded during this control activity";
        }

        public string Display_Activity()
        {
            if (display == true)
                return "display used during this control activity";
            else
                return "no display used during this control activity";
        }

        public string Printing_Activity()
        {
            if (printing == true)
                return "printing done during this control activity";
            else
                return "no printing done during this control activity";
        }
    }
}
