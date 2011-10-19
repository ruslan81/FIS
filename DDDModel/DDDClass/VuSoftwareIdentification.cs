using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuSoftwareIdentification//8bytes
    {
        public VuSoftwareVersion vuSoftwareVersion { get; set; }
        public VuSoftInstallationDate vuSoftInstallationDate { get; set; }

        public VuSoftwareIdentification()
        {
            vuSoftwareVersion = new VuSoftwareVersion();
            vuSoftInstallationDate = new VuSoftInstallationDate();
        }

        public VuSoftwareIdentification(byte[] value)
        {
            vuSoftwareVersion = new VuSoftwareVersion(ConvertionClass.arrayCopy(value, 0, 4));
            vuSoftInstallationDate = new VuSoftInstallationDate(ConvertionClass.arrayCopy(value, 4, 4));
        }

    }
}
