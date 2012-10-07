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
    public int UploadFile(string Profile, string UserName, string Password, byte[] File, string FileName)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            if (CheckUser(Profile, UserName, Password, dataBlock))
            {
                if (File != null)
                {
                    if (BLL.DataBlock.checkDataBlock(File) || FileName.Substring(FileName.Length - 4, 4).ToLower() == ".plf")
                    {
                        int orgId = dataBlock.organizationTable.GetOrgId_byOrgName(Profile);
                        dataBlock.AddData(orgId, File, FileName);
                        int dataBlockID = dataBlock.GET_DATA_BLOCK_ID();
                        dataBlock.CloseConnection();

                        return dataBlockID;
                    }
                    else
                    {
                        throw new Exception("Invalid file format.");
                    }
                }
                else
                {
                    throw new Exception("File is null.");
                }
            }
            else
            {
                throw new Exception("Invalid authorization parameters.");
            }
        }
        catch (Exception ex)
        {
            dataBlock.CloseConnection();
            throw ex;
        }
    }

    [WebMethod]
    public bool ParseFile(string Profile, string UserName, string Password, int DataBlockID)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        try
        {
            dataBlock.OpenConnection();
            dataBlock.OpenTransaction();
            if (CheckUser(Profile, UserName, Password, dataBlock))
            {
                int userId = dataBlock.usersTable.Get_UserID_byName(UserName);
                int orgId = dataBlock.organizationTable.GetOrgId_byOrgName(Profile);

                dataBlock.SetDataBlockIdForParse(DataBlockID);
                dataBlock.SetOrgIdForParse(orgId);
                if (dataBlock.GetDataBlockState(DataBlockID) == "Not parsed")
                {
                    string output = HttpContext.Current.Server.MapPath("~/XML_PLF") + "\\";
                    object parsedObject = dataBlock.ParseRecords(true, output, userId);
                    return true;
                }
                else
                {
                    throw new Exception("Ivalid DataBlock state.");
                }
            }
            else
            {
                throw new Exception("Invalid authorization parameters.");
            }
        }
        catch (Exception ex)
        {
            dataBlock.RollbackConnection();
            dataBlock.CloseConnection();
            throw ex;
        }
    }

    private bool CheckUser(string Profile, string UserName, string Password, DataBlock DataBlock)
    {
        int orgId = DataBlock.organizationTable.GetOrgId_byOrgName(Profile);
        if (DataBlock.usersTable.CustomAuthenticate(UserName, Password, orgId))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
