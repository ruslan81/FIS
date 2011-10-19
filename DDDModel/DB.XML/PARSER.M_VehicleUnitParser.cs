using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VehichleUnit;

namespace PARSER
{
    /// <summary>
    /// Парсер M_ файлов(DDD для транспортных средств)
    /// </summary>
    public class M_VehicleUnitParser
    {
        private  Vehicle_Overview vehicleOverview;
        private  Vehicle_Detailed_Speed vehicleDetailedSpeed;
        private  Vehicle_Events_And_Faults vehicleEventsAndFaults;
        private  List<Vehicle_Activities> vehicleActivities;
        private  Vehicle_Technical_Data vehicleTechnicalData;

        private VehicleUnitClass vehicleUnitClass;

        /// <summary>
        /// Переменная указывает какой блок данных сейчас обрабатывается.
        /// </summary>
        private int trep;
        /// <summary>
        /// Конструктор
        /// </summary>
        public M_VehicleUnitParser()
        { }
        /// <summary>
        /// Разбор ДДД файла для ТС
        /// </summary>
        /// <param name="src">ДДД файл</param>
        /// <returns>VehicleUnitClass</returns>
        public VehicleUnitClass VehicleUnitData_Parse(byte[] src)
        {
            vehicleOverview = new Vehicle_Overview();
            vehicleDetailedSpeed = new Vehicle_Detailed_Speed();
            vehicleEventsAndFaults = new Vehicle_Events_And_Faults();
            vehicleActivities = new List<Vehicle_Activities>();
            vehicleTechnicalData = new Vehicle_Technical_Data();


            int pos = 0;
            int prdtLength;
            bool parseResult = true;


            while (true)
            {
                byte[] tag = new byte[2];
                byte[] value;

                while (true)
                {
                    prdtLength = 0;

                    if (src.Length < pos + 2)
                    {
                        // end of stream
                        // break tag parser
                        break;
                    }

                    // copy tag
                    Array.Copy(src, pos, tag, 0, 2);
                    pos += 2;
                    isValidSIDTREP(tag);

                    if (trep == 1)//76h 01h
                    {
                        prdtLength = 194 + 194 + 17 + 1 + 14 + 4 + 4 + 4 + 1 + 4 + 18 + 36;
                        int noOfLocks = (src[pos + prdtLength] & 0xff);
                        prdtLength += 1;
                        prdtLength += (noOfLocks * 98);
                        int noOfControls = (src[pos + prdtLength] & 0xff);
                        prdtLength += 1;
                        prdtLength += (noOfControls * 31);
                        // signature length
                        prdtLength += 128;
                        parseResult = true;
                        break;
                    }
                    else if (trep == 2)//76h 02h
                    {
                        prdtLength = 4 + 3;

                        int noOfVuCardIWRecords = ((src[pos + prdtLength] & 0xff) << 8) + (src[pos + prdtLength + 1] & 0xff);
                        prdtLength += 2;
                        prdtLength += (noOfVuCardIWRecords * 129);
                        int noOfActivityChanges = ((src[pos + prdtLength] & 0xff) << 8) + (src[pos + prdtLength + 1] & 0xff);
                        prdtLength += 2;
                        prdtLength += (noOfActivityChanges * 2);
                        int noOfPlaceRecords = src[pos + prdtLength] & 0xff;
                        prdtLength += 1;
                        prdtLength += (noOfPlaceRecords * 28);
                        int noOfSpecificConditionsRecords = ((src[pos + prdtLength] & 0xff) << 8) + (src[pos + prdtLength + 1] & 0xff);
                        prdtLength += 2;
                        prdtLength += (noOfSpecificConditionsRecords * 5);
                        // signature length
                        prdtLength += 128;
                        parseResult = true;
                        break;
                    }
                    else if (trep == 3)//76h 03h
                    {
                        int noOfVuFaults = src[pos + prdtLength] & 0xff;
                        prdtLength += 1;
                        prdtLength += (noOfVuFaults * 82);
                        int noOfVuEvents = src[pos + prdtLength] & 0xff;
                        prdtLength += 1;
                        prdtLength += (noOfVuEvents * 83);
                        prdtLength += 4 + 4 + 1;
                        int noOfVuOverSpeedingRecords = src[pos + prdtLength] & 0xff;
                        prdtLength += 1;
                        prdtLength += (noOfVuOverSpeedingRecords * 31);
                        int noOfVuTimeAdjRecords = src[pos + prdtLength] & 0xff;
                        prdtLength += 1;
                        prdtLength += (noOfVuTimeAdjRecords * 98);
                        // signature length
                        prdtLength += 128;
                        parseResult = true;
                        break;
                    }
                    else if (trep == 4)//76h 04h
                    {
                        int noOfSpeedBlocks = ((src[pos + prdtLength] & 0xff) << 8) + (src[pos + prdtLength + 1] & 0xff);
                        prdtLength += 2;
                        prdtLength += (noOfSpeedBlocks * 64);
                        // signature length
                        prdtLength += 128;
                        parseResult = true;
                        break;
                    }
                    else if (trep == 5)//76h 05h
                    {
                        prdtLength = 36 + 36 + 16 + 8 + 4 + 4 + 4 + 8 + 8 + 8 + 4;
                        int noOfVuCalibrationsRecords = (src[pos + prdtLength] & 0xff);
                        prdtLength += 1;
                        prdtLength += (noOfVuCalibrationsRecords * 167);
                        // signature length
                        prdtLength += 128;
                        parseResult = true;
                        break;
                    }
                    else
                    {
                        parseResult = false;
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
                    // end of stream
                    // break data parser
                    break;
                }

                if (src.Length < pos + prdtLength)
                {
                    parseResult = false;
                    // break data parser
                    break;
                }

                // copy value
                value = new byte[prdtLength];
                Array.Copy(src, pos, value, 0, prdtLength);
                pos += prdtLength;

                // add data
                switch (trep)
                {
                    case 1:
                        {
                            vehicleOverview = new Vehicle_Overview(value);
                        }
                        break;
                    case 2:
                        {
                            vehicleActivities.Add(new Vehicle_Activities(value));
                        }
                        break;
                    case 3:
                        {
                            vehicleEventsAndFaults = new Vehicle_Events_And_Faults(value);
                        }
                        break;
                    case 4:
                        {
                            vehicleDetailedSpeed = new Vehicle_Detailed_Speed(value);
                        }
                        break;
                    case 5:
                        {
                            vehicleTechnicalData = new Vehicle_Technical_Data(value);
                        }
                        break;
                    default:
                        {
                            parseResult = false;
                        }
                        break;
                }
                if (parseResult == false)
                {
                    // break data parser
                    break;
                }
            }// end data parser

            vehicleUnitClass = new VehicleUnitClass();
            vehicleUnitClass.vehicleActivities      = vehicleActivities;
            vehicleUnitClass.vehicleDetailedSpeed   = vehicleDetailedSpeed;
            vehicleUnitClass.vehicleEventsAndFaults = vehicleEventsAndFaults;
            vehicleUnitClass.vehicleOverview        = vehicleOverview;
            vehicleUnitClass.vehicleTechnicalData   = vehicleTechnicalData;

            return vehicleUnitClass;
        }
        /// <summary>
        /// Смотрит тэг, выбирает какой блок данных идет далее и устанавиливает глобальную переменную.
        /// </summary>
        /// <param name="tag">два байта с описанием следующего блока данных</param>
        /// <returns>Возвращает строку, использовалась ранее, для вывода в консоль, сейчас пока ненадо. МОжно использовать для записи лога.</returns>
        private string isValidSIDTREP(byte[] tag)//2 bytes
        {
            string result;
            if ((HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x76, 0x01 })))
            {
                trep = 1;
                result = " SID 76H, TREP 01H: overview";
            }
            else if (HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x76, 0x02 }))
            {
                trep = 2;
                result = " SID 76H, TREP 02H: activities";
            }
            else if ((HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x76, 0x03 })))
            {
                trep = 3;
                result = " SID 76H, TREP 03H: events and faults";
            }
            else if ((HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x76, 0x04 })))
            {
                trep = 4;
                result = " SID 76H, TREP 04H: detailed speed";
            }
            else if ((HexBytes.CompareByteArrays(new byte[] { tag[0], tag[1] }, new byte[] { 0x76, 0x05 })))
            {
                trep = 5;
                result = " SID 76H, TREP 05H: technical data";

            }
            else
            {
                trep = 321;
                return "Error, not Vehicle Unit file!\n\r";
            }

            return result;
        }
    }
}