using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardSlotsStatus//1byte
    {
        public byte cardSlotsStatus { get; set; }

        public static readonly byte DRIVER_CARD_INSERTED = 0x1;
        public static readonly byte WORKSHOP_CARD_INSERTED = 0x2;
        public static readonly byte CONTROL_CARD_INSERTED = 0x3;
        public static readonly byte COMPANY_CARD_INSERTED = 0x4;


        public CardSlotsStatus()
        {
            cardSlotsStatus = 0;
        }

        public CardSlotsStatus(byte[] value)
        {
            cardSlotsStatus = value[0];
        }

        public CardSlotsStatus(byte value)
        {
            cardSlotsStatus = value;
        }
        /// <summary>
        /// возвращает статус(тип) карты в слоте для карты второго водителя
        /// </summary>
        /// <returns>один байт</returns>
        public byte getCoDriverCardSlotsStatus()
        {
            return (byte)((cardSlotsStatus >> 4) & 0xf);
        }
        /// <summary>
        /// возвращает статус(тип) карты в слоте для карты водителя
        /// </summary>
        /// <returns>один байт</returns>
        public byte getDriverCardSlotsStatus()
        {
            return (byte)(cardSlotsStatus & 0xf);
        }
        /// <summary>
        /// возвращает статус(тип) карты в слоте для карты водителя
        /// </summary>
        /// <returns>DRIVER CARD, WORKSHOP CARD, CONTROL CARD, COMPANY CARD, UNKNOWN CARD</returns>
        public string getDriverCardSlotsStatus_toString()
        {
            string result;
            switch (ConvertionClass.convertIntoUnsigned1ByteInt(getDriverCardSlotsStatus()))
            {
                case 0x1:
                    result = "DRIVER CARD";
                    break;
                case 0x2:
                    result = "WORKSHOP CARD";
                    break;
                case 0x3:
                    result = "CONTROL CARD";
                    break;
                case 0x4:
                    result = "COMPANY CARD";
                    break;
                default:
                    result = "UNKNOWN CARD";
                    break;
            }
            return result;
        }
        /// <summary>
        /// возвращает статус(тип) карты в слоте для карты второго водителя
        /// </summary>
        /// <returns>DRIVER CARD, WORKSHOP CARD, CONTROL CARD, COMPANY CARD, UNKNOWN CARD</returns>
        public string getCoDriverCardSlotsStatus_toString()
        {
            string result;
            switch (ConvertionClass.convertIntoUnsigned1ByteInt(getCoDriverCardSlotsStatus()))
            {
                case 0x1:
                    result = "DRIVER CARD";
                    break;
                case 0x2:
                    result = "WORKSHOP CARD";
                    break;
                case 0x3:
                    result = "CONTROL CARD";
                    break;
                case 0x4:
                    result = "COMPANY CARD";
                    break;
                default:
                    result = "UNKNOWN CARD";
                    break;
            }
            return result;
        }
        /// <summary>
        /// перегруженный метод. В основном использовался для дебага и разработки.
        /// </summary>
        /// <returns>возращает строку со всеми данными сразу</returns>
        public override string ToString()
        {
            string result;

            switch (ConvertionClass.convertIntoUnsigned1ByteInt(getDriverCardSlotsStatus()))
            {
                case 0x1:
                    result = "DriverCard Slot Status: DRIVER_CARD_INSERTED \r\n";
                    break;
                case 0x2:
                    result = "DriverCard Slot Status: WORKSHOP_CARD_INSERTED \r\n";
                    break;
                case 0x3:
                    result = "DriverCard Slot Status: CONTROL_CARD_INSERTED \r\n";
                    break;
                case 0x4:
                    result = "DriverCard Slot Status: COMPANY_CARD_INSERTED \r\n";
                    break;
                default:
                    result = "DriverCardSlotsStatus: UNKNOWN_CARD_INSERTED \r\n";
                    break;
            }
            switch (ConvertionClass.convertIntoUnsigned1ByteInt(getCoDriverCardSlotsStatus()))
            {
                case 0x1:
                    result += "CoDriverCard Slot Status: DRIVER_CARD_INSERTED\r\n";
                    break;
                case 0x2:
                    result += "CoDriverCard Slot Status: WORKSHOP_CARD_INSERTED\r\n";
                    break;
                case 0x3:
                    result += "CoDriverCard Slot Status: CONTROL_CARD_INSERTED\r\n";
                    break;
                case 0x4:
                    result += "CoDriverCard Slot Status: COMPANY_CARD_INSERTED\r\n";
                    break;
                default:
                    result += "CoDriverCard Slot Status: UNKNOWN_CARD_INSERTED\r\n";
                    break;
            }
                return result;
        }
    }
}
