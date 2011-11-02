using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DriverGroup
/// </summary>
public class TreeGroup
{
    public String GroupName { get; set; }
    //public String GroupKey { get; set; }
    public List<MapItem> values { get; set; }

    public TreeGroup()
    {
        values = new List<MapItem>();
    }

    public void addValue(String key, String value)
    {
        values.Add(new MapItem(key, value));
    }
}