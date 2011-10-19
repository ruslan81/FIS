using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class NoOfCalibrationsSinceDownload//2bytes
    {
        public int noOfCalibrationsSinceDownload { get; set; }

        public NoOfCalibrationsSinceDownload()
        {
            noOfCalibrationsSinceDownload = 0;
        }

        public NoOfCalibrationsSinceDownload(byte[] value)
        {
            noOfCalibrationsSinceDownload = ConvertionClass.convertIntoUnsigned2ByteInt(value);
        }

        public NoOfCalibrationsSinceDownload(string value)
        {
            noOfCalibrationsSinceDownload = Convert.ToInt32(value);
        }

        public override string ToString()
        {
            return noOfCalibrationsSinceDownload.ToString();
        }
    }
}
