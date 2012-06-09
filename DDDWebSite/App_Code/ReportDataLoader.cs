using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using System.Web;
using BLL;
using System.Configuration;

/// <summary>
/// Summary description for ReportDataLoader
/// </summary>
public class ReportDataLoader
{
    public ReportDataLoader()
    {
    }

    public static string onlyforInternal { get; set; }

    public static DataTable Driver_Header_1(DDDClass.CardIdentification driver, DDDClass.DriverCardHolderIdentification driverCard, DateTime from, DateTime to)
    {
        DataTable activityTable = new DataTable(Guid.NewGuid().ToString());
        DataRow dr;
        string Col_1 = "Период с " + from.ToLongDateString();
        string Col_2 = "Имя водителя";
        string Col_3 = "Номер водителя";

        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(string)));

        dr = activityTable.NewRow();
        dr[Col_1] = "\tпо " + to.ToLongDateString();
        dr[Col_2] = driverCard.cardHolderName.ToString();
        dr[Col_3] = driver.cardNumber.driverIdentificationNumber();
        activityTable.Rows.Add(dr);

        return activityTable;
    }

    public static DataTable VehicleOverSpeedingData(List<DDDClass.VuOverSpeedingEventRecord> data)
    {
        DataTable dt = new DataTable("VehicleOverSpeedingData");
        DataRow dr;

        dt.Columns.Add(new DataColumn("Дата начала", typeof(string)));
        dt.Columns.Add(new DataColumn("Дата окончания", typeof(string)));
        dt.Columns.Add(new DataColumn("Имя водителя", typeof(string)));
        dt.Columns.Add(new DataColumn("Максимальная скорость", typeof(int)));
        dt.Columns.Add(new DataColumn("Средняя скорость", typeof(int)));
        dt.Columns.Add(new DataColumn("Причина регистрации", typeof(string)));

        /*if (data.Count == 0)
            throw new Exception("нет ни одного нарушения!");*/

        foreach (DDDClass.VuOverSpeedingEventRecord item in data)
        {
            dr = dt.NewRow();
            dr["Дата начала"] = item.eventBeginTime.getTimeRealDate().ToString("dd.MM.yyyy HH:mm:ss");
            dr["Дата окончания"] = item.eventEndTime.getTimeRealDate().ToString("dd.MM.yyyy HH:mm:ss");
            //dr["Имя водителя"] = "UNKNOWN";
            dr["Максимальная скорость"] = item.maxSpeedValue.speed;
            dr["Средняя скорость"] = item.averageSpeedValue.speed;
            dr["Причина регистрации"] = item.eventRecordPurpose.ToString();
            dt.Rows.Add(dr);
        }
        return dt;
    }

    public static DataTable Vehicle_DetailedSpeed_Data(DDDClass.VuDetailedSpeedData data)
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("Дата", typeof(string)));
        dt.Columns.Add(new DataColumn("Скорость", typeof(string)));

        DateTime dateTimeBegin;
        foreach (DDDClass.VuDetailedSpeedBlock item in data.vuDetailedSpeedBlocks)
        {
            dateTimeBegin = new DateTime();
            dateTimeBegin = item.speedBlockBeginDate.getTimeRealDate();
            foreach (DDDClass.Speed speed in item.speedsPerSecond)
            {
                dr = dt.NewRow();
                dr["Дата"] = dateTimeBegin.ToShortDateString() + " " + dateTimeBegin.ToShortTimeString();
                dr["Скорость"] = speed.ToString();
                dt.Rows.Add(dr);
                dateTimeBegin = dateTimeBegin.AddSeconds(1);
            }
        }
        if (data.vuDetailedSpeedBlocks.Count == 0)
            throw new Exception("нет записей о скорости!");
        return dt;
    }

    public static DataTable VehicleUsedData(List<DDDClass.CardVehicleRecord> data)
    {
        DataTable dt = new DataTable("VehicleUsedData");
        DataRow dr;

        dt.Columns.Add(new DataColumn("Дата", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("Регистрационный номер ТС", typeof(string)));
        dt.Columns.Add(new DataColumn("Страна регистрации ТС", typeof(string)));
        dt.Columns.Add(new DataColumn("Начало использования", typeof(TimeSpan)));
        dt.Columns.Add(new DataColumn("Конец использования", typeof(TimeSpan)));
        dt.Columns.Add(new DataColumn("Одометр старт", typeof(string)));
        dt.Columns.Add(new DataColumn("Одометр стоп", typeof(string)));
        dt.Columns.Add(new DataColumn("Пробег", typeof(string)));
        DateTime dateTime = new DateTime();

        foreach (DDDClass.CardVehicleRecord item in data)
        {
            dateTime = item.vehicleFirstUse.getTimeRealDate();
            dr = dt.NewRow();
            dr["Дата"] = dateTime;
            dr["Регистрационный номер ТС"] = item.vehicleRegistration.vehicleRegistrationNumber.ToString();
            dr["Страна регистрации ТС"] = item.vehicleRegistration.vehicleRegistrationNation.ToString();
            //dateTime = item.vehicleFirstUse.getTimeRealDate();
            dr["Начало использования"] = dateTime.TimeOfDay;
            dr["Конец использования"] = item.vehicleLastUse.getTimeRealDate().TimeOfDay;
            dr["Одометр старт"] = item.vehicleOdometerBegin.odometerShort.ToString();
            dr["Одометр стоп"] = item.vehicleOdometerEnd.odometerShort.ToString();
            dr["Пробег"] = (item.vehicleOdometerEnd.odometerShort - item.vehicleOdometerBegin.odometerShort).ToString();

            dt.Rows.Add(dr);
        }
        return dt;
    }

    public static DataTable DriversGeneralInfo(string driversName)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        DataTable dt = new DataTable();
        DataRow dr;
        string driversIdentificationNumber;
        int driversDataBlockId;
        string cardIssuingMemberState;

        dt.Columns.Add(new DataColumn("Имя водителя: ", typeof(string)));
        dt.Columns.Add(new DataColumn("Номер водителя: ", typeof(string)));
        dt.Columns.Add(new DataColumn("Страна карты: ", typeof(string)));
        dt.Columns.Add(new DataColumn("Дополнительная информация", typeof(string)));
        DateTime dateTime = new DateTime();

        driversIdentificationNumber = dataBlock.GetDriversIdentificationNumber(driversName);
        driversDataBlockId = dataBlock.GetDataBlockIdByDriversName(driversName);
        cardIssuingMemberState = dataBlock.GetDriversCardIssuingMemberState(driversDataBlockId);

        dr = dt.NewRow();
        dr["Имя водителя: "] = driversName;
        dr["Номер водителя: "] = driversIdentificationNumber;
        dr["Страна карты: "] = cardIssuingMemberState;
        dt.Rows.Add(dr);

        onlyforInternal = driversDataBlockId.ToString();

        return dt;
    }

    public static DataTable VehicleGeneralInfo(string vehicleNumber)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_EN");
        DataTable dt = new DataTable();
        DataRow dr;

        List<int> vehicleDataBlockId = new List<int>();

        vehicleDataBlockId = dataBlock.GetDataBlockIdByVehicleNumber(vehicleNumber);
        int dataBlockId = vehicleDataBlockId[0];
        DDDClass.VehicleRegistrationIdentification regIdent = dataBlock.vehicleUnitInfo.Get_VehicleOverview_RegistrationIdentification(dataBlockId);
        DDDClass.VuDownloadablePeriod cardPeriod = dataBlock.vehicleUnitInfo.Get_VehicleOverview_VuDownloadablePeriod(dataBlockId);
        int recordsCount = dataBlock.GetDataBlock_RecorsCount(dataBlockId);

        dt.Columns.Add(new DataColumn("Номер ТС: ", typeof(string)));
        dt.Columns.Add(new DataColumn("Рег номер: ", typeof(string)));
        dt.Columns.Add(new DataColumn("Страна Рег-ции: ", typeof(string)));
        dt.Columns.Add(new DataColumn("Период: ", typeof(string)));
        dt.Columns.Add(new DataColumn("Колл-во записей: ", typeof(string)));
        DateTime dateTime = new DateTime();

        dr = dt.NewRow();
        dr["Номер ТС: "] = vehicleNumber;
        dr["Рег номер: "] = regIdent.vehicleRegistrationNumber.ToString();
        dr["Страна Рег-ции: "] = regIdent.vehicleRegistrationNation.ToString();
        dr["Период: "] = cardPeriod.minDownloadableTime.ToString() + Environment.NewLine + cardPeriod.maxDownloadableTime.ToString();
        dr["Колл-во записей: "] = recordsCount;
        dt.Rows.Add(dr);

        onlyforInternal = dataBlockId.ToString();

        return dt;
    }

    public static DataTable DailyDriverActivityProtocol_AllDataForReport(List<DDDClass.CardActivityDailyRecord> activityRecords)
    {
        DataTable activityTable = new DataTable("DriversDailyData");
        DataRow dr;

        activityTable.Columns.Add(new DataColumn("Время начала", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Время окончания", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Код активности", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Время вождения", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Время работы", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Время свободное", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Время отдыха", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Слот", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Статус вождения", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Статус карты", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Дата", typeof(DateTime)));

        DateTime todaysDate = new DateTime();
        int itemIndex;

        foreach (DDDClass.CardActivityDailyRecord day in activityRecords)
        {
            todaysDate = day.activityRecordDate.getTimeRealDate();

            if (day.activityChangeInfo.Count == 0)
            {
                dr = activityTable.NewRow();
                dr["Время начала"] = new TimeSpan();
                dr["Время окончания"] = new TimeSpan();
                dr["Код активности"] = "";
                dr["Время вождения"] = new TimeSpan();
                dr["Время свободное"] = new TimeSpan();
                dr["Время отдыха"] = new TimeSpan();
                dr["Время работы"] = new TimeSpan();
                dr["Слот"] = "";
                dr["Статус вождения"] = "";
                dr["Статус карты"] = "";
                dr["Дата"] = todaysDate;
                activityTable.Rows.Add(dr);
            }
            else
            {
                foreach (DDDClass.ActivityChangeInfo record in day.activityChangeInfo)
                {
                    itemIndex = day.activityChangeInfo.IndexOf(record);
                    dr = activityTable.NewRow();
                    dr["Время начала"] = day.getActivityStartTime(itemIndex);
                    dr["Время окончания"] = day.getActivityEndTime(itemIndex);
                    dr["Код активности"] = record.ToString();

                    dr["Время вождения"] = new TimeSpan();
                    dr["Время свободное"] =  new TimeSpan();
                    dr["Время отдыха"] =  new TimeSpan();
                    dr["Время работы"] =  new TimeSpan();
                    dr["Дата"] = todaysDate;

                    if (record.ToString() == "driving")
                    {
                        dr["Время вождения"] = day.getActivityDuration(itemIndex);
                    }
                    else
                        if (record.ToString() == "work")
                            dr["Время работы"] = day.getActivityDuration(itemIndex);
                        else
                            if (record.ToString() == "availability")
                                dr["Время свободное"] = day.getActivityDuration(itemIndex);
                            else
                                if (record.ToString() == "break")
                                    dr["Время отдыха"] = day.getActivityDuration(itemIndex);
                    dr["Слот"] = record.getSlotStatus();
                    dr["Статус вождения"] = record.getDrivingStatus();
                    dr["Статус карты"] = record.getCardStatus();
                    activityTable.Rows.Add(dr);
                }
            }
        }
        return activityTable;
    }

    public static DataTable WeeklyDriverActivityProtocol_DataForReportNEW(List<DDDClass.CardDriverActivity> weeklyActivityRecords,   List<DDDClass.CardVehicleRecord> vehicleUsed)
    {
        DataTable activityTable = new DataTable("DriversWeeklyData");
        DataRow dr;

        //List<DataColumn> Columns = new List<DataColumn>(); //

        activityTable.Columns.Add(new DataColumn("Дата", typeof(DateTime)));
        activityTable.Columns.Add(new DataColumn("Время начала", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Время окончания", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Всего", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Время вождения", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Время работы", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Время свободное", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("Время отдыха", typeof(TimeSpan)));
        activityTable.Columns.Add(new DataColumn("ТС", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Пробег", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Средняя скорость", typeof(int)));
        activityTable.Columns.Add(new DataColumn("Номер недели", typeof(int)));
        
        DateTime date = new DateTime();
        List<DDDClass.CardVehicleRecord> vehUsed = new List<DDDClass.CardVehicleRecord>();
        string VehNumbers;
        TotalsClasses.CardVehicleList cardVehList = new TotalsClasses.CardVehicleList(vehicleUsed);
        int weekNumber = 0;
        foreach (DDDClass.CardDriverActivity records in weeklyActivityRecords)
        {
            weekNumber++;
            if (records.activityDailyRecords.Count != 0)
            {
                foreach (DDDClass.CardActivityDailyRecord record in records.activityDailyRecords)
                {
                    dr = activityTable.NewRow();
                    date = record.activityRecordDate.getTimeRealDate();
                    dr["Дата"] = date;//дата
                    dr["Время начала"] = date.TimeOfDay;//"Время начала"
                    dr["Время окончания"] = date.TimeOfDay.Add(record.Get_TotalTimeSpan());//"Время окончания"
                    dr["Всего"] = record.Get_TotalTimeSpan();//"Всего"
                    dr["Время вождения"] = record.Get_TotalDrivingTimeSpan();//"Время вождения"
                    dr["Время работы"] = record.Get_TotalWorkingTimeSpan();//"Время работы"
                    dr["Время свободное"] = record.Get_TotalAvailabilityTimeSpan();//"Время свободное"
                    dr["Время отдыха"] = record.Get_TotalBreakTimeSpan();//"Время отдыха"                    
                    vehUsed = cardVehList.FindVehicleNumberByDate(record.activityRecordDate.getTimeRealDate().Date);
                    VehNumbers="";
                    foreach(DDDClass.CardVehicleRecord foundVehicle in vehUsed)
                    {
                        VehNumbers += foundVehicle.vehicleRegistration.vehicleRegistrationNumber+ " ";
                    }
                    dr["ТС"] = VehNumbers;//"ТС"
                    dr["Пробег"] = record.activityDayDistance.ToString();//"Дистанция"
                    dr["Средняя скорость"] = 0;//"Средняя скорость"
                    dr["Номер недели"] = weekNumber;//"Средняя скорость"
                    
                    activityTable.Rows.Add(dr);
                }
            }
        }
        return activityTable;
    }

    public static DataColumn[] DriverInfringementReport_DataHeader()
    {
        List<DataColumn> Columns = new List<DataColumn>();

        Columns.Add(new DataColumn("Водитель", typeof(string)));
        Columns.Add(new DataColumn("Код нарушения", typeof(string)));
        Columns.Add(new DataColumn("Дата обнаружения", typeof(string)));
        Columns.Add(new DataColumn("Время обнаружения", typeof(string)));
        Columns.Add(new DataColumn("Описание", typeof(string)));

        return Columns.ToArray();
    }

    public static DataColumn[] DriverEventsFaults_DataHeader()
    {
        List<DataColumn> Columns = new List<DataColumn>();

        Columns.Add(new DataColumn("Дата начала", typeof(string)));
        Columns.Add(new DataColumn("Дата окончания", typeof(string)));
        Columns.Add(new DataColumn("Страна регистрации", typeof(string)));
        Columns.Add(new DataColumn("Регистрационный номер ТС", typeof(string)));
        Columns.Add(new DataColumn("Тип события", typeof(string)));

        return Columns.ToArray();
    }

    public static List<List<string>> DriverEvents_Data(List<DDDClass.CardEventRecord> eventRecords)
    {
        List<List<string>> returnArray = new List<List<string>>();
        List<string> row;
        DateTime dateTime = new DateTime();
        foreach (DDDClass.CardEventRecord record in eventRecords)
        {
            dateTime = new DateTime();
            dateTime = record.eventBeginTime.getTimeRealDate();
            row = new List<string>();
            row.Add(record.eventBeginTime.ToString());
            row.Add(record.eventEndTime.ToString());
            row.Add(record.eventVehicleRegistration.vehicleRegistrationNation.ToString());
            row.Add(record.eventVehicleRegistration.vehicleRegistrationNumber.ToString());
            row.Add(record.eventType.ToString());
            returnArray.Add(row);
        }
        return returnArray;
    }

    public static List<List<string>> DriverFaults_Data(List<DDDClass.CardFaultRecord> faultRecords)
    {
        List<List<string>> returnArray = new List<List<string>>();
        List<string> row;
        DateTime dateTime = new DateTime();
        foreach (DDDClass.CardFaultRecord record in faultRecords)
        {
            row = new List<string>();
            row.Add(record.faultBeginTime.ToString());
            row.Add(record.faultEndTime.ToString());
            row.Add(record.faultVehicleRegistration.vehicleRegistrationNation.ToString());
            row.Add(record.faultVehicleRegistration.vehicleRegistrationNumber.ToString());
            row.Add(record.faultType.ToString());
            returnArray.Add(row);
        }
        return returnArray;
    }

    public static DataTable VehicleActivityReport_Data(List<VehichleUnit.Vehicle_Activities> activityRecord)
    {
        DataTable activityTable = new DataTable("VehiclesDailyData");
        DataRow dr;
        activityTable.Columns.Add(new DataColumn("Дата", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Время начала", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Время окончания", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Время вождения", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Время без вождения", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Дистанция", typeof(int)));
        activityTable.Columns.Add(new DataColumn("Средняя скорость", typeof(Double)));
        activityTable.Columns.Add(new DataColumn("Расход топлива", typeof(Double)));
        activityTable.Columns.Add(new DataColumn("Средний расход", typeof(Double)));
        activityTable.Columns.Add(new DataColumn("Код активности", typeof(string)));
        activityTable.Columns.Add(new DataColumn("Одометр Старт", typeof(int)));


        DateTime todaysDate = new DateTime();
        int itemIndex;
        foreach (VehichleUnit.Vehicle_Activities day in activityRecord)
        {
            todaysDate = day.downloadedDayDate.getTimeRealDate();

            if (day.vuActivityDailyData.activityChangeInfo.Count == 0)
            {
                dr = activityTable.NewRow();
                dr["Дата"] = todaysDate.ToString("dd.MM.yyyy HH:mm:ss");
                dr["Время начала"] = new TimeSpan().ToString();
                dr["Время окончания"] = new TimeSpan().ToString();
                dr["Время вождения"] = new TimeSpan().ToString();
                dr["Время без вождения"] = new TimeSpan().ToString();
                dr["Дистанция"] = 0;
                dr["Средняя скорость"] = 0;
                dr["Расход топлива"] = 0;
                dr["Средний расход"] = 0;
                activityTable.Rows.Add(dr);
            }
            else
            {
                foreach (DDDClass.ActivityChangeInfo record in day.vuActivityDailyData.activityChangeInfo)
                {
                    itemIndex = day.vuActivityDailyData.activityChangeInfo.IndexOf(record);
                    dr = activityTable.NewRow();
                    dr["Время начала"] = day.vuActivityDailyData.getActivityStartTime(itemIndex).ToString();
                    dr["Время окончания"] = day.vuActivityDailyData.getActivityEndTime(itemIndex).ToString();

                    dr["Время вождения"] = new TimeSpan().ToString();
                    dr["Время без вождения"] = new TimeSpan().ToString();
                    dr["Дата"] = todaysDate.ToString("dd.MM.yyyy HH:mm:ss");

                    if (record.ToString() == "driving")
                    {
                        dr["Время вождения"] = day.vuActivityDailyData.getActivityEndTime(itemIndex).Subtract(day.vuActivityDailyData.getActivityStartTime(itemIndex)).ToString();
                        //dr["Время вождения"] = day.vuActivityDailyData.getActivityDuration(itemIndex);
                        dr["Код активности"] = "D";
                    }
                    else
                    {
                        //dr["Время без вождения"] = day.vuActivityDailyData.getActivityDuration(itemIndex);
                        dr["Время без вождения"] = day.vuActivityDailyData.getActivityEndTime(itemIndex).Subtract(day.vuActivityDailyData.getActivityStartTime(itemIndex)).ToString();
                        if (record.ToString() == "work")
                            dr["Код активности"] = "W";
                        if (record.ToString() == "availability")
                            dr["Код активности"] = "A";
                        if (record.ToString() == "break")
                            dr["Код активности"] = "R";
                    }
                    
                    dr["Дистанция"] = 0;
                    dr["Средняя скорость"] = 0;
                    dr["Расход топлива"] = 0;
                    dr["Средний расход"] = 0;
                    dr["Одометр Старт"] = day.odoMeterValueMidnight.odometerShort;

                    activityTable.Rows.Add(dr);
                }
            }
        }
        return activityTable;
    }

    private static string addTwoTimes(string first, string second, string separator)//separator == ":"
    {
        DateTime datetime = new DateTime();
        int hours = 0;
        string returnString;

        try
        {
            string[] split1 = first.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            string[] split2 = second.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            datetime = datetime.AddMinutes(Convert.ToDouble(split1[1]));
            datetime = datetime.AddMinutes(Convert.ToDouble(split2[1]));
            hours += Convert.ToInt32(split1[0]) + Convert.ToInt32(split2[0]) + datetime.Hour;
            returnString = String.Format("{0:00}", hours) + separator + datetime.ToString("mm");
        }
        catch
        {
            returnString = "Ошибка";
        }
        return returnString;
    }

    private static string divideTimeOnInt(string time, int days, string separator)
    {
        float divResult;
        string returnString;
        try
        {
            string[] splittedString = time.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            int hours = Convert.ToInt32(splittedString[0]);
            int minutes = Convert.ToInt32(splittedString[1])/days;
            divResult = (float)hours / (float)days;
            string[] divSplit = divResult.ToString().Split(',');
            if (divSplit.Length == 2 && divSplit[1] != "")
            {
                string afterz = "0," + divSplit[1];
                minutes += Convert.ToInt32((Convert.ToSingle(afterz) * 60));
            }
            hours = Convert.ToInt32(divSplit[0]);
            DateTime date = new DateTime();
            date = date.AddMinutes(minutes);
            hours += date.Hour;
            returnString = String.Format("{0:00}", hours) + separator + date.ToString("mm");
        }
        catch(Exception ex)
        {
            returnString = "Ошибка";
        }
        return returnString;
    }

    private static string divideTimeOnTime(string time1, string time2, string separator)
    {
        string returnString = "";
        try
        {
            string[] splitedStr = time1.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            int minutes1 = Convert.ToInt32(splitedStr[0]) * 60 + Convert.ToInt32(splitedStr[1]);

            splitedStr = time2.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            int minutes2 = Convert.ToInt32(splitedStr[0]) * 60 + Convert.ToInt32(splitedStr[1]);
            float result = (float)((float)minutes1/(float)minutes2);
            returnString = String.Format("{0:0.##}", result);
        }
        catch (Exception ex)
        {
            returnString = "Ошибка";
        }
        return returnString;
    }

    public static DataTable UsersList(List<UserFromTable> usersList)
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("#", typeof(string)));
        dt.Columns.Add(new DataColumn("Имя", typeof(string)));
        dt.Columns.Add(new DataColumn("Пароль", typeof(string)));
        dt.Columns.Add(new DataColumn("Тип пользователя", typeof(string)));
        dt.Columns.Add(new DataColumn("Роль пользователя", typeof(string)));
        dt.Columns.Add(new DataColumn("Время последней аутентификации", typeof(string)));
        dt.Columns.Add(new DataColumn("Название организации", typeof(string)));
        int i = 1;
        foreach (UserFromTable users in usersList)
        {
            dr = dt.NewRow();
            dr["#"] = i++;
            dr["Имя"] = users.name;
            dr["Пароль"] = users.pass;
            dr["Тип пользователя"] = users.userType;
            dr["Роль пользователя"] = users.userRole;
            if (users.timeConnection == (new DateTime()))
                dr["Время последней аутентификации"] = "Вход не производился";
            else
                dr["Время последней аутентификации"] = users.timeConnection.ToLongDateString() + " " + users.timeConnection.ToShortTimeString();
            dr["Название организации"] = users.orgName;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    public static DataTable OrganizationsList(List<OrganizationFromTable> orgList)
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("#", typeof(string)));
        dt.Columns.Add(new DataColumn("Название организации", typeof(string)));
        dt.Columns.Add(new DataColumn("Тип организации", typeof(string)));
        dt.Columns.Add(new DataColumn("Страна", typeof(string)));
        dt.Columns.Add(new DataColumn("Регион", typeof(string)));
        int i = 1;
        foreach (OrganizationFromTable org in orgList)
        {
            dr = dt.NewRow();
            dr["#"] = i++;
            dr["Название организации"] = org.orgName;
            dr["Тип организации"] = org.orgType;
            dr["Страна"] = org.countryName;
            dr["Регион"] = org.regionName;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    public static DataTable OrgInfoDataSorce(List<KeyValuePair<string, int>> orgInfoList)
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("ORG_INFO_ID", typeof(string)));
        dt.Columns.Add(new DataColumn("ORG_INFO_NAME", typeof(string)));        
        foreach (KeyValuePair<string, int> orgInfo in orgInfoList)
        {
            dr = dt.NewRow();
            dr["ORG_INFO_ID"] = orgInfo.Value.ToString();
            dr["ORG_INFO_NAME"] = orgInfo.Key;
            dt.Rows.Add(dr);
        }
        return dt;
    }
}
