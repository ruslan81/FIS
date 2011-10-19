using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace CardUnit
{
    /// <summary>
    /// активность водителя
    /// </summary>
    public class EF_Driver_Activity_Data
    {
        public CardDriverActivity cardDriverActivity { get; set; }

        public EF_Driver_Activity_Data()
        {
            cardDriverActivity = new CardDriverActivity();
        }

        public EF_Driver_Activity_Data(byte[] value, int activityStructureLength)
        {
            cardDriverActivity = new CardDriverActivity(value, activityStructureLength);
        }
    }
}
