using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Threading;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using System.IO;
using System.Messaging;
using System.Web.UI;
using System.Configuration;
using System.Collections.Generic;
using System.Net.Mail;
using BLL;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System.Threading;

namespace TestCacheTimeout
{
    /// <summary>
    /// Summary description for Global.
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        private const string DummyCacheItemKey = "GagaGuguGigi";

        public static ArrayList _JobQueue = new ArrayList();
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        public Global()
        {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e)
        {
            RegisterCacheEntry();
        }
        /// <summary>
        /// Register a cache entry which expires in 1 minute and gives us a callback.
        /// </summary>
        /// <returns></returns>
        private void RegisterCacheEntry()
        {
            int casheTime = Convert.ToInt32(ConfigurationSettings.AppSettings["ScheduleMailSendInterval"]);
            // Prevent duplicate key addition
            if (null != HttpContext.Current.Cache[DummyCacheItemKey]) return;

            HttpContext.Current.Cache.Add(DummyCacheItemKey, "Test", null, DateTime.MaxValue,
                TimeSpan.FromMinutes(casheTime), CacheItemPriority.NotRemovable,
                new CacheItemRemovedCallback(CacheItemRemovedCallback));
        }
        /// <summary>
        /// Callback method which gets invoked whenever the cache entry expires.
        /// We can do our "service" works here.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="reason"></param>
        public void CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            Debug.WriteLine("Cache item callback: " + DateTime.Now.ToString());
            try
            {
                // Do the service works
                DoWork();
            }
            finally
            {
                // We need to register another cache item which will expire again in one
                // minute. However, as this callback occurs without any HttpContext, we do not
                // have access to HttpContext and thus cannot access the Cache object. The
                // only way we can access HttpContext is when a request is being processed which
                // means a webpage is hit. So, we need to simulate a web page hit and then 
                // add the cache item.
                HitPage();
            }
        }
        /// <summary>
        /// Hits a local webpage in order to add another expiring item in cache
        /// </summary>
        private void HitPage()
        {
            string address = ConfigurationSettings.AppSettings["DummyPageAddress"];
            //string dummyPagePath = getFullUrlPath(address);
            WebClient client = new WebClient();
            client.DownloadData(address);
        }
        /// <summary>
        /// Asynchronously do the 'service' works
        /// </summary>
        private void DoWork()
        {
            Debug.WriteLine("Begin DoWork...");
            Debug.WriteLine("Running as: " + WindowsIdentity.GetCurrent().Name);
            DoSomeFileWritingStuff();
            new Thread(DoSheduleWork).Start();
            new Thread(CheckReminds).Start();

            Debug.WriteLine("End DoWork...");
        }
        /// <summary>
        /// Пишет время и даду каждый раз, когда выполняется в файл в корне
        /// </summary>
        private void DoSomeFileWritingStuff()
        {
            string address = "~/Cachecallback.txt";
            Debug.WriteLine("Writing to file...");
            string filePath = System.Web.Hosting.HostingEnvironment.MapPath(address);

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Cache Callback: {0}", DateTime.Now);
                    writer.Close();
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine(x);
            }
            Debug.WriteLine("File write successful");
        }
        /// <summary>
        /// Пишет время и даду каждый раз, когда выполняется в файл в корне
        /// </summary>
        private void DoSomeFileWritingStuff(string message)
        {
            string address = "~/Cachecallback.txt";
            Debug.WriteLine("Writing to file...");
            string filePath = System.Web.Hosting.HostingEnvironment.MapPath(address);

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Cache Callback: {0}", "Message: " + message);
                    writer.Close();
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine(x);
            }
            Debug.WriteLine("File write successful");
        }
        static readonly object locker = new object();
        /// <summary>
        /// Рассылка
        /// </summary>
        private void DoSheduleWork()
        {
            lock (locker)
            {
                string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
                DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
                try
                {
                    dataBlock.OpenConnection();
                    List<SingleEmailSchedule> shedulesToSend = dataBlock.emailScheduleTable.GetAllEmailShedules_ForSending();
                    foreach (SingleEmailSchedule shed in shedulesToSend)
                    {
                        SendReportByEmail(generateReport(shed.REPORT_ID, shed.CARD_ID, shed.USER_ID, dataBlock),
                            StiExportFormat.Pdf, shed.EMAIL_ADDRESS, "Рассылка SmartFis.ru local",
                            "Сообщение сгенерировано автоматически, в приложении отчет за период",
                            "Attachment");
                        dataBlock.emailScheduleTable.SetEmailSheduleLastSendDate(shed.EMAIL_SCHEDULE_ID);
                    }
                }
                catch (Exception x)
                {
                    DoSomeFileWritingStuff(x.Message);
                    Debug.WriteLine(x);
                }
                finally
                {
                    dataBlock.CloseConnection();
                }
            }
        }

        private void SendRemindMessage(string addr, string text)
        {
            MailMessage Message = new MailMessage();
            Message.Subject = "Напоминание SmartFIS";
            Message.Body = text;
            Message.To.Add(new MailAddress(addr));
            Message.From = new MailAddress(ConfigurationManager.AppSettings["DefaultEmailAddress"]);

            SmtpClient smtpClient = new SmtpClient();
            NetworkCredential basicCredential =
                new NetworkCredential("u274550", "67cd6ab5");
            MailMessage message = new MailMessage();

            smtpClient.Host = "smtp-19.1gb.ru";
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = basicCredential;

            smtpClient.Send(Message);
        }

        private List<int> FormDriversIdList(int remindId, DataBlock dataBlock)
        {
            List<int> result = new List<int>();
            int id = dataBlock.remindTable.GetRemindSource(remindId);
            int orgId = dataBlock.remindTable.GetRemindOrgId(remindId);
            int type = dataBlock.remindTable.GetRemindSourceType(remindId);
            switch (type)
            {
                case 0: { result.Add(id); break; }
                case 1:
                    {
                        result = dataBlock.cardsTable.GetAllCardIdsByGroupId(orgId, dataBlock.cardsTable.driversCardTypeId, id);
                        break;
                    }
                case 2:
                    {
                        List<int> groupIds = dataBlock.cardsTable.GetAllGroupIds(orgId);
                        foreach (int groupId in groupIds)
                        {
                            result.AddRange(dataBlock.cardsTable.GetAllCardIdsByGroupId(orgId, dataBlock.cardsTable.driversCardTypeId, groupId));
                        }
                        break;
                    }
            }
            return result;
        }

        /// <summary>
        /// Рассылка напоминаний
        /// </summary>
        private void CheckReminds()
        {
            lock (locker)
            {
                string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];

                DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
                try
                {
                    dataBlock.OpenConnection();

                    DateTime now = DateTime.Now;

                    //Processing every-hour reminds
                    int minute = Convert.ToInt32(ConfigurationSettings.AppSettings["ScheduleHourlyMailSendMinute"]);
                    if (now.Minute == minute)
                    {
                        List<int> hourIds = dataBlock.remindTable.GetAllHourRemindIds();

                        foreach (int id in hourIds)
                        {
                            //string addr = "shu.dv@tut.by";
                            string addr = "ai@programist.ru";

                            //NEW
                            //DateTime from = now.AddHours(-1);
                            //DateTime to = now;
                            DateTime from = new DateTime(2005, 12, 13);
                            DateTime to = new DateTime(2005, 12, 13);

                            List<int> cardIds = FormDriversIdList(id, dataBlock);
                            foreach (int cardId in cardIds)
                            {
                                List<PLFUnit.PLFRecord> records = new List<PLFUnit.PLFRecord>();
                                List<int> blockIds = dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId);
                                if (blockIds.Count == 0)
                                {
                                    continue;
                                }
                                DataSet dataset = ReportDataSetLoader.Get_PLF_ALLData(blockIds, new DateTime(from.Year, from.Month, from.Day), new DateTime(to.Year, to.Month, to.Day), cardId, 0, ref records);
                                StreamWriter wr = new StreamWriter("MailLog.txt", true);
                                string source = dataBlock.cardsTable.GetCardHolderNameByCardId(cardId);
                                switch (dataBlock.remindTable.GetRemindType(id))
                                {
                                    //SPEED
                                    case 1:
                                        {
                                            CriteriaTable oneCriteria = dataBlock.criteriaTable.LoadCriteria(8);
                                            string type = dataBlock.remindTable.GetRemindTypeName(dataBlock.remindTable.GetRemindType(id));
                                            string text = "Данное сообщение отправлено сервисом SmartFIS.\nПериодичность: каждый час.                                                        \nВодитель: " + source + ";\nТип напоминания: " + type +
                                                ";\nПериод анализа: " + from.ToString("yyyy-mm-dd hh:mm") + " - " + to.ToString("yyyy-mm-dd hh:mm") + ";\nНормативное значение параметра: " + oneCriteria.MaxValue;
                                            bool flag = false;
                                            foreach (PLFUnit.PLFRecord record in records)
                                            {
                                                if (Convert.ToDouble(record.SPEED.Replace('.', ',')) > oneCriteria.MaxValue)
                                                {
                                                    flag = true;
                                                    text = text + ";\n" + record.SYSTEM_TIME.systemTime + " - " + record.SPEED;
                                                }
                                            }
                                            if (flag)
                                            {
                                                SendRemindMessage(addr, text);
                                                dataBlock.remindTable.UpdateRemind(id, now);
                                                wr.WriteLine("Mail sent to " + addr + "; text:\n" + text);
                                            }

                                            break;
                                        }
                                    //RPM
                                    case 2:
                                        {
                                            CriteriaTable oneCriteria = dataBlock.criteriaTable.LoadCriteria(7);
                                            string type = dataBlock.remindTable.GetRemindTypeName(dataBlock.remindTable.GetRemindType(id));
                                            string text = "Данное сообщение отправлено сервисом SmartFIS.\nПериодичность: каждый час.                                                        \nВодитель: " + source + ";\nТип напоминания: " + type +
                                                ";\nПериод анализа: " + from.ToString("yyyy-mm-dd hh:mm") + " - " + to.ToString("yyyy-mm-dd hh:mm") + ";\nНормативное значение параметра: " + oneCriteria.MaxValue;
                                            bool flag = false;
                                            foreach (PLFUnit.PLFRecord record in records)
                                            {
                                                if (Convert.ToDouble(record.ENGINE_RPM.Replace('.', ',')) > oneCriteria.MaxValue)
                                                {
                                                    flag = true;
                                                    text = text + ";\n" + record.SYSTEM_TIME.systemTime + " - " + record.ENGINE_RPM;
                                                }
                                            }
                                            if (flag)
                                            {
                                                SendRemindMessage(addr, text);
                                                dataBlock.remindTable.UpdateRemind(id, now);
                                                wr.WriteLine("Mail sent to " + addr + "; text:\n" + text);
                                            }

                                            break;
                                        }
                                }
                                wr.Close();
                            }
                        }
                        //NEW
                    }
                    //Processing every-day reminds
                    string daytime = ConfigurationSettings.AppSettings["ScheduleDailyMailSendTime"];
                    if (now.TimeOfDay.ToString().Substring(0, 5) == daytime)
                    {
                        List<int> dayIds = dataBlock.remindTable.GetAllDayRemindIds();
                        StreamWriter wr = new StreamWriter("MailLog.txt", true);
                        foreach (int id in dayIds)
                        {
                            //string addr = "shu.dv@tut.by";
                            string addr = "ai@programist.ru";
                            string source = dataBlock.cardsTable.GetCardHolderNameByCardId(dataBlock.remindTable.GetRemindSource(id));
                            string type = dataBlock.remindTable.GetRemindTypeName(dataBlock.remindTable.GetRemindType(id));
                            string text = "Данное сообщение отправлено сервисом SmartFIS.\nПериодичность: каждый день.\nВодитель (группа): " + source + ";\nТип напоминания: " + type + ";";
                            //SendRemindMessage(addr, text);
                            //wr.WriteLine("Mail sent to " + addr + "; text:\n" + text);
                        }
                        wr.Close();
                    }
                    //Processing every-month reminds
                    int day = Convert.ToInt32(ConfigurationSettings.AppSettings["ScheduleMonthlyMailSendDay"]);
                    daytime = ConfigurationSettings.AppSettings["ScheduleMonthlyMailSendTime"];
                    if (now.Day == day && now.TimeOfDay.ToString().Substring(0, 5) == daytime)
                    {
                        List<int> monthIds = dataBlock.remindTable.GetAllMonthRemindIds();
                        StreamWriter wr = new StreamWriter("MailLog.txt", true);
                        foreach (int id in monthIds)
                        {
                            //string addr = "shu.dv@tut.by";
                            string addr = "ai@programist.ru";
                            string source = dataBlock.cardsTable.GetCardHolderNameByCardId(dataBlock.remindTable.GetRemindSource(id));
                            string type = dataBlock.remindTable.GetRemindTypeName(dataBlock.remindTable.GetRemindType(id));
                            string text = "Данное сообщение отправлено сервисом SmartFIS.\nПериодичность: каждый месяц.\nВодитель (группа): " + source + ";\nТип напоминания: " + type + ";";
                            //SendRemindMessage(addr, text);
                            //wr.WriteLine("Mail sent to " + addr + "; text:\n" + text);
                        }
                        wr.Close();
                    }

                    /*List<SingleEmailSchedule> shedulesToSend = dataBlock.emailScheduleTable.GetAllEmailShedules_ForSending();
                    foreach (SingleEmailSchedule shed in shedulesToSend)
                    {
                        SendReportByEmail(generateReport(shed.REPORT_ID, shed.CARD_ID, shed.USER_ID, dataBlock),
                            StiExportFormat.Pdf, shed.EMAIL_ADDRESS, "Рассылка SmartFis.ru local",
                            "Сообщение сгенерировано автоматически, в приложении отчет за период",
                            "Attachment");
                        dataBlock.emailScheduleTable.SetEmailSheduleLastSendDate(shed.EMAIL_SCHEDULE_ID);
                    }*/
                }
                catch (Exception x)
                {
                    Debug.WriteLine(x);
                }
                finally
                {
                    dataBlock.CloseConnection();
                }
            }
        }
        /// <summary>
        /// Генерирует отчет
        /// </summary>
        /// <param name="reportId">ID отчета</param>
        /// <param name="cardId">ID карты</param>
        /// <param name="dataBlock">DataBlock Для подключения к базе с ОТКРЫТЫМ подключением</param>
        /// <returns>сгенерированый отчет</returns>
        private StiReport generateReport(int reportId, int cardId, int userId, DataBlock dataBlock)
        {
            DataSet dataset = new DataSet();
            string templateName = dataBlock.reportsTable.GetUserReportTemplateName(reportId);
            templateName = "~/" + templateName;
            templateName = System.Web.Hosting.HostingEnvironment.MapPath(templateName);
            FileStream fs = new FileStream(templateName, FileMode.Open, FileAccess.Read);
            fs.Position = 0;
            StiReport report = new StiReport();
            report.Load(fs);

            if (templateName.Contains("DriverVehicleUsing.mrt"))
            {
                dataset = ReportDataSetLoader.Get_Driver_VehicleUsed(dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId),
                    DateTime.MinValue, DateTime.Now, cardId, userId);
            }
            if (templateName.Contains("DailyDriverActivityProtocol.mrt"))
            {
                dataset = ReportDataSetLoader.Get_Driver_DriverDailyActivityData(dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId),
                    DateTime.MinValue, DateTime.Now, cardId, userId);
            }
            if (templateName.Contains("WeeklyDriverActivityProtocol.mrt"))
            {
                dataset = ReportDataSetLoader.Get_Driver_DriverWeeklyActivityData(dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId),
                    DateTime.MinValue, DateTime.Now, cardId, userId);
            }
            if (templateName.Contains("PlfUnitsByDays.mrt"))
            {
                List<PLFUnit.PLFRecord> records = new List<PLFUnit.PLFRecord>();
                dataset = ReportDataSetLoader.Get_PlfUnitsByDays(dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId),
                    DateTime.MinValue, DateTime.Now, cardId, userId, ref records, "");
            }
            if (templateName.Contains("PlfFullCalendarReport.mrt"))
            {
                List<PLFUnit.PLFRecord> records = new List<PLFUnit.PLFRecord>();
                dataset = ReportDataSetLoader.Get_PLF_ALLData(dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId),
                    DateTime.MinValue, DateTime.Now, cardId, userId, ref records);
            }
            if (templateName.Contains("PlfUnitsEficienty.mrt"))
            {
                List<PLFUnit.PLFRecord> records = new List<PLFUnit.PLFRecord>();
                dataset = ReportDataSetLoader.Get_PlfUnitsEficienty(dataBlock.cardsTable.GetAllDataBlockIds_byCardId(cardId),
                    DateTime.MinValue, DateTime.Now, cardId, userId, ref records);
            }
            report.RegData(dataset);
            report.Render(new Stimulsoft.Report.Engine.StiRenderState(false));
            // report.Compile();
            return report;
        }
        /// <summary>
        /// Отправляет отчет по почте
        /// </summary>
        /// <param name="report">сгенерированый отчет</param>
        /// <param name="expFormat">формат отправки(Pdf Excel и др.)</param>
        /// <param name="mailTO">Кому отправить</param>
        /// <param name="mailSubject">Заголовок письма</param>
        /// <param name="mailBody">Тело письма</param>
        /// <param name="attachmentName">Имя вложения</param>
        /// <returns>Статус отправки</returns>
        public string SendReportByEmail(StiReport report, StiExportFormat expFormat, string mailTO, string mailSubject, string mailBody, string attachmentName)
        {
            MemoryStream stream = new MemoryStream();
            report.ExportDocument(expFormat, stream);
            string mimeString;

            MailMessage Message = new MailMessage();
            Message.Subject = mailSubject;
            Message.Body = mailBody;
            Message.To.Add(new MailAddress(mailTO));
            Message.From = new MailAddress(ConfigurationSettings.AppSettings["DefaultEmailAddress"]);

            switch (expFormat)
            {
                case StiExportFormat.Pdf:
                    {
                        mimeString = "application/pdf";
                        attachmentName = attachmentName + ".pdf";
                    } break;
                case StiExportFormat.Excel:
                    {
                        mimeString = "application/vnd.ms-excel";
                        attachmentName = attachmentName + ".xls";
                    } break;
                case StiExportFormat.Csv:
                    {
                        mimeString = "text/plain";
                        attachmentName = attachmentName + ".csv";
                    } break;
                case StiExportFormat.Rtf:
                    {
                        mimeString = "application/rtf";
                        attachmentName = attachmentName + ".rtf";
                    } break;
                case StiExportFormat.Text:
                    {
                        mimeString = "text/plain";
                        attachmentName = attachmentName + ".txt";
                    } break;
                case StiExportFormat.ImageJpeg:
                    {
                        mimeString = "image/jpeg";
                        attachmentName = attachmentName + ".jpg";
                    } break;
                default: { mimeString = "text/plain"; } break;
            }
            stream.Position = 0;
            Attachment attachment = new Attachment(stream, attachmentName, mimeString);
            Message.Attachments.Add(attachment);

            try
            {
                SmtpClient client = new SmtpClient("smtp-19.1gb.ru", 25);
                client.Credentials = new System.Net.NetworkCredential("u258981", "ab383f8e");
                //client.Timeout = 3000;
                client.Send(Message);
            }
            catch (Exception ex)
            {
                DoSomeFileWritingStuff(ex.Message);
                return ("Произошла ошибка, сообщение не отправлено.");
            }
            return "Успешно отправлено";
        }
        /// <summary>
        /// Получает путь к файлу на сервере
        /// </summary>
        /// <param name="address">Имя файла</param>
        /// <returns>Путь к файлу</returns>
        private string getFullUrlPath(string address)
        {
            try
            {
                if (HttpContext.Current == null)
                {
                    throw new Exception("Method must be called from a web context");
                }
                // string url = HttpContext.Current.Request.Url.ToString();
                // int index = url.IndexOf("DDDWebSite", 0, StringComparison.OrdinalIgnoreCase);
                // url = url.Substring(0, index)+"DDDWebSite/"+address;
                HttpServerUtility server = HttpContext.Current.Server;
                string filePath = server.MapPath(address);
                return filePath;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        protected void Session_Start(Object sender, EventArgs e)
        {

        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //string address = "WebForm1.aspx";
            string dummyPagePath = ConfigurationSettings.AppSettings["DummyPageAddress"];
            // If the dummy page is hit, then it means we want to add another item
            // in cache
            if (HttpContext.Current.Request.Url.ToString() == dummyPagePath)
            {
                // Add the item in cache and when succesful, do the work.
                RegisterCacheEntry();
            }
            //DoSheduleWork();
        }
        protected void Application_EndRequest(Object sender, EventArgs e)
        {

        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
        }
        protected void Application_Error(Object sender, EventArgs e)
        {
            Debug.WriteLine(Server.GetLastError());
            DoSomeFileWritingStuff(Server.GetLastError().Message);
        }
        protected void Session_End(Object sender, EventArgs e)
        {

        }
        protected void Application_End(Object sender, EventArgs e)
        {

        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
        }
        #endregion
    }
}

