using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// Добавляет запись в историю
    /// </summary>
    public class HistoryWriter : Singleton<HistoryWriter>
    {
        private HistoryWriter()
        {
        }

        private DateTime lastActionDate { get; set; }
        private int lastUserId { get; set; }
        private int lastActionId { get; set; }
        private string lastTableName { get; set; }
        private BLL.HistoryTable history { get; set; }
        
        public void AddHistoryRecord(string tableName, string tableKeyFieldName, int TABLE_KEYFIELD_VALUE, int userId, int actionId, string Note, SQLDB SQLForAdding)
        {
            if (history == null)
                history = new BLL.HistoryTable("", "STRING_EN", SQLForAdding);

            DateTime actionDate = DateTime.Now;
            if (actionDate.Second == lastActionDate.Second &&
                lastUserId == userId &&
                lastActionId == actionId &&
                lastTableName == tableName)
                actionDate = actionDate.AddSeconds(1);
            lastActionDate = history.AddHistoryRecord(tableName, tableKeyFieldName, TABLE_KEYFIELD_VALUE, userId, actionId, actionDate, Note, SQLForAdding);
            lastUserId = userId;
            lastActionId = actionId;
            lastTableName = tableName;
        }
    }

    /// <summary>
    /// generic Singleton (потокобезопасный с использованием generic-класса и с отложенной инициализацией)
    /// </summary>
    /// <typeparam name="T">Singleton class</typeparam>
    public class Singleton<T> where T : class
    {
        /// <summary>
        ///  Защищенный конструктор по умолчанию необходим для того, чтобы
        ///  предотвратить создание экземпляра класса Singleton
        /// </summary>
        protected Singleton() { }
        /// <summary>
        /// Фабрика используется для отложенной инициализации экземпляра класса
        /// </summary>
        /// <typeparam name="S"></typeparam>
        private sealed class SingletonCreator<S> where S : class
        {
            /// <summary>
            /// Используется Reflection для создания экземпляра класса без публичного конструктора
            /// </summary>
            private static readonly S instance = (S)typeof(S).GetConstructor(
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        new Type[0],
                        new ParameterModifier[0]).Invoke(null);

            public static S CreatorInstance
            {
                get { return instance; }
            }
        }
        public static T Instance
        {
            get { return SingletonCreator<T>.CreatorInstance; }
        }
    }
}
