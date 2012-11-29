using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// Класс отвечает за работу с отправкой почты по расписанию.
    /// </summary>
    public class EmailScheduleTable
    {
        /// <summary>
        /// Текущий язык
        /// </summary>
        private string CurrentLanguage;//STRING_RU,STRING_RUG etc.
        /// <summary>
        /// Строка подключения
        /// </summary>
        private string connectionString;
        /// <summary>
        /// Обьект подключения к базе данных
        /// </summary>
        private SQLDB sqlDb;
         /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="connectionsStringTMP">Строка подключения к базе данных(не обязательна, так как передается подключение</param>
        /// <param name="Current_Language">Текущий язык</param>
        /// <param name="sql">Обьект подключения к базе данных</param>
        public EmailScheduleTable(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            sqlDb = sql;
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
        }
        /// <summary>
        /// Добавить отправку отчета на почту по расписанию.
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="userId">ID пользователя</param>
        /// <param name="reportId">ID отчета</param>
        /// <param name="cardId">ID карты водителя</param>
        /// <param name="period">период, за который нужно отправлять отчет в единицах, заданных в periodType.</param>
        /// <param name="periodType">единицы периода. 0-Минуты, 1-часы, 2-дни, 3-месяцы, 4-годы</param>
        /// <param name="emailAddress">адрес, на который отправлять отчет(пока только один адрес)</param>
        /// <returns>ID записи отправки отчета на почту.</returns>
        public int AddEmailSchedule(int orgId, int userId, int reportId, int cardId, int period, int periodType, string emailAddress)
        {
            return sqlDb.AddEmailSchedule(orgId, userId, reportId, cardId, periodType, period, emailAddress);
        }
        /// <summary>
        /// Внести изменения в существующую запись расписания рассылок
        /// </summary>
        /// <param name="sheduleId">ID записи отправки отчета на почту.</param>
        /// <param name="orgId">ID организации</param>
        /// <param name="userId">ID пользователя</param>
        /// <param name="reportId">ID отчета</param>
        /// <param name="cardId">ID карты водителя</param>
        /// <param name="period">период, за который нужно отправлять отчет в единицах, заданных в periodType.</param>
        /// <param name="periodType">единицы периода. 0-Минуты, 1-часы, 2-дни, 3-месяцы, 4-годы</param>
        /// <param name="emailAddress">адрес, на который отправлять отчет(пока только один адрес)</param>
        public void EditEmailSchedule(int sheduleId, int orgId, int userId, int reportId, int cardId, int period, int periodType, string emailAddress)
        {
            sqlDb.EditEmailSchedule(sheduleId, orgId, userId, reportId, cardId, period, periodType, emailAddress);
        }
        /// <summary>
        /// Возвращает список обьектов типа SingleEmailSchedule, каждый из которых представляет собой запись на очередь для отправку письма по рассылке.
        /// Необходимо именно этот метод вызывать каждое обновление кэша(или другое событие, если что-то поменяется).
        /// </summary>
        /// <returns> список обьектов типа SingleEmailSchedule.
        /// Каждый из полученных обьектов необходимо отправить по почте получателю как можно скорее.</returns>
        public List<SingleEmailSchedule> GetAllEmailShedules_ForSending()
        {
            List<SingleEmailSchedule> returnList = new List<SingleEmailSchedule>();
            List<int> allSchedules = new List<int>();
            allSchedules = sqlDb.GetAllEmailScheduleIds();
            allSchedules.Sort();
            //первое значение - Время последней отправки, второе - период.
            List<KeyValuePair<DateTime, KeyValuePair<int, int>>> allTimes = sqlDb.GetEmailScheduleTimes(allSchedules);

            DateTime nextSendDate = new DateTime();
            int periodTotalMinutes;
            int timePassedFromLastSend;
            int currentIdIndex = 0;
            foreach (KeyValuePair<DateTime, KeyValuePair<int, int>> scheduleTime in allTimes)
            {
                switch (scheduleTime.Value.Key)
                {
                    case 0://Min
                        nextSendDate = scheduleTime.Key.AddMinutes(scheduleTime.Value.Value);
                        break;
                    case 1://Hours
                        nextSendDate = scheduleTime.Key.AddHours(scheduleTime.Value.Value);
                        break;
                    case 2://Days
                        nextSendDate = scheduleTime.Key.AddDays(scheduleTime.Value.Value);
                        break;
                    case 3://Month
                        nextSendDate = scheduleTime.Key.AddMonths(scheduleTime.Value.Value);
                        break;
                    case 4://Years
                        nextSendDate = scheduleTime.Key.AddYears(scheduleTime.Value.Value);
                        break;
                }
                if (DateTime.Now >= nextSendDate)
                {
                    returnList.Add(new SingleEmailSchedule(sqlDb.GetAllEmailScheduleTable(allSchedules[currentIdIndex])));
                }
                currentIdIndex++;
            }
            return returnList;
        }
        /// <summary>
        /// Возвращает все рассылки для выбранной организации и пользователя
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="userId">ID пользователя</param>
        /// <returns>список обьектов типа SingleEmailSchedule. Использовать, например, для вывода пользователю активных подписок на рассылку.</returns>
        public List<SingleEmailSchedule> GetAllEmailShedules(int orgId, int userId)
        {
            List<SingleEmailSchedule> returnList = new List<SingleEmailSchedule>();
            List<int> allSchedules = new List<int>();
            allSchedules = sqlDb.GetAllEmailScheduleIds(orgId, userId);
            allSchedules.Sort();
            foreach (int id in allSchedules)
            {
                returnList.Add(new SingleEmailSchedule(sqlDb.GetAllEmailScheduleTable(id)));
            }
            return returnList; 
        }
        /// <summary>
        ///  Получить одно расписание отправки почты по ID
        /// </summary>
        /// <param name="sheduleId">ID расписания отправки почты</param>
        /// <returns></returns>
        public SingleEmailSchedule GetEmailShedule(int sheduleId)
        {
            return new SingleEmailSchedule(sqlDb.GetAllEmailScheduleTable(sheduleId));
        }
        /// <summary>
        /// Удалить расписание отправки почты
        /// </summary>
        /// <param name="sheduleId">ID расписания отправки почты</param>
        public void DeleteShedule(int sheduleId)
        {
            sqlDb.DeleteEmailShedule(sheduleId);
        }
        /// <summary>
        /// Устанавливает дату последней отправки сообщения в текущюю(datetime.now)
        /// </summary>
        /// <param name="sheduleId">ID расписания отправки почты</param>
        public void SetEmailSheduleLastSendDate(int sheduleId)
        {
            sqlDb.SetEmailSheduleLastSendDate(sheduleId);
        }
        /// <summary>
        /// Получает количество дней в году
        /// </summary>
        /// <param name="year">Номер года</param>
        /// <returns></returns>
        private int GetDaysInAYear(int year)
        {
            int days = 0;
            for (int i = 1; i <= 12; i++)
            {
                days += DateTime.DaysInMonth(year, i);
            }
            return days;
        }

    }

    /// <summary>
    /// Класс представляет собой одну запись в расписании рассылки почты
    /// </summary>
    public class SingleEmailSchedule
    {
        /// <summary>
        /// ID рассылки по почте(из базы)
        /// </summary>
        public int EMAIL_SCHEDULE_ID { get; set; }
        /// <summary>
        /// ID организации
        /// </summary>
        public int ORG_ID { get; set; }
        /// <summary>
        /// ID пользователя
        /// </summary>
        public int USER_ID { get; set; }
        /// <summary>
        /// ID отчета
        /// </summary>
        public int REPORT_ID { get; set; }
        /// <summary>
        /// ID карты водителя или ТС
        /// </summary>
        public int CARD_ID { get; set; }
        /// <summary>
        /// Значение периода
        /// </summary>
        public int PERIOD { get; set; }
        /// <summary>
        /// тип периода. 0-Минуты, 1-часы, 2-дни, 3-месяцы, 4-годы
        /// </summary>
        public int PERIOD_TYPE { get; set; }
        /// <summary>
        /// Дата последней отправки письма
        /// </summary>
        public DateTime LAST_SEND_DATE { get; set; }
        /// <summary>
        /// Адрес получателя
        /// </summary>
        public string EMAIL_ADDRESS { get; set; }
        /// <summary>
        /// Пустой конструктор
        /// </summary>
        public SingleEmailSchedule()
        {
        }
        /// <summary>
        /// Конструктор для инициализации обьекта данными, полученными функцией GetAllEmailScheduleTable(int scheduleId) из sqlDB
        /// </summary>
        /// <param name="gettedFromDBList">список обьектов</param>
        public SingleEmailSchedule(List<object> gettedFromDBList)
        {
            int i = 0;
            EMAIL_SCHEDULE_ID = (int)gettedFromDBList[i++];
            ORG_ID = (int)gettedFromDBList[i++];
            USER_ID = (int)gettedFromDBList[i++];
            REPORT_ID = (int)gettedFromDBList[i++];
            CARD_ID = (int)gettedFromDBList[i++];
            PERIOD = (int)gettedFromDBList[i++];
            PERIOD_TYPE = (int)gettedFromDBList[i++];
            LAST_SEND_DATE = DateTime.Parse(gettedFromDBList[i++].ToString());
            EMAIL_ADDRESS = gettedFromDBList[i++].ToString();
        }
    }
}
