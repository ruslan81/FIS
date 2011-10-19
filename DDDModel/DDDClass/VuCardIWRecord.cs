using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuCardIWRecord//129bytes
    {
        public static int structureSize = 129;

        public HolderName cardHolderName { get; set; }
        public FullCardNumber fullCardNumber { get; set; }
        public TimeReal cardExpiryDate { get; set; }
        public TimeReal cardInsertionTime { get; set; }
        public OdometerShort vehicleOdometerValueAtInsertion { get; set; }
        public CardSlotNumber cardSlotNumber { get; set; }
        public TimeReal cardWithdrawalTime { get; set; }
        public OdometerShort vehicleOdometerValueAtWithdrawal { get; set; }
        public PreviousVehicleInfo previousVehicleInfo { get; set; }
        public ManualInputFlag manualInputFlag { get; set; }

        public VuCardIWRecord()
        {
            cardHolderName = new HolderName();
            fullCardNumber = new FullCardNumber();
            cardExpiryDate = new TimeReal();
            cardInsertionTime = new TimeReal();
            vehicleOdometerValueAtInsertion = new OdometerShort();
            cardSlotNumber = new CardSlotNumber();
            cardWithdrawalTime = new TimeReal();
            vehicleOdometerValueAtWithdrawal = new OdometerShort();
            previousVehicleInfo = new PreviousVehicleInfo();
            manualInputFlag = new ManualInputFlag();
        }

        public VuCardIWRecord(byte[] value)
        {
            cardHolderName = new HolderName(ConvertionClass.arrayCopy(value, 0, 72));
            fullCardNumber = new FullCardNumber(ConvertionClass.arrayCopy(value, 72, 18));
            cardExpiryDate = new TimeReal(ConvertionClass.arrayCopy(value, 90, 4));
            cardInsertionTime = new TimeReal(ConvertionClass.arrayCopy(value, 94, 4));
            vehicleOdometerValueAtInsertion = new OdometerShort(ConvertionClass.arrayCopy(value, 98, 3));
            cardSlotNumber = new CardSlotNumber(value[101]);
            cardWithdrawalTime = new TimeReal(ConvertionClass.arrayCopy(value, 102, 4));
            vehicleOdometerValueAtWithdrawal = new OdometerShort(ConvertionClass.arrayCopy(value, 106, 3));
            previousVehicleInfo = new PreviousVehicleInfo(ConvertionClass.arrayCopy(value, 109, 19));
            manualInputFlag = new ManualInputFlag(value[128]);	
        }
    }
}
