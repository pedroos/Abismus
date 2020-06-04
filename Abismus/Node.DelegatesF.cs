using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abismus.Node
{
    public static class DelsF
    {
        public delegate void O<T>(out T out1);
        public delegate void OFromO<T>(T in1, out T out1);
        public delegate void OFromOO<T>(T in1, T in2, out T out1);
        public delegate void OO<T>(out T out1, out T out2);
        public delegate void OOFromO<T>(T in1, out T out1, out T out2);
        public delegate void OOFromOO<T>(T in1, T in2, out T out1, out T out2);
    }
}
