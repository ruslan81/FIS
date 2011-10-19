using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// EF_Current_Usage
    /// </summary>
    public class EF_Current_Usage
    {
        private readonly static int structureSize = CardCurrentUse.structureSize;

        public CardCurrentUse cardCurrentUse { get; set; }

        public EF_Current_Usage()
        {
            cardCurrentUse = new CardCurrentUse();
        }

        public EF_Current_Usage(byte[] value)
        {
            cardCurrentUse = new CardCurrentUse(value);
        }
    }
}
