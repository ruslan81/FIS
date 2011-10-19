using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class ManualInputFlag//1byte
    {
        public bool manualInputFlag { get; set; }

        public ManualInputFlag()
        {
            manualInputFlag = false;
        }

        public ManualInputFlag(byte value)
        {
            manualInputFlag = ((value & (byte)0x01) == (byte)0x01);
        }

        public override string ToString()
        {
            if (manualInputFlag == true)
                return "MANUAL_ENTRY";
            else
                return "NO_ENTRY";
        }
    }
}
