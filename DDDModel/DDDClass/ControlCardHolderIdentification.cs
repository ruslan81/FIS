using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class ControlCardHolderIdentification//146 байт
    {
        public Name controlBodyName { get; set; }
        public Address controlBodyAddress { get; set; }
        public HolderName cardHolderName { get; set; }
        public Language cardHolderPreferredLanguage { get; set; }


        public ControlCardHolderIdentification()
        {
            controlBodyName = new Name();
            controlBodyAddress = new Address();
            cardHolderName = new HolderName();
            cardHolderPreferredLanguage = new Language();
        }


        public ControlCardHolderIdentification(byte[] value)
        {
            controlBodyName = new Name(ConvertionClass.arrayCopy(value, 0, 36));
            controlBodyAddress = new Address(ConvertionClass.arrayCopy(value, 36, 36));
            cardHolderName = new HolderName(ConvertionClass.arrayCopy(value, 72, 72));
            cardHolderPreferredLanguage = new Language(ConvertionClass.arrayCopy(value, 144, 2));
        }
    }
}
