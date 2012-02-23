using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text;

/// <summary>
/// Summary description for PageHeaderFooter
/// </summary>
public class PageHeaderFooter : iTextSharp.text.pdf.PdfPageEventHelper
{
	public PageHeaderFooter()
	{
		
	}

    // This is the contentbyte object of the writer
    PdfContentByte cb;

    // we will put the final number of pages in a template
    PdfTemplate template;

    // this is the BaseFont we are going to use for the header / footer
    BaseFont bf = null;

    // This keeps track of the creation time
    DateTime PrintTime = DateTime.Now;

    #region Properties
    private string _Title;
    public string Title
    {
        get { return _Title; }
        set { _Title = value; }
    }

    private string _TitleSubject;
    public string TitleSubject
    {
        get { return _TitleSubject; }
        set { _TitleSubject = value; }
    }

    private string _HeaderLeft;
    public string HeaderLeft
    {
        get { return _HeaderLeft; }
        set { _HeaderLeft = value; }
    }

    private string _HeaderRight;
    public string HeaderRight
    {
        get { return _HeaderRight; }
        set { _HeaderRight = value; }
    }

    private Font _HeaderFont;
    public Font HeaderFont
    {
        get { return _HeaderFont; }
        set { _HeaderFont = value; }
    }

    private Font _FooterFont;
    public Font FooterFont
    {
        get { return _FooterFont; }
        set { _FooterFont = value; }
    }
    #endregion

    // we override the onOpenDocument method
    public override void OnOpenDocument(PdfWriter writer, Document document)
    {
        try
        {
            PrintTime = DateTime.Now;
            string path = HttpContext.Current.Server.MapPath("~/fonts") + "\\";
            bf = iTextSharp.text.pdf.BaseFont.CreateFont(path + "tahoma.ttf", System.Text.Encoding.GetEncoding(1251).BodyName, true);
            cb = writer.DirectContent;
            template = cb.CreateTemplate(50, 50);
        }
        catch (DocumentException de)
        {
        }
        catch (System.IO.IOException ioe)
        {
        }
    }

    public override void OnStartPage(PdfWriter writer, Document document)
    {
        base.OnStartPage(writer, document);

        Rectangle pageSize = document.PageSize;

        cb.BeginText();
        cb.SetFontAndSize(bf, 8);
        cb.SetRGBColorFill(150, 150, 150);
        cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetTop(40));
        cb.ShowText(Title);
        cb.EndText();

        cb.SetRGBColorStroke(150,150,150);
        cb.SetLineWidth(1f);
        cb.SetLineDash(1f, 1f);
        cb.MoveTo(pageSize.GetLeft(40), pageSize.GetTop(45));
        cb.LineTo(pageSize.GetRight(40), pageSize.GetTop(45));
        cb.Stroke();
        cb.SetLineDash(0f);

        cb.BeginText();
        cb.SetFontAndSize(bf, 10);
        cb.SetRGBColorFill(150, 150, 150);
        cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetTop(56));
        cb.ShowText(TitleSubject);
        cb.EndText();

    }

    public override void OnEndPage(PdfWriter writer, Document document)
    {
        base.OnEndPage(writer, document);

        int pageN = writer.PageNumber;
        String text = "Страница " + pageN + " из ";
        float len = bf.GetWidthPoint(text, 8);

        Rectangle pageSize = document.PageSize;

        cb.SetRGBColorFill(150, 150, 150);

        cb.BeginText();
        cb.SetFontAndSize(bf, 8);
        cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(30));
        cb.ShowText(text);
        cb.EndText();

        cb.AddTemplate(template, pageSize.GetLeft(40) + len, pageSize.GetBottom(30));

        cb.BeginText();
        cb.SetFontAndSize(bf, 8);
        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
            "Время создания " + PrintTime.ToString("dd.MM.yyyy HH:mm:ss"),
            pageSize.GetRight(40),
            pageSize.GetBottom(30), 0);
        cb.EndText();
    }

    public override void OnCloseDocument(PdfWriter writer, Document document)
    {
        base.OnCloseDocument(writer, document);

        template.BeginText();
        template.SetFontAndSize(bf, 8);
        template.SetTextMatrix(0, 0);
        template.ShowText("" + (writer.PageNumber - 1));
        template.EndText();
    }
}