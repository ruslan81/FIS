using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// Посещенные места
    /// </summary>
    public class EF_Places
    {
        private readonly int structureSize;

        public CardPlaceDailyWorkPeriod cardPlaceDailyWorkPeriod { get; set; }

        public EF_Places()
        { }

        public EF_Places(byte[] value, short noOfCardPlaceRecords)
        {
            cardPlaceDailyWorkPeriod = new CardPlaceDailyWorkPeriod(value, noOfCardPlaceRecords);
            structureSize = cardPlaceDailyWorkPeriod.structureSize;
        }
    }
}
