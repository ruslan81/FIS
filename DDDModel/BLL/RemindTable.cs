using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    public class RemindTable
    {
        /// <summary>
        /// Текущий язык
        /// </summary>
        private string CurrentLanguage;//STRING_RU,STRING_ENG etc.
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
        public RemindTable(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            sqlDb = sql;
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
        }

        /// <summary>
        /// Создает новое напоминание
        /// </summary>
        /// <param name="remindActive">Активность</param>
        /// <param name="userId">ID пользователя</param>
        /// <param name="orgId">ID организации</param>
        /// <param name="sourceId">ID водителя</param>
        /// <param name="period">Периодичность</param>
        /// <param name="lastDate">Дата последнего напоминания</param>
        /// <param name="remindType">Тип напоминания</param>
        public void CreateNewRemind(int orgId, bool remindActive, int userId, int sourceType, int sourceId, int period, DateTime lastDate, int remindType)
        {
            sqlDb.CreateNewRemind(orgId, remindActive, userId, sourceType, sourceId, period, lastDate, remindType);
        }
        /// <summary>
        /// Редактирует напоминание
        /// </summary>
        /// <param name="remindId">ID напоминания</param>
        /// <param name="remindActive">Активность</param>
        /// <param name="userId">ID пользователя</param>
        /// <param name="sourceId">ID водителя</param>
        /// <param name="period">Периодичность</param>
        /// <param name="lastDate">Дата последнего напоминания</param>
        /// <param name="remindType">Тип напоминания</param>
        public void UpdateRemind(int remindId, bool remindActive, int userId, int sourceType, int sourceId, int period, DateTime lastDate, int remindType)
        {
            sqlDb.UpdateRemind(remindId, remindActive, userId, sourceType, sourceId, period, lastDate, remindType);
        }
        /// <summary>
        /// Редактирует напоминание
        /// </summary>
        /// <param name="remindId">ID напоминания</param>
        /// <param name="lastDate">Дата последнего напоминания</param>
        public void UpdateRemind(int remindId, DateTime lastDate)
        {
            sqlDb.UpdateRemind(remindId,lastDate);
        }
        /// <summary>
        /// Удаляет напоминание
        /// </summary>
        /// <param name="remindId">ID напоминания</param>
        public void DeleteRemind(int remindId)
        {
            sqlDb.DeleteRemind(remindId);
        }
        /// <summary>
        /// Получает активность напоминания по ID
        /// </summary>
        /// <param name="remindId">ID напоминания</param>
        /// <returns>Активность</returns>
        public bool GetRemindActive(int remindId)
        {
            return sqlDb.GetRemindActive(remindId);
        }
        /// <summary>
        /// Получает ID пользователя напоминания по ID
        /// </summary>
        /// <param name="remindId">ID напоминания</param>
        /// <returns>ID пользователя</returns>
        public int GetRemindUser(int remindId)
        {
            return sqlDb.GetRemindUser(remindId);
        }
        /// <summary>
        /// Получает ID источника напоминания по ID
        /// </summary>
        /// <param name="remindId">ID напоминания</param>
        /// <returns>ID источника</returns>
        public int GetRemindSource(int remindId)
        {
            return sqlDb.GetRemindSource(remindId);
        }
        /// <summary>
        /// Получает тип источника напоминания по ID
        /// </summary>
        /// <param name="remindId">ID напоминания</param>
        /// <returns>Тип источника</returns>
        public int GetRemindSourceType(int remindId)
        {
            return sqlDb.GetRemindSourceType(remindId);
        }
        /// <summary>
        /// Получает периодичность напоминания по ID
        /// </summary>
        /// <param name="remindId">ID напоминания</param>
        /// <returns>Периодичность</returns>
        public int GetRemindPeriod(int remindId)
        {
            return sqlDb.GetRemindPeriod(remindId);
        }
        /// <summary>
        /// Получает тип напоминания по ID
        /// </summary>
        /// <param name="remindId">ID напоминания</param>
        /// <returns>Тип</returns>
        public int GetRemindType(int remindId)
        {
            return sqlDb.GetRemindType(remindId);
        }
        /// <summary>
        /// Получает последнюю дату напоминания по ID
        /// </summary>
        /// <param name="remindId">ID напоминания</param>
        /// <returns>Последняя дата</returns>
        public DateTime GetRemindLastDate(int remindId)
        {
            return sqlDb.GetRemindLastDate(remindId);
        }
        /// <summary>
        /// Получает название типа напоминания по ID
        /// </summary>
        /// <param name="remindId">ID типа напоминания</param>
        /// <returns>Название</returns>
        public string GetRemindTypeName(int remindId)
        {
            return sqlDb.GetRemindTypeName(remindId);
        }
        /// <summary>
        /// Получает название периодичности напоминания по ID
        /// </summary>
        /// <param name="remindId">ID периодичности напоминания</param>
        /// <returns>Название</returns>
        public string GetRemindPeriodName(int remindId)
        {
            return sqlDb.GetRemindPeriodName(remindId);
        }
        /// <summary>
        /// Получает ID напоминаний
        /// </summary>
        /// <returns>ID напоминаний</returns>
        public List<int> GetAllRemindIds(int orgId)
        {
            return sqlDb.GetAllRemindIds(orgId);
        }
        /// <summary>
        /// Получает ID ежечасных напоминаний
        /// </summary>
        /// <returns>ID напоминаний</returns>
        public List<int> GetAllHourRemindIds()
        {
            return sqlDb.GetAllHourRemindIds();
        }
        /// <summary>
        /// Получает ID ежедневных напоминаний
        /// </summary>
        /// <returns>ID напоминаний</returns>
        public List<int> GetAllDayRemindIds()
        {
            return sqlDb.GetAllDayRemindIds();
        }
        /// <summary>
        /// Получает ID ежемесячных напоминаний
        /// </summary>
        /// <returns>ID напоминаний</returns>
        public List<int> GetAllMonthRemindIds()
        {
            return sqlDb.GetAllMonthRemindIds();
        }
    }
}
