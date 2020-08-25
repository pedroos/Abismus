<Query Kind="Program" />

namespace MyProgram {

public static class Program {
	static void Main1()
	{
		var f1 = (IO<int, int>)MyProgram.Functions.AddOne;
		f1.Dump("AddOne priori delegate");
		f1.GetType().Dump("AddOne type");
		var addOne = typeof(MyProgram.Functions).GetMethod("AddOne");
		addOne.Dump("AddOne priori MethodInfo");
		
		var f1c = MyProgram.Functions.Curry<int, int>(MyProgram.Functions.AddOne, 3);
		f1c.Dump("Curry priori delegate");
		f1c.GetType().Dump("Curry type");
		
		Type.GetType("MyProgram.Functions").GetMethod("AddOne").Dump("AddOne posteriori MethodInfo");
		
		var methods = Type.GetType("MyProgram.Functions").GetMethods();
		var curries = methods.Where(m => m.Name == "Curry");
		curries.First().Dump("First curry");
		
		curries.First().GetGenericArguments().Dump("First curry generic arguments");
		
		// Late bound operations cannot be performed on types or methods for which ContainsGenericParameters is true.
		try {
			curries.First().Invoke(null, new object[] {(IO<int, int>)MyProgram.Functions.AddOne, 3});
		}
		catch (InvalidOperationException) {}
		
		var curryMet = curries.First().MakeGenericMethod(typeof(int), typeof(int))
			.Invoke(null, new object[] {(IO<int, int>)MyProgram.Functions.AddOne, 3});
		curryMet.GetType().Dump("Substituted first curry type");
		curryMet.Dump("Substituted first curry delegate");
		
		var curryMetDel = (O<int>)curryMet;
		curryMetDel(out int res);
		res.Dump("Result");
	}
	
	static void Main() {
		//Expression<IO<int, int>> addOneExpr = (IO<int, int>)MyProgram.Functions.AddOne; // doesn't work
		O<int> addOneCurried = (out int b) => Functions.AddOne(4, out b);
		//Expression<O<int>> addOneCurriedExpr = (out int b) => Functions.AddOne(4, out b); // An expression tree lambda may not 
			// contain a ref, in or out parameter
		Expression<O2<int>> addOneCurriedExpr = () => Functions.AddOne2(4);
		addOneCurriedExpr.Dump("Curried expression");
		
		addOneCurriedExpr.ToString().Dump("Curried expression string");
		
		//System.Linq.Expressions.InvocationExpression invocationExpression =
		//    System.Linq.Expressions.Expression.Invoke(
		//        addOneCurriedExpr,
		//        System.Linq.Expressions.Expression.Constant(539),
		//        System.Linq.Expressions.Expression.Constant(281));
		
		//Console.WriteLine(invocationExpression.ToString());
	}
}

public delegate TO1 O2<TO1>();

public delegate void O<TO1>(out TO1 out1);
public delegate void IO<TI1, TO1>(TI1 in1, out TO1 out1);
public delegate void IIO<TI1, TI2, TO1>(TI1 in1, TI2 in2, out TO1 out1);

public static class Functions {
    public static void AddOne(int a, out int b) {
	    b = a + 1;
	}
	
    public static int AddOne2(int a) {
	    return a + 1;
	}
	
	public static O<TO1> Curry<TI1, TO1>(IO<TI1, TO1> f, TI1 in1) => 
		(out TO1 out1) => f(in1, out out1);
		
    public static O<TO1> Curry<TI1, TI2, TO1>(IIO<TI1, TI2, TO1> f, TI1 in1, TI2 in2) =>
        (out TO1 out1) => f(in1, in2, out out1);
}

}