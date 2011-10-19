using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuTimeAdjustmentRecord
    {
        public static int structureSize = 98;

        public TimeReal oldTimeValue { get; set; }
        public TimeReal newTimeValue { get; set; }
        public Name workshopName { get; set; }
        public Address workshopAddress { get; set; }
        public FullCardNumber workshopCardNumber { get; set; }


        public VuTimeAdjustmentRecord()
        {
            oldTimeValue = new TimeReal();
            newTimeValue = new TimeReal();
            workshopName = new Name();
            workshopAddress = new Address();
            workshopCardNumber = new FullCardNumber();
        }

        public VuTimeAdjustmentRecord(byte[] value)
        {
            oldTimeValue = new TimeReal(ConvertionClass.arrayCopy(value, 0, 4));
            newTimeValue = new TimeReal(ConvertionClass.arrayCopy(value, 4, 4));
            workshopName = new Name(ConvertionClass.arrayCopy(value, 8, 36));
            workshopAddress = new Address(ConvertionClass.arrayCopy(value, 44, 36));
            workshopCardNumber = new FullCardNumber(ConvertionClass.arrayCopy(value, 80, 18));
        }
    }
}
