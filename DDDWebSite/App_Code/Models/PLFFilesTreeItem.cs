using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PLFFilesTreeItem
/// </summary>
public class PLFFilesTreeItem
{
    public string Name { get; set; }
    public string Number { get; set; }
    public int ID { get; set; }
    public List<PLFItem> PLFItems { get; set; }

	public PLFFilesTreeItem()
	{
        PLFItems = new List<PLFItem>();
	}
}