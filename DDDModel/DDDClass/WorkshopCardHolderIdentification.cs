using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class WorkshopCardHolderIdentification//146bytes
    {
        public readonly static int structureSize = 146;

        public Name workshopName{ get; set; }
        public Address workshopAddress{ get; set; }
        public HolderName cardHolderName{ get; set; }
        public Language cardHolderPreferredLanguage{ get; set; }

        public WorkshopCardHolderIdentification()
        {
            workshopName = new Name();
            workshopAddress = new Address();
            cardHolderName = new HolderName();
            cardHolderPreferredLanguage = new Language();
        }

        public WorkshopCardHolderIdentification(byte[] value)
        {
            workshopName = new Name(ConvertionClass.arrayCopy(value, 0, 36));
            workshopAddress = new Address(ConvertionClass.arrayCopy(value, 36, 36));
            cardHolderName = new HolderName(ConvertionClass.arrayCopy(value, 72, 72));
            cardHolderPreferredLanguage = new Language(ConvertionClass.arrayCopy(value, 144, 2));
        }
    }
}

/*
	 * WorkshopCardHolderIdentification ::= SEQUENCE {
	 * 	workshopName Name, 36 bytes
	 * 	workshopAddress Address, 36 bytes
	 * 	cardHolderName HolderName, 72 bytes
	 * 	cardHolderPreferredLanguage Language, 2 bytes
	 * }
	 */