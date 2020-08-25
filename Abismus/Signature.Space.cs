using Abismus.Node;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Abismus.Signature
{
    public interface IProperty : IEquatable<IProperty>
    {
        public string Name { get; }
        public Type Type { get; }
    }

    public class Property<T> : IProperty
    {
        public string Name { get; }
        public Type Type { get; }
        public Func<T, string> ValueString { get; }
        public IEnumerable<T> Values { get; }

        public Property(string name, Func<T, string> valueString, IEnumerable<T> values)
        {
            Name = name;
            Type = GetType().GenericTypeArguments.First();
            ValueString = valueString;
            Values = values;
        }

        public bool Equals(IProperty? other) => other != null && other.Name == Name;
    }

    public interface ISpacePartition : IEquatable<ISpacePartition>
    {
        public int Size { get; }
        public IProperty Property { get; }
        public Func<int, ulong> PropertyIdMap { get; }

        // For internal Space use
        internal IEnumerable<(ulong PropId, object PropValue)> Enumerate(int partitionNumber);
    }

    public class SpacePartition<TProp> : ISpacePartition, IEquatable<SpacePartition<TProp>>, IEquatable<ISpacePartition>
        where TProp : notnull
    {
        public int Size { get; }
        public Property<TProp> Property { get; }
        public Func<int, ulong> PropertyIdMap { get; }
        IProperty ISpacePartition.Property => Property;

        public SpacePartition(int size, Property<TProp> property, Func<int, ulong> propertyIdMap)
        {
            Size = size;
            Property = property;
            PropertyIdMap = propertyIdMap;
        }

        public IEnumerable<(ulong PropId, TProp PropValue)> Enumerate(int partitionNumberOneBased)
        {
            int sizeFloorHalf = Size.FloorHalf();
            int propSpacePos = sizeFloorHalf * (partitionNumberOneBased - 1);
            for (int j = 0; j < Property.Values.Count(); ++j)
            {
                //if (Property.Values.ElementAt(j) == default) continue;
                ulong propId = PropertyIdMap(j + propSpacePos);
                yield return (propId, Property.Values.ElementAt(j));
            }
        }

        // For internal Space use
        IEnumerable<(ulong PropId, object PropValue)> ISpacePartition.Enumerate(int partitionNumberOneBased) => 
            Enumerate(partitionNumberOneBased).CastElementsContravariant<ulong, TProp, ulong, object>().ToArray();

        public IEnumerable<(ulong PropId, object PropValue)> EnumerateObject(int partitionNumberOneBased) => 
            ((ISpacePartition)this).Enumerate(partitionNumberOneBased);

        public IEnumerable<(ulong PropId, TPropAny PropValue)> Enumerate<TPropAny>(int partitionNumberOneBased) 
            where TPropAny : TProp => Enumerate(partitionNumberOneBased).CastElementsCovariant<ulong, TProp, ulong, TPropAny>()
            .ToArray();

        public bool Equals(SpacePartition<TProp>? other) => other != null && ((ISpacePartition)this).Equals(other);

        public bool Equals(ISpacePartition? other) => other != null && 
            other.Property.Equals(Property) && other.Size == Size && other.PropertyIdMap.Equals(PropertyIdMap);
            // TODO: Check Func equality

        public override string ToString() => string.Format("Partition, size {0}, property: {1} ({2})", Size, Property.Name, 
            Property.Type.Name);
    }

    public class Space : IEquatable<Space>
    {
        readonly List<ISpacePartition> partitions;

        public Space()
        {
            partitions = new List<ISpacePartition>();
        }

        public Space WithPartition<TProp>(SpacePartition<TProp> partition)
            where TProp : notnull
        {
            partitions.Add(partition);
            return this;
        }

        public IEnumerable<(ulong PropId, object PropValue)> 
            Enumerate() => partitions.SelectMany((ISpacePartition p, int i) => p.Enumerate(i + 1));

        public void WriteTo(Stream stream)
        {
            using var writer = new StreamWriter(stream, Encoding.UTF8);
            var pars = partitions.Select((ISpacePartition p, int i) => (Label: string.Format("# ({0}) {1}", i + 1, p), 
                Items: p.Enumerate(i + 1)));
            foreach (var (label, items) in pars)
            {
                writer.WriteLine(label + Environment.NewLine);
                foreach (var (propId, propValue) in items) 
                    writer.WriteLine(string.Format("{0} {1}", propId, propValue));
                writer.WriteLine();
            }
        }

        public bool Equals(Space? other) => other != null && 
            other.partitions.Count == partitions.Count && other.partitions.SequenceEqual(partitions);
    }

    public static class PropIdMaps
    {
        public static ulong PositionPropIdMap(int zeroBasedPosition) => (ulong)zeroBasedPosition + 1;

        public static ulong PowersOfTwoPropIdMap(int position) => (ulong)Math.Pow(2, position);
    }

    public static partial class Extensions
    {
        public static int FloorHalf(this int n) => (int)Math.Floor(n / (float)2);

        public static IEnumerable<(TTo1, TTo2)> CastElementsCovariant<TFrom1, TFrom2, TTo1, TTo2>
            (this IEnumerable<(TFrom1, TFrom2)> en) 
            where TTo1 : TFrom1
            where TTo2 : TFrom2 => en.Select(e => ((TTo1)e.Item1, (TTo2)e.Item2));

        public static IEnumerable<(TTo1, TTo2)> CastElementsContravariant<TFrom1, TFrom2, TTo1, TTo2>
            (this IEnumerable<(TFrom1, TFrom2)> en)
            where TFrom1 : TTo1
            where TFrom2 : TTo2 => en.Select(e => ((TTo1)e.Item1, (TTo2)e.Item2));
    }
}
