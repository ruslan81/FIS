using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// класс описывает активность Транспортного средства за один день
    /// </summary>
    public class VuActivityDailyData:ActivityBase
    {
        public int structureSize { get; set; }
        /// <summary>
        /// количество смен активности за день
        /// </summary>
        public int noOfActivityChanges { get; set; }
       // public List<ActivityChangeInfo> activityChangeInfos { get; set; }

        public VuActivityDailyData()
        {
            activityChangeInfo = new List<ActivityChangeInfo>();
        }

        public VuActivityDailyData(byte[] value, TimeReal downloadedDayDate)
        {
            activityChangeInfo = new List<ActivityChangeInfo>();

            noOfActivityChanges = ConvertionClass.convertIntoUnsigned2ByteInt(ConvertionClass.arrayCopy(value, 0, 2));
            structureSize = 2 + noOfActivityChanges * ActivityChangeInfo.structureSize;

            if (noOfActivityChanges != 0)
            {             
                for (int i = 0; i < noOfActivityChanges; i++)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, 2 + (i * ActivityChangeInfo.structureSize), ActivityChangeInfo.structureSize);
                    ActivityChangeInfo aci = new ActivityChangeInfo(record);
                    activityChangeInfo.Add(aci);                  
                }
            }
        }
        /// <summary>
        /// Получить длительность активностей за день
        /// </summary>
        /// <returns>TimeSpan</returns>
        public TimeSpan Get_TotalTimeSpan()
        {
            int total = 0;
            for (int i = 0; i < activityChangeInfo.Count; i++)
                total += (int)getActivityDuration(i).TotalMinutes;
            return new TimeSpan(0, total, 0);
        }
    }
}
