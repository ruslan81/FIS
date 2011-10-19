using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// описывает производителя БУ.
    /// Все кодируется числом, в перегруженном методе ToString возвращается название предприятия производителя БУ.
    /// 
    /// </summary>
    public class ManufacturerCode
    {
        public short manufacturerCode { get; set; }

        public ManufacturerCode()
        {
            manufacturerCode = 0;
        }

        public ManufacturerCode(short manufacturerCode)
        {
            this.manufacturerCode = manufacturerCode;
        }

        public ManufacturerCode(byte b)
        {
            manufacturerCode = ConvertionClass.convertIntoUnsigned1ByteInt(b);
        }
        /// <summary>
        /// перегруженная строка соответственно документации
        /// </summary>
        /// <returns>строка по документам</returns>
        public override string ToString()
        {
            // invalid manufacturer codes > 0xdf
            if (manufacturerCode > 0xdf) { return "Unknown manufacturer(or something wrong)"; }
            // valid manufacturer codes
            if (manufacturerCode == 0x00) { return "No information available"; }
            if (manufacturerCode == 0x01) { return "Reserved value"; }
            if (manufacturerCode == 0x10) { return "Actia S.A."; }
            if (manufacturerCode == 0x11) { return "Autoguard & Insurance"; }
            if (manufacturerCode == 0x12) { return "Austria Card Plastikkarten und Ausweissysteme GmbH"; }
            if (manufacturerCode == 0x20) { return "CETIS d.d."; }
            if (manufacturerCode == 0x21) { return "certSIGN"; }
            if (manufacturerCode == 0x30) { return "Sdu Identification B.V. (formerly Enschedé/Sdu B.V.)"; }
            if (manufacturerCode == 0x31) { return "Electricfil Industries"; }
            if (manufacturerCode == 0x32) { return "EFKON AG."; }
            if (manufacturerCode == 0x38) { return "Fábrica Nacional de Moneda y Timbre"; }
            if (manufacturerCode == 0x40) { return "Giesecke & Devrient GmbH"; }
            if (manufacturerCode == 0x41) { return "GEM plus"; }
            if (manufacturerCode == 0x42) { return "Grundig Car InterMedia System GmbH"; }
            if (manufacturerCode == 0x43) { return "Giesecke & Devrient GB Ltd."; }
            if (manufacturerCode == 0x48) { return "Hungarian Banknote Printing Co. Ltd."; }
            if (manufacturerCode == 0x50) { return "Imprimerie Nationale"; }
            if (manufacturerCode == 0x51) { return "Imprensa Nacional-Casa da Moeda, SA"; }
            if (manufacturerCode == 0x52) { return "InfoCamere S.C.p.A."; }
            if (manufacturerCode == 0x80) { return "OSCard"; }
            if (manufacturerCode == 0x81) { return "Sagem Orga (formerly ORGA Kartensysteme GmbH)"; }
            if (manufacturerCode == 0x82) { return "Österreichische Staatsdruckerei GmbH"; }
            if (manufacturerCode == 0x88) { return "PVT a.s."; }
            if (manufacturerCode == 0xa0) { return "Setec"; }
            if (manufacturerCode == 0xa1) { return "Continental Automotive GmbH (formerly Siemens AG - Siemens VDO Automotive Siemens Automotive)"; }
            if (manufacturerCode == 0xa2) { return "Stoneridge Electronics AB"; }
            if (manufacturerCode == 0xa3) { return "Axalto (formerly SchlumbergerSEMA)"; }
            if (manufacturerCode == 0xa4) { return "Security Printing and Systems Ltd."; }
            if (manufacturerCode == 0xa5) { return "ST Incard S.r.l."; }
            if (manufacturerCode == 0xaa) { return "Tachocontrol"; }
            if (manufacturerCode == 0xab) { return "T-Systems International GmbH"; }
            if (manufacturerCode == 0xac) { return "Trüb AG"; }
            if (manufacturerCode == 0xad) { return "Trüb Baltic AS"; }
            if (manufacturerCode == 0xae) { return "TEMPEST a.s."; }
            if (manufacturerCode == 0xaf) { return "Trueb - DEMAX PLC"; }
            return "???????????????";
        }
    }
}
