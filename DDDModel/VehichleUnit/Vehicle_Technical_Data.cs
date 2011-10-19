using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace VehichleUnit
{
    using HexBytes = ConvertionClass;
    /// <summary>
    /// Техническиая информация о ТС
    /// </summary>
    public class Vehicle_Technical_Data
    {
        private readonly int size;

        public VuIdentification vuIdentification { get; set; }
        public SensorPaired sensorPaired { get; set; }
        public VuCalibrationData vuCalibrationData { get; set; }

        public Vehicle_Technical_Data()
        {
            vuIdentification = new VuIdentification();
            sensorPaired = new SensorPaired();
            vuCalibrationData = new VuCalibrationData();
        }

        public Vehicle_Technical_Data(byte[] value)
        {

            int offset1 = 116 + 20;
            vuIdentification = new VuIdentification(HexBytes.arrayCopy(value, 0, 116));

            sensorPaired = new SensorPaired(HexBytes.arrayCopy(value, 116, 20));

            int offset2 = 1 + HexBytes.convertIntoUnsigned1ByteInt(value[136]) * VuCalibrationRecord.structureSize;
            vuCalibrationData = new VuCalibrationData(HexBytes.arrayCopy(value, 136, offset2));

            size = offset1 + offset2;
        }
    }
}
