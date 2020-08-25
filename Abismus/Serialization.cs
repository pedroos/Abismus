using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Markup;
using System.Xml.Linq;

namespace Abismus.Serialization
{
    public interface ISerializable<T> : IEquatable<ISerializable<T>>
    {
        public XObject Serialize();
        public IDeserializer<T> Deserializer { get; }
    }

    public interface IDeserializer<T>
    {
        public T Deserialize(XObject ser);
    }

    public abstract class ToStringSerializable<T> : ISerializable<T>, ISerializable<ToStringSerializable<T>>
    {
        public T Value { get; }

        public ToStringSerializable(T value)
        {
            Value = value;
        }

        public XObject Serialize() =>
            new XAttribute("Value", Value!.ToString());

        public bool Equals([AllowNull] ISerializable<T> other) => other != null && 
            Value!.Equals(((ToStringSerializable<T>)other).Value);

        public bool Equals([AllowNull] ISerializable<ToStringSerializable<T>> other) => other != null && 
            Value!.Equals(((ToStringSerializable<T>)other).Value);

        //public static implicit operator ToStringSerializable<T>(T t) => new ToStringSerializable<T>(t);

        public abstract IDeserializer<T> Deserializer { get; }

        IDeserializer<ToStringSerializable<T>> ISerializable<ToStringSerializable<T>>.Deserializer => 
            (IDeserializer<ToStringSerializable<T>>)Deserializer;

        public override string ToString() => Value!.ToString()!;
    }

    public class IntSerializable : ToStringSerializable<int>, ISerializable<IntSerializable>, 
        IEquatable<IntSerializable>
    {
        public IntSerializable(int value) : base(value) { }

        public override IDeserializer<int> Deserializer => (IDeserializer<int>)IntSerializableDeserializer.Get;

        IDeserializer<IntSerializable> ISerializable<IntSerializable>.Deserializer => IntSerializableDeserializer.Get;

        public bool Equals([AllowNull] ISerializable<IntSerializable> other) => 
            ((ISerializable<ToStringSerializable<int>>)this).Equals(other);

        public bool Equals([AllowNull] IntSerializable other) => ((ISerializable<int>)this).Equals(other);

        public static implicit operator IntSerializable(int t) => new IntSerializable(t);
    }

    public class IntDeserializer : IDeserializer<int>
    {
        private IntDeserializer() { }
        public int Deserialize(XObject ser)
        {
            if (!(ser is XAttribute attr))
                throw new ArgumentException(nameof(ser));
            if (!(attr.Name == "Value"))
                throw new ArgumentException(nameof(ser));
            if (!int.TryParse(attr.Value, out int val))
                throw new ArgumentException(nameof(ser));
            return val;
        }

        public static IntDeserializer Get { get; } = new IntDeserializer();
    }

    public class IntSerializableDeserializer : IDeserializer<IntSerializable>
    {
        private IntSerializableDeserializer() { }
        public IntSerializable Deserialize(XObject ser)
        {
            if (!(ser is XElement el))
                throw new ArgumentException(nameof(ser));
            if (!el.Attributes().Any(a => a.Name == "Value"))
                throw new ArgumentException(nameof(ser));
            int val = IntDeserializer.Get.Deserialize(el.Attribute("Value"));
            return new IntSerializable(val);
        }
        public static IntSerializableDeserializer Get { get; } = new IntSerializableDeserializer();
    }
}
