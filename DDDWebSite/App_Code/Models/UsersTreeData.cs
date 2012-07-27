using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UsersTreeData
/// </summary>
public class UsersTreeData
{
    public List<UsersTreeOrgData> orgs = new List<UsersTreeOrgData>();

	public UsersTreeData()
	{
	}
}

public class UsersTreeOrgData 
{
    public string OrgName = "";
    public string OrgID = "";
    public List<MapItem> admins = new List<MapItem>();
    public List<MapItem> managers = new List<MapItem>();

    public UsersTreeOrgData() { }
}