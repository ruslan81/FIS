using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    class CardPublicKey : PublicKey
    {
        public CardPublicKey()
            : base()
        { }

        public CardPublicKey(byte[] value) :
            base(value)
        { }
    }
}
