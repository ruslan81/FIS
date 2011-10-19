using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class Datef
    {
        public string year { get; set; }
        public string month { get; set; }
        public string day { get; set; }

        public Datef()
        {
            year = "0000";
            month = "00";
            day = "00";
        }
      
        public Datef(byte[] value)
        {
            year = ConvertionClass.ConvertBytesToBCDString(false, ConvertionClass.arrayCopy(value, 0, 2));
            month = ConvertionClass.ConvertBytesToBCDString(false, value[2]);
            day = ConvertionClass.ConvertBytesToBCDString(false, value[3]);
        }

        public void Set_Day(string value)
        {
            day = value;
        }

        public void Set_Month(string value)
        {
            month = value;
        }

        public void Set_Year(string value)
        {            
            year = value;
        }        

        public DateTime GetDateTime()
        {
            int d = Convert.ToInt32(day);
            int m = Convert.ToInt32(month);
            int y = Convert.ToInt32(year);
            DateTime returnValue = new DateTime(y, m, d);
            return returnValue;
        }

        public override string ToString()
        {
            string returnDate;
            returnDate = GetDateTime().ToLongDateString();
            return returnDate;
        }
    }
}
