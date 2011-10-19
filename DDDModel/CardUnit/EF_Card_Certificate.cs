using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// Сертификат карты
    /// </summary>
    public class EF_Card_Certificate
    {
        public CardCertificate cardCertificate { get; set; }

        public EF_Card_Certificate()
        { }

        public EF_Card_Certificate(byte[] value)
        {
            cardCertificate = new CardCertificate(value);
        }
    }
}
