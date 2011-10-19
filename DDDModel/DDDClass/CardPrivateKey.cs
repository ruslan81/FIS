using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    class CardPrivateKey : RSAKeyPrivateExponent
    {
        public CardPrivateKey()
            : base()
        { }

        public CardPrivateKey(byte[] value)
            : base(value)
        { }
    }
}
