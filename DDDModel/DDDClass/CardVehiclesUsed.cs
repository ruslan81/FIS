using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardVehiclesUsed
    {
        public readonly int structureSize;

        public List<CardVehicleRecord> cardVehicleRecords { get; set; }

        public CardVehiclesUsed()
        { }

        public CardVehiclesUsed(byte[] value, int noOfCardVehicleRecords)
        {
            int noOfValidCardVehicleRecords = 0;
            cardVehicleRecords = new List<CardVehicleRecord>();

            for (int i = 0; i < noOfCardVehicleRecords; i += 1)
            {
                byte[] record =ConvertionClass.arrayCopy(value, 2 + (i * CardVehicleRecord.structureSize), CardVehicleRecord.structureSize);

                CardVehicleRecord cvr = new CardVehicleRecord(record);

                // only add entries with non-default values, i.e. skip empty entries
                if (cvr.vehicleFirstUse.timereal != 0)
                {
                    cardVehicleRecords.Add(cvr);

                    noOfValidCardVehicleRecords += 1;
                }
            }

            structureSize = 2 + noOfValidCardVehicleRecords * CardVehicleRecord.structureSize;
        }

    }
}
