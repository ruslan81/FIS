using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// Описание последнего считываения информации с карты и количества настроек БУ после этого
    /// </summary>
    public class EF_Card_Download
    {
        public int cardType;
        public LastCardDownload lastCardDownload { get; set; }
        public NoOfCalibrationsSinceDownload noOfCalibrationsSinceDownload { get; set; }

        public EF_Card_Download()
        { }

        public EF_Card_Download(byte[] value, int cardType)
        {
            this.cardType = cardType;

            switch (cardType)
            {
                case 1 ://DRIVER_CARD
                    lastCardDownload = new LastCardDownload(value);
                    break;

                case 2 ://WORKSHOP_CARD
                    noOfCalibrationsSinceDownload = new NoOfCalibrationsSinceDownload(value);
                    break;

                default:
                    break;
            }
        }
    }
}
