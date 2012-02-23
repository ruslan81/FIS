using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;

namespace BLL
{
    /// <summary>
    /// Класс организует работу с таблицами критериев fd_key.
    /// </summary>
    public class CriteriaTable //работа с таблицей fd_key
    {
        /// <summary>
        /// Текущий язык
        /// </summary>
        private string CurrentLanguage;//STRING_RU,STRING_ENG etc.
        /// <summary>
        /// Строка подключения
        /// </summary>
        private string connectionString;
        /// <summary>
        /// Обьект подключения к базе данных
        /// </summary>
        private SQLDB sqlDB;

        /// <summary>
        /// Идентификатор критерия(первичный ключ из базы данных)
        /// </summary>
        public int KeyId { get; set; }
        /// <summary>
        /// Название критерия
        /// </summary>
        public string CriteriaName { get; set; }
        /// <summary>
        /// Комментарий к критерию
        /// </summary>
        public string CriteriaNote { get; set; }
        /// <summary>
        /// Минимальное значение критерия
        /// </summary>
        public int MinValue { get; set; }
        /// <summary>
        /// Максимальное значение критерия
        /// </summary>
        public int MaxValue { get; set; }
        /// <summary>
        /// ID единицы измерения
        /// </summary>
        public int MeasureId { get; set; }
        /// <summary>
        /// Название единицы измерения
        /// </summary>
        public string MeasureName { get; set; }
        
