using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// Данные о проведенном контроле.
    /// </summary>
    public class EF_Control_Activity_Data
    {
        private readonly static int structureSize = CardControlActivityDataRecord.structureSize;

        public CardControlActivityDataRecord cardControlActivityDataRecord { get; set; }

        public EF_Control_Activity_Data()
        {
            cardControlActivityDataRecord = new CardControlActivityDataRecord();
        }

        public EF_Control_Activity_Data(byte[] value)
        {
            cardControlActivityDataRecord = new CardControlActivityDataRecord(value);
        }
    }
}
