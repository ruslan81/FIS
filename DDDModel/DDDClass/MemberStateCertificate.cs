using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class MemberStateCertificate : Certificate
    {
        public MemberStateCertificate()
            : base()
        {
        }

        public MemberStateCertificate(byte[] certificate)
            : base(certificate)
        {
        }
    }
}
