using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Abismus.Node.Tests
{
    using Abismus.Node;
    using Abismus.Graph;

    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void Nodes1()
        {
            var node1 = new Node((Dels.O<int>)Funcs.Fixed);
            var node2 = new Node((Dels.OFromO<int>)Funcs.Mirror);
            var node3 = new Node((Dels.OOFromO<int>)Funcs.Duplicate);
            var node4 = new Node((Dels.OFromOO<int>)Funcs.Mult);
            var hs = new HashSet<Edge<Node>>();
            hs.Add(new Edge<Node>(node1, node2));
            hs.Add(new Edge<Node>(node2, node3));
            hs.Add(new Edge<Node>(node3, node4));
            var tree = hs.Tree(hs.First());
            Assert.ThrowsException<WrongTypeFinalValueException>(() => tree.GetFinalValue<float>());
            int final = tree.GetFinalValue<int>();
            Assert.AreEqual(25, final);
        }

        [TestMethod]
        public void Nodes1F()
        {
            var node1 = new Node((DelsF.O<int>)FuncsF.Fixed);
            var node2 = new Node((DelsF.OFromO<int>)FuncsF.Mirror);
            var node3 = new Node((DelsF.OOFromO<int>)FuncsF.Duplicate);
            var node4 = new Node((DelsF.OFromOO<int>)FuncsF.Mult);
            var hs = new HashSet<Edge<Node>>();
            hs.Add(new Edge<Node>(node1, node2));
            hs.Add(new Edge<Node>(node2, node3));
            hs.Add(new Edge<Node>(node3, node4));
            var tree = hs.Tree(hs.First());
            var final = tree.GetFinalValueF();
            Assert.ThrowsException<WrongTypeFinalValueException>(() => final.As<float>());
            int finalAs = final.As<int>();
            Assert.AreEqual(25, finalAs);
            var node5 = new Node((DelsF.OOFromO<int>)FuncsF.Duplicate);
            hs.Add(new Edge<Node>(node4, node5));
            tree = hs.Tree(hs.First());
            var final2 = tree.GetFinalValueF();
            Assert.ThrowsException<WrongTypeFinalValueException>(() => final2.As<int>());
            Assert.ThrowsException<WrongTypeFinalValueException>(() => final2.AsTuple3<int>());
            var final2As = final2.AsTuple2<int>();
            Assert.AreEqual((25, 25), final2As);
        }
    }
}
