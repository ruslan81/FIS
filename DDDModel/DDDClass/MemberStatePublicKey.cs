using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDClass
{
    class MemberStatePublicKey : PublicKey
    {
        public MemberStatePublicKey()
            : base()
        {
        }

        public MemberStatePublicKey(byte[] value)
            : base(value)
        {
        }
    }
}
