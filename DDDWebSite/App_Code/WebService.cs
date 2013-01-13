using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using BLL;
using System.Data;
/// <summary>
/// Веб сервисы!
/// </summary>
[WebService(Description = "Web-сервис для работы с Plf", Namespace = "MyWebService")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class WebService : System.Web.Services.WebService {

    public WebService ()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "Загружает файл на сервер, сохраняет в папку PLF и добавляет в базу данных(без разбора). Возвращает Id блока добавленных данных в базе или -1 если ошибка.")]
    public int LoadPLFFileOnServer(byte[] FileInBytes, string fileName)
    {
        int dataBlockId = -1;
        try
        {
            string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
            DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

            if (FileInBytes != null)
            { 
                if (BLL.DataBlock.checkDataBlock(FileInBytes) || fileName.Substring(fileName.Length - 4, 4).ToLower() == ".plf")
                {
                    dataBlock.AddData(FileInBytes, fileName);
                    dataBlockId = dataBlock.GET_DATA_BLOCK_ID();

                    ByteArrayToFile(fileName, FileInBytes);
                }
                else
                    throw new Exception("Неправильный формат файла");
            }
        }
        catch
        {
            return -1;
        }
        return dataBlockId;
    }

    [WebMethod(Description = "Разбор загруженного файла по переданному ID. Возвращает сообщения об ощибках или успехе.")]
    public string ParsePLFFile(int dataBlockId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "EN_STRING");
        List<int> dataBlockIDs = new List<int>();

        try
        {          
                dataBlock = new DataBlock(connectionString, dataBlockId, "EN_STRING");
                PLFUnit.PLFUnitClass plf = (PLFUnit.PLFUnitClass)dataBlock.ParseRecords(0);
                string result = SaveXmlPlfFile(plf);
                return "Успешно разобрано. Создан XML файл " + result;
        }
        catch (Exception ex)
        {
            return "Ошибка: " + ex.Message;
        }
    }

    public string SaveXmlPlfFile(PLFUnit.PLFUnitClass plfUnit)
    {
        string output = Server.MapPath("~/XML_PLF") + "\\";
        return plfUnit.SerializePLFUnitClass(output);
    }


    public bool ByteArrayToFile(string _FileName, byte[] _ByteArray) 
	{ 
	    try
	    {
            string output = Server.MapPath("PLF") + "\\" + _FileName;
            if (!Directory.Exists(output))
                Directory.CreateDirectory(output);
            System.IO.FileStream _FileStream = new System.IO.FileStream(output, System.IO.FileMode.Create, System.IO.FileAccess.Write); 
	        _FileStream.Write(_ByteArray, 0, _ByteArray.Length); 
	        _FileStream.Close(); 
	        return true; 
	    } 
	    catch (Exception _Exception) 
	    { 
	        throw new Exception("Exception caught in process: " + _Exception.ToString()); 
	    } 
	} 
}

