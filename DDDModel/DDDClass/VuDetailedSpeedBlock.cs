using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// скоростной блок. Описывает скорость за одну минуту(60 секунд)
    /// </summary>
    public class VuDetailedSpeedBlock//64byte
    {
        public readonly static int structureSize = 64;
        /// <summary>
        /// дата и время начада скоростного блока. 
        /// После этой даты каждую секунду идет запись в этот блок(в переменную Speed[] speedsPerSecond)
        /// скорости(всего 60 записей)
        /// </summary>
        public TimeReal speedBlockBeginDate { get; set; }
        /// <summary>
        /// массив из 60 записей со скоростью(за каждую секунду)
        /// </summary>
        public Speed[] speedsPerSecond { get; set; }

        public VuDetailedSpeedBlock()
        {
            speedBlockBeginDate = new TimeReal();
            speedsPerSecond = new Speed[60];
        }

        public VuDetailedSpeedBlock(byte[] value)
        {
            speedBlockBeginDate = new TimeReal(ConvertionClass.arrayCopy(value, 0, 4));
            speedsPerSecond = new Speed[60];

            for (int i = 0; i < 60; i++)
            {
                speedsPerSecond[i] = new Speed(value[4 + i]);
            }
        }

    }
}
