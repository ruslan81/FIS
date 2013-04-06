using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using DB.SQL;
using DB.Interface;
using VehichleUnit;
using PARSER;
using System.Diagnostics;

namespace BLL
{
    /// <summary>
    /// главный класс логики, включающий или использующий все остальные в этом пространстве имен.
    /// Для работы с логикой на сайте в 99 процентах случаев используется этот класс.
    /// Поэтому для работы с базой данных или чем-либо другим нужно создавать этот класс.
    /// Он также содержит методы для работы с базой данных, такие как открытие/закрытие подключения
    /// и открытие/закрытие транзакций. Поэтому не рекомендуется открывать подключение любым другим способом.
    /// </summary>
    public class DataBlock
    {
        /// <summary>
        /// Подключение к БД
        /// </summary>
        public SQLDB sqlDb { get; set; }
        /// <summary>
        /// Открыть подключение к БД
        /// </summary>
        public void OpenConnection()
        {
            sqlDb.OpenConnection();
        }
        /// <summary>
        /// Зкрыть подключение к БД
        /// </summary>
        public void CloseConnection()
        {
            sqlDb.CloseConnection();
        }
        /// <summary>
        /// Открыть транзакцию
        /// </summary>
        public void OpenTransaction()
        {
            sqlDb.OpenTransaction();
        }
        /// <summary>
        /// Commit транзакцию
        /// </summary>
        public void CommitTransaction()
        {
            sqlDb.CommitConnection();
        }
        /// <summary>
        /// Откатить транзакцию
        /// </summary>
        public void RollbackConnection()
        {
            sqlDb.RollbackConnection();
        }

        public int GetedParsedRecords { get; set; }
        public int AddedRecords { get; set; }

        public string ReturnTimeForDebugOnly { get; set; }//удалить потом
        /// <summary>
        /// dataBlockId с которым сейчас ведется работа
        /// </summary>
        int DATA_BLOCK_ID;
        /// <summary>
        /// dataBlockId с которым велась работа ранее(сейчас кажется не используется)
        /// </summary>
        int DATA_BLOCK_ID_PREVIOUS { get; set; }
        /// <summary>
        /// ID БУ
        /// </summary>
        int DEVICE_ID { get; set; }
        /// <summary>
        /// ID состояния датаблока
        /// </summary>
        int DATA_BLOCK_STATE_ID { get; set; }
        /// <summary>
        /// ДАта начала парсинга
        /// </summary>
        DateTime PARSE_BDATE { get; set; }
        /// <summary>
        /// дата окончания парсинга
        /// </summary>
        DateTime PARSE_EDATE { get; set; }
        /// <summary>
        /// ID Сообщения
        /// </summary>
        int STRID_PARSE_MESSAGE { get; set; }
        /// <summary>
        /// тип карты
        /// </summary>
        public int currentCardType { get; set; }
        /// <summary>
        /// Язык
        /// </summary>
        private string CurrentLanguage;//STRING_RU,STRING_RUG etc.
        /// <summary>
        /// Обьект записи 
        /// </summary>
        public DataRecords dataRecord { get; set; }
        /// <summary>
        /// строка полключения
        /// </summary>
        private string connectionString;
        /// <summary>
        /// Id организации
        /// </summary>
        private int organizationID { get; set; }
        /// <summary>
        /// выгрузка ДДД данных для транспортного средства.
        /// </summary>
        public VehicleUnitInfo vehicleUnitInfo { get; set; }
        /// <summary>
        /// Класс отвечает за выборку информации по ДДД картам.
        /// </summary>
        public CardUnitInfo cardUnitInfo { get; set; }
        /// <summary>
        /// Класс отвечает за выборку информации по ДДД файлам.
        /// </summary>
        public PLFUnitInfo plfUnitInfo { get; set; }
        /// <summary>
        /// Работа с таблицами Карт и БД. Карты создаются для Водителей и транспортных средств.
        /// </summary>
        public CardsTable cardsTable { get; set; }
        /// <summary>
        /// работа с таблицами описывающими организацию
        /// </summary>
        public OrganizationTable organizationTable { get; set; }
        /// <summary>
        ///  работа с таблицами пользователей.
        /// </summary>
        public UsersTables usersTable { get; set; }
        /// <summary>
        /// работа с таблицей строк.
        /// </summary>
        public StringTable stringTable { get; set; }
        /// <summary>
        /// работа с таблицами Транспортного средства.
        /// </summary>
        public VehiclesTable vehiclesTables { get; set; }
        /// <summary>
        /// Класс организует работу с таблицами критериев fd_key.
        /// </summary>
        public CriteriaTable criteriaTable { get; set; }
        /// <summary>
        /// работа с таблицами, описывающими установленное бортовое оборудование
        /// </summary>
        public DeviceTable deviceTable { get; set; }
        // public HistoryTable historyTable { get; set; }
        /// <summary>
        /// работа с таблицами отчетов.
        /// </summary>
        public ReportsTable reportsTable { get; set; }
        /// <summary>
        /// Работа с таблицами счетов.
        /// </summary>
        public InvoiceTable invoiceTable { get; set; }
        /// <summary>
        /// Класс отвечает за работу с отправкой почты по расписанию.
        /// </summary>
        public EmailScheduleTable emailScheduleTable { get; set; }
        /// <summary>
        /// Класс отвечает за работу с таблицей напоминаний.
        /// </summary>
        public RemindTable remindTable { get; set; }

        /// <summary>
        /// Возврашает текущий  dataBlcokId
        /// </summary>
        /// <returns>dataBlcokId</returns>
        public int GET_DATA_BLOCK_ID()
        {
            return DATA_BLOCK_ID;
        }

