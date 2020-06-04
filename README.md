Abismus is a general offline data processing framework.

Abismus is 

- Offline: Transformations are executed in one pass from compiled code. Executing a set of transformations means executing your program one time.
  
- Typed: Transformations are defined as functions which are typed by C#'s type system. Compiling your nodeset-program prevents any kind of typing mistakes.

- General: Arbitrary functions can be run on node inputs and generate node outputs. Any function can be connected to any other function and there can be multiple inputs and outputs into and out of the system (forest model).

- Economical: Tries to allocate as little as possible.
