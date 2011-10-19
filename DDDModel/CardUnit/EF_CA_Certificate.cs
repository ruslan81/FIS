using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// СА_сертификаты
    /// </summary>
    public class EF_CA_Certificate
    {
        public MemberStateCertificate cardCertificate { get; set; }

        public EF_CA_Certificate()
        { }

        public EF_CA_Certificate(byte[] value)
        {
            cardCertificate = new MemberStateCertificate(value);
        }

    }
}
