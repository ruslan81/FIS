using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BLL;
using System.Data;
using System.IO;
using DevExpress.XtraReports.UI;
using System.Drawing.Imaging;

/// <summary>
/// Service class of downloading files
/// </summary>
public partial class Administrator_download : System.Web.UI.Page
{
    /// <summary>
    /// Section "Восстановить у пользователя"
    /// </summary>
    public const string DOWNLOAD_FILE_RECOVER_USER = "RecoverUserDownload";

    /// <summary>
    /// Section "PLF Файлы"
    /// </summary>
    public const string GET_REPORT = "GetReport";

    protected void Page_Load(object sender, EventArgs e)
    {
        string Type=Request.Form.Get("type");

        //Section "Восстановить у пользователя"
        if (Type == DOWNLOAD_FILE_RECOVER_USER)
        {
            if (Request.Form.Get("dataBlockId") != null)
            {
                int dataBlockId = Convert.ToInt32(Request.Form.Get("dataBlockId"));
                string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
                DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
                dataBlock.OpenConnection();
                byte[] fileBytes = dataBlock.GetDataBlock_BytesArray(dataBlockId);
                string fileName = dataBlock.GetDataBlock_FileName(dataBlockId);
                dataBlock.CloseConnection();

                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.AddHeader("Content-Length", fileBytes.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
                Response.End();
            }
        }

        //Section "PLF Файлы"
        if (Type == GET_REPORT)
        {
            string CardID=Request.Form.Get("CardID");
            string PLFID=Request.Form.Get("PLFID");
            string UserName = Request.Form.Get("UserName");
            string Format = Request.Form.Get("Format");
            string ReportType = Request.Form.Get("ReportType");
            if (string.IsNullOrEmpty(ReportType))
            {
                ReportType = "FullReport";
            }

            int dataBlockId = int.Parse(PLFID);
            List<int> dataBlockIDS = new List<int>();
            dataBlockIDS.Add(dataBlockId);
            int cardID = int.Parse(CardID);

            string connectionString = System.Configuration.ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
            BLL.DataBlock dataBlock = new BLL.DataBlock(connectionString, "STRING_EN");
            dataBlock.OpenConnection();

            DateTime from = new DateTime();
            from = dataBlock.plfUnitInfo.Get_START_PERIOD(dataBlockId);

            DateTime to = new DateTime();
            to = dataBlock.plfUnitInfo.Get_END_PERIOD(dataBlockId);

            string vehicle = dataBlock.plfUnitInfo.Get_VEHICLE(dataBlockId);
            string deviceID = dataBlock.plfUnitInfo.Get_ID_DEVICE(dataBlockId);

            DataSet dataset = new DataSet();

            int userId = dataBlock.usersTable.Get_UserID_byName(UserName);

            List<PLFUnit.PLFRecord> records = new List<PLFUnit.PLFRecord>();
            dataset = ReportDataSetLoader.Get_PLF_ALLData(dataBlockIDS,
                new DateTime(from.Year, from.Month, from.Day), new DateTime(to.Year, to.Month, to.Day),
                cardID, userId, ref records);

            dataBlock.CloseConnection();

            //gets table PlfHeader_1
            DataTable dt = dataset.Tables[0];
            //gets the first row
            DataRow dr = dt.Rows[0];
            string driverName = dr["Имя водителя"].ToString();

            //load needed template
            string path = HttpContext.Current.Server.MapPath("~/templates") + "\\";
            XtraReport report = new XtraReport();
            report.LoadLayout(path + ReportType+".repx");
            report.DataSource = dataset;
            MemoryStream reportStream = new MemoryStream();
            switch (Format)
            {
                case "html": report.ExportToHtml(reportStream); break;
                case "pdf": report.ExportToPdf(reportStream); break;
                case "rtf": report.ExportToRtf(reportStream); break;
                case "png": report.ExportToImage(reportStream,ImageFormat.Png); break;
            }
            

            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + "Отчет "+driverName+" "+
                new DateTime(from.Year, from.Month, from.Day).ToString("dd_MM_yyyy") + "-" + new DateTime(to.Year, to.Month, to.Day).ToString("dd_MM_yyyy")+"."+Format);
            Response.AddHeader("Content-Length", reportStream.GetBuffer().Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(reportStream.GetBuffer(), 0, reportStream.GetBuffer().Length);
            Response.End();
        }
    }
}