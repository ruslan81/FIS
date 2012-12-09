using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// Работа с таблицами Карт и БД. Карты создаются для Водителей и транспортных средств.
    /// </summary>
    public class CardsTable
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
        /// Переменная. Идентификатор водительского типа карты 
        /// </summary>
        public int driversCardTypeId { get; set; }
        /// <summary>
        /// Переменная. Идентификатор типа карты транспортного средства
        /// </summary>
        public int vehicleCardTypeId { get; set; }
        /// <summary>
        /// Переменная. Идентификатор типа карты организации
        /// </summary>
        public int orgInitCardTypeId { get; set; }

        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="connectionsStringTMP">Строка подключения к базе данных(не обязательна, так как передается подключение</param>
        /// <param name="Current_Language">Текущий язык</param>
        /// <param name="sql">Обьект подключения к базе данных</param>
        public CardsTable(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            sqlDb = sql;
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            driversCardTypeId = 1;
            vehicleCardTypeId = 2;
            orgInitCardTypeId = 3;
        }

        /// <summary>
        /// Создать новую карту
        /// </summary>
        /// <param name="cardHolderName">Имя владельца карты</param>
        /// <param name="cardNumber">Номер карты</param>
        /// <param name="cardTypeId">Тип карты</param>
        /// <param name="orgId">Идентификатор организации</param>
        /// <param name="CardNote">Комментарий к карту</param>
        /// <param name="curUserId">Текущий пользователь, создающий карты(для лога)</param>
        /// <returns>Id новой карты</returns>
        public int CreateNewCard(string cardHolderName, string cardNumber, int cardTypeId, int orgId, string CardNote, int curUserId, int groupID)
        {
            Exception userNameAllreadyExists = new Exception("Пользователь с таким именем уже существует!");

            int returnValue = -1;
            string Login = "";
            //SQLDB sqlDB = new SQLDB(connectionString);
            //sqlDB.OpenConnection();
            if (cardTypeId == driversCardTypeId)
            {
                UsersTables userTables = new UsersTables(connectionString, CurrentLanguage, sqlDb);
                int userId = sqlDb.GetUserId_byUserName(cardHolderName);
                if (userId > 0)
                    throw userNameAllreadyExists;
                if(cardHolderName.Length>10)
                    Login = cardHolderName.Substring(0, 10);
                else
                    Login = cardHolderName;
                userId = sqlDb.AddNewUser(Login, cardNumber, userTables.DriverUserTypeId, 1, orgId, cardHolderName, cardNumber);
                //userTables.OpenConnection();
                userTables.AddUserInfoValue(userId, DataBaseReference.UserInfo_Name, cardHolderName);
                userTables.AddUserInfoValue(userId, DataBaseReference.UserInfo_RegDate, DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                //userTables.CloseConnection();
                returnValue = sqlDb.CreateNewCard(cardHolderName, cardNumber, cardTypeId, orgId, CardNote, userId, groupID);

                if (curUserId > 0)
                {
                    HistoryTable log = new HistoryTable(connectionString, CurrentLanguage, sqlDb);
                    log.AddHistoryRecord("fn_card", "card_id", returnValue, curUserId, log.newDriverRegistered, "Drivers name: " + cardHolderName + ", code: " + returnValue, sqlDb);
                }
            }
            else
            {
                returnValue = sqlDb.CreateNewCard(cardHolderName, cardNumber, cardTypeId, orgId, CardNote, groupID);

                if (curUserId > 0 && cardTypeId == vehicleCardTypeId)
                {
                    HistoryTable log = new HistoryTable(connectionString, CurrentLanguage, sqlDb);
                    log.AddHistoryRecord("fn_card", "card_id", returnValue, curUserId, log.newVehicleRegistered, "Vehicles number: " + cardHolderName + ", code: " + returnValue, sqlDb);
                }
            }
            //sqlDB.CloseConnection();
            return returnValue;
        }
        /// <summary>
        /// Получаем ID карты
        /// </summary>
        /// <param name="cardHolderName">Имя держателя карты</param>
        /// <param name="cardNumber">Номер карты</param>
        /// <param name="cardTypeId">Тип карты</param>
        /// <returns>ID карты</returns>
        public int GetCardId(string cardHolderName, string cardNumber, int cardTypeId)
        {
            int returnValue = -1;
            returnValue = sqlDb.GetCardId(cardHolderName, cardNumber, cardTypeId);
            return returnValue;
        }
        /// <summary>
        /// Получаем ID карты
        /// </summary>
        /// <param name="cardHolderName_Serial">имя держателя карты+серийный номер через пробел( устаревший метод, не стоит им пользоватся)</param>
        /// <param name="cardTypeId">тип карты</param>
        /// <returns>ID карты</returns>
        public int GetCardId(string cardHolderName_Serial, int cardTypeId)
        {
            string[] splitted = cardHolderName_Serial.Split(new char[] { ' ' });
            string name = "";
            string number = "";

            for(int i=0;i<splitted.Length - 1;i++)
            {
                name+= splitted[i] + " ";
            }
            name.Trim();

            number = splitted[splitted.Length - 1];

            return GetCardId(name, number, cardTypeId);
        }
        /// <summary>
        /// получаем ID пользователя, связанного с картой(для каждой карты создается пользователь, кроме ТС)
        /// </summary>
        /// <param name="cardId">ID карты</param>
        /// <returns>ID пользователя</returns>
        public int GetCardUserId(int cardId)
        {
            int userId = sqlDb.GetCardUserId(cardId);
            return userId;
        }
        /// <summary>
        /// Получаем все ID карт
        /// </summary>
        /// <param name="OrgId">ID организации</param>
        /// <param name="cardTypeId">ID типа карты(описанные тут же как проперти)</param>
        /// <returns>Лист ID карт</returns>
        public List<int> GetAllCardIds(int OrgId, int cardTypeId)
        {
            List<int> gettedIds = new List<int>();
            gettedIds = sqlDb.GetAllCardIds(OrgId, cardTypeId);
            return gettedIds;
        }
        /// <summary>
        /// Получаем все ID групп
        /// </summary>
        /// <param name="OrgId">ID организации</param>
        /// <param name="cardTypeId">ID типа карты(описанные тут же как проперти)</param>
        /// <returns>Лист ID групп</returns>
        public List<int> GetAllGroupIds(int OrgId, int cardTypeId)
        {
            return sqlDb.GetAllGroupIds(OrgId, cardTypeId);
        }
        /// <summary>
        /// Получаем все ID групп
        /// </summary>
        /// <param name="OrgId">ID организации</param>
        /// <returns>Лист ID групп</returns>
        public List<int> GetAllGroupIds(int OrgId)
        {
            return sqlDb.GetAllGroupIds(OrgId);
        }
        /// <summary>
        /// Получаем все имена владельцев карт принадлежащих какой-либо группе
        /// </summary>
        /// <param name="OrgId">ID организации</param>
        /// <param name="cardTypeId">ID типа карты(описанные тут же как проперти)</param>
        /// <param name="groupId">ID группы</param>
        /// <returns>Лист имен владельцев карт</returns>
        public List<int> GetAllCardIdsByGroupId(int OrgId, int cardTypeId, int groupId)
        {
            return sqlDb.GetAllCardIdsByGroupId(OrgId,cardTypeId,groupId);
        }
        /// <summary>
        /// Получаем имя владельца карты с указанным ID
        /// </summary>
        /// <param name="cardId">ID карты</param>
        /// <returns>Имя владельца карт</returns>
        public String GetCardHolderNameByCardId(int cardId)
        {
            return sqlDb.GetCardHolderNameByCardId(cardId);
        }
        
        /// <summary>
        /// Получаем имя группы по ID
        /// </summary>
        /// <param name="groupId">ID группы</param>
        /// <returns>Имя группы</returns>
        public string GetGroupNameById(int groupId)
        {
            return sqlDb.GetGroupNameById(groupId);
        }
        /// <summary>
        /// Получаем комментарий группы по ID
        /// </summary>
        /// <param name="groupId">ID группы</param>
        /// <returns>Комментарий группы</returns>
        public string GetGroupCommentById(int groupId)
        {
            return sqlDb.GetGroupCommentById(groupId);
        }
        /// <summary>
        /// Получаем тип карты группы по ID
        /// </summary>
        /// <param name="groupId">ID группы</param>
        /// <returns>Тип карты группы</returns>
        public int GetGroupCardTypeById(int groupId)
        {
            return sqlDb.GetGroupCardTypeById(groupId);
        }
        /// <summary>
        /// Удаляем группу по ID
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="groupId">ID группы</param>
        public void DeleteGroup(int orgId, int groupId)
        {
            sqlDb.DeleteGroup(orgId,groupId);
        }
        /// <summary>
        /// Редактируем группу по ID
        /// </summary>
        /// <param name="groupId">ID организации</param>
        /// <param name="name">Имя группы</param>
        /// <param name="comment">Комментарий группы</param>
        /// <param name="cardType">Тип карты</param>
        public void UpdateGroup(int groupId, String name, String comment, int cardType)
        {
            sqlDb.UpdateGroup(groupId,name,comment,cardType);
        }
        /// <summary>
        /// Создание группы
        /// </summary>
        /// <param name="orgID">ID организации</param>
        /// <param name="name">Имя группы</param>
        /// <param name="comment">Комментарий группы</param>
        /// <param name="cardType">Тип карты</param>
        public void CreateGroup(int orgID,String name, String comment, int cardType)
        {
            sqlDb.CreateGroup(orgID, name, comment, cardType);
        }
        /// <summary>
        /// Создание группы по умолчанию
        /// </summary>
        /// <param name="orgID">ID организации</param>
        public void CreateDefaultGroup(int orgID)
        {
            sqlDb.CreateDefaultGroup(orgID);
        }
        /// <summary>
        /// получаем все имена карты
        /// </summary>
        /// <param name="cardIds">Лист ID карт(можно получить например GetAllCardIds, либо выбрать как-то еще)</param>
        /// <returns>Лист строк имен. длина массива имен равна длине массива ID карт</returns>
        public List<string> GetCardNames(List<int> cardIds)
        {
            List<string> names = new List<string>();
            foreach (int id in cardIds)
            {
                names.Add(sqlDb.GetCardName(id));
            }
            return names;
        }
        /// <summary>
        /// Получаем имя для карты ID
        /// </summary>
        /// <param name="cardId">ID карты</param>
        /// <returns>Имя карты</returns>
        public string GetCardName(int cardId)
        {
            string name = "";
            name = sqlDb.GetCardName(cardId);
            return name;
        }
        /// <summary>
        /// получаем все номера карт
        /// </summary>
        /// <param name="cardIds">Лист ID карт(можно получить например GetAllCardIds, либо выбрать как-то еще)</param>
        /// <returns>Лист строк номеров карт. длина массива номеров равна длине массива ID карт</returns>
        public List<string> GetCardNumbers(List<int> cardIds)
        {
            List<string> numbers = new List<string>();
            foreach (int id in cardIds)
            {
                numbers.Add(sqlDb.GetCardNumber(id));
            }
            return numbers;
        }
        /// <summary>
        /// Получает номер карты по ID карты
        /// </summary>
        /// <param name="cardId">ID карты</param>
        /// <returns>Номер карты</returns>
        public string GetCardNumber(int cardId)
        {
            string returnValue = sqlDb.GetCardNumber(cardId);
            return returnValue;
        }
        /// <summary>
        /// Получает комментарий к карте
        /// </summary>
        /// <param name="cardId">ID карты</param>
        /// <returns>Коментарий</returns>
        public string GetCardNote(int cardId)
        {
            string returnValue = sqlDb.GetCardNote(cardId);
            return returnValue;
        }
        /// <summary>
        /// Получает группу карты по ID карты
        /// </summary>
        /// <param name="cardId">ID карты</param>
        /// <returns>Группа карты</returns>
        public int GetCardGroupID(int cardId)
        {
            return sqlDb.GetCardGroupID(cardId);
        }
        /// <summary>
        /// Присваеваем комментарий к карте
        /// </summary>
        /// <param name="cardId">ID карты</param>
        /// <param name="Note">строка комментария</param>
        public void SetCardNote(int cardId, string Note)
        {
            sqlDb.SetCardNote(cardId, Note);
        }
        /// <summary>
        /// Изменяет имя карты
        /// </summary>
        /// <param name="cardHolderName">Новое имя карты</param>
        /// <param name="cardId">ID карты</param>
        public void ChangeCardName(string cardHolderName, int cardId)
        {
            sqlDb.ChangeCardHolderName(cardHolderName, cardId);
        }
        /// <summary>
        /// Изменяет группу карты
        /// </summary>
        /// <param name="groupId">Новый ID группы</param>
        /// <param name="cardId">ID карты</param>
        public void ChangeCardGroup(int groupId, int cardId)
        {
            sqlDb.ChangeCardGroup(groupId, cardId);
        }
        /// <summary>
        /// Изменяет комментарий карты
        /// </summary>
        /// <param name="cardComment">Новое имя карты</param>
        /// <param name="cardId">ID карты</param>
        public void ChangeCardComment(string cardComment, int cardId)
        {
            sqlDb.ChangeCardComment(cardComment, cardId);
        }

        /// <summary>
        /// Изменяет номер карты
        /// </summary>
        /// <param name="cardNumber">Новый номер карты</param>
        /// <param name="cardId">ID карты</param>
        /// <param name="curUserId">ID пользователя выполяющего действие</param>
        public void ChangeCardNumber(string cardNumber, int cardId, int curUserId)
        {
            sqlDb.ChangeCardNumber(cardNumber, cardId);
            //HistoryTable log = new HistoryTable(connectionString, CurrentLanguage, sqlDb);
            //log.AddHistoryRecord("fn_card", "card_id", cardId, curUserId, log.driversRegDataChanged, "Drivers card number: " + cardNumber + ", code: " + cardId, sqlDb);
        }
        /// <summary>
        /// Удаляет карту и все связанные с ней файлы и блоки данных
        /// </summary>
        /// <param name="CardId">ID карты</param>
        public void DeleteCardAndAllFiles(int CardId)
        {
            sqlDb.DeleteCard(CardId);
        }
        /// <summary>
        /// Получает все блоки данных, связанные с картой
        /// </summary>
        /// <param name="cardId">ID карты</param>
        /// <returns></returns>
        public List<int> GetAllDataBlockIds_byCardId(int cardId)
        {
            List<int> ids = new List<int>();
            ids = sqlDb.GetDataBlockIdsByCardId(cardId);
            return ids;
        }
    }
}
