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
        /*if (Type == GET_REPORT)
        {
            String CardID=Request.Form.Get("CardID");
            String PLFID=Request.Form.Get("PLFID");
            String UserName = Request.Form.Get("UserName");

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
            string dateFrom = dr["С"].ToString();
            string dateTo = dr["По"].ToString();
            string regNumber = dr["Регистрационный номер"].ToString();
            string userName = dr["Имя пользователя"].ToString();
            string driverNumber = dr["Номер водителя"].ToString();
            string driverName = dr["Имя водителя"].ToString();
            string orgName = dr["Название организации"].ToString();
            string deviceNumber = dr["Номер бортового устройства"].ToString();
            string logoPath = dr["Путь к фото"].ToString();

            //gets table PLFReport_FullCalendar_Totals
            DataTable dtTotals = dataset.Tables[2];
            //gets the first row
            DataRow drTotals = dtTotals.Rows[0];

            TimeSpan tTotalWorkTime = (TimeSpan)drTotals["Суммарное время работы"];
            string totalWorkTime = String.Format("{0:00}", (int)tTotalWorkTime.TotalHours) + ":" + String.Format("{0:00}", tTotalWorkTime.Minutes)+ ":" + String.Format("{0:00}", tTotalWorkTime.Seconds);

            TimeSpan tMotorWorkTime = (TimeSpan)drTotals["Время работы двигателя"];
            string motorWorkTime = String.Format("{0:00}", (int)tMotorWorkTime.TotalHours) + ":" + String.Format("{0:00}", tMotorWorkTime.Minutes) + ":" + String.Format("{0:00}", tMotorWorkTime.Seconds);

            TimeSpan tMovement = (TimeSpan)drTotals["Время движения"];
            string movement = String.Format("{0:00}", (int)tMovement.TotalHours) + ":" + String.Format("{0:00}", tMovement.Minutes) + ":" + String.Format("{0:00}", tMovement.Seconds);

            TimeSpan tMaxMovement = (TimeSpan)drTotals["Максимальное непрерывное время движения"];
            string maxMovement = String.Format("{0:00}", (int)tMaxMovement.TotalHours) + ":" + String.Format("{0:00}", tMaxMovement.Minutes) + ":" + String.Format("{0:00}", tMaxMovement.Seconds);

            TimeSpan tDowntime = (TimeSpan)drTotals["Время простоя с заведенным двигателем"];
            string downtime = String.Format("{0:00}", (int)tDowntime.TotalHours) + ":" + String.Format("{0:00}", tDowntime.Minutes) + ":" + String.Format("{0:00}", tDowntime.Seconds);

            TimeSpan tMaxDowntime = (TimeSpan)drTotals["Максимальное непрерывное время простоя"];
            string maxDowntime = String.Format("{0:00}", (int)tMaxDowntime.TotalHours) + ":" + String.Format("{0:00}", tMaxDowntime.Minutes) + ":" + String.Format("{0:00}", tMaxDowntime.Seconds);

            string maxDistance = drTotals["Максимальный непрерывный пройденный путь"].ToString();
            double startFuelVolume = (double)drTotals["Объем топлива в баках на начало периода"];
            double finishFuelVolume = (double)drTotals["Объем топлива в баках на конец периода"];
            string totalRefills = drTotals["Количество заправок"].ToString();
            string totalDropout = drTotals["Количество возможных сливов"].ToString();
            double totalRefillsVolume = (double)drTotals["Всего заправлено топлива"];
            double totalDropoutVolume = (double)drTotals["Всего возможно слито топлива"];

            //gets table PLFReport_FullCalendar_Refills
            DataTable dtRefills = dataset.Tables[3];

            double distance = 0;

            double averageSpeed = 0;
            double maxSpeed = 0;

            double maxFuelValue = 0;
            double minFuelValue = double.Parse(records[0].FUEL_VOLUME1);

            double maxRPM = 0;
            double minRPM = double.PositiveInfinity;
            double averageRPM = 0;
            double sumRPM = 0;
            int notZeroRPMCounter = 0;

            double maxVoltage = 0;
            double minVoltage = double.Parse(records[0].VOLTAGE, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            double averageVoltage = 0;

            foreach (PLFUnit.PLFRecord record in records)
            {
                distance += double.Parse(record.DISTANCE_COUNTER, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                double speed = double.Parse(record.SPEED, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                if (speed > maxSpeed)
                {
                    maxSpeed = speed;
                }

                double fuelValue = double.Parse(record.FUEL_VOLUME1);
                if (fuelValue > maxFuelValue)
                {
                    maxFuelValue = fuelValue;
                }
                if (fuelValue < minFuelValue)
                {
                    minFuelValue = fuelValue;
                }

                double RPM = double.Parse(record.ENGINE_RPM);
                if (RPM > maxRPM)
                {
                    maxRPM = RPM;
                }
                if (RPM > 0)
                {
                    notZeroRPMCounter++;
                    sumRPM += RPM;
                    if (RPM < minRPM)
                    {
                        minRPM = RPM;
                    }
                }

                double voltage = double.Parse(record.VOLTAGE, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
                if (voltage > maxVoltage)
                {
                    maxVoltage = voltage;
                }
                if (voltage < minVoltage)
                {
                    minVoltage = voltage;
                }

                averageVoltage += voltage / records.Count;
            }

            averageSpeed = distance / tMovement.TotalSeconds * 3600;

            averageRPM = sumRPM / notZeroRPMCounter;//timeMovement.TotalSeconds * 60;

            // step 1: creation of a document-object
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 50, 50, 70, 50);

            MemoryStream ms = new MemoryStream();
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);
            //document.Open();

            //load needed fonts
            string path = HttpContext.Current.Server.MapPath("~/fonts") + "\\";
            iTextSharp.text.pdf.BaseFont tahoma = iTextSharp.text.pdf.BaseFont.CreateFont(path + "tahoma.ttf", System.Text.Encoding.GetEncoding(1251).BodyName, true);

            writer.ViewerPreferences = iTextSharp.text.pdf.PdfWriter.PageModeUseOutlines;

            // Our custom Header and Footer is done using Event Handler
            PageHeaderFooter PageEventHandler = new PageHeaderFooter();
            writer.PageEvent = PageEventHandler;

            // Define the page header
            PageEventHandler.Title = "Отчет предоставлен системой SmartFIS";
            PageEventHandler.TitleSubject = "Полный отчет за календарный период";

            //colors for values
            iTextSharp.text.BaseColor normalColor = new iTextSharp.text.BaseColor(45, 70, 155);
            iTextSharp.text.BaseColor alertColor = new iTextSharp.text.BaseColor(96, 0, 0);
            iTextSharp.text.BaseColor textColor = new iTextSharp.text.BaseColor(50, 50, 50);
            iTextSharp.text.BaseColor borderColor = new iTextSharp.text.BaseColor(200, 200, 200);

            document.Open();

            //--- Title section ---

            //title
            iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("Полный отчет за календарный период",
                 new iTextSharp.text.Font(tahoma, 22, iTextSharp.text.Font.BOLD, normalColor)));
            p.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            document.Add(p);

            //title fields
            p = new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("Организация: ",
                new iTextSharp.text.Font(tahoma, 12, iTextSharp.text.Font.NORMAL, textColor)));
            p.Add(new iTextSharp.text.Chunk(orgName,
                new iTextSharp.text.Font(tahoma, 12, iTextSharp.text.Font.BOLD, normalColor)));
            p.SpacingBefore = 20;
            document.Add(p);

            p = new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("Водитель: ",
                new iTextSharp.text.Font(tahoma, 12, iTextSharp.text.Font.NORMAL, textColor)));
            p.Add(new iTextSharp.text.Chunk(driverName + " / " + driverNumber,
                new iTextSharp.text.Font(tahoma, 12, iTextSharp.text.Font.BOLD, normalColor)));
            document.Add(p);

            p = new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("Транспортное средство: ",
                new iTextSharp.text.Font(tahoma, 12, iTextSharp.text.Font.NORMAL, textColor)));
            p.Add(new iTextSharp.text.Chunk(vehicle + " / " + deviceID,
                new iTextSharp.text.Font(tahoma, 12, iTextSharp.text.Font.BOLD, normalColor)));
            document.Add(p);

            //--- "Период" section ---

            p = new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("Период ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.BOLD, textColor)));
            p.SpacingBefore = 10;
            document.Add(p);

            iTextSharp.text.pdf.PdfPTable aTable = new iTextSharp.text.pdf.PdfPTable(2);    //2 columns

            aTable.SpacingBefore = 10f;
            aTable.SpacingAfter = 10f;
            aTable.TotalWidth = 500f;
            aTable.LockedWidth = true;
            writer.DirectContent.SetLineWidth(1f);
            writer.DirectContent.SetLineDash(0f);

            iTextSharp.text.pdf.PdfPCell cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Начало периода: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(dateFrom,
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Конец периода: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(dateTo,
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            document.Add(aTable);

            //--- "Работа ТС" section ---

            p = new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("Работа ТС ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.BOLD, textColor)));
            document.Add(p);

            aTable = new iTextSharp.text.pdf.PdfPTable(2);    //2 columns

            aTable.SpacingBefore = 10f;
            aTable.SpacingAfter = 10f;
            aTable.TotalWidth = 500f;
            aTable.LockedWidth = true;
            writer.DirectContent.SetLineWidth(1f);
            writer.DirectContent.SetLineDash(0f);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Начало работы: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(records[0].SYSTEM_TIME.GetSystemTime().ToString("dd.MM.yyyy HH:mm:ss"),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Окончание работы: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(records[records.Count - 1].SYSTEM_TIME.GetSystemTime().ToString("dd.MM.yyyy HH:mm:ss"),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Продолжительность периода: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            TimeSpan diff = records[records.Count - 1].SYSTEM_TIME.GetSystemTime().Subtract(records[0].SYSTEM_TIME.GetSystemTime());
            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0}",(int)diff.TotalHours)+":"+String.Format("{0:00}",diff.Minutes)+":"+String.Format("{0:00}",diff.Seconds),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Суммарное время работы: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(totalWorkTime,
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Время работы двигателя: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(motorWorkTime,
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Время движения: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(movement,
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Максимальное непрерывное время движения: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(maxMovement,
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Время простоя с заведенным двигателем: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(downtime,
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Максимальное непрерывное время простоя: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(maxDowntime,
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            document.Add(aTable);

            //--- "Путь и скорость" section ---

            p = new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("Путь и скорость ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.BOLD, textColor)));
            document.Add(p);

            aTable = new iTextSharp.text.pdf.PdfPTable(2);    //2 columns

            aTable.SpacingBefore = 10f;
            aTable.SpacingAfter = 10f;
            aTable.TotalWidth = 500f;
            aTable.LockedWidth = true;
            writer.DirectContent.SetLineWidth(1f);
            writer.DirectContent.SetLineDash(0f);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Пройденный путь, км: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}",distance),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Максимальный непрерывный пройденный путь, км: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}",maxDistance),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Средняя скорость за время движения, км/ч: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}", averageSpeed),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Максимальная скорость за время движения, км/ч: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}", maxSpeed),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            document.Add(aTable);


            //--- "Обороты двигателя" section ---

            p = new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("Обороты двигателя ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.BOLD, textColor)));
            document.Add(p);

            aTable = new iTextSharp.text.pdf.PdfPTable(2);    //2 columns

            aTable.SpacingBefore = 10f;
            aTable.SpacingAfter = 10f;
            aTable.TotalWidth = 500f;
            aTable.LockedWidth = true;
            writer.DirectContent.SetLineWidth(1f);
            writer.DirectContent.SetLineDash(0f);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Средние обороты двигателя за время движения, об/мин: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0}", averageRPM),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Максимальные обороты двигателя за время движения, об/мин: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0}", maxRPM),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Минимальные обороты двигателя за время движения, об/мин: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0}", minRPM),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            document.Add(aTable);


            //--- "Напряжение бортовой сети" section ---

            p = new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("Напряжение бортовой сети ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.BOLD, textColor)));
            document.Add(p);

            aTable = new iTextSharp.text.pdf.PdfPTable(2);    //2 columns

            aTable.SpacingBefore = 10f;
            aTable.SpacingAfter = 10f;
            aTable.TotalWidth = 500f;
            aTable.LockedWidth = true;
            writer.DirectContent.SetLineWidth(1f);
            writer.DirectContent.SetLineDash(0f);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Среднее напряжение бортсети, В: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.0}", averageVoltage),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Максимальное напряжение бортсети, В: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.0}", maxVoltage),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Минимальное напряжение бортсети, В: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.0}", minVoltage),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            document.Add(aTable);


            //--- "Топливо" section ---

            p = new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("Топливо ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.BOLD, textColor)));
            document.Add(p);

            aTable = new iTextSharp.text.pdf.PdfPTable(2);    //2 columns

            aTable.SpacingBefore = 10f;
            aTable.SpacingAfter = 10f;
            aTable.TotalWidth = 500f;
            aTable.LockedWidth = true;
            writer.DirectContent.SetLineWidth(1f);
            writer.DirectContent.SetLineDash(0f);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Объем топлива в баках на начало периода, л: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}", startFuelVolume),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Объем топлива в баках на конец периода, л: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}", finishFuelVolume),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Максимальный объем топлива в баках, л: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}", maxFuelValue),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Минимальный  объем топлива в баках, л: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}", minFuelValue),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Количество заправок: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(totalRefills,
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Всего заправлено топлива, л: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}", totalRefillsVolume),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Количество возможных сливов: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(totalDropout,
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Всего возможно слито топлива, л: ",
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}", totalDropoutVolume),
                new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, normalColor)));
            cell.BorderColor = borderColor;
            aTable.AddCell(cell);

            document.Add(aTable);


            //--- "Детально о топливе" section ---
            if (dtRefills.Rows.Count > 0)
            {
                p = new iTextSharp.text.Paragraph(new iTextSharp.text.Chunk("Детально о топливе",
                    new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.BOLD, textColor)));
                document.Add(p);

                aTable = new iTextSharp.text.pdf.PdfPTable(6);    //6 columns

                aTable.SpacingBefore = 10f;
                aTable.SpacingAfter = 10f;
                aTable.TotalWidth = 500f;
                aTable.LockedWidth = true;
                writer.DirectContent.SetLineWidth(1f);
                writer.DirectContent.SetLineDash(0f);

                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Действие",
                        new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
                cell.BorderColor = borderColor;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(210, 210, 210);
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                aTable.AddCell(cell);

                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Начальное время",
                    new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
                cell.BorderColor = borderColor;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(210, 210, 210);
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                aTable.AddCell(cell);

                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Конечное время",
                    new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
                cell.BorderColor = borderColor;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(210, 210, 210);
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                aTable.AddCell(cell);

                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Начальный объем, л",
                    new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
                cell.BorderColor = borderColor;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(210, 210, 210);
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                aTable.AddCell(cell);

                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Конечный объем, л",
                    new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
                cell.BorderColor = borderColor;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(210, 210, 210);
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                aTable.AddCell(cell);

                cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase("Объем, л",
                    new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, textColor)));
                cell.BorderColor = borderColor;
                cell.BackgroundColor = new iTextSharp.text.BaseColor(210, 210, 210);
                cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                aTable.AddCell(cell);

                const string DROPOUT_ACTION = "Возможный слив";

                foreach (DataRow row in dtRefills.Rows)
                {
                    string action = row["Заправка/Слив"].ToString();
                    DateTime sDateTime = (DateTime)row["НачВремя"];
                    DateTime fDateTime = (DateTime)row["КонВремя"];
                    double sFuelVolume = (double)row["НачОбъем"];
                    double fFuelVolume = (double)row["КонОбъем"];
                    double diffVolume = fFuelVolume - sFuelVolume;

                    iTextSharp.text.BaseColor currColor;
                    if (action.Equals(DROPOUT_ACTION))
                    {
                        currColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    }
                    else
                    {
                        currColor = normalColor;
                    }

                    cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(action,
                        new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, currColor)));
                    cell.BorderColor = borderColor;
                    if (action.Equals(DROPOUT_ACTION))
                        cell.BackgroundColor = alertColor;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    aTable.AddCell(cell);

                    cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(sDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                        new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, currColor)));
                    cell.BorderColor = borderColor;
                    if (action.Equals(DROPOUT_ACTION))
                        cell.BackgroundColor = alertColor;
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    aTable.AddCell(cell);

                    cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(fDateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                        new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, currColor)));
                    cell.BorderColor = borderColor;
                    if (action.Equals(DROPOUT_ACTION))
                        cell.BackgroundColor = alertColor;
                    cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    aTable.AddCell(cell);

                    cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}", sFuelVolume),
                        new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, currColor)));
                    cell.BorderColor = borderColor;
                    if (action.Equals(DROPOUT_ACTION))
                        cell.BackgroundColor = alertColor;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    aTable.AddCell(cell);

                    cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}", fFuelVolume),
                        new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, currColor)));
                    cell.BorderColor = borderColor;
                    if (action.Equals(DROPOUT_ACTION))
                        cell.BackgroundColor = alertColor;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    aTable.AddCell(cell);

                    cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(String.Format("{0:0.000}", diffVolume),
                        new iTextSharp.text.Font(tahoma, 10, iTextSharp.text.Font.NORMAL, currColor)));
                    cell.BorderColor = borderColor;
                    if (action.Equals(DROPOUT_ACTION))
                        cell.BackgroundColor = alertColor;
                    cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                    aTable.AddCell(cell);

                }

                document.Add(aTable);
            }

            document.Close();


            Response.Clear();
            Response.AddHeader("content-disposition", "attachment; filename=" + "Отчет "+driverName+" "+
                new DateTime(from.Year, from.Month, from.Day).ToString("dd_MM_yyyy") + "-" + new DateTime(to.Year, to.Month, to.Day).ToString("dd_MM_yyyy")+".pdf");
            Response.AddHeader("Content-Length", ms.GetBuffer().Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.End();
        }*/
    }
}