using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

[assembly: InternalsVisibleTo("AbismusTests")]

namespace Abismus.Signature
{
    public class Signature : IEquatable<Signature>
    {
        public IEnumerable<Type> Ins { get; private set; }
        public IEnumerable<Type> Outs { get; }
        
        internal IEnumerable<ulong> valsI;
        internal IEnumerable<double> valsD;
        internal double number;

        public Signature(Delegate del)
        {
            // Read signature:
            var parameters = del.Method.GetParameters();
            Ins = parameters.Where(p => !p.IsOut).Select(p => p.ParameterType).ToArray();
            var outputTypesList = new List<Type>();
            if (del.Method.ReturnType.IsAssignableFrom(typeof(ValueTuple)))
            {
                // Tuple return parameter
                // TODO: Count tuple type items
            }
            else
            {
                // Single return parameter
                if (del.Method.ReturnType != typeof(void))
                    outputTypesList.Add(del.Method.ReturnType);
            }
            // Look for out parameters:
            var outParameters = parameters.Where(p => p.IsOut);
            if (outParameters.Count() > 0)
            {
                outputTypesList.AddRange(outParameters.Select(
                    p => p.ParameterType.IsByRef ?
                        p.ParameterType.GetElementType()! :
                        p.ParameterType
                ));
            }
            Outs = outputTypesList.ToArray();

            // Calculate value:
            valsI = Signatures.MakeList(Ins, Outs);
            valsD = valsI.ToDoubleArray();
            number = Noper.Calc(valsD);
        }

        public bool Equals([AllowNull] Signature other) => other != null && Noper.DoubleEquals(other.number, number, Noper.Eps);
    }

    internal static class Signatures
    {
        internal static ulong[] PropIds { get; private set; }
        internal static List<Type> Types { get; }
        internal static Dictionary<Type, ulong> InputTypesValues { get; private set; }
        internal static Dictionary<Type, ulong> OutputTypesValues { get; private set; }

        static Signatures()
        {
            // PropId is an automatically generated number to identify values.
            // Currently 1 - 50. For combinations of values, use powers of two.
            PropIds = new ulong[50];
            int propIdsHalfLength = (int)Math.Floor(PropIds.Length / (float)2);
            for (int i = 0; i < PropIds.Length; ++i)
            {
                ulong curr = (ulong)(i + 1);
                PropIds[i] = curr;
            }

             // A list of all types possible to be evaluated.
             // If changed, results are invalidated.
            Types = new List<Type>() {
                typeof(bool),
                typeof(int),
                typeof(float),
                typeof(double),
                typeof(decimal),
                typeof(DateTime),
                typeof(string),
                typeof(bool?),
                typeof(int?),
                typeof(float?),
                typeof(double?),
                typeof(decimal?),
                typeof(DateTime?), 
                typeof(IEnumerable<string>),
                typeof(IEnumerable<(string Line, IEnumerable<(int Start, int Length)> Matches)>)
            };
            if (Types.Count > propIdsHalfLength) throw new InvalidOperationException("PropIds is short.");

            InputTypesValues = new Dictionary<Type, ulong>();
            OutputTypesValues = new Dictionary<Type, ulong>();

            // Index input types with values in the first PropIds subrange.
            // Input type values range from PropIds[0] to PropIds[Types.Count].
            for (int i = 0; i < Types.Count; ++i)
            {
                if (Types[i] == default) continue;
                InputTypesValues.Add(Types[i], PropIds[i]);
            }
            // Index output types with values in the second and final PropIds subrange.
            // Output types values range from PropIds[propIdsHalfLength] to PropIds[proIdsHalfLength + Types.Count].
            for (int i = 0; i < Types.Count; ++i)
            {
                if (Types[i] == default) continue;
                OutputTypesValues.Add(Types[i], PropIds[i + propIdsHalfLength]);
            }
        }

        internal static ulong[] MakeList(IEnumerable<Type> ins, IEnumerable<Type> outs)
        {
            var values = new ulong[
                /*(inputTypes != null ? */ins.Count()/* : 0)*/ +
                /*(outputTypes != null ? */outs.Count()/* : 0)*/
            ];
            //if (inputTypes != null) 
            for (int i = 0; i < ins.Count(); ++i)
            {
                if (!InputTypesValues.ContainsKey(ins.ElementAt(i)))
                    throw new TypeUnmappedException(false, ins.ElementAt(i));
                values[i] = InputTypesValues[ins.ElementAt(i)];
            }
            //if (outputTypes != null)
            for (int i = 0; i < outs.Count(); ++i)
            {
                if (!OutputTypesValues.ContainsKey(outs.ElementAt(i)))
                    throw new TypeUnmappedException(true, outs.ElementAt(i));
                values[i + /*(inputTypes != null ? */ins.Count()/* : 0)*/] = OutputTypesValues[
                    outs.ElementAt(i)];
            }
            return values;
        }
    }

    public static partial class Extensions
    {
        public static IEnumerable<double> ToDoubleArray(this IEnumerable<ulong> funValues) => 
            funValues.Select(v => Convert.ToDouble(v)).ToArray();
    }

    public class TypeUnmappedException : Exception
    {
        public bool IsOutput { get; }
        public Type Type { get; }
        public TypeUnmappedException(bool isOutput, Type type) : base(string.Format("{0} type '{1}' is unmapped", 
            isOutput ? "Output" : "Input", type.Name))
        {
            IsOutput = isOutput;
            Type = type;
        }
    }
}
