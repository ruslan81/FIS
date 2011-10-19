using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLFUnit
{
    /// <summary>
    /// Класс описывает одну запись в PLF файле
    /// </summary>
    public class PLFRecord
    {        
        public PLFSystemTime SYSTEM_TIME { get; set; }
        public string FUEL_VOLUME1 { get; set; }
        public string FUEL_VOLUME2 { get; set; }
        public string SPEED { get; set; }
        public string FUEL_COUNTER { get; set; }
        public string DISTANCE_COUNTER { get; set; }
        public string FUEL_CONSUMPTION { get; set; }
        public string ENGINE_RPM { get; set; }
        public string VOLTAGE { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
        public string ALTITUDE { get; set; }
        public string TEMPERATURE1 { get; set; }
        public string TEMPERATURE2 { get; set; }
        public string WEIGHT1 { get; set; }
        public string WEIGHT2 { get; set; }
        public string WEIGHT3 { get; set; }
        public string WEIGHT4 { get; set; }
        public string WEIGHT5 { get; set; }
        public string MAIN_STATES { get; set; }
        public string ADDITIONAL_SENSORS { get; set; }
        public string RESERVED_3 { get; set; }
        public string RESERVED_4 { get; set; }
        public string RESERVED_5 { get; set; }
        [System.Xml.Serialization.XmlIgnore] public string YesString;
        [System.Xml.Serialization.XmlIgnore] public string NoString;

        public PLFRecord()
        {
            YesString = "Y";
            NoString = "N";
            SYSTEM_TIME = new PLFSystemTime();
            FUEL_VOLUME1 = null;
            FUEL_VOLUME2 = null;
            SPEED = null;
            FUEL_COUNTER = null;
            DISTANCE_COUNTER = null;
            FUEL_CONSUMPTION = null;
            ENGINE_RPM = null;
            VOLTAGE = null;
            LATITUDE = null;
            LONGITUDE = null;
            ALTITUDE = null;
            TEMPERATURE1 = null;
            TEMPERATURE2 = null;
            WEIGHT1 = null;
            WEIGHT2 = null;
            WEIGHT3 = null;
            WEIGHT4 = null;
            WEIGHT5 = null;
            MAIN_STATES = null;
            ADDITIONAL_SENSORS = null;
            RESERVED_3 = null;
            RESERVED_4 = null;
            RESERVED_5 = null;
        }        
        /// <summary>
        /// Устанавливает значение всех параметров в N(когда параметр в N, значит нет датчика и информация отсутствует.
        /// </summary>
        public void SetNForAllParams()
        {
            SYSTEM_TIME = new PLFSystemTime();
            SYSTEM_TIME.systemTime = NoString;
            FUEL_VOLUME1 = NoString;
            FUEL_VOLUME2 = NoString;
            SPEED = NoString;
            FUEL_COUNTER = NoString;
            DISTANCE_COUNTER = NoString;
            FUEL_CONSUMPTION = NoString;
            ENGINE_RPM = NoString;
            VOLTAGE = NoString;
            LATITUDE = NoString;
            LONGITUDE = NoString;
            ALTITUDE = NoString;
            TEMPERATURE1 = NoString;
            TEMPERATURE2 = NoString;
            WEIGHT1 = NoString;
            WEIGHT2 = NoString;
            WEIGHT3 = NoString;
            WEIGHT4 = NoString;
            WEIGHT5 = NoString;
            MAIN_STATES = NoString;
            ADDITIONAL_SENSORS = NoString;
            RESERVED_3 = NoString;
            RESERVED_4 = NoString;
            RESERVED_5 = NoString;
        }
        /// <summary>
        /// из строки преобразует в Double. Меняет с точки на запятую разделитель и делает Double.Parse()
        /// </summary>
        /// <param name="param">любой параметр, который нужно из строки перевести в double</param>
        /// <returns>double</returns>
        public double getDoubleParam(string param)
        {
            if (param != null)
                return Double.Parse(param.Replace('.', ','));
            else
                return 0;
        }
    }
}
