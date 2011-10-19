using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardVehicleRecord
    {
        public readonly static int structureSize = 31;

        public OdometerShort vehicleOdometerBegin { get; set; }
        public OdometerShort vehicleOdometerEnd { get; set; }
        public TimeReal vehicleFirstUse { get; set; }
        public TimeReal vehicleLastUse { get; set; }
        public VehicleRegistrationIdentification vehicleRegistration { get; set; }
        public VuDataBlockCounter vuDataBlockCounter { get; set; }

        public CardVehicleRecord()
        {
            vehicleOdometerBegin = new OdometerShort();
            vehicleOdometerEnd = new OdometerShort();
            vehicleFirstUse = new TimeReal();
            vehicleLastUse = new TimeReal();
            vehicleRegistration = new VehicleRegistrationIdentification();
            vuDataBlockCounter = new VuDataBlockCounter();
        }

        public CardVehicleRecord(byte[] value)
        {
            vehicleOdometerBegin = new OdometerShort(ConvertionClass.arrayCopy(value, 0, 3));
            vehicleOdometerEnd = new OdometerShort(ConvertionClass.arrayCopy(value, 3, 3));
            vehicleFirstUse = new TimeReal(ConvertionClass.arrayCopy(value, 6, 4));
            vehicleLastUse = new TimeReal(ConvertionClass.arrayCopy(value, 10, 4));
            vehicleRegistration = new VehicleRegistrationIdentification(ConvertionClass.arrayCopy(value, 14, 15));
            vuDataBlockCounter = new VuDataBlockCounter(ConvertionClass.arrayCopy(value, 29, 2));
        }
    }
}
