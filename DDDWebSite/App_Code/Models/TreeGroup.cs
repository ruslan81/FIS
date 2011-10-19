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
    public List<StringValue> values { get; set; }

    public TreeGroup()
    {
        values = new List<StringValue>();
    }

    public void addValue(String value)
    {
        values.Add(new StringValue(value));
    }
}