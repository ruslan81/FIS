using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuCalibrationData
    {
        public int structureSize { get; set; }
        public short noOfVuCalibrationRecords { get; set; }
        public List<VuCalibrationRecord> vuCalibrationRecords { get; set; }

        public VuCalibrationData()
        { }

        public VuCalibrationData(byte[] value)
        {
            vuCalibrationRecords = new List<VuCalibrationRecord>();

            noOfVuCalibrationRecords =ConvertionClass.convertIntoUnsigned1ByteInt(value[0]);
            structureSize = 1 + noOfVuCalibrationRecords * VuCalibrationRecord.structureSize;

            if (noOfVuCalibrationRecords != 0)
            {
                for (int i = 0; i < noOfVuCalibrationRecords; i++)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, 1 + (i * VuCalibrationRecord.structureSize), VuCalibrationRecord.structureSize);
                    VuCalibrationRecord vcr = new VuCalibrationRecord(record);
                    vuCalibrationRecords.Add(vcr);
                }
            }
        }
        
    }
}
