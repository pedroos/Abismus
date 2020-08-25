using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abismus.Node
{
    using Abismus.Graph;
    public static class DynamicExecutorF
    {
        public static object[] GetFinalValueDynamic(this IEnumerable<Node> vert)
        {
            var vertEn = vert.GetEnumerator();

            var currValue = new List<object?>();

            void NodeValue(Node node)
            {
                for (int i = 0; i < node.FunSig.Outs.Count(); ++i)
                    currValue.Add(null);
                var cva = currValue.ToArray();
                node.Fun.DynamicInvoke(cva);

                currValue = cva.Skip(node.FunSig.Ins.Count()).Take(node.FunSig.Outs.Count()).ToList();
            }

            while (vertEn.MoveNext()) 
                NodeValue(vertEn.Current);

            return currValue.Cast<object>().ToArray();
        }

        public static T GetFinalValueDynamic<T>(this IEnumerable<Node> vert)
        {
            var res = GetFinalValueDynamic(vert);
            return (T)res.First();
        }
    }
}
