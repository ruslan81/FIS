using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class DriverCardHolderIdentification//78 bytes
    {
        public readonly static int structureSize = 78;

        public HolderName cardHolderName { get; set; }
        public Datef cardHolderBirthDate { get; set; }
        public Language cardHolderPreferredLanguage { get; set; }

        public DriverCardHolderIdentification()
        {
            cardHolderName = new HolderName();
            cardHolderBirthDate = new Datef();
            cardHolderPreferredLanguage = new Language();
        }

        public DriverCardHolderIdentification(byte[] value)
        {
            cardHolderName = new HolderName(ConvertionClass.arrayCopy(value, 0, 72));
            cardHolderBirthDate = new Datef(ConvertionClass.arrayCopy(value, 72, 4));
            cardHolderPreferredLanguage = new Language(ConvertionClass.arrayCopy(value, 76, 2));
        }

    }
}
