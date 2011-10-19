using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class ControlCardApplicationIdentification
    {
        public readonly static int structureSize = 5;

        public EquipmentType typeOfTachographCardId { get; set; }
        public CardStructureVersion cardStructureVersion { get; set; }
        public NoOfControlActivityRecords noOfControlActivityRecords { get; set; }

        public ControlCardApplicationIdentification()
        {
            typeOfTachographCardId = new EquipmentType();
            cardStructureVersion = new CardStructureVersion();
            noOfControlActivityRecords = new NoOfControlActivityRecords();
        }

        public ControlCardApplicationIdentification(byte[] value)
        {
            typeOfTachographCardId = new EquipmentType(value[0]);
            cardStructureVersion = new CardStructureVersion(ConvertionClass.arrayCopy(value, 1, 2));
            noOfControlActivityRecords = new NoOfControlActivityRecords(ConvertionClass.arrayCopy(value, 3, 2));
        }

    }
}
