﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Record
/// </summary>
public class Report
{
    public string report { get; set; }
    public double[] speed { get; set; }
    public double[] time { get; set; }
    public double[] voltage { get; set; }

	public Report()
	{
	}
}