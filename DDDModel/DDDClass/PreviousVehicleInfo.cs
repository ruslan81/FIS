using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class PreviousVehicleInfo
    {
        public VehicleRegistrationIdentification vehicleRegistrationIdentification { get; set; }
        public TimeReal cardWithdrawalTime { get; set; }

        public PreviousVehicleInfo()
        {
            vehicleRegistrationIdentification = new VehicleRegistrationIdentification();
            cardWithdrawalTime = new TimeReal();
        }

        public PreviousVehicleInfo(byte[] value)
        {
            vehicleRegistrationIdentification = new VehicleRegistrationIdentification(ConvertionClass.arrayCopy(value, 0, 15));
            cardWithdrawalTime = new TimeReal(ConvertionClass.arrayCopy(value, 15, 4));
        }
    }
}
