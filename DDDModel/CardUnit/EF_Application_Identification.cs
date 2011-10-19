using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    using HexBytes = ConvertionClass;
    /// <summary>
    /// Описание структуры данных на карточке
    /// </summary>
    [Serializable]
    public class EF_Application_Identification
    {
        public DriverCardApplicationIdentification driverCardApplicationIdentification { get; set; }
        public WorkshopCardApplicationIdentification workshopCardApplicationIdentification { get; set; }
        public ControlCardApplicationIdentification controlCardApplicationIdentification { get; set; }
        public CompanyCardApplicationIdentification companyCardApplicationIdentification { get; set; }

        // all cards
        public  short cardType{get; set;}
        private byte[] cardStructureVersion;

        // driver card, workshop card
        public readonly short noOfEventsPerType;
        public readonly short noOfFaultsPerType;
        public readonly int activityStructureLength;
        public readonly int noOfCardVehicleRecords;
        public readonly short noOfCardPlaceRecords;

        // workshop card
        public readonly short noOfCalibrationRecords;

        // control card
        public readonly int noOfControlActivityRecords;

        // company card
        public readonly int noOfCompanyActivityRecords;

        public EF_Application_Identification()
        {
            driverCardApplicationIdentification = new DriverCardApplicationIdentification();
            workshopCardApplicationIdentification = new WorkshopCardApplicationIdentification();
            controlCardApplicationIdentification = new ControlCardApplicationIdentification();
            companyCardApplicationIdentification = new CompanyCardApplicationIdentification();
        }

        public EF_Application_Identification(byte[] value)
        {

            // size = value.length;
            cardType = HexBytes.convertIntoUnsigned1ByteInt(value[0]);

            switch (cardType)
            {
                case 1: //DRIVER_CARD
                    {
                        driverCardApplicationIdentification = new DriverCardApplicationIdentification(value);

                        cardStructureVersion = driverCardApplicationIdentification.cardStructureVersion.Get_CardStructureVersion_Bytes();
                        noOfEventsPerType = driverCardApplicationIdentification.noOfEventsPerType.noOfEventsPerType;
                        noOfFaultsPerType = driverCardApplicationIdentification.noOfFaultsPerType.noOfFaultsPerType;
                        activityStructureLength = driverCardApplicationIdentification.activityStructureLength.cardActivityLengthRange;
                        noOfCardVehicleRecords = driverCardApplicationIdentification.noOfCardVehicleRecords.noOfCardVehicleRecords;
                        noOfCardPlaceRecords = driverCardApplicationIdentification.noOfCardPlaceRecords.noOfCardPlaceRecords;
                    }
                    break;

                case 2: //WORKSHOP_CARD
                    {
                        workshopCardApplicationIdentification = new WorkshopCardApplicationIdentification(value);

                        cardStructureVersion = workshopCardApplicationIdentification.cardStructureVersion.Get_CardStructureVersion_Bytes();
                        noOfEventsPerType = workshopCardApplicationIdentification.noOfEventsPerType.noOfEventsPerType;
                        noOfFaultsPerType = workshopCardApplicationIdentification.noOfFaultsPerType.noOfFaultsPerType;
                        activityStructureLength = workshopCardApplicationIdentification.activityStructureLength.cardActivityLengthRange;
                        noOfCardVehicleRecords = workshopCardApplicationIdentification.noOfCardVehicleRecords.noOfCardVehicleRecords;
                        noOfCardPlaceRecords = workshopCardApplicationIdentification.noOfCardPlaceRecords.noOfCardPlaceRecords;
                        noOfCalibrationRecords = workshopCardApplicationIdentification.noOfCalibrationRecords.noOfCalibrationRecords;
                    }
                    break;

                case 3: //CONTROL_CARD
                    {
                        controlCardApplicationIdentification = new ControlCardApplicationIdentification(value);
                        cardStructureVersion = controlCardApplicationIdentification.cardStructureVersion.Get_CardStructureVersion_Bytes();
                        noOfControlActivityRecords = controlCardApplicationIdentification.noOfControlActivityRecords.noOfControlActivityRecords;
                    }
                    break;

                case 4: //COMPANY_CARD
                    {
                        companyCardApplicationIdentification = new CompanyCardApplicationIdentification(value);

                        cardStructureVersion = companyCardApplicationIdentification.cardStructureVersion.Get_CardStructureVersion_Bytes();
                        noOfCompanyActivityRecords = companyCardApplicationIdentification.noOfCompanyActivityRecords.noOfCompanyActivityRecords;
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
