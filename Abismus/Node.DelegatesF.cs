using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Abismus.Node
{
    public static class DelsF
    {
        public delegate void O<TO1>(out TO1 out1);
        public delegate void IO<TI1, TO1>(TI1 in1, out TO1 out1);
        public delegate void IIO<TI1, TI2, TO1>(TI1 in1, TI2 in2, out TO1 out1);
        public delegate void OO<TO1, TO2>(out TO1 out1, out TO2 out2);
        public delegate void IOO<TI1, TO1, TO2>(TI1 in1, out TO1 out1, out TO2 out2);
        public delegate void IIOO<TI1, TI2, TO1, TO2>(TI1 in1, TI2 in2, out TO1 out1, out TO2 out2);

        public static O<TO1> Curry<TI1, TO1>(IO<TI1, TO1> f, TI1 in1) => 
            (out TO1 out1) => f(in1, out out1);

        public static O<TO1> Curry<TI1, TI2, TO1>(IIO<TI1, TI2, TO1> f, TI1 in1, TI2 in2) =>
            (out TO1 out1) => f(in1, in2, out out1);

        public static IO<TI2, TO1> Curry<TI1, TI2, TO1>(IIO<TI1, TI2, TO1> f, TI1 in1) => 
            (TI2 in2, out TO1 out1) => f(in1, in2, out out1);

        public static IO<TI1, TO1> CurryI2<TI1, TI2, TO1>(IIO<TI1, TI2, TO1> f, TI2 in2) =>
            (TI1 in1, out TO1 out1) => f(in1, in2, out out1);

        public static OO<TO1, TO2> Curry<TI1, TO1, TO2>(IOO<TI1, TO1, TO2> f, TI1 in1) => 
            (out TO1 out1, out TO2 out2) => f(in1, out out1, out out2);

        public static OO<TO1, TO2> Curry<TI1, TI2, TO1, TO2>(IIOO<TI1, TI2, TO1, TO2> f, TI1 in1, TI2 in2) =>
            (out TO1 out1, out TO2 out2) => f(in1, in2, out out1, out out2);

        public static IOO<TI2, TO1, TO2> Curry<TI1, TI2, TO1, TO2>(IIOO<TI1, TI2, TO1, TO2> f, TI1 in1) =>
            (TI2 in2, out TO1 out1, out TO2 out2) => f(in1, in2, out out1, out out2);

        public static IOO<TI1, TO1, TO2> CurryI2<TI1, TI2, TO1, TO2>(IIOO<TI1, TI2, TO1, TO2> f, TI2 in2) =>
            (TI1 in1, out TO1 out1, out TO2 out2) => f(in1, in2, out out1, out out2);
    }
}
