using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// все активности водителя
    /// </summary>
    public class CardDriverActivity
    {
        public readonly int structureSize;
        /// <summary>
        /// номер самой старой активности
        /// </summary>
        public int activityPointerOldestDayRecord { get; set; }
        /// <summary>
        /// номер самой новой активности
        /// </summary>
        public int activityPointerNewestRecord    { get; set; }
        /// <summary>
        /// лист CardActivityDailyRecord(этот тип описывает активности за один день)
        /// </summary>
        public List<CardActivityDailyRecord> activityDailyRecords { get; set; }

        public CardDriverActivity()
        {
            activityPointerOldestDayRecord = 0;
            activityPointerNewestRecord = 0;
            activityDailyRecords = new List<CardActivityDailyRecord>();
        }

        public CardDriverActivity(byte[] value, int activityStructureLength)
        {
            activityDailyRecords = new List<CardActivityDailyRecord>();

            activityPointerOldestDayRecord = ConvertionClass.convertIntoUnsigned2ByteInt(ConvertionClass.arrayCopy(value, 0, 2)); // = first CardActivityDailyRecord
            activityPointerNewestRecord = ConvertionClass.convertIntoUnsigned2ByteInt(ConvertionClass.arrayCopy(value, 2, 2)); // = last CardActivityDailyRecord           
            byte[] records = new byte[activityStructureLength];
            int lengthToEnd = records.Length - activityPointerOldestDayRecord;
            Array.Copy(value, 4 + activityPointerOldestDayRecord, records, 0, lengthToEnd);

            if (activityPointerOldestDayRecord != 0)
            {
                Array.Copy(value, 4, records, lengthToEnd, activityPointerOldestDayRecord);
            }
            int activityPointerLastRecordOffset;

            if (activityPointerNewestRecord >= activityPointerOldestDayRecord)
            {
                activityPointerLastRecordOffset = activityPointerNewestRecord - activityPointerOldestDayRecord;
            }
            else
            {
                activityPointerLastRecordOffset = records.Length - activityPointerOldestDayRecord + activityPointerNewestRecord;
            }

            int cardActivityDailyRecordsOffset = 0;

            int cadrActivityPreviousRecordLength = 0;
            int cadrActivityRecordLength = 0;

            int cadrIntegrityCheckActivityPreviousRecordLength = 0;

            while (cardActivityDailyRecordsOffset <= activityPointerLastRecordOffset)
            {
                int cadrLength = ConvertionClass.convertIntoUnsigned2ByteInt(ConvertionClass.arrayCopy(records, cardActivityDailyRecordsOffset + 2, 2));

                if (cadrLength == 0)
                {
                    break;
                }

                CardActivityDailyRecord cadr = new CardActivityDailyRecord(ConvertionClass.arrayCopy(records, cardActivityDailyRecordsOffset, cadrLength));

                cadrActivityPreviousRecordLength = cadr.activityPreviousRecordLength.cardActivityLengthRange;
                cadrActivityRecordLength = cadr.activityRecordLength.cardActivityLengthRange;

                cardActivityDailyRecordsOffset += cadrActivityRecordLength; // next CardActivityDailyRecord

                cadrIntegrityCheckActivityPreviousRecordLength = cadrActivityRecordLength; // save record length for integrity check

                activityDailyRecords.Add(cadr);
            }

            structureSize = 2 + 2 + cardActivityDailyRecordsOffset;
        }
        /// <summary>
        /// получаем все время, сколько длятся все активности
        /// </summary>
        /// <returns>длительность всех активностей типа TimeSpan</returns>
        public TimeSpan GetTotalTime()
        {
            TimeSpan totals = new TimeSpan();
            foreach (CardActivityDailyRecord record in activityDailyRecords)
            {
                totals = totals.Add(record.Get_TotalTimeSpan());
            }
            return totals;
        }
        /// <summary>
        /// получаем все время, сколько длятся активности типа Driving
        /// </summary>
        /// <returns>длительность всех активностей типа TimeSpan</returns>
        public TimeSpan GetTotalDrivingTime()
        {
            TimeSpan totals = new TimeSpan();
            foreach (CardActivityDailyRecord record in activityDailyRecords)
            {
                totals.Add(record.Get_TotalDrivingTimeSpan());
            }
            return totals;
        }
        /// <summary>
        /// получаем все время, сколько длятся активности типа Working
        /// </summary>
        /// <returns>длительность всех активностей типа TimeSpan</returns>
        public TimeSpan GetTotalWorkingTime()
        {
            TimeSpan totals = new TimeSpan();
            foreach (CardActivityDailyRecord record in activityDailyRecords)
            {
                totals.Add(record.Get_TotalWorkingTimeSpan());
            }
            return totals;
        }
        /// <summary>
        /// получаем все время, сколько длятся активности типа Availability
        /// </summary>
        /// <returns>длительность всех активностей типа TimeSpan</returns>
        public TimeSpan GetTotalAvailabilityTime()
        {
            TimeSpan totals = new TimeSpan();
            foreach (CardActivityDailyRecord record in activityDailyRecords)
            {
                totals.Add(record.Get_TotalAvailabilityTimeSpan());
            }
            return totals;
        }
        /// <summary>
        /// получаем все время, сколько длятся активности типа Break
        /// </summary>
        /// <returns>длительность всех активностей типа TimeSpan</returns>
        public TimeSpan GetTotalBreakTime()
        {
            TimeSpan totals = new TimeSpan();
            foreach (CardActivityDailyRecord record in activityDailyRecords)
            {
                totals.Add(record.Get_TotalBreakTimeSpan());
            }
            return totals;
        }      
        /// <summary>
        /// Получает дистанцию, пройденную транспортным средством под управлением водителя за все время.
        /// </summary>
        /// <returns>дистанция в километрах</returns>
        public int GetTotalDistance()
        {
            int dist = 0;
            foreach (CardActivityDailyRecord record in activityDailyRecords)
            {
                dist += record.activityDayDistance.distance;
            }
            return dist;
        }
        /// <summary>
        /// нигде не используется, незнаю что это такое, какой-то хвост. Думаю можно удалить.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        private static string addTwoTimes(string first, string second, string separator)//separator == ":"
        {
            DateTime datetime = new DateTime();
            int hours = 0;
            string returnString;

            try
            {
                string[] split1 = first.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                string[] split2 = second.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

                datetime = datetime.AddMinutes(Convert.ToDouble(split1[1]));
                datetime = datetime.AddMinutes(Convert.ToDouble(split2[1]));
                hours += Convert.ToInt32(split1[0]) + Convert.ToInt32(split2[0]) + datetime.Hour;
                returnString = String.Format("{0:00}", hours) + separator + datetime.ToString("mm");
            }
            catch
            {
                returnString = "Ошибка";
            }
            return returnString;
        }        

        //----------------------------new functions TIMESPAN
        /// <summary>
        /// Дублирование функции GetTotalTime
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTotalTimeSpan()
        {
            TimeSpan totals = new TimeSpan();
            foreach (CardActivityDailyRecord record in activityDailyRecords)
            {
                totals = totals.Add(record.Get_TotalTimeSpan());
            }
            return totals;
        }
        /// <summary>
        /// Дублирование функции GetTotalDrivingTime
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTotalDrivingTimeSpan()
        {
            TimeSpan totals = new TimeSpan();
            foreach (CardActivityDailyRecord record in activityDailyRecords)
            {
                totals = totals.Add(record.Get_TotalDrivingTimeSpan());
            }
            return totals;

        }
        /// <summary>
        /// Дублирование функции GetTotalWorkingTime
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTotalWorkingTimeSpan()
        {
            TimeSpan totals = new TimeSpan();
            foreach (CardActivityDailyRecord record in activityDailyRecords)
            {
                totals = totals.Add(record.Get_TotalWorkingTimeSpan());
            }
            return totals;
        }
        /// <summary>
        /// Дублирование функции GetTotalAvailabilityTime
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTotalAvailabilityTimeSpan()
        {
            TimeSpan totals = new TimeSpan();
            foreach (CardActivityDailyRecord record in activityDailyRecords)
            {
                totals = totals.Add(record.Get_TotalAvailabilityTimeSpan());
            }
            return totals;
        }
        /// <summary>
        /// Дублирование функции  GetTotalBreakTime
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTotalBreakTimeSpan()
        {
            TimeSpan totals = new TimeSpan();
            foreach (CardActivityDailyRecord record in activityDailyRecords)
            {
                totals = totals.Add(record.Get_TotalBreakTimeSpan());
            }
            return totals;
        }

    }
}
