using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.Interface;

namespace Factory
{
    public static class DBFactory
    {
        public static DBI CreateDataBase(String assembly, String typeName)
        {
            return (DBI)Activator.CreateInstanceFrom(assembly, typeName).Unwrap();
        }
    }
}
