using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// описывает с какой даты и по какую имеется информация на карте
    /// </summary>
    public class VuDownloadablePeriod//8bytes
    {
        /// <summary>
        /// с какого числа и времени имеется информация на карте
        /// </summary>
        public TimeReal minDownloadableTime { get; set; }
        /// <summary>
        /// по какое число и время имеется информация на карте
        /// </summary>
        public TimeReal maxDownloadableTime { get; set; }

        public VuDownloadablePeriod()
        {
            minDownloadableTime = new TimeReal();
            maxDownloadableTime = new TimeReal();
        }

        public VuDownloadablePeriod(byte[] tmp)
        {
            minDownloadableTime = new TimeReal(ConvertionClass.arrayCopy(tmp, 0, 4));
            maxDownloadableTime = new TimeReal(ConvertionClass.arrayCopy(tmp, 4, 4));
        }
    }
}
