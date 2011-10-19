using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CompanyCardApplicationIdentification
    {
        public EquipmentType typeOfTachographCardId { get; set; }
        public CardStructureVersion cardStructureVersion { get; set; }
        public NoOfCompanyActivityRecords noOfCompanyActivityRecords { get; set; }

        public CompanyCardApplicationIdentification()
        {
            typeOfTachographCardId = new EquipmentType();
            cardStructureVersion = new CardStructureVersion();
            noOfCompanyActivityRecords = new NoOfCompanyActivityRecords();
        }

        public CompanyCardApplicationIdentification(byte[] value)
        {
            typeOfTachographCardId = new EquipmentType(value[0]);
            cardStructureVersion = new CardStructureVersion(ConvertionClass.arrayCopy(value, 1, 2));
            noOfCompanyActivityRecords = new NoOfCompanyActivityRecords(ConvertionClass.arrayCopy(value, 3, 2));
        }
    }
}
