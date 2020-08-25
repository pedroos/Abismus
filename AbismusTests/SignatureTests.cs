using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Abismus.Tests.Signature
{
    using Abismus.Signature;
    using Abismus.Node;

    [TestClass]
    public class SignatureTests
    {
        [TestMethod]
        public void SignaturesTest()
        {
            Assert.IsTrue(Signatures.PropIds.Take(5).SequenceEqual(new ulong[] { 1, 2, 3, 4, 5 }));
            Assert.IsTrue(Signatures.PropIds.Skip(Signatures.PropIds.Length - 5).Take(5)
                .SequenceEqual(new ulong[] { 46, 47, 48, 49, 50 }));
            Assert.IsTrue(Signatures.InputTypesValues.Keys.Take(6).SequenceEqual(Signatures.Types.Take(6)));
            Assert.IsTrue(Signatures.OutputTypesValues.Keys.Take(6).SequenceEqual(Signatures.Types.Take(6)));
            Assert.IsTrue(Signatures.InputTypesValues.Values.Take(6).SequenceEqual(new ulong[] { 1, 2, 3, 4, 5, 6 }));
            Assert.IsTrue(Signatures.OutputTypesValues.Values.Take(6).SequenceEqual(new ulong[] { 26, 27, 28, 29, 30, 31 }));
        }

        class SomeClass { }

        [TestMethod]
        public void MakeListTest()
        {
            var types = new Type[] { typeof(int), typeof(float), typeof(double) };
            var types2 = new Type[] { typeof(decimal), typeof(string), typeof(DateTime) };

            var fv = Signatures.MakeList(types, types2);
            Assert.IsTrue(fv.SequenceEqual(new ulong[] { 2, 3, 4, 30, 32, 31 }));
            Assert.ThrowsException<TypeUnmappedException>(() => Signatures.MakeList(new Type[] { typeof(SomeClass) }, 
                Array.Empty<Type>()));
            fv = Signatures.MakeList(Array.Empty<Type>(), new Type[] { typeof(DateTime?) });
            Assert.IsTrue(fv.SequenceEqual(new ulong[] { 38 }));
        }

        [TestMethod]
        public void SignatureTest()
        {
            var del1 = new Dels.O<int>(() => 1);
            var sig1 = new Signature(del1);
            Assert.IsTrue(sig1.Ins.Count() == 0);
            Assert.IsTrue(sig1.Outs.Count() == 1);
            Assert.IsTrue(sig1.Outs.SequenceEqual(new Type[] { typeof(int) }));

            var del2 = new DelsF.IIOO<int, float, decimal, DateTime>((int i1, float i2, out decimal o1, out DateTime o2) => {
                o1 = default; o2 = default;
            });
            var sig2 = new Signature(del2);
            Assert.IsTrue(sig2.Ins.Count() == 2);
            Assert.IsTrue(sig2.Ins.SequenceEqual(new Type[] { typeof(int), typeof(float) }));
            Assert.IsTrue(sig2.Outs.Count() == 2);
            Assert.IsTrue(sig2.Outs.SequenceEqual(new Type[] { typeof(decimal), typeof(DateTime) }));
        }

        [TestMethod]
        public void SignatureEqualsTest()
        {
            Assert.IsTrue(
                new Signature(new Dels.O<int>(() => 1))
                .Equals(new Signature(new Dels.O<int>(() => 2))));
            Assert.IsTrue(
                new Signature(new Dels.O<int>(() => 1))
                .Equals(new Signature(new DelsF.O<int>((out int out1) => out1 = 2))));
            Assert.IsFalse(
                new Signature(new Dels.O<int>(() => 1))
                .Equals(new Signature(new Dels.O<double>(() => 1.0))));
            Assert.IsFalse(
                new Signature(new Dels.O<int>(() => 1))
                .Equals(new Signature(new Dels.IO<int, int>((int in1) => in1))));
        }

        [TestMethod]
        public void RecoverDelegateSignatureTest()
        {
            var fun = new Dels.O<int>(() => 1);
            object funO = fun;
            Assert.IsTrue(new Signature(fun).Equals(new Signature((System.Delegate)funO)));
            Assert.IsTrue(new Signature(fun).Equals(new Signature((System.Delegate)funO)));
        }
    }
}
