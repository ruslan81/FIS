using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Security;

namespace PARSER
{
    /// <summary>
    /// класс конвертирует байты и байтовые массивы в различные типы.
    /// </summary>
    public abstract class HexBytes
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
        /// byte[] convert Into Hex String
        /// </summary>
        /// <param name="b">byte[] b</param>
        /// <returns>string</returns>
        static public string convertIntoHexString(byte[] b)
        {
            char[] digits = {
			'0' , '1' , '2' , '3' , '4' , '5' , '6' , '7' , '8' , '9' ,
			'a' , 'b' , 'c' , 'd' , 'e' , 'f'   
		};

            string str = new string("".ToCharArray());

            for (int ptr = 0; ptr < b.Length; ptr++)
            {
                int i = b[ptr];
                char[] buf = new char[2];
                int charPos = 2;
                int radix = 1 << 4; // 1000(b)
                int mask = radix - 1; // 0111(b)
                for (int c = 0; c < 2; c++)
                {
                    buf[--charPos] = digits[i & mask];
                    i >>= 4;
                }

                str += new String(buf);
            }
            return str;
        }

        /// <summary>
        /// byte convert Into Hex String
        /// </summary>
        /// <param name="b">byte</param>
        /// <returns>string</returns>
        static public string convertIntoHexString(byte b)
        {
            return convertIntoHexString(new byte[] { b });
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

        //Compare Two Objects Properties
        /// <summary>
        /// Сравнивает Property двух любых обьектов.
        /// </summary>
        /// <param name="left">первый обьект</param>
        /// <param name="right">второй обьект</param>
        /// <returns>bool - true если все равны, false, если нет</returns>
        public static bool MemberCompare(object left, object right)
        {
            if (Object.ReferenceEquals(left, right))
                return true;

            if (left == null || right == null)
                return false;

            Type type = left.GetType();
            if (type != right.GetType())
                return false;

            if (left as ValueType != null)
            {
                // do a field comparison, or use the override if Equals is implemented:
                return left.Equals(right);
            }

            // check for override:
            if (type != typeof(object)
                && type == type.GetMethod("Equals").DeclaringType)
            {
                // the Equals method is overridden, use it:
                return left.Equals(right);
            }

            // all Arrays, Lists, IEnumerable<> etc implement IEnumerable
            if (left as IEnumerable != null)
            {
                IEnumerator rightEnumerator = (right as IEnumerable).GetEnumerator();
                rightEnumerator.Reset();
                foreach (object leftItem in left as IEnumerable)
                {
                    // unequal amount of items
                    if (!rightEnumerator.MoveNext())
                        return false;
                    else
                    {
                        if (!MemberCompare(leftItem, rightEnumerator.Current))
                            return false;
                    }
                }
            }
            else
            {
                // compare each property
                foreach (PropertyInfo info in type.GetProperties(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.GetProperty))
                {
                    // TODO: need to special-case indexable properties
                    if (!MemberCompare(info.GetValue(left, null), info.GetValue(right, null)))
                        return false;
                }

                // compare each field
                foreach (FieldInfo info in type.GetFields(
                    BindingFlags.GetField |
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.Instance))
                {
                    if (!MemberCompare(info.GetValue(left), info.GetValue(right)))
                        return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Конвертирует массив байт в строку
        /// </summary>
        /// <param name="b">byte[] b</param>
        /// <returns>string</returns>
        static public string convertIntoString(byte[] b)
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            string myString = enc.GetString(b);

            return myString;          
        }
        /// <summary>
        /// Конвертирует один байт в строку
        /// </summary>
        /// <param name="b">byte b</param>
        /// <returns>string</returns>
        static public string convertIntoString(byte b)
        {
            return convertIntoString(new byte[] { b });
        }
    }
}
