using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SmallDealerData
/// </summary>
public class SmallDealerData
{
    public int Key;
    public int level;
    public String DealerName;
    public List<SmallDealerData> dealers = new List<SmallDealerData>();

	public SmallDealerData()
	{    
	}
}