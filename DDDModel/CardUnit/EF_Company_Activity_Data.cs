using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// Описывает активность компании
    /// </summary>
    public class EF_Company_Activity_Data
    {
        private readonly int structureSize;

        public CompanyActivityData companyActivityData { get; set; }

        public EF_Company_Activity_Data()
        { }

        public EF_Company_Activity_Data(byte[] value, int noOfCompanyActivityRecords)
        {
            companyActivityData = new CompanyActivityData(value, noOfCompanyActivityRecords);
            structureSize = companyActivityData.structureSize;
        }
    }
}
