using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abismus.Node
{
    public static class FuncsF
    {
        public static void Mult(int in1, int in2, out int out1)
        {
            out1 = in1 * in2;
        }
        public static void Mirror(int in1, out int out1)
        {
            out1 = in1;
        }

        public static void Fixed(out int out1)
        {
            out1 = 5;
        }

        public static void Duplicate(int in1, out int out1, out int out2)
        {
            out1 = in1;
            out2 = in1;
        }
    }
}
