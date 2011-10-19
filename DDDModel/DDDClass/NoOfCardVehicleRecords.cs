using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class NoOfCardVehicleRecords//2bytes
    {

        public int noOfCardVehicleRecords { get; set; }

        public NoOfCardVehicleRecords()
        {
            noOfCardVehicleRecords = 0;
        }

        public NoOfCardVehicleRecords(byte[] value)
        {
            this.noOfCardVehicleRecords = ConvertionClass.convertIntoUnsigned2ByteInt(value);
        }

        public NoOfCardVehicleRecords(string value)
        {
            this.noOfCardVehicleRecords = Convert.ToInt32(value);
        }

        public override string ToString()
        {
            return noOfCardVehicleRecords.ToString();
        }
    }
}
