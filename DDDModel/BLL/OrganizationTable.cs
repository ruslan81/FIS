using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB.SQL;
using System.IO;

namespace BLL
{
    /// <summary>
    /// работа с таблицами описывающими организацию
    /// </summary>
    public class OrganizationTable
    {
        private string CurrentLanguage;//STRING_RU,STRING_ENG etc.
        private string connectionString;
        SQLDB sqlDBR;

        public int DealerTypeId { get; set; }
        public int SubdealerTypeId { get; set; }
        public int PredealerTypeId { get; set; }
        public int ClientTypeId { get; set; }

        public void OpenConnection()
        {
            sqlDBR.OpenConnection();
        }
        public void CloseConnection()
        {
            sqlDBR.CloseConnection();
        }
        public void OpenTransaction()
        {
            sqlDBR.OpenTransaction();
        }
        public void CommitTransaction()
        {
            sqlDBR.CommitConnection();
        }
        public void RollbackConnection()
        {
            sqlDBR.RollbackConnection();
        }
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="connectionsStringTMP">строка подключения(не обязательно указывать)</param>
        /// <param name="Current_Language">Язык</param>
        /// <param name="sql">обьект SQLDB</param>
        public OrganizationTable(string connectionsStringTMP, string Current_Language, SQLDB sql)
        {
            connectionString = connectionsStringTMP;
            CurrentLanguage = Current_Language;
            //sqlDBR = new SQLDB(connectionString);
            sqlDBR = sql;
            DealerTypeId = 4;
            SubdealerTypeId = 5;
            PredealerTypeId = 6;
            ClientTypeId = 1;//Тип организации клиента по-умолчанию
        }

