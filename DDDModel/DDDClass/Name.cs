using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
   // [Serializable]
    public class Name//36bytes
    {
        public short codePage { get; set; }
        public byte[] name { get; set; }

       
        private void setName(byte[] value)
        { name = ConvertionClass.arrayCopy(value, 0, 35); }

        public Name()
        {
            codePage = 0;
            name = new byte[35];
        }

        public Name(byte[] value)
        {
            byte codePageTmp = value[0];
            byte[] nameTmp = ConvertionClass.arrayCopy(value, 1, value.Length - 1);

            codePage = ConvertionClass.convertIntoUnsigned1ByteInt(codePageTmp);
            setName(nameTmp);
        }

        public Name(byte codePageTmp, string name)
        {
            codePage = ConvertionClass.convertIntoUnsigned1ByteInt(codePageTmp);
            setName(name);
        }

        public void setCodePage(short codePage)
        {
            this.codePage = codePage;
        }

        public void setCodePage(byte codePage)
        {
            this.codePage = (short)(codePage & 0xff);
        }

        public void setName(string nameTmp)
        {
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] _bytes;

            _bytes = enc.GetBytes(nameTmp);
            name = _bytes;
        }

        public override string ToString()
        {
            return ConvertionClass.convertIntoString(name).Trim();
        }
     
    }
}
