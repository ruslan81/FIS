using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using System.Configuration;
using BLL;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Возвращает DataSets для отчетов
/// </summary>
public class ReportDataSetLoader
{

    private static string SYSTEM_DELAY_TABLES = "SYSTEM_DELAY_TABLES";
    private static string SYSTEM_NEXT_PAGE = "SYSTEM_NEXT_PAGE ";


    public static DataSet Get_Driver_DriverDailyActivityData(List<int> dataBlockIds, DateTime from, DateTime to, int DriversCardId, int curUserId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        int dataBlockId = dataBlockIds[0];
        List<DDDClass.CardActivityDailyRecord> cardActivity = new List<DDDClass.CardActivityDailyRecord>();
        dataBlock.OpenConnection();
        cardActivity = dataBlock.cardUnitInfo.Get_EF_Driver_Activity_Data(dataBlockId, from, to).activityDailyRecords;
        dataBlock.CloseConnection();
        DataSet dataSet = new DataSet();
        dataSet.Tables.Add(PlfReportsDataTables.Get_PlfHeader_1(from, to, DriversCardId, curUserId, "", "", ""));
        dataSet.Tables.Add(ReportDataLoader.DailyDriverActivityProtocol_AllDataForReport(cardActivity));

        return dataSet;
    }

    public static DataSet Get_Driver_DriverWeeklyActivityData(List<int> dataBlockIds, DateTime from, DateTime to, int DriversCardId, int curUserId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        int dataBlockId = dataBlockIds[0];
        dataBlock.OpenConnection();
        List<DDDClass.CardDriverActivity> cardActivity = new List<DDDClass.CardDriverActivity>();
        cardActivity = dataBlock.cardUnitInfo.Get_EF_Driver_Activity_Data_byWeeks(dataBlockId, from, to);

        List<DDDClass.CardVehicleRecord> cardVehicleUsed = new List<DDDClass.CardVehicleRecord>();
        cardVehicleUsed = dataBlock.cardUnitInfo.Get_EF_Vehicles_Used(dataBlockIds, from, to);
        dataBlock.CloseConnection();
        DataSet dataSet = new DataSet();

        dataSet.Tables.Add(ReportDataLoader.WeeklyDriverActivityProtocol_DataForReportNEW(cardActivity, cardVehicleUsed));
        dataSet.Tables.Add(PlfReportsDataTables.Get_PlfHeader_1(from, to, DriversCardId, curUserId, "", "", ""));

        return dataSet;
    }

    public static DataSet Get_VehicleEventsAndFaults_VuOverSpeedingEventData(int VehicleId, List<int> dataBlockIds, DateTime from, DateTime to, int curUserId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        dataBlock.OpenConnection();

        List<DDDClass.VuOverSpeedingEventRecord> vuOverSpeedingEventRecords = new List<DDDClass.VuOverSpeedingEventRecord>();
        vuOverSpeedingEventRecords = dataBlock.vehicleUnitInfo.Get_VehicleEventsAndFaults_VuOverSpeedingEventData(dataBlockIds, from, to);

        DataSet dataset = new DataSet();
        dataset.Tables.Add(PlfReportsDataTables.Get_VehicleHeader_1(VehicleId, from, to, curUserId));
        dataset.Tables.Add(ReportDataLoader.VehicleOverSpeedingData(vuOverSpeedingEventRecords));

        dataBlock.CloseConnection();

        return dataset;
    }

    public static DataSet Get_Driver_VehicleUsed(List<int> dataBlockIDS, DateTime from, DateTime to, int DriversCardId, int curUserId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        dataBlock.OpenConnection();
        List<DDDClass.CardVehicleRecord> cardVehicleUsed = new List<DDDClass.CardVehicleRecord>();
        cardVehicleUsed = dataBlock.cardUnitInfo.Get_EF_Vehicles_Used(dataBlockIDS, from, to);
        dataBlock.CloseConnection();
        DataSet dataset = new DataSet();
        dataset.Tables.Add(ReportDataLoader.VehicleUsedData(cardVehicleUsed));
        dataset.Tables.Add(PlfReportsDataTables.Get_PlfHeader_1(from, to, DriversCardId, curUserId, "", "", ""));

        return dataset;
    }

