using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// Работа с таблицами счетов.
    /// </summary>
    public class InvoiceTable
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
        SQLDB sqlDBR;

        /// <summary>
        /// Статус платежа - оплачено
        /// </summary>
        public int Status_Paid { get; set; }
        /// <summary>
        /// Статус платежа - не оплачено
        /// </summary>
        public int Status_NotPaid { get; set; }

        /// <summary>
        /// ДОбавление допольнительного типа отчета
        /// </summary>
        public int InvoiceType_addReportInvoice { get; set; }
        /// <summary>
        /// Добавление дополнительного пользователя в систему
        /// </summary>
        public int InvoiceType_moreUsers { get; set; }
        /// <summary>
        /// Абонентская плата
        /// </summary>
        public int InvoiceType_licenseFee { get; set; }
       

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
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="connectionsStringTMP">строка подключение(необязательно ее указывать)</param>
        /// <param name="Current_Language">Язык</param>
        /// <param name="sql">обьект SQLDB</param>
        public InvoiceTable(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            //тест
            // sqlDBR = new SQLDB(connectionString);
            sqlDBR = sql;
            //
            string LanguageForStatus = "STRING_EN";
            sqlDBR.OpenConnection();
            Status_Paid = AddOrGetInvoiceStatusId("Paid", LanguageForStatus);
            Status_NotPaid = AddOrGetInvoiceStatusId("Not paid", LanguageForStatus);
            InvoiceType_addReportInvoice = AddOrGetInvoiceTypeId("Additional report invoice", LanguageForStatus);
            InvoiceType_moreUsers = AddOrGetInvoiceTypeId("Additional users invoice", LanguageForStatus);
            InvoiceType_licenseFee = AddOrGetInvoiceTypeId("License fee", LanguageForStatus);
            sqlDBR.CloseConnection();
        }

        //FN_INVOICE
        /// <summary>
        /// Перводит счет из состояния неоплаченный в оплаченный
        /// </summary>
        /// <param name="invoiceId">ID счета</param>
        public void PayABill(int invoiceId)
        {
            sqlDBR.UpdateInvoice(invoiceId, Status_Paid, DateTime.Now);
        }
        /// <summary>
        /// Перводит счет из состояния оплаченный в неоплаченный
        /// </summary>
        /// <param name="invoiceId">ID счета</param>
        public void UnPayABill(int invoiceId)
        {
            sqlDBR.UpdateInvoice(invoiceId, Status_NotPaid, new DateTime());
        }
        /// <summary>
        /// Получить все счета
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <returns>Массив ID счетов</returns>
        public List<int> GetAllInvoices(int orgId)
        {
            return sqlDBR.GetAllInvoices(orgId);
        }
        /// <summary>
        /// Выставить новый счет
        /// </summary>
        /// <param name="invoiceTypeId">ID типа счета</param>
        /// <param name="invoiceStatusId">ID статуса счета</param>
        /// <param name="orgId">ID организации</param>
        /// <param name="BillName">Название счета</param>
        /// <param name="dateInvoice">Дата выставления счета</param>
        /// <param name="datePaymentTerm">Оплатить до...</param>
        /// <param name="datePayment">Дата оплаты(new DateTime() если еще не оплачено)</param>
        /// <returns>ID счета</returns>
        public int AddInvoice(int invoiceTypeId, int invoiceStatusId, int orgId, string BillName, DateTime dateInvoice, DateTime datePaymentTerm, DateTime datePayment)
        {
            return sqlDBR.AddInvoice(invoiceTypeId, invoiceStatusId, orgId, BillName, dateInvoice, datePaymentTerm, datePayment, CurrentLanguage);
        }
        /// <summary>
        /// Получить имя счета
        /// </summary>
        /// <param name="invoiceId">ID счета</param>
        /// <returns>Имя счета</returns>
        public string GetInvoiceName(int invoiceId)
        {
            int strid = sqlDBR.GetInvoice_BillNameStrId(invoiceId);
            return sqlDBR.GetString(strid, CurrentLanguage);
        }
        /// <summary>
        /// Получить дату выставления счета
        /// </summary>
        /// <param name="invoiceId">ID счета</param>
        /// <returns>Дата выставления счета</returns>
        public DateTime GetDateInvoice(int invoiceId)
        {
            string gettedVal = sqlDBR.GetInvoice_Date(invoiceId);
            if (gettedVal == "")
                return new DateTime();
            else
                return DateTime.Parse(gettedVal);
        }
        /// <summary>
        /// Получить дату, до которой оплатить счет.
        /// </summary>
        /// <param name="invoiceId">ID счета</param>
        /// <returns>Дата, до которой оплатить счет.</returns>
        public DateTime GetDatePaymentTerm(int invoiceId)
        {
            string gettedVal = sqlDBR.GetInvoice_DatePaymentTerm(invoiceId);
            if (gettedVal == "")
                return new DateTime();
            else
                return DateTime.Parse(gettedVal);
        }
        /// <summary>
        /// Получить дату оплаты счета
        /// </summary>
        /// <param name="invoiceId">ID счета</param>
        /// <returns>Тут возвращается строка, потому что даты оплаты может не быть.
        /// Тогда Делаем обработку If(date=="") то пишем НЕ ОПЛАЧЕНО или типа того.
        /// Можно переделать на Nullable DateTime (DateTime?), тогда можно будет возвращать Null если ничего нет.
        /// </returns>
        public string GetDatePayment(int invoiceId)//
        {
            string retVal = sqlDBR.GetInvoice_DatePayment(invoiceId);
            DateTime date = new DateTime();
            retVal.Trim();//???
            if (DateTime.TryParse(retVal, out date))
            {
                if (date == new DateTime())
                    retVal = "";
            }
            return retVal;
        }
        /// <summary>
        /// Получить тип счета
        /// </summary>
        /// <param name="invoiceId">ID счета</param>
        /// <returns>Название типа счета</returns>
        public string GetInvoiceType(int invoiceId)
        {
            int invoiceTypeId = sqlDBR.GetInvoice_TypeId(invoiceId);
            int typeSTRID = sqlDBR.GetInvoiceTypeNameStrId(invoiceTypeId);
            return sqlDBR.GetString(typeSTRID, CurrentLanguage);
        }
        /// <summary>
        /// Получить статус счета
        /// </summary>
        /// <param name="invoiceId">ID счета</param>
        /// <returns>Статус счета</returns>
        public string GetInvoiceStatus(int invoiceId)
        {
           int invoiceStatusId = sqlDBR.GetInvoice_StatusId(invoiceId);
           int statusSTRID = sqlDBR.GetInvoiceStatusNameStrId(invoiceStatusId);
           return sqlDBR.GetString(statusSTRID, CurrentLanguage);
        }
        /// <summary>
        /// Получить ID статуса счета
        /// </summary>
        /// <param name="invoiceId">ID счета</param>
        /// <returns>Статус счета</returns>
        public int GetInvoiceStatusId(int invoiceId)
        {
            return sqlDBR.GetInvoice_StatusId(invoiceId);
        }
        /// <summary>
        /// Получить имя статуса счета
        /// </summary>
        /// <param name="invoiceId">ID статуса счета</param>
        /// <returns>Имя статус счета</returns>
        public string GetInvoiceStatusName(int invoiceStatusId)
        {
            return sqlDBR.GetInvoiceStatusName(invoiceStatusId);
        }
        /// <summary>
        /// Получить возможные статусы счетов
        /// </summary>
        /// <returns>Статусы счетов</returns>
        public List<Int32> GetAllInvoiceStatuses()
        {
            return sqlDBR.GetAllInvoiceStatuses();
        }

        //FD_INVOICE_STATUS
        /// <summary>
        /// Добавляет либо получает ID существуещего статуса счета, с языком по-умолчанию(английский)
        /// </summary>
        /// <param name="statusString">Название статуса счета</param>
        /// <returns>ID статуса счета</returns>
        private int AddOrGetInvoiceStatusId(string statusString)
        {
            return AddOrGetInvoiceStatusId(statusString, CurrentLanguage);
        }
        /// <summary>
        /// Добавляет либо получает ID существуещего статуса счета
        /// </summary>
        /// <param name="statusString">Название статуса счета</param>
        /// <param name="Language">Язык</param>
        /// <returns>ID статуса счета</returns>
        private int AddOrGetInvoiceStatusId(string statusString, string Language)
        {
            int stringId = sqlDBR.GetStringId(statusString, Language);
            int statusId = -1;
            if (stringId > 0)
                statusId = sqlDBR.GetInvoiceStatusId(stringId);
            if (stringId == 0 || statusId == 0)
                statusId = sqlDBR.AddInvoiceStatus(statusString, Language);
            return statusId;
        }

        //FD_INVOICE_TYPE
        /// <summary>
        /// Добавить или получить существующий тип счета с языком по-умолчанию(английский).
        /// </summary>
        /// <param name="typeString">Название типа счета</param>
        /// <returns>ID типа счета</returns>
        private int AddOrGetInvoiceTypeId(string typeString)
        {
            return AddOrGetInvoiceTypeId(typeString, CurrentLanguage);
        }
        /// <summary>
        /// Добавить или получить существующий тип счета.
        /// </summary>
        /// <param name="typeString">Название типа счета</param>
        /// <param name="Language">Язык</param>
        /// <returns>ID типа счета</returns>
        private int AddOrGetInvoiceTypeId(string typeString, string Language)
        {
            int stringId = sqlDBR.GetStringId(typeString, Language);
            int typeId = -1;
            if (stringId > 0)
                typeId = sqlDBR.GetInvoiceTypeId(stringId);
            if(stringId==0 || typeId==0)
                typeId = sqlDBR.AddInvoiceType(typeString, Language);
            return typeId;
        }
    }
}
