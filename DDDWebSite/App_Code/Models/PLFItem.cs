using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Plf item exmaple {vehicle: ОМ5628, deviceID: 300756, PLFs: [{key: 1, value: 13.05.2005 - 13.05.2005}, {key: 2, value: 13.05.2005 - 13.05.2005}]}
/// </summary>
public class PLFItem
{
    public string Vehicle { get; set; }
    public string DeviceID { get; set; }
    public List<MapItem> PLFs { get; set; }
    
	public PLFItem()
	{
        PLFs = new List<MapItem>();
	}
}