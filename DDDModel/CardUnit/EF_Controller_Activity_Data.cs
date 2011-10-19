using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// Описывает активность контролируещего(когда карта контроллера)
    /// </summary>
    public class EF_Controller_Activity_Data
    {
        private readonly int structureSize;

        public ControlCardControlActivityData controlCardControlActivityData { get; set; }

        public EF_Controller_Activity_Data()
        { }

        public EF_Controller_Activity_Data(byte[] value, int noOfControlActivityRecords)
        {
            controlCardControlActivityData = new ControlCardControlActivityData(value, noOfControlActivityRecords);
            structureSize = controlCardControlActivityData.structureSize;
        }
    }
}
