using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Globalization;


namespace PLFUnit
{
    /// <summary>
    /// Класс полностью описывает PLF файл.
    /// </summary>
    [Serializable]
    public class PLFUnitClass
    {
        /// <summary>
        /// тип карты. Устанавливается во время разбора.
        /// </summary>
        public int cardType { get; set; } // тип карты - 0 driver, 1 - vehicle , 2 - PLF  
        /// <summary>
        /// Массив записей типа PLFRecord
        /// </summary>
        public List<PLFRecord> Records { get; set; }
        /// <summary>
        /// ID бортового устройства
        /// </summary>
        public string ID_DEVICE { get; set; }
        /// <summary>
        /// Имя/номер транспортного средства
        /// </summary>
        public string VEHICLE { get; set; }
        /// <summary>
        /// Шаг времени.
        /// </summary>
        public string TIME_STEP { get; set; }
        /// <summary>
        /// дата первой записи в файле
        /// </summary>
        public PLFSystemTime START_PERIOD { get; set; }
        /// <summary>
        /// дата последней записи в файле
        /// </summary>
        public PLFSystemTime END_PERIOD { get; set; }
        /// <summary>
        /// Список установленных сенсоров
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]public PLFRecord installedSensors { get; set; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public PLFUnitClass()
        {
            Records = new List<PLFRecord>();
            installedSensors = new PLFRecord();
            ID_DEVICE = "";
            VEHICLE = "";
            TIME_STEP = "";
            START_PERIOD = new PLFSystemTime();
            END_PERIOD = new PLFSystemTime();
        }
        /// <summary>
        /// Сериализует класс в XML
        /// </summary>
        /// <param name="output">путь, куда ложить файл</param>
        /// <returns>имя созданного файла</returns>
        public string SerializePLFUnitClass(string output)
        {
            DateTime dateTime;            
            string fileName;
            string YYYYMMDD_HHmm;
            string format;

            dateTime = Records[Records.Count - 1].SYSTEM_TIME.GetSystemTime();
            format = "dd.MM.yyyy";

            YYYYMMDD_HHmm = dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);

            fileName = "PLF_" + YYYYMMDD_HHmm
                + "_"
                + VEHICLE
                + "_"
                + ID_DEVICE
                + ".XML";

            XmlSerializer xmlFormat = new XmlSerializer(typeof(PLFUnitClass));
            using (Stream fStream = new FileStream(output+fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xmlFormat.Serialize(fStream, this);
            }
            return fileName;
        }
        /// <summary>
        /// Десериализует из XML в экземпляр этого класса
        /// </summary>
        /// <param name="filename">путь к файлу.</param>
        /// <returns>обьект класса PLFUnitClass</returns>
        public static PLFUnitClass DeserializePlfUnitClass(string filename)
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer serializer = new XmlSerializer(typeof(PLFUnitClass));

            // A FileStream is needed to read the XML document.
            FileStream fs = new FileStream(filename, FileMode.Open);
            XmlReader reader = new XmlTextReader(fs);

            // Declare an object variable of the type to be deserialized.
            PLFUnitClass i = new PLFUnitClass();

            // Use the Deserialize method to restore the object's state.
            i = (PLFUnitClass)serializer.Deserialize(reader);

            reader.Close();
            fs.Close();
            return i;
        }
        /// <summary>
        /// Сортирует по дням
        /// </summary>
        /// <returns>возвращает массив обьектов PLFUnitClass, каждый содержит информацию за сутки</returns>
        public List<PLFUnitClass> SortPlfEveryDay()
        {
            List<PLFUnitClass> plfEveryDay = new List<PLFUnitClass>();
            PLFUnitClass oneDay = new PLFUnitClass();
            bool newDay = true;
            foreach (PLFRecord record in this.Records)
            {
                if (!newDay)
                {
                    if (oneDay.Records[oneDay.Records.Count - 1].SYSTEM_TIME.GetSystemTime().DayOfWeek != record.SYSTEM_TIME.GetSystemTime().DayOfWeek)
                    {
                        oneDay.END_PERIOD = oneDay.Records[oneDay.Records.Count - 1].SYSTEM_TIME;
                        oneDay.TIME_STEP = this.TIME_STEP;
                        oneDay.ID_DEVICE = this.ID_DEVICE;
                        oneDay.VEHICLE = this.VEHICLE;
                        oneDay.installedSensors = this.installedSensors;

                        plfEveryDay.Add(oneDay);
                        oneDay = new PLFUnitClass();                        
                        newDay = true;
                    }
                }
                if (newDay)
                {
                    oneDay.Records.Add(record);
                    oneDay.START_PERIOD = record.SYSTEM_TIME;
                    newDay = false;
                }
                else
                {
                    oneDay.Records.Add(record);
                }
                if (this.Records[this.Records.Count - 1] == record)
                {
                    oneDay.END_PERIOD = oneDay.Records[oneDay.Records.Count - 1].SYSTEM_TIME;
                    oneDay.TIME_STEP = this.TIME_STEP;
                    oneDay.ID_DEVICE = this.ID_DEVICE;
                    oneDay.VEHICLE = this.VEHICLE;
                    oneDay.installedSensors = this.installedSensors;

                    plfEveryDay.Add(oneDay);
                }
            }
            return plfEveryDay;
        }
        /// <summary>
        /// Возвращает  все время Time_Step*records.count
        /// </summary>
        /// <returns>продолжительность работы</returns>
        public TimeSpan Get_AllWorkingTime()
        {
            int i = Records.Count;
            TimeSpan DrivingTime = new TimeSpan(0, 0, Convert.ToInt32(this.TIME_STEP) * Records.Count);//time_step*i
            return DrivingTime;
        }
        /// <summary>
        /// Время движения Speed!=0
        /// </summary>
        /// <returns>продолжительность движения</returns>
        public TimeSpan Get_MovingTime()
        {
            int indexes = 0;
            foreach(PLFRecord record in Records)
            {
                if (record.SPEED != "0")
                    indexes++;
            }
            TimeSpan DrivingTime = new TimeSpan(0, 0, Convert.ToInt32(this.TIME_STEP) * indexes);//time_step*i
            return DrivingTime;
        }
        /// <summary>
        /// Получает процент движения
        /// </summary>
        /// <returns>Double процент движения</returns>
        public double Get_MovingPercents()//работает только с одним днем
        {
            TimeSpan date =  Get_AllWorkingTime();
            int allThis = date.Hours * 60 + date.Minutes;
            double onePercent = (double)allThis / 100;
            date = Get_MovingTime();
            allThis = date.Hours * 60 + date.Minutes;
            double percents = allThis / onePercent;
            return percents;
        }
        /// <summary>
        /// Получает процент отдыха
        /// </summary>
        /// <returns>Double процент отдыха</returns>
        public double Get_RestPercents()
        {
            TimeSpan date = Get_AllWorkingTime();
            int allThis = date.Hours * 60 + date.Minutes;
            double onePercent = (double)allThis / 100;
            date = Get_RestTime();
            allThis = date.Hours * 60 + date.Minutes;
            double percents = allThis / onePercent;
            return percents;
        }
        /// <summary>
        ///  700-PRM-1100. Доделать, чтобы IdleRpm бралось из настроек конкретной машины
        /// </summary>
        /// <returns>время идеальных оборотов двигателя</returns>
        public TimeSpan Get_IdleRpmTime()
        {
            int indexes = 0;
            foreach (PLFRecord record in Records)
            {
                if ((Double.Parse(record.ENGINE_RPM.Replace('.', ',')) >= 700) && (Double.Parse(record.ENGINE_RPM.Replace('.', ',')) <= 1100))
                {
                    indexes++;
                }
            }
            TimeSpan DrivingTime = new TimeSpan(0,0, Convert.ToInt32(this.TIME_STEP) * indexes);//time_step*i
            return DrivingTime;
        }
        /// <summary>
        /// Engine_RPM!=0
        /// </summary>
        /// <returns>время работы двигателя</returns>
        public TimeSpan Get_WorkingEngineTime()
        {
            int indexes = 0;
            for (int i = 0; i < Records.Count; i++)
            {
                if (Records[i].ENGINE_RPM != "0")
                    indexes++;
            }

            TimeSpan retTime = new TimeSpan(0, 0, 0, indexes * Convert.ToInt32(TIME_STEP), 0);
            return retTime;
        }
        /// <summary>
        /// Engine_RPM!=0 && SPEED==0
        /// </summary>
        /// <returns>время работы двигателя без движения</returns>
        public TimeSpan Get_WorkingEngineWithNoMovingTime()
        {
            int indexes = 0;
            for (int i = 0; i < Records.Count; i++)
            {
                if (Records[i].ENGINE_RPM != "0" && Records[i].SPEED == "0")
                    indexes++;
            }
            TimeSpan retTime = new TimeSpan(0, 0, 0, indexes * Convert.ToInt32(TIME_STEP), 0);
            return retTime;
        }
        /// <summary>
        /// Процент идеальных оборотов двигателя
        /// </summary>
        /// <returns>double процент идеальных оборотов двигателя</returns>
        public double Get_IdleRpmPercent()
        {
            TimeSpan date = Get_AllWorkingTime();
            int allThis = date.Hours * 60 + date.Minutes;
            double onePercent = (double)allThis / 100;
            date = Get_IdleRpmTime();
            allThis = date.Hours * 60 + date.Minutes;
            double percents = allThis / onePercent;
            return percents;
        }
        /// <summary>
        /// SPEED==0 && ENGINE_RPM==0
        /// </summary>
        /// <returns>Время отдыха</returns>
        public TimeSpan Get_RestTime()
        {
            int indexes = 0;
            foreach (PLFRecord record in Records)
            {
                if (record.SPEED == "0" && record.ENGINE_RPM=="0" )
                {
                    indexes++;
                }
            }
            TimeSpan DrivingTime = new TimeSpan(0, 0, Convert.ToInt32(this.TIME_STEP) * indexes);//time_step*i
            return DrivingTime;
        }
        /// <summary>
        /// Получает все заправки
        /// </summary>
        /// <returns>Список обьектов типа PLFRefillClass</returns>
        public List<PLFRefillClass> Get_AllRefills()
        {
            double minRefillLitres = 4;
            double minRefillLitresAll = 19;
            List<PLFRefillClass> allRefills = new List<PLFRefillClass>();
            PLFRefillClass oneRefill = new PLFRefillClass();
            if(Records.Count<0)
                return allRefills;
            double prev = Records[0].getDoubleParam(Records[0].FUEL_VOLUME1);
            double current;
            bool refillStart = true;
            double refillLitresCount = 0;
            for(int i=1;i<Records.Count;i++)
            {
                current = Records[i].getDoubleParam(Records[i].FUEL_VOLUME1);
                if ((current - prev) > minRefillLitres)
                {                   
                    if (refillStart)
                    {
                        oneRefill.capacityStart = prev;
                        oneRefill.timeStart = Records[i - 1].SYSTEM_TIME.GetSystemTime();
                        refillStart = false;
                    }                    
                     refillLitresCount += current - prev;
                }
                else
                {
                    if (!refillStart)
                    {
                        if (refillLitresCount > minRefillLitresAll)
                        {
                            oneRefill.capacityEnd = prev;
                            oneRefill.timeEnd = Records[i - 1].SYSTEM_TIME.GetSystemTime();
                            allRefills.Add(oneRefill);
                        }
                        oneRefill = new PLFRefillClass();
                        refillStart = true;
                        refillLitresCount = 0;
                    }
                }
                prev = current;
            }
            return allRefills;
        }
        /// <summary>
        /// Получает все сливы
        /// </summary>
        /// <returns>Список обьектов типа PLFRefillClass</returns>
        public List<PLFRefillClass> Get_AllDropOuts()
        {
            double minSPEED = 3;
            double minDropOutLitresAll = 19;
            double startFuelDropout = 7;
            bool newDropout = false;
            List<PLFRefillClass> allDropouts = new List<PLFRefillClass>();
            PLFRefillClass oneDropout = new PLFRefillClass();
            if (Records.Count < 0)
                return allDropouts;
            for (int i = 1; i < Records.Count; i++)
            {
                if (Records[i].getDoubleParam(Records[i].SPEED) < minSPEED)
                {
                    if (newDropout == false)
                    {
                        newDropout = true;
                        oneDropout.capacityStart = Records[i].getDoubleParam(Records[i].FUEL_VOLUME1);
                        oneDropout.timeStart = Records[i].SYSTEM_TIME.GetSystemTime();
                        startFuelDropout = Records[i].getDoubleParam(Records[i].FUEL_VOLUME1);
                    }
                }
                else
                {
                    if (newDropout == true)
                    {
                        newDropout = false;
                        if (Records[i].getDoubleParam(Records[i].FUEL_VOLUME1) < (startFuelDropout - minDropOutLitresAll))
                        {
                            oneDropout.capacityEnd = Records[i].getDoubleParam(Records[i].FUEL_VOLUME1);
                            oneDropout.timeEnd = Records[i].SYSTEM_TIME.GetSystemTime();
                            allDropouts.Add(oneDropout);
                        }
                        oneDropout = new PLFRefillClass();
                    }
                }
            }
            return allDropouts;
        }
        /// <summary>
        /// Все пройденное расстрояние
        /// </summary>
        /// <returns>Все пройденное расстояние</returns>
        public double GetPath()
        {
            double path = 0f;
            foreach (PLFRecord record in Records)
            {
                path += Double.Parse(record.DISTANCE_COUNTER.Replace('.', ','));
            }
            return path;
        }
        /// <summary>
        /// Возвращает показания Одометра, пока только 0
        /// </summary>
        /// <returns>int, всегда 0</returns>
        public int GetOdometer()
        {
            return 0;
        }
        /// <summary>
        /// Сумма уровня топлива... оО Думаю неверно. нужно сделать расход или типа того.
        /// </summary>
        /// <returns>Топливо</returns>
        public double GetFuel()
        {
            double fuel = 0;
            foreach (PLFRecord record in Records)
            {
                fuel += Double.Parse(record.FUEL_VOLUME1.Replace('.', ','));
            }
            fuel = fuel / Records.Count;
            return fuel;
        }
        /// <summary>
        /// Максимальное непрерывное время движения
        /// </summary>
        /// <returns>Максимальное непрерывное время движения</returns>
        public TimeSpan MaxContinuousMovingTime()
        {
            int maxsecondsCount = Convert.ToInt32(TIME_STEP) * 2;
            int MaxTimeCounter = 0;
            int CurrentMaxTimeCounter = 0;
            if (Records.Count < 0)
                return new TimeSpan();
            DateTime prevRecordTime = new DateTime();
            DateTime curRecordTime = new DateTime();
            TimeSpan substractRESULT = new TimeSpan();
            for (int i = 0; i < Records.Count; i++)
            {
                if (Records[i].SPEED != "0" && CurrentMaxTimeCounter == 0)
                {
                    prevRecordTime = Records[i].SYSTEM_TIME.GetSystemTime();
                    CurrentMaxTimeCounter++;
                }
                else
                {
                    curRecordTime = Records[i].SYSTEM_TIME.GetSystemTime();
                    substractRESULT = curRecordTime.Subtract(prevRecordTime);
                    int seconds = (int)substractRESULT.TotalSeconds;
                    if (Records[i].SPEED != "0" && (seconds <= maxsecondsCount))
                    {
                        CurrentMaxTimeCounter+=seconds;
                    }
                    else
                    {
                        if (MaxTimeCounter < CurrentMaxTimeCounter)
                        {
                            MaxTimeCounter = CurrentMaxTimeCounter;
                        }
                        CurrentMaxTimeCounter = 0;
                    }
                }
                prevRecordTime = Records[i].SYSTEM_TIME.GetSystemTime();
            }
            TimeSpan maxTime = new TimeSpan(0, 0, 0, MaxTimeCounter);
            return maxTime;
        }
        /// <summary>
        ///  Максимальное непрерывное время простоя(Speed==0)
        /// </summary>
        /// <returns>Максимальное непрерывное время простоя</returns>
        public TimeSpan MaxContinuousWorkingWithNoMovingTime()
        {
            int maxsecondsCount = Convert.ToInt32(TIME_STEP)*2;
            int MaxTimeCounter = 0;
            int CurrentMaxTimeCounter = 0;
            if (Records.Count < 0)
                return new TimeSpan();
            DateTime prevRecordTime = new DateTime();
            DateTime curRecordTime = new DateTime();
            TimeSpan substractRESULT = new TimeSpan();
            for (int i = 0; i < Records.Count; i++)
            {
                if (Records[i].SPEED == "0" && CurrentMaxTimeCounter == 0)
                {
                    CurrentMaxTimeCounter++;
                }
                else
                {
                    curRecordTime = Records[i].SYSTEM_TIME.GetSystemTime();
                    substractRESULT = curRecordTime.Subtract(prevRecordTime);
                    int seconds = (int)substractRESULT.TotalSeconds;
                    if (Records[i].SPEED == "0" && (seconds <= maxsecondsCount))
                    {
                        CurrentMaxTimeCounter += seconds;
                    }
                    else
                    {
                        if (MaxTimeCounter < CurrentMaxTimeCounter)
                        {
                            MaxTimeCounter = CurrentMaxTimeCounter;
                        }
                        CurrentMaxTimeCounter = 0;
                    }
                }
                prevRecordTime = Records[i].SYSTEM_TIME.GetSystemTime();
            }
            TimeSpan maxTime = new TimeSpan(0, 0, 0, MaxTimeCounter);
            return maxTime;
        }
        /// <summary>
        /// Максимальный непрерывный пройденный путь
        /// </summary>
        /// <returns>Максимальный непрерывный пройденный путь</returns>
        public double MaxContinuousPath()
        {
            int maxsecondsCount = Convert.ToInt32(TIME_STEP) * 2;
            double MaxPath = 0;
            double CurrentMaxPath = 0;
            if (Records.Count < 0)
                return 0;
            DateTime prevRecordTime = new DateTime();
            DateTime curRecordTime = new DateTime();
            TimeSpan substractRESULT = new TimeSpan();
            for (int i = 0; i < Records.Count; i++)
            {
                if (Records[i].SPEED != "0" && CurrentMaxPath == 0)
                {
                    CurrentMaxPath++;
                }
                else
                {
                    curRecordTime = Records[i].SYSTEM_TIME.GetSystemTime();
                    substractRESULT = curRecordTime.Subtract(prevRecordTime);
                    int seconds = (int)substractRESULT.TotalSeconds;
                    if (Records[i].SPEED != "0" && (seconds <= maxsecondsCount))
                    {
                        CurrentMaxPath += Records[i].getDoubleParam(Records[i].DISTANCE_COUNTER);
                    }
                    else
                    {
                        if (MaxPath < CurrentMaxPath)
                        {
                            MaxPath = CurrentMaxPath;
                        }
                        CurrentMaxPath = 0;
                    }
                }
                prevRecordTime = Records[i].SYSTEM_TIME.GetSystemTime();
            }
            return MaxPath;
        }
        /// <summary>
        /// Сглаживание функции. Для подсчета заправок сперва нужно сгладить значения(убрать скачки от болтания) Алгоритм крайне простой и не требует описания.
        /// </summary>
        /// <param name="recordsList">передавать можно List-PLFRecord- только с заполнеными полями FUEL_VOLUME1</param>
        /// <returns>возвразает массив Double, в нем сглаженный уровень топлива</returns>
        public static List<double> FFT_Fuel(ref List<PLFRecord> recordsList)
        {
            List<double> fuelVol = new List<double>();
            foreach (PLFRecord record in recordsList)
            {
                fuelVol.Add(record.getDoubleParam(record.FUEL_VOLUME1));
            }
            List<double> fuelVolAproximated = new List<double>();
            double singleAprVal = 0;
            fuelVolAproximated.Add(fuelVol[0]);
            fuelVolAproximated.Add(fuelVol[1]);
            fuelVolAproximated.Add(fuelVol[2]);
            //fuelVolAproximated.Add(fuelVol[3]);
            for (int i = 3; i < fuelVol.Count-3; i++)
            {
                //+-3
                singleAprVal = (fuelVol[i - 3] + fuelVol[i - 2] + fuelVol[i - 1] + fuelVol[i] + fuelVol[i + 1] + fuelVol[i + 2] + fuelVol[i + 3]) / 7;
                //+-4
                //singleAprVal = (fuelVol[i - 4]*0.5 + fuelVol[i - 3]*0.9 + fuelVol[i - 2] + fuelVol[i - 1] + fuelVol[i] + fuelVol[i + 1] + fuelVol[i + 2] + fuelVol[i + 3]*0.9 + fuelVol[i + 4]*0.5) / 7.8;
                fuelVolAproximated.Add(singleAprVal);
            }
           // fuelVolAproximated.Add(fuelVol[fuelVol.Count - 4]);
            fuelVolAproximated.Add(fuelVol[fuelVol.Count - 3]);
            fuelVolAproximated.Add(fuelVol[fuelVol.Count - 2]);
            fuelVolAproximated.Add(fuelVol[fuelVol.Count - 1]);

            for(int i=0;i<fuelVolAproximated.Count;i++)
            {
                recordsList[i].FUEL_VOLUME1 = fuelVolAproximated[i].ToString();
            }
           
            return fuelVol;
        }
    }
}
