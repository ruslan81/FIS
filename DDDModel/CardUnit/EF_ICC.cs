using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// ICC
    /// </summary>
    public class EF_ICC
    {
        public CardIccIdentification cardIccIdentification { get; set; }

        public EF_ICC()
        {
            cardIccIdentification = new CardIccIdentification();
        }

        public EF_ICC(byte[] value)
        {
            cardIccIdentification = new CardIccIdentification(value);
        }
    }
}
