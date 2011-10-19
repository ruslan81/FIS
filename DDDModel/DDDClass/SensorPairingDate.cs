using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class SensorPairingDate : TimeReal
    {
        public SensorPairingDate()
            : base()
        { }

        public SensorPairingDate(byte[] value)
            : base(value)
        { }

        public SensorPairingDate(long i)
            : base(i)
        { }
    }
}
