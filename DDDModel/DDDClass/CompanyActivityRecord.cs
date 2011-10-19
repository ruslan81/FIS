using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CompanyActivityRecord
    {
        public readonly static int structureSize = 46;

        public CompanyActivityType companyActivityType { get; set; }
        public TimeReal companyActivityTime { get; set; }
        public FullCardNumber cardNumberInformation { get; set; }
        public VehicleRegistrationIdentification vehicleRegistrationInformation { get; set; }
        public TimeReal downloadPeriodBegin { get; set; }
        public TimeReal downloadPeriodEnd { get; set; }

        public CompanyActivityRecord()
        {
            companyActivityType = new CompanyActivityType();
            companyActivityTime = new TimeReal();
            cardNumberInformation = new FullCardNumber();
            vehicleRegistrationInformation = new VehicleRegistrationIdentification();
            downloadPeriodBegin = new TimeReal();
            downloadPeriodEnd = new TimeReal();
        }

        public CompanyActivityRecord(byte[] value)
        {
            companyActivityType = new CompanyActivityType(value[0]);
            companyActivityTime = new TimeReal(ConvertionClass.arrayCopy(value, 1, 4));
            cardNumberInformation = new FullCardNumber(ConvertionClass.arrayCopy(value, 5, 18));
            vehicleRegistrationInformation = new VehicleRegistrationIdentification(ConvertionClass.arrayCopy(value, 23, 15));
            downloadPeriodBegin = new TimeReal(ConvertionClass.arrayCopy(value, 38, 4));
            downloadPeriodEnd = new TimeReal(ConvertionClass.arrayCopy(value, 42, 4));
        }
    }
}
