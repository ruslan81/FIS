using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class VuApprovalNumber//8byte
    {
        public string vuApprovalNumber { get; set; }

        public VuApprovalNumber()
        {
            vuApprovalNumber = new string("".ToCharArray());
        }

        public VuApprovalNumber(byte[] value)
        {
            vuApprovalNumber = ConvertionClass.convertIntoString(ConvertionClass.arrayCopy(value, 0, 8));
        }

        public VuApprovalNumber(string value)
        {
            vuApprovalNumber = value;
        }

        public override string ToString()
        {
            return this.vuApprovalNumber;
        }
    }
}
