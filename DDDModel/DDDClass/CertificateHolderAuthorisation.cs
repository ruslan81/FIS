using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CertificateHolderAuthorisation
    {
        public byte[] tachographApplicationID { get; set; }        
        public EquipmentType equipmentType { get; set; }

        public CertificateHolderAuthorisation()
        {
            tachographApplicationID = new byte[6];
            equipmentType = new EquipmentType();
        }

        public CertificateHolderAuthorisation(byte[] value)
        {
            tachographApplicationID = ConvertionClass.arrayCopy(value, 0, 6);
            equipmentType = new EquipmentType(value[6]);
        }
    }
}
