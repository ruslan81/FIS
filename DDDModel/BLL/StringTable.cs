using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// работа с таблицей строк.
    /// </summary>
    public class StringTable
    {
        private string CurrentLanguage;//STRING_RU,STRING_RUG etc.
        private string connectionString;
        SQLDB sqlDB;

        public void OpenConnection()
        {
            sqlDB.OpenConnection();
        }

        public void CloseConnection()
        {
            sqlDB.CloseConnection();
        }

        public StringTable(string connectionsStringTMP, string Current_Language)
        {
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            sqlDB = new SQLDB(connectionString);
        }

        public string GetString(int stringId)
        {
            SQLDB sqldb = new SQLDB(connectionString);
            string gettedString;

            sqldb.OpenConnection();
            gettedString = sqldb.GetString(stringId, CurrentLanguage);
            sqldb.CloseConnection();

            return gettedString;
        }
        /// <summary>
        /// Использовать аккуратно - можно запороть базу со строками, можно использовать другие методы в SQLDB
        /// </summary>
        /// <param name="stringId">Id строки</param>
        /// <param name="newValue">Значение строки</param>
        /// <param name="Language">Язык</param>
        public void UpdateString(int stringId, string newValue, string Language)
        {
            sqlDB.TranslateString(newValue, Language, stringId);
        }
        /// <summary>
        /// Получает ID строки
        /// </summary>
        /// <param name="stringValue">Значение строки</param>
        /// <param name="Language">Язык</param>
        /// <returns>ID строки</returns>
        public int GetStringId(string stringValue, string Language)
        {
            return sqlDB.GetStringId(stringValue, Language);
        }
    }
}
