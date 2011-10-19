using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class LastCardDownload :TimeReal
    {
        public LastCardDownload()
            : base()
        { }

        public LastCardDownload(byte[] value)
            : base(value)
        { }

        public LastCardDownload(long i)
            : base(i)
        { }
    }
}
