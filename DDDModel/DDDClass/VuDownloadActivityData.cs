using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuDownloadActivityData//40 bytes
    {
        public TimeReal downloadingTime { get; set; }
        public FullCardNumber fullCardNumber { get; set; }
        public Name companyOrWorkshopName { get; set; }

        public VuDownloadActivityData()
        {
            downloadingTime = new TimeReal();
            fullCardNumber = new FullCardNumber();
            companyOrWorkshopName = new Name();
        }

        public VuDownloadActivityData(byte[] value)
        {
            downloadingTime = new TimeReal(ConvertionClass.arrayCopy(value, 0, 4));
            fullCardNumber = new FullCardNumber(ConvertionClass.arrayCopy(value, 4, 18));
            companyOrWorkshopName = new Name(ConvertionClass.arrayCopy(value, 22, 36));
        }
    }
}
