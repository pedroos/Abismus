using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abismus.Node
{
    using Abismus.Graph;
    public static class ExecutorF
    {
        public static object[] GetFinalValueF(this IEnumerable<Edge<Node>> tree)
        {
            var treeEn = tree.GetEnumerator();

            T CheckPcval<T>(object previousCurrentValue)
            {
                if (previousCurrentValue == default)
                    throw new NoCurrentValueException();
                if (!(previousCurrentValue is T))
                    throw new WrongTypeCurrentValueException();
                return (T)previousCurrentValue;
            }

            object[] currValue = new object[2];

            bool DelCall<T>(object f, ref object[] cval)
            {
                if (f is DelsF.O<T> f1)
                {
                    f1(out T out1);
                    cval[0] = out1;
                    cval[1] = null;
                }
                else if (f is DelsF.OFromO<T> f2)
                {
                    f2(CheckPcval<T>(cval[0]), out T out1);
                    cval[0] = out1;
                    cval[1] = null;
                }
                else if (f is DelsF.OFromOO<T> f3)
                {
                    f3(CheckPcval<T>(cval[0]), CheckPcval<T>(cval[1]), out T out1);
                    cval[0] = out1;
                    cval[1] = null;
                }
                else if (f is DelsF.OO<T> f4)
                {
                    f4(out T out1, out T out2);
                    cval[0] = out1;
                    cval[1] = out2;
                }
                else if (f is DelsF.OOFromO<T> f5)
                {
                    f5(CheckPcval<T>(cval[0]), out T out1, out T out2);
                    cval[0] = out1;
                    cval[1] = out2;
                }
                else if (f is DelsF.OOFromOO<T> f6)
                {
                    f6(CheckPcval<T>(cval[0]), CheckPcval<T>(cval[1]), out T out1, out T out2);
                    cval[0] = out1;
                    cval[1] = out2;
                }
                else
                {
                    return false;
                }
                return true;
            };

            Type previousFunType = default;

            void NodeValue(Node node)
            {
                Type funType = node.Fun.GetType();

                if (!DelCall<int>(node.Fun, ref currValue)) 
                if (!DelCall<float>(node.Fun, ref currValue)) 
                if (!DelCall<double>(node.Fun, ref currValue)) 
                    throw new NotImplementedException();

                previousFunType = funType;
            }

            while (treeEn.MoveNext())
            {
                NodeValue(treeEn.Current.Source);
            }
            NodeValue(treeEn.Current.Sink);

            return currValue;
        }

        delegate TFinal TryConvertDelegate<TFinal>(ref object[] final);

        static TFinal TryConvert<TFinal>(ref object[] final, int positions, TryConvertDelegate<TFinal> @as)
        {
            if (final.Skip(positions).Any(f => f != null))
                throw new WrongTypeFinalValueException(final);
            try
            {
                return @as(ref final);
            }
            catch (InvalidCastException)
            {
                throw new WrongTypeFinalValueException(final);
            }
            catch (IndexOutOfRangeException)
            {
                throw new WrongTypeFinalValueException(final);
            }
        }

        public static T As<T>(this object[] final)
        {
            return TryConvert(ref final, 1, delegate (ref object[] f) {
                return (T)final[0];
            });
        }

        public static ValueTuple<T, T> AsTuple2<T>(this object[] final)
        {
            return TryConvert(ref final, 2, delegate (ref object[] f) {
                return new ValueTuple<T, T>((T)final[0], (T)final[1]); 
            });
        }

        public static ValueTuple<T, T, T> AsTuple3<T>(this object[] final)
        {
            return TryConvert(ref final, 3, delegate (ref object[] f) {
                return new ValueTuple<T, T, T>((T)final[0], (T)final[1], (T)final[2]);
            });
        }
    }
}
