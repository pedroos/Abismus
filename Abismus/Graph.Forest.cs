using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Abismus.Graph
{
    using Abismus.Serialization;

    public static partial class Extensions
    {
        public static IEnumerable<Edge<T>> FindInitialEdges<T>(this ISet<Edge<T>> edges)
            where T : ISerializable<T>, IEquatable<T> => edges.Where(e => !edges.Any(e2 => e2.Sink.Equals(e.Source)));

        public static IEnumerable<Edge<T>> FindTerminalEdges<T>(this ISet<Edge<T>> edges)
            where T : ISerializable<T>, IEquatable<T> => edges.Where(e => !edges.Any(e2 => e2.Source.Equals(e.Sink)));

        public static IEnumerable<Edge<T>> FindForkEdges<T>(this ISet<Edge<T>> edges)
            where T : ISerializable<T>, IEquatable<T> => edges.Where(e => edges.Any(e2 => e2.Source.Equals(e.Source) && 
            !e2.Equals(e)));

        public static IEnumerable<Edge<T>> FindJoinEdges<T>(this ISet<Edge<T>> edges)
            where T : ISerializable<T>, IEquatable<T> => edges.Where(e => edges.Any(e2 => e2.Sink.Equals(e.Sink) && !e2.Equals(e)));

        /// <summary>
        /// Finds an edge set's initial edges and returns the trees starting from them
        /// </summary>
        public static IEnumerable<IEnumerable<Edge<T>>> AllTrees<T>(this ISet<Edge<T>> edges)
            where T : ISerializable<T>, IEquatable<T>
        {
            var initial = edges.FindInitialEdges();
            return initial.Select(i => edges.Tree(i));
        }

        /// <summary>
        /// Validate an edge set
        /// </summary>
        public static void ValidateEdges<T>(this ISet<Edge<T>> edges)
            where T : ISerializable<T>, IEquatable<T>
        {
            // Look for an edge with equal source and sink
            var edge = edges.FirstOrDefault(e => e.Source.Equals(e.Sink));
            if (edge != default)
                throw new CycleException<T>(edge);

            // Look for an edge disconnected both at source and at sink, or both initial and terminal
            var initials = edges.FindInitialEdges();
            var terminals = edges.FindTerminalEdges();
            edge = initials.FirstOrDefault(e => terminals.Contains(e));
            if (edge != default)
                throw new UnconnectedEdgeException<T>(edge);
        }
    }

    #region Exceptions

    public abstract class InvalidEdgeException<T> : Exception
        where T : ISerializable<T>, IEquatable<T>
    {
        public Edge<T> Edge { get; }
        public InvalidEdgeException(Edge<T> edge)
        {
            Edge = edge;
        }
        public override string Message => string.Format("Invalid edge: {0}", Edge);
    }

    public class UnconnectedEdgeException<T> : InvalidEdgeException<T>
        where T : ISerializable<T>, IEquatable<T>
    {
        public UnconnectedEdgeException(Edge<T> edge) : base(edge) { }
    }

    public class CycleException<T> : InvalidEdgeException<T>
        where T : ISerializable<T>, IEquatable<T>
    {
        public CycleException(Edge<T> edge) : base(edge) { }
    }

    #endregion
}
