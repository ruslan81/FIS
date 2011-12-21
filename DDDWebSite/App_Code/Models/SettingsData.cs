using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SettingsData
/// </summary>
public class SettingsData
{
    public String MeasureName { get; set; }
    public String CriteriaName { get; set; }
    public String CriteriaNote { get; set; }
    public int keyID { get; set; }
    public int MinValue { get; set; }
    public int MaxValue { get; set; }

	public SettingsData()
	{
	}

    public SettingsData(int keyID)
    {
        this.keyID = keyID;
    }
}