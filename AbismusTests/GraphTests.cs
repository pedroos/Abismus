using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;

namespace Abismus.Graph.Tests
{
    using Abismus.Graph;

    [TestClass]
    public class GraphTests
    {
        private static readonly HashSet<Edge<int>> alteryxWorkflow, alteryxPaths;

        static GraphTests()
        {
            #region Sample data

            // http://graphonline.ru/en/?graph=rnNIuHBgplgRqXlv
            alteryxWorkflow = new HashSet<Edge<int>>
            {
                (1, 2),
                (2, 3),
                (3, 4),
                (3, 5),
                (5, 6),
                (6, 7),
                (7, 8),
                (8, 9),
                (9, 10),
                (9, 12),
                (12, 13),
                (13, 14),
                (7, 11),
                (6, 15),
                (6, 16),
                (16, 17),
                (16, 18),
                (19, 20),
                (20, 21),
                (21, 22),
                (21, 23),
                (23, 24),
                (24, 16), // The tree-connecting edge
                (24, 25),
                (24, 26),
                (26, 27),
                (27, 30),
                (30, 32),
                (32, 13),
                (30, 31),
                (26, 28),
                (26, 29)
            };

            // http://graphonline.ru/en/?graph=SuWTpPlKVyCTUkGj
            alteryxPaths = new HashSet<Edge<int>>
            {
                //// Path 1
                //(1, 2), 
                //(2, 3),
                //(3, 4),
                //(4, 5),
                //// Path 2
                //(6, 7),
                //(7, 10),
                //(10, 11),
                //(11, 13),
                //// Path 3
                //(8, 9),
                //// Path 4
                //(10, 12),
                //// Path 5
                //(14, 15), 
                //// Join 1
                //(5, 14), 
                //(13, 14), 
                //// Join 2
                //(2, 8), 
                //(7, 8)
                (0, 1),
                (1, 2),
                (2, 3),
                (3, 4),
                (4, 13),
                (13, 14),
                (1, 5),
                (5, 6),
                (7, 8),
                (8, 5),
                (8, 9),
                (9, 10),
                (10, 12),
                (12, 13),
                (9, 11)
            };

            #endregion
        }

        [TestMethod]
        public void EdgeEquality1Test()
        {
            var edge1 = (1, 2);
            var edge2 = (1, 2);
            Assert.AreEqual(edge1, edge2);
            Assert.IsTrue(edge1.Equals(edge2));
        }

        [TestMethod]
        public void EdgeEquality2Test()
        {
            var edge1 = (1, 1);
            var edge2 = edge1;
            Assert.AreEqual(edge1, edge2);
            Assert.IsTrue(edge1.Equals(edge2));
        }

        [TestMethod]
        public void EdgeInequality1Test()
        {
            var edge1 = (1, 2);
            var edge2 = (1, 3);
            Assert.AreNotEqual(edge1, edge2);
            Assert.IsFalse(edge1.Equals(edge2));
        }

        [TestMethod]
        public void EdgeInequality2Test()
        {
            var edge1 = (1, 2);
            var edge2 = (2, 1);
            Assert.AreNotEqual(edge1, edge2);
            Assert.IsFalse(edge1.Equals(edge2));
        }

        [TestMethod]
        public void SampleDataTest()
        {
            Assert.AreEqual(32, alteryxWorkflow.Count);
        }

        [TestMethod]
        public void FindInitialEdgesTest()
        {
            var edges = alteryxPaths.FindInitialEdges();
            Assert.AreEqual(2, edges.Count());
            //Assert.IsTrue(edges.Contains((1, 2)));
            //Assert.IsTrue(edges.Contains((6, 7)));
            Assert.IsTrue(edges.Contains((0, 1)));
            Assert.IsTrue(edges.Contains((7, 8)));
        }

