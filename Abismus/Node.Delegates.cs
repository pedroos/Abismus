using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abismus.Node
{
    public static class Dels
    {
        public delegate TO1 O<TO1>();
        public delegate TO1 IO<TI1, TO1>(TI1 @in);
        public delegate TO1 IIO<TI1, TI2, TO1>((TI1, TI2) @in);
        public delegate TO1 IIIO<TI1, TI2, TI3, TO1>((TI1, TI2, TI3) @in);
        public delegate (TO1, TO2) OO<TO1, TO2>();
        public delegate (TO1, TO2) IOO<TI1, TO1, TO2>(TI1 @in);
        public delegate (TO1, TO2) IIOO<TI1, TI2, TO1, TO2>((TI1, TI2) @in);
    }
}
