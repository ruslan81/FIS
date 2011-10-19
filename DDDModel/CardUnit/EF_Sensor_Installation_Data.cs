using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// дата установки датчиков
    /// </summary>
    public class EF_Sensor_Installation_Data
    {
        private readonly int structureSize = SensorInstallationSecData.structureSize;

        private SensorInstallationSecData sensorInstallationSecData { get; set; }

        public EF_Sensor_Installation_Data()
        { }

        public EF_Sensor_Installation_Data(byte[] value)
        {
            sensorInstallationSecData = new SensorInstallationSecData(value);
        }
    }
}
