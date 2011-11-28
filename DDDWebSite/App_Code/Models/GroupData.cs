using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for GroupData
/// </summary>
public class GroupData
{
    public String Name { get; set; }
    public String Comment { get; set; }
    public int grID { get; set; }
    public int Number { get; set; }

	public GroupData(int id)
	{
        this.grID = id;
	}

    public GroupData()
    {
    }
}