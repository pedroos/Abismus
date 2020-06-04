Abismus is a general offline data processing framework.

Abismus is 

- Offline: Transformations are executed in one pass from compiled code. Executing a set of transformations means executing your program one time.
  
- Typed: Transformations are defined as functions which are typed by C#'s type system. Compiling your nodeset-program prevents any kind of typing mistakes and assures all connections are valid.

- General: Arbitrary functions can be run on node inputs and generate node outputs. Any function can be connected to any other function and there can be multiple inputs and outputs into and out of the system (forest model).

- Economical: Tries to allocate as little as possible.

Here is an example from the tests suite:

```
var node1 = new Node((Dels.O<int>)Funcs.Fixed);
var node2 = new Node((Dels.OFromO<int>)Funcs.Mirror);
var node3 = new Node((Dels.OOFromO<int>)Funcs.Duplicate);
var node4 = new Node((Dels.OFromOO<int>)Funcs.Mult);
var hs = new HashSet<Edge<Node>>();
hs.Add(new Edge<Node>(node1, node2));
hs.Add(new Edge<Node>(node2, node3));
hs.Add(new Edge<Node>(node3, node4));
var tree = hs.Tree(hs.First());
int final = tree.GetFinalValue<int>();
Assert.AreEqual(25, final);
```
