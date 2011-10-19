using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDClass;

namespace VehichleUnit
{
    using HexBytes = ConvertionClass;
    /// <summary>
    /// События и Нарушения ТС
    /// </summary>
    public class Vehicle_Events_And_Faults
    {
        private readonly int structureSize;

        public VuFaultData vuFaultData { get; set; }
        public VuEventData vuEventData { get; set; }
        public VuOverSpeedingControlData vuOverSpeedingControlData { get; set; }
        public VuOverSpeedingEventData vuOverSpeedingEventData { get; set; }
        public VuTimeAdjustmentData vuTimeAdjustmentData { get; set; }

        public Vehicle_Events_And_Faults()
        {
            vuFaultData = new VuFaultData();
            vuEventData = new VuEventData();
            vuOverSpeedingControlData = new VuOverSpeedingControlData();
            vuOverSpeedingEventData = new VuOverSpeedingEventData();
            vuTimeAdjustmentData = new VuTimeAdjustmentData();
        }

        public Vehicle_Events_And_Faults(byte[] value)
        {
            int offset1 = 1 + HexBytes.convertIntoUnsigned1ByteInt(value[0]) * VuFaultRecord.structureSize;
            vuFaultData = new VuFaultData(HexBytes.arrayCopy(value, 0, offset1));

            int offset2 = 1 + HexBytes.convertIntoUnsigned1ByteInt(value[offset1]) * VuEventRecord.structureSize;
            vuEventData = new VuEventData(HexBytes.arrayCopy(value, offset1, offset2));

            int offset3 = 9;
            vuOverSpeedingControlData = new VuOverSpeedingControlData(HexBytes.arrayCopy(value, offset1 + offset2, offset3));

            int offset4 = 1 + HexBytes.convertIntoUnsigned1ByteInt(value[offset1 + offset2 + offset3]) * VuOverSpeedingEventRecord.structureSize;
            vuOverSpeedingEventData = new VuOverSpeedingEventData(HexBytes.arrayCopy(value, offset1 + offset2 + offset3, offset4));

            int offset5 = 1 + HexBytes.convertIntoUnsigned1ByteInt(value[offset1 + offset2 + offset3 + offset4]) * VuTimeAdjustmentRecord.structureSize;
            vuTimeAdjustmentData = new VuTimeAdjustmentData(HexBytes.arrayCopy(value, offset1 + offset2 + offset3 + offset4, offset5));

            structureSize = offset1 + offset2 + offset3 + offset4 + offset5;
        }
    }
}
