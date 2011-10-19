using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// Класс предок для активностей водителя или ТС
    /// </summary>
    public abstract class ActivityBase
    {
        /// <summary>
        /// список всех активностей
        /// </summary>
        public List<ActivityChangeInfo> activityChangeInfo { get; set; }
        /// <summary>
        /// Получает длительность некоторой выбранной активности
        /// </summary>
        /// <param name="index">номер активности в activityChangeInfo</param>
        /// <returns>длительность активности</returns>
        public TimeSpan getActivityDuration(int index)
        {
            TimeSpan activityDuration = new TimeSpan();
            if (activityChangeInfo[index] != null)
            {
                if (activityChangeInfo.Count >= (index + 1) && (index + 1) < activityChangeInfo.Count)
                    activityDuration = new TimeSpan(0, activityChangeInfo[index + 1].time - activityChangeInfo[index].time, 0);
                else
                    // if ((index + 1) < activityChangeInfo[index + 1]))
                    activityDuration = new TimeSpan(0, 1440 - activityChangeInfo[index].time, 0);
            }
            else
                throw new Exception("Ошибка в разборе длительности активностей");

            //if (activityChangeInfo.Count - 1 == index)


            return activityDuration;
        }
        /// <summary>
        ///  Получает время начала 
        ///  некоторой выбранной активности
        /// </summary>
        /// <param name="index">номер активности в activityChangeInfo</param>
        /// <returns>время начала выбранной активности</returns>
        public TimeSpan getActivityStartTime(int index)
        {
            TimeSpan returnTime = new TimeSpan();
            if (activityChangeInfo.Count > 0)
                returnTime = new TimeSpan(0, activityChangeInfo[index].getActivityTimeMinutes(), 0);
            return returnTime;
        }
        /// <summary>
        ///  Получает время окончания 
        ///  некоторой выбранной активности
        /// </summary>
        /// <param name="index">номер активности в activityChangeInfo</param>
        /// <returns>время окончания выбранной активности</returns>
        public TimeSpan getActivityEndTime(int index)
        {
            TimeSpan activityEndTime = new TimeSpan();
            if (activityChangeInfo[index] != null)
            {
                if (activityChangeInfo.Count >= (index + 1) && (index + 1) < activityChangeInfo.Count)
                    activityEndTime = new TimeSpan(0, activityChangeInfo[index + 1].time, 0);
                else
                    activityEndTime = new TimeSpan(0, 1440, 0);
            }
            else
                throw new Exception("Ошибка в разборе длительности активностей");
            return activityEndTime;
        }
    }
}
