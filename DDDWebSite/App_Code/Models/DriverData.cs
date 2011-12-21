using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DriverData
/// </summary>
public class CardData
{
	public String Name { get; set; }
    public String Comment { get; set; }
    public int grID { get; set; }
    public string Number { get; set; }
    public int groupID { get; set; }
    public string groupName { get; set; }
    //public List<MapItem> groups { get; set; }

	public CardData(int id)
	{
        this.grID = id;
	}

    public CardData()
    {
    }
}