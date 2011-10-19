using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// IC
    /// </summary>
    public class EF_IC
    {
        public CardChipIdentification cardChipIdentification { get; set; }

        public EF_IC()
        {
            cardChipIdentification = new CardChipIdentification();
        }

        public EF_IC(byte[] value)
        {
            cardChipIdentification = new CardChipIdentification(value);
        }
    }
}
