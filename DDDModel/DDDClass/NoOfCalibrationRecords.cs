using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class NoOfCalibrationRecords//1byte
    {
        public short noOfCalibrationRecords { get; set; }

        public NoOfCalibrationRecords()
        {
            noOfCalibrationRecords = 0;
        }

        public NoOfCalibrationRecords(byte b)
        {
            noOfCalibrationRecords =ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }

        public NoOfCalibrationRecords(string value)
        {
            byte b = Convert.ToByte(value);
            noOfCalibrationRecords = ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }

        public override string ToString()
        {
            return noOfCalibrationRecords.ToString();
        }
    }
}