    public static DataSet Get_MultiDrivers_ActivitySummary(List<int> dataBlockIds, List<int> DriversCardsIds, DateTime from, DateTime to, int curUserId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        DataSet dataset = new DataSet();
        DataTable summaryDataTable = new DataTable("DriversSummaryData");

        List<string> rows = new List<string>();
        DDDClass.DriverCardHolderIdentification cardHolder;
        DDDClass.CardDriverActivity driverActivity;
        DataRow dr;

        summaryDataTable.Columns.Add(new DataColumn("Код водителя", typeof(int)));
        summaryDataTable.Columns.Add(new DataColumn("Имя водителя", typeof(string)));
        summaryDataTable.Columns.Add(new DataColumn("Количество дней", typeof(int)));
        summaryDataTable.Columns.Add(new DataColumn("Всего в пути", typeof(TimeSpan)));
        summaryDataTable.Columns.Add(new DataColumn("Время вождения", typeof(TimeSpan)));
        summaryDataTable.Columns.Add(new DataColumn("Активное время", typeof(TimeSpan)));
        summaryDataTable.Columns.Add(new DataColumn("Пассивное время", typeof(TimeSpan)));
        summaryDataTable.Columns.Add(new DataColumn("Время отдыха", typeof(TimeSpan)));
        summaryDataTable.Columns.Add(new DataColumn("Пробег", typeof(int)));
        int i = 0;
        foreach (int id in dataBlockIds)
        {
            dr = summaryDataTable.NewRow();

            cardHolder = dataBlock.cardUnitInfo.Get_EF_Identification_DriverCardHolderIdentification(id);
            driverActivity = dataBlock.cardUnitInfo.Get_EF_Driver_Activity_Data(id, from, to);

            dr["Код водителя"] = DriversCardsIds[i++];
            dr["Имя водителя"] = cardHolder.cardHolderName.ToString();
            dr["Количество дней"] = driverActivity.activityDailyRecords.Count;
            dr["Всего в пути"] = driverActivity.GetTotalTimeSpan();
            dr["Время вождения"] = driverActivity.GetTotalDrivingTimeSpan();
            dr["Активное время"] = driverActivity.GetTotalWorkingTimeSpan();
            dr["Пассивное время"] = driverActivity.GetTotalAvailabilityTimeSpan();
            dr["Время отдыха"] = driverActivity.GetTotalBreakTimeSpan();
            dr["Пробег"] = driverActivity.GetTotalDistance();

            summaryDataTable.Rows.Add(dr);
        }
        dataset.Tables.Add(PlfReportsDataTables.Get_PlfHeader_1(from, to, curUserId));
        dataset.Tables.Add(summaryDataTable);

        return dataset;
    }

    public static DataSet Get_Driver_InfringementReport(int dataBlockIds, DateTime from, DateTime to)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        List<List<string>> rowsForTotals = new List<List<string>>();
        DataTable infringementDataTable = new DataTable();
        infringementDataTable.Columns.AddRange(ReportDataLoader.DriverInfringementReport_DataHeader());

        DataSet dataset = new DataSet();
        dataset.DataSetName = Guid.NewGuid().ToString();
        List<string> rows = new List<string>();
        DDDClass.DriverCardHolderIdentification cardHolder;
        DDDClass.CardDriverActivity driverActivity;
        dataset.Tables.Add(Get_SimpleDateHeader(from, to));
        dataset.Tables.Add(new DataTable(SYSTEM_DELAY_TABLES));
        dataset.Tables.Add(infringementDataTable);

