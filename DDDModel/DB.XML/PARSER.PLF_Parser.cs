using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLFUnit;

namespace PARSER
{
    /// <summary>
    /// Парсер PLF файлов
    /// </summary>
    class PLF_Parser
    {
        /// <summary>
        /// Разбирает PLF файл, возвращает информацию в обьекте класса PLFUnitClass
        /// </summary>
        /// <param name="src">Битовый массив - считаный в память PLF файл</param>
        /// <returns>экземпляр PLFUnitClass</returns>
        public static PLFUnitClass PLFUnitData_Parse(byte[] src)
        {
            PLFUnitClass returnObject = new PLFUnitClass();
            List<string> records = new List<string>();
            System.Text.Encoding enc = System.Text.Encoding.Default;
            string myString = enc.GetString(src);
            string[] separator = { "#", "\r", "\n", "END", ";" };
            string[] splittedString;
            string YESString = returnObject.installedSensors.YesString;

            returnObject.installedSensors.SetNForAllParams();

            myString = DeleteComments(myString);

            splittedString = myString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            List<string> strings = splittedString.ToList<string>();

            #region "Y N Switch Region"
            bool yes = false;
            splittedString = new string[2];
            foreach (string str in strings)
            {
                splittedString = str.Split(new string[] { " = " }, StringSplitOptions.RemoveEmptyEntries);
                if (splittedString[1] == YESString)
                    yes = true;

                switch (splittedString[0])
                {
                    case "ID_DEVICE":
                        {
                            returnObject.ID_DEVICE = splittedString[1];
                        } break;
                    case "VEHICLE":
                        {
                            returnObject.VEHICLE = splittedString[1];
                        } break;
                    case "TIME_STEP":
                        {
                            returnObject.TIME_STEP = splittedString[1];
                        } break;

                    case "SYSTEM_TIME":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.SYSTEM_TIME.systemTime = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "FUEL_VOLUME1":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.FUEL_VOLUME1 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "FUEL_VOLUME2":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.FUEL_VOLUME2 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "SPEED":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.SPEED = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "FUEL_COUNTER":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.FUEL_COUNTER = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "DISTANCE_COUNTER":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.DISTANCE_COUNTER = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "FUEL_CONSUMPTION":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.FUEL_CONSUMPTION = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "ENGINE_RPM":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.ENGINE_RPM = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "VOLTAGE":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.VOLTAGE = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "LATITUDE":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.LATITUDE = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "LONGITUDE":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.LONGITUDE = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "ALTITUDE":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.ALTITUDE = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "TEMPERATURE1":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.TEMPERATURE1 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "TEMPERATURE2":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.TEMPERATURE2 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "WEIGHT1":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.WEIGHT1 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "WEIGHT2":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.WEIGHT2 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "WEIGHT3":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.WEIGHT3 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "WEIGHT4":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.WEIGHT4 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "WEIGHT5":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.WEIGHT5 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "MAIN_STATES":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.MAIN_STATES = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "ADDITIONAL_SENSORS":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.ADDITIONAL_SENSORS = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "RESERVED_3":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.RESERVED_3 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "RESERVED_4":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.RESERVED_4 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "RESERVED_5":
                        {
                            if (yes)
                            {
                                returnObject.installedSensors.RESERVED_5 = splittedString[1];
                                yes = false;
                            }
                        } break;
                    case "RECORD":
                        {
                            records.Add(splittedString[1]);
                        } break;
                }
            }
            #endregion

            PLFRecord plfRecord;
            int startIndex = 0;
            for (int recordIndex = startIndex; recordIndex < records.Count; recordIndex++)
            {
                plfRecord = new PLFRecord();
                splittedString = records[recordIndex].Split(new string[] { ",", " ", ";" }, StringSplitOptions.RemoveEmptyEntries);

                #region "Присваивание значений"

                if (returnObject.installedSensors.SYSTEM_TIME.systemTime == YESString)
                {
                    plfRecord.SYSTEM_TIME.systemTime = splittedString[0] + " " + splittedString[1];
                    if (recordIndex == startIndex)
                        returnObject.START_PERIOD = new PLFSystemTime(splittedString[0]+" " + splittedString[1]);
                    if(recordIndex == (records.Count-1))
                        returnObject.END_PERIOD = new PLFSystemTime(splittedString[0] + " " + splittedString[1]);
                }

                if (returnObject.installedSensors.FUEL_VOLUME1 == YESString)
                    plfRecord.FUEL_VOLUME1 = splittedString[2];

                if (returnObject.installedSensors.FUEL_VOLUME2 == YESString)
                    plfRecord.FUEL_VOLUME2 = splittedString[3];

                if (returnObject.installedSensors.SPEED == YESString)
                    plfRecord.SPEED = splittedString[4];

                if (returnObject.installedSensors.FUEL_COUNTER == YESString)
                    plfRecord.FUEL_COUNTER = splittedString[5];

                if (returnObject.installedSensors.DISTANCE_COUNTER == YESString)
                    plfRecord.DISTANCE_COUNTER = splittedString[6];

                if (returnObject.installedSensors.FUEL_CONSUMPTION == YESString)
                    plfRecord.FUEL_CONSUMPTION = splittedString[7];

                if (returnObject.installedSensors.ENGINE_RPM == YESString)
                    plfRecord.ENGINE_RPM = splittedString[8];

                if (returnObject.installedSensors.VOLTAGE == YESString)
                    plfRecord.VOLTAGE = splittedString[9];

                if (returnObject.installedSensors.LATITUDE == YESString)
                    plfRecord.LATITUDE = splittedString[10];

                if (returnObject.installedSensors.LONGITUDE == YESString)
                    plfRecord.LONGITUDE = splittedString[11];

                if (returnObject.installedSensors.ALTITUDE == YESString)
                    plfRecord.ALTITUDE = splittedString[12];

                if (returnObject.installedSensors.TEMPERATURE1 == YESString)
                    plfRecord.TEMPERATURE1 = splittedString[13];

                if (returnObject.installedSensors.TEMPERATURE2 == YESString)
                    plfRecord.TEMPERATURE2 = splittedString[14];

                if (returnObject.installedSensors.WEIGHT1 == YESString)
                    plfRecord.WEIGHT1 = splittedString[15];

                if (returnObject.installedSensors.WEIGHT2 == YESString)
                    plfRecord.WEIGHT2 = splittedString[16];

                if (returnObject.installedSensors.WEIGHT3 == YESString)
                    plfRecord.WEIGHT3 = splittedString[17];

                if (returnObject.installedSensors.WEIGHT4 == YESString)
                    plfRecord.WEIGHT4 = splittedString[18];

                if (returnObject.installedSensors.WEIGHT5 == YESString)
                    plfRecord.WEIGHT5 = splittedString[19];

                if (returnObject.installedSensors.MAIN_STATES == YESString)
                    plfRecord.MAIN_STATES = splittedString[20];

                if (returnObject.installedSensors.ADDITIONAL_SENSORS == YESString)
                    plfRecord.ADDITIONAL_SENSORS = splittedString[21];

                if (returnObject.installedSensors.RESERVED_3 == YESString)
                    plfRecord.RESERVED_3 = splittedString[22];

                if (returnObject.installedSensors.RESERVED_4 == YESString)
                    plfRecord.RESERVED_4 = splittedString[23];

                if (returnObject.installedSensors.RESERVED_5 == YESString)
                    plfRecord.RESERVED_5 = splittedString[24];
                #endregion

                returnObject.Records.Add(plfRecord);
            }
            return returnObject;
        }
        /// <summary>
        /// метод удаляет комментарии в PLF файле, чтобы парсер их игнорировал
        /// </summary>
        /// <param name="str">Одна строка из PLF файла</param>
        /// <returns>строка без комментариев</returns>
        private static string DeleteComments(string str)
        {
            string[] separator = {"\r\n"};
            string[] strings = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            string returnString = "";

            foreach (string s in strings)
            {
                if (s[0] != '/' && s[1] != '/')
                {
                    returnString += s + "\r\n";
                }
            }

            return returnString;
        }
    }
}
