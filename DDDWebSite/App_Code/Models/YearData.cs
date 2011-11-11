using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for YearData
/// </summary>
public class YearData
{
    public String YearName { get; set; }
    public String MonthName { get; set; }
    public String DayName { get; set; }
    public String Percent { get; set; }
    public int key { get; set; }

    public YearData()
    {
        YearName = " ";
        MonthName = " ";
        DayName = " ";
        Percent = " ";
        key = 0;
    }
}