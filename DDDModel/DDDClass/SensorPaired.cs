using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class SensorPaired//20bytes
    {
        public SensorSerialNumber sensorSerialNumber { get; set; }
        public SensorApprovalNumber sensorApprovalNumber { get; set; }
        public SensorPairingDate sensorPairingDateFirst { get; set; }

        public SensorPaired()
        {
            sensorSerialNumber = new SensorSerialNumber();
            sensorApprovalNumber = new SensorApprovalNumber();
            sensorPairingDateFirst = new SensorPairingDate();
        }

        public SensorPaired(byte[] value)
        {
            sensorSerialNumber = new SensorSerialNumber(ConvertionClass.arrayCopy(value, 0, 8));
            sensorApprovalNumber = new SensorApprovalNumber(ConvertionClass.arrayCopy(value, 8, 8));
            sensorPairingDateFirst = new SensorPairingDate(ConvertionClass.arrayCopy(value, 16, 4));
        }
    }
}
