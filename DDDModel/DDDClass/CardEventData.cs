using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardEventData
    {
        public readonly int structureSize;
        public List<List<CardEventRecord>> cardEventRecords { get; set; }

        public CardEventData()
        {
            cardEventRecords = new List<List<CardEventRecord>>();
        }

        public CardEventData(byte[] value, short noOfEventsPerType)
        {
            int noOfValidEventRecords = 0;
            cardEventRecords = new List<List<CardEventRecord>>();

            for (int j = 0; j < 6 /*SEQUENCE SIZE*/; j++)
            {
                cardEventRecords.Add(new List<CardEventRecord>(12));

                for (int i = (noOfEventsPerType * CardEventRecord.structureSize * j); i < (noOfEventsPerType * CardEventRecord.structureSize * (j + 1)); i += CardEventRecord.structureSize)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, i, CardEventRecord.structureSize);
                    CardEventRecord cer = new CardEventRecord(record);
                    cardEventRecords[j].Add(cer);
                    noOfValidEventRecords += 1;
                }
            }
            structureSize = noOfValidEventRecords * CardEventRecord.structureSize;
        }
        /// <summary>
        /// ToString перегружен. Возвращает строки в соотвествии с документацией.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string returnString = "";

            for (int j = 0; j < 6 /*SEQUENCE SIZE*/; j++)
            {
                switch (j)
                {
                    case 0:
                        returnString = " time overlap events:";
                        break;

                    case 1:
                        returnString = " card insertion while driving events:";
                        break;

                    case 2:
                        returnString = " last card session not correctly closed events:";
                        break;

                    case 3:
                        returnString = " power supply interruption events:";
                        break;

                    case 4:
                        returnString = " motion data error events:";
                        break;

                    case 5:
                        returnString = " security breach attempt events:";
                        break;

                    default:
                        break;
                }

                for (int i = 0; i < cardEventRecords[j].Count; i += 1)
                {
                    CardEventRecord cer = cardEventRecords[j][i];

                    if (cer.eventBeginTime.timereal != 0)
                    {
                        returnString += ("\r\n - event fault type " + cer.eventType.eventFaultType + " " + cer.eventType.ToString());
                    }
                }
            }

            return returnString;
        }
    }
}

/*
	  CardEventData ::= SEQUENCE SIZE(6) OF 
      {
	  	cardEventRecords SET SIZE(NoOfEventsPerType) OF CardEventRecord
	  }
	  NoOfEventsPerType ::= 6..12 for driver card
	  NoOfEventsPerType ::= 3     for workshop card
	  according to requirement 204:
	  sequence				description								eventfaulttype
	  cardEventRecords[0]:	time overlap							'03'H
	  cardEventRecords[1]:	card insertion while driving			'05'H
	  cardEventRecords[2]:	last card session not correctly closed	'06'H
	  cardEventRecords[3]:	power supply interruption				'08'H
	  cardEventRecords[4]:	motion data error						'09'H
	  cardEventRecords[5]:	security breach attempts,
	  						vehicle unit related & sensor related	'10'H...'2F'H
	  sechs Folgen von Datensätzen (cardEventRecords[0..5]), die jeweils
	  "NoOfEventsPerType"-fach CardEventRecords der "EventFaultTypes" oben enthalten
	  (req. 204 & 223).
	 */

