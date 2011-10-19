using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CompanyActivityData
    {
        public readonly int structureSize;
        public List<CompanyActivityRecord> companyActivityRecords { get; set; }

        public CompanyActivityData()
        {
            companyActivityRecords = new List<CompanyActivityRecord>();
            structureSize = 0;
        }

        public CompanyActivityData(byte[] value, int noOfCompanyActivityRecords)
        {
            int noOfValidCompanyActivityRecords = 0;

            companyActivityRecords = new List<CompanyActivityRecord>();

            for (int i = 0; i < noOfCompanyActivityRecords; i += 1)
            {
                byte[] record = ConvertionClass.arrayCopy(value, 2 + (i * CompanyActivityRecord.structureSize), CompanyActivityRecord.structureSize);

                CompanyActivityRecord car = new CompanyActivityRecord(record);
                // only add entries with non-default values, i.e. skip empty entries
                if (car.companyActivityTime.timereal != 0)
                {
                    companyActivityRecords.Add(car);
                    noOfValidCompanyActivityRecords += 1;
                }
            }
            structureSize = 2 + noOfValidCompanyActivityRecords * CompanyActivityRecord.structureSize;
        }
    }
}
