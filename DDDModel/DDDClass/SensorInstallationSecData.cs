using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class SensorInstallationSecData : TDesSessionKey
    {
        public SensorInstallationSecData()
            : base()
        { }

        public SensorInstallationSecData(byte[] value)
            : base(value)
        { }
    }
}
