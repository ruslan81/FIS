using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardIdentification//65bytes
    {
        public NationNumeric cardIssuingMemberState { get; set; }
        public CardNumber cardNumber { get; set; }
        public Name cardIssuingAuthorityName { get; set; }
        public TimeReal cardIssueDate { get; set; }
        public TimeReal cardValidityBegin { get; set; }
        public TimeReal cardExpiryDate { get; set; }

        public CardIdentification()
        {
            cardIssuingMemberState = new NationNumeric();
            cardNumber = new CardNumber();
            cardIssuingAuthorityName = new Name();
            cardIssueDate = new TimeReal();
            cardValidityBegin = new TimeReal();
            cardExpiryDate = new TimeReal();
        }

        public CardIdentification(byte[] value, short cardType)
        {
            cardIssuingMemberState = new NationNumeric(value[0]);
            cardNumber = new CardNumber(ConvertionClass.arrayCopy(value, 1, 16), cardType);
            cardIssuingAuthorityName = new Name(ConvertionClass.arrayCopy(value, 17, 36));
            cardIssueDate = new TimeReal(ConvertionClass.arrayCopy(value, 53, 4));
            cardValidityBegin = new TimeReal(ConvertionClass.arrayCopy(value, 57, 4));
            cardExpiryDate = new TimeReal(ConvertionClass.arrayCopy(value, 61, 4));
        }

    }
}

/*      CardIdentification ::= SEQUENCE 
        {
	 	    cardIssuingMemberState NationNumeric, 1 byte
	  	    cardNumber CardNumber, 16 bytes
	  	    cardIssuingAuthorityName Name, 36 bytes
	  	    cardIssueDate TimeReal, 4 bytes
	  	    cardValidityBegin TimeReal, 4 bytes
	  	    cardExpiryDate TimeReal, 4 bytes
	     }
	 */
