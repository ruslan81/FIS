using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// работа с таблицами пользователей.
    /// </summary>
    public class UsersTables
    {
        private string CurrentLanguage;//STRING_RU,STRING_ENG etc.
        private string connectionString;
        private SQLDB sqlDb;

        public int DriverUserTypeId { get; set; }//1
        public int ManagerUserTypeId { get; set; }//2
        public int AdministratorUserTypeId { get; set; }//3
        public int DealerUserTypeId { get; set; }//3

        public UsersTables(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            //sqlDb = new SQLDB(connectionString);
            sqlDb = sql;
            DriverUserTypeId = 1;
            ManagerUserTypeId = 2;
            AdministratorUserTypeId = 3;
            DealerUserTypeId = 4;
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
        //Authentification
        public bool CustomAuthenticate(string username, string password)
        {
            int usersCount;
            sqlDb.OpenConnection();
            usersCount = sqlDb.GetUserAndPasswordCount(username, password);
            sqlDb.CloseConnection();

            if (usersCount > 0)
                return true;
            else
                return false;
        }
        public bool CustomAuthenticate(string username, string password, int orgId)
        {
            int usersCount;

            usersCount = sqlDb.GetUserAndPasswordCount(username, password, orgId);

            if (usersCount > 0)
                return true;
            else
                return false;
        }
        // FD_USER      
        public int GetUserRoleId(int userId)
        {
            int userRoleId = sqlDb.getUserRoleId(userId);
            return userRoleId;
        }
        public string Get_UserPassword(int userId)
        {
            return sqlDb.GetUserPassword(userId);
        }
        public string Get_UserPassword(string userEmail)
        {
            int stringId = sqlDb.GetStringId(userEmail);
            int userId = sqlDb.GetUserInfoUserId(stringId);
            return Get_UserPassword(userId);
        }
        public string Get_UserName(int userId)
        {
            return sqlDb.GetUserName(userId);
        }
        public int Get_UserID_byName(string userName)
        {
            return sqlDb.GetUserId_byUserName(userName);
        }
        public string Get_UserTypeStr(int userId)
        {
            int userTypeId = sqlDb.GetUserTypeId(userId);
            int userTypeNameId = sqlDb.GetUserTypeNameId(userTypeId);
            return sqlDb.GetString(userTypeNameId, CurrentLanguage);
        }
        public int Get_UserTypeId(int userId)
        {
            return sqlDb.GetUserTypeId(userId);
        }
        public string Get_UserOrgName(int userId)
        {
            int ORG_ID = sqlDb.GetUserOrgId(userId);
            int orgNameId = sqlDb.GetOrgNameId(ORG_ID);
            return sqlDb.GetString(orgNameId, CurrentLanguage);
        }
        public int Get_UserOrgId(int userId)
        {
            return sqlDb.GetUserOrgId(userId);
        }
        public DateTime Get_TimeConnect(int userId)
        {
            string dateString = sqlDb.GetDateConnect(userId);
            if (dateString == "")
                return new DateTime();
            DateTime datetoReturn;
            try
            {
                datetoReturn = Convert.ToDateTime(dateString);
            }
            catch
            {
                datetoReturn = new DateTime();
            }
            return datetoReturn;
        }
        public void Set_TimeConnect(int userId)
        {
            sqlDb.SetCurrentTime("fd_user", "USER_ID", userId, "DATE_CONNECT");
        }
        public string GetUserRoleName(int userRoleId)
        {
            int userRoleNameID = sqlDb.GetUserRoleNameId(userRoleId);
            return sqlDb.GetString(userRoleNameID, CurrentLanguage);
        }
        public string Get_UserRoleName(int userId)
        {
            int userRoleId = sqlDb.getUserRoleId(userId);
            int userRoleNameID = sqlDb.GetUserRoleNameId(userRoleId);
            return sqlDb.GetString(userRoleNameID, CurrentLanguage);
        }
        public string Get_UserRoleName(string userName)
        {
            int userId = Get_UserID_byName(userName);
            return Get_UserRoleName(userId);
        }
        public List<int> Get_AllUsersId()
        {
            return sqlDb.GetAllUsersId();
        }
        public List<int> Get_AllUsersId(int orgId)
        {
            return sqlDb.GetAllUsersId(orgId);
        }
        public List<int> Get_AllUsersId(int orgId, int UserType)
        {
            return sqlDb.GetAllUsersId(orgId, UserType);
        }
        public List<KeyValuePair<string, int>> GetAllUsersRoles()
        {
            return sqlDb.GetUserRoles(CurrentLanguage);
        }
        public List<KeyValuePair<string, int>> GetAllUsersTypes()
        {
            return sqlDb.GetUserTypes(CurrentLanguage);
        }
        public void EditUser(string oldPass, string oldName, UserFromTable newUser, int userTypeId, int userRoleId, int orgId, int curUserId)
        {
            Exception userNameAllreadyExists = new Exception("Пользователь с таким именем уже существует!");
            int userNewId = sqlDb.GetUserId_byUserName(newUser.name);
            int userOldId = sqlDb.GetUserId_byUserName(oldName);
            if (userNewId > 0)
            {
                if (userOldId != userNewId)
                    throw userNameAllreadyExists;
            }

            int userId = sqlDb.AddNewUser(newUser.name, newUser.pass, userTypeId, userRoleId, orgId, oldName, oldPass);

            if (curUserId > 0)
            {
                HistoryTable log = new HistoryTable(connectionString, CurrentLanguage, sqlDb);
                log.AddHistoryRecord("fd_user", "USER_ID", userId, curUserId, log.driversRegDataChanged, "Code: " + userId.ToString(), sqlDb);
            }
        }
        public int AddNewUser(UserFromTable newUser, int userTypeId, int userRoleId, int orgId, int curUserId)
        {
            Exception userNameAllreadyExists = new Exception("Пользователь с таким именем уже существует!");
            int returnUserId = -1;
            int userId = sqlDb.GetUserId_byUserName(newUser.name);
            if (userId > 0)
                throw userNameAllreadyExists;

            returnUserId = sqlDb.AddNewUser(newUser.name, newUser.pass, userTypeId, userRoleId, orgId, newUser.name, newUser.pass);
            AddUserInfoValue(returnUserId, DataBaseReference.UserInfo_RegDate, DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            AddUserInfoValue(returnUserId, DataBaseReference.UserInfo_EndOfRegistrationDate, DateTime.Now.AddMonths(6).ToShortDateString());

            if (curUserId > 0)
            {
                HistoryTable log = new HistoryTable(connectionString, CurrentLanguage, sqlDb);
                log.AddHistoryRecord("fd_user", "USER_ID", returnUserId, curUserId, log.newUserRegistered, "Login: " + newUser.name + ", code: " + returnUserId, sqlDb);
            }

            return returnUserId;
        }
        public void DeleteUser(int userId)
        {
            sqlDb.OpenConnection();
            sqlDb.DeleteUser(userId);
            sqlDb.CloseConnection();
        }
        //FD_USER_INFO_SET and FD_USER_INFO
        public string GetUserInfoValue(int userId, int UserInfoId)
        {
            int stringId = sqlDb.GetUserInfoValueStrId(userId, UserInfoId);
            return sqlDb.GetString(stringId, CurrentLanguage);
        }
        public string GetUserInfoValue(int userId, string UserInfoName)
        {
            int infoNameId = GetUserInfoNameId(UserInfoName);
            int stringId = sqlDb.GetUserInfoValueStrId(userId, infoNameId);
            return sqlDb.GetString(stringId, CurrentLanguage);
        }
        public int GetUserInfoNameId(string InfoName)
        {
            int stringId = sqlDb.GetStringId(InfoName);
            int UserInfoId = sqlDb.GetUserInfoName(stringId);

            if (UserInfoId > 0)
                return UserInfoId;
            else
                return AddUserInfoName(InfoName);
        }
        public List<KeyValuePair<string, int>> GetAllUserInfoNames()
        {
            return sqlDb.GetAllUserInfoNames(CurrentLanguage);
        }
        public int AddUserInfoName(string InfoName)
        {
            return sqlDb.AddUserInfoName(InfoName, CurrentLanguage);
        }
        public void AddUserInfoValue(int userId, int UserInfoId, string value)
        {
            sqlDb.AddUserInfoValue(userId, UserInfoId, value, CurrentLanguage);
        }
        public void AddUserInfoValue(int userId, string UserInfoName, string value)
        {
            int infoNameId = GetUserInfoNameId(UserInfoName);
            if (infoNameId <= 0)
                infoNameId = AddUserInfoName(UserInfoName);
            sqlDb.AddUserInfoValue(userId, infoNameId, value, CurrentLanguage);
        }
        public void EditUserInfo(int userId, int UserInfoId, string newValue)
        {
            if(sqlDb.GetUserInfoValueStrId(userId, UserInfoId) > 0)
                sqlDb.EditUserInfo(userId, UserInfoId, newValue, CurrentLanguage);
            else
                sqlDb.AddUserInfoValue(userId, UserInfoId, newValue, CurrentLanguage);
        }       
    }
    /// <summary>
    /// Класс описывает одного пользователя
    /// </summary>
    public class UserFromTable
    {
        /// <summary>
        /// ID пользователя
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// имя
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// пароль
        /// </summary>
        public string pass { get; set; }
        /// <summary>
        /// тип пользователя
        /// </summary>
        public string userType { get; set; }
        /// <summary>
        /// роль пользователя
        /// </summary>
        public string userRole { get; set; }
        /// <summary>
        /// время подключения
        /// </summary>
        public DateTime timeConnection { get; set; }
        /// <summary>
        /// название организации
        /// </summary>
        public string orgName { get; set; }

        public UserFromTable()
        {
            name = "";
            pass = "";
            userType = "";
            userRole = "";
            timeConnection = new DateTime();
            orgName = "";
        }

        public UserFromTable(string nameT, string passT, string userTypeT, string userRoleT, DateTime timeConnectionT, string orgNameT)
        {
            name = nameT;
            pass = passT;
            userType = userTypeT;
            userRole = userRoleT;
            timeConnection = timeConnectionT;
            orgName = orgNameT;
        }
        /// <summary>
        /// Заполнить поля информацией
        /// </summary>
        /// <param name="UserId">ID пользователя</param>
        /// <param name="tables">обьект класса UsersTables с открытым подключением</param>
        /// <returns>this</returns>
        public UserFromTable FillWithInfo(int UserId, UsersTables tables)
        {
            name = tables.Get_UserName(UserId);
            pass = tables.Get_UserPassword(UserId);
            userType = tables.Get_UserTypeStr(UserId);
            userRole = tables.Get_UserRoleName(UserId);
            timeConnection = tables.Get_TimeConnect(UserId);
            orgName = tables.Get_UserOrgName(UserId);
            id = UserId;
            return this;
        }
    }
}
