using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;
using System.Data;

namespace BLL
{
    /// <summary>
    /// работа с логированием действий пользователя.
    /// </summary>
    public class HistoryTable//Класс работы с таблицами истории действий пользователя
    {
        /// <summary>
        /// Текущия язык
        /// </summary>
        private string CurrentLanguage;//STRING_RU,STRING_ENG etc.
        /// <summary>
        /// Строка подключения(в большинстве не нужна)
        /// </summary>
        private string connectionString;
        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        private SQLDB sqlDb;
        #region "Возможные записи истории"
        /// <summary>
        /// Изменение учетных данных пользователя/водителя
        /// </summary>
        public int driversRegDataChanged { get; set; }
        /// <summary>
        /// Изменение учетных данных БУ ТС
        /// </summary>
        public int vehiclesRegDataChanged { get; set; }
        /// <summary>
        /// Загружен блок данных PLF
        /// </summary>
        public int PLFDataBlockLoaded { get; set; }
        /// <summary>
        /// Загружен блок данных карты
        /// </summary>
        public int DDDDriversDataBlockLoaded { get; set; }
        /// <summary>
        /// Загружен блок данных бортового устройства
        /// </summary>
        public int DDDVehiclesDataBlockLoaded { get; set; }
        /// <summary>
        /// Регистрация нового пользователя в системе
        /// </summary>
        public int newUserRegistered { get; set; }
        /// <summary>
        /// Регистрация нового водителя
        /// </summary>
        public int newDriverRegistered { get; set; }
        /// <summary>
        /// Регистрация нового транспортного средства
        /// </summary>
        public int newVehicleRegistered { get; set; }
        /// <summary>
        /// Изменение прав доступа к отчету для ролей пользователей
        /// </summary>
        public int setReportUserRoles { get; set; }
        /// <summary>
        /// Добавление отчета для организации
        /// </summary>
        public int setReportUserOrg { get; set; }
        /// <summary>
        /// Оплата счета
        /// </summary>
        public int invoicePaid { get; set; }
        #endregion
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionsStringTMP">Строка подключения. МОжет быть любой</param>
        /// <param name="Current_Language">Язык</param>
        /// <param name="sql">обьект SQLDB</param>
        public HistoryTable(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            bool toCloseConncection = false;
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            //sqlDb = new SQLDB(connectionString);
            sqlDb = sql;
            string LanguageForAction = "STRING_EN";
            if (!sqlDb.IsConnectionOpened())
            {
                sqlDb.OpenConnection();
                toCloseConncection = true;
            }
            driversRegDataChanged = AddOrGetAction("Users/drivers registration data changed", LanguageForAction);
            vehiclesRegDataChanged = AddOrGetAction("Vehicles registration data changed", LanguageForAction);
            PLFDataBlockLoaded = AddOrGetAction("PLF data block loaded", LanguageForAction);
            DDDDriversDataBlockLoaded = AddOrGetAction("Cards data block loaded", LanguageForAction);
            DDDVehiclesDataBlockLoaded = AddOrGetAction("Vehicles unit data block loaded", LanguageForAction);
            newUserRegistered = AddOrGetAction("New user registered", LanguageForAction);
            newDriverRegistered = AddOrGetAction("New driver registered", LanguageForAction);
            newVehicleRegistered = AddOrGetAction("New vehicle registered", LanguageForAction);
            setReportUserRoles = AddOrGetAction("Change permissions to the report for user role", LanguageForAction);
            setReportUserOrg = AddOrGetAction("New report type successfully added", LanguageForAction);
            invoicePaid = AddOrGetAction("Invoice paid", LanguageForAction);
            if(toCloseConncection)
                sqlDb.CloseConnection();
        }

        public void OpenConnection()
        {
            sqlDb.OpenConnection();
        }
        public void CloseConnection()
        {
            sqlDb.CloseConnection();
        }
        public void OpenTransaction()
        {
            sqlDb.OpenTransaction();
        }
        public void CommitTransaction()
        {
            sqlDb.CommitConnection();
        }
        public void RollbackConnection()
        {
            sqlDb.RollbackConnection();
        }

