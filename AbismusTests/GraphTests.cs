using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Abismus.Tests.Graph
{
    using Abismus.Graph;
    using Abismus.Serialization;

    [TestClass]
    public class GraphTests
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        HashSet<Edge<IntSerializable>> sampleWorkflow, samplePaths;
#pragma warning restore CS8618

        [TestInitialize]
        public void Init()
        {
            #region Sample data

            // http://graphonline.ru/en/?graph=rnNIuHBgplgRqXlv
            sampleWorkflow = new HashSet<Edge<IntSerializable>>
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
            samplePaths = new HashSet<Edge<IntSerializable>>
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
        public void EdgeEquality1()
        {
            var edge1 = (1, 2);
            var edge2 = (1, 2);
            Assert.AreEqual(edge1, edge2);
            Assert.IsTrue(edge1.Equals(edge2));
        }

        [TestMethod]
        public void EdgeEquality2()
        {
            var edge1 = (1, 1);
            var edge2 = edge1;
            Assert.AreEqual(edge1, edge2);
            Assert.IsTrue(edge1.Equals(edge2));
        }

        [TestMethod]
        public void EdgeInequality1()
        {
            var edge1 = (1, 2);
            var edge2 = (1, 3);
            Assert.AreNotEqual(edge1, edge2);
            Assert.IsFalse(edge1.Equals(edge2));
        }

        [TestMethod]
        public void EdgeInequality2()
        {
            var edge1 = (1, 2);
            var edge2 = (2, 1);
            Assert.AreNotEqual(edge1, edge2);
            Assert.IsFalse(edge1.Equals(edge2));
        }

        [TestMethod]
        public void SampleData()
        {
            Assert.AreEqual(32, sampleWorkflow.Count);
        }

        [TestMethod]
        public void FindInitialEdges()
        {
            var edges = samplePaths.FindInitialEdges();
            Assert.AreEqual(2, edges.Count());
            //Assert.IsTrue(edges.Contains((1, 2)));
            //Assert.IsTrue(edges.Contains((6, 7)));
            Assert.IsTrue(edges.Contains((0, 1)));
            Assert.IsTrue(edges.Contains((7, 8)));
        }

        [TestMethod]
        public void FindTerminalEdges()
        {
            var edges = samplePaths.FindTerminalEdges();
            Assert.AreEqual(3, edges.Count());
            //Assert.IsTrue(edges.Contains((8, 9)));
            //Assert.IsTrue(edges.Contains((10, 12)));
            //Assert.IsTrue(edges.Contains((14, 15)));
            Assert.IsTrue(edges.Contains((5, 6)));
            Assert.IsTrue(edges.Contains((9, 11)));
            Assert.IsTrue(edges.Contains((13, 14)));
        }

        [TestMethod]
        public void FindForkEdges()
        {
            var edges = samplePaths.FindForkEdges();
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
        public void FindJoinEdges()
        {
            var edges = samplePaths.FindJoinEdges();
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
        //public void Walk()
        //{
        //    var trees = samplePaths.Walk();
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
        //public void TreePositions()
        //{
        //    var treePositions = samplePaths.TreePositions(0);
        //    Assert.IsTrue(treePositions.SequenceEqual(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }));
        //}

        //[TestMethod]
        //public void TreePositionsTest2()
        //{
        //    var treePositions = samplePaths.TreePositions(1);
        //    Assert.IsTrue(treePositions.SequenceEqual(new int[] { 1, 2, 3, 4, 5 }));
        //}

        //[TestMethod]
        //public void TreeByPositions()
        //{
        //    var tree = samplePaths.TreeByPositions(0);
        //    var edgeTree = samplePaths.Tree((0, 1));
        //    Assert.IsTrue(tree.SequenceEqual(edgeTree));
        //}

        [TestMethod]
        public void Tree1()
        {
            var tree = samplePaths.Tree((0, 1));
            Assert.IsTrue(tree.SequenceEqual(new Edge<IntSerializable>[] { 
                (0, 1), (1, 2), (2, 3), (3, 4), (4, 13), (13, 14), (1, 5), (5, 6) 
            }));
        }

        [TestMethod]
        public void Tree1b()
        {
            var tree = samplePaths.Tree((7, 8));
            Assert.IsTrue(tree.SequenceEqual(new Edge<IntSerializable>[] {
                (7, 8), (8, 5), (5, 6), (8, 9), (9, 10), (10, 12), (12, 13), (13, 14), (9, 11)
            }));
        }

        [TestMethod]
        public void Tree1c()
        {
            var tree = samplePaths.Tree((2, 3));
            Assert.IsTrue(tree.SequenceEqual(new Edge<IntSerializable>[] {
                (2, 3), (3, 4), (4, 13), (13, 14)
            }));
        }

        [TestMethod]
        public void Tree1d()
        {
            var tree = samplePaths.Tree((10, 9));
            Assert.IsTrue(tree.SequenceEqual(Array.Empty<Edge<IntSerializable>>()));
        }

        [TestMethod]
        public void TreeVertexes1()
        {
            var vert = samplePaths.TreeVertexes((0, 1));

            Assert.IsTrue(vert.SequenceEqual(new IntSerializable[] {
                0, 1, 2, 3, 4, 13, 14, 5, 6
            }));
        }

        [TestMethod]
        public void TreeVertexes1b()
        {
            var vert = samplePaths.TreeVertexes((7, 8));

            Assert.IsTrue(vert.SequenceEqual(new IntSerializable[] {
                7, 8, 5, 6, 9, 10, 12, 13, 14, 11
            }));
        }

        [TestMethod]
        public void TreeVertexes1c()
        {
            var vert = samplePaths.TreeVertexes((2, 3));

            Assert.IsTrue(vert.SequenceEqual(new IntSerializable[] {
                2, 3, 4, 13, 14
            }));
        }

        [TestMethod]
        public void TreeVertexes1d()
        {
            var vert = samplePaths.TreeVertexes((10, 9));

            Assert.IsTrue(vert.SequenceEqual(Array.Empty<IntSerializable>()));
        }

        [TestMethod]
        public void TreeVertexes2()
        {
            var hs = new HashSet<Edge<IntSerializable>>()
            {
                (1, 2),
                (2, 3)
            };
            var vert = hs.TreeVertexes(hs.First());
            Assert.IsTrue(vert.SequenceEqual(new IntSerializable[] {
                1, 2, 3
            }));
        }

        [TestMethod]
        public void TreeVertexes2b()
        {
            var hs = new HashSet<Edge<IntSerializable>>()
            {
                (1, 2),
                (2, 3)
            };
            _ = hs.TreeVertexes(hs.First());
            hs.Add((3, 4));
            var vert = hs.TreeVertexes(hs.First());
            Assert.IsTrue(vert.SequenceEqual(new IntSerializable[] {
                1, 2, 3, 4
            }));
        }

        [TestMethod]
        public void TreeVertexes2c()
        {
            var hs = new HashSet<Edge<IntSerializable>>()
            {
                (1, 2),
                (2, 3)
            };
            _ = hs.TreeVertexes(hs.First());
            hs.Add((3, 4));
            var vert = hs.TreeVertexes(hs.First()).ToArray();
            Assert.IsTrue(vert.SequenceEqual(new IntSerializable[] {
                1, 2, 3, 4
            }));
        }

        [TestMethod]
        public void Tree2()
        {
            var tree = samplePaths.Tree((1, 2));
            Assert.AreEqual(5, tree.Count());
            Assert.IsTrue(tree.SequenceEqual(new Edge<IntSerializable>[] { 
                (1, 2), (2, 3), (3, 4), (4, 13), (13, 14) 
            }));
        }

        [TestMethod]
        public void Tree3()
        {
            var tree = samplePaths.Tree((1, int.MaxValue));
            Assert.AreEqual(0, tree.Count());
        }

        [TestMethod]
        public void Tree4()
        {
            var tree = samplePaths.Tree((13, 14));
            Assert.AreEqual(1, tree.Count());
            Assert.IsTrue(tree.First().Equals((13, 14)));
        }

        [TestMethod]
        public void Tree5()
        {
            var hs = new HashSet<Edge<IntSerializable>>()
            {
                (1, 2),
                (2, 3)
            };
            var tree = hs.Tree((1, 2));
            Assert.IsTrue(tree.SequenceEqual(new Edge<IntSerializable>[] {
                (1, 2), (2, 3)
            }));
        }

        [TestMethod]
        public void Tree5b()
        {
            var hs = new HashSet<Edge<IntSerializable>>()
            {
                (1, 2),
                (2, 3)
            };
            var tree = hs.Tree((1, 2));
            hs.Add((3, 4));
            Assert.IsTrue(tree.SequenceEqual(new Edge<IntSerializable>[] {
                (1, 2), (2, 3), (3, 4)
            }));
        }

        [TestMethod]
        public void TreeCycle1()
        {
            var hs = new HashSet<Edge<IntSerializable>>()
            {
                (1, 2),
                (2, 1)
            };
            var tree = hs.Tree((1, 2));
            var treeEn = tree.GetEnumerator();
            treeEn.MoveNext();
            Assert.AreEqual((1, 2), treeEn.Current);
            // Next produces a crash
            //treeEn.MoveNext();
        }

        [TestMethod]
        public void TreeParents()
        {
            var tree = samplePaths.Tree((0, 1));
            var parents = tree.Select(e => e.Parent);
            Assert.IsTrue(parents.SequenceEqual(new Edge<IntSerializable>[] { 
                (0, 1), (1, 2), (2, 3), (3, 4), (4, 13), (0, 1), (1, 5) 
            }));
        }

        [TestMethod]
        public void WalkUp1()
        {
            var tree = samplePaths.Tree((0, 1));
            var walkUp = tree.Single(e => e.Equals((13, 14))).WalkUp();
            Assert.IsTrue(walkUp.SequenceEqual(new Edge<IntSerializable>[] { 
                (13, 14), (4, 13), (3, 4), (2, 3), (1, 2), (0, 1) 
            }));
        }

        [TestMethod]
        public void Overlap()
        {
            var trees = samplePaths.AllTrees();
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
            var sum = new HashSet<Edge<IntSerializable>>(tree1MinusOverlap.Union(tree2MinusOverlap).Union(overlap));

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
        public void WalkInitials()
        {
            // Test that the first edge in each tree returned by Walk() is one of the initial edges
            var initials = samplePaths.FindInitialEdges();
            var trees = samplePaths.AllTrees();
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
        public void InitialsPlusJoins()
        {
            // Test that the initial edge's trees plus the join edge's trees summed equal the original graph
            var trees = samplePaths.AllTrees();
            var joins = samplePaths.FindJoinEdges();
            // Walk from each join
            var joinTrees = joins.Select(j => samplePaths.Tree(j));
            // Intersect
            var overlap = trees.First().Intersect(trees.Skip(1).First());
            // Subtract
            var treesMinusOverlap = trees.Select(t => t.Except(overlap));
            // Sum
            var sum = new HashSet<Edge<IntSerializable>>(treesMinusOverlap.First().Union(treesMinusOverlap.Skip(1)
                .First()).Union(overlap));

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
        public void ValidateTreePositive1()
        {
            var tree = samplePaths.Tree((0, 1));
            tree.ValidateTree();
        }

        [TestMethod]
        public void ValidateTreePositive2()
        {
            var tree = new HashSet<Edge<IntSerializable>>() { (0, 1) }.Tree((0, 1));
            tree.ValidateTree();
        }

        [TestMethod]
        public void ValidateTreeNegative1()
        {
            var edges = samplePaths.ToList();
            edges.Insert(1, (1, 1));
            var tree = edges.AsSet().Tree((0, 1));
            Assert.ThrowsException<CycleException<IntSerializable>>(tree.ValidateTree);
            try
            {
                tree.ValidateTree();
            }
            catch (CycleException<IntSerializable> e)
            {
                Assert.IsTrue(e.Edge.Equals((1, 1)));
            }
        }

        [TestMethod]
        public void ValidateTreeNegative2()
        {
            var edges = samplePaths.ToList();
            edges.Insert(1, (2, 0));
            var tree = edges.AsSet().Tree((0, 1));
            Assert.ThrowsException<CycleException<IntSerializable>>(tree.ValidateTree);
            try
            {
                tree.ValidateTree();
            }
            catch (CycleException<IntSerializable> e)
            {
                Assert.IsTrue(e.Edge.Equals((2, 0)));
            }
        }

        [TestMethod]
        public void ValidateEdges()
        {
            samplePaths.ValidateEdges();
            var samplePathsChanged = samplePaths.ToList();
            samplePathsChanged[0] = (1, 1);
            Assert.ThrowsException<CycleException<IntSerializable>>(samplePathsChanged.AsSet().ValidateEdges);

            samplePathsChanged[0] = (0, 1);
            samplePathsChanged[1] = (15, 2); // (1, 2), invalid source
            samplePathsChanged[6] = (16, 5); // (1, 5), invalid source
            Assert.ThrowsException<UnconnectedEdgeException<IntSerializable>>(samplePathsChanged.AsSet().ValidateEdges);
        }

        [TestMethod]
        public void EdgeSerializeDeserialize()
        {
            var edge1 = new Edge<IntSerializable>(0, 1);
            var ser = edge1.Serialize();
            var edge1b = edge1.Deserializer.Deserialize(ser);
            Assert.IsTrue(edge1.Equals(edge1b));
        }

        [TestMethod]
        public void EdgeSetSerialize()
        {
            var edgeSet = new HashSet<Edge<IntSerializable>>(
                new Edge<IntSerializable>[] {
                    (0, 1), (1, 2)
                }
            );
            var ser = edgeSet.Serialize();
            Assert.IsTrue(
                XNode.DeepEquals(
                    new XElement("EdgeSet",
                        new XElement("Edge",
                            new XElement("Source",
                                new XAttribute("Value", "0")
                            ),
                            new XElement("Sink",
                                new XAttribute("Value", "1")
                            )
                        ),
                        new XElement("Edge",
                            new XElement("Source",
                                new XAttribute("Value", "1")
                            ),
                            new XElement("Sink",
                                new XAttribute("Value", "2")
                            )
                        )
                    ), ser
                )
            );
        }

        // TODO: Test that all (current and future) edge set operations bypass null edges
    }
}
