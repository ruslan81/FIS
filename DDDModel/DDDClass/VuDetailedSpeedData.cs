using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    /// <summary>
    /// Вся скорость ТС, записанная на карточку(каждую секунду)
    /// </summary>
    public class VuDetailedSpeedData
    {
        public int size { get; set; }
        /// <summary>
        /// количество скоростных блоков
        /// </summary>
        public int noOfSpeedBlocks { get; set; }
        /// <summary>
        /// массив скоростных блоков
        /// </summary>
        public List<VuDetailedSpeedBlock> vuDetailedSpeedBlocks { get; set; }

        public VuDetailedSpeedData()
        {
            vuDetailedSpeedBlocks = new List<VuDetailedSpeedBlock>();
        }

        public VuDetailedSpeedData(byte[] value)
        {
            vuDetailedSpeedBlocks = new List<VuDetailedSpeedBlock>();
       
            noOfSpeedBlocks = ConvertionClass.convertIntoUnsigned2ByteInt(ConvertionClass.arrayCopy(value, 0, 2));
            size = 2 + noOfSpeedBlocks * VuDetailedSpeedBlock.structureSize;

            if (noOfSpeedBlocks != 0)
            {
                for (int i = 0; i < noOfSpeedBlocks; i++)
                {
                    byte[] record = ConvertionClass.arrayCopy(value, 2 + (i * VuDetailedSpeedBlock.structureSize), VuDetailedSpeedBlock.structureSize);
                    VuDetailedSpeedBlock vdsb = new VuDetailedSpeedBlock(record);
                    vuDetailedSpeedBlocks.Add(vdsb);
                }
            }
        }
    }
}