        //FN_HISTORY
        /// <summary>
        /// Добавить запись к логу
        /// </summary>
        /// <param name="tableName">Название измененной таблицы в БД</param>
        /// <param name="tableKeyFieldName">Название измененного поля в таблице в БД</param>
        /// <param name="TABLE_KEYFIELD_VALUE">Измененное значение</param>
        /// <param name="userId">ID пользователя, который произвел действие</param>
        /// <param name="actionId">ID действия</param>
        /// <param name="actionDate">Дата действия</param>
        /// <param name="Note">Комментарий</param>
        /// <param name="SQLForAdding">SQL подключение с открытым подключением(созданное внешне)</param>
        /// <returns>Дата записи в логе</returns>
        public DateTime AddHistoryRecord(string tableName, string tableKeyFieldName, int TABLE_KEYFIELD_VALUE, int userId, int actionId, DateTime actionDate, string Note, SQLDB SQLForAdding)
        {
            if (Note.Length > 1023)
                Note = Note.Substring(0, 1023);

            int tableId = GetOrAddTable(tableName, tableKeyFieldName, SQLForAdding);
            DateTime returnDate = SQLForAdding.AddHistoryRecord(tableId, TABLE_KEYFIELD_VALUE, userId, actionId, actionDate, Note);
            return returnDate;
        }
        /// <summary>
        /// Добавить запись к логу
        /// </summary>
        /// <param name="tableName">Название измененной таблицы в БД</param>
        /// <param name="tableKeyFieldName">Название измененного поля в таблице в БД</param>
        /// <param name="TABLE_KEYFIELD_VALUE">Измененное значение</param>
        /// <param name="userId">ID пользователя, который произвел действие</param>
        /// <param name="actionId">ID действия</param>
        /// <param name="Note">Комментарий</param>
        /// <param name="SQLForAdding">SQL подключение с открытым подключением(созданное внешне)</param>
        /// <returns>Дата записи в логе</returns>
        public DateTime AddHistoryRecord(string tableName, string tableKeyFieldName, int TABLE_KEYFIELD_VALUE, int userId, int actionId, string Note, SQLDB SQLForAdding)
        {
            return AddHistoryRecord(tableName, tableKeyFieldName, TABLE_KEYFIELD_VALUE, userId, actionId, DateTime.Now, Note, SQLForAdding);
        }
        /// <summary>
        /// Получает всю исторю для выбранного пользователя
        /// </summary>
        /// <param name="usersId">ID пользователя</param>
        /// <returns>DataTable, который можно напрямую использовать как источник данных.</returns>
        public DataTable GetAllHistorysForUsers(int usersId)
        {
            List<int> userIds = new List<int>();
            userIds.Add(usersId);
            return GetAllHistorysForUsers(userIds);
        }
        /// <summary>
        /// Получает всю исторю для группы выбранных пользователей
        /// </summary>
        /// <param name="usersIds">Массив ID пользователей</param>
        /// <returns>DataTable, который можно напрямую использовать как источник данных.</returns>
        public DataTable GetAllHistorysForUsers(List<int> usersIds)
        {
            return GetAllHistorysForUsers(usersIds, new DateTime(), DateTime.Now, -1, "");
        }
        /// <summary>
        /// Получает всю исторю для группы выбранных пользователей с выполнением некоторых условий
        /// </summary>
        /// <param name="usersIds">Массив ID пользователей</param>
        /// <param name="from">Дата с</param>
        /// <param name="to">Дата по</param>
        /// <param name="actionIdIns">ID типа действия</param>
        /// <param name="searchString">Строка, которая должна встречаться в логе</param>
        /// <returns>DataTable, который можно напрямую использовать как источник данных.</returns>
        public DataTable GetAllHistorysForUsers(List<int> usersIds, DateTime from, DateTime to, int actionIdIns, string searchString)
        {
            Exception noValues = new Exception("Нет значений для Журнала");
            DataTable UsersHistoryData = new DataTable("UsersHistoryData");
            string userName;
            UsersTables userTable = new UsersTables(connectionString, CurrentLanguage, sqlDb);
            DataRow dr;
            List<List<KeyValuePair<string, string>>> gettedIdList = new List<List<KeyValuePair<string, string>>>();
            List<string> gettedDateTimes = new List<string>();
            UsersHistoryData.Columns.Add(new DataColumn("USER_ID", typeof(int)));
            UsersHistoryData.Columns.Add(new DataColumn("ACTION_ID", typeof(int)));
            UsersHistoryData.Columns.Add(new DataColumn("TABLE_ID", typeof(int)));
            UsersHistoryData.Columns.Add(new DataColumn("Дата и время", typeof(DateTime)));
            UsersHistoryData.Columns.Add(new DataColumn("Пользователь", typeof(string)));
            UsersHistoryData.Columns.Add(new DataColumn("Описание", typeof(string)));

            int tableId = -1;
            int actionId = -1;
            string description;
            DateTime date = new DateTime();
            foreach (int userId in usersIds)
            {
                //userTable.OpenConnection();
                userName = userTable.Get_UserName(userId);
                //userTable.CloseConnection();

                gettedIdList = sqlDb.GetHistoryActionIdAndTableId(userId, from, to, actionIdIns);
                                
                tableId = -1;
                actionId = -1;
                foreach (List<KeyValuePair<string, string>> record in gettedIdList)
                {
                    foreach (KeyValuePair<string, string> oneValue in record)
                    {
                        if (oneValue.Key == "ACTION_ID")
                            actionId = Convert.ToInt32(oneValue.Value);
                        if (oneValue.Key == "TABLE_ID")
                            tableId = Convert.ToInt32(oneValue.Value);
                        if (oneValue.Key == "ACTION_DATE")
                            date = DateTime.Parse(oneValue.Value);
                    }
                    if (actionId <= 0 || tableId <= 0)
                        throw noValues;
                    dr = UsersHistoryData.NewRow();
                    dr["Дата и время"] = date;//дата
                    dr["Пользователь"] = userName;
                    description =  GetActionString(actionId) + ". " + GetHistoryNote(userId, actionId, tableId, date) + ".";
                    if (searchString != "")//очень плохой способ. Если будет тормозить - переделать на выборку из базы или еще чего...
                    {
                        //if (description.Contains(searchString))
                        if (description.ToLower().Contains(searchString.ToLower()))
                        {
                            description = description.Replace(searchString, "&lbb&rb&lbFONT COLOR=RED&rb" + searchString + "&lb&slFONT&rb&lb&slb&rb");
                            description = description.Replace(searchString.ToLower(), "&lbb&rb&lbFONT COLOR=RED&rb" + searchString.ToLower() + "&lb&slFONT&rb&lb&slb&rb");
                            String s=searchString.Substring(0,1).ToUpper()+searchString.Substring(1);
                            description = description.Replace(s, "&lbb&rb&lbFONT COLOR=RED&rb" + s + "&lb&slFONT&rb&lb&slb&rb");
                        }
                        else
                            continue;
                    }
                    dr["Описание"] = description;
                    dr["USER_ID"] = userId;
                    dr["ACTION_ID"] = actionId;
                    dr["TABLE_ID"] = tableId;

                    UsersHistoryData.Rows.Add(dr);
                }
            }
            UsersHistoryData.DefaultView.Sort = "Дата и время DESC";
            
            return UsersHistoryData;
        }
        /// <summary>
        /// Получает дату записи в логе
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="actionId">ID типа действия</param>
        /// <param name="tableId">ID таблицы</param>
        /// <returns>Дата записи в логе</returns>
        public DateTime GetHistoryDate(int userId, int actionId, int tableId)
        {
            string dateString = sqlDb.GetHistoryDate(userId, actionId, tableId);
            DateTime returnDate = DateTime.Parse(dateString);
            return returnDate;
        }
        /// <summary>
        /// Получить комментарий к записи в логе
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="actionId">ID типа действия</param>
        /// <param name="tableId">ID таблицы</param>
        /// <param name="historyDate">Дата записи</param>
        /// <returns>Комментарий к записи в логе</returns>
        public string GetHistoryNote(int userId, int actionId, int tableId, DateTime historyDate)
        {
            return sqlDb.GetHistoryNote(userId, actionId, tableId, historyDate);
        }
        //FD_TABLE
        /// <summary>
        /// Получить ID таблицы в БД таблице FD_TABLE
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <param name="SQLForAdding">sql подключение с открытым подключением</param>
        /// <returns>ID таблицы в БД таблице FD_TABLE</returns>
        public int Get_TableId(string tableName, SQLDB SQLForAdding)
        {
            return SQLForAdding.GetTableId(tableName);
        }
        /// <summary>
        /// Получить ID таблицы в БД таблице FD_TABLE
        /// </summary>
        /// <param name="tableName">Название таблицы</param>
        /// <returns>ID таблицы в БД таблице FD_TABLE</returns>
        public int Get_TableId(string tableName)
        {
            return sqlDb.GetTableId(tableName);
        }
        /// <summary>
        /// Добавляет Новую таблицу, или возвращает ID существующей таблицы
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="tableKeyFieldName">Имя primary key в таблице</param>
        /// <param name="SQLForAdding">sql подключение с открытым подключением</param>
        /// <returns>ID таблицы</returns>
        public int GetOrAddTable(string tableName, string tableKeyFieldName, SQLDB SQLForAdding)
        {
            int tableId = Get_TableId(tableName,SQLForAdding);
            if (tableId <= 0)
                tableId = SQLForAdding.AddTable(tableName, tableKeyFieldName, "Автоматически созданная запись для таблицы " + tableName, CurrentLanguage);
            return tableId;
        }
        /// <summary>
        /// Добавляет Новую таблицу, или возвращает ID существующей таблицы
        /// </summary>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="tableKeyFieldName">Имя primary key в таблице</param>
        /// <returns>ID таблицы</returns>
        public int GetOrAddTable(string tableName, string tableKeyFieldName)
        {
            int tableId = Get_TableId(tableName);
            if (tableId <= 0)
                tableId = sqlDb.AddTable(tableName, tableKeyFieldName, "Автоматически созданная запись для таблицы " + tableName, CurrentLanguage);
            return tableId;
        }
        /// <summary>
        /// Получить название таблицы
        /// </summary>
        /// <param name="tableId">ID таблицы</param>
        /// <returns>Название таблицы</returns>
        public string Get_TableName(int tableId)
        {
            return sqlDb.GetTableName(tableId);
        }
        /// <summary>
        /// ПОлучить название primary key
        /// </summary>
        /// <param name="tableId">ID таблицы</param>
        /// <returns>название primary key</returns>
        public string Get_TableKeyFieldName(int tableId)
        {
            return sqlDb.GetTableKeyFieldName(tableId);
        }
        /// <summary>
        /// Получить комментарий к таблице
        /// </summary>
        /// <param name="tableId">ID таблицы</param>
        /// <returns>Комментарий к таблице</returns>
        public string Get_TableNote(int tableId)
        {
            int noteId = sqlDb.GetTableNoteId(tableId);
            return sqlDb.GetString(noteId, CurrentLanguage);
        }
        //FD_ACTION
        /// <summary>
        /// Получить все возможные действия для лога
        /// </summary>
        /// <returns>Массив пар (название действия, ID действия) </returns>
        public List<KeyValuePair<string, int>> GetAllActions()
        {
            return sqlDb.GetAllActions(CurrentLanguage);
        }            
        /// <summary>
        /// получить название действия
        /// </summary>
        /// <param name="actionId">ID действия</param>
        /// <returns>название действия</returns>
        public string GetActionString(int actionId)
        {
            int actionStrid = sqlDb.GetActionSrId(actionId);
            return sqlDb.GetString(actionStrid, CurrentLanguage);
        }
        /// <summary>
        /// Получить ID действия по ID строки с названием этого действия
        /// </summary>
        /// <param name="actionStrId">ID строки с названием этого действия</param>
        /// <returns>ID действия</returns>
        private int GetActionString_BySTRID(int actionStrId)
        {
            return sqlDb.GetActionId(actionStrId);
        }
        /// <summary>
        /// Добавить или получить(если уже есть такое) действие, с языком по умолчинию(английский).
        /// </summary>
        /// <param name="actionString">Название действия</param>
        /// <returns>ID действия</returns>
        public int AddOrGetAction(string actionString)
        {
            return AddOrGetAction(actionString, CurrentLanguage);
        }
        /// <summary>
        /// Добавить или получить(если уже есть такое) действие
        /// </summary>
        /// <param name="actionString">Название действия</param>
        /// <param name="Language">Язык названия действия</param>
        /// <returns>ID действия</returns>
        private int AddOrGetAction(string actionString, string Language)
        {
            int stringId = sqlDb.GetStringId(actionString, Language);
            int actionId = -1;
            if (stringId > 0)
                actionId = GetActionString_BySTRID(stringId);
            if (stringId == 0 || actionId == 0)
                actionId = sqlDb.AddAction(actionString, Language);
            return actionId;
        }
    }
}


