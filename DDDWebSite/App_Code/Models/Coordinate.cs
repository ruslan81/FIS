using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Coordinate
/// </summary>
public class Coordinate
{
    public double x { get; set; }
    public float y { get; set; }

	public Coordinate(double x, float y)
	{
        this.x=x;
        this.y=y;
	}
}