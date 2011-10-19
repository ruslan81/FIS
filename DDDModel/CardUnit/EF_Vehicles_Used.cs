using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// Использование ТС
    /// </summary>
    public class EF_Vehicles_Used
    {
        public readonly int structureSize;

        public CardVehiclesUsed cardVehiclesUsed { get; set; }

        public EF_Vehicles_Used()
        { }

        public EF_Vehicles_Used(byte[] value, int noOfCardVehicleRecords)
        {
            cardVehiclesUsed = new CardVehiclesUsed(value, noOfCardVehicleRecords);
            structureSize = cardVehiclesUsed.structureSize;
        }
    }
}
