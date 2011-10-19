using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuIdentification
    {
        //size 116
        public VuManufacturerName vuManufacturerName { get; set; }
        public VuManufacturerAddress vuManufacturerAddress { get; set; }
        public VuPartNumber vuPartNumber { get; set; }
        public VuSerialNumber vuSerialNumber { get; set; }
        public VuSoftwareIdentification vuSoftwareIdentification { get; set; }
        public VuManufacturingDate vuManufacturingDate { get; set; }
        public VuApprovalNumber vuApprovalNumber { get; set; }

        public VuIdentification()
        {
            vuManufacturerName = new VuManufacturerName();
            vuManufacturerAddress = new VuManufacturerAddress();
            vuPartNumber = new VuPartNumber();
            vuSerialNumber = new VuSerialNumber();
            vuSoftwareIdentification = new VuSoftwareIdentification();
            vuManufacturingDate = new VuManufacturingDate();
            vuApprovalNumber = new VuApprovalNumber();
        }

        public VuIdentification(byte[] value)
        {
            vuManufacturerName = new VuManufacturerName(ConvertionClass.arrayCopy(value, 0, 36));
            vuManufacturerAddress = new VuManufacturerAddress(ConvertionClass.arrayCopy(value, 36, 36));
            vuPartNumber = new VuPartNumber(ConvertionClass.arrayCopy(value, 72, 16));
            vuSerialNumber = new VuSerialNumber(ConvertionClass.arrayCopy(value, 88, 8));
            vuSoftwareIdentification = new VuSoftwareIdentification(ConvertionClass.arrayCopy(value, 96, 8));
            vuManufacturingDate = new VuManufacturingDate(ConvertionClass.arrayCopy(value, 104, 4));
            vuApprovalNumber = new VuApprovalNumber(ConvertionClass.arrayCopy(value, 108, 8));
        }
    }
}
