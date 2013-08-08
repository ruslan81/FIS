using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLFUnit
{
    /// <summary>
    /// Класс описывает время в PLF файле.
    /// </summary>
    public class PLFSystemTime
    {
        /// <summary>
        /// Переменная времени. хранится как строка.
        /// </summary>
        public string systemTime { get; set; }
        /// <summary>
        /// конструктор с параметром
        /// </summary>
        /// <param name="value">тут любой System_Time параметр</param>
        public PLFSystemTime(string value)
        {
            systemTime = value;
        }
        /// <summary>
        /// Конструктор по-умолчанию
        /// </summary>
        public PLFSystemTime()
        {
            systemTime = "";
        }
        /// <summary>
        /// Парсит любой переданный параметр System_Time, незнаю почему не статический метод, но видимо так надо было.
        /// </summary>
        /// <param name="value">параметр System_Time</param>
        /// <returns>тип DateTime</returns>
        public DateTime GetSystemTime (string value)
        {
            if (value.Equals(" ")) {
                return new DateTime();
            }
            string[] splitString = value.Split(new string[] { ":", " "}, StringSplitOptions.RemoveEmptyEntries);
            if (splitString.Length != 6)
                throw new Exception("Ошибка в формате PLF SYstem Date");
            int year = Convert.ToInt32(splitString[0]) + 2000;
            int month = Convert.ToInt32(splitString[1]);
            int day = Convert.ToInt32(splitString[2]);
            int hour = Convert.ToInt32(splitString[3]);
            int minute = Convert.ToInt32(splitString[4]);
            int second = Convert.ToInt32(splitString[5]);
            DateTime systemDate = new DateTime(year, month, day, hour,minute, second, DateTimeKind.Local);

            return systemDate;
        }
        /// <summary>
        /// Парсит содержимое переменной systemTime в тип DateTime и возвращает его.
        /// </summary>
        /// <returns>тип DateTime</returns>
        public DateTime GetSystemTime()
        {
            return GetSystemTime(systemTime);
        }
        /// <summary>
        /// Незнаю что это такое, нигде не используется
        /// </summary>
        /// <param name="timeStep">?</param>
        /// <returns>?</returns>
        public string GetRoundedSystemTime(int timeStep)
        {
            if (timeStep == 0)
                return systemTime;
            string str = systemTime.Remove(systemTime.Length - timeStep);
            if (timeStep == 2)
                str += "00";
            if (timeStep == 1)
                str += "0";
            return str;
        }
        /// <summary>
        /// Перегруженная функция ToString()
        /// </summary>
        /// <returns>Строковое представление для времени</returns>
        public override string ToString()
        {
            DateTime sysTime = new DateTime();
            sysTime = GetSystemTime(systemTime);
            string retStr = sysTime.ToShortDateString() + " " + sysTime.ToLongTimeString();
            return retStr;
        }
    }
}
