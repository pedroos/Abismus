using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Abismus.Graph
{
    /// <typeparam name="T">The vertex type</typeparam>
    public class Edge<T> : IEquatable<Edge<T>>
        where T : IComparable<T>
    {
        public T Source { get; }
        public T Sink { get; }
        public Edge<T> Parent { get; set; }
        private readonly string toString;
        public Edge(T source, T sink)
        {
            Source = source;
            Sink = sink;
            toString = string.Format("{0}-{1}", Source, Sink);
        }
        public static implicit operator Edge<T>(ValueTuple<T, T> t) =>
            new Edge<T>(t.Item1, t.Item2);
        public override string ToString()
        {
            return toString;
        }
        public override int GetHashCode()
        {
            return Source.GetHashCode() * 31 + Sink.GetHashCode();
        }
        public override bool Equals(object other)
        {
            return other is Edge<T> edge ?
                edge.Source.Equals(Source) &&
                    edge.Sink.Equals(Sink) :
                false;
        }
        public bool Equals(Edge<T> other)
        {
            return other.Source.Equals(Source) && other.Sink.Equals(Sink);
        }
    }

    public static partial class ExtensionMethods
    {
        public static ISet<Edge<T>> AsSet<T>(this IEnumerable<Edge<T>> edges)
            where T : IComparable<T>
        {
            return new HashSet<Edge<T>>(edges);
        }
    }
}