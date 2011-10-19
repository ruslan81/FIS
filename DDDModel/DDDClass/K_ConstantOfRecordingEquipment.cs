using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class K_ConstantOfRecordingEquipment//2bytes
    {
        public int kConstantOfRecordingEquipment { get; set; }

        public K_ConstantOfRecordingEquipment()
        {
            kConstantOfRecordingEquipment = 0;
        }
        
        public K_ConstantOfRecordingEquipment(byte[] value)
        {
            kConstantOfRecordingEquipment = ConvertionClass.convertIntoUnsigned2ByteInt(value);
        }

    }
}
