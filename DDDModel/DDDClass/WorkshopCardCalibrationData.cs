using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class WorkshopCardCalibrationData
    {
        public int structureSize;

        public int calibrationTotalNumber{ get; set; }
        public short calibrationPointerNewestRecord{ get; set; }
        public List<WorkshopCardCalibrationRecord> calibrationRecords{ get; set; }

        public WorkshopCardCalibrationData()
        {
            calibrationRecords = new List<WorkshopCardCalibrationRecord>();
        }

        public WorkshopCardCalibrationData(byte[] value, short noOfCalibrationRecords)
        {
            calibrationTotalNumber = ConvertionClass.convertIntoUnsigned2ByteInt(ConvertionClass.arrayCopy(value, 0, 2));
            calibrationRecords = new List<WorkshopCardCalibrationRecord>();

            int noOfValidCalibrationRecords = 0;

            for (int i = 0; i < noOfCalibrationRecords; i += 1)
            {
                byte[] record = ConvertionClass.arrayCopy(value, 3 + (i * WorkshopCardCalibrationRecord.structureSize), WorkshopCardCalibrationRecord.structureSize);

                WorkshopCardCalibrationRecord wccr = new WorkshopCardCalibrationRecord(record);

                // only add entries with non-default values, i.e. skip empty entries
                if (wccr.calibrationPurpose.calibrationPurpose != 0)
                {
                    calibrationRecords.Add(wccr);

                    noOfValidCalibrationRecords++;
                }
            }

            structureSize = 3 + noOfValidCalibrationRecords * WorkshopCardCalibrationRecord.structureSize;
        }
    }
}
