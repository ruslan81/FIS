using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardCertificate : Certificate
    {
        public CardCertificate()
            : base()
        { }

        public CardCertificate(byte[] certificate)
            : base(certificate)
        { }
    }
}