        /// <summary>
        /// Конструктор для работы с уже занессеной записью(использовать например для удаления DataBlock
        /// </summary>
        /// <param name="connectionStringTMP">строка подключения</param>
        /// <param name="dataBlockId">dataBlockId</param>
        /// <param name="Current_Language">Язык</param>
        public DataBlock(string connectionStringTMP, int dataBlockId, string Current_Language)//Работа с уже занесенной записью
        {
            //тестовое подключение
            sqlDb = new SQLDB(connectionStringTMP);
            ///////

            SQLDB sqlDB = new SQLDB(connectionStringTMP);
            int dataIdTemp = sqlDB.checkTableExistence("fn_data_block", "DATA_BLOCK_ID", dataBlockId);
            if (dataIdTemp == 0)
                throw (new Exception("There is no Data Block with id " + dataBlockId.ToString()));

            connectionString = connectionStringTMP;
            DATA_BLOCK_ID = dataBlockId;
            DATA_BLOCK_ID_PREVIOUS = -1;
            CurrentLanguage = Current_Language;
            dataRecord = new DataRecords(connectionStringTMP, dataBlockId, Current_Language, sqlDb);
            vehicleUnitInfo = new VehicleUnitInfo(connectionString, Current_Language, sqlDb);
            cardUnitInfo = new CardUnitInfo(connectionString, Current_Language, sqlDb);
            plfUnitInfo = new PLFUnitInfo(connectionString, Current_Language, sqlDb);
            cardsTable = new CardsTable(connectionString, Current_Language, sqlDb);
            organizationTable = new OrganizationTable(connectionString, Current_Language, sqlDb);
            usersTable = new UsersTables(connectionString, CurrentLanguage, sqlDb);
            stringTable = new StringTable(connectionString, CurrentLanguage);
            vehiclesTables = new VehiclesTable(connectionString, CurrentLanguage, sqlDb);
            criteriaTable = new CriteriaTable(connectionString, CurrentLanguage, sqlDb);
            deviceTable = new DeviceTable(connectionString, CurrentLanguage, sqlDb);
            //historyTable = new HistoryTable(connectionString, CurrentLanguage, sqlDb);
            reportsTable = new ReportsTable(connectionString, CurrentLanguage, sqlDb);
            invoiceTable = new InvoiceTable(connectionString, CurrentLanguage, sqlDb);
            emailScheduleTable = new EmailScheduleTable(connectionString, CurrentLanguage, sqlDb);
            remindTable = new RemindTable(connectionString, CurrentLanguage, sqlDb);
            currentCardType = -1;
            organizationID = 0;
        }
        /// <summary>
        /// Конструктор для работы с занесенной записью + конкретной записью блока(устрело)
        /// </summary>
        /// <param name="connectionStringTMP">Строка подключения</param>
        /// <param name="dataBlockId">dataBlockId</param>
        /// <param name="dataRecordId">dataRecordId</param>
        /// <param name="Current_Language">Язык</param>
        [Obsolete("вроде нигде не используется уже", false)]
        public DataBlock(string connectionStringTMP, int dataBlockId, int dataRecordId, string Current_Language)//Работа с уже занесенной записью
        {
            //тестовое подключение
            sqlDb = new SQLDB(connectionStringTMP);
            ///////

            connectionString = connectionStringTMP;
            DATA_BLOCK_ID = dataBlockId;
            DATA_BLOCK_ID_PREVIOUS = -1;
            CurrentLanguage = Current_Language;
            dataRecord = new DataRecords(connectionStringTMP, dataBlockId, dataRecordId, Current_Language, sqlDb);
            vehicleUnitInfo = new VehicleUnitInfo(connectionString, Current_Language, sqlDb);
            cardUnitInfo = new CardUnitInfo(connectionString, Current_Language, sqlDb);
            plfUnitInfo = new PLFUnitInfo(connectionString, Current_Language, sqlDb);
            cardsTable = new CardsTable(connectionString, Current_Language, sqlDb);
            organizationTable = new OrganizationTable(connectionString, Current_Language, sqlDb);
            usersTable = new UsersTables(connectionString, CurrentLanguage, sqlDb);
            stringTable = new StringTable(connectionString, CurrentLanguage);
            vehiclesTables = new VehiclesTable(connectionString, CurrentLanguage, sqlDb);
            criteriaTable = new CriteriaTable(connectionString, CurrentLanguage, sqlDb);
            deviceTable = new DeviceTable(connectionString, CurrentLanguage, sqlDb);
            //historyTable = new HistoryTable(connectionString, CurrentLanguage, sqlDb);
            reportsTable = new ReportsTable(connectionString, CurrentLanguage, sqlDb);
            invoiceTable = new InvoiceTable(connectionString, CurrentLanguage, sqlDb);
            emailScheduleTable = new EmailScheduleTable(connectionString, CurrentLanguage, sqlDb);
            remindTable = new RemindTable(connectionString, CurrentLanguage, sqlDb);
            currentCardType = -1;
            organizationID = 0;
        }
        /// <summary>
        /// Конструктор(рекомендовано пользоваться этим)
        /// </summary>
        /// <param name="connectionStringTMP">Строка подключения</param>
        /// <param name="Current_Language">Язык</param>
        public DataBlock(string connectionStringTMP, string Current_Language)
        {
            //тестовое подключение
            sqlDb = new SQLDB(connectionStringTMP);
            ///////

            connectionString = connectionStringTMP;
            DATA_BLOCK_ID = -1;
            DATA_BLOCK_ID_PREVIOUS = -1;
            CurrentLanguage = Current_Language;
            vehicleUnitInfo = new VehicleUnitInfo(connectionString, Current_Language, sqlDb);
            cardUnitInfo = new CardUnitInfo(connectionString, Current_Language, sqlDb);
            plfUnitInfo = new PLFUnitInfo(connectionString, Current_Language, sqlDb);
            cardsTable = new CardsTable(connectionString, Current_Language, sqlDb);
            organizationTable = new OrganizationTable(connectionString, Current_Language, sqlDb);
            usersTable = new UsersTables(connectionString, CurrentLanguage, sqlDb);
            stringTable = new StringTable(connectionString, CurrentLanguage);
            vehiclesTables = new VehiclesTable(connectionString, CurrentLanguage, sqlDb);
            criteriaTable = new CriteriaTable(connectionString, CurrentLanguage, sqlDb);
            deviceTable = new DeviceTable(connectionString, CurrentLanguage, sqlDb);
            //historyTable = new HistoryTable(connectionString, CurrentLanguage, sqlDb);
            reportsTable = new ReportsTable(connectionString, CurrentLanguage, sqlDb);
            invoiceTable = new InvoiceTable(connectionString, CurrentLanguage, sqlDb);
            emailScheduleTable = new EmailScheduleTable(connectionString, CurrentLanguage, sqlDb);
            remindTable = new RemindTable(connectionString, CurrentLanguage, sqlDb);
            currentCardType = -1;
            organizationID = 0;
        }
        /// <summary>
        /// Конструктор(рекомендовано пользоваться этим если следующее действие - парсинг)
        /// </summary>
        /// <param name="connectionStringTMP">строка подключения</param>
        /// <param name="dataBlockId">dataBlockId</param>
        /// <param name="Current_Language">язык</param>
        /// <param name="orgId">ID организации</param>
        public DataBlock(string connectionStringTMP, int dataBlockId, string Current_Language, int orgId)//Внести запись и начать работать
        {
            //тестовое подключение
            sqlDb = new SQLDB(connectionStringTMP);
            ///////

            SQLDB sqlDB = new SQLDB(connectionStringTMP);
            int dataIdTemp = sqlDB.checkTableExistence("fn_data_block", "DATA_BLOCK_ID", dataBlockId);
            if (dataIdTemp == 0)
                throw (new Exception("There is no Data Block with id " + dataBlockId.ToString()));

            connectionString = connectionStringTMP;
            DATA_BLOCK_ID = dataBlockId;
            DATA_BLOCK_ID_PREVIOUS = -1;
            CurrentLanguage = Current_Language;
            dataRecord = new DataRecords(connectionStringTMP, dataBlockId, Current_Language, sqlDb);
            vehicleUnitInfo = new VehicleUnitInfo(connectionString, Current_Language, sqlDb);
            cardUnitInfo = new CardUnitInfo(connectionString, Current_Language, sqlDb);
            plfUnitInfo = new PLFUnitInfo(connectionString, Current_Language, sqlDb);
            cardsTable = new CardsTable(connectionString, Current_Language, sqlDb);
            organizationTable = new OrganizationTable(connectionString, Current_Language, sqlDb);
            usersTable = new UsersTables(connectionString, CurrentLanguage, sqlDb);
            stringTable = new StringTable(connectionString, CurrentLanguage);
            vehiclesTables = new VehiclesTable(connectionString, CurrentLanguage, sqlDb);
            criteriaTable = new CriteriaTable(connectionString, CurrentLanguage, sqlDb);
            deviceTable = new DeviceTable(connectionString, CurrentLanguage, sqlDb);
            // historyTable = new HistoryTable(connectionString, CurrentLanguage, sqlDb);
            reportsTable = new ReportsTable(connectionString, CurrentLanguage, sqlDb);
            invoiceTable = new InvoiceTable(connectionString, CurrentLanguage, sqlDb);
            emailScheduleTable = new EmailScheduleTable(connectionString, CurrentLanguage, sqlDb);
            currentCardType = -1;
            organizationID = orgId;
        }
        /// <summary>
        /// Устанавливает ID организации(нужно сделать перед парсингом, если не использовался конструктор с указанием orgId
        /// </summary>
        /// <param name="orgId">ID организации</param>
        public void SetOrgIdForParse(int orgId)
        {
            organizationID = orgId;
        }
        /// <summary>
        /// Устанавливает, какой dataBlockId разбирать
        /// </summary>
        /// <param name="dataBlockId">dataBlockId</param>
        public void SetDataBlockIdForParse(int dataBlockId)
        {
            DATA_BLOCK_ID = dataBlockId;
            DATA_BLOCK_ID_PREVIOUS = -1;
            dataRecord = new DataRecords(connectionString, dataBlockId, CurrentLanguage, sqlDb);
        }
        /// <summary>
        /// Получить имя загруженного файла
        /// </summary>
        /// <param name="dataBlockId">ID блока данных</param>
        /// <returns></returns>
        public string GetDataBlock_FileName(int dataBlockId)
        {
            SQLDB_Records sqlDB_rec = new SQLDB_Records(connectionString, sqlDb.GETMYSQLCONNECTION());
            string dataBlockFilename;
            try
            {
                int dataBlockFilenameParamID = sqlDb.getParamId("DataBlock_FileName");
                dataBlockFilename = sqlDB_rec.Get_ParamValue(dataBlockId, dataBlockFilenameParamID);
            }
            catch
            {
                return "Имя недоступно";
            }
            return dataBlockFilename;
        }
        /// <summary>
        /// Получить количество байт загруженного файла
        /// </summary>
        /// <param name="dataBlockId">ID блока данных</param>
        /// <returns>количество байт в файле</returns>
        public string GetDataBlock_BytesCount(int dataBlockId)
        {
            SQLDB_Records sqlDB_rec = new SQLDB_Records(connectionString, sqlDb.GETMYSQLCONNECTION());
            string dataBlockRecordsCount;
            try
            {
                int dataBlockRecordsCountParamID = sqlDb.getParamId("DataBlock_BytesCount");
                dataBlockRecordsCount = sqlDB_rec.Get_ParamValue(dataBlockId, dataBlockRecordsCountParamID);
            }
            catch
            {
                return "Значение недоступно";
            }
            return dataBlockRecordsCount;
        }
        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="dataTemp">Файл в битовом массиве</param>
        /// <param name="fileName">Имя файла</param>
        /// <returns>ID блока данных</returns>
        public int AddData(byte[] dataTemp, string fileName)
        {
            if (organizationID <= 0)
                throw new Exception("Не задана организация для добавления блока данных!");
            int orgCardId = cardsTable.GetAllCardIds(organizationID, cardsTable.orgInitCardTypeId)[0];
            return AddData(dataTemp, fileName, orgCardId);
        }
        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="dataTemp">Файл в битовом массиве</param>
        /// <param name="fileName">Имя файла</param>
        /// <returns>ID блока данных</returns>
        public int AddData(int orgId, byte[] dataTemp, string fileName)
        {
            int orgCardId = cardsTable.GetAllCardIds(orgId, cardsTable.orgInitCardTypeId)[0];
            return AddData(dataTemp, fileName, orgCardId);
        }
        /// <summary>
        /// Загрузить файл PLF
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <param name="dataTemp">Файл в битовом массиве</param>
        /// <param name="fileName">Имя файла</param>
        /// <param name="selectedDriverCardId">ID карты водителя, кому загружается информация PLF</param>
        /// <returns>ID блока данных</returns>
        public int AddPlfTypeData(int orgId, byte[] dataTemp, string fileName, int selectedDriverCardId)
        {
            int orgCardId = cardsTable.GetAllCardIds(orgId, cardsTable.orgInitCardTypeId)[0];
            int dataBlockId = AddData(dataTemp, fileName, orgCardId);
            int cardTypeParamId = sqlDb.AddParam("cardType", 0, 255);
            sqlDb.AddDataRecord(selectedDriverCardId.ToString(), dataBlockId, cardTypeParamId);

            return dataBlockId;
        }
        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="dataTemp">Файл в битовом массиве</param>
        /// <param name="fileName">Имя файла</param>
        /// <param name="cardId">Id карты</param>
        /// <returns>ID блока данных</returns>
        private int AddData(byte[] dataTemp, string fileName, int cardId)
        {
            CRC32.Crc32 crc32 = new CRC32.Crc32();
            //SQLDB sqlDB = new SQLDB(connectionString);
            string bytesCount = dataTemp.Length.ToString();

            crc32.ComputeHash(dataTemp);//Этот метод может иметь повторы при большом колличестве файлов. Если будут проблемы - заменить на md5
            uint crc = crc32.CrcValue;
            List<int> dataBlockCrcIds = new List<int>();
            dataBlockCrcIds = GetDataBlockId_byCRC32(crc);
            if (dataBlockCrcIds.Count > 0)
                throw new Exception("Этот файл уже загружен!");

            //sqlDB.OpenConnection();
            int dataBlock_fileNameID = sqlDb.AddParam("DataBlock_FileName", 0, 255);
            int dataBlock_bytesCount = sqlDb.AddParam("DataBlock_BytesCount", 0, 255);
            int dataBlock_CRC32 = sqlDb.AddParam("DataBlock_CRC32", 0, 255);
            //sqlDB.CloseConnection();
            dataTemp = ZipBytes(dataTemp);//Зипуем файл для добавления в базу данных!
            int dataIdTemp = sqlDb.AddDataBlock(dataTemp);
            //sqlDB.OpenConnection();
            sqlDb.AddDataRecord(fileName, dataIdTemp, dataBlock_fileNameID);
            sqlDb.AddDataRecord(bytesCount, dataIdTemp, dataBlock_bytesCount);
            sqlDb.AddDataRecord(crc.ToString(), dataIdTemp, dataBlock_CRC32);
            if (cardId != -1)
                sqlDb.SetDataBlock_CardId(dataIdTemp, cardId);
            //sqlDB.CloseConnection();

            if (dataIdTemp == -1)
            {
                throw (new Exception("Can't add data block from byte[]"));
            }
            else
            {
                DATA_BLOCK_ID_PREVIOUS = DATA_BLOCK_ID;
                DATA_BLOCK_ID = dataIdTemp;
                dataRecord = new DataRecords(connectionString, DATA_BLOCK_ID, CurrentLanguage, sqlDb);
                return dataIdTemp;
            }
        }
        /// <summary>
        /// Распаковать байты
        /// </summary>
        /// <param name="_bytes">Массив запакованых байт</param>
        /// <returns>Массив распакованых байт</returns>
        private byte[] UnZipBytes(byte[] _bytes)
        {
            byte[] bar = Compressor.Compressor.Decompress(_bytes);
            return bar;
        }
        /// <summary>
        /// Заспаковать байты
        /// </summary>
        /// <param name="_bytes">Массив распакованых байт</param>
        /// <returns>Массив запакованых байт</returns>
        private byte[] ZipBytes(byte[] _bytes)
        {
            byte[] bar = Compressor.Compressor.Compress(_bytes);
            return bar;
        }
        [Obsolete("Метод устарел, либо требует доработки", false)]
        public int DeleteDataRecords()
        {
            SQLDB sqlDB = new SQLDB(connectionString);
            int deletedCount;

            sqlDB.OpenConnection();
            sqlDB.OpenTransaction();

            deletedCount = dataRecord.DeleteAllData(sqlDB);
            sqlDB.SetDataBlockState(DATA_BLOCK_ID, 3);

            sqlDB.CommitConnection();
            sqlDB.CloseConnection();

            return deletedCount;
        }
        [Obsolete("Метод устарел, либо требует доработки", false)]
        public int DeleteDataBlockAndRecords()
        {
            SQLDB sqlDB = new SQLDB(connectionString);
            int deletedCount = 3;
            try
            {
                sqlDB.OpenConnection();
                sqlDB.OpenTransaction();

                // deletedCount = dataRecord.DeleteAllData(sqlDB);
                sqlDB.DeleteAllDataRecordsFast(DATA_BLOCK_ID);
                sqlDB.DeleteDataBlock(DATA_BLOCK_ID);

                sqlDB.CommitConnection();
                sqlDB.CloseConnection();
            }
            catch (Exception ex)
            {
                sqlDB.RollbackConnection();
                sqlDB.CloseConnection();
                throw ex;
            }
            return deletedCount + 1;
        }
        [Obsolete("Метод устарел, либо требует доработки", false)]
        public string GetDataState()
        {
            SQLDB sqlDB = new SQLDB(connectionString);
            int blockStateId;//в таблице fn_data_block
            int STRIddataBlockState;//в таблице fd_data_block_state
            string blockStateMessage;

            sqlDB.OpenConnection();
            blockStateId = sqlDB.GetDataBlockState(DATA_BLOCK_ID);
            STRIddataBlockState = sqlDB.GetSTRIdDataBlockStateName(blockStateId);
            blockStateMessage = sqlDB.GetString(STRIddataBlockState, CurrentLanguage);
            sqlDB.CloseConnection();

            return blockStateMessage;
        }
        /// <summary>
        /// Получает состояние Блока данных
        /// </summary>
        /// <param name="dataBlockId">ID блока данных</param>
        /// <returns>Состояние</returns>
        public string GetDataBlockState(int dataBlockId)
        {
            //SQLDB sqlDB = new SQLDB(connectionString);
            int blockStateId;//в таблице fn_data_block
            int STRIddataBlockState;//в таблице fd_data_block_state
            string blockStateMessage;

            //sqlDB.OpenConnection();
            blockStateId = sqlDb.GetDataBlockState(dataBlockId);
            STRIddataBlockState = sqlDb.GetSTRIdDataBlockStateName(blockStateId);
            blockStateMessage = sqlDb.GetString(STRIddataBlockState, CurrentLanguage);
            //sqlDB.CloseConnection();

            return blockStateMessage;
        }
        /// <summary>
        /// Парсить блок данных
        /// </summary>
        /// <param name="userId">ID пользователя, от имени которого вызывается этот метод</param>
        /// <returns>разобранный обьект</returns>
        public object ParseRecords(int userId)
        {
            return ParseRecords(false, "", userId);
        }
        /// <summary>
        /// Парсить блок данных
        /// </summary>
        /// <param name="generateXML">генерировать ли XML файл</param>
        /// <param name="output">путь, куда XML сохранять</param>
        /// <param name="userId">ID пользователя, от имени которого вызывается этот метод</param>
        /// <returns>разобранный обьект</returns>
        public object ParseRecords(bool generateXML, string output, int userId)
        {
            Exception noOrg = new Exception("No organization entered");
            Exception CardVehicleError = new Exception("Ошибка в базе данных. Нет связи Карта - ТС!");

