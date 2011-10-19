using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BLL;

/// <summary>
/// Service class of downloading files
/// </summary>
public partial class Administrator_download : System.Web.UI.Page
{
    /// <summary>
    /// Section "Восстановить у пользователя"
    /// </summary>
    public const string DOWNLOAD_FILE_RECOVER_USER = "RecoverUserDownload";

    protected void Page_Load(object sender, EventArgs e)
    {
        string Type=Request.Form.Get("type");

        //Section "Восстановить у пользователя"
        if (Type == DOWNLOAD_FILE_RECOVER_USER)
        {
            if (Request.Form.Get("dataBlockId") != null)
            {
                int dataBlockId = Convert.ToInt32(Request.Form.Get("dataBlockId"));
                string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
                DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
                dataBlock.OpenConnection();
                byte[] fileBytes = dataBlock.GetDataBlock_BytesArray(dataBlockId);
                string fileName = dataBlock.GetDataBlock_FileName(dataBlockId);
                dataBlock.CloseConnection();

                Response.Clear();
                Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                Response.AddHeader("Content-Length", fileBytes.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
                Response.End();
            }
        }
    }
}