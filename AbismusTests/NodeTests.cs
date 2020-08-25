using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Abismus.Tests.Node
{
    using Abismus.Node;
    using Abismus.Graph;
    using Microsoft.VisualStudio.TestPlatform.Utilities;
    using Abismus.Signature;

    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void FuncsCurry1()
        {
            var curried = DelsF.Curry<int, int, int>(FuncsF.Minus, 7);
            curried(5, out int res);
            Assert.AreEqual(2, res);
            curried = DelsF.CurryI2<int, int, int>(FuncsF.Minus, 7);
            curried(5, out res);
            Assert.AreEqual(-2, res);
        }

        [TestMethod]
        public void GetFinalValue1()
        {
            var node1 = new Node((Dels.O<int>)Funcs.Fixed);
            var node2 = new Node((Dels.IO<int, int>)Funcs.Mirror);
            var node3 = new Node((Dels.IOO<int, int, int>)Funcs.Duplicate);
            var node4 = new Node((Dels.IIO<int, int, int>)Funcs.Mult);
            var hs = new HashSet<Edge<Node>>()
            {
                new Edge<Node>(node1, node2), 
                new Edge<Node>(node2, node3), 
                new Edge<Node>(node3, node4)
            };
            var tree = hs.Tree(hs.First());
            Assert.ThrowsException<WrongTypeFinalValueException>(() => tree.GetFinalValue<float>());
            int final = tree.GetFinalValue<int>();
            Assert.AreEqual(25, final);
        }

        [TestMethod]
        public void GetFinalValueDynamic1()
        {
            var node1 = new Node((DelsF.O<int>)FuncsF.Fixed);
            var node2 = new Node((DelsF.IO<int, int>)FuncsF.Mirror);
            var node3 = new Node((DelsF.IOO<int, int, int>)FuncsF.Duplicate);
            var node4 = new Node((DelsF.IIO<int, int, int>)FuncsF.Mult);
            var hs = new HashSet<Edge<Node>>()
            {
                new Edge<Node>(node1, node2), 
                new Edge<Node>(node2, node3), 
                new Edge<Node>(node3, node4)
            };
            var vert = hs.TreeVertexes(hs.First()).ToArray();
            var final = vert.GetFinalValueDynamic();
            //Assert.ThrowsException<WrongTypeFinalValueException>(() => final.As<float>());
            //int finalAs = final.As<int>();
            Assert.AreEqual(25, (int)final[0]);
        }

        [TestMethod]
        public void GetFinalValueDynamic2()
        {
            var node1 = new Node((DelsF.O<int>)FuncsF.Fixed);
            var node2 = new Node((DelsF.IO<int, int>)FuncsF.Mirror);
            var node3 = new Node((DelsF.IOO<int, int, int>)FuncsF.Duplicate);
            var node4 = new Node((DelsF.IIO<int, int, int>)FuncsF.Mult);
            var hs = new HashSet<Edge<Node>>()
            {
                new Edge<Node>(node1, node2), 
                new Edge<Node>(node2, node3), 
                new Edge<Node>(node3, node4)
            };
            var vert = hs.TreeVertexes(hs.First());
            var final = vert.GetFinalValueDynamic();
            var node5 = new Node((DelsF.IOO<int, int, int>)FuncsF.Duplicate);
            hs.Add(new Edge<Node>(node4, node5));

            vert = hs.TreeVertexes(hs.First());
            var final2 = vert.GetFinalValueDynamic();
            //Assert.ThrowsException<WrongTypeFinalValueException>(() => final2.As<int>());
            //Assert.ThrowsException<WrongTypeFinalValueException>(() => final2.AsTuple3<int>());
            //var final2As = final2.AsTuple2<int>();
            //Assert.AreEqual((25, 25), final2As);
            Assert.IsTrue(final2.SequenceEqual(new object[] { 25, 25 }));
        }

        //[TestMethod]
        //public void NodesCurry1F()
        //{
        //    var node1 = new Node(DelsF.Curry<int, int, int>(FuncsF.Mult, 3);
        //    var node1 = new Node(FuncsF.MirrorCurryA(4));
        //    var node2 = new Node(FuncsF.MultCurryA(3));
        //    var hs = new HashSet<Edge<Node>>();
        //    hs.Add(new Edge<Node>(node1, node2));
        //    var tree = hs.Tree(hs.First());
        //    int final = tree.GetFinalValue<int>();
        //    Assert.AreEqual(12, final);
        //}

        [TestMethod]
        public void Equality()
        {
            var node1 = new Node((DelsF.O<int>)FuncsF.Fixed);
            var node2 = new Node((DelsF.O<int>)FuncsF.Fixed);
            Assert.IsFalse(node1.Equals(node2));
            var node3 = new Node((DelsF.IO<int, int>)FuncsF.Mirror);
            Assert.IsFalse(node2.Equals(node3));
            Assert.IsTrue(node1.Equals(node1));
        }

        [TestMethod]
        public void SerializeDeserialize()
        {
            var node1 = new Node((DelsF.O<int>)FuncsF.Fixed);
            var ser = node1.Serialize();
            var node1b = node1.Deserializer.Deserialize(ser);
            Assert.IsTrue(node1.Fun.Equals(node1b.Fun));
        }

        [TestMethod]
        public void SerializeDeserializeCurried()
        {
            var node1 = new Node(DelsF.Curry<int, int>(FuncsF.Mirror, 6));
            var node2 = new Node(DelsF.Curry<int, int, int>(FuncsF.Mult, 3));
            var ser = node1.Serialize();
            var node1b = node1.Deserializer.Deserialize(ser);
            Assert.IsTrue(node1.Fun.Equals(node1b.Fun));
        }

        [TestMethod]
        public void Signature1()
        {
            var node1 = new Node((DelsF.O<int>)FuncsF.Fixed);
            var node2 = new Node((DelsF.IO<int, int>)FuncsF.Mirror);
            var node3 = new Node((DelsF.IOO<int, int, int>)FuncsF.Duplicate);
            var node4 = new Node((DelsF.IOO<int, int, int>)FuncsF.Negate);
            Assert.IsFalse(node1.FunSig.Equals(node2.FunSig));
            Assert.IsFalse(node2.FunSig.Equals(node3.FunSig));
            Assert.IsTrue(node3.FunSig.Equals(node4.FunSig));
        }
    }
}