        [TestMethod]
        public void FindTerminalEdgesTest()
        {
            var edges = alteryxPaths.FindTerminalEdges();
            Assert.AreEqual(3, edges.Count());
            //Assert.IsTrue(edges.Contains((8, 9)));
            //Assert.IsTrue(edges.Contains((10, 12)));
            //Assert.IsTrue(edges.Contains((14, 15)));
            Assert.IsTrue(edges.Contains((5, 6)));
            Assert.IsTrue(edges.Contains((9, 11)));
            Assert.IsTrue(edges.Contains((13, 14)));
        }

        [TestMethod]
        public void FindForkEdgesTest()
        {
            var edges = alteryxPaths.FindForkEdges();
            Assert.AreEqual(6, edges.Count());
            //Assert.IsTrue(edges.Contains((2, 3)));
            //Assert.IsTrue(edges.Contains((2, 8)));
            //Assert.IsTrue(edges.Contains((7, 8)));
            //Assert.IsTrue(edges.Contains((7, 10)));
            //Assert.IsTrue(edges.Contains((10, 11)));
            //Assert.IsTrue(edges.Contains((10, 12)));
            Assert.IsTrue(edges.Contains((1, 2)));
            Assert.IsTrue(edges.Contains((1, 5)));
            Assert.IsTrue(edges.Contains((8, 5)));
            Assert.IsTrue(edges.Contains((8, 9)));
            Assert.IsTrue(edges.Contains((9, 10)));
            Assert.IsTrue(edges.Contains((9, 11)));
        }

        [TestMethod]
        public void FindJoinEdgesTest()
        {
            var edges = alteryxPaths.FindJoinEdges();
            Assert.AreEqual(4, edges.Count());
            //Assert.IsTrue(edges.Contains((2, 8)));
            //Assert.IsTrue(edges.Contains((7, 8)));
            //Assert.IsTrue(edges.Contains((5, 14)));
            //Assert.IsTrue(edges.Contains((13, 14)));
            Assert.IsTrue(edges.Contains((1, 5)));
            Assert.IsTrue(edges.Contains((8, 5)));
            Assert.IsTrue(edges.Contains((4, 13)));
            Assert.IsTrue(edges.Contains((12, 13)));
        }

        //[TestMethod]
        //public void WalkTest()
        //{
        //    var trees = alteryxPaths.Walk();
        //    Assert.AreEqual(2, trees.Count());

        //    var tree1 = trees.First();
        //    Assert.AreEqual(8, tree1.Count());
        //    var tree1En = tree1.GetEnumerator();
        //    //tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((1, 2)));
        //    //tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((2, 3)));
        //    //tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((3, 4)));
        //    //tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((4, 5)));
        //    //tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((5, 14)));
        //    //tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((14, 15)));
        //    //tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((2, 8)));
        //    //tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((8, 9)));
        //    tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((0, 1)));
        //    tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((1, 2)));
        //    tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((2, 3)));
        //    tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((3, 4)));
        //    tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((4, 13)));
        //    tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((13, 14)));
        //    tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((1, 5)));
        //    tree1En.MoveNext(); Assert.IsTrue(tree1En.Current.Equals((5, 6)));

        //    var tree2 = trees.Skip(1).First();
        //    Assert.AreEqual(9, tree2.Count());
        //    var tree2En = tree2.GetEnumerator();
        //    //tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((6, 7)));
        //    //tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((7, 10)));
        //    //tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((10, 11)));
        //    //tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((11, 13)));
        //    //tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((13, 14)));
        //    //tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((14, 15)));
        //    //tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((10, 12)));
        //    //tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((7, 8)));
        //    //tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((8, 9)));
        //    tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((7, 8)));
        //    tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((8, 5)));
        //    tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((5, 6)));
        //    tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((8, 9)));
        //    tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((9, 10)));
        //    tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((10, 12)));
        //    tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((12, 13)));
        //    tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((13, 14)));
        //    tree2En.MoveNext(); Assert.IsTrue(tree2En.Current.Equals((9, 11)));
        //}

        //[TestMethod]
        //public void TreePositionsTest()
        //{
        //    var treePositions = alteryxPaths.TreePositions(0);
        //    Assert.IsTrue(treePositions.SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }));
        //}

