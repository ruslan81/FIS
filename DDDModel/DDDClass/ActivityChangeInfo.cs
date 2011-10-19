using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace DDDClass
{
    /// <summary>
    /// структура из 2х байт. описывает одну активность ТС или водителя
    /// </summary>
    public class ActivityChangeInfo//2byte
    {
        public static readonly int structureSize = 2;

        public string value { get; set; }
        public bool slot;
        public bool drivingStatus;
        private bool activityStatus;
        public bool cardStatus;
        public byte activity;
        public int time;

        private static readonly byte    SLOT_MASK = (byte)0x80;
        public static readonly bool     DRIVER = false;
        public static readonly bool     CO_DRIVER = true;
        private static readonly byte    DRIVINGSTATUS_MASK = (byte)0x40;
        private static readonly byte    ACTIVITYSTATUS_MASK = (byte)0x40;
        public static readonly bool     SINGLE = false;
        public static readonly bool     UNKNOWN = false;
        public static readonly bool     CREW = true;
        public static readonly bool     KNOWN = true;
        private static readonly byte    CARDSTATUS_MASK = (byte)0x20;
        public static readonly bool     INSERTED = false;
        public static readonly bool     NOT_INSERTED = true;
        private static readonly byte    ACTIVITY_MASK = (byte)0x18;
       /* public static readonly byte     BREAK = 0;
        public static readonly byte     AVAILABILITY = 1;
        public static readonly byte     WORK = 2;
        public static readonly byte     DRIVING = 3;*/
        private static readonly byte    TIME_UPPERBYTE_MASK = (byte)0x07;
        private static readonly byte    TIME_LOWERBYTE_MASK = (byte)0xff;

        public ActivityChangeInfo()
        { }

        public ActivityChangeInfo(byte[] value)
        {
            this.value = value[0]+"&&"+value[1];

            slot = ((value[0] & SLOT_MASK) == SLOT_MASK);
            drivingStatus = ((value[0] & DRIVINGSTATUS_MASK) == DRIVINGSTATUS_MASK);
            activityStatus = ((value[0] & ACTIVITYSTATUS_MASK) == ACTIVITYSTATUS_MASK);
            cardStatus = ((value[0] & CARDSTATUS_MASK) == CARDSTATUS_MASK);

            activity = (byte)(value[0] & ACTIVITY_MASK);
            activity = (byte)(activity >> 3);

            byte tmp = (byte)(value[0] & TIME_UPPERBYTE_MASK);
            time = ConvertionClass.convertIntoUnsigned2ByteInt(new byte[] { tmp, (byte)(value[1] & TIME_LOWERBYTE_MASK) });
        }

        public ActivityChangeInfo(string valueString)
        {
            byte[] byteArray = new byte[2];
            string[] strArray;

            strArray = valueString.Split(new string[] {"&&"}, StringSplitOptions.RemoveEmptyEntries);
            byteArray[0] = Convert.ToByte(strArray[0]);
            byteArray[1] = Convert.ToByte(strArray[1]);

            this.value = valueString;

            slot = ((byteArray[0] & SLOT_MASK) == SLOT_MASK);
            drivingStatus = ((byteArray[0] & DRIVINGSTATUS_MASK) == DRIVINGSTATUS_MASK);
            activityStatus = ((byteArray[0] & ACTIVITYSTATUS_MASK) == ACTIVITYSTATUS_MASK);
            cardStatus = ((byteArray[0] & CARDSTATUS_MASK) == CARDSTATUS_MASK);

            activity = (byte)(byteArray[0] & ACTIVITY_MASK);
            activity = (byte)(activity >> 3);

            byte tmp = (byte)(byteArray[0] & TIME_UPPERBYTE_MASK);
            time = ConvertionClass.convertIntoUnsigned2ByteInt(new byte[] { tmp, (byte)(byteArray[1] & TIME_LOWERBYTE_MASK) });
        }
        /// <summary>
        /// Возвращает название активности
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            switch (activity)
            {
                case 0 : // Break
                    return "break";

                case 1 : //Availability
                    return "availability";

                case 2 : //Work
                    return "work";

                case 3 : //driving
                    return "driving";

                default: //???
                    return "????";
            }
        }
        /// <summary>
        /// возращает длительность активности, если тип активности вождение, иначе пустую строку
        /// </summary>
        /// <returns>строка, с длительностью активности</returns>
        public string getDrivingTime()
        {
            if (activity == 3)
                return getActivityTime();
            else
                return "";
        }
        /// <summary>
        /// возращает длительность активности, если тип активности отдых, иначе пустую строку
        /// </summary>
        /// <returns>строка, с длительностью активности</returns>
        public string getBreakTime()
        {
            if (activity == 0)
                return getActivityTime();
            else
                return "";
        }
        /// <summary>
        /// возращает длительность активности, если тип активности available, иначе пустую строку
        /// </summary>
        /// <returns>строка, с длительностью активности</returns>
        public string getAvailabilityTime()
        {
            if (activity == 1)
                return getActivityTime();
            else
                return "";
        }
        /// <summary>
        /// возращает длительность активности, если тип активности work, иначе пустую строку
        /// </summary>
        /// <returns>строка, с длительностью активности</returns>
        public string getWorkTime()
        {
            if (activity == 2)
                return getActivityTime();
            else
                return "";
        }
        /// <summary>
        /// Возвращает статус слота для карты, карта там водителя или помошника водителя(это тоже записывается у водителя на карте)
        /// </summary>
        /// <returns>строка driver или co-driver</returns>
        public string getSlotStatus()
        {
            if (slot == false)
            {
                return "driver";
            }
            else
            {
                return "co-driver";
            }
        }
        /// <summary>
        /// возвращает строку single или crew. То есть едет ли водитель один или с напарником.
        /// </summary>
        /// <returns>строка single, crew </returns>
        public string getDrivingStatus()
        {
            if (drivingStatus == false)
            {
                return "single";
            }
            else
            {
                return "crew";
            }
        }
        /// <summary>
        /// показывает известен ли статус активности
        /// </summary>
        /// <returns>строка unknown или known</returns>
        public string getActivityStatus()
        {
            if (activityStatus == false)
            {
                return "unknown";
            }
            else
            {
                return "known";
            }
        }
        /// <summary>
        /// показывает вставлена ли карточка вообще
        /// </summary>
        /// <returns>значение inserted или not inserted</returns>
        public string getCardStatus()
        {
            if (cardStatus == false)
            {
                return "inserted";
            }
            else
            {
                return "not inserted";
            }
        }
        /// <summary>
        /// Возвращает время активности
        /// </summary>
        /// <returns>строка в формате HH:mm</returns>
        public string getActivityTime()
        {
            string dateString;
            string format;
            DateTime dateTime = new DateTime();

            dateTime = dateTime.AddMinutes(time); 

            format = "HH:mm";
            dateString = dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);

            return dateString;
        }
        /// <summary>
        /// возвращает время активности просто в минутах.
        /// </summary>
        /// <returns>кол-во минут</returns>
        public int getActivityTimeMinutes()
        {
            return time;
        }


    }
}
