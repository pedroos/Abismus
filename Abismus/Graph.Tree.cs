using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Abismus.Graph
{
    using Abismus.Serialization;

    public static partial class Extensions
    {
        ///// <summary>
        ///// Returns a tree from an edge set by walking from an initial position (by position)
        ///// </summary>
        //public static IEnumerable<Edge<T>> TreeByPositions<T>(this ISet<Edge<T>> edges,
        //    int fromPosition)
        //{
        //    var treeByPosition = TreePositions(edges, fromPosition);
        //    return treeByPosition.TreeByEdges(edges);
        //}

        ///// <summary>
        ///// Returns a position list for a tree from an edge set by walking from an initial 
        ///// position
        ///// </summary>
        //public static IEnumerable<int> TreePositions<T>(this ISet<Edge<T>> edges,
        //    int fromPosition)
        //{
        //    // Return the current edge's position
        //    if (edges.Count() <= fromPosition) yield break;
        //    yield return fromPosition;
        //    var fromEdge = edges.ElementAt(fromPosition);
        //    // Search among all edges for connected edges
        //    var connectedPositions = new List<int>();
        //    for (int i = 0; i < edges.Count(); ++i)
        //    {
        //        var edge = edges.ElementAt(i);
        //        if (edge.Source.Equals(fromEdge.Sink))
        //            connectedPositions.Add(i);
        //    }
        //    foreach (int connectedPosition in connectedPositions)
        //        // Return each connected edge's positions and their connected edge's positions
        //        foreach (int position in edges.TreePositions(connectedPosition))
        //            yield return position;
        //}

        /// <summary>
        /// Creates a tree from an edge set by walking from an initial position.
        /// Set edge's Parents by connecting edges from source to sink.
        /// </summary>
        public static IEnumerable<Edge<T>> Tree<T>(this ISet<Edge<T>> edges, Edge<T> fromEdge)
            where T : ISerializable<T>, IEquatable<T>
        {
            if (!edges.Any(e => e.Equals(fromEdge))) yield break;
            // Return the current edge
            yield return fromEdge;
            // Search among all edges for connected edges
            var connecteds = edges.Where(e => e.Source.Equals(fromEdge.Sink));
            foreach (var connected in connecteds)
            {
                connected.Parent = fromEdge;
                // Return each connected edge and their connected edges
                var subTree = edges.Tree(connected);
                foreach (var edge in subTree)
                {
                    yield return edge;
                }
            }
        }

        public static IEnumerable<T> TreeVertexes<T>(this ISet<Edge<T>> edges, Edge<T> fromEdge)
            where T : ISerializable<T>, IEquatable<T>
        {
            if (!edges.Any(e => e.Equals(fromEdge))) yield break;
            yield return fromEdge.Source;
            yield return fromEdge.Sink;
            var connecteds = edges.Where(e => e.Source.Equals(fromEdge.Sink));
            foreach (var connected in connecteds)
            {
                connected.Parent = fromEdge;
                var tree = edges.Tree(connected);
                foreach (var edge in tree)
                {
                    yield return edge.Sink;
                }
            }
        }

        /// <summary>
        /// Walk the path from an edge to the root vertex of a tree returned from Tree().
        /// </summary>
        public static IEnumerable<Edge<T>> WalkUp<T>(this Edge<T> fromEdge)
            where T : ISerializable<T>, IEquatable<T>
        {
            yield return fromEdge;
            while (fromEdge.Parent != default)
            {
                yield return fromEdge.Parent;
                fromEdge = fromEdge.Parent;
            }
        }

        ///// <summary>
        ///// Reads the tree from an edge set and position list
        ///// </summary>
        //private static IEnumerable<Edge<T>> TreeByPositions<T>(this ISet<Edge<T>> edges, 
        //    IEnumerable<int> positions)
        //{
        //    return positions.Select(p => edges.ElementAt(p));
        //}

        ///// <summary>
        ///// Reads the tree from a position list and edge set (inverse of OfPositions())
        ///// </summary>
        //private static IEnumerable<Edge<T>> TreeByEdges<T>(this IEnumerable<int> positions, 
        //    ISet<Edge<T>> edges)
        //{
        //    return TreeByPositions(edges, positions);
        //}

        public static void ValidateTree<T>(this IEnumerable<Edge<T>> tree)
            where T : ISerializable<T>, IEquatable<T>
        {
            var added = new List<Edge<T>>();
            var treeEn = tree.GetEnumerator();
            while (treeEn.MoveNext())
            {
                if (added.Any(e => e.Source.Equals(treeEn.Current.Sink)))
                    throw new CycleException<T>(treeEn.Current);
                added.Add(treeEn.Current);
            }
        }
    }
}
