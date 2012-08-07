using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for YearData
/// </summary>
public class YearData
{
    public string YearName { get; set; }
    public string MonthName { get; set; }
    public string DayName { get; set; }
    public string Percent { get; set; }
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