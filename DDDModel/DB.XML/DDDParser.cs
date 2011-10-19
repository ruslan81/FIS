using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VehichleUnit;
using CardUnit;
using PLFUnit;

namespace PARSER
{
    /// <summary>
    /// Класс парсера. В него включены все парсеры, когда подаем файл на вход, сам определяет тип файла.
    /// </summary>
    public class DDDParser
    {
        /// <summary>
        /// имя файла
        /// </summary>
        private string fileName;
        /// <summary>
        /// массив байт - обьект для разбора
        /// </summary>
        private byte[] bytes;
        /// <summary>
        /// тип файла
        /// </summary>
        private int srcType { get; set; }// 0 - card, 1 - vehicle, 2 - PLF, -1 - exception
        /// <summary>
        /// обьект типа VehicleUnitClass - саздается если разбираемый файл - ДДД ТС 
        /// </summary>
        public VehicleUnitClass vehicleUnitClass { get; set; }
        /// <summary>
        /// обьект типа CardUnitClass - саздается если разбираемый файл - ДДД карты
        /// </summary>
        public CardUnitClass cardUnitClass { get; set; }
        /// <summary>
        /// обьект типа PLFUnitClass - саздается если разбираемый файл - PLF
        /// </summary>
        public PLFUnitClass plfUnitClass { get; set; }
        /// <summary>
        /// Получить тип разбираемого обьекта
        /// </summary>
        /// <returns>тип разбираемого обьекта</returns>
        public int GetCardType()
        {
            return srcType;
        }
        /// <summary>
        /// Разбирает файл
        /// </summary>
        /// <param name="dddBytes">обьект для разбора</param>
        /// <param name="fileNameTmp">Имя файла</param>
        /// <returns>Дебаг информация или для лога.</returns>
        public string ParseFile(byte[] dddBytes, string fileNameTmp)
        {
            byte[] twoLetters = new byte[2];
            bytes = dddBytes;
            fileName = fileNameTmp;

            string[] splitStr = fileName.Split(new char[] { '.' });
            if (splitStr[splitStr.Length - 1].ToLower() == "plf")
            {
                srcType = 2;
            }
            else
            {
                twoLetters = HexBytes.arrayCopy(bytes, 0, 2);
                srcType = checkWhatCardIsIt(twoLetters);
            }
            return ParseIt();
        }
        /// <summary>
        /// Разбирает файл.
        /// </summary>
        /// <param name="filename">путь к файлу</param>
        /// <returns>Дебаг информация или для лога.</returns>
        public string ParseFile(string filename)
        {
            byte[] twoLetters = new byte[2];
            fileName = filename;
            bytes = File.ReadAllBytes(filename);

            string[] splitStr = fileName.Split(new char[] {'.'});
            if (splitStr[splitStr.Length - 1].ToLower() == "plf")
            {
                srcType = 2;
            }
            else
            {
                twoLetters = HexBytes.arrayCopy(bytes, 0, 2);
                srcType = checkWhatCardIsIt(twoLetters);
            }
            return ParseIt();
        }
        /// <summary>
        /// Разбирает обьект, вызывается методами ParseFile
        /// </summary>
        /// <returns>результат действия</returns>
        private string ParseIt()
        {
            try
            {
                switch (srcType)
                {
                    case 0: // SRC_TYPE_CARD
                        {
                            C_DriversParser c_driversParser = new C_DriversParser();                            
                            cardUnitClass = c_driversParser.CardUnitData_Parse(bytes);
                            cardUnitClass.cardType = srcType;
                        }
                        break;
                    case 1:// SRC_TYPE_VU
                        {
                            M_VehicleUnitParser m_vehicleUnitParser = new M_VehicleUnitParser();
                            vehicleUnitClass = m_vehicleUnitParser.VehicleUnitData_Parse(bytes);
                            vehicleUnitClass.cardType = srcType;
                        }
                        break;
                    case 2:// SRC_TYPE_PLF
                        {
                            plfUnitClass = PLF_Parser.PLFUnitData_Parse(bytes);
                            plfUnitClass.cardType = srcType;
                        } break;
                    case 3://wrong SRC
                        {
                            return "Error! Wrong file Format!\r\n";
                        }                        
                    default://default
                        {
                            return "Error! Wrong file Format!-default\r\n";
                        }
                }

                return "successfully!\r\n\r\n";
            }
            catch (Exception ex)
            {
                throw ex;
                return "unsuccessfully \r\n\r\n" + ex;
            }
        }
        /// <summary>
        /// Устанавливает тип файла для разбора
        /// </summary>
        /// <param name="twoLetters">первых два байта файла</param>
        /// <returns></returns>
        public int checkWhatCardIsIt (byte[] twoLetters)
        {           
            // Файл начинаеться с EF_ICC(0x00, 0x02) или имя начинаеться с C_
            if (HexBytes.CompareByteArrays(twoLetters, new byte[] { 0x00, 0x02 }))
            {
                srcType = 0;
                return srcType;// SRC_TYPE_CARD
            }
            // файл начинаеться с SID/TREP 0x76/0x03 или с М_
            else if (HexBytes.CompareByteArrays(twoLetters, new byte[] { 0x76, 0x01 }))
            {
                srcType = 1;
                return srcType;// SRC_TYPE_VU
            }
            else
            {
                srcType = -1;
                return srcType;//EXCEPTION
            }
        }
        /// <summary>
        /// Генерирует XML файл разобранного файла
        /// </summary>
        /// <param name="output">путь для сохранения файла</param>
        /// <returns>результат.</returns>
        public string GenerateXmlFile(string output)
        {
            string xmlName = " ";
            switch (srcType)
            {
                case 0: // SRC_TYPE_CARD
                    {
                        xmlName = cardUnitClass.SerializeCardUnitClass(output);
                        return "successfully!  " + xmlName + "\r\n\r\n";
                    }
                case 1:// SRC_TYPE_VU
                    {
                        xmlName = vehicleUnitClass.SerializeVehicleClass(output);
                        return "successfully!  " + xmlName + "\r\n\r\n";
                    }
                case 2:// SRC_TYPE_PLF
                    {
                        xmlName = plfUnitClass.SerializePLFUnitClass(output);
                        return "successfully!  " + xmlName + "\r\n\r\n";
                    }
                case -1://wrong SRC
                    {
                        xmlName = "Error! Empty Object!\r\n";
                    }
                    break;
                default://default
                    {
                        xmlName = "Error! ....unsuccessfully.... -default\r\n";
                    }
                    break;
            }
            return xmlName;
        }
    }
}
