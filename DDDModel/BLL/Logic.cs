using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.Interface;
using DB.Factory;
using DB.SQL;


namespace BLL
{
    /// <summary>
    /// недоделано
    /// </summary>
    public class Logic
    {
        DBI dbI { get; set; }

        public Logic(String assembly, String database)
        {
            dbI = DBFactory.CreateDataBase(assembly, database);
        }           

        public Logic()
        {         
          //  dbI = new SQLDB();
        }        
    }
}
