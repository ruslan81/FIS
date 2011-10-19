using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Class, storing 
/// </summary>
public class GroupTree
{
    public String OrgName { get; set; }
    public List<TreeGroup> groups { get; set; }

    public GroupTree()
    {
        groups = new List<TreeGroup>();
    }

    public void addGroup(TreeGroup gr)
    {
        groups.Add(gr);
    }
}