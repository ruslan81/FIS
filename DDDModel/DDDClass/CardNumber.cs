using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// класс описывает номер карты
    /// </summary>
    public class CardNumber//16bytes
    {
        /// <summary>
        /// типа карты - водитель, тс, предприятие либо мастерская
        /// </summary>
        private short cardType;
        //следующая информация для всех типов карт.
        public string driverIdentification { get; set; }
        public CardReplacementIndex cardReplacementIndex { get; set; }
        public CardRenewalIndex cardRenewalIndex { get; set; }
        //доп.информация для НЕ водительских карт(всех трех остальных) 
        public string ownerIdentification { get; set; }
        public CardConsecutiveIndex cardConsecutiveIndex { get; set; }

        public CardNumber()
        {
            cardType = 0;
            driverIdentification = new String("".ToCharArray());
            cardReplacementIndex = new CardReplacementIndex();
            cardRenewalIndex = new CardRenewalIndex();
            ownerIdentification = "";
            cardConsecutiveIndex = new CardConsecutiveIndex();
        }
       
        public CardNumber(short cardType)
        {
            this.cardType = cardType;

            if (cardType == EquipmentType.DRIVER_CARD)
            {               
                driverIdentification = "";
                cardReplacementIndex = new CardReplacementIndex();
                cardRenewalIndex = new CardRenewalIndex();

                ownerIdentification = "";
                cardConsecutiveIndex = new CardConsecutiveIndex();
            }
            else
            {
                // workshop, control or company card
                ownerIdentification = "";
                cardConsecutiveIndex = new CardConsecutiveIndex();
                cardReplacementIndex = new CardReplacementIndex();
                cardRenewalIndex = new CardRenewalIndex();
                driverIdentification = "";
            }
        }

        public CardNumber(byte[] value, short cardType)
        {
            this.cardType = cardType;

            if (cardType == EquipmentType.DRIVER_CARD)
            {
                driverIdentification = ConvertionClass.convertIntoString(ConvertionClass.arrayCopy(value, 0, 14));
                cardReplacementIndex = new CardReplacementIndex(value[14]);
                cardRenewalIndex = new CardRenewalIndex(value[15]);

                ownerIdentification = "";
                cardConsecutiveIndex = new CardConsecutiveIndex();
            }
            else
            {
                ownerIdentification = ConvertionClass.convertIntoString(ConvertionClass.arrayCopy(value, 0, 13));
                cardConsecutiveIndex = new CardConsecutiveIndex(value[13]);
                cardReplacementIndex = new CardReplacementIndex(value[14]);
                cardRenewalIndex = new CardRenewalIndex(value[15]);
                driverIdentification = "";
            }
        }
        /// <summary>
        /// возвращает номер карты водителя. Использовать, когда есть уверенность, что это карта водителя,
        /// иначе использовать метод ownerIdentificationNumber();
        /// </summary>
        /// <returns>строка с полным номером водителя</returns>
        public string driverIdentificationNumber()
        {
            string returnString = driverIdentification.ToString() + cardReplacementIndex.cardReplacementIndex + cardRenewalIndex.cardRenewalIndex;
            if (returnString.Length < 10)
                returnString = " ";
            return returnString;
        }
        /// <summary>
        /// Возвращает номер владельца карты, если это не водитель.
        /// Если водитель - использовать метод driverIdentificationNumber();
        /// </summary>
        /// <returns>строка с полным номером владельца карты</returns>
        public string ownerIdentificationNumber()
        {
            string returnString = ownerIdentification.ToString() + cardConsecutiveIndex.cardConsecutiveIndex + cardReplacementIndex.cardReplacementIndex + cardRenewalIndex.cardRenewalIndex;
            if (returnString.Length < 10)
                returnString = " ";
            return returnString;
        }
        /// <summary>
        /// перегруженный метод ToString сам определяет тип карты и возвращает строку с номером карты.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string returnString;

            if (this.cardType == EquipmentType.DRIVER_CARD)
            {
                returnString = driverIdentification.ToString() + cardReplacementIndex.cardReplacementIndex + cardRenewalIndex.cardRenewalIndex;
            }
            else
            {
                returnString = ownerIdentification.ToString() + cardConsecutiveIndex.cardConsecutiveIndex + cardReplacementIndex.cardReplacementIndex + cardRenewalIndex.cardRenewalIndex;
            }

            return returnString;
        }

    }
}
