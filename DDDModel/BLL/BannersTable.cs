using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// Работа с баннерами
    /// </summary>
    public class BannersTable
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
        public BannersTable(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            sqlDb = sql;
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
        }

        /// <summary>
        /// Получаем все баннеры
        /// </summary>
        /// <param name="OrgId">ID организации</param>
        /// <param name="cardTypeId">ID типа карты(описанные тут же как проперти)</param>
        /// <returns>Лист ID карт</returns>
        public List<KeyValuePair<string,string>> GetAllBanners()
        {
            return sqlDb.GetAllBanners();
        }
    }
}
