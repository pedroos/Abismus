using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace Abismus
{
    public class IncompatibleTypesException : Exception
    {
        protected Type TypeShould { get; }
        protected Type? TypeIs { get; }
        public IncompatibleTypesException(Type typeShould, Type? typeIs)
        {
            TypeShould = typeShould;
            TypeIs = typeIs;
        }
        public IncompatibleTypesException(Type typeShould) : this(typeShould, default) { }
        public override string ToString()
        {
            return string.Format("Incompatible types: {0} should be assignable from {1}", 
                TypeIs != default ? TypeIs.Name : "(indeterminate)", 
                TypeShould.Name);
        }
    }

    public class WrongTypeException : IncompatibleTypesException
    {
        public WrongTypeException(Type typeShould, Type? typeIs) : base(typeShould, typeIs) { }

        public WrongTypeException(Type typeShould) : base(typeShould) { }
        public override string ToString()
        {
            return string.Format("Wrong type: {0} should be {1}", 
                TypeIs != default ? TypeIs.Name : "(indeterminate)", 
                TypeShould.Name);
        }
    }
}
