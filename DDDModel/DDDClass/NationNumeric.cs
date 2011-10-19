using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// список стран. Кодируется одним байтом, в методе ToString() расшифровывается и возвращается название.
    /// </summary>
    public class NationNumeric//1byte
    {
        public short nationNumeric { get; set; }

        public NationNumeric()
        {
            nationNumeric = 0;
        }

        public NationNumeric(byte value)
        {
            this.nationNumeric = ConvertionClass.convertIntoUnsigned1ByteInt(value);
        }

        public NationNumeric(short value)
        {
            this.nationNumeric = value;
        }

        public NationNumeric(string value)
        {
            if (value != " ")
                this.nationNumeric = Convert.ToInt16(value);
            else
                this.nationNumeric = 0;
        }

        public override string ToString()
        {
            if ((nationNumeric < 0) || (nationNumeric > 255))
            {
                return "UNKNOWN!";
            }

            switch (nationNumeric)
            {
                case 0x00:
                    // No information available (00)H
                    return "No information available";

                case 0x01:
                    // Austria (01)H
                    return "Austria";

                case 0x02:
                    // Albania (02)H
                    return "Albania";

                case 0x03:
                    // Andorra (03)H
                    return "Andorra";

                case 0x04:
                    // Armenia (04)H
                    return "Armenia";

                case 0x05:
                    // Azerbaijan (05)H
                    return "Azerbaijan";

                case 0x06:
                    // Belgium (06)H
                    return "Belgium";

                case 0x07:
                    // Bulgaria (07)H 
                    return "Bulgaria";

                case 0x08:
                    // Bosnia and Herzegovina (08)H
                    return "Bosnia and Herzegovina";

                case 0x09:
                    // Belarus (09)H
                    return "Belarus";

                case 0x0a:
                    // Switzerland (0A)H
                    return "Switzerland";

                case 0x0b:
                    // Cyprus (0B)H,
                    return "Cyprus";

                case 0x0c:
                    // Czech Republic (0C)H
                    return "Czech Republic";

                case 0x0d:
                    // Germany (0D)H
                    return "Germany";

                case 0x0e:
                    // Denmark (0E)H
                    return "Denmark";

                case 0x0f:
                    // Spain (0F)H
                    return "Spain";

                case 0x10:
                    // Estonia (10)H
                    return "Estonia";

                case 0x11:
                    // France (11)H
                    return "France";

                case 0x12:
                    // Finland (12)H
                    return "Finland";

                case 0x13:
                    // Liechtenstein (13)H
                    return "Liechtenstein";

                case 0x14:
                    // Faeroe Islands (14)H
                    return "Faeroe Islands";

                case 0x15:
                    // United Kingdom (15)H
                    return "United Kingdom";

                case 0x16:
                    // Georgia (16)H
                    return "Georgia";

                case 0x17:
                    // Greece (17)H
                    return "Greece";

                case 0x18:
                    // Hungary (18)H
                    return "Hungary";

                case 0x19:
                    // Croatia (19)H
                    return "Croatia";

                case 0x1a:
                    // Italy (1A)H
                    return "Italy";

                case 0x1b:
                    // Ireland (1B)H
                    return "Ireland";

                case 0x1c:
                    // Iceland (1C)H
                    return "Iceland";

                case 0x1d:
                    // Kazakhstan (1D)H
                    return "Kazakhstan";

                case 0x1e:
                    // Luxembourg (1E)H
                    return "Luxembourg";

                case 0x1f:
                    // Lithuania (1F)H
                    return "Lithuania";

                case 0x20:
                    // Latvia (20)H
                    return "Latvia";

                case 0x21:
                    // Malta (21)H
                    return "Malta";

                case 0x22:
                    // Monaco (22)H
                    return "Monaco";

                case 0x23:
                    // Republic of Moldova (23)H
                    return "Republic of Moldova";

                case 0x24:
                    // Macedonia (24)H
                    return "Macedonia";

                case 0x25:
                    // Norway (25)H
                    return "Norway";

                case 0x26:
                    // Netherlands (26)H
                    return "Netherlands";

                case 0x27:
                    // Portugal (27)H
                    return "Portugal";

                case 0x28:
                    // Poland (28)H
                    return "Poland";

                case 0x29:
                    // Romania (29)H
                    return "Romania";

                case 0x2a:
                    // San Marino (2A)H
                    return "San Marino";

                case 0x2b:
                    // Russian Federation (2B)H
                    return "Russian Federation";

                case 0x2c:
                    // Sweden (2C)H
                    return "Sweden";

                case 0x2d:
                    // Slovakia (2D)H
                    return "Slovakia";

                case 0x2e:
                    // Slovenia (2E)H
                    return "Slovenia";

                case 0x2f:
                    // Turkmenistan (2F)H
                    return "Turkmenistan";

                case 0x30:
                    // Turkey (30)H
                    return "Turkey";

                case 0x31:
                    // Ukraine (31)H
                    return "Ukraine";

                case 0x32:
                    // Vatican City (32)H
                    return "Vatican City";

                case 0x33:
                    // Yugoslavia (33)H
                    return "Yugoslavia";

                /*
                    0x34..0xfc:
                    // RFU (34..FC)H (зарезервировано для будущего использования) 
                */

                case 0xfd:
                    // European Community (FD)H
                    return "European Community";

                case 0xfe:
                    // Rest of Europe (FE)H
                    return "Rest of Europe";

                case 0xff:
                    // Rest of the world (FF)H
                    return "Rest of the world";

                default:
                    // RFU (34..FC)H
                    return "RFU";
            }
        }
    }
}