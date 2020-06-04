using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Abismus.Graph
{
    public static partial class ExtensionMethods
    {
        ///// <summary>
        ///// Returns a tree from an edge set by walking from an initial position (by position)
        ///// </summary>
        //public static IEnumerable<Edge<T>> TreeByPositions<T>(this ISet<Edge<T>> edges,
        //    int fromPosition)
        //    where T : IComparable<T>
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
        //    where T : IComparable<T>
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
        /// Returns a tree from an edge set by walking from an initial position
        /// </summary>
        public static IEnumerable<Edge<T>> Tree<T>(this ISet<Edge<T>> edges, Edge<T> fromEdge)
            where T : IComparable<T>
        {
            if (!edges.Contains(fromEdge)) yield break;
            // Return the current edge
            yield return fromEdge;
            // Search among all edges for connected edges
            var connecteds = edges.Where(e => e.Source.Equals(fromEdge.Sink));
            foreach (var connected in connecteds)
            {
                connected.Parent = fromEdge;
                // Return each connected edge and their connected edges
                foreach (var edge in edges.Tree(connected))
                {
                    yield return edge;
                }
            }
        }

        /// <summary>
        /// Walk the path from an edge to the root vertex of a tree returned from Tree().
        /// </summary>
        public static IEnumerable<Edge<T>> WalkUp<T>(this Edge<T> fromEdge)
            where T : IComparable<T>
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
        //    where T : IComparable<T>
        //{
        //    return positions.Select(p => edges.ElementAt(p));
        //}

        ///// <summary>
        ///// Reads the tree from a position list and edge set (inverse of OfPositions())
        ///// </summary>
        //private static IEnumerable<Edge<T>> TreeByEdges<T>(this IEnumerable<int> positions, 
        //    ISet<Edge<T>> edges)
        //    where T : IComparable<T>
        //{
        //    return TreeByPositions(edges, positions);
        //}

        public static void ValidateTree<T>(this IEnumerable<Edge<T>> tree)
            where T : IComparable<T>
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
