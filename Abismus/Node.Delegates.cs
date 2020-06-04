using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abismus.Node
{
    public static class Dels
    {
        public delegate T O<T>();
        public delegate T OFromO<T>(T @in);
        public delegate T OFromOO<T>((T, T) @in);
        public delegate (T, T) OO<T>();
        public delegate (T, T) OOFromO<T>(T @in);
        public delegate (T, T) OOFromOO<T>((T, T) @in);
    }
}
