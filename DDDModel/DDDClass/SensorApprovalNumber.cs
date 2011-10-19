using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class SensorApprovalNumber//8 bytes
    {
        public string sensorApprovalNumber { get; set; }

        public SensorApprovalNumber()
        {
            sensorApprovalNumber = new string("".ToCharArray());
        }

        public SensorApprovalNumber(byte[] value)
        {
            sensorApprovalNumber = ConvertionClass.convertIntoString(ConvertionClass.arrayCopy(value, 0, 8));
        }

        public SensorApprovalNumber(string value)
        {
            sensorApprovalNumber = value;
        }

        public override string ToString()
        {
            return this.sensorApprovalNumber;
        }
    }
}
