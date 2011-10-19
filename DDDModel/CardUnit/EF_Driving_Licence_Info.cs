using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// информация водительской лицензии
    /// </summary>
    public class EF_Driving_Licence_Info
    {
        private readonly static int structureSize = CardDrivingLicenceInformation.structureSize;

        public CardDrivingLicenceInformation cardDrivingLicenceInformation { get; set; }

        public EF_Driving_Licence_Info()
        {
            cardDrivingLicenceInformation = new CardDrivingLicenceInformation();
        }

        public EF_Driving_Licence_Info(byte[] value)
        {
            cardDrivingLicenceInformation = new CardDrivingLicenceInformation(value);
        }
    }
}
