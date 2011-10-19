using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB.Interface
{
    /// <summary>
    /// Интерфейс для SQLDB_Records.cs
    /// </summary>
    public interface DBI_Records
    {
        List<string> Get_DriverNames_ByDataBlockIdList(List<int> dataBlockIds);
        List<string> Get_VehicleNumbers_ByDataBlockIdList(List<int> dataBlockIds);
        string Get_ParamValue(int dataBlockId, int paramId);
        //   string Get_ParamValue(int dataRecordId);
        int Get_DataBlockCardType(int dataBlockId);//0-card, 1 - vehicle
        string Get_DriversNumber(int dataBlockId);
        List<int> Get_DataBlockIdByDriversName(string name, string surname);
        List<int> Get_DataBlockIdByVehicleNumber(string number);
        List<int> Get_DataBlockIdByFileNameAndBytesCount(string fileName, int bytesCount);
        List<int> Get_DataBlockIdByRecordsCount(int recordsCount);
        string Get_DriversCardIssuingMemberState(int dataBlockId);
        string Get_DriversNameOrVehiclesNumberByBlockId(int dataBlockId);
        int Get_DataBlock_RecordsCount(int dataBlockId);
        string Get_DataBlock_EDate(int dataBlockId);
        List<string> Get_AllParamsArray(int dataBlockId, string paramName);
        //vehicle_overwiew_____________________________________
        byte Get_VOverview_CardSlotsStatus(int dataBlockId);
        long Get_VOverview_CurrentDateTime(int dataBlockId);
        string Get_VOverview_IdentificationNumber(int dataBlockId);
        string Get_VOverview_RegistrationNumber(int dataBlockId);
        short Get_VOverview_RegistrationNation(int dataBlockId);
       
    }
}
