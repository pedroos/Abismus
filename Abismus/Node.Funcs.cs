using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abismus.Node
{
    public static class Funcs
    {
        public static int Mult((int a, int b) @in)
        {
            var (a, b) = @in;
            return a * b;
        }

        public static int Mirror(int a)
        {
            return a;
        }

        public static int Fixed()
        {
            return 5;
        }

        public static (int b, int c) Duplicate(int a)
        {
            return (a, a);
        }

        public static (int b, int c) Negate(int a)
        {
            return (-a, a);
        }
    }
}
