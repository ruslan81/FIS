using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// Информация о калибровке. используется только когда карточка мастерской.
    /// </summary>
    public class EF_Calibration
    {
        private readonly int structureSize;

        public WorkshopCardCalibrationData workshopCardCalibrationData { get; set; }

        public EF_Calibration()
        { }

        public EF_Calibration(byte[] value, short noOfCalibrationRecords)
        {
            workshopCardCalibrationData = new WorkshopCardCalibrationData(value, noOfCalibrationRecords);
            structureSize = workshopCardCalibrationData.structureSize;
        }
    }
}
