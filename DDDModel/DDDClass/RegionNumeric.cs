using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// кодирует регион. Регионы не все кажется. Добавить при необходимости!
    /// </summary>
    public class RegionNumeric//1byte
    {
        public byte regionNumeric { get; set; }

        public RegionNumeric()
        {
            regionNumeric = 0;
        }
        
        public RegionNumeric(byte regionNumeric)
        {
            this.regionNumeric = regionNumeric;
        }

        public RegionNumeric(string regionNumeric)
        {
            this.regionNumeric = Convert.ToByte(regionNumeric);
        }

        public override string ToString()
        {
            switch (regionNumeric)
            {
                case 0x00:
                    return "No information available";
                case 0x01:
                    return "Andalucía";
                case 0x02:
                    return "Aragón";
                case 0x03:
                    return "Asturias";
                case 0x04:
                    return "Cantabria";
                case 0x05:
                    return "Cataluña";
                case 0x06:
                    return "Castilla-León";
                case 0x07:
                    return "Castilla-La-Mancha";
                case 0x08:
                    return "Valencia";
                case 0x09:
                    return "Extremadura";
                case 0x0a:
                    return "Galicia";
                case 0x0b:
                    return "Baleares";
                case 0x0c:
                    return "Canarias";
                case 0x0d:
                    return "La Rioja";
                case 0x0e:
                    return "Madrid";
                case 0x0f:
                    return "Murcia";
                case 0x10:
                    return "Navarra";
                case 0x011:
                    return "País Vasco";
                default:
                    return "Invalid";
            }
        }
    }
}
