using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abismus.Node
{
    using Abismus.Graph;

    public class NoCurrentValueException : Exception { }

    public class WrongTypeCurrentValueException : Exception { }

    public class WrongTypeFinalValueException : Exception
    {
        public object FinalValue { get; }
        public WrongTypeFinalValueException(object finalValue)
        {
            FinalValue = finalValue;
        }
    }
    public static class Executor
    {
        public static TFinal GetFinalValue<TFinal>(this IEnumerable<Edge<Node>> tree)
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

            object DelCall<T>(object f, object pcval)
            {
                if (f is Dels.O<T> f1) return f1();
                else if (f is Dels.OFromO<T> f2) return f2(CheckPcval<T>(pcval));
                else if (f is Dels.OFromOO<T> f3) return f3(CheckPcval<(T, T)>(pcval));
                else if (f is Dels.OO<T> f4) return f4();
                else if (f is Dels.OOFromO<T> f5) return f5(CheckPcval<T>(pcval));
                else if (f is Dels.OOFromOO<T> f6) return f6(CheckPcval<(T, T)>(pcval));
                return default;
            };

            Type previousFunType = default;
            object currValue = default;

            void NodeValue(Node node)
            {
                Type funType = node.Fun.GetType();

                object pcval = previousFunType != default ? currValue : default;

                object val =
                    DelCall<int>(node.Fun, pcval) ??
                    DelCall<float>(node.Fun, pcval) ??
                    DelCall<double>(node.Fun, pcval);
                if (val == default) throw new NotImplementedException();

                previousFunType = funType;

                currValue = val;
            }

            while (true)
            {
                if (!treeEn.MoveNext())
                {
                    NodeValue(treeEn.Current.Sink);
                    break;
                };
                NodeValue(treeEn.Current.Source);
            }

            if (!(currValue is TFinal))
                throw new WrongTypeFinalValueException(currValue);

            return (TFinal)currValue;
        }
    }
}