        return dataset;
    }

    public static DataSet Get_Driver_EventsReport(int dataBlockId, DateTime from, DateTime to)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        DataTable events = new DataTable();
        events.Columns.AddRange(ReportDataLoader.DriverEventsFaults_DataHeader());

        DataSet dataset = new DataSet();
        dataset.DataSetName = Guid.NewGuid().ToString();
        List<List<string>> rows = new List<List<string>>();
        List<DDDClass.CardEventRecord> eventRecords = new List<DDDClass.CardEventRecord>();
        eventRecords = dataBlock.cardUnitInfo.Get_EF_Events_Data(dataBlockId, from, to);

        rows = ReportDataLoader.DriverEvents_Data(eventRecords);
        foreach (List<string> row in rows)
        {
            events.Rows.Add(row.ToArray());
        }

        dataset.Tables.Add(Get_SimpleDateHeader(from, to));
        dataset.Tables.Add(new DataTable(SYSTEM_DELAY_TABLES));
        dataset.Tables.Add(events);

        return dataset;
    }

    public static DataSet Get_Driver_FaultsReport(int dataBlockId, DateTime from, DateTime to)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        DataTable faults = new DataTable();
        faults.Columns.AddRange(ReportDataLoader.DriverEventsFaults_DataHeader());

        DataSet dataset = new DataSet();
        dataset.DataSetName = Guid.NewGuid().ToString();
        List<List<string>> rows = new List<List<string>>();
        List<DDDClass.CardFaultRecord> faultRecords = new List<DDDClass.CardFaultRecord>();
        faultRecords = dataBlock.cardUnitInfo.Get_EF_Faults_Data(dataBlockId, from, to);

        rows = ReportDataLoader.DriverFaults_Data(faultRecords);
        foreach (List<string> row in rows)
        {
            faults.Rows.Add(row.ToArray());
        }

        dataset.Tables.Add(Get_SimpleDateHeader(from, to));
        dataset.Tables.Add(new DataTable(SYSTEM_DELAY_TABLES));
        dataset.Tables.Add(faults);

        return dataset;
    }

    public static DataSet Get_Vehicle_ActivityReport(int VehicleId, List<int> dataBlockIds, DateTime from, DateTime to, int curUserId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        dataBlock.OpenConnection();

        DataTable activityReport = new DataTable();
        DataSet dataset = new DataSet("VehiclesDailyData");

        List<VehichleUnit.Vehicle_Activities> vehicleActivitiesList = new List<VehichleUnit.Vehicle_Activities>();
        vehicleActivitiesList = dataBlock.vehicleUnitInfo.Get_VehicleActivities_AllInOne(dataBlockIds, from, to);

        dataset.Tables.Add(PlfReportsDataTables.Get_VehicleHeader_1(VehicleId, from, to, curUserId));
        dataset.Tables.Add(ReportDataLoader.VehicleActivityReport_Data(vehicleActivitiesList));

        dataBlock.CloseConnection();

        return dataset;
    }

    public static DataSet Get_Vehicle_ALLDate(int VehicleId, List<int> dataBlockIds, DateTime from, DateTime to, int curUserId)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        dataBlock.OpenConnection();

        DataTable activityReport = new DataTable();
        DataSet dataset = new DataSet("VehiclesAllData");

        List<VehichleUnit.Vehicle_Activities> vehicleActivitiesList = new List<VehichleUnit.Vehicle_Activities>();
        vehicleActivitiesList = dataBlock.vehicleUnitInfo.Get_VehicleActivities_AllInOne(dataBlockIds, from, to);

        dataset.Tables.Add(PlfReportsDataTables.Get_VehicleHeader_1(VehicleId, from, to, curUserId));
        dataset.Tables.Add(ReportDataLoader.VehicleActivityReport_Data(vehicleActivitiesList));

        List<DDDClass.VuOverSpeedingEventRecord> vuOverSpeedingEventRecords = new List<DDDClass.VuOverSpeedingEventRecord>();
        vuOverSpeedingEventRecords = dataBlock.vehicleUnitInfo.Get_VehicleEventsAndFaults_VuOverSpeedingEventData(dataBlockIds, from, to);

        dataset.Tables.Add(ReportDataLoader.VehicleOverSpeedingData(vuOverSpeedingEventRecords));

        dataBlock.CloseConnection();

        if (dataset.Tables[1].Rows.Count == 0 && dataset.Tables[2].Rows.Count == 0)
        {
            throw new Exception("За указанный период нет данных");
        }

        return dataset;
    }
    public static DataSet Get_PLF_ALLData(List<int> dataBlockIds, DateTime from, DateTime to, int DriversCardId, int curUserId, ref List<PLFUnit.PLFRecord> records)
    {
        string connectionString = ConfigurationManager.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        PLFUnit.PLFUnitClass plf = new PLFUnit.PLFUnitClass();
        dataBlock.OpenConnection();
        plf.Records = dataBlock.plfUnitInfo.Get_Records(dataBlockIds, from, to, DriversCardId);

        //Тест сглаживания!
        if (plf.Records.Count > 0)
        {
            List<PLFUnit.PLFRecord> recordsToAproximate = new List<PLFUnit.PLFRecord>();
            recordsToAproximate = plf.Records;
            PLFUnit.PLFUnitClass.FFT_Fuel(ref recordsToAproximate);
            plf.Records = recordsToAproximate;
        }
        else
        {
            throw new Exception("За указанный период нет данных");
        }

        if (dataBlockIds.Count > 0)
            plf.installedSensors = dataBlock.plfUnitInfo.Get_InstalledSensors(dataBlockIds[0]);
        records = plf.Records;
        if (dataBlockIds.Count > 0)
        {
            plf.TIME_STEP = dataBlock.plfUnitInfo.Get_TIME_STEP(dataBlockIds[0]);
            plf.VEHICLE = dataBlock.plfUnitInfo.Get_VEHICLE(dataBlockIds[0]);
        }
        else
        {
            throw new Exception("За указанный период нет данных");
        }

        string vehRegNumber = dataBlock.plfUnitInfo.Get_VEHICLE(dataBlockIds[0]);
        string vehDeviceNumber = dataBlock.plfUnitInfo.Get_ID_DEVICE(dataBlockIds[0]);

        dataBlock.CloseConnection();

        DataSet dataSet = new DataSet();
        if (dataBlockIds.Count <= 0)
        {
            return dataSet;
        }
        //string vehRegNumber = plf.VEHICLE;
        //string vehDeviceNumber = plf.ID_DEVICE;

        dataSet.Tables.Add(PlfReportsDataTables.Get_PlfHeader_1(from, to, DriversCardId, curUserId, vehRegNumber, vehDeviceNumber, ""));
        dataSet.Tables.Add(PlfReportsDataTables.PlfReport_FullCalendar(records));
        if (plf.Records.Count > 0)
        {
            dataSet.Tables.Add(PlfReportsDataTables.PlfReport_FullCalendar_Totals(plf));
        }
        else {
            dataSet.Tables.Add();
        }
        if (plf.Records.Count > 0)
        {
            dataSet.Tables.Add(PlfReportsDataTables.PlfReport_FullCalendar_Refills(plf));
        }
        else
        {
            dataSet.Tables.Add();
        }

        return dataSet;
    }

    public static DataSet Get_PlfUnitsByDays(List<int> dataBlockIds, DateTime from, DateTime to, int DriversCardId, int curUserId, ref List<PLFUnit.PLFRecord> records, string VehiclePhotoPath)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        PLFUnit.PLFUnitClass plf = new PLFUnit.PLFUnitClass();
        dataBlock.OpenConnection();
        plf.Records = dataBlock.plfUnitInfo.Get_Records(dataBlockIds, from, to, DriversCardId);
        if (dataBlockIds.Count > 0)
            plf.installedSensors = dataBlock.plfUnitInfo.Get_InstalledSensors(dataBlockIds[0]);
        records = plf.Records;
        if (dataBlockIds.Count > 0)
        {
            plf.TIME_STEP = dataBlock.plfUnitInfo.Get_TIME_STEP(dataBlockIds[0]);
            plf.VEHICLE = dataBlock.plfUnitInfo.Get_VEHICLE(dataBlockIds[0]);
        }
        else
            throw new Exception("Нельзя расчитать значения для отчета, не задан Шаг Времени");
        dataBlock.CloseConnection();
        DataSet dataSet = new DataSet();
        string vehRegNumber = plf.VEHICLE;
        string vehDeviceNumber = plf.ID_DEVICE;

        dataSet.Tables.Add(PlfReportsDataTables.Get_PlfHeader_1(from, to, DriversCardId, curUserId, vehRegNumber, vehDeviceNumber, VehiclePhotoPath));
        dataSet.Tables.Add(PlfReportsDataTables.PlfReport_ByDays(plf.SortPlfEveryDay()));

        return dataSet;
    }

    public static DataSet Get_PlfUnitsEficienty(List<int> dataBlockIds, DateTime from, DateTime to, int DriversCardId, int curUserId, ref List<PLFUnit.PLFRecord> records)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        DataTable activityReport = new DataTable();
        DataSet dataset = new DataSet(Guid.NewGuid().ToString());
        dataset.DataSetName = Guid.NewGuid().ToString();

        PLFUnit.PLFUnitClass plf = new PLFUnit.PLFUnitClass();
        dataBlock.OpenConnection();
        plf.Records = dataBlock.plfUnitInfo.Get_Records(dataBlockIds, from, to, DriversCardId);
        records = plf.Records;
        if (dataBlockIds[0] != null)
        {
            plf.TIME_STEP = dataBlock.plfUnitInfo.Get_TIME_STEP(dataBlockIds[0]);
            plf.VEHICLE = dataBlock.plfUnitInfo.Get_VEHICLE(dataBlockIds[0]);
        }
        else
            throw new Exception("Нельзя расчитать значения для отчета, не задан Шаг Времени");

        string driversName = dataBlock.cardsTable.GetCardName(DriversCardId);
        string orgName = dataBlock.usersTable.Get_UserOrgName(curUserId);
        dataBlock.CloseConnection();
        string vehRegNumber = plf.VEHICLE;
        string vehDeviceNumber = plf.ID_DEVICE;

        dataset.Tables.Add(PlfReportsDataTables.Get_PlfHeader_1(from, to, DriversCardId, curUserId, vehDeviceNumber, vehRegNumber, ""));
        dataset.Tables.Add(PlfReportsDataTables.PlfReport_Efficiency_ByDays(plf, driversName));

        return dataset;
    }

    public static DataSet Get_PlfReportByPeriod(List<int> dataBlockIds, DateTime from, DateTime to, int DriversCardId, int curUserId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);
        DataTable activityReport = new DataTable();
        DataSet dataset = new DataSet();
        dataset.DataSetName = Guid.NewGuid().ToString();

        PLFUnit.PLFUnitClass plf = new PLFUnit.PLFUnitClass();
        plf.Records = dataBlock.plfUnitInfo.Get_Records(dataBlockIds, from, to, DriversCardId);
        if (dataBlockIds[0] != null)
        {
            plf.TIME_STEP = dataBlock.plfUnitInfo.Get_TIME_STEP(dataBlockIds[0]);
            plf.VEHICLE = dataBlock.plfUnitInfo.Get_VEHICLE(dataBlockIds[0]);
        }
        else
            throw new Exception("Нельзя расчитать значения для отчета, не задан Шаг Времени");

        string driversName = dataBlock.cardsTable.GetCardName(DriversCardId);
        dataBlock.usersTable.OpenConnection();
        string orgName = dataBlock.usersTable.Get_UserOrgName(curUserId);
        dataBlock.usersTable.CloseConnection();
        foreach (PLFUnit.PLFUnitClass plfUnit in plf.SortPlfEveryDay())
        {
            if (plfUnit.Records.Count == 0)
                continue;
            activityReport = new DataTable();
            // dataset.Tables.Add(PlfReportsDataTables.PlfReport_Efficiency_ByDays_Header(orgName, from, to, plfUnit.Records[0].SYSTEM_TIME.GetSystemTime()));
            dataset.Tables.Add(new DataTable(SYSTEM_DELAY_TABLES + Guid.NewGuid().ToString()));
            //  dataset.Tables.Add(PlfReportsDataTables.PlfReport_Efficiency_ByDays(plfUnit, driversName));
            dataset.Tables.Add(SYSTEM_NEXT_PAGE + Guid.NewGuid().ToString());
        }

        return dataset;
    }

    private static DataTable Get_DriverHeader_1(int dataBlockId, DateTime from, DateTime to)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, ConfigurationManager.AppSettings["language"]);

        DDDClass.CardIdentification cardIdentification = new DDDClass.CardIdentification();
        cardIdentification = dataBlock.cardUnitInfo.Get_EF_Identification_CardIdentification(dataBlockId);
        DDDClass.DriverCardHolderIdentification driverCard = new DDDClass.DriverCardHolderIdentification();
        driverCard = dataBlock.cardUnitInfo.Get_EF_Identification_DriverCardHolderIdentification(dataBlockId);

        DataTable header = ReportDataLoader.Driver_Header_1(cardIdentification, driverCard, from, to);

        return header;
    }

    private static DataTable Get_SimpleDateHeader(DateTime from, DateTime to)
    {
        DataTable activityTable = new DataTable("SimpleDateHeader_" + Guid.NewGuid().ToString());
        string fromStr = from.ToShortDateString();
        string toStr = to.ToShortDateString();
        string Col_1 = "Период c " + fromStr + " по " + toStr;
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        return activityTable;
    }

    private static DataTable Get_SimpleVehiclesDateHeader(DateTime from, DateTime to, DateTime current)
    {
        DataTable activityTable = new DataTable("SimpleDateHeader_" + Guid.NewGuid().ToString());
        DataRow dr;
        string fromStr = from.ToShortDateString();
        string toStr = to.ToShortDateString();
        string Col_1 = "Период c " + fromStr + " по " + toStr;
        string Col_2 = "Дата " + current.ToLongDateString();
        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        dr = activityTable.NewRow();
        dr[Col_1] = "Регистрационный номер: ";
        dr[Col_2] = "Идентификационный номер: ";

        activityTable.Rows.Add(dr);

        return activityTable;
    }


}
