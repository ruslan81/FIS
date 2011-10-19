using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DDDClass
{
    /// <summary>
    /// класс конвертирует байты и байтовые массивы в различные типы. Используется много где в данном пространстве имен.
    /// </summary>
    public class ConvertionClass
    {
        /// <summary>
        /// копирует часть массива байт
        /// </summary>
        /// <param name="value">исходный массив байт</param>
        /// <param name="from">номер индекса, откуда начать копирование</param>
        /// <param name="length">колличество байт, которые надо скопировать</param>
        /// <returns>номый массив, полученный копированием части старого</returns>
        static public byte[] arrayCopy(byte[] value, int from, int length)
        {
            byte[] tmp = new byte[length];
            Array.Copy(value, from, tmp, 0, length);
            return tmp;
        }

        /// <summary>
        ///  byte to short. 
        /// </summary>
        /// <param name="b">byte</param>
        /// <returns>short</returns> 
        static public short convertIntoUnsigned1ByteInt(byte b)
        {
            short s;
            s = (short)(b & 0xff);
            return s;
        }

        /// <summary>
        /// byte[0] to short.
        /// </summary>
        /// <param name="b">byte[]</param>
        /// <returns>short</returns>
        static public short convertIntoUnsigned1ByteInt(byte[] b)
        {
            return convertIntoUnsigned1ByteInt(b[0]);
        }

        /// <summary>
        ///  byte[0]&&byte[1] to int.
        /// </summary>
        /// <param name="b">byte[]</param>
        /// <returns>int</returns>
        static public int convertIntoUnsigned2ByteInt(byte[] b)
        {
            int i = 0;

            i += (b[0] & 0xff) << 8;
            i += (b[1] & 0xff);

            return i;
        }

        /// <summary>
        /// byte[0]&&byte[1]&&byte[2] to int.
        /// </summary>
        /// <param name="b">byte[]</param>
        /// <returns>int</returns> 
        static public int convertIntoUnsigned3ByteInt(byte[] b)
        {
            int i = 0;

            i += (b[0] & 0xff) << 16;
            i += (b[1] & 0xff) << 8;
            i += (b[2] & 0xff);

            return i;
        }

        /// <summary>
        /// byte[0]&&byte[1]&&byte[2]&&byte[3] to long.
        /// </summary>
        /// <param name="b">byte[]</param>
        /// <returns>long</returns> 
        static public long convertIntoUnsigned4ByteInt(byte[] b)
        {
            long l = 0;

            long al = (((long)b[0]) & 0xff) << 24;
            long bl = (((long)b[1]) & 0xff) << 16;
            long cl = (((long)b[2]) & 0xff) << 8;

            int di = (b[3] & 0xff);
            long dl = ((long)di) << 0;

            l = al + bl + cl + dl;

            return l;
        }

        /// <summary>
        /// BCD string to String
        /// </summary>
        /// <param name="b">byte[]</param>
        /// <returns>string</returns>
        static public string convertBCDStringIntoString(byte[] b)
        {
            string tmp = new string("".ToCharArray());

            for (int i = 0; i < b.Length; i++)
            {
                int hNibble, lNibble;
                hNibble = b[i] & 0xf0;
                hNibble = hNibble >> 4;
                hNibble += 0x30;

                lNibble = b[i] & 0x0f;
                lNibble += 0x30;

                if (hNibble > 0x39)
                {
                    hNibble = '_';
                }

                if (lNibble > 0x39)
                {
                    lNibble = '_';
                }

                tmp = tmp + (char)hNibble + (char)lNibble;
            }

            return tmp;
        }
        /// <summary>
        /// byte[] to BCD string
        /// </summary>
        /// <param name="isLittleEndian">isLittleEndian</param>
        /// <param name="bytes">byte[]</param>
        /// <returns>string</returns>
        public static string ConvertBytesToBCDString(bool isLittleEndian, params byte[] bytes)
        {
            StringBuilder bcd = new StringBuilder(bytes.Length * 2);
            if (isLittleEndian)
            {
                for (int i = bytes.Length - 1; i >= 0; i--)
                {
                    byte bcdByte = bytes[i];
                    int idHigh = bcdByte >> 4;
                    int idLow = bcdByte & 0x0F;
                    if (idHigh > 9 || idLow > 9)
                    {
                        throw new ArgumentException(
                            String.Format("One of the argument bytes was not in binary-coded decimal format: byte[{0}] = 0x{1:X2}.",
                            i, bcdByte));
                    }
                    bcd.Append(string.Format("{0}{1}", idHigh, idLow));
                }
            }
            else
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    byte bcdByte = bytes[i];
                    int idHigh = bcdByte >> 4;
                    int idLow = bcdByte & 0x0F;
                    if (idHigh > 9 || idLow > 9)
                    {
                        throw new ArgumentException(
                            String.Format("One of the argument bytes was not in binary-coded decimal format: byte[{0}] = 0x{1:X2}.",
                            i, bcdByte));
                    }
                    bcd.Append(string.Format("{0}{1}", idHigh, idLow));
                }
            }
            return bcd.ToString();
        }
        /// <summary>
        /// BCD to byte[]
        /// </summary>
        /// <param name="isLittleEndian"></param>
        /// <param name="bcdString">bcdString</param>
        /// <returns>isLittleEndian</returns>
        public static byte[] ConvertBCDStringToBytes(bool isLittleEndian, string bcdString)
        {
            bool isValid = true;
            isValid = isValid && !String.IsNullOrEmpty(bcdString);
            // Check that the string is made up of sets of two numbers (e.g. "01" or "3456")
            isValid = isValid && Regex.IsMatch(bcdString, "^([0-9]{2})+$");
            byte[] bytes;
            if (isValid)
            {
                char[] chars = bcdString.ToCharArray();
                int len = chars.Length / 2;
                bytes = new byte[len];
                if (isLittleEndian)
                {
                    for (int i = 0; i < len; i++)
                    {
                        byte highNibble = byte.Parse(chars[2 * (len - 1) - 2 * i].ToString());
                        byte lowNibble = byte.Parse(chars[2 * (len - 1) - 2 * i + 1].ToString());
                        bytes[i] = (byte)((byte)(highNibble << 4) | lowNibble);
                    }
                }
                else
                {
                    for (int i = 0; i < len; i++)
                    {
                        byte highNibble = byte.Parse(chars[2 * i].ToString());
                        byte lowNibble = byte.Parse(chars[2 * i + 1].ToString());
                        bytes[i] = (byte)((byte)(highNibble << 4) | lowNibble);
                    }
                }
            }
            else
            {
                throw new ArgumentException(String.Format(
                    "Input string ({0}) was invalid.", bcdString));
            }
            return bytes;
        }
        
        /// <summary>
        /// Compares one byte[] to another byte[].
        /// </summary>
        /// <param name="array1">byte[] array1</param>
        /// <param name="array2">byte[] array2</param>
        /// <returns>bool</returns>
        static public bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            bool areEqual = array1.SequenceEqual(array2);
            return areEqual;
        }
        /// <summary>
        /// конвертируем byte[] в строку
        /// </summary>
        /// <param name="b">byte[]</param>
        /// <returns>строка</returns>
        static public string convertIntoString(byte[] b)
        {          
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            string myString = enc.GetString(b);

            return myString;        
        }
        /// <summary>
        /// конвертируем один байт в строку
        /// </summary>
        /// <param name="b">byte</param>
        /// <returns>string</returns>
        static public string convertIntoString(byte b)
        {
            return convertIntoString(new byte[] { b });
        }
    }
}
