using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardPlaceDailyWorkPeriod
    {
        public readonly int structureSize;

        public List<PlaceRecord> placeRecords { get; set; }

        public CardPlaceDailyWorkPeriod()
        {
            placeRecords = new List<PlaceRecord>();
        }

        public CardPlaceDailyWorkPeriod(byte[] value, short noOfCardPlaceRecords)
        {
            int noOfValidCardPlaceRecords = 0;
            placeRecords = new List<PlaceRecord>();

            for (int i = 0; i < noOfCardPlaceRecords; i += 1)
            {
                byte[] record = ConvertionClass.arrayCopy(value, 1 + (i * PlaceRecord.structureSize), PlaceRecord.structureSize);

                PlaceRecord pr = new PlaceRecord(record);
                if (pr.entryTime.timereal != 0)
                {
                    placeRecords.Add(pr);
                    noOfValidCardPlaceRecords += 1;
                }
            }
            structureSize = 1 + noOfValidCardPlaceRecords * PlaceRecord.structureSize;
        }
    }
}
