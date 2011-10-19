using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    /// <summary>
    /// – используется в процессе добавления разобранных данных в базу данных. 
    /// Представляет собой одну запись для добавления.
    /// </summary>
    public class ReflectionClass
    {
        /// <summary>
        /// имя параметра
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// структура параметра
        /// </summary>
        public List<string> paramStructure { get; set; }
        /// <summary>
        /// ID параметра
        /// </summary>
        public int PARAM_ID { get; set; }
        /// <summary>
        /// ID параметра предка
        /// </summary>
        public int PARENT_PARAM_ID { get; set; }
        /// <summary>
        /// Значение
        /// </summary>
        public string value { get; set; }

        public ReflectionClass()
        {
            name = "";
            PARAM_ID = -1;
            PARENT_PARAM_ID = 0;
            value = "";
            paramStructure = new List<string>();
        }

        public ReflectionClass(string PName, string PValue)
        {
            name = PName;
            PARAM_ID = -1;
            value = PValue;
            paramStructure = new List<string>();
        }
        /// <summary>
        /// получает имя параметра предка
        /// </summary>
        /// <returns>имя параметра предка</returns>
        public string GetParentParamName()
        {
            string[] nameWords;
            string parentParamName = "";
            nameWords = name.Split(new char[] { '.' });
            for (int i = 0; i < nameWords.Length - 1; i++)
            {
                if(i!=0)
                    parentParamName += ".";
                parentParamName += nameWords[i];
            }            
            return parentParamName;
        }

        public override string ToString()
        {
            return (PARAM_ID + "-" + PARENT_PARAM_ID + "  " + name + "  " + value).ToString();
        }
    }   
}
