using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace DDDClass
{
    /// <summary>
    /// описывает время. Время в карте хранится как количество минут с 00:00 1.1.1970.
    /// Так и тут хранится. Класс имеет методы для перевода такого представления времени в DateTime и строки
    /// </summary>
    public class TimeReal//4 байта
    {
        public long timereal { get; set; }

        public TimeReal()
        {
            timereal = 0;
        }

        public TimeReal(long i)
        {
            timereal = i;
        }

        public TimeReal(byte[] value)
        {
            timereal = ConvertionClass.convertIntoUnsigned4ByteInt(value);
        }

        public TimeReal(string value)
        {
            if (value == " ")
                timereal = 0;
            else
                timereal = Convert.ToInt64(value) ;
        }
        /// <summary>
        /// Переводит время из внутреннего типа в DateTime
        /// </summary>
        /// <returns>Возвращает DateTime</returns>
        public DateTime getTimeRealDate()
        {           

            DateTime data = new DateTime(1970, 1, 1, 0, 0, 0); //по умолчанию 01.01.01  00:00
            // Время в тахографе считается в секундах начиная с 00:00 1 января 1970 года                      
            data = data.AddSeconds((double)timereal);

            return data;
        }
        /// <summary>
        /// Возвращает только время в виде строки в формате "HH:mm"
        /// </summary>
        /// <returns>строка с временем в формате "HH:mm"</returns>
        public string GetTime()
        {
            string dateString;
            string format;
            DateTime dateTime = new DateTime();

            dateTime = getTimeRealDate();

            format = "HH:mm";
            dateString = dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);

            return dateString;
        }
        /// <summary>
        /// Возвращает только дату в виде строки в формате "dd/MM/yyyy"
        /// </summary>
        /// <returns>строка с датой в формате "dd/MM/yyyy"</returns>
        public string GetDate()
        {
            string dateString;
            string format;
            DateTime dateTime = new DateTime();

            dateTime = getTimeRealDate();

            format = "dd/MM/yyyy";
            dateString = dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);

            return dateString;
        }
        /// <summary>
        /// перегруженный метод ToString()
        /// </summary>
        /// <returns>строка с датой и временем в формате "dd/MM/yyyy HH:mm"</returns>
        public override string ToString()
        {
            string dateString;
            string format;
            DateTime dateTime = new DateTime();

            dateTime = getTimeRealDate();

            format = "dd/MM/yyyy HH:mm";
            dateString = dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);

            return dateString;
        }
    }
}
