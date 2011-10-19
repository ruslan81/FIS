using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;
using CardUnit;

namespace PARSER
{
    /// <summary>
    /// Парсер для ДДД файлов карточек водителя и других.
    /// </summary>
    public class C_DriversParser
    {
        private EF_ICC ef_icc;
        private EF_IC ef_ic;
        private EF_Application_Identification ef_application_identification;
        private EF_Card_Certificate ef_card_certificate;
        private EF_CA_Certificate ef_ca_certificate;
        private EF_Identification ef_identification;
        private EF_Card_Download ef_card_download;
        private EF_Driving_Licence_Info ef_driving_licence_info;
        private EF_Events_Data ef_events_data;
        private EF_Faults_Data ef_faults_data;
        private EF_Driver_Activity_Data ef_driver_activity_data;
        private EF_Vehicles_Used ef_vehicles_used;
        private EF_Places ef_places;
        private EF_Current_Usage ef_current_usage;
        private EF_Control_Activity_Data ef_control_activity_data;
        private EF_Specific_Conditions ef_specific_conditions;
        private EF_Calibration ef_calibration;
        private EF_Sensor_Installation_Data ef_sensor_installation_data;
        private EF_Controller_Activity_Data ef_controller_activity_data;
        private EF_Company_Activity_Data ef_company_activity_data;

        private CardUnitClass cardUnitClass;
        /// <summary>
        /// указывает, какой блок данных обрабатывать
        /// </summary>
        private int fileId;
        /// <summary>
        /// типа карты
        /// </summary>
        private short cardType;
        private byte[] ef_card_value;

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public C_DriversParser()
        {
        }

        /// <summary>
        /// Разбирает ДДД файл водителя(или другой карты)
        /// </summary>
        /// <param name="src">файл ДДД</param>
        /// <returns>обьект класса CardUnitClass</returns>
        public CardUnitClass CardUnitData_Parse(byte[] src)
        {
            bool parseResult = false;
            int pos = 0;
            cardUnitClass = new CardUnitClass();

            while (true)
            {
                byte[] tag = new byte[3];
                byte[] length = new byte[2];
                int length_i = 0;

                byte[] value;

                // tag parser
                while (true)
                {
                    if (src.Length < pos + 3)
                    {
                        // end of stream
                        // break tag parser
                        break;
                    }

                    // copy tag
                    Array.Copy(src, pos, tag, 0, 3);
                    pos += 3;
                    isValidFileID(tag);
                    if (fileId == -1) //Invalid Tag
                    {
                        if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x76, 0x06 }))
                        {
                            // valid tag
                            // OPTAC download tools with firmware < v2.3 write two bytes (76 06, SID/TREP?!) at the
                            // beginning of a .DDD file that are out of specs...
                            pos -= 1;
                        }
                        else
                        {
                            // invalid tag
                            pos -= 2;
                        }
                    }
                    else
                    {
                        parseResult = true;
                        // break tag parser
                        break;
                    }
                }// end tag parser

                if (parseResult == false)
                {
                    // break data parser
                    break;
                }

                if (src.Length < pos + 2)
                {
                    // break data parser
                    break;
                }

                // copy length
                Array.Copy(src, pos, length, 0, 2);
                pos += 2;

                length_i = calculateLength(length);
                if (src.Length < pos + length_i)
                {
                    parseResult = false;
                    // break data parser
                    break;
                }

                // copy value
                value = new byte[length_i];
                Array.Copy(src, pos, value, 0, length_i);
                pos += length_i;

