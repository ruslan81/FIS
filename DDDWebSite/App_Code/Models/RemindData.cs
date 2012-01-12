using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RemindData
/// </summary>
public class RemindData
{
    public int active { get; set; }
    public int id { get; set; }
    public int userId { get; set; }
    public string userName { get; set; }
    public int sourceType { get; set; }
    public int sourceId { get; set; }
    public string sourceName { get; set; }
    public int periodType { get; set; }
    public int type { get; set; }
    public string date { get; set; }

	public RemindData()
	{
	}
}