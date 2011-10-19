using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class SensorSerialNumber : ExtendedSerialNumber //8bytes
    {
        public SensorSerialNumber()
            : base()
        { }
        public SensorSerialNumber(byte[] value)
            : base(value)
        { }   
    }
}
