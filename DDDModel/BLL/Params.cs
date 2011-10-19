using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// Класс отвечает за добавление параметров в базу данных(таблица fd_param)
    /// </summary>
    public class Params // Добаввил сюда крит секцию,когда во время добавления параметра ругалось на дублирование записей
    {
       public int AddParam(string name, string parentName, int size, SQLDB sqlDB)
       {
           int parentParamId;
           if (parentName != "")
               parentParamId = sqlDB.getParamId(parentName);
           else
               parentParamId = 0;

           if (parentParamId == -1)
               return -1; //нету Parent param.
           else
           {
               int paramId;
               Object thisLock = new Object();
               lock (thisLock)
               {
                   paramId = sqlDB.AddParam(name, parentParamId, size);
               }
               return paramId;
           }
       }      
    }
}
