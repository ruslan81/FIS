using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    [Serializable]
    public class DriverCardApplicationIdentification//10 bytes
    {
        public readonly static int structureSize = 10;

        public EquipmentType typeOfTachographCardId { get; set; }
        public CardStructureVersion cardStructureVersion { get; set; }
        public NoOfEventsPerType noOfEventsPerType { get; set; }
        public NoOfFaultsPerType noOfFaultsPerType { get; set; }
        public CardActivityLengthRange activityStructureLength { get; set; }
        public NoOfCardVehicleRecords noOfCardVehicleRecords { get; set; }
        public NoOfCardPlaceRecords noOfCardPlaceRecords { get; set; }

        public DriverCardApplicationIdentification()
        {
            typeOfTachographCardId = new EquipmentType();
            cardStructureVersion = new CardStructureVersion();
            noOfEventsPerType = new NoOfEventsPerType();
            noOfFaultsPerType = new NoOfFaultsPerType();
            activityStructureLength = new CardActivityLengthRange();
            noOfCardVehicleRecords = new NoOfCardVehicleRecords();
            noOfCardPlaceRecords = new NoOfCardPlaceRecords();
        }
      
        public DriverCardApplicationIdentification(byte[] value)
        {
            typeOfTachographCardId = new EquipmentType(value[0]);
            cardStructureVersion = new CardStructureVersion(ConvertionClass.arrayCopy(value, 1, 2));
            noOfEventsPerType = new NoOfEventsPerType(value[3]);
            noOfFaultsPerType = new NoOfFaultsPerType(value[4]);
            activityStructureLength = new CardActivityLengthRange(ConvertionClass.arrayCopy(value, 5, 2));
            noOfCardVehicleRecords = new NoOfCardVehicleRecords(ConvertionClass.arrayCopy(value, 7, 2));
            noOfCardPlaceRecords = new NoOfCardPlaceRecords(value[9]);
        }

    }
}
