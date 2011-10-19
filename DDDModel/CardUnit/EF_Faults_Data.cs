using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// Нарушения
    /// </summary>
    public class EF_Faults_Data
    {
        private readonly int structureSize;

        public CardFaultData cardFaultData { get; set; }

        public EF_Faults_Data()
        { }

        public EF_Faults_Data(byte[] value, short noOfFaultsPerType)
        {
            cardFaultData = new CardFaultData(value, noOfFaultsPerType);
            structureSize = cardFaultData.structureSize;
        }
    }
}
