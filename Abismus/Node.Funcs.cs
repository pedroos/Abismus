using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abismus.Node
{
    public static class Funcs
    {
        public static int Mult((int, int) @in)
        {
            return @in.Item1 * @in.Item2;
        }

        public static int Mirror(int @in)
        {
            return @in;
        }


        public static int Fixed()
        {
            return 5;
        }

        public static (int, int) Duplicate(int @in)
        {
            return (@in, @in);
        }
    }
}
