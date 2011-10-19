using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Globalization;

namespace CardUnit
{
    /// <summary>
    /// Класс полностью описывает структуру ДДД карточки.
    /// </summary>
    [Serializable]
    public class CardUnitClass
    {
        /// <summary>
        /// тип карты
        /// </summary>
        public int cardType { get; set; } // тип карты - 0 driver, 1 - vehicle 
        public EF_ICC ef_icc { get; set; }//!
        public EF_IC ef_ic { get; set; }//!
        public EF_Application_Identification ef_application_identification { get; set; }//!
        public EF_Card_Certificate ef_card_certificate { get; set; }
        public EF_CA_Certificate ef_ca_certificate { get; set; }
        public EF_Identification ef_identification { get; set; } ////!!
        public EF_Card_Download ef_card_download { get; set; }//!
        public EF_Driving_Licence_Info ef_driving_licence_info { get; set; }//!
        public EF_Events_Data ef_events_data { get; set; }//!
        public EF_Faults_Data ef_faults_data { get; set; }//!
        public EF_Driver_Activity_Data ef_driver_activity_data { get; set; }//!
        public EF_Vehicles_Used ef_vehicles_used { get; set; }//!
        public EF_Places ef_places { get; set; }//!
        public EF_Current_Usage ef_current_usage { get; set; }//!
        public EF_Control_Activity_Data ef_control_activity_data { get; set; }//!
        public EF_Specific_Conditions ef_specific_conditions { get; set; }//!
        public EF_Calibration ef_calibration { get; set; }//Нет в картах такой информации, когда появиться - доделать непременно!!!!
        public EF_Sensor_Installation_Data ef_sensor_installation_data { get; set; }//Нет в картах такой информации, когда появиться - доделать непременно!!!!
        public EF_Controller_Activity_Data ef_controller_activity_data { get; set; }//Нет в картах такой информации, когда появиться - доделать непременно!!!!
        public EF_Company_Activity_Data ef_company_activity_data { get; set; }//Нет в картах такой информации, когда появиться - доделать непременно!!!!
        /// <summary>
        /// сериализует обьект в XML
        /// </summary>
        /// <param name="output">путь, куда сохранять XML</param>
        /// <returns>имя файла</returns>
        public string SerializeCardUnitClass(string output)
        {
            DateTime dateTime;
            string fileName;
            string YYYYMMDD_HHmm;
            string format;

            dateTime = ef_identification.cardIdentification.cardValidityBegin.getTimeRealDate();
            format = "yyyyMMdd_HHmm";

            YYYYMMDD_HHmm = dateTime.ToString(format, DateTimeFormatInfo.InvariantInfo);

            fileName = "C_" + YYYYMMDD_HHmm + "(дату пока не знаю какую брать)_"
                + ef_identification.driverCardHolderIdentification.cardHolderName.holderFirstNames.ToString()[0]
                + "_"
                + ef_identification.driverCardHolderIdentification.cardHolderName.holderSurname.ToString()
                + "_"
                + ef_identification.cardIdentification.cardNumber.driverIdentification
                + ".XML";


            XmlSerializer xmlFormat = new XmlSerializer(typeof(CardUnitClass));
            using (Stream fStream = new FileStream(output + fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                xmlFormat.Serialize(fStream, this);
            }
            return fileName;
        }
        /// <summary>
        /// Десериализует XML в обьект класса CardUnitClass
        /// </summary>
        /// <param name="filename">путь к файлу XML</param>
        /// <returns>созданный обьект CardUnitClass</returns>
        public static CardUnitClass DeserializeCardUnitClass(string filename)
        {
            Console.WriteLine("Reading with XmlReader");

            // Create an instance of the XmlSerializer specifying type and namespace.
            XmlSerializer serializer = new XmlSerializer(typeof(CardUnitClass));

            // A FileStream is needed to read the XML document.
            FileStream fs = new FileStream(filename, FileMode.Open);
            XmlReader reader = new XmlTextReader(fs);

            // Declare an object variable of the type to be deserialized.
            CardUnitClass i;

            // Use the Deserialize method to restore the object's state.
            i = (CardUnitClass)serializer.Deserialize(reader);

            reader.Close();
            fs.Close();
            return i;
        }
    }
}
