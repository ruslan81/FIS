using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for VehiclesTreeFileItem
/// </summary>
public class VehiclesTreeFileItem
{
    public int DataBlockID { get; set; }
    public string FileName { get; set; }
    public string VehicleCardPeriodBegin { get; set; }
    public string VehicleCardPeriodEnd { get; set; }

	public VehiclesTreeFileItem()
	{
	}
}