            if (organizationID == 0)
                throw noOrg;

            if (DATA_BLOCK_ID == -1)
                throw (new Exception("Can't find this Data Block"));
            else
            {
                //SQLDB sqlDB = new SQLDB(connectionString);
                SQLDB_Records sqlDB_rec = new SQLDB_Records(connectionString, sqlDb.GETMYSQLCONNECTION());
                ReflectObjectToTableClass reflectedItemsList;
                Type type = null;
                object myParseObject = new object();
                PARSER.DDDParser dddParser = new DDDParser();
                string fileName = "";

                try
                {

                    fileName = sqlDB_rec.Get_ParamValue(DATA_BLOCK_ID, "DataBlock_FileName");
                    byte[] blockDataBlob = sqlDb.GetDataBlock(DATA_BLOCK_ID);
                    blockDataBlob = UnZipBytes(blockDataBlob);//АнЗипим файл!
                    dddParser.ParseFile(blockDataBlob, fileName);

                    int cardType = dddParser.GetCardType();
                    currentCardType = cardType;

                    //test
                    //OpenConnection();
                    //OpenTransaction();
                    //

                    if (cardType == 0)// 0 - card(driver)
                    {
                        int cardId;
                        if (dddParser.cardUnitClass.ef_identification.driverCardHolderIdentification != null)
                        {
                            string drName = dddParser.cardUnitClass.ef_identification.driverCardHolderIdentification.cardHolderName.ToString();
                            string drNumber = dddParser.cardUnitClass.ef_identification.cardIdentification.cardNumber.ToString();
                            //sqlDB.OpenConnection();
                            cardId = cardsTable.GetCardId(drName, drNumber, cardsTable.driversCardTypeId);
                            if (cardId <= 0)
                            {
                                //cardId = cardsTable.CreateNewCard(drName, drNumber, cardsTable.driversCardTypeId, organizationID, "Init DataBlockId = " + DATA_BLOCK_ID, userId, 1);
                            }
                            sqlDb.SetDataBlock_CardId(DATA_BLOCK_ID, cardId);
                            //sqlDB.CloseConnection();
                        }
                        else
                        {
                            //sqlDB.OpenConnection();
                            sqlDb.SetDataBlockState(DATA_BLOCK_ID, 4);
                            //sqlDB.CloseConnection();
                            throw new Exception("Поддерживаются только водительские карты и информация с бортового устройства.");
                        }
                        type = dddParser.cardUnitClass.GetType();
                        myParseObject = dddParser.cardUnitClass;
                    }
                    else if (cardType == 1)//vehicle
                    {
                        /////////////////////////
                        int vehicleId;
                        if (dddParser.vehicleUnitClass.vehicleOverview.vehicleRegistrationIdentification != null)
                        {
                            int cardId;
                            string vehRegNumber = dddParser.vehicleUnitClass.vehicleOverview.vehicleRegistrationIdentification.vehicleRegistrationNumber.ToString();
                            string vin = dddParser.vehicleUnitClass.vehicleOverview.vehicleIdentificationNumber.ToString();
                            cardId = cardsTable.GetCardId(vehRegNumber, vin, cardsTable.vehicleCardTypeId);
                            vehicleId = vehiclesTables.GetVehicleId_byVinRegNumbers(vin, vehRegNumber);
                            if ((vehicleId <= 0 && cardId > 0) || (vehicleId > 0 && cardId <= 0))
                                throw CardVehicleError;

                            if (cardId <= 0)
                            {
                                string marka = "";
                                DateTime BLOCKED = dddParser.vehicleUnitClass.vehicleOverview.vuDownloadablePeriod.maxDownloadableTime.getTimeRealDate();
                                int grId = cardsTable.GetAllGroupIds(organizationID,0)[0];
                                cardId = cardsTable.CreateNewCard(vehRegNumber, vin, cardsTable.vehicleCardTypeId, organizationID, userId, "Init DataBlockId = " + DATA_BLOCK_ID, userId, grId);
                                //vehiclesTables.OpenConnection();
                                vehicleId = vehiclesTables.AddNewVehicle(vehRegNumber, marka, vin, 0, 1, cardId, BLOCKED, 1);
                                //vehiclesTables.CloseConnection();
                                // SetAllVehiclesIDS(vehicleId);
                            }
                            sqlDb.SetDataBlock_CardId(DATA_BLOCK_ID, cardId);
                        }
                        /////////////////////////
                        type = dddParser.vehicleUnitClass.GetType();
                        myParseObject = dddParser.vehicleUnitClass;
                    }
                    else if (cardType == 2)//plf
                    {
                        //////////////////////устанавливаем PLF карту нужного водителя. Незнаю почему именно здесь, но так получилось.
                        int plfDriversCardType = sqlDB_rec.Get_DataBlockCardType(DATA_BLOCK_ID);
                        sqlDb.SetDataBlock_CardId(DATA_BLOCK_ID, plfDriversCardType);
                        // sqlDB.OpenConnection();
                        int cardTypeParamId = sqlDb.AddParam("cardType", 0, 255);
                        //sqlDB.OpenConnection();
                        sqlDb.DeleteDataRecord(DATA_BLOCK_ID, cardTypeParamId);
                        // sqlDB.CloseConnection();
                        //////////////////////
                        type = dddParser.plfUnitClass.GetType();
                        myParseObject = dddParser.plfUnitClass;
                    }
                    else if (cardType == -1)
                        throw new Exception("Информация непригодна для разбора(не опознан тип карты");

                    //sqlDB.OpenConnection();
                    SetParseBDate(sqlDb);
                    //sqlDB.CloseConnection();

                    List<ReflectObjectToTableClass> allRecordsToAdd = new List<ReflectObjectToTableClass>();

                    foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        reflectedItemsList = new ReflectObjectToTableClass();
                        string fieldName = pi.Name;
                        object field2 = pi.GetValue(myParseObject, null);

                        if (field2 != null)
                        {
                            reflectedItemsList.ReflectObjectToTable(fieldName, field2);//Не удалять!
                            allRecordsToAdd.Add(AddRecords(reflectedItemsList));//не удалять
                        }
                    }

                    foreach (ReflectObjectToTableClass recordList in allRecordsToAdd)
                    {
                        dataRecord.AddDataArray(recordList.reflectedItemsList);
                    }

                    //sqlDB.OpenConnection();
                    SetParseEDate(sqlDb);
                    sqlDb.SetDataBlockState(DATA_BLOCK_ID, 2);
                    int dataBlockParseRecords = sqlDb.SetDataBlockParseRecords(DATA_BLOCK_ID);
                    Console.WriteLine("\n\r" + dataBlockParseRecords.ToString() + " records added");
                    //XML GENERATING
                    if (generateXML)
                        dddParser.GenerateXmlFile(output);
                    //
                    //Добавляем лог для каждого типа блока данных свой.
                    string logNote;
                    HistoryTable historyTable = new HistoryTable(connectionString, CurrentLanguage, sqlDb);
                    if (currentCardType == 0)//driver
                    {
                        string drName = dddParser.cardUnitClass.ef_identification.driverCardHolderIdentification.cardHolderName.ToString();
                        string drNumber = dddParser.cardUnitClass.ef_identification.cardIdentification.cardNumber.ToString();
                        logNote = "Driver: " + drName + "(" + drNumber + ")" + ", records number: " + dataBlockParseRecords.ToString();
                        historyTable.AddHistoryRecord("fn_data_block", "DATA_BLOCK_ID", DATA_BLOCK_ID, userId, historyTable.DDDDriversDataBlockLoaded, logNote, sqlDb);
                    }
                    if (currentCardType == 1)//vehicle
                    {
                        string logVehRegNumber = dddParser.vehicleUnitClass.vehicleOverview.vehicleRegistrationIdentification.vehicleRegistrationNumber.ToString();
                        string logVin = dddParser.vehicleUnitClass.vehicleOverview.vehicleIdentificationNumber.ToString();
                        logNote = "Vehicle: " + logVehRegNumber + "(" + logVin + ")" + ", records number: " + dataBlockParseRecords.ToString();
                        historyTable.AddHistoryRecord("fn_data_block", "DATA_BLOCK_ID", DATA_BLOCK_ID, userId, historyTable.DDDVehiclesDataBlockLoaded, logNote, sqlDb);
                    }
                    if (currentCardType == 2)//plf
                    {
                        string vehPlfIdent = dddParser.plfUnitClass.VEHICLE;
                        string plfDeviceId = dddParser.plfUnitClass.ID_DEVICE;
                        string period = dddParser.plfUnitClass.START_PERIOD.GetSystemTime().ToShortDateString() + " - " + dddParser.plfUnitClass.END_PERIOD.GetSystemTime().ToShortDateString();
                        logNote = "PLF File: " + vehPlfIdent + "(" + plfDeviceId + ")" + ", period: " + period + ", records number: " + dataBlockParseRecords.ToString();
                        historyTable.AddHistoryRecord("fn_data_block", "DATA_BLOCK_ID", DATA_BLOCK_ID, userId, historyTable.PLFDataBlockLoaded, logNote, sqlDb);
                    }
                    //
                    //sqlDb.CommitConnection();
                    //sqlDb.CloseConnection();
                }
                catch (Exception ex)
                {
                    //sqlDb.RollbackConnection();
                    //sqlDb.CloseConnection();
                    throw ex;
                }
                finally
                {
                }
                return myParseObject;
            }
        }
        /// <summary>
        /// Установить дату начала разбора файла
        /// </summary>
        /// <param name="sqlDb">обьект подключения</param>
        private void SetParseBDate(SQLDB sqlDb)
        {
            DateTime dt = sqlDb.SetCurrentTime("fn_data_block", "DATA_BLOCK_ID", DATA_BLOCK_ID, "PARSE_BDATE");
            Console.WriteLine("\r\nParse Begin Time " + dt.ToString("dd-MM-yyyy HH:mm:ss"));
        }
        /// <summary>
        /// Установить дату окончания разбора файла
        /// </summary>
        /// <param name="sqlDb">обьект подключения</param>
        private void SetParseEDate(SQLDB sqlDb)
        {
            DateTime dt = sqlDb.SetCurrentTime("fn_data_block", "DATA_BLOCK_ID", DATA_BLOCK_ID, "PARSE_EDATE");
            Console.WriteLine("Parse End Time " + dt.ToString("dd-MM-yyyy HH:mm:ss"));
        }
        /// <summary>
        /// Получить  название типа БлокаДанных
        /// </summary>
        /// <param name="dataBlockId">Id блока данных</param>
        /// <returns>Название типа БлокаДанных</returns>
        public string GetCardTypeName(int dataBlockId)
        {
            int returnValue = -1;
            Exception unknownCardType = new Exception("Неизвестный тип карты");
            SQLDB_Records sqlDB_records = new SQLDB_Records(connectionString, sqlDb.GETMYSQLCONNECTION());
            returnValue = sqlDB_records.Get_DataBlockCardType(dataBlockId);

            switch (returnValue)
            {
                case 0:
                    return "Card";
                case 1:
                    return "Vehicle";
                case 2:
                    return "Plf";
                default:
                    return unknownCardType.Message;
            }
        }

        //-------------------------------------------------------------Parser data-- 

        private struct Arrays
        {
            public string arrayName;
            public int maxCount;
        }
        [Obsolete("раньше должно было приводить к общему виду все названия, сейчас это вроде не требуется", false)]
        private List<ReflectionClass> ParseRecord(List<ReflectionClass> reflectionClass)
        {
            List<ReflectionClass> returnReflectionClass = new List<ReflectionClass>();
            string[] splitedString;
            List<Arrays> arrayNames = new List<Arrays>();
            Arrays oneList;

            foreach (ReflectionClass r in reflectionClass)
            {
                splitedString = r.name.Split(new char[] { '.' });
                for (int i = 0; i < splitedString.Length; i++)
                {
                    if (splitedString[i].Contains('[') && splitedString[i].Contains(']'))
                    {
                        string arrayFullName = "";
                        oneList = new Arrays();
                        string[] splitNameNumber = splitedString[i].Split((new char[] { '[', ']' }), StringSplitOptions.RemoveEmptyEntries);

                        for (int j = 0; j < r.paramStructure.Count; j++)
                        {
                            arrayFullName += r.paramStructure[j] + ".";
                        }
                        arrayFullName += splitNameNumber[0];

                        oneList.arrayName = arrayFullName;
                        oneList.maxCount = Convert.ToInt32(splitNameNumber[1]);

                        int result = arrayNames.FindIndex(
                            delegate(Arrays arrayName)
                            {
                                if (arrayName.arrayName.Equals(arrayFullName))
                                    return true;
                                return false;
                            }
                        );

                        if (result != -1)
                        {
                            if (arrayNames[result].maxCount < oneList.maxCount)
                            {
                                Arrays temp = new Arrays();
                                temp.arrayName = oneList.arrayName;
                                temp.maxCount = oneList.maxCount;
                                arrayNames[result] = temp;
                            }
                        }
                        else
                        {
                            arrayNames.Add(oneList);
                        }
                    }
                    r.paramStructure.Add(splitedString[i]);
                }
                returnReflectionClass.Add(r);
            }
            foreach (Arrays n in arrayNames)
            {
                ReflectionClass r = new ReflectionClass();
                r.name = n.arrayName + ".arrayCount";
                r.value = (n.maxCount + 1).ToString();
                returnReflectionClass.Add(r);
            }
            return returnReflectionClass;
        }
        /// <summary>
        /// Создает список параметров для разобранного файла, либо бедет айди, если параметр есть
        /// либо создает новый
        /// </summary>
        /// <param name="reflectionClass">Разобранный обьект</param>
        /// <returns>список параметров для разобранного файла с ID</returns>
        private ReflectObjectToTableClass AddRecords(ReflectObjectToTableClass reflectionClass)
        {
            Params parameters = new Params();
            ReflectObjectToTableClass myAddedParameters = new ReflectObjectToTableClass();
            //SQLDB sqlDB = new SQLDB(connectionString);
            string lastCashName = "";
            int paramId = -1;

            // sqlDB.OpenConnection();
            //sqlDB.OpenTransaction();
            Console.WriteLine("Adding Parameters....");
            foreach (int n in reflectionClass.getAllItemsLevels())
            {
                foreach (ReflectionClass item in reflectionClass.GetItemsByLevel(n))
                {
                    /*Новая версия с развитым кешем*/
                    paramId = GetParamIdFromCashe(item.name, -1, true);
                    if (paramId == -1)
                    {
                        paramId = parameters.AddParam(item.name, item.GetParentParamName(), 0, sqlDb);
                        if (paramId == -1)
                        {
                            throw new Exception("Error adding parameters!");
                        }
                        GetParamIdFromCashe(item.name, paramId, false);
                    }
                    /*Новая версия с развитым кешем*/

                    ReflectionClass r = new ReflectionClass();
                    r.name = item.name;
                    r.PARAM_ID = paramId;  //старая версия с одним кешем

                    r.value = item.value;
                    myAddedParameters.reflectedItemsList.Add(r);
                }
            }
            return myAddedParameters;
        }
        private List<string> namesForCashe;
        private List<int> idsForCashe;
        private int GetParamIdFromCashe(string name, int id, bool checkOnly)//при переполнении сделать выталкивание а не обнуление
        {
            int MaxCasheCount = 30;
            KeyValuePair<string, int> thisItem = new KeyValuePair<string, int>(name, id);
            if (namesForCashe == null)
            {
                namesForCashe = new List<string>();
                idsForCashe = new List<int>();
            }
            int index = namesForCashe.IndexOf(thisItem.Key);
            if (index > -1)
            {
                return idsForCashe[index];
            }
            else
            {
                if (checkOnly)
                    return -1;
                if (namesForCashe.Count > MaxCasheCount)
                {
                    namesForCashe = new List<string>();
                    idsForCashe = new List<int>();
                }
                namesForCashe.Add(thisItem.Key);
                idsForCashe.Add(thisItem.Value);
            }
            return -1;
        }

        private string deleteBackingName(string field)
        {
            string returnString = "";
            string[] returnStrings;
            string[] splitStrings = { "<", ">", "k__BackingField" };

            returnStrings = field.Split(splitStrings, StringSplitOptions.RemoveEmptyEntries);

            foreach (string s in returnStrings)
                returnString += s;

            return returnString;
        }
        /// <summary>
        /// Получить все блоки данных для организации
        /// </summary>
        /// <param name="UserId">ID организации</param>
        /// <returns> List<int></returns>
        public List<int> GetAllDataBlockIds(int UserId)
        {
            List<int> gettedIds = new List<int>();
            gettedIds = sqlDb.GetAllDataBlocksId(UserId);
            return gettedIds;
        }
        /// <summary>
        /// Получить все неразобранные блоки данных для организации
        /// </summary>
        /// <param name="orgId">ID организации</param>
        /// <returns>List<int></returns>
        public List<int> GetAllUnparsedDataBlockIDs(int orgId)
        {
            List<int> gettedIds = new List<int>();
            int cardId = 0;
            List<int> cardIds = cardsTable.GetAllCardIds(orgId, cardsTable.orgInitCardTypeId);
            if (cardIds.Count == 0) { return gettedIds; }
            else
            {
                cardId = cardsTable.GetAllCardIds(orgId, cardsTable.orgInitCardTypeId)[0];
            }
            gettedIds = cardsTable.GetAllDataBlockIds_byCardId(cardId);
            return gettedIds;
        }
        /// <summary>
        /// Получить все разобранные блоки данных для организации
        /// </summary>
        /// <param name="UserId">ID организации</param>
        /// <returns>List<int></returns>
        public List<int> GetAllParsedDataBlockIDs(int UserId)
        {
            List<int> gettedIds = new List<int>();
            int cardType = -1;
            List<int> dataBlockIdsSorted = new List<int>();
            gettedIds = sqlDb.GetAllParsedDataBlockIDs(UserId);

            SQLDB_Records sqldb_records = new SQLDB_Records(connectionString, sqlDb.GETMYSQLCONNECTION());
            int middle = 0;
            foreach (int id in gettedIds)
            {
                cardType = sqldb_records.Get_DataBlockCardType(id);
                if (cardType == 0)
                {
                    dataBlockIdsSorted.Insert(0, id);
                    middle++;
                }
                else
                    if (cardType == 2)
                        dataBlockIdsSorted.Add(id);
                    else
                        if (cardType == 1)
                            dataBlockIdsSorted.Insert(middle, id);
            }

            return gettedIds;
        }
        [Obsolete("", false)]
        public List<string> GetAllDriverNames(int UserId)
        {
            List<int> gettedIds = new List<int>();
            List<string> gettedNames = new List<string>();

            SQLDB sqlDB = new SQLDB(connectionString);
            SQLDB_Records sqlDB_Records = new SQLDB_Records(connectionString);

            sqlDB.OpenConnection();
            gettedIds = sqlDB.GetAllDataBlocksId_byCardType(UserId, 0);
            sqlDB.CloseConnection();

            gettedNames = sqlDB_Records.Get_DriverNames_ByDataBlockIdList(gettedIds);

            return gettedNames;
        }
        [Obsolete("", false)]
        public List<string> GetAllVehiclesNumber(int UserId)
        {
            List<int> gettedIds = new List<int>();
            List<string> gettedVehicleNumbers = new List<string>();

            //SQLDB sqlDB = new SQLDB(connectionString);
            SQLDB_Records sqlDB_Records = new SQLDB_Records(connectionString, sqlDb.GETMYSQLCONNECTION());

            sqlDb.OpenConnection();
            gettedIds = sqlDb.GetAllDataBlocksId_byCardType(UserId, 1);
            gettedVehicleNumbers = sqlDB_Records.Get_VehicleNumbers_ByDataBlockIdList(gettedIds);
            sqlDb.CloseConnection();

            return gettedVehicleNumbers;
        }
        [Obsolete("", false)]
        public string GetDriversIdentificationNumber(string driversName)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString);
            int driversDataBlock;
            string driversIdentificationNumber;

            driversDataBlock = GetDataBlockIdByDriversName(driversName);
            driversIdentificationNumber = sqldbRecords.Get_DriversNumber(driversDataBlock);
            return driversIdentificationNumber;
        }
        [Obsolete("", false)]
        public int GetDataBlockIdByDriversName(string driversName)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString);
            int driversDataBlockId;
            string[] splittedName = driversName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string name = splittedName[0];
            string surname = splittedName[1];
            List<int> driversDataBlock = new List<int>();

            driversDataBlock = sqldbRecords.Get_DataBlockIdByDriversName(name, surname);
            if (driversDataBlock != null && driversDataBlock.Count > 0)
                driversDataBlockId = driversDataBlock[0];
            else
                throw new Exception("Данные не могут быть извлечены");
            return driversDataBlockId;
        }
        public List<int> GetDataBlockIdByVehicleNumber(string number)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString);
            List<int> dataBlockIdList = new List<int>();

            dataBlockIdList = sqldbRecords.Get_DataBlockIdByVehicleNumber(number);

            return dataBlockIdList;
        }
        public List<int> GetDataBlockId_byFilenameAndBytesCount(string filename, int bytesCount)
        {
            List<int> dataBlockIds = new List<int>();
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString);

            dataBlockIds = sqldbRecords.Get_DataBlockIdByFileNameAndBytesCount(filename, bytesCount);

            return dataBlockIds;
        }
        public List<int> GetDataBlockId_byCRC32(uint crc)
        {
            List<int> dataBlockIds = new List<int>();
            //SQLDB sqlDB = new SQLDB(connectionString);
            //sqlDB.OpenConnection();
            dataBlockIds = sqlDb.GetAllDataBlocksId_byCRC32(0, crc);
            //sqlDB.CloseConnection();
            return dataBlockIds;
        }
        [Obsolete("", false)]
        public List<int> GetAllDataBlockId_byCardType(int cardType)
        {
            List<int> gettedIds = new List<int>();
            SQLDB sqlDB = new SQLDB(connectionString);
            int UserId = 0;

            sqlDB.OpenConnection();
            gettedIds = sqlDB.GetAllDataBlocksId_byCardType(UserId, cardType);
            sqlDB.CloseConnection();

            return gettedIds;
        }
        [Obsolete("", false)]
        public string GetDriversCardIssuingMemberState(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString);
            string cardIssuingMemberState = sqldbRecords.Get_DriversCardIssuingMemberState(dataBlockId);
            string stateName;
            DDDClass.NationNumeric nationNumeric = new DDDClass.NationNumeric(Convert.ToSByte(cardIssuingMemberState));
            stateName = nationNumeric.ToString();
            return stateName;
        }
        public string GetDriversNameOrVehiclesNumberByBlockId(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString);
            string Name = "";
            try
            {
                Name = sqldbRecords.Get_DriversNameOrVehiclesNumberByBlockId(dataBlockId);
            }
            catch
            {
                return "Имя не задано!";
            }
            return Name;
        }
        /// <summary>
        /// Получить количество записей в блоке данных
        /// </summary>
        /// <param name="dataBlockId">ID блока данных</param>
        /// <returns>количество записей в блоке данных</returns>
        public int GetDataBlock_RecorsCount(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDb.GETMYSQLCONNECTION());
            int recordsCount;
            try
            {
                recordsCount = sqldbRecords.Get_DataBlock_RecordsCount(dataBlockId);
            }
            catch
            {
                return 0;
            }
            return recordsCount;
        }
        /// <summary>
        /// ПОлучить дату окончания разбора блока данных
        /// </summary>
        /// <param name="dataBlockId">ID блока данных</param>
        /// <returns>дата окончания разбора блока данных</returns>
        public string GetDataBlock_EDate(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDb.GETMYSQLCONNECTION());
            string gettedDate;
            gettedDate = sqldbRecords.Get_DataBlock_EDate(dataBlockId);
            return gettedDate;
        }
        public List<int> GetDataBlockIdByRecordsCount(int recordsCount)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString);
            List<int> dataBlockIdList = new List<int>();

            dataBlockIdList = sqldbRecords.Get_DataBlockIdByRecordsCount(recordsCount);

            return dataBlockIdList;
        }
        //_________________________________________
        [Obsolete("", false)]
        public List<DDDClass.ActivityChangeInfo> DriversActivityChangeInfo(int dataBlockId) // Траблы. Некоторые ActivityChangeInfo после расшифровки из String в Байт[] дает тока один элемент и идет эксепшн
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString);
            List<string> DriversActivityChangeInfo_getted = new List<string>();
            List<DDDClass.ActivityChangeInfo> DriversActivityChangeInfo = new List<DDDClass.ActivityChangeInfo>();

            string paramName = "ef_driver_activity_data.activityDailyRecords.activityChangeInfo";
            DDDClass.ActivityChangeInfo activityChangeInfo;
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding(); // конвертация строки в массив байт

            byte[] _bytes;

            sqldbRecords.OpenConnection();
            DriversActivityChangeInfo_getted = sqldbRecords.Get_AllParamsArray(dataBlockId, paramName);
            sqldbRecords.CloseConnection();

            foreach (string activity in DriversActivityChangeInfo_getted)
            {
                if (activity != "It's parent array")
                {
                    _bytes = encoding.GetBytes(activity);
                    activityChangeInfo = new DDDClass.ActivityChangeInfo(_bytes);
                    DriversActivityChangeInfo.Add(activityChangeInfo);
                }
            }
            return DriversActivityChangeInfo;
        }
        /// <summary>
        /// Получить тип карты блока данных
        /// </summary>
        /// <param name="dataBlockId">ID блока данных</param>
        /// <returns>тип карты блока данных</returns>
        public int GetDataBlock_CardType(int dataBlockId)
        {
            SQLDB_Records sqldbRecords = new SQLDB_Records(connectionString, sqlDb.GETMYSQLCONNECTION());
            int cardType;
            try
            {
                cardType = sqldbRecords.Get_DataBlockCardType(dataBlockId);
            }
            catch
            {
                return -1;
            }
            return cardType;
        }
        /// <summary>
        /// Инициализировать базу данных
        /// </summary>
        /// <param name="password">пароль - ввести qqq</param>
        public void InitDataBase(string password)
        {
            if (password == "qqq")
            {
                DBI sqlDB = new SQLDB(connectionString);
                try
                {
                    sqlDB.OpenConnection();
                    sqlDB.OpenTransaction();
                    sqlDB.DataBaseInit();
                    sqlDB.CommitConnection();
                }
                catch (Exception ex)
                {
                    sqlDB.RollbackConnection();
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    sqlDB.CloseConnection();
                }
            }
        }

        public void ADDCOUNTERTESTRECORD()//TO DELETE
        {
            SQLDB_Records sqlDB_rec = new SQLDB_Records(connectionString);
            sqlDB_rec.AddCounter();
        }
        /// <summary>
        /// Выгрузить из базы данных файл, из которого создан блок данных
        /// </summary>
        /// <param name="dataBlockId">ID блока данных</param>
        /// <returns>массив байт - файл ДДД или ПЛФ</returns>
        public byte[] GetDataBlock_BytesArray(int dataBlockId)
        {
            PARSER.DDDParser dddParser = new PARSER.DDDParser();

            byte[] blockDataBlob = sqlDb.GetDataBlock(dataBlockId);
            blockDataBlob = UnZipBytes(blockDataBlob);//АнЗипим файл!

            return blockDataBlob;
        }

        /*public int EditAnySTRIDValueTEST(string newValue, string STRID_NAME, string Language, string tableName, string primaryName, int primaryValue)
        {
            DBI sqlDB = new SQLDB(connectionString);

            return sqlDB.EditAnySTRIDValue(newValue, STRID_NAME, Language, tableName, primaryName, primaryValue);
        }*/

        //------------------------static

        public static bool checkDataBlock(byte[] _bytes)
        {
            DDDParser parser = new DDDParser();
            byte[] twoLetters = HexBytes.arrayCopy(_bytes, 0, 2);
            bool isItRight = false;
            if (parser.checkWhatCardIsIt(twoLetters) != -1)
                isItRight = true;
            return isItRight;
        }

    }
}
