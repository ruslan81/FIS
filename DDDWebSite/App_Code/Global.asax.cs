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
			if( null != HttpContext.Current.Cache[ DummyCacheItemKey ] ) return;

			HttpContext.Current.Cache.Add( DummyCacheItemKey, "Test", null, DateTime.MaxValue,
                TimeSpan.FromMinutes(casheTime), CacheItemPriority.NotRemovable,
				new CacheItemRemovedCallback( CacheItemRemovedCallback ) );
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
			Debug.WriteLine("Cache item callback: " + DateTime.Now.ToString() );
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
			Debug.WriteLine("Running as: " + WindowsIdentity.GetCurrent().Name );
			DoSomeFileWritingStuff();
            new Thread(DoSheduleWork).Start();

			Debug.WriteLine("End DoWork...");
		}
		/// <summary>
		/// ����� ����� � ���� ������ ���, ����� ����������� � ���� � �����
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
			catch( Exception x )
			{
				Debug.WriteLine( x );
			}
			Debug.WriteLine("File write successful");
		}
        /// <summary>
        /// ����� ����� � ���� ������ ���, ����� ����������� � ���� � �����
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
		/// ��������
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
                            StiExportFormat.Pdf, shed.EMAIL_ADDRESS, "�������� SmartFis.ru local",
                            "��������� ������������� �������������, � ���������� ����� �� ������",
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
        /// <summary>
        /// ���������� �����
        /// </summary>
        /// <param name="reportId">ID ������</param>
        /// <param name="cardId">ID �����</param>
        /// <param name="dataBlock">DataBlock ��� ����������� � ���� � �������� ������������</param>
        /// <returns>�������������� �����</returns>
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
        /// ���������� ����� �� �����
        /// </summary>
        /// <param name="report">�������������� �����</param>
        /// <param name="expFormat">������ ��������(Pdf Excel � ��.)</param>
        /// <param name="mailTO">���� ���������</param>
        /// <param name="mailSubject">��������� ������</param>
        /// <param name="mailBody">���� ������</param>
        /// <param name="attachmentName">��� ��������</param>
        /// <returns>������ ��������</returns>
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
                return ("��������� ������, ��������� �� ����������.");
            }
            return "������� ����������";
        }
        /// <summary>
        /// �������� ���� � ����� �� �������
        /// </summary>
        /// <param name="address">��� �����</param>
        /// <returns>���� � �����</returns>
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
			Debug.WriteLine( Server.GetLastError() );
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
