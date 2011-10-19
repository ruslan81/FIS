using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.Interface;

namespace DB.Factory
{
    /// <summary>
    /// содержит класс для фабрики создания подключения к базе данных.
    /// Будет использоваться для перехода на другую базу, если потребуется.
    /// Если будет необходимо – дописать фабрику методами создания необходимого подключения к БД
    /// и заменить переменные конкретного подключения(как сейчас) на объект этого класса.
    /// </summary>
    public static class DBFactory
    {
        public static DBI CreateDataBase(String assembly, String typeName)
        {
            return (DBI)Activator.CreateInstanceFrom(assembly, typeName).Unwrap();
        }
    }
}
