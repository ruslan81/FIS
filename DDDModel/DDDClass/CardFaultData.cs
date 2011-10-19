using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardFaultData
    {
        public readonly int structureSize;

        public List<List<CardFaultRecord>> cardFaultRecords { get; set; }

        public CardFaultData()
        {}

        public CardFaultData(byte[] value, short noOfFaultsPerType)
        {
            int noOfValidFaultRecords = 0;
            cardFaultRecords = new List<List<CardFaultRecord>>();

            for (int j = 0; j < 2 /*SEQUENCE SIZE*/; j++)
            {
                cardFaultRecords.Add(new List<CardFaultRecord>(24));

                for (int i = (noOfFaultsPerType * CardFaultRecord.structureSize * j); i < (noOfFaultsPerType * CardFaultRecord.structureSize * (j + 1)); i += CardFaultRecord.structureSize)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, i, CardFaultRecord.structureSize);
                    CardFaultRecord cfr = new CardFaultRecord(record);
                    cardFaultRecords[j].Add(cfr);
                    noOfValidFaultRecords += 1;
                }
            }
            structureSize = noOfValidFaultRecords * CardFaultRecord.structureSize;
        }
        /// <summary>
        /// ToString перегружен. Возвращает строки в соотвествии с документацией.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string returnString = "";

            for (int j = 0; j < 2 /*SEQUENCE SIZE*/; j++)
            {

                switch (j)
                {
                    case 0:
                        returnString = " recording eq. faults:";
                        break;

                    case 1:
                        returnString = " card faults:";
                        break;

                    default:
                        break;
                }

                for (int i = 0; i < cardFaultRecords[j].Count; i += 1)
                {
                    CardFaultRecord cfr = cardFaultRecords[j][i];

                    if (cfr.faultBeginTime.timereal != 0)
                    {
                        returnString += "\r\n - event fault type: " + cfr.faultType.eventFaultType.ToString() + " " + cfr.faultType.ToString();
                    }
                }
                returnString += "\r\n";
            }

            return returnString;
        }
    }
}
            
            
            

