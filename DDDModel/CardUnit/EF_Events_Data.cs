using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// События
    /// </summary>
    public class EF_Events_Data
    {
        private readonly int structureSize;

        public CardEventData cardEventData { get; set; }

        public EF_Events_Data()
        { }

        public EF_Events_Data(byte[] value, short noOfEventsPerType)
        {
            cardEventData = new CardEventData(value, noOfEventsPerType);
            structureSize = cardEventData.structureSize;
        }
    }
}