                // add data
                //------------------------------------------
               // parseresult = cardData.add(tag, length, value);
                switch (fileId)
                {
                    case 1://"EF_ICC";
                        {
                            ef_icc = new EF_ICC(value);
                        }
                        break;
                    case 2: //"EF_IC";
                        {
                            ef_ic = new EF_IC(value);
                        }
                        break;
                    case 3://EF_Card_Certificate
                        {
                            ef_card_certificate = new EF_Card_Certificate(value);
                            ef_card_value = value;
                        }
                        break;
                    case 4://EF_CA_Certificate
                        {
                            ef_ca_certificate = new EF_CA_Certificate(value);
                        }
                        break;
                    case 5://EF_Application_Identification
                        {
                                ef_application_identification = new EF_Application_Identification(value);
                                cardType = ef_application_identification.cardType;
                        }
                        break;

                    case 6://EF_Identification
                        {
                                ef_identification = new EF_Identification(value, cardType);
                        }
                        break;
                    case 7://EF_Card_Download (driver card)
                        {
                            ef_card_download = new EF_Card_Download(value, cardType);
                        }
                        break;
                    case 8://EF_Driving_Licence_Info
                        {
                                ef_driving_licence_info = new EF_Driving_Licence_Info(value);
                        }
                        break;
                    case 9://EF_Events_Data
                        {
                            ef_events_data = new EF_Events_Data(value, ef_application_identification.noOfEventsPerType);
                        }
                        break;
                    case 10://EF_Faults_Data
                        {
                            ef_faults_data = new EF_Faults_Data(value, ef_application_identification.noOfFaultsPerType);
                        }
                        break;
                    case 11://EF_Driver_Activity_Data
                        {
                            ef_driver_activity_data = new EF_Driver_Activity_Data(value, ef_application_identification.activityStructureLength);
                        }
                        break;
                    case 12://EF_Vehicles_Used
                        {
                            ef_vehicles_used = new EF_Vehicles_Used(value, ef_application_identification.noOfCardVehicleRecords);
                        }
                        break;
                    case 13://EF_Places
                        {
                            ef_places = new EF_Places(value, ef_application_identification.noOfCardPlaceRecords);
                        }
                        break;
                    case 14://EF_Current_Usage
                        {
                            ef_current_usage = new EF_Current_Usage(value);
                        }
                        break;
                    case 15://EF_Control_Activity_Data
                        {
                            ef_control_activity_data = new EF_Control_Activity_Data(value);
                        }
                        break;
                    case 16://EF_Specific_Conditions
                        {
                            ef_specific_conditions = new EF_Specific_Conditions(value, cardType);
                        }
                        break;
                    case 17://EF_Card_Download (workshop card)
                        {
                            ef_card_download = new EF_Card_Download(value, cardType);
                        }
                        break;
                    case 18://EF_Calibration
                        {
                            ef_calibration = new EF_Calibration(value, ef_application_identification.noOfCalibrationRecords);
                        }
                        break;
                    case 19://EF_Sensor_Installation_Data
                        {
                            ef_sensor_installation_data = new EF_Sensor_Installation_Data(value);
                        }
                        break;
                    case 20://EF_Controller_Activity_Data
                        {
                            ef_controller_activity_data = new EF_Controller_Activity_Data(value, ef_application_identification.noOfControlActivityRecords);
                        }
                        break;
                    case 21://EF_Company_Activity_Data
                        {
                            ef_company_activity_data = new EF_Company_Activity_Data(value, ef_application_identification.noOfCompanyActivityRecords);
                        }
                        break;
                    case -222:
                        {
                            //skipping signature Info!!
                        }
                        break;
                    default:
                        {
                            parseResult = false;
                        }
                        break;
                }

