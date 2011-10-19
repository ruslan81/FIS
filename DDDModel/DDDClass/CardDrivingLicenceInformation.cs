using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardDrivingLicenceInformation//53bytes
    {
        public readonly static int structureSize = 53;

        public Name drivingLicenceIssuingAuthority { get; set; }
        public NationNumeric drivingLicenceIssuingNation { get; set; }
        public string drivingLicenceNumber { get; set; }

        public CardDrivingLicenceInformation()
        {
            drivingLicenceIssuingAuthority = new Name();
            drivingLicenceIssuingNation = new NationNumeric();
            drivingLicenceNumber = new string("".ToCharArray());
        }

        public CardDrivingLicenceInformation(byte[] value)
        {
            drivingLicenceIssuingAuthority = new Name(ConvertionClass.arrayCopy(value, 0, 36));
            drivingLicenceIssuingNation = new NationNumeric(value[36]);
            drivingLicenceNumber = ConvertionClass.convertIntoString(ConvertionClass.arrayCopy(value, 37, 16)).Trim();
        }

    }
}
