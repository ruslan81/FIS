using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuCompanyLocksRecord//98 bytes
    {
        public static int structureSize = 98;

        public TimeReal lockInTime { get; set; }
        public TimeReal lockOutTime { get; set; }
        public Name companyName { get; set; }
        public Address companyAddress { get; set; }
        public FullCardNumber companyCardNumber { get; set; }

        public VuCompanyLocksRecord()
        {
            lockInTime = new TimeReal();
            lockOutTime = new TimeReal();
            companyName = new Name();
            companyAddress = new Address();
            companyCardNumber = new FullCardNumber();
        }

        public VuCompanyLocksRecord(byte[] value)
        {
            lockInTime = new TimeReal(ConvertionClass.arrayCopy(value, 0, 4));
            lockOutTime = new TimeReal(ConvertionClass.arrayCopy(value, 4, 4));
            companyName = new Name(ConvertionClass.arrayCopy(value, 8, 36));
            companyAddress = new Address(ConvertionClass.arrayCopy(value, 44, 36));
            companyCardNumber = new FullCardNumber(ConvertionClass.arrayCopy(value, 80, 18));
        }
    }
}
