using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using BLL;

/// <summary>
/// Summary description for PublicServices
/// </summary>
[WebService(Namespace = "http://smartfis.ru/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class PublicServices : System.Web.Services.WebService {

    public PublicServices () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public bool Login(string Profile, string UserName, string Password)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            int orgId = dataBlock.organizationTable.GetOrgId_byOrgName(Profile);
            if (dataBlock.usersTable.CustomAuthenticate(UserName, Password, orgId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    
}
