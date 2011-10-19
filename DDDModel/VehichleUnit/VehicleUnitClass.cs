using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Globalization;

namespace VehichleUnit
{
    /// <summary>
    /// Этот класс содержит в себе все остальные классы в данном
    /// пространстве имен и полностью описывает стрктуру ДДД файла транспортного средства
    /// </summary>
    [Serializable]
    public class VehicleUnitClass
    {
        public int cardType { get; set; } // тип карты - 0 driver, 1 - vehicle 
        public Vehicle_Overview          vehicleOverview { get; set; }        // SID 76H, TREP 01H: overview            !done
        public Vehicle_Detailed_Speed    vehicleDetailedSpeed { get; set; }   // SID 76H, TREP 04H: detailed speed      !done
        public Vehicle_Events_And_Faults vehicleEventsAndFaults { get; set; } // SID 76H, TREP 03H: events and faults   !done
        public List<Vehicle_Activities>  vehicleActivities { get; set; }      // SID 76H, TREP 02H: activities
        public Vehicle_Technical_Data    vehicleTechnicalData { get; set; }   // SID 76H, TREP 05H: technical data      !done  

        public string SerializeVehicleClass(string output)
        {
            DateTime dateTime;
            string fileName;
            string YYYYMMDD_HHmm;
            string format;

            dateTime = vehicleOverview.currentDateTime.getTimeRealDate();
            format = "yyyyMMdd_HHmm";

            YYYYMMDD_HHmm = dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);

            fileName = "M_" + YYYYMMDD_HHmm
                + "_"
                + vehicleOverview.vehicleRegistrationIdentification.vehicleRegistrationNumber.ToString()
                + "_"
                + vehicleOverview.vehicleIdentificationNumber.vehicleIdentificationNumber
                + ".XML";

            XmlSerializer xmlFormat = new XmlSerializer ( typeof(VehicleUnitClass));
            using (Stream fStream = new FileStream(output + fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xmlFormat.Serialize(fStream, this);
            }
            return fileName;
        }

        public static VehicleUnitClass DeserializeCardUnitClass(string filename)
        {   
            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer serializer = new XmlSerializer(typeof(VehicleUnitClass));

            // A FileStream is needed to read the XML document.
            FileStream fs = new FileStream(filename, FileMode.Open);
            XmlReader reader = new XmlTextReader(fs);

            // Declare an object variable of the type to be deserialized.
            VehicleUnitClass i = new VehicleUnitClass();

            // Use the Deserialize method to restore the object's state.
            i = (VehicleUnitClass)serializer.Deserialize(reader);

            reader.Close();
            fs.Close();
            return i;
        }
        /// <summary>
        /// Получает все время активности Транспортного средства
        /// </summary>
        /// <returns>тип TimeSpan</returns>
        public TimeSpan GetTotalVehicleActivitiesTime()
        {
            TimeSpan totals = new TimeSpan();
            foreach (Vehicle_Activities record in vehicleActivities)
            {
                totals = totals.Add(record.vuActivityDailyData.Get_TotalTimeSpan());
            }
            return totals;
        }
    }
}
