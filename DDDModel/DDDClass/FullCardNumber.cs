using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class FullCardNumber//18bytes
    {
        public EquipmentType cardType { get; set; }
        public NationNumeric cardIssuingMemberState { get; set; }
        public CardNumber cardNumber { get; set; }

        public FullCardNumber()
        {
            cardType = new EquipmentType();
            cardIssuingMemberState = new NationNumeric();
            cardNumber = new CardNumber();
        }

        public FullCardNumber(byte[] value)
        {
            cardType = new EquipmentType(value[0]);
            cardIssuingMemberState = new NationNumeric(value[1]);
            cardNumber = new CardNumber(ConvertionClass.arrayCopy(value, 2, 16), cardType.equipmentType);
        }
    }
}
