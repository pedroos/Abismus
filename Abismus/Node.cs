using Abismus.Graph;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Abismus.Node
{
    public class Node : IComparable<Node>
    {
        public object Fun { get; }
        public Node(object fun)
        {
            Fun = fun;
        }
        public int CompareTo(Node other)
        {
            throw new NotImplementedException();
        }
    }
}