                //------------------------------------------
                if (parseResult == false)
                {
                    // break data parser
                    break;
                }
            }// end data parser           
            cardUnitClass.ef_application_identification = ef_application_identification;
            cardUnitClass.ef_ca_certificate = ef_ca_certificate;
            cardUnitClass.ef_calibration = ef_calibration;
            cardUnitClass.ef_card_certificate = ef_card_certificate;
            cardUnitClass.ef_card_download = ef_card_download;
            cardUnitClass.ef_company_activity_data = ef_company_activity_data;
            cardUnitClass.ef_control_activity_data = ef_control_activity_data;
            cardUnitClass.ef_controller_activity_data = ef_controller_activity_data;
            cardUnitClass.ef_current_usage = ef_current_usage;
            cardUnitClass.ef_driver_activity_data = ef_driver_activity_data;
            cardUnitClass.ef_driving_licence_info = ef_driving_licence_info;
            cardUnitClass.ef_events_data = ef_events_data;
            cardUnitClass.ef_faults_data = ef_faults_data;
            cardUnitClass.ef_ic = ef_ic;
            cardUnitClass.ef_icc = ef_icc;
            cardUnitClass.ef_identification = ef_identification;
            cardUnitClass.ef_places = ef_places;
            cardUnitClass.ef_sensor_installation_data = ef_sensor_installation_data;
            cardUnitClass.ef_specific_conditions = ef_specific_conditions;
            cardUnitClass.ef_vehicles_used = ef_vehicles_used;

            return cardUnitClass;
        } // end card data parser

        private int calculateLength(byte[] b)
        {
            return ((b[0] & 0xff) << 8) + (b[1] & 0xff);
        }
        /// <summary>
        /// Указывает, какой блок данных идет дальше
        /// </summary>
        /// <param name="tag">массив из двух байт, заголовок блока данных</param>
        /// <returns>название следующего блока и дополнительная информация. Использовать для дебага или лога.</returns>
        private string isValidFileID(byte[] tag)
        {
            string tagInfo;
            fileId = -1;

            if (tag.Length != 3)
            {
                fileId = -1;
                return " That's not 3 bytes!";
            }

            if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x00, 0x02 }))
            {
                //	EF_ICC
                fileId = 1;
                tagInfo = "EF_ICC";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x00, 0x05 }))
            {
                //	EF_IC
                fileId = 2;
                tagInfo = "EF_IC";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { (byte)0xc1, 0x00 }))
            {
                //	EF_Card_Certificate
                fileId = 3;
                tagInfo = "EF_Card_Certificate";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { (byte)0xc1, 0x08 }))
            {
                //	EF_CA_Certificate
                fileId = 4;
                tagInfo = "EF_CA_Certificate";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x01 }))
            {
                //	EF_Application_Identification
                fileId = 5;
                tagInfo = "EF_Application_Identification";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x20 }))
            {
                //	EF_Identification
                fileId = 6;
                tagInfo = "EF_Identification";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x0e }))
            {
                //	EF_Card_Download (driver card)
                fileId = 7;
                tagInfo = "EF_Card_Download (driver card)";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x21 }))
            {
                //	EF_Driving_Licence_Info
                fileId = 8;
                tagInfo = "EF_Driving_Licence_Info";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x02 }))
            {
                //	EF_Events_Data
                fileId = 9;
                tagInfo = "EF_Events_Data";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x03 }))
            {
                //	EF_Faults_Data
                fileId = 10;
                tagInfo = "EF_Faults_Data";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x04 }))
            {
                //	EF_Driver_Activity_Data
                fileId = 11;
                tagInfo = "EF_Driver_Activity_Data";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x05 }))
            {
                //	EF_Vehicles_Used
                fileId = 12;
                tagInfo = "EF_Vehicles_Used";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x06 }))
            {
                //	EF_Places
                fileId = 13;
                tagInfo = "EF_Places";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x07 }))
            {
                //	EF_Current_Usage
                fileId = 14;
                tagInfo = "EF_Current_Usage";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x08 }))
            {
                //	EF_Control_Activity_Data
                fileId = 15;
                tagInfo = "EF_Control_Activity_Data";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x22 }))
            {
                //	EF_Specific_Conditions
                fileId = 16;
                tagInfo = "EF_Specific_Conditions";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x09 }))
            {
                //	EF_Card_Download (workshop card)
                fileId = 17;
                tagInfo = "EF_Card_Download (workshop card)";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x0a }))
            {
                //	EF_Calibration
                fileId = 18;
                tagInfo = "EF_Calibration";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x0b }))
            {
                //	EF_Sensor_Installation_Data
                fileId = 19;
                tagInfo = "EF_Sensor_Installation_Data";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x0c }))
            {
                //	EF_Controller_Activity_Data
                fileId = 20;
                tagInfo = "EF_Controller_Activity_Data";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x05, 0x0d }))
            {
                //	EF_Company_Activity_Data
                fileId = 21;
                tagInfo = "EF_Company_Activity_Data";
            }
            else
            {
                fileId = -1;
                tagInfo = "Unknown tag name";
            }

            // type of tag
            if (tag[2] == 0x00)
            {
                // data tag
                tagInfo = tagInfo + " data tag";
            }
            else if (tag[2] == 0x01)
            {
                // signature tag
                tagInfo = tagInfo + " signature tag";
                fileId = -222; //скипаем цифровую подпись(по-крайней мере пока что)
            }
            else
            {
                // unknown tag
                tagInfo = tagInfo + " unknown tag type";
            }
            StringBuilder stringReturn = new StringBuilder("");
            stringReturn.AppendFormat("\nCard data tag: FID {0:x4} {1:x4} {2:x4}, {3}", tag[0], tag[1], tag[2], tagInfo);
            return stringReturn.ToString();
        }
    }
}
