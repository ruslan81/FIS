using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// класс представляет текущую дату. Образован от класса TimeReal(как и все классы описывающие время)
    /// </summary>
    public class CurrentDateTime : TimeReal//4байта
    {
        public CurrentDateTime()
            : base()
        {
        }

        public CurrentDateTime(long i)
            : base(i)
        {
        }

        public CurrentDateTime(byte[] value)
            : base(value)
        {
        }      
    }
}
