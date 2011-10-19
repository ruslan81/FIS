using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    public class CardStructureVersion//2bytes  ///Обратить внимание на укладывание в базу и выборку. тут чет мутновато получилось, а все из примера карты значения 0 и 0...
    {//Возможно стоит в БСД конвертировать и обратно..
        public string cardStructureVersion { get; set; }

        public CardStructureVersion()
        {
            cardStructureVersion = "";
        }

        public CardStructureVersion(byte[] value)
        {
            byte[] _bytes = ConvertionClass.arrayCopy(value, 0, 2);

            cardStructureVersion = _bytes[0].ToString() + "&&" + _bytes[1].ToString();
        }

        public CardStructureVersion(string specialString)
        {
            cardStructureVersion = specialString;
        }

        public byte[] Get_CardStructureVersion_Bytes()
        {
            byte[] _bytes = new byte[2];
            string[] strArray;

            strArray = cardStructureVersion.Split(new string[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
            _bytes[0] = Convert.ToByte(strArray[0]);
            _bytes[1] = Convert.ToByte(strArray[1]);

            return _bytes;
        }

        public override string ToString()
        {
            return ConvertionClass.convertIntoUnsigned2ByteInt(Get_CardStructureVersion_Bytes()).ToString();
        }
    }
}
