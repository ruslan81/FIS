using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class TDesSessionKey//16 byte
    {
        public readonly static int structureSize = 16;

        public byte[] tDesKeyA { get; set; }
        public byte[] tDesKeyB { get; set; }

        public void AddTDesSessionKey(byte[] value)
        {
            tDesKeyA = ConvertionClass.arrayCopy(value, 0, 8);
            tDesKeyB = ConvertionClass.arrayCopy(value, 8, 8);
        }

        public TDesSessionKey()
        {
            tDesKeyA = new byte[8];
            tDesKeyB = new byte[8];
        }

        public TDesSessionKey(byte[] value)
        {
            tDesKeyA = ConvertionClass.arrayCopy(value, 0, 8);
            tDesKeyB = ConvertionClass.arrayCopy(value, 8, 8);
        }
    }
}