        //[TestMethod]
        //public void TreePositionsTest2()
        //{
        //    var treePositions = alteryxPaths.TreePositions(1);
        //    Assert.IsTrue(treePositions.SequenceEqual(new int[] { 1, 2, 3, 4, 5 }));
        //}

        //[TestMethod]
        //public void TreeByPositionsTest()
        //{
        //    var tree = alteryxPaths.TreeByPositions(0);
        //    var edgeTree = alteryxPaths.Tree((0, 1));
        //    Assert.IsTrue(tree.SequenceEqual(edgeTree));
        //}

        [TestMethod]
        public void TreeTest1()
        {
            var tree = alteryxPaths.Tree((0, 1));
            Assert.IsTrue(tree.SequenceEqual(new Edge<int>[] { (0, 1), (1, 2), (2, 3), (3, 4),
                (4, 13), (13, 14), (1, 5), (5, 6) }));
        }

        [TestMethod]
        public void TreeTest2()
        {
            var tree = alteryxPaths.Tree((1, 2));
            Assert.AreEqual(5, tree.Count());
            Assert.IsTrue(tree.SequenceEqual(new Edge<int>[] { (1, 2), (2, 3), (3, 4), (4, 13),
                (13, 14) }));
        }

        [TestMethod]
        public void TreeTest3()
        {
            var tree = alteryxPaths.Tree((1, int.MaxValue));
            Assert.AreEqual(0, tree.Count());
        }

        [TestMethod]
        public void TreeTest4()
        {
            var tree = alteryxPaths.Tree((13, 14));
            Assert.AreEqual(1, tree.Count());
            Assert.IsTrue(tree.First().Equals((13, 14)));
        }

        [TestMethod]
        public void TreeParentsTest()
        {
            var tree = alteryxPaths.Tree((0, 1));
            var parents = tree.Select(e => e.Parent);
            Assert.IsTrue(parents.SequenceEqual(new Edge<int>[] { default, (0, 1), (1, 2), (2, 3),
                (3, 4), (4, 13), (0, 1), (1, 5) }));
        }

        [TestMethod]
        public void WalkUpTest1()
        {
            var tree = alteryxPaths.Tree((0, 1));
            var walkUp = tree.Single(e => e.Equals((13, 14))).WalkUp();
            Assert.IsTrue(walkUp.SequenceEqual(new Edge<int>[] { (13, 14), (4, 13), (3, 4), (2, 3),
                (1, 2), (0, 1) }));
        }

        [TestMethod]
        public void OverlapTest()
        {
            var trees = alteryxPaths.AllTrees();
            var tree1 = trees.First();
            var tree2 = trees.Skip(1).First();

            // Verify the overlap and that subtracting the overlap from each tree and summing the 
            // trees back together with the overlap equals the original graph.

            var overlap = tree1.Intersect(tree2);
            Assert.AreEqual(2, overlap.Count());
            //Assert.IsTrue(overlap.ElementAt(0).Equals((14, 15)));
            //Assert.IsTrue(overlap.ElementAt(1).Equals((8, 9)));
            Assert.IsTrue(overlap.ElementAt(0).Equals((13, 14)));
            Assert.IsTrue(overlap.ElementAt(1).Equals((5, 6)));

            var tree1MinusOverlap = tree1.Except(overlap);
            var tree2MinusOverlap = tree2.Except(overlap);
            var sum = new HashSet<Edge<int>>(tree1MinusOverlap.Union(tree2MinusOverlap)
                .Union(overlap));

            // Walk the entire graph again, but this time the assembled graph. Then, check that 
            // each tree equals the original graph's trees.

            var sumTrees = sum.AllTrees();

            var sumTree1 = sumTrees.First();
            Assert.IsTrue(sumTree1.SequenceEqual(tree1));

            var sumTree2 = sumTrees.Skip(1).First();
            Assert.IsTrue(sumTree2.SequenceEqual(tree2));

            var sumOverlap = sumTree1.Intersect(sumTree2);
            Assert.AreEqual(overlap.Count(), sumOverlap.Count());
            Assert.IsTrue(sumOverlap.SequenceEqual(overlap));
        }

