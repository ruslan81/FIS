using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    using HexBytes = ConvertionClass;

    /// <summary>
    /// Описание держателя карты
    /// </summary>
    public class EF_Identification
    {
        public readonly int structureSize;

        public int cardType{ get; set; }
        public CardIdentification cardIdentification{ get; set; }
        public DriverCardHolderIdentification driverCardHolderIdentification{ get; set; }
        public WorkshopCardHolderIdentification workshopCardHolderIdentification{ get; set; }
        public ControlCardHolderIdentification controlCardHolderIdentification { get; set; }
        public CompanyCardHolderIdentification companyCardHolderIdentification{ get; set; }

        public EF_Identification()
        { }

        public EF_Identification(byte[] value, short cardType)
        {
            this.cardType = cardType;
            cardIdentification = new CardIdentification(HexBytes.arrayCopy(value, 0, 65), cardType);

            if (EquipmentType.COMPANY_CARD == cardType)
            {
                companyCardHolderIdentification = new CompanyCardHolderIdentification(HexBytes.arrayCopy(value, 65, value.Length - 65));
                structureSize = CompanyCardHolderIdentification.structureSize;
            }
            else if (EquipmentType.CONTROL_CARD == cardType)
            {
                controlCardHolderIdentification = new ControlCardHolderIdentification(HexBytes.arrayCopy(value, 65, value.Length - 65));
                structureSize = ControlCardApplicationIdentification.structureSize;
            }
            else if (EquipmentType.DRIVER_CARD == cardType)
            {
                driverCardHolderIdentification = new DriverCardHolderIdentification(HexBytes.arrayCopy(value, 65, 78));
                structureSize = DriverCardHolderIdentification.structureSize;
            }
            else if (EquipmentType.WORKSHOP_CARD == cardType)
            {
                workshopCardHolderIdentification = new WorkshopCardHolderIdentification(HexBytes.arrayCopy(value, 65, value.Length - 65));
                structureSize = WorkshopCardHolderIdentification.structureSize;
            }
            else
            {
                //Error!!!
            }
        }
    }
}
