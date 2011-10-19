using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Web.Caching;
using System.Text.RegularExpressions;
using System.IO;

/// <summary>
/// Summary description for PdfCreate
/// </summary>
public class PdfCreate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePdf"/> class.
    /// </summary>
    /// <param name="ds">The dataset containing one or more datatables.</param>
    /// <param name="name">The filename and pdf title.</param>
    public PdfCreate(DataSet ds, string name, string fontPathTmp)
    {
        this.ds = ds;
        this.name = name;
        this.fontPath = fontPathTmp;
    }

    private readonly DataSet ds;
    private readonly string name;
    private readonly string fontPath;

    public void Execute()
    {
        HttpResponse Response = HttpContext.Current.Response;
        Response.Clear();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + name + ".pdf");
        Response.ContentType = "application/pdf";
        // step 1: creation of a document-object
        Document document = new Document(PageSize.A4, 10, 10, 90, 10);
        // step 2: we create a writer that listens to the document
        PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
        //set some header stuff
        document.AddTitle(name);
        document.AddSubject("Table of " + name);
        document.AddCreator("This Application");
        document.AddAuthor("Me");
        // we Add a Header that will show up on PAGE 1
        //  Phrase phr = new Phrase(""); //empty phrase for page numbering
        //  HeaderFooter footer = new HeaderFooter(phr, true);
        //  document..Footer = new  footer;
        // step 3: we open the document
        document.Open();
        // step 4: we add content to the document
        CreatePages(document);
        // step 5: we close the document
        document.Close();
    }

    public void CreatePages(Document document)
    {
        bool first = true;
        foreach (DataTable table in ds.Tables)
        {
            if (first)
            {
                first = false;
                document.Add(FormatHeaderPhrase(table.TableName));
            }
            else
                document.Add(FormatPhrase("       "));
            if (table.TableName.Contains("SYSTEM_NEXT_PAGE"))
                document.NewPage();
            else
                if(table.TableName.Contains("SYSTEM_DELAY_TABLES"))
                    document.Add(FormatPhrase("       "));
            else
            {

                PdfPTable pdfTable = new PdfPTable(table.Columns.Count);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100; // percentage
                pdfTable.DefaultCell.BorderWidth = 1;
                pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                foreach (DataColumn column in table.Columns)
                {
                    pdfTable.AddCell(FormatPhrase(column.ColumnName));
                }
                pdfTable.HeaderRows = 1;  // this is the end of the table header
                BaseColor altRow = new BaseColor(242, 242, 242);
                int i = 0;
                foreach (DataRow row in table.Rows)
                {
                    i++;
                    if (i % 2 == 1)
                        pdfTable.DefaultCell.BackgroundColor = altRow;
                    foreach (object cell in row.ItemArray)
                    {
                        //assume toString produces valid output
                        pdfTable.AddCell(FormatPhrase(cell.ToString()));
                    }
                    if (i % 2 == 1)
                        pdfTable.DefaultCell.BackgroundColor = BaseColor.WHITE;
                }
                document.Add(pdfTable);
            }
        }
    }

    /// <summary>
    /// Formats the phrase. Apply your own font and size here.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    private Phrase FormatPhrase(string value)
    {
        BaseFont bf = BaseFont.CreateFont(this.fontPath, System.Text.Encoding.GetEncoding(1251).BodyName, BaseFont.NOT_EMBEDDED);
        Font font = new Font(bf, 8);
        return new Phrase(value, font);
    }

    private Phrase FormatHeaderPhrase(string value)
    {
        
        BaseFont bf = BaseFont.CreateFont(fontPath, System.Text.Encoding.GetEncoding(1251).BodyName, BaseFont.NOT_EMBEDDED);
        Font font = new Font(bf, 10, 0, BaseColor.DARK_GRAY);
        return new Phrase(value, font);
    }
}
//@"D:\Work\Projects\DDDReportWebSite\arial.ttf"