using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CalibrationPurpose//1byte
    {
        public byte calibrationPurpose { get; set; }

        public CalibrationPurpose()
        {
            calibrationPurpose = 0;
        }

        public CalibrationPurpose(byte[] value)
        {
            calibrationPurpose = value[0];
        }

        public CalibrationPurpose(byte value)
        {
            this.calibrationPurpose = value;
        }

        public override string ToString()
        {
            if (calibrationPurpose == 0x00) { return "reserved value"; }
            if (calibrationPurpose == 0x01) { return "activation: recording of calibration parameters known, at the moment of the VU activation"; }
            if (calibrationPurpose == 0x02) { return "first installation: first calibration of the VU after its activation"; }
            if (calibrationPurpose == 0x03) { return "installation: first calibration of the VU in the current vehicle"; }
            if (calibrationPurpose == 0x04) { return "periodic inspection"; }
            return "????";
        }
    }
}
