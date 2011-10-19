using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CompanyCardHolderIdentification//74 байта
    {
        public readonly static int structureSize = 74;

        public Name companyName { get; set; }
        public Address companyAddress { get; set; }
        public Language cardHolderPreferredLanguage { get; set; }

        public CompanyCardHolderIdentification()
        {
            companyName = new Name();
            companyAddress = new Address();
            cardHolderPreferredLanguage = new Language();
        }

        public CompanyCardHolderIdentification(byte[] value)
        {
            companyName = new Name(ConvertionClass.arrayCopy(value, 0, 36));
            companyAddress = new Address(ConvertionClass.arrayCopy(value, 36, 36));
            cardHolderPreferredLanguage = new Language(ConvertionClass.arrayCopy(value, 72, 2));
        }
    }
}
