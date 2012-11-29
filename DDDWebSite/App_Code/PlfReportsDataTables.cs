using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using BLL;
using System.Configuration;
/// <summary>
/// Summary description for PlfReportsDataTables
/// </summary>
public class PlfReportsDataTables
{
	public PlfReportsDataTables()
	{
	}

    public static DataTable PlfReport_ByDays(List<PLFUnit.PLFUnitClass> plf)
    {
        DataTable dt = new DataTable("PlfReport_ByDays_Data");
        DataRow dr;
        dt.Columns.Add(new DataColumn("Дата", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("Начало", typeof(TimeSpan)));
        dt.Columns.Add(new DataColumn("Окончание", typeof(TimeSpan)));
        dt.Columns.Add(new DataColumn("Движение", typeof(TimeSpan)));
        dt.Columns.Add(new DataColumn("Стоянка", typeof(TimeSpan)));
        dt.Columns.Add(new DataColumn("Путь", typeof(double)));
        dt.Columns.Add(new DataColumn("Одометр", typeof(int)));
        dt.Columns.Add(new DataColumn("Топливо", typeof(double)));
        DateTime dateStart;
        DateTime dateEnd;
        foreach (PLFUnit.PLFUnitClass plfUC in plf)
        {
            dateStart = new DateTime();
            dateEnd = new DateTime();
            dr = dt.NewRow();
            dateStart = plfUC.START_PERIOD.GetSystemTime();
            dateEnd = plfUC.END_PERIOD.GetSystemTime();
            dr["Дата"] = dateStart;
            dr["Начало"] = dateStart.TimeOfDay;
            dr["Окончание"] = dateEnd.TimeOfDay;
            dr["Движение"] = plfUC.Get_MovingTime();
            dr["Стоянка"] = plfUC.Get_RestTime();
            dr["Путь"] = plfUC.GetPath();
            dr["Одометр"] = plfUC.GetOdometer();
            if (plfUC.installedSensors.FUEL_VOLUME1 == plfUC.installedSensors.YesString)
                dr["Топливо"] = plfUC.GetFuel();
            else
                dr["Топливо"] = 0;
            dt.Rows.Add(dr);
        }

        return dt;
    }

    public static DataTable PlfReport_FullCalendar(List<PLFUnit.PLFRecord> plf)
    {
        DataTable dt = new DataTable("PLFReport_FullCalendar");
        DataRow dr;
        dt.Columns.Add(new DataColumn("ДатаВремя", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("Путь", typeof(double)));
        dt.Columns.Add(new DataColumn("Скорость", typeof(double)));
        dt.Columns.Add(new DataColumn("Уровень топлива", typeof(double)));
        dt.Columns.Add(new DataColumn("Расход топлива", typeof(double)));
        dt.Columns.Add(new DataColumn("RPM", typeof(int)));
        dt.Columns.Add(new DataColumn("Номинальные обороты", typeof(int)));
        dt.Columns.Add(new DataColumn("Напряжение", typeof(double)));
        //dt.Columns.Add(new DataColumn("Суммарное время работы", typeof(TimeSpan)));
        /*  dt.Columns.Add(new DataColumn("Время работы двигателя", typeof(TimeSpan)));
          dt.Columns.Add(new DataColumn("Время движения", typeof(TimeSpan)));
          dt.Columns.Add(new DataColumn("Максимальное непрерывное время простоя", typeof(double)));
          dt.Columns.Add(new DataColumn("Объем топлива в баках на начало периода", typeof(double)));
          dt.Columns.Add(new DataColumn("Объем топлива в баках на конец периода", typeof(double)));
          dt.Columns.Add(new DataColumn("Количество заправок", typeof(int)));
          dt.Columns.Add(new DataColumn("Всего заправлено топлива", typeof(int)));
          dt.Columns.Add(new DataColumn("Количество возможных сливов", typeof(int)));
          dt.Columns.Add(new DataColumn("Всего возможно слито топлива", typeof(int)));
          dt.Columns.Add(new DataColumn("Израсходовано топлива (ДУТ | ДРТ)", typeof(int)));
          dt.Columns.Add(new DataColumn("Средний общий путевой расход топлива (ДУТ | ДРТ) л/100 км", typeof(int)));
          dt.Columns.Add(new DataColumn("Средний часовой расход топлива за время простоя л/ч", typeof(int)));
          dt.Columns.Add(new DataColumn("Средний часовой расход топлива за время движения л/ч", typeof(int)));
          dt.Columns.Add(new DataColumn("Средний часовой расход топлива за время работы ДРТ л/ч", typeof(int)));
          dt.Columns.Add(new DataColumn("Максимальный часовой расход топлива за время простоя л/ч", typeof(int)));
					
						
      Максимальный часовой расход топлива за время простоя л/ч					
      Средний общий часовой расход топлива (ДУТ | ДРТ) л/ч					
      Перерасход топлива (без учета возможных сливов) (ДУТ | ДРТ) л
	
         */
        foreach (PLFUnit.PLFRecord record in plf)
        {
            dr = dt.NewRow();
            dr["ДатаВремя"] = record.SYSTEM_TIME.GetSystemTime();
            dr["Путь"] = record.getDoubleParam(record.DISTANCE_COUNTER);
            dr["Скорость"] = record.getDoubleParam(record.SPEED);
            dr["Уровень топлива"] = record.getDoubleParam(record.FUEL_VOLUME1);
            dr["Расход топлива"] = record.getDoubleParam(record.FUEL_CONSUMPTION);
            dr["RPM"] = Convert.ToInt32(record.ENGINE_RPM);
            dr["Номинальные обороты"] = 0;//Их нету. эта постоянная задана в доп.инфо о ТС.
            dr["Напряжение"] = record.getDoubleParam(record.VOLTAGE);
           // dr["Суммарное время работы"] = record.getDoubleParam(record.VOLTAGE);
            dt.Rows.Add(dr);
        }
        return dt;
    }

    public static DataTable PlfReport_FullCalendar_Totals(PLFUnit.PLFUnitClass plf)
    {
        DataTable dt = new DataTable("PLFReport_FullCalendar_Totals");
        DataRow dr;
        dt.Columns.Add(new DataColumn("Начало работы", typeof(string)));
        dt.Columns.Add(new DataColumn("Окончание работы", typeof(string)));
        dt.Columns.Add(new DataColumn("Продолжительность периода", typeof(string)));
        dt.Columns.Add(new DataColumn("Суммарное время работы", typeof(string)));
        dt.Columns.Add(new DataColumn("Время работы двигателя", typeof(string)));
        dt.Columns.Add(new DataColumn("Время движения", typeof(string)));
        dt.Columns.Add(new DataColumn("Максимальное непрерывное время движения", typeof(string)));
        dt.Columns.Add(new DataColumn("Время простоя с заведенным двигателем", typeof(string)));
        dt.Columns.Add(new DataColumn("Максимальное непрерывное время простоя", typeof(string)));
        dt.Columns.Add(new DataColumn("Максимальный непрерывный пройденный путь", typeof(double)));
        dt.Columns.Add(new DataColumn("Объем топлива в баках на начало периода", typeof(double)));
        dt.Columns.Add(new DataColumn("Объем топлива в баках на конец периода", typeof(double)));
        dt.Columns.Add(new DataColumn("Количество заправок", typeof(int)));
        dt.Columns.Add(new DataColumn("Всего заправлено топлива", typeof(double)));
        dt.Columns.Add(new DataColumn("Количество возможных сливов", typeof(double)));
        dt.Columns.Add(new DataColumn("Всего возможно слито топлива", typeof(double)));
        dt.Columns.Add(new DataColumn("Пройденный путь", typeof(double)));
        dt.Columns.Add(new DataColumn("Средняя скорость за время движения", typeof(double)));
        dt.Columns.Add(new DataColumn("Максимальная скорость за время движения", typeof(double)));
        dt.Columns.Add(new DataColumn("Средние обороты двигателя за время движения", typeof(double)));
        dt.Columns.Add(new DataColumn("Максимальные обороты двигателя за время движения", typeof(double)));
        dt.Columns.Add(new DataColumn("Минимальные обороты двигателя за время движения", typeof(double)));
        dt.Columns.Add(new DataColumn("Среднее напряжение бортсети", typeof(double)));
        dt.Columns.Add(new DataColumn("Максимальное напряжение бортсети", typeof(double)));
        dt.Columns.Add(new DataColumn("Минимальное напряжение бортсети", typeof(double)));
        dt.Columns.Add(new DataColumn("Максимальный объем топлива в баках", typeof(double)));
        dt.Columns.Add(new DataColumn("Минимальный  объем топлива в баках", typeof(double)));

        /*  
         * dt.Columns.Add(new DataColumn("Минимальные обороты двигателя за время движения", typeof(int)));
         * dt.Columns.Add(new DataColumn("Средние обороты двигателя за время движения", typeof(int)));
         * 
         dt.Columns.Add(new DataColumn("Израсходовано топлива (ДУТ | ДРТ)", typeof(int)));
         dt.Columns.Add(new DataColumn("Средний общий путевой расход топлива (ДУТ | ДРТ) л/100 км", typeof(int)));
         dt.Columns.Add(new DataColumn("Средний часовой расход топлива за время простоя л/ч", typeof(int)));
         dt.Columns.Add(new DataColumn("Средний часовой расход топлива за время движения л/ч", typeof(int)));
         dt.Columns.Add(new DataColumn("Средний часовой расход топлива за время работы ДРТ л/ч", typeof(int)));
         dt.Columns.Add(new DataColumn("Максимальный часовой расход топлива за время простоя л/ч", typeof(int)));
         * 
         * 
         Максимальный часовой расход топлива за время простоя л/ч
         Средний общий часовой расход топлива (ДУТ | ДРТ) л/ч
         Перерасход топлива (без учета возможных сливов) (ДУТ | ДРТ) л
         * 
        */
        dr = dt.NewRow();

        dr["Начало работы"] = plf.Records[0].SYSTEM_TIME.GetSystemTime().ToString("dd.MM.yyyy HH:mm:ss");
        dr["Окончание работы"] = plf.Records[plf.Records.Count - 1].SYSTEM_TIME.GetSystemTime().ToString("dd.MM.yyyy HH:mm:ss");

        TimeSpan diff = plf.Records[plf.Records.Count - 1].SYSTEM_TIME.GetSystemTime().Subtract(plf.Records[0].SYSTEM_TIME.GetSystemTime());
        dr["Продолжительность периода"] = String.Format("{0:0}", (int)diff.TotalHours) + ":" + String.Format("{0:00}", diff.Minutes) + ":" + String.Format("{0:00}", diff.Seconds);

        TimeSpan tTotalWorkTime = plf.Get_AllWorkingTime();
        string totalWorkTime = String.Format("{0:00}", (int)tTotalWorkTime.TotalHours) + ":" + String.Format("{0:00}", tTotalWorkTime.Minutes) + ":" + String.Format("{0:00}", tTotalWorkTime.Seconds);
        dr["Суммарное время работы"] = totalWorkTime;

        TimeSpan tMotorWorkTime = plf.Get_WorkingEngineTime();
        string motorWorkTime = String.Format("{0:00}", (int)tMotorWorkTime.TotalHours) + ":" + String.Format("{0:00}", tMotorWorkTime.Minutes) + ":" + String.Format("{0:00}", tMotorWorkTime.Seconds);
        dr["Время работы двигателя"] = motorWorkTime;

        TimeSpan tMovement = plf.Get_MovingTime();
        string movement = String.Format("{0:00}", (int)tMovement.TotalHours) + ":" + String.Format("{0:00}", tMovement.Minutes) + ":" + String.Format("{0:00}", tMovement.Seconds);
        dr["Время движения"] = movement;

        TimeSpan tMaxMovement = plf.MaxContinuousMovingTime();
        string maxMovement = String.Format("{0:00}", (int)tMaxMovement.TotalHours) + ":" + String.Format("{0:00}", tMaxMovement.Minutes) + ":" + String.Format("{0:00}", tMaxMovement.Seconds);
        dr["Максимальное непрерывное время движения"] = maxMovement;

        TimeSpan tDowntime = plf.Get_WorkingEngineWithNoMovingTime();
        string downtime = String.Format("{0:00}", (int)tDowntime.TotalHours) + ":" + String.Format("{0:00}", tDowntime.Minutes) + ":" + String.Format("{0:00}", tDowntime.Seconds);
        dr["Время простоя с заведенным двигателем"] = downtime;

        TimeSpan tMaxDowntime = plf.MaxContinuousWorkingWithNoMovingTime();
        string maxDowntime = String.Format("{0:00}", (int)tMaxDowntime.TotalHours) + ":" + String.Format("{0:00}", tMaxDowntime.Minutes) + ":" + String.Format("{0:00}", tMaxDowntime.Seconds);
        dr["Максимальное непрерывное время простоя"] = maxDowntime;

        dr["Максимальный непрерывный пройденный путь"] = plf.MaxContinuousPath();
       
        if (plf.Records.Count > 0)
        {
            dr["Объем топлива в баках на начало периода"] = plf.Records[0].getDoubleParam(plf.Records[0].FUEL_VOLUME1);
            dr["Объем топлива в баках на конец периода"] = plf.Records[0].getDoubleParam(plf.Records[plf.Records.Count - 1].FUEL_VOLUME1);
        }
        List<PLFUnit.PLFRefillClass> refillsArray = plf.Get_AllRefills();
        double totalFuelRefilled = 0;
        foreach (PLFUnit.PLFRefillClass refill in refillsArray)
            totalFuelRefilled += refill.capacityEnd - refill.capacityStart;
        dr["Количество заправок"] = refillsArray.Count;
        dr["Всего заправлено топлива"] = totalFuelRefilled;

        refillsArray = plf.Get_AllDropOuts();
        totalFuelRefilled = 0;
        foreach (PLFUnit.PLFRefillClass refill in refillsArray)
            totalFuelRefilled += refill.capacityStart - refill.capacityEnd;
        dr["Количество возможных сливов"] = refillsArray.Count;
        dr["Всего возможно слито топлива"] = totalFuelRefilled;
        dr["Всего возможно слито топлива"] = totalFuelRefilled;

        double distance = 0;

        double averageSpeed = 0;
        double maxSpeed = 0;

        double maxFuelValue = 0;
        double minFuelValue = double.Parse(plf.Records[0].FUEL_VOLUME1);

        double maxRPM = 0;
        double minRPM = double.PositiveInfinity;
        double averageRPM = 0;
        double sumRPM = 0;
        int notZeroRPMCounter = 0;

        double maxVoltage = 0;
        double minVoltage = double.Parse(plf.Records[0].VOLTAGE, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
        double averageVoltage = 0;

        foreach(PLFUnit.PLFRecord record in plf.Records)
        {
            distance += double.Parse(record.DISTANCE_COUNTER, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            double speed = double.Parse(record.SPEED, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            if (speed > maxSpeed)
            {
                maxSpeed = speed;
            }

            double fuelValue = double.Parse(record.FUEL_VOLUME1);
            if (fuelValue > maxFuelValue)
            {
                maxFuelValue = fuelValue;
            }
            if (fuelValue < minFuelValue)
            {
                minFuelValue = fuelValue;
            }

            double RPM = double.Parse(record.ENGINE_RPM);
            if (RPM > maxRPM)
            {
                maxRPM = RPM;
            }
            if (RPM > 0)
            {
                notZeroRPMCounter++;
                sumRPM += RPM;
                if (RPM < minRPM)
                {
                    minRPM = RPM;
                }
            }

            double voltage = double.Parse(record.VOLTAGE, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo);
            if (voltage > maxVoltage)
            {
                maxVoltage = voltage;
            }
            if (voltage < minVoltage)
            {
                minVoltage = voltage;
            }

            averageVoltage += voltage / plf.Records.Count;
        }

        if (tMovement.TotalSeconds != 0)
        {
            averageSpeed = distance / tMovement.TotalSeconds * 3600;
        }
        else
        {
            averageSpeed = 0;
        }

        if (notZeroRPMCounter != 0)
        {
            averageRPM = sumRPM / notZeroRPMCounter;
        }
        else
        {
            averageRPM = 0;
        }

        dr["Пройденный путь"] = distance;
        dr["Средняя скорость за время движения"] = averageSpeed;
        dr["Максимальная скорость за время движения"] = maxSpeed;
        dr["Средние обороты двигателя за время движения"] = averageRPM;
        dr["Максимальные обороты двигателя за время движения"] = maxRPM;
        dr["Минимальные обороты двигателя за время движения"] = minRPM;
        dr["Среднее напряжение бортсети"] = averageVoltage;
        dr["Максимальное напряжение бортсети"] = maxVoltage;
        dr["Минимальное напряжение бортсети"] = minVoltage;
        dr["Максимальный объем топлива в баках"] = maxFuelValue;
        dr["Минимальный  объем топлива в баках"] = minFuelValue;

        dt.Rows.Add(dr);
        return dt;
    }

    public static DataTable PlfReport_FullCalendar_Refills(PLFUnit.PLFUnitClass plf)
    {
        DataTable dt = new DataTable("PLFReport_FullCalendar_Refills");
        DataRow dr;
        dt.Columns.Add(new DataColumn("Заправка/Слив", typeof(string)));
        dt.Columns.Add(new DataColumn("НачВремя", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("КонВремя", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("НачОбъем", typeof(double)));
        dt.Columns.Add(new DataColumn("КонОбъем", typeof(double)));
      
        List<PLFUnit.PLFRefillClass> refillsArray = plf.Get_AllDropOuts();
        refillsArray.AddRange(plf.Get_AllRefills());

        foreach (PLFUnit.PLFRefillClass refill in refillsArray)
        {
            dr = dt.NewRow();
            if(refill.capacityEnd - refill.capacityStart>0)
                dr["Заправка/Слив"] = "Заправка";
            else
                dr["Заправка/Слив"] = "Возможный слив";
            dr["НачВремя"] = refill.timeStart;
            dr["КонВремя"] = refill.timeEnd;
            dr["НачОбъем"] = refill.capacityStart;
            dr["КонОбъем"] = refill.capacityEnd;

            dt.Rows.Add(dr);
        }
        return dt;
    }

    public static DataTable PlfReport_Efficiency_ByDays(PLFUnit.PLFUnitClass plf, string DriversName, DateTime currDate)
    {
        DataTable dt = new DataTable("Efficiency_ByDays_DataTable");
        DataRow dr;
        dt.Columns.Add(new DataColumn("ТС", typeof(string)));
        dt.Columns.Add(new DataColumn("Одометр начало", typeof(string)));
        dt.Columns.Add(new DataColumn("Одометр конец", typeof(string)));
        dt.Columns.Add(new DataColumn("Время движения", typeof(string)));
        dt.Columns.Add(new DataColumn("Процент движения", typeof(string)));
        dt.Columns.Add(new DataColumn("Время стоянки", typeof(string)));
        dt.Columns.Add(new DataColumn("Процент стоянки", typeof(string)));
        dt.Columns.Add(new DataColumn("Время на холостых", typeof(string)));
        dt.Columns.Add(new DataColumn("Процент на холостых", typeof(string)));
        dt.Columns.Add(new DataColumn("Пройденный путь", typeof(string)));
        dt.Columns.Add(new DataColumn("Текущая дата", typeof(string)));

        TimeSpan timespan = new TimeSpan();

        dr = dt.NewRow();
        dr["ТС"] = DriversName + " (" + plf.VEHICLE + ")";
        dr["Одометр начало"] = "0";
        dr["Одометр конец"] = "0";
        timespan = plf.Get_MovingTime();
        dr["Время движения"] = timespan.Hours + ":" + timespan.Minutes;
        dr["Процент движения"] = String.Format("{0:0.##}", plf.Get_MovingPercents())+"%";
        timespan = plf.Get_RestTime();
        dr["Время стоянки"] = timespan.Hours + ":" + timespan.Minutes;
        dr["Процент стоянки"] = String.Format("{0:0.##}", plf.Get_RestPercents()) + "%";
        timespan = plf.Get_IdleRpmTime();
        dr["Время на холостых"] = timespan.Hours + ":" + timespan.Minutes;
        dr["Процент на холостых"] = String.Format("{0:0.##}", plf.Get_IdleRpmPercent()) + "%";
        dr["Пройденный путь"] = String.Format("{0:0.##}", plf.GetPath());
        dr["Текущая дата"] = currDate.ToLongDateString();
        dt.Rows.Add(dr);

        return dt;
    }

    public static DataTable PlfReport_Efficiency_ByDays(PLFUnit.PLFUnitClass plf, string DriversName)
    {
        DataTable dt = new DataTable("Efficiency_ByDays_DataTable");
        DataRow dr;
        dt.Columns.Add(new DataColumn("ТС", typeof(string)));
        dt.Columns.Add(new DataColumn("Одометр начало", typeof(string)));
        dt.Columns.Add(new DataColumn("Одометр конец", typeof(string)));
        dt.Columns.Add(new DataColumn("Время движения", typeof(string)));
        dt.Columns.Add(new DataColumn("Процент движения", typeof(string)));
        dt.Columns.Add(new DataColumn("Время стоянки", typeof(string)));
        dt.Columns.Add(new DataColumn("Процент стоянки", typeof(string)));
        dt.Columns.Add(new DataColumn("Время на холостых", typeof(string)));
        dt.Columns.Add(new DataColumn("Процент на холостых", typeof(string)));
        dt.Columns.Add(new DataColumn("Пройденный путь", typeof(string)));
        dt.Columns.Add(new DataColumn("Текущая дата", typeof(string)));


        List<PLFUnit.PLFUnitClass> plfList = new List<PLFUnit.PLFUnitClass>();
        plfList = plf.SortPlfEveryDay();
        TimeSpan timespan = new TimeSpan();
        foreach (PLFUnit.PLFUnitClass plfUnit in plfList)
        {
            dr = dt.NewRow();
            dr["ТС"] = DriversName + " (" + plfUnit.VEHICLE + ")";
            dr["Одометр начало"] = "0";
            dr["Одометр конец"] = "0";
            timespan = plfUnit.Get_MovingTime();
            dr["Время движения"] = timespan.Hours + ":" + timespan.Minutes;
            dr["Процент движения"] = String.Format("{0:0.##}", plfUnit.Get_MovingPercents()) + "%";
            timespan = plfUnit.Get_RestTime();
            dr["Время стоянки"] = timespan.Hours + ":" + timespan.Minutes;
            dr["Процент стоянки"] = String.Format("{0:0.##}", plfUnit.Get_RestPercents()) + "%";
            timespan = plfUnit.Get_IdleRpmTime();
            dr["Время на холостых"] = timespan.Hours + ":" + timespan.Minutes;
            dr["Процент на холостых"] = String.Format("{0:0.##}", plfUnit.Get_IdleRpmPercent()) + "%";
            dr["Пройденный путь"] = String.Format("{0:0.##}", plfUnit.GetPath());
            dr["Текущая дата"] = plfUnit.START_PERIOD.GetSystemTime().ToLongDateString();
            dt.Rows.Add(dr);
        }
        return dt;
    }

    public static DataTable PLF_Data(List<PLFUnit.PLFRecord> plfUnitClass)
    {
        DataTable dt = new DataTable("PLF_AllData");
        DataRow dr;

        if (plfUnitClass.Count == 0)
            throw new Exception("Нет данных");
        #region "Columns init"
        if (plfUnitClass[0].SYSTEM_TIME != null)
            dt.Columns.Add(new DataColumn("Время", typeof(string)));

        if (plfUnitClass[0].ADDITIONAL_SENSORS != null)
            dt.Columns.Add(new DataColumn("Доп. Сенсор", typeof(string)));

        if (plfUnitClass[0].ALTITUDE != null)
            dt.Columns.Add(new DataColumn("Высота", typeof(string)));

        if (plfUnitClass[0].LATITUDE != null)
            dt.Columns.Add(new DataColumn("Широта", typeof(string)));

        if (plfUnitClass[0].LONGITUDE != null)
            dt.Columns.Add(new DataColumn("Долгота", typeof(string)));

        if (plfUnitClass[0].DISTANCE_COUNTER != null)
            dt.Columns.Add(new DataColumn("Дистанция", typeof(string)));

        if (plfUnitClass[0].ENGINE_RPM != null)
            dt.Columns.Add(new DataColumn("RPM", typeof(string)));

        if (plfUnitClass[0].FUEL_CONSUMPTION != null)
            dt.Columns.Add(new DataColumn("Расход топлива", typeof(string)));

        if (plfUnitClass[0].FUEL_COUNTER != null)
            dt.Columns.Add(new DataColumn("Счетчик топлива", typeof(string)));

        if (plfUnitClass[0].FUEL_VOLUME1 != null)
            dt.Columns.Add(new DataColumn("Уровень топлива 1", typeof(string)));

        if (plfUnitClass[0].FUEL_VOLUME2 != null)
            dt.Columns.Add(new DataColumn("Уровень топлива 2", typeof(string)));

        if (plfUnitClass[0].MAIN_STATES != null)
            dt.Columns.Add(new DataColumn("MAIN_STATES", typeof(string)));

        if (plfUnitClass[0].SPEED != null)
            dt.Columns.Add(new DataColumn("Скорость", typeof(string)));

        if (plfUnitClass[0].TEMPERATURE1 != null)
            dt.Columns.Add(new DataColumn("Темпер.1", typeof(string)));

        if (plfUnitClass[0].TEMPERATURE2 != null)
            dt.Columns.Add(new DataColumn("Темпер.2", typeof(string)));

        if (plfUnitClass[0].VOLTAGE != null)
            dt.Columns.Add(new DataColumn("Напряжение", typeof(string)));

        if (plfUnitClass[0].WEIGHT1 != null)
            dt.Columns.Add(new DataColumn("Вес 1", typeof(string)));

        if (plfUnitClass[0].WEIGHT2 != null)
            dt.Columns.Add(new DataColumn("Вес 2", typeof(string)));

        if (plfUnitClass[0].WEIGHT3 != null)
            dt.Columns.Add(new DataColumn("Вес 3", typeof(string)));

        if (plfUnitClass[0].WEIGHT4 != null)
            dt.Columns.Add(new DataColumn("Вес 4", typeof(string)));

        if (plfUnitClass[0].WEIGHT5 != null)
            dt.Columns.Add(new DataColumn("Вес 5", typeof(string)));

        if (plfUnitClass[0].RESERVED_3 != null)
            dt.Columns.Add(new DataColumn("RESERVED_3", typeof(string)));

        if (plfUnitClass[0].RESERVED_4 != null)
            dt.Columns.Add(new DataColumn("RESERVED_4", typeof(string)));

        if (plfUnitClass[0].RESERVED_5 != null)
            dt.Columns.Add(new DataColumn("RESERVED_5", typeof(string)));
        #endregion
        DateTime dateTime = new DateTime();

        foreach (PLFUnit.PLFRecord item in plfUnitClass)
        {
            dr = dt.NewRow();

            #region "Columns init"
            if (item.SYSTEM_TIME != null)
            {
                dateTime = new DateTime();
                dateTime = item.SYSTEM_TIME.GetSystemTime();
                dr["Время"] = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
            }

            if (item.ADDITIONAL_SENSORS != null)
                dr["Доп. Сенсор"] = item.ADDITIONAL_SENSORS;

            if (item.ALTITUDE != null)
                dr["Высота"] = item.ALTITUDE;

            if (item.LATITUDE != null)
                dr["Широта"] = item.LATITUDE;

            if (item.LONGITUDE != null)
                dr["Долгота"] = item.LONGITUDE;

            if (item.DISTANCE_COUNTER != null)
                dr["Дистанция"] = item.DISTANCE_COUNTER;

            if (item.ENGINE_RPM != null)
                dr["RPM"] = item.ENGINE_RPM;

            if (item.FUEL_CONSUMPTION != null)
                dr["Расход топлива"] = item.FUEL_CONSUMPTION;

            if (item.FUEL_COUNTER != null)
                dr["Счетчик топлива"] = item.FUEL_COUNTER;

            if (item.FUEL_VOLUME1 != null)
                dr["Уровень топлива 1"] = item.FUEL_VOLUME1;

            if (item.FUEL_VOLUME2 != null)
                dr["Уровень топлива 2"] = item.FUEL_VOLUME2;

            if (item.MAIN_STATES != null)
                dr["MAIN_STATES"] = item.MAIN_STATES;

            if (item.SPEED != null)
                dr["Скорость"] = item.SPEED;

            if (item.TEMPERATURE1 != null)
                dr["Темпер.1"] = item.TEMPERATURE1;

            if (item.TEMPERATURE2 != null)
                dr["Темпер.2"] = item.TEMPERATURE2;

            if (item.VOLTAGE != null)
                dr["Напряжение"] = item.VOLTAGE;

            if (item.WEIGHT1 != null)
                dr["Вес 1"] = item.WEIGHT1;

            if (item.WEIGHT2 != null)
                dr["Вес 2"] = item.WEIGHT2;

            if (item.WEIGHT3 != null)
                dr["Вес 3"] = item.WEIGHT3;

            if (item.WEIGHT4 != null)
                dr["Вес 4"] = item.WEIGHT4;

            if (item.WEIGHT5 != null)
                dr["Вес 5"] = item.WEIGHT5;

            if (item.RESERVED_3 != null)
                dr["RESERVED_3"] = item.RESERVED_3;

            if (item.RESERVED_4 != null)
                dr["RESERVED_4"] = item.RESERVED_4;

            if (item.RESERVED_5 != null)
                dr["RESERVED_5"] = item.RESERVED_5;
            #endregion

            dt.Rows.Add(dr);
        }
        return dt;
    }

    public static DataTable Get_PlfHeader_1(DateTime from, DateTime to, int DriversCardId, int curUserId, string VehRegNumber, string VehDeviceName, string PhotoPath)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");

        DataTable activityTable = new DataTable("PlfHeader_1");
        DataRow dr;
        string Col_1 = "С";
        string Col_2 = "По";
        string Col_3 = "Регистрационный номер";
        string Col_4 = "Имя пользователя";
        string Col_5 = "Номер водителя";
        string Col_6 = "Имя водителя";
        string Col_7 = "Название организации";
        string Col_8 = "Путь к фото";
        string Col_9 = "Номер бортового устройства";

        dataBlock.OpenConnection();
        string driversName = dataBlock.cardsTable.GetCardName(DriversCardId);
        string driversNumber = dataBlock.cardsTable.GetCardNumber(DriversCardId);

        string userName = dataBlock.usersTable.Get_UserName(curUserId);
        string orgName = dataBlock.organizationTable.GetOrganizationName(dataBlock.usersTable.Get_UserOrgId(curUserId));
        dataBlock.CloseConnection();

        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_5, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_6, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_7, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_8, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_9, typeof(string)));

        dr = activityTable.NewRow();
        dr[Col_1] = from.ToString("dd.MM.yyyy");
        dr[Col_2] = to.ToString("dd.MM.yyyy");
        dr[Col_3] = VehRegNumber;
        dr[Col_4] = userName;
        dr[Col_5] = driversNumber;
        dr[Col_6] = driversName;
        dr[Col_7] = orgName;
        dr[Col_8] = PhotoPath;
        dr[Col_9] = VehDeviceName;
        activityTable.Rows.Add(dr);

        return activityTable;
    }

    public static DataTable Get_PlfHeader_1(DateTime from, DateTime to, int curUserId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");

        DataTable activityTable = new DataTable("PlfHeader_1");
        DataRow dr;
        string Col_1 = "С";
        string Col_2 = "По";
        string Col_4 = "Имя пользователя";
        string Col_7 = "Название организации";

        dataBlock.OpenConnection();
        string userName = dataBlock.usersTable.Get_UserName(curUserId);
        string orgName = dataBlock.organizationTable.GetOrganizationName(dataBlock.usersTable.Get_UserOrgId(curUserId));
        dataBlock.CloseConnection();

        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_7, typeof(string)));

        dr = activityTable.NewRow();
        dr[Col_1] = from.ToString("dd.MM.yyyy");
        dr[Col_2] = to.ToString("dd.MM.yyyy");
        dr[Col_4] = userName;
        dr[Col_7] = orgName;
        activityTable.Rows.Add(dr);

        return activityTable;
    }

    public static DataTable Get_VehicleHeader_1(int VehicleId, DateTime from, DateTime to, int curUserId)
    {
        string connectionString = ConfigurationSettings.AppSettings["fleetnetbaseConnectionString"];
        DataBlock dataBlock = new DataBlock(connectionString, "STRING_RU");

        DataTable activityTable = new DataTable("VehicleHeader_1");
        DataRow dr;
        string Col_1 = "С";
        string Col_2 = "По";
        string Col_3 = "Регистрационный номер";
        string Col_4 = "Код ТС";
        string Col_5 = "Имя пользователя";
        string Col_6 = "Название организации";
        dataBlock.OpenConnection();
        string VehRegNumber = dataBlock.vehiclesTables.GetVehicleGOSNUM(VehicleId);
        string IdentNumber = dataBlock.vehiclesTables.GetVehicleVin(VehicleId);
        string userName = dataBlock.usersTable.Get_UserName(curUserId);
        string orgName = dataBlock.usersTable.Get_UserOrgName(curUserId);
        dataBlock.CloseConnection();


        activityTable.Columns.Add(new DataColumn(Col_1, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_2, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_3, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_4, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_5, typeof(string)));
        activityTable.Columns.Add(new DataColumn(Col_6, typeof(string)));

        dr = activityTable.NewRow();
        dr[Col_1] = from.ToString("dd.MM.yyyy");
        dr[Col_2] = to.ToString("dd.MM.yyyy");
        dr[Col_3] = VehRegNumber;
        dr[Col_4] = IdentNumber;
        dr[Col_5] = userName;
        dr[Col_6] = orgName;
        activityTable.Rows.Add(dr);

        return activityTable;
    }
}
