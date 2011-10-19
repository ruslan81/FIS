using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Stimulsoft.Report;
using Stimulsoft.Report.Export;
using System.IO;
using System.Net.Mail;
using System.Configuration;

public partial class Administrator_Reports_UserControls_NavigationReportControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewModeDropDownList.Items.Add(new ListItem("Одна страница", "OnePage"));
            ViewModeDropDownList.Items.Add(new ListItem("Весь отчет", "WholeReport"));
        }
        if (IsPostBack)
            LoadStiReport();
    }

    public void SetReportData(DataSet dataset, string Path)
    {
        Stimulsoft.Report.StiReport report = new Stimulsoft.Report.StiReport();
        report.Load(Server.MapPath(Path));
        report.RegData(dataset);
        StiWebViewer1.Report = report;

        StiWebViewer1.LastPage();
        ofPagesLabel.Text = "из " + (StiWebViewer1.CurrentPage + 1).ToString();
        if ((StiWebViewer1.CurrentPage + 1) > 11)
        {
            ViewModeDropDownList.SelectedIndex = 0;
            StiWebViewer1.ViewMode = Stimulsoft.Report.Web.StiWebViewMode.OnePage;
            ViewModeDropDownList.Enabled = false;
        }
        else
            ViewModeDropDownList.Enabled = true;

        StiWebViewer1.FirstPage();
        PageNumberTextBox.Text = (StiWebViewer1.CurrentPage + 1).ToString();
        ZoomPercentsTextBox.Text = StiWebViewer1.ZoomPercent.ToString();

        if (StiWebViewer1.ViewMode == Stimulsoft.Report.Web.StiWebViewMode.OnePage)
            ViewModeDropDownList.SelectedIndex = 0;
        else
            ViewModeDropDownList.SelectedIndex = 1;

        //StiWebViewer1.Visible = true;
        SaveStiReport();
    }

    public void ExportReport(StiExportFormat expFormat, StiPagesRange pageRange)
    {
        /*MemoryStream stream = new MemoryStream();
        StiWebViewer1.Report.ExportDocument(expFormat, stream);*/

        switch (expFormat)
        {
            case StiExportFormat.Pdf:
                {
                    Stimulsoft.Report.Web.StiReportResponse.ResponseAsPdf(this.Page, StiWebViewer1.Report, pageRange);
                } break;
            case StiExportFormat.Excel:
                {
                    Stimulsoft.Report.Web.StiReportResponse.ResponseAsXls(this.Page, StiWebViewer1.Report, pageRange);
                } break;
            case StiExportFormat.Csv:
                {
                    Stimulsoft.Report.Web.StiReportResponse.ResponseAsCsv(this.Page, StiWebViewer1.Report, pageRange);
                } break;
            case StiExportFormat.Rtf:
                {
                    Stimulsoft.Report.Web.StiReportResponse.ResponseAsRtf(this.Page, StiWebViewer1.Report, pageRange);
                } break;
            case StiExportFormat.Text:
                {
                    Stimulsoft.Report.Web.StiReportResponse.ResponseAsText(this.Page, StiWebViewer1.Report, pageRange);
                } break;
            case StiExportFormat.ImageJpeg:
                {
                    Stimulsoft.Report.Web.StiReportResponse.ResponseAsJpeg(this.Page, StiWebViewer1.Report);
                } break;
        }
    }

    public string SendReportByEmail(StiExportFormat expFormat, string mailTO, string mailSubject, string mailBody, string attachmentName)
    {

        MemoryStream stream = new MemoryStream();
        StiWebViewer1.Report.ExportDocument(expFormat, stream);
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
            return ("Произошла ошибка, сообщение не отправлено.");
        }

        return "Успешно отправлено";
    }

    public int GetPagesCountOnSiteAtonce()
    {
        if (StiWebViewer1.ViewMode == Stimulsoft.Report.Web.StiWebViewMode.OnePage)
            return 1;
        else
        {
            int curPage = StiWebViewer1.CurrentPage;
            StiWebViewer1.LastPage();
            int retVal = StiWebViewer1.CurrentPage + 1;
            StiWebViewer1.CurrentPage = curPage;
            return retVal;
        }
    }

    public int GetCurrentPage()
    {
        return StiWebViewer1.CurrentPage;
    }

    protected void NextButtonClick(object sender, EventArgs e)
    {
        StiWebViewer1.NextPage();
        PageNumberTextBox.Text = (StiWebViewer1.CurrentPage + 1).ToString();
       // SaveStiReport();
    }

    protected void PrevButtonClick(object sender, EventArgs e)
    {
        StiWebViewer1.PrevPage();
        PageNumberTextBox.Text = (StiWebViewer1.CurrentPage + 1).ToString();
       // SaveStiReport();
    }

    protected void LastButtonClick(object sender, EventArgs e)
    {
        StiWebViewer1.LastPage();
        PageNumberTextBox.Text = (StiWebViewer1.CurrentPage + 1).ToString();
        //SaveStiReport();
    }

    protected void FirstButtonClick(object sender, EventArgs e)
    {
        StiWebViewer1.FirstPage();
        PageNumberTextBox.Text = (StiWebViewer1.CurrentPage + 1).ToString();
        //SaveStiReport();
    }

    protected void ViewModeChanged(object sender, EventArgs e)
    {
        try
        {
            switch (ViewModeDropDownList.SelectedValue)
            {
                case "OnePage":
                    {
                        StiWebViewer1.ViewMode = Stimulsoft.Report.Web.StiWebViewMode.OnePage;
                    }
                    break;

                case "WholeReport":
                    {
                        StiWebViewer1.ViewMode = Stimulsoft.Report.Web.StiWebViewMode.WholeReport;
                    }
                    break;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ZoomInClick(object sender, EventArgs e)
    {
        StiWebViewer1.ZoomPercent += 10;
        ZoomPercentsTextBox.Text = StiWebViewer1.ZoomPercent.ToString();
    }

    protected void ZoomOutClick(object sender, EventArgs e)
    {
        StiWebViewer1.ZoomPercent -= 10;
        ZoomPercentsTextBox.Text = StiWebViewer1.ZoomPercent.ToString();
    }

    protected void ZoomTextChanged(object sender, EventArgs e)
    {
        try
        {
            StiWebViewer1.ZoomPercent = Convert.ToSingle(ZoomPercentsTextBox.Text);
        }
        catch
        {
        }
    }

    protected void PageTextChanged(object sender, EventArgs e)
    {
        try
        {
            StiWebViewer1.CurrentPage = Convert.ToInt32(PageNumberTextBox.Text);
        }
        catch
        {
        }
    }

    private void SaveStiReport()
    {
        Session["StiWebReportData1"] = StiWebViewer1.Report;
    }

    private void LoadStiReport()
    {
        StiWebViewer1.Report = (Stimulsoft.Report.StiReport)Session["StiWebReportData1"];
    }

    public override bool Visible
    {
        get
        {
            return base.Visible;
        }
        set
        {
            base.Visible = value;
            NavigationReportsPanel.Visible = value;
        }
    }
}
