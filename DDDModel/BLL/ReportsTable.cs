using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// работа с таблицами отчетов.
    /// </summary>
    public class ReportsTable
    {
        private string CurrentLanguage;//STRING_RU,STRING_ENG etc.
        private string connectionString;
        SQLDB sqlDBR;
        public int STRID_REPORT_NAME_Ident { get; set; }
        public int STRID_REPORT_SHORT_NAME_Ident { get; set; }
        public int STRID_REPORT_FULL_NAME_Ident { get; set; }
        public int STRID_REPORT_PRINT_NAME_Ident { get; set; }

        public void OpenConnection()
        {
            sqlDBR.OpenConnection();
        }
        public void CloseConnection()
        {
            sqlDBR.CloseConnection();
        }
        public void OpenTransaction()
        {
            sqlDBR.OpenTransaction();
        }
        public void CommitTransaction()
        {
            sqlDBR.CommitConnection();
        }
        public void RollbackConnection()
        {
            sqlDBR.RollbackConnection();
        }

        public ReportsTable(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            //sqlDBR = new SQLDB(connectionString);
            sqlDBR = sql;
            STRID_REPORT_NAME_Ident = 0;
            STRID_REPORT_SHORT_NAME_Ident = 1;
            STRID_REPORT_FULL_NAME_Ident = 2;
            STRID_REPORT_PRINT_NAME_Ident = 3;
        }
        //USER_REPORTS
        public int AddUserReport(int ReportTypeId, string reportUserName, int Price, string NOTE)
        {
            return sqlDBR.AddUserReport(ReportTypeId, reportUserName, DateTime.Now, DateTime.Now, Price, NOTE, CurrentLanguage);
        }
        public List<int> GetAllReportsIds()
        {
            return sqlDBR.GetAllUserReportsIds();
        }
        public List<int> GetAllReportsIds(int reportTypeId)
        {
            return sqlDBR.GetAllUserReportsIds(reportTypeId);
        }
        /// <summary>
        /// Берем название типа отчета.  NameType:
        /// 0 - STRID_REPORT_NAME
        /// 1 - STRID_REPORT_SHORT_NAME
        /// 2 - STRID_REPORT_FULL_NAME
        /// 3 - STRID_REPORT_PRINT_NAME
        /// </summary>
        /// <param name="reportUserId">ID отчета</param>
        /// <param name="nameType">
        /// 0 - STRID_REPORT_NAME
        /// 1 - STRID_REPORT_SHORT_NAME
        /// 2 - STRID_REPORT_FULL_NAME
        /// 3 - STRID_REPORT_PRINT_NAME
        /// </param>
        /// <returns>имя типа отчета</returns>
        public string GetReportTypeName(int reportUserId, int nameType)
        {
            int reportTypeId = sqlDBR.GetUserReport_ReportTypeId(reportUserId);
            return GetReportType_ReportName(reportTypeId, nameType);
        }
        public string GetReportName(int reportUserId)
        {
            int nameStrId = sqlDBR.GetUserReport_ReportUserNameStrId(reportUserId);
            string returnValue = sqlDBR.GetString(nameStrId, CurrentLanguage);
            if (returnValue == "")
                return "Нет значения";
            else
                return returnValue;
        }
        public int GetReportPrice(int reportUserId)
        {
            return sqlDBR.GetUserReport_PRICE(reportUserId);
        }
        public DateTime GetReportDateCreate(int reportUserId)
        {
            string gettedVal = sqlDBR.GetUserReport_DateCreate(reportUserId);
            if (gettedVal == "")
                return new DateTime();
            else
                return DateTime.Parse(gettedVal);
        }
        public DateTime GetReportDateUpdate(int reportUserId)
        {
            string gettedVal = sqlDBR.GetUserReport_DateUpdate(reportUserId);
            if (gettedVal == "")
                return new DateTime();
            else
                return DateTime.Parse(gettedVal);
        }
        public string GetUserReport_Note(int reportUserId)
        {
            return sqlDBR.GetUserReport_Note(reportUserId);
        }
        public int GetUserReport_TypeID(int reportUserId)
        {
            return sqlDBR.GetUserReport_ReportTypeId(reportUserId);
        }
        public string GetUserReportTemplateName(int reportUserId)
        {
            return sqlDBR.GetUserReport_TemplateName(reportUserId);
        }
        //reportTypes
        /// <summary>
        /// Берем название типа отчета.  NameType:
        /// 0 - STRID_REPORT_NAME
        /// 1 - STRID_REPORT_SHORT_NAME
        /// 2 - STRID_REPORT_FULL_NAME
        /// 3 - STRID_REPORT_PRINT_NAME
        /// </summary>
        /// <param name="reportTypeId"></param>
        /// <param name="nameType"></param>
        /// <returns></returns>
        public string GetReportType_ReportName(int reportTypeId, int nameType)
        {
            int strId = 0;
            string returnValue = "";
            switch (nameType)
            {
                case 0:
                    strId = sqlDBR.GetReportType_ReportNameStrId(reportTypeId);
                    break;
                case 1:
                    strId = sqlDBR.GetReportType_ReportShortNameStrId(reportTypeId);
                    break;
                case 2:
                    strId = sqlDBR.GetReportType_ReportFullNameStrId(reportTypeId);
                    break;
                case 3:
                    strId = sqlDBR.GetReportType_ReportPrintNameStrId(reportTypeId);
                    break;
                default:
                    strId = -1;
                    break;
            }
            if (strId == 0)
                returnValue = "Нет значения";
            else
                if (strId > 0)
                    returnValue = sqlDBR.GetString(strId, CurrentLanguage);
                else
                    if (strId < 0)
                        returnValue = "Неправильно введен идентификатор названия";

            return returnValue;
        }
        public List<KeyValuePair<string, int>> GetAllReportTypes()
        {
            return sqlDBR.GetAllReportTypes(CurrentLanguage);
        }
        public int AddReportType(string reportName, string shortName, string fullName, string printName)
        {
            return sqlDBR.AddReportType(reportName, shortName, fullName, printName, CurrentLanguage);
        }
        public void EditReportType(int reportTypeId, string reportName, string shortName, string fullName, string printName)
        {
            sqlDBR.EditReportType(reportTypeId, reportName, shortName, fullName, printName, CurrentLanguage);
        }
        //FN_REPORT_USER_ORG
        public string GetReportUserOrg_BDATE(int reportId, int orgId)
        {
            string retVal = sqlDBR.GetReportUserOrg_BDATE(reportId, orgId);
            DateTime date = new DateTime();
            if (DateTime.TryParse(retVal, out date))
            {
                if (date == new DateTime())
                    retVal = "";
            }
            return retVal;
        }
        public string GetReportUserOrg_EDATE(int reportId, int orgId)
        {
            string retVal = sqlDBR.GetReportUserOrg_EDATE(reportId, orgId);
            DateTime date = new DateTime();
            if (DateTime.TryParse(retVal, out date))
            {
                if (date == new DateTime())
                    retVal = "";
            }
            return retVal;
        }
        public string GetReportUserOrg_SETDATE(int reportId, int orgId)
        {
            return sqlDBR.GetReportUserOrg_SETDATE(reportId, orgId);
        }
        public void AddOrSetReportUserOrg_ActivateReportForORG(int reportsUserId, int orgId, int curUserId)
        {
            sqlDBR.AddOrSetReportUserOrg_SETDATE(reportsUserId, orgId, true);
            if (curUserId > 0)
            {
                HistoryTable log = new HistoryTable(connectionString, CurrentLanguage, sqlDBR);
                string reportName = GetReportName(reportsUserId);

                log.AddHistoryRecord("fn_report_user_org", "REPORT_USER_ID", reportsUserId, curUserId, log.setReportUserOrg,
                    @"Report """ + reportName + @""" is active now", sqlDBR);
            }
        }
        public void AddOrSetReportUserOrg_DEactivateReportForORG(int reportsUserId, int orgId, int curUserId)
        {
            sqlDBR.AddOrSetReportUserOrg_SETDATE(reportsUserId, orgId, false);
        }
        //FN_REPORT_USER_ROLES
        public string GetReportUserRoles_BDATE(int reportId, int orgId, int roleId)
        {
            return sqlDBR.GetReportUserRoles_BDATE(reportId, orgId, roleId);
        }
        public string GetReportUserRoles_EDATE(int reportId, int orgId, int roleId)
        {
            return sqlDBR.GetReportUserRoles_EDATE(reportId, orgId, roleId);
        }
        public string GetReportUserRoles_SETDATE(int reportId, int orgId, int roleId)
        {
            string retVal = sqlDBR.GetReportUserRoles_SETDATE(reportId, orgId, roleId);
            DateTime date = new DateTime();
            if (DateTime.TryParse(retVal, out date))
            {
                if (date == new DateTime())
                    retVal = "";
            }
            return retVal;
        }
        public void AddOrSetReportUserRoles_SETDATE(int reportsUserId, int orgId, int userRoleId, bool IsActive, int curUserId)
        {
            sqlDBR.AddOrSetReportUserRoles_SETDATE(reportsUserId, orgId, userRoleId, IsActive);

            if (curUserId > 0) //Пока убрал, потому что слишком много обновлений за одну секунду(
            {
                string isItActive = "active";
                if (!IsActive)
                    isItActive = "not active";
                HistoryTable log = new HistoryTable(connectionString, CurrentLanguage, sqlDBR);
                string reportName = GetReportName(reportsUserId);
                //userrolename
                UsersTables usrt = new UsersTables(connectionString, CurrentLanguage, sqlDBR);
                string userRoleName = usrt.GetUserRoleName(userRoleId);

                HistoryWriter.Instance.AddHistoryRecord("fn_report_user_roles", "REPORT_USER_ID", reportsUserId, curUserId, log.setReportUserRoles,
                    @"Report """ + reportName + @"""" + " for user role " + userRoleName + " is " + isItActive + " now", sqlDBR);
            }
        }
    }
}
