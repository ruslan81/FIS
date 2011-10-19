using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardSlotNumber//1byte
    {
        //DRIVERSLOT     = 0
        //CO_DRIVER_SLOT = 1
        public short cardSlotNumber { get; set; }

        public CardSlotNumber()
        {
            cardSlotNumber = 0;
        }

        public CardSlotNumber(byte[] value)
        {
            cardSlotNumber = ConvertionClass.convertIntoUnsigned1ByteInt(value[0]);
        }

        public CardSlotNumber(byte value)
        {
            cardSlotNumber = ConvertionClass.convertIntoUnsigned1ByteInt(value);
        }
        /// <summary>
        /// перегруженный метод ToString()
        /// возвращает какой слот в данный используется
        /// </summary>
        /// <returns>какой слот используется в данный момент Driver slot или Co-driver slot</returns>
        public override string ToString()
        {
            if (cardSlotNumber == 0)//DRIVERSLOT
            {
                return "Driver slot";
            }
            if (cardSlotNumber == 1) //CO_DRIVER_SLOT
            {
                return "Co-driver slot";
            }

            return "????";
        }
    }
}
