using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuSoftInstallationDate : TimeReal
    {
        public VuSoftInstallationDate()
            : base()
        { }

        public VuSoftInstallationDate(byte[] value)
            : base(value)
        { }

        public VuSoftInstallationDate(long i)
            : base(i)
        { }
    }
}
