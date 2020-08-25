using Abismus.Graph;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace Abismus.Node
{
    using Abismus.Signature;
    using Abismus.Serialization;

    public class Node : ISerializable<Node>, IEquatable<Node>
    {
        public Delegate Fun { get; }
        public MethodInfo FunMethodInfo { get; }
        readonly string funMethodInfoName;

        readonly Lazy<Signature> funSig;
        public Signature FunSig { get { return funSig.Value; } }

        public Node(Delegate fun)
        {
            Fun = fun;

            FunMethodInfo = fun.GetMethodInfo()!;
            funMethodInfoName = FunMethodInfo.Name;

            funSig = new Lazy<Signature>(() => new Signature(Fun));
        }

        public XObject Serialize() =>
            new XElement("Node",
                new XAttribute("FunctionTypeName", FunMethodInfo.DeclaringType!.AssemblyQualifiedName),
                new XAttribute("FunctionName", FunMethodInfo.Name),
                //new XAttribute("FunctionTypeName", FunMethodInfo.DeclaringType.AssemblyQualifiedName), 
                new XAttribute("DelegateTypeName", Fun.GetType().AssemblyQualifiedName));

        public IDeserializer<Node> Deserializer => NodeDeserializer.Get;

        public bool Equals([AllowNull] Node other) => other != null && ((object)other).Equals(this);

        public bool Equals([AllowNull] ISerializable<Node> other) => Equals(other);

        public override string ToString()
        {
            return funMethodInfoName;
        }
    }

    public class NodeDeserializer : IDeserializer<Node>
    {
        private NodeDeserializer() { }
        public Node Deserialize(XObject ser)
        {
            if (!(ser is XElement el))
                throw new ArgumentException(nameof(ser));

            string functionTypeName = el.Attribute("FunctionTypeName").Value;
            //string functionName = el.Attribute("FunctionName").Value;
            string delegateTypeName = el.Attribute("DelegateTypeName").Value;

            var functionType = Type.GetType(functionTypeName)!;
            //var function = functionType.GetMethod(functionName, BindingFlags.Public | BindingFlags.Static)!;
            var delegateType = Type.GetType(delegateTypeName)!;
            //var @delegate = delegateType.GetMember(delegateName, BindingFlags.Public | BindingFlags.Static);

            //var del = functionType.GetMethod().CreateDelegate(@delegateType);

            //return new Node(del);
            return default;
        }
        public static NodeDeserializer Get { get; } = new NodeDeserializer();
    }
}
