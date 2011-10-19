using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BLL
{
    /// <summary>
    /// Содержит список типа ReflectionClass, и имеет методы для работы с этим списком.
    /// </summary>
    public class ReflectObjectToTableClass
    {
        private int currentId = 0;
        public List<ReflectionClass> reflectedItemsList { get; set; }

        public ReflectObjectToTableClass()
        {
            reflectedItemsList = new List<ReflectionClass>();
        }

        public List<ReflectionClass> ReflectObjectToTable(String name, Object obj)
        {
            reflectedItemsList = new List<ReflectionClass>();

            PerfectwORK(name, obj, 0);

            return reflectedItemsList;
        }

        public List<int> getAllItemsLevels()
        {
            List<int> returnLevels = new List<int>();
            bool allreadyExists = false;

            foreach (ReflectionClass r in reflectedItemsList)
            {
                int count = r.paramStructure.Count;
                foreach (int level in returnLevels)
                {
                    if (level == count)
                    {
                        allreadyExists = true;
                    }
                }
                if (allreadyExists == false)
                {
                    returnLevels.Add(count);
                }
                allreadyExists = false;
            }
            returnLevels.Sort();
            return returnLevels;
        }

        public IEnumerable<ReflectionClass> GetItemsByLevel(int level)
        {
            IEnumerable<ReflectionClass> returnItems = from number in reflectedItemsList where number.paramStructure.Count == level select number;
            return returnItems;
        }

        private void PerfectwORK(String name, Object obj, int parentParamId)
        {
            string s = "";
            byte[] b = new byte[0];

            if (obj == null)
            {
                /*ReflectionClass rf = new ReflectionClass(name, "NOTHING");
                rf.PARAM_ID = ++currentId;
                rf.PARENT_PARAM_ID = parentParamId;
                reflectedItemsList.Add(rf);*/
            }
            else
            {
                Type type = obj.GetType();

                if (type.IsPrimitive || type.IsInstanceOfType(s))
                {
                    ReflectionClass rf = new ReflectionClass(name, obj.ToString());
                    rf.PARAM_ID = ++currentId;
                    rf.PARENT_PARAM_ID = parentParamId;
                    reflectedItemsList.Add(rf);
                }
                else if (type.IsInstanceOfType(b))//byte[]
                {
                    ReflectionClass rf = new ReflectionClass(name, convertIntoString((byte[])obj).Trim());
                    rf.PARAM_ID = ++currentId;
                    rf.PARENT_PARAM_ID = parentParamId;
                    reflectedItemsList.Add(rf);
                }
                else if (type.IsArray)//array[]
                {
                    IList list = (IList)obj;

                    ReflectionClass parentItem = new ReflectionClass(name, "It's parent array");
                    parentItem.PARAM_ID = ++currentId;
                    parentItem.PARENT_PARAM_ID = parentParamId;

                  //  reflectedItemsList.Add(parentItem);

                    for (int idx = 0; idx < list.Count; idx++)
                    {
                        Object objectTemp = list[idx];
                        if (objectTemp != null)
                            PerfectwORK(name, objectTemp, parentItem.PARAM_ID);
                    }
                }
                else if (type.IsGenericType)
                {
                    IList list = (IList)obj;
                        ReflectionClass parentItem = new ReflectionClass(name, "It's parent array");
                        parentItem.PARAM_ID = ++currentId;
                        parentItem.PARENT_PARAM_ID = parentParamId;

                      //  reflectedItemsList.Add(parentItem);

                        for (int idx = 0; idx < list.Count; idx++)
                        {
                            Object objectTemp = list[idx];
                            if (objectTemp != null)
                                PerfectwORK(name, objectTemp, parentItem.PARAM_ID);
                        }
                }

                else if (type.IsClass)
                {                    
                    PropertyInfo[] propertyInfo = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo.Length == 1)
                    {
                        object field2 = propertyInfo[0].GetValue(obj, null);

                        PerfectwORK(name, field2, parentParamId);
                    }
                    else
                    {

                        ReflectionClass parentItem = new ReflectionClass(name, "It's parent");
                        parentItem.PARAM_ID = ++currentId;
                        parentItem.PARENT_PARAM_ID = parentParamId;

                        reflectedItemsList.Add(parentItem);

                        foreach (PropertyInfo pi in propertyInfo)
                        {
                            string fieldName = name + "." + pi.Name;
                            object field2 = pi.GetValue(obj, null);
                            PerfectwORK(fieldName, field2, parentItem.PARAM_ID);
                        }
                    }
                }
            }
        }

        static public string convertIntoString(byte[] b)
        {
            System.Text.Encoding enc = System.Text.Encoding.ASCII;
            string myString = enc.GetString(b);

            return myString;
        }

        static public string convertIntoString(byte b)
        {
            return convertIntoString(new byte[] { b });
        }
    }
}