        /// <summary>
        /// желательно не использовать, если пользуемся через dataBlock, а вызывать в dataBlock
        /// </summary>
        public void OpenConnection()
        {
            sqlDB.OpenConnection();
        }
        /// <summary>
        /// желательно не использовать, если пользуемся через dataBlock, а вызывать в dataBlock
        /// </summary>
        public void CloseConnection()
        {
            sqlDB.CloseConnection();
        }
///////////////////////Criteria
        public CriteriaTable()
        {
            connectionString = "";
            CurrentLanguage = "";
            sqlDB = new SQLDB(connectionString);
            KeyId = -1;
            CriteriaName = "";
            CriteriaNote = "";
            MinValue = -1;
            MaxValue = -1;
            MeasureId = -1;
            MeasureName = "";
        }
        public CriteriaTable(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            //sqlDB = new SQLDB(connectionString);
            sqlDB = sql;
            KeyId = -1;
            CriteriaName = "";
            CriteriaNote = "";
            MinValue = -1;
            MaxValue = -1;
            MeasureId = -1;
            MeasureName = "";
        }
        /// <summary>
        /// Загружает значения критерия в текущий экзмемпляр класса, а также возвращает его.
        /// </summary>
        /// <param name="keyId">ID критерия</param>
        /// <returns></returns>
        public CriteriaTable LoadCriteria(int keyId)
        {
            KeyId = keyId;
            //sqlDB.OpenConnection();
            CriteriaName = sqlDB.GetString(sqlDB.GetCriteriaNameId(keyId), CurrentLanguage);
            CriteriaNote = sqlDB.GetString(sqlDB.GetCriteriaNoteId(keyId), CurrentLanguage);
            MinValue = sqlDB.GetCriteriaMinValue(keyId);
            MaxValue = sqlDB.GetCriteriaMaxValue(keyId);
            MeasureId = sqlDB.GetCriteriaMeasureId(keyId);
            MeasureName = sqlDB.GetString(sqlDB.GetMeasureFullNameId(MeasureId), CurrentLanguage);
            //sqlDB.CloseConnection();
            return this;
        }
        /// <summary>
        /// Создает новый критерий
        /// </summary>
        /// <param name="MeasureId">ID единицы измерения</param>
        /// <param name="Name">Имя критерия</param>
        /// <param name="Note">Комментарий к критерию</param>
        /// <param name="minValue">Минимальное значение</param>
        /// <param name="maxValue">Максимальное значение</param>
        /// <returns>ID критерия</returns>
        public int AddNewCriteria(int MeasureId, string Name, string Note, int minValue, int maxValue)
        {
            sqlDB.OpenConnection();
            Exception ex = new Exception("Такой критерий уже существует");
            int retVal = -1;
            if (sqlDB.GetCriteriaId_byNameAndMeasureId(Name, MeasureId, CurrentLanguage) == -1)
                retVal = sqlDB.AddNewCriteria(MeasureId, Name, Note, minValue, maxValue);
            else
                throw ex;
            sqlDB.CloseConnection();
            return retVal;
        }
        /// <summary>
        /// Удалить критерий
        /// </summary>
        /// <param name="criteriaId">ID критерия</param>
        public void DeleteCriteria(int criteriaId)
        {
            sqlDB.OpenConnection();
            sqlDB.DeleteCriteria(criteriaId);
            sqlDB.CloseConnection();
        }
        /// <summary>
        /// Редактировать критерий
        /// </summary>
        /// <param name="keyId">ID критерия</param>
        /// <param name="MeasureId">ID единицы измерения</param>
        /// <param name="Name">Имя критерия</param>
        /// <param name="Note">Комментарий</param>
        /// <param name="minValue">Минимальное значение</param>
        /// <param name="maxValue">Максимальное значение</param>
        public void EditCriteria(int keyId, int MeasureId, string Name, string Note, int minValue, int maxValue)
        {
            sqlDB.EditCriteria(keyId, MeasureId, Name, Note, minValue, maxValue, CurrentLanguage);
        }
        /// <summary>
        /// Редактировать критерий
        /// </summary>
        /// <param name="keyId">ID критерия</param>
        /// <param name="Note">Комментарий</param>
        /// <param name="minValue">Минимальное значение</param>
        /// <param name="maxValue">Максимальное значение</param>
        public void EditCriteria(int keyId, string Note, int minValue, int maxValue)
        {
            CriteriaTable thisCriteria = this.LoadCriteria(keyId);
            sqlDB.OpenConnection();
            sqlDB.EditCriteria(keyId, thisCriteria.MeasureId, thisCriteria.CriteriaName, Note, minValue, maxValue, CurrentLanguage);
            sqlDB.CloseConnection();
        }
        /// <summary>
        /// Получает все критерии
        /// </summary>
        /// <returns>Массив пар (Имя критерия, Id критерия)</returns>
        public List<KeyValuePair<string, int>> GetAllCriteria_Name_n_Id()
        {
            List<KeyValuePair<string, int>> allCriteria = new List<KeyValuePair<string, int>>();
            sqlDB.OpenConnection();
            allCriteria = sqlDB.GetAllCriteria_Name_n_Id(CurrentLanguage);
            sqlDB.CloseConnection();
            return allCriteria;
        }
        /// <summary>
        /// Получает имя критерия
        /// </summary>
        /// <param name="keyId">ID критерия</param>
        /// <returns>Имя критерия</returns>
        public string GetCriteriaName(int keyId)
        {
            CriteriaName = sqlDB.GetString(sqlDB.GetCriteriaNameId(keyId), CurrentLanguage);
            return CriteriaName;
        }
        /// <summary>
        /// Получает ID критерия
        /// </summary>
        /// <param name="keyName">Название критерия</param>
        /// <param name="MeasureId">ID единицы измерения</param>
        /// <returns>ID критерия</returns>
        public int GetCriteriaId_byNameAndMeasureId(string keyName, int MeasureId)
        {
           return sqlDB.GetCriteriaId_byNameAndMeasureId(keyName, MeasureId, CurrentLanguage);
        }
        /// <summary>
        /// Получает ID критерия
        /// </summary>
        /// <param name="keyName">Название критерия</param>
        /// <returns>ID критерия</returns>
        public int GetCriteriaId_byName(string keyName)
        {
            return sqlDB.GetCriteriaId_byName(keyName, CurrentLanguage);
        }
///////////////////////Measures
        /// <summary>
        /// Добавляет единицу измерения
        /// </summary>
        /// <param name="shortName">Короткое название</param>
        /// <param name="fullName">Полное название</param>
        /// <returns>ID единицы измерения</returns>
        public int AddNewMeasure(string shortName, string fullName)
        {
            sqlDB.OpenConnection();
            Exception ex = new Exception("Такая единица измерения уже существует");
            int retVal = -1;
            if (sqlDB.GetMeasureId_byFullName(fullName, CurrentLanguage) == -1)
                retVal = sqlDB.AddNewMeasure(shortName, fullName);
            else
                throw ex;
            sqlDB.CloseConnection();
            return retVal;
        }
        /// <summary>
        /// Получает короткое название единицы измерения
        /// </summary>
        /// <param name="MeasureId">ID единицы измерения</param>
        /// <returns>Короткое название единицы измерения</returns>
        public string GetMeasureShortName(int MeasureId)
        {
           int nameId = sqlDB.GetMeasureShortNameId(MeasureId);
           return sqlDB.GetString(nameId, CurrentLanguage);
        }
        /// <summary>
        /// Получает полное название единицы измерения
        /// </summary>
        /// <param name="MeasureId">ID единицы измерения</param>
        /// <returns>Полное название единицы измерения</returns>
        public string GetMeasureFullName(int MeasureId)
        {
            int nameId = sqlDB.GetMeasureFullNameId(MeasureId);
            return sqlDB.GetString(nameId, CurrentLanguage);
        }
        /// <summary>
        /// Получает все ID единиц измерения
        /// </summary>
        /// <returns>Массив ID единиц измерения</returns>
        public List<int> GetAllMeasuresIds()
        {
            List<int> allMeasuresIDS = new List<int>();
            allMeasuresIDS = sqlDB.GetAllMeasuresIds();
            return allMeasuresIDS;
        }
        /// <summary>
        /// Редактировать едницу измерения
        /// </summary>
        /// <param name="MeasureId">ID единицы измерения</param>
        /// <param name="shortName">Короткое название единицы измерения</param>
        /// <param name="fullName">Полное название единицы измерения</param>
        public void EditMeasure(int MeasureId, string shortName, string fullName)
        {
            sqlDB.EditMeasure(MeasureId, shortName, fullName, CurrentLanguage);
        }
        /// <summary>
        /// Удалить единицу измерения
        /// </summary>
        /// <param name="measureId">ID единицы измерения</param>
        public void DeleteMeasure(int measureId)
        {
            sqlDB.OpenConnection();
            sqlDB.DeleteMeasure(measureId);
            sqlDB.CloseConnection();
        }
        /// <summary>
        /// Получить единицу измерения по ID критерия.
        /// </summary>
        /// <param name="keyId">ID критерия</param>
        /// <returns>ID единицы измерения</returns>
        public int GetMeasure_byKeyID(int keyId)
        {
            int MeasureId = sqlDB.GetCriteriaMeasureId(keyId);
            return MeasureId;
        }
    }
}
