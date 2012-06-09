using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for VehiclesTreeItem
/// </summary>
public class VehiclesTreeItem
{
    public string Name { get; set; }
    public int VehicleID { get; set; }
    public string VehicleVin { get; set; }
    public List<VehiclesTreeFileItem> Files { get; set; }

	public VehiclesTreeItem()
	{
		
	}
}