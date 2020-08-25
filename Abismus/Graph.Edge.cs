using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics.CodeAnalysis;

namespace Abismus.Graph
{
    using Abismus.Serialization;

    /// <typeparam name="T">The vertex type</typeparam>
    public class Edge<T> : IEquatable<Edge<T>>, ISerializable<Edge<T>> 
        where T : ISerializable<T>, IEquatable<T>
    {
        public T Source { get; }
        public T Sink { get; }

        // TODO: Parent is set when tree is created, but can be re-set indefinitely (that's why settable). Maybe copy or find 
        // another, immutable, representation for the tree instead of re-using Edge.
        public Edge<T>? Parent { get; set; }

        private readonly string toString;
        
        public Edge(T source, T sink)
        {
            Source = source;
            Sink = sink;

            toString = string.Format("{0}-{1}", Source, Sink);
        }
        
        public static implicit operator Edge<T>(ValueTuple<T, T> t) => new Edge<T>(t.Item1, t.Item2);

        public override string ToString() => toString;

        public override int GetHashCode() => Source.GetHashCode() * 31 + Sink.GetHashCode();

        public override bool Equals(object? other) => 
            other != null && other is Edge<T> edge && edge.Source.Equals(Source) && edge.Sink.Equals(Sink);

        public bool Equals(Edge<T>? other) => other != null && other.Source.Equals(Source) && other.Sink.Equals(Sink);

        public XObject Serialize() =>
            new XElement("Edge",
                new XElement("Source", Source.Serialize()),
                new XElement("Sink", Sink.Serialize())
            );

        public bool Equals([AllowNull] ISerializable<Edge<T>> other) => Equals(other);

        public IDeserializer<Edge<T>> Deserializer => new EdgeDeserializer<T>((Source ?? Sink).Deserializer);
    }

    public class EdgeDeserializer<T> : IDeserializer<Edge<T>> 
        where T : ISerializable<T>, IEquatable<T>
    {
        readonly IDeserializer<T> tDeserializer;

        public EdgeDeserializer(IDeserializer<T> tDeserializer)
        {
            this.tDeserializer = tDeserializer;
        }

        public Edge<T> Deserialize(XObject ser)
        {
            if (!(ser is XElement el))
                throw new ArgumentException(nameof(ser));
            var source = tDeserializer.Deserialize(el.Element("Source"));
            var sink = tDeserializer.Deserialize(el.Element("Sink"));
            return new Edge<T>(source, sink);
        }
    }

    public static partial class Extensions
    {
        public static ISet<Edge<T>> AsSet<T>(this IEnumerable<Edge<T>> edges)
            where T : ISerializable<T>, IEquatable<T> => new HashSet<Edge<T>>(edges);

        public static XElement Serialize<T>(this ISet<Edge<T>> edgeSet)
            where T : ISerializable<T>, IEquatable<T> =>
            new XElement("EdgeSet", 
                edgeSet.Where(edge => edge != default).Select(edge => edge.Serialize()));
    }
}