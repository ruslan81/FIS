using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Driver
/// </summary>
public class Driver
{

    public int DataBlockId { get; set; }
    public int Number { get; set; }
    public string CardTypeName { get; set; }
    public string Name { get; set; }
    public int RecordsCount { get; set; }
    public string CreationTime { get; set; }
    public string FromDate { get; set; }

    public void setFromDate(DateTime FromDate)
    {
        this.FromDate = FromDate.ToString("dd.MM.yyyy");
    }

    public string ToDate { get; set; }

    public void setToDate(DateTime ToDate)
    {
        this.ToDate = ToDate.ToString("dd.MM.yyyy");
    }

}