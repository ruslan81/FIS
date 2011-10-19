using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLFUnit
{
    /// <summary>
    /// Описывает одну заправку или слив топлива.
    /// </summary>
    public class PLFRefillClass
    {
        /// <summary>
        /// начало слива/заправки
        /// </summary>
        public DateTime timeStart { get; set; }
        /// <summary>
        /// окончание слива/заправки
        /// </summary>
        public DateTime timeEnd { get; set; }
        /// <summary>
        /// Количество топлива на начало слива/заправки
        /// </summary>
        public double capacityStart { get; set; }
        /// <summary>
        /// Количество топлива на окончание слива/заправки
        /// </summary>
        public double capacityEnd { get; set; }
        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public PLFRefillClass()
        {
            timeStart = new DateTime();
            timeEnd = new DateTime();
            capacityStart = 0;
            capacityEnd = 0;
        }
    }
}
