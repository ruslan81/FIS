using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuCertificate : Certificate
    {
        public VuCertificate()
            : base()
        {
        }

        public VuCertificate(byte[] certificate)
            : base(certificate)
        { }
    }
}