        [TestMethod]
        public void WalkInitialsTest()
        {
            // Test that the first edge in each tree returned by Walk() is one of the initial edges
            var initials = alteryxPaths.FindInitialEdges();
            var trees = alteryxPaths.AllTrees();
            Assert.AreEqual(initials.Count(), trees.Count());
            var initialsEn = initials.GetEnumerator();
            var treesEn = trees.GetEnumerator();
            while (initialsEn.MoveNext())
            {
                treesEn.MoveNext();
                Assert.IsTrue(initialsEn.Current.Equals(treesEn.Current.First()));
            }
        }

        [TestMethod]
        public void InitialsPlusJoinsTest()
        {
            // Test that the initial edge's trees plus the join edge's trees summed equal the 
            // original graph
            var trees = alteryxPaths.AllTrees();
            var joins = alteryxPaths.FindJoinEdges();
            // Walk from each join
            var joinTrees = joins.Select(j => alteryxPaths.Tree(j));
            // Intersect
            var overlap = trees.First().Intersect(trees.Skip(1).First());
            // Subtract
            var treesMinusOverlap = trees.Select(t => t.Except(overlap));
            // Sum
            var sum = new HashSet<Edge<int>>(treesMinusOverlap.First().Union(
                treesMinusOverlap.Skip(1).First()).Union(overlap));

            var sumTrees = sum.AllTrees();
            Assert.AreEqual(sumTrees.Count(), trees.Count());
            var treesEn = trees.GetEnumerator();
            var sumTreesEn = sumTrees.GetEnumerator();
            while (treesEn.MoveNext())
            {
                sumTreesEn.MoveNext();
                Assert.IsTrue(treesEn.Current.SequenceEqual(sumTreesEn.Current));
            }
        }

        [TestMethod]
        public void ValidateTreePositive1Test()
        {
            var tree = alteryxPaths.Tree((0, 1));
            tree.ValidateTree();
        }

        [TestMethod]
        public void ValidateTreePositive2Test()
        {
            var tree = new HashSet<Edge<int>>() { (0, 1) }.Tree((0, 1));
            tree.ValidateTree();
        }

        [TestMethod]
        public void ValidateTreeNegative1Test()
        {
            var edges = alteryxPaths.ToList();
            edges.Insert(1, (1, 1));
            var tree = edges.AsSet().Tree((0, 1));
            Assert.ThrowsException<CycleException<int>>(tree.ValidateTree);
            try
            {
                tree.ValidateTree();
            }
            catch (CycleException<int> e)
            {
                Assert.IsTrue(e.Edge.Equals((1, 1)));
            }
        }

        [TestMethod]
        public void ValidateTreeNegative2Test()
        {
            var edges = alteryxPaths.ToList();
            edges.Insert(1, (2, 0));
            var tree = edges.AsSet().Tree((0, 1));
            Assert.ThrowsException<CycleException<int>>(tree.ValidateTree);
            try
            {
                tree.ValidateTree();
            }
            catch (CycleException<int> e)
            {
                Assert.IsTrue(e.Edge.Equals((2, 0)));
            }
        }

        [TestMethod]
        public void ValidateEdgesTest()
        {
            alteryxPaths.ValidateEdges();
            var alteryxPathsChanged = alteryxPaths.ToList();
            alteryxPathsChanged[0] = (1, 1);
            Assert.ThrowsException<CycleException<int>>(alteryxPathsChanged.AsSet().
                ValidateEdges);

            alteryxPathsChanged[0] = (0, 1);
            alteryxPathsChanged[1] = (15, 2); // (1, 2), invalid source
            alteryxPathsChanged[6] = (16, 5); // (1, 5), invalid source
            Assert.ThrowsException<UnconnectedEdgeException<int>>(alteryxPathsChanged.AsSet().
                ValidateEdges);
        }
    }
}
