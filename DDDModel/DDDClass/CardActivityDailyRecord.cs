using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// класс описывает активность за один день в карточке
    /// </summary>
    public class CardActivityDailyRecord : ActivityBase
    {
        public readonly int structureSize;
        /// <summary>
        /// длина предыдущей записи
        /// </summary>
        public CardActivityLengthRange activityPreviousRecordLength { get; set; }
        /// <summary>
        /// длина этой записи
        /// </summary>
        public CardActivityLengthRange activityRecordLength { get; set; }
        /// <summary>
        /// дата дня
        /// </summary>
        public TimeReal activityRecordDate { get; set; }
        /// <summary>
        /// номер дня, записанного в карточке
        /// </summary>
        public DailyPresenceCounter activityDailyPresenceCounter { get; set; }
        /// <summary>
        /// сколько проехано за этот день
        /// </summary>
        public Distance activityDayDistance { get; set; }
        //public List<ActivityChangeInfo> activityChangeInfo { get; set; }

        public CardActivityDailyRecord()
        {
            activityPreviousRecordLength = new CardActivityLengthRange();
            activityRecordLength = new CardActivityLengthRange();
            activityRecordDate = new TimeReal();
            activityDailyPresenceCounter = new DailyPresenceCounter();
            activityDayDistance = new Distance();
            activityChangeInfo = new List<ActivityChangeInfo>();
        }

        public CardActivityDailyRecord(byte[] value)
        {
            this.activityPreviousRecordLength = new CardActivityLengthRange(ConvertionClass.arrayCopy(value, 0, 2));
            this.activityRecordLength = new CardActivityLengthRange(ConvertionClass.arrayCopy(value, 2, 2));
            this.activityRecordDate = new TimeReal(ConvertionClass.arrayCopy(value, 4, 4));
            this.activityDailyPresenceCounter = new DailyPresenceCounter(ConvertionClass.arrayCopy(value, 8, 2));
            this.activityDayDistance = new Distance(ConvertionClass.arrayCopy(value, 10, 2));
            this.activityChangeInfo = new List<ActivityChangeInfo>();

            for (int length = 12; length < activityRecordLength.cardActivityLengthRange; length += ActivityChangeInfo.structureSize)
            {
                ActivityChangeInfo aci = new ActivityChangeInfo(ConvertionClass.arrayCopy(value, length, ActivityChangeInfo.structureSize));
                activityChangeInfo.Add(aci);
            }
            structureSize = 12 + activityRecordLength.cardActivityLengthRange;
        }

//--------------------------new functions TIMESPAN
        /// <summary>
        /// расчитывает сколько всего длились все активности
        /// </summary>
        /// <returns>время в TimeSpan</returns>
        public TimeSpan Get_TotalTimeSpan()
        {
            int total = 0;
            for (int i = 0; i < activityChangeInfo.Count; i++)
                total += (int)getActivityDuration(i).TotalMinutes;
            return new TimeSpan(0, total, 0);
        }
        /// <summary>
        /// расчитывает сколько всего длились все активности типа driving
        /// </summary>
        /// <returns>время в TimeSpan</returns>
        public TimeSpan Get_TotalDrivingTimeSpan()
        {
            int total = 0;
            for (int i = 0; i < activityChangeInfo.Count; i++)
            {
                if (activityChangeInfo[i].getDrivingTime() != "")
                    total += (int)getActivityDuration(i).TotalMinutes;
            }
            return new TimeSpan(0, total, 0);
        }
        /// <summary>
        /// расчитывает сколько всего длились все активности типа working
        /// </summary>
        /// <returns>время в TimeSpan</returns>
        public TimeSpan Get_TotalWorkingTimeSpan()
        {
            int total = 0;

            for (int i = 0; i < activityChangeInfo.Count; i++)
            {
                if (activityChangeInfo[i].getWorkTime() != "")
                    total += (int)getActivityDuration(i).TotalMinutes;
            }
            return new TimeSpan(0, total, 0);
        }
        /// <summary>
        /// расчитывает сколько всего длились все активности типа break
        /// </summary>
        /// <returns>время в TimeSpan</returns>
        public TimeSpan Get_TotalBreakTimeSpan()
        {
            int total = 0;

            for (int i = 0; i < activityChangeInfo.Count; i++)
            {
                if (activityChangeInfo[i].getBreakTime() != "")
                    total += (int)getActivityDuration(i).TotalMinutes;
            }
            return new TimeSpan(0, total, 0);
        }
        /// <summary>
        /// расчитывает сколько всего длились все активности типа availability
        /// </summary>
        /// <returns>время в TimeSpan</returns>
        public TimeSpan Get_TotalAvailabilityTimeSpan()
        {
            List<int> durTimes = new List<int>();
            int total = 0;

            for (int i = 0; i < activityChangeInfo.Count; i++)
            {
                if (activityChangeInfo[i].getAvailabilityTime() != "")
                    total += (int)getActivityDuration(i).TotalMinutes;
            }
            return new TimeSpan(0, total, 0);
        }      

    }
}
