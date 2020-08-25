using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Abismus.Tests.Signature
{
    using Abismus.Signature;

    [TestClass]
    public class SpaceTests
    {
        [TestMethod]
        public void EnumerateTest1()
        {
            var types = new List<Type>() {
                typeof(bool),
                typeof(DateTime),
                typeof(string),
                typeof(decimal?),
                typeof(DateTime?)
            };
            var prop1 = new Property<Type>("TypeProperty", (Type t) => t.FullName ?? "(Unnamed type)", types);
            var par1 = new SpacePartition<Type>(50, prop1, PropIdMaps.PositionPropIdMap);
            var en = par1.Enumerate(1);
            Assert.IsTrue(en.SequenceEqual(new List<(ulong, Type)>
            {
                (1, typeof(bool)),
                (2, typeof(DateTime)),
                (3, typeof(string)),
                (4, typeof(decimal?)),
                (5, typeof(DateTime?))
            }));
        }

        [TestMethod]
        public void EnumerateTest2()
        {
            var types = new List<Type>() {
                typeof(bool),
                typeof(DateTime),
                typeof(string),
                typeof(decimal?),
                typeof(DateTime?)
            };
            var prop1 = new Property<Type>("TypeProperty", (Type t) => t.FullName ?? "(Unnamed type)", types);
            var par1 = new SpacePartition<Type>(50, prop1, PropIdMaps.PositionPropIdMap);
            var en = par1.Enumerate(2);
            Assert.IsTrue(en.SequenceEqual(new List<(ulong, Type)>
            {
                (26, typeof(bool)),
                (27, typeof(DateTime)),
                (28, typeof(string)),
                (29, typeof(decimal?)),
                (30, typeof(DateTime?))
            }));
        }

        [TestMethod]
        public void EnumerateTest3()
        {
            var types = new List<Type>() {
                typeof(bool),
                typeof(DateTime),
                typeof(string),
                typeof(decimal?),
                typeof(DateTime?)
            };
            var prop1 = new Property<Type>("TypeProperty", (Type t) => t.FullName ?? "(Unnamed type)", types);
            var par1 = new SpacePartition<Type>(50, prop1, PropIdMaps.PositionPropIdMap);
            var enObj1 = ((ISpacePartition)par1).Enumerate(1);
            var enObj2 = par1.EnumerateObject(1);
            Assert.IsTrue(enObj1.GetType() == enObj2.GetType());
            Assert.IsTrue(enObj1.SequenceEqual(enObj2));
        }

        [TestMethod]
        public void EnumerateSpaceTest1()
        {
            var types = new List<Type>() {
                typeof(bool),
                typeof(DateTime),
                typeof(string),
                typeof(decimal?),
                typeof(DateTime?)
            };
            var prop1 = new Property<Type>("TypeProperty1", (Type t) => t.FullName ?? "(Unnamed type)", types);
            var par1 = new SpacePartition<Type>(50, prop1, PropIdMaps.PositionPropIdMap);
            var floats = new List<float>()
            {
                3.2f,
                4.45f,
                6.192f,
                2.1f,
                3.32f
            };
            var prop2 = new Property<float>("FloatProperty1", (float t) => t.ToString(), floats);
            var par2 = new SpacePartition<float>(50, prop2, PropIdMaps.PositionPropIdMap);
            var prop3 = new Property<Type>("TypeProperty2", (Type t) => t.FullName ?? "(Unnamed type)", types);
            var par3 = new SpacePartition<Type>(40, prop3, PropIdMaps.PositionPropIdMap);
            var prop4 = new Property<float>("FloatProperty2", (float t) => t.ToString(), floats);
            var par4 = new SpacePartition<float>(30, prop4, PropIdMaps.PositionPropIdMap);
            var prop5 = new Property<Type>("TypeProperty3", (Type t) => t.FullName ?? "(Unnamed type)", types);
            var par5 = new SpacePartition<Type>(25, prop5, PropIdMaps.PositionPropIdMap);
            var space = new Space()
                .WithPartition(par1)
                .WithPartition(par2)
                .WithPartition(par3)
                .WithPartition(par4)
                .WithPartition(par5);
            var en = space.Enumerate();
            // TODO: Indexes below are senseless. Needs partition sequencing.
            Assert.IsTrue(en.SequenceEqual(new List<(ulong, object)>
            {
                (1, typeof(bool)),
                (2, typeof(DateTime)),
                (3, typeof(string)),
                (4, typeof(decimal?)),
                (5, typeof(DateTime?)),
                (26, 3.2f),
                (27, 4.45f),
                (28, 6.192f),
                (29, 2.1f),
                (30, 3.32f),
                (41, typeof(bool)),
                (42, typeof(DateTime)),
                (43, typeof(string)),
                (44, typeof(decimal?)),
                (45, typeof(DateTime?)),
                (46, 3.2f),
                (47, 4.45f),
                (48, 6.192f),
                (49, 2.1f),
                (50, 3.32f),
                (49, typeof(bool)),
                (50, typeof(DateTime)),
                (51, typeof(string)),
                (52, typeof(decimal?)),
                (53, typeof(DateTime?)),
            }));
        }

        //TODO: Equality tests
    }
}