        /// <summary>
        /// Присвоить имя организации
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="ORG_ID">ID Организации</param>
        public void SetOrganizationName(string name, int ORG_ID)
        {
            sqlDBR.SetOrgName(name, ORG_ID, CurrentLanguage);
        }
        /// <summary>
        /// Получить имя организации
        /// </summary>
        /// <param name="ORG_ID">ID Организации</param>
        /// <returns>Имя организации</returns>
        public string GetOrganizationName(int ORG_ID)
        {
            int orgNameId = sqlDBR.GetOrgNameId(ORG_ID);
            return sqlDBR.GetString(orgNameId, CurrentLanguage);
        }
        /// <summary>
        /// Функция инициализирует БД странами и регионами. Пока там есть только беларусь и польша. При необходимости добавить все остальные.
        /// Все названия и регионы для необходимых стран можно взять на википедии.
        /// </summary>
        /// <param name="insertPass">пароль, чтобы нечайно не вызвать</param>
        /// <param name="iconPath">Путь к папке, где будут лежать флаги стран. Смотреть комментарии в коде. Функционал не реализован.</param>
        public void FillCountryAndRegionsTable(string insertPass, string iconPath)
        {
            if (insertPass == "qqq")
            {
                try
                {
                    int countryNameId;
                    int countryABBRId;
                    byte[] countryFlagPic = new byte[1];
                    List<KeyValuePair<int, int>> regionNames_short_long = new List<KeyValuePair<int, int>>();
                    sqlDBR.OpenConnection();
                    sqlDBR.OpenTransaction();
                    //Belarus
                    countryNameId = sqlDBR.AddOrGetString("Belarus");
                    countryABBRId = sqlDBR.AddOrGetString("BY");
                    //countryFlagPic = File.ReadAllBytes(iconPath +"/belarus.png");
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Gomel"), sqlDBR.AddOrGetString("Gomel")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Grodno"), sqlDBR.AddOrGetString("Grodno")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Brest"), sqlDBR.AddOrGetString("Brest")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Mogilev"), sqlDBR.AddOrGetString("Mogilev")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Vitebsk"), sqlDBR.AddOrGetString("Vitebsk")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Minsk"), sqlDBR.AddOrGetString("Minsk")));

                    sqlDBR.AddCountryAndRegion(countryNameId, countryABBRId, countryFlagPic, regionNames_short_long);

                    //Poland
                    countryNameId = sqlDBR.AddOrGetString("Poland");
                    countryABBRId = sqlDBR.AddOrGetString("Pl");
                    countryFlagPic = new byte[0];
                    regionNames_short_long = new List<KeyValuePair<int, int>>();
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Greater Poland"), sqlDBR.AddOrGetString("Greater Poland")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Kuyavian-Pomeranian"), sqlDBR.AddOrGetString("Kuyavian-Pomeranian")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Lesser Poland"), sqlDBR.AddOrGetString("Lesser Poland")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Łódź"), sqlDBR.AddOrGetString("Łódź")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Lower Silesian"), sqlDBR.AddOrGetString("Lower Silesian")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Lublin"), sqlDBR.AddOrGetString("Lublin")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Lubusz"), sqlDBR.AddOrGetString("Lubusz")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Masovian"), sqlDBR.AddOrGetString("Masovian")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Opole"), sqlDBR.AddOrGetString("Opole")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Podlaskie"), sqlDBR.AddOrGetString("Podlaskie")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Pomeranian"), sqlDBR.AddOrGetString("Pomeranian")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Silesian"), sqlDBR.AddOrGetString("Silesian")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Subcarpathian"), sqlDBR.AddOrGetString("Subcarpathian")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Świętokrzyskie"), sqlDBR.AddOrGetString("Świętokrzyskie")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Warmian-Masurian"), sqlDBR.AddOrGetString("Świętokrzyskie")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("Świętokrzyskie"), sqlDBR.AddOrGetString("Świętokrzyskie")));
                    regionNames_short_long.Add(new KeyValuePair<int, int>(sqlDBR.AddOrGetString("West Pomeranian"), sqlDBR.AddOrGetString("Świętokrzyskie")));
                    //countryFlagPic = File.ReadAllBytes(iconPath + "/poland.png");

                    sqlDBR.AddCountryAndRegion(countryNameId, countryABBRId, countryFlagPic, regionNames_short_long);
                    sqlDBR.CommitConnection();
                    sqlDBR.CloseConnection();
                }
                catch (Exception ex)
                {
                    sqlDBR.RollbackConnection();
                    sqlDBR.CloseConnection();
                    throw ex;
                }
            }
            else throw new Exception("Неправильный пароль");
        }
        /// <summary>
        /// Получить массив байт - картинку флага выбранной страны(не тестировал)
        /// </summary>
        /// <param name="countryId">ID страны</param>
        /// <returns>Массив байт - картинка с флагом выбранной страны</returns>
        public byte[] GetCountryFlag_pngbytes(int countryId)
        {
            byte[] flag = sqlDBR.GetCountryFlag(countryId);
            return flag;
        }
        /// <summary>
        /// Получить все ID организаций типа Дилер
        /// </summary>
        /// <param name="parentOrgId">ID организации предка для Дилеров</param>
        /// <returns>Массив ID организаций</returns>
        public List<int> Get_AllDealersId(int parentOrgId)
        {
            return sqlDBR.GetAllDealersId(parentOrgId, DealerTypeId, SubdealerTypeId);
        }
        /// <summary>
        /// Получить ID всех организаций
        /// </summary>
        /// <returns>Массив ID организаций</returns>
        public List<int> Get_AllOrganizationsId()
        {
            return sqlDBR.GetAllOrganizationsId();
        }
        /// <summary>
        /// Массив ID организаций с указанной организацией предком
        /// </summary>
        /// <param name="orgId">ID организации предка</param>
        /// <returns>Массив ID организаций</returns>
        public List<int> Get_AllOrganizationsId(int orgId)
        {
            return sqlDBR.GetAllOrganizationsId(orgId, DealerTypeId, SubdealerTypeId, PredealerTypeId);
        }
        /// <summary>
        /// Получить все названия организаций
        /// </summary>
        /// <returns>Массив пар значений (имя организации, ID организации)</returns>
        public List<KeyValuePair<string, int>> GetAllOrganizationNames()
        {
            return sqlDBR.GetOrganizationNames(CurrentLanguage);
        }
        /// <summary>
        /// Получить все типы организаций
        /// </summary>
        /// <returns>Массив пар значений (имя типа организаций, ID типа организаций)</returns>
        public List<KeyValuePair<string, int>> GetAllOrganizationTypes()
        {
            return sqlDBR.GetOrgTypes(CurrentLanguage);
        }
        /// <summary>
        /// Получить название типа организации
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <returns>Название типа организации</returns>
        public string GetOrgTypeName(int orgId)
        {
            int orgTypeId = sqlDBR.GetOrgTypeId(orgId);
            int orgtypeNameId = sqlDBR.GetOrgTypeNameId(orgTypeId);
            return sqlDBR.GetString(orgtypeNameId, CurrentLanguage);
        }
        /// <summary>
        /// Получить тип организации
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <returns>ID типа организации</returns>
        public int GetOrgTypeId(int orgId)
        {
            int orgTypeId = sqlDBR.GetOrgTypeId(orgId);
            return orgTypeId;
        }
        //ЗЛО!
        /// <summary>
        /// Получить название страны организации
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <returns>Название страны организации</returns>
        public string GetOrgCountryName(int orgId)
        {
            int orgCountryId = sqlDBR.GetOrgCountryId(orgId);
            int orgCountryNameId = sqlDBR.GetCountryNameId(orgCountryId);
            return sqlDBR.GetString(orgCountryNameId, CurrentLanguage);
        }
        //ЗЛО!
        /// <summary>
        /// Получить название региона организации
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <returns>Название региона организации</returns>
        public string GetOrgRegionName(int orgId)
        {
            int orgRegionId = sqlDBR.GetOrgRegionId(orgId);
            int orgRegionNameId = sqlDBR.GetRegionLongNameId(orgRegionId);
            return sqlDBR.GetString(orgRegionNameId, CurrentLanguage);
        }
        /// <summary>
        /// Получить ID страны организации
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <returns>ID страны организации</returns>
        public int GetOrgCountryId(int orgId)
        {
            return sqlDBR.GetOrgCountryId(orgId);
        }
        /// <summary>
        /// Получить ID региона организации
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <returns>ID региона организации</returns>
        public int GetOrgRegionId(int orgId)
        {
            return sqlDBR.GetOrgRegionId(orgId);
        }
        /// <summary>
        /// Получить все страны
        /// </summary>
        /// <returns>Массив пар значений (имя страны, ID типа страны)</returns>
        public List<KeyValuePair<string, int>> GetAllCountry()
        {
            return sqlDBR.GetAllCountry(CurrentLanguage);
        }
        /// <summary>
        /// Получить все регионы
        /// </summary>
        /// <param name="countryId">ID страны</param>
        /// <returns>Массив пар значений (имя региона, ID региона)</returns>
        public List<KeyValuePair<string, int>> GetAllRegions(int countryId)
        {
            return sqlDBR.GetAllRegions(countryId, CurrentLanguage);
        }
        /// <summary>
        /// Редактировать организацию
        /// </summary>
        /// <param name="oldName">старое имя</param>
        /// <param name="newName">новое имя</param>
        /// <param name="orgTypeId">ID типа организации</param>
        /// <param name="orgCountryId">ID страны</param>
        /// <param name="orgRegionId">ID региона</param>
        public void EditOrganization(string oldName, string newName, int orgTypeId, int orgCountryId, int orgRegionId)
        {
            Exception orgNameAllreadyExists = new Exception("Организация с таким названием уже существует!");
            int orgNewNameId = sqlDBR.GetOrgId_byOrgNameStr(newName, CurrentLanguage);
            int orgOldNameId = sqlDBR.GetOrgId_byOrgNameStr(oldName, CurrentLanguage);
            if (orgNewNameId > 0)
            {
                if (orgNewNameId != orgOldNameId)
                    throw orgNameAllreadyExists;
            }
            sqlDBR.AddNewOrganization(newName, orgTypeId, orgCountryId, orgRegionId, oldName, CurrentLanguage);
        }
        /// <summary>
        /// Установить страну и регион для организации
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="countryId">ID страны</param>
        /// <param name="regionId">ID региона</param>
        public void SetOrgCountryAndRegion(int orgId, int countryId, int regionId)
        {
            sqlDBR.SetOrgCountryAndRegion(orgId, countryId, regionId);
        }
        /// <summary>
        /// Добавить новую организацию
        /// </summary>
        /// <param name="newName">Новое имя</param>
        /// <param name="orgTypeId">Id типа организации</param>
        /// <param name="orgCountryId">ID страны</param>
        /// <param name="orgRegionId">ID региона</param>
        /// <returns></returns>
        public int AddNewOrganization(string newName, int orgTypeId, int orgCountryId, int orgRegionId)
        {
            Exception orgNameAllreadyExists = new Exception("Организация с таким названием уже существует!");
            int orgNewNameId = sqlDBR.GetOrgId_byOrgNameStr(newName, CurrentLanguage);
            if (orgNewNameId > 0)
            {
                throw orgNameAllreadyExists;
            }

            int orgId = sqlDBR.AddNewOrganization(newName, orgTypeId, orgCountryId, orgRegionId, newName, CurrentLanguage);
            AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_RegistrationDate, DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            AddOrEditAdditionalOrgInfo(orgId, DataBaseReference.OrgInfo_EndOfRegistrationDate, DateTime.Now.AddMonths(6).ToShortDateString());
            //внимательно следить при неполадках. описана эта функция в CardsTable
            sqlDBR.CreateNewCard(newName + "ORG", "000", 3, orgId, "Карта организации " + newName + " для неразобранных блоков данных",1);

            return orgId;
        }
        /// <summary>
        /// Добавить новую организацию
        /// </summary>
        /// <param name="newName">Новое имя</param>
        /// <param name="orgTypeId">Id типа организации</param>
        /// <param name="orgCountryId">ID страны</param>
        /// <param name="orgRegionId">ID региона</param>
        /// <param name="parentOrgId">ID организации предка</param>
        /// <returns></returns>
        public int AddNewOrganization(string newName, int orgTypeId, int orgCountryId, int orgRegionId, int parentOrgId)
        {
            int newOrgId = AddNewOrganization(newName, orgTypeId, orgCountryId,orgRegionId);
            SetParentOrganization(newOrgId, parentOrgId);
            return newOrgId;
        }
        /// <summary>
        /// Установить организацию предка
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="ParentOrgId">ID организации предка</param>
        public void SetParentOrganization(int orgId, int ParentOrgId)
        {
            sqlDBR.SetParentOrganization(orgId, ParentOrgId);
        }
        /// <summary>
        /// Получить ID организации предка
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <returns>ID организации предка</returns>
        public int GetParentOrganization(int orgId)
        {
            return sqlDBR.GetOrgParentOrganization(orgId);
        }
        /// <summary>
        /// Получить все возможные доп. сведения для организации
        /// </summary>
        /// <returns>Массив пар значений (имя доп параметра, ID доп параметра)</returns>
        public List<KeyValuePair<string, int>> GetAllOrgInfos()
        {
            return sqlDBR.GetAllOrgInfoIds(CurrentLanguage);
        }
        /// <summary>
        /// Получить дополнительную информацию об организации
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="ORG_INFO_ID">ID доп параметра</param>
        /// <returns>Значение дополнительного параметра</returns>
        public string GetAdditionalOrgInfo(int orgId, int ORG_INFO_ID)
        {
            int strId = sqlDBR.GetAdditionalOrgInfoValueId(orgId, ORG_INFO_ID);
            if (strId > 0)
                return sqlDBR.GetString(strId, CurrentLanguage);
            else
                return "Нет значения";
        }
        /// <summary>
        /// Получить дополнительную информацию об организации
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="ORG_INFO_NAME">имя доп параметра</param>
        /// <returns>Значение дополнительного параметра</returns>
        public string GetAdditionalOrgInfo(int orgId, string ORG_INFO_NAME)
        {
            int ORG_INFO_IDSTRID = sqlDBR.GetStringId(ORG_INFO_NAME);
            int ORG_INFO_ID = sqlDBR.GetOrgInfoId_bySTRID(ORG_INFO_IDSTRID);
            if (ORG_INFO_ID <= 0)
                return "Нет значения";
            return GetAdditionalOrgInfo(orgId, ORG_INFO_ID);
        }
        /// <summary>
        /// Добавить или редактировать Доп параметр(информацию) организации.
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="ORG_INFO_ID">ID доп. параметра</param>
        /// <param name="value">Значение доп. параметра</param>
        public void AddOrEditAdditionalOrgInfo(int orgId, int ORG_INFO_ID, string value)
        {
            sqlDBR.AddAdditionalOrgInfo(orgId, ORG_INFO_ID, value, CurrentLanguage);
        }
        /// <summary>
        /// Добавить или редактировать Доп параметр(информацию) организации.
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="ORG_INFO_NAME">Название доп. параметра</param>
        /// <param name="value">Значение доп. параметра</param>
        public void AddOrEditAdditionalOrgInfo(int orgId, string ORG_INFO_NAME, string value)
        {
            int infoNameId = GetOrgInfoNameId(ORG_INFO_NAME);
            if (infoNameId <= 0)
                infoNameId = AddOrgInfo(ORG_INFO_NAME);
            sqlDBR.AddAdditionalOrgInfo(orgId, infoNameId, value, CurrentLanguage);
        }
        /// <summary>
        /// Получить ID доп.параметра организации
        /// </summary>
        /// <param name="InfoName">Имя доп. параметра</param>
        /// <returns>ID доп. параметра</returns>
        public int GetOrgInfoNameId(string InfoName)
        {
            int stringId = sqlDBR.GetStringId(InfoName);
            int OrgInfoId = sqlDBR.GetOrgInfoName(stringId);

            if (OrgInfoId > 0)
                return OrgInfoId;
            else
                return AddOrgInfo(InfoName);
        }
        /// <summary>
        /// Добавить доп. параметр для организации
        /// </summary>
        /// <param name="newName">Имя нового параметра</param>
        /// <returns>ID нового доп.параметра</returns>
        public int AddOrgInfo(string newName)
        {
            return sqlDBR.AddNewOrgInfo(newName);
        }
        /// <summary>
        /// Удалить организацию. Мягко. Очень.
        /// </summary>
        /// <param name="orgId">ID организации</param>
        public void DeleteOrganization(int orgId)
        {
            sqlDBR.DeleteOrganization(orgId);
        }
        /// <summary>
        /// Удалить доп параметр организации
        /// </summary>
        /// <param name="Org_info_id">ID доп. параметра</param>
        public void DeleteOrgInfo(int Org_info_id)
        {
            sqlDBR.DeleteOrgInfo(Org_info_id);
        }
        public int GetOrgId_byOrgName(string orgName)
        {
            return sqlDBR.GetOrgId_byOrgNameStr(orgName, CurrentLanguage);
        }
    }
    /// <summary>
    /// Класс описывает организацию для удобства выборки.
    /// </summary>
    public class OrganizationFromTable
    {
        public string orgName { get; set; }
        public string orgType { get; set; }
        public string countryName { get; set; }
        public string regionName { get; set; }
        public byte[] country_flag { get; set; }
        /// <summary>
        /// конструктор по умолчанию
        /// </summary>
        public OrganizationFromTable()
        {
            orgName = "";
            orgType = "";
            countryName = "";
            regionName = "";
            country_flag = new byte[0];
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="orgNameT">имя организации</param>
        /// <param name="orgTypeT">тип организации</param>
        /// <param name="countryNameT">название страны</param>
        /// <param name="regionNameT">название региона</param>
        /// <param name="country_flagT">файл флага страны</param>
        public OrganizationFromTable(string orgNameT, string orgTypeT, string countryNameT, string regionNameT, byte[] country_flagT)
        {
            orgName = orgNameT;
            orgType = orgTypeT;
            countryName = countryNameT;
            regionName = regionNameT;
            country_flag = country_flagT;
        }
        /// <summary>
        /// Загружает данный экземпляр данными
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="tables">экземпляр класса OrganizationTable</param>
        /// <returns>this</returns>
        public OrganizationFromTable FillWithInfo(int orgId, OrganizationTable tables)
        {
            tables.OpenConnection();
            orgName = tables.GetOrganizationName(orgId);
            orgType = tables.GetOrgTypeName(orgId);
            countryName = tables.GetOrgCountryName(orgId);
            regionName = tables.GetOrgRegionName(orgId);
            tables.CloseConnection();
            return this;
        }
    }
}