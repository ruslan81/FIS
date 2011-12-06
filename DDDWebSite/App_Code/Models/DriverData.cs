using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DriverData
/// </summary>
public class DriverData
{
	 public String Name { get; set; }
    public String Comment { get; set; }
    public int grID { get; set; }
    public string Number { get; set; }

	public DriverData(int id)
	{
        this.grID = id;
	}

    public DriverData()
    {
    }
}