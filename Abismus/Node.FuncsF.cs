using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Abismus.Node
{
    public static class FuncsF
    {
        public static void Mirror(int a, out int b) => b = a;

        public static void Fixed(out int a) => a = 5;

        public static void Minus(int a, int b, out int c) => c = a - b;

        public static void Mult(int a, int b, out int c) => c = a * b;

        public static void Mult(int a, float b, out float c) => c = a * b;

        public static void Duplicate(int a, out int b, out int c)
        {
            b = a;
            c = a;
        }

        public static void Negate(int a, out int b, out int c)
        {
            b = -a;
            c = a;
        }
    }
}
