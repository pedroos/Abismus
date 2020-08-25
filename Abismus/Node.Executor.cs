using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Abismus.Node
{
    using Abismus.Graph;
    using System.Data.Odbc;
    using System.Reflection;

    public class NoCurrentValueException : Exception { }

    public class WrongTypeFinalValueException : WrongTypeException
    {
        public object FinalValue { get; }
        public WrongTypeFinalValueException(Type typeShould, Type? typeIs, object finalValue) : base(typeShould, typeIs)
        {
            FinalValue = finalValue;
        }
    }

    public static class Executor
    {
        static Executor()
        {
            string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Abismus");
            if (!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);
            string executionsPath = Path.Combine(dataPath, "Executions.txt");
            if (File.Exists(executionsPath))
            {
                string[] executions = File.ReadAllLines(executionsPath);
                // Recuperar execução
            }
        }
        static string executionName;

        public static void SetExecutionName(string name)
        {
            // Persistir fisicamente execução
        }

        //public static TFinal GetFinalValueA<TFinal>(this IEnumerable<Edge<object>> tree)
        //{
        //    var treeEn = tree.GetEnumerator();

        //    //Edge<object> nodeA = treeEn.Current;
        //    //if (nodeA)

        //    return default;
        //}

        public static TFinal GetFinalValue<TFinal>(this IEnumerable<Edge<Node>> tree)
        {
            var treeEn = tree.GetEnumerator();

            TInput CheckPcval<TInput>(object previousCurrentValue)
            {
                if (previousCurrentValue == default)
                    throw new NoCurrentValueException();
                if (!(previousCurrentValue is TInput))
                    throw new WrongTypeException(typeof(TInput), previousCurrentValue.GetType());
                return (TInput)previousCurrentValue;
            }

            object DelCall1<T>(object f, object pcval)
            {
                if (f is Dels.O<T> f1) return f1();
                else if (f is Dels.IO<T, T> f2) return f2(CheckPcval<T>(pcval));
                else if (f is Dels.IIO<T, T, T> f3) return f3(CheckPcval<(T, T)>(pcval));
                else if (f is Dels.OO<T, T> f4) return f4();
                else if (f is Dels.IOO<T, T, T> f5) return f5(CheckPcval<T>(pcval));
                else if (f is Dels.IIOO<T, T, T, T> f6) return f6(CheckPcval<(T, T)>(pcval));
                return default;
            };

            //object DelCall2<T1, T2>(object f, object pcval, out int pt)
            //{
            //    //pt = 0;
            //    bool eq12 = typeof(T1) == typeof(T2);

            //    if (eq12) return DelCall1<T1>(f, pcval, out pt);

            //    if (f is Dels.IToO<T1, T2> f1) return f1(CheckPcval<T1>(pcval));
            //    else if (f is Dels.OO<T1, T2> f2) return f2();
            //    return default;
            //}

            //object DelCall3<T1, T2, T3>(object f, object pcval, out int pt)
            //{
            //    //pt = 0;
            //    bool eq12 = typeof(T1) == typeof(T2);
            //    bool eq13 = typeof(T1) == typeof(T3);
            //    bool eq23 = typeof(T2) == typeof(T3);
            //    bool eq123 = eq12 && eq23;

            //    if (eq123) return DelCall1<T1>(f, pcval, out pt);
            //    else if (eq12 && !eq23) return DelCall2<T2, T3>(f, pcval, out pt);
            //    else if (!eq12 && (eq23 || eq13)) return DelCall2<T1, T2>(f, pcval, out pt);

            //    if (f is Dels.IIToO<T1, T2, T3> f1) return f1(CheckPcval<(T1, T2)>(pcval));
            //    if (f is Dels.IToOO<T1, T2, T3> f2) return f2(CheckPcval<T1>(pcval));
            //    return default;
            //}

            //object DelCall4<T1, T2, T3, T4>(object f, object pcval, out int pt)
            //{
            //    //pt = 0;
            //    bool eq12 = typeof(T1) == typeof(T2);
            //    bool eq13 = typeof(T1) == typeof(T3);
            //    bool eq14 = typeof(T1) == typeof(T4);
            //    bool eq23 = typeof(T2) == typeof(T3);
            //    bool eq24 = typeof(T2) == typeof(T4);
            //    bool eq34 = typeof(T3) == typeof(T4);
            //    bool eq123 = eq12 && eq23;
            //    bool eq234 = eq23 && eq34;
            //    bool eq124 = eq12 && eq24;
            //    bool eq134 = eq13 && eq14;
            //    bool eq1234 = eq123 && eq34;

            //    if (eq1234) return DelCall1<T1>(f, pcval, out pt);
            //    else if (eq123 && !eq34) return DelCall2<T3, T4>(f, pcval, out pt);
            //    else if (eq234 && !eq12) return DelCall2<T1, T2>(f, pcval, out pt);
            //    else if (eq124 && !eq23) return DelCall2<T2, T3>(f, pcval, out pt);
            //    else if (eq134 && !eq12) return DelCall2<T1, T2>(f, pcval, out pt);
            //    else if (eq12 && !eq23 && eq34) return DelCall2<T1, T3>(f, pcval, out pt);
            //    else if (eq12 && !eq23 && !eq34) return DelCall3<T2, T2, T4>(f, pcval, out pt);
            //    else if (!eq12 && eq23 && !eq34) return DelCall3<T1, T2, T4>(f, pcval, out pt);
            //    else if (!eq12 && !eq23 && eq34) return DelCall3<T1, T2, T3>(f, pcval, out pt);

            //    if (f is Dels.IIToOO<T1, T2, T3, T4> f1) return f1(CheckPcval<(T1, T2)>(pcval));
            //    return default;
            //}

            var node = new Node((Dels.O<int>)Funcs.Fixed);
            //node.Fun.GetType().
            var methodInfo = ((Delegate)node.Fun).GetMethodInfo();
            var parameterInfo = methodInfo.GetParameters();
            //parameterInfo

            //Type previousFunType = default;
            //object currValue = default;

            //void NodeValue(Node node, int pta = -1, int ptb = -1)
            //{
            //    Type funType = node.Fun.GetType();

            //    object pcval = previousFunType != default ? currValue : default;

            //    void SavePt(int pta_, int ptb_)
            //    {

            //    }

            //    //if (pta != -1 && ptb != -1)
            //    //{
            //    object val = default;
            //    int ptb2 = default;
            //    if (pta == 1 || pta == -1) val = DelCall1<int>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == -1 && ptb2 != -1) { SavePt(1, ptb2); goto Cont; }
            //    if (pta == 2 || pta == -1) val = DelCall1<float>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 3) val = DelCall1<double>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 4) val = DelCall2<int, float>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 5) val = DelCall2<int, double>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 6) val = DelCall2<float, double>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 7) val = DelCall2<float, int>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 8) val = DelCall2<double, int>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 9) val = DelCall2<double, float>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 10) val = DelCall3<int, int, float>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 11) val = DelCall3<int, float, int>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 12) val = DelCall3<float, int, int>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 13) val = DelCall3<int, float, float>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 14) val = DelCall3<float, float, int>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 15) val = DelCall3<float, int, float>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 16) val = DelCall3<int, float, float>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 17) val = DelCall3<float, float, int>(node.Fun, pcval, ptb, out ptb2);
            //    if (pta == 18) val = DelCall3<float, int, float>(node.Fun, pcval, ptb, out ptb2);

            //    Cont:

            //    //int parameterCount = ((Delegate)node.Fun).Method.GetParameters().Count();
            //    //if (parameterCount > 0) 
            //    //    currValue = ((Delegate)node.Fun).DynamicInvoke(pcval);
            //    //else
            //    //    currValue = ((Delegate)node.Fun).DynamicInvoke();
            //    previousFunType = funType;
            //}

            //// Se houver execução, ler o pta e ptb de cada nó
            //var nodesPts = new Dictionary<Guid, (int pta, int ptb)>();

            //// Terá de haver um "node id" composto pelo que identifica unicamente um nó no contexto de uma árvore. 
            //// A posição e a função.

            //while (true)
            //{
            //    if (!treeEn.MoveNext())
            //    {
            //        NodeValue(treeEn.Current.Sink);
            //        break;
            //    };
            //    NodeValue(treeEn.Current.Source);
            //}

            //if (!(currValue is TFinal))
            //    throw new WrongTypeFinalValueException(currValue);

            //return (TFinal)currValue;
            return default;
        }
    }
}
