namespace Wires
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;

	public static class ReflectionExtensions
	{
		#region Finding methods

		public static MethodInfo FindMethod(this Type type, string name, params Type[] parameters)
		{
			var methods = type.GetRuntimeMethods();
			return methods.First(m => m.Name == name && m.GetParameters().Select(pi => pi.ParameterType).SequenceEqual(parameters));
		}

		#endregion

		#region Accessor expression for faster property updates

		public static Action<object, object> BuildSetExpression(this PropertyInfo property)
		{
			var method = property.SetMethod;

			var instance = Expression.Parameter(typeof(object), "instance");
			var value = Expression.Parameter(typeof(object));

			var call = Expression.Call(Expression.Convert(instance, method.DeclaringType),method,Expression.Convert(value, method.GetParameters()[0].ParameterType));
			var expr = Expression.Lambda<Action<object, object>>(call,instance,value);

			return expr.Compile();
		}

		public static Func<object, object> BuildGetExpression(this PropertyInfo property)
		{
			var method = property.GetMethod;

			var instance = Expression.Parameter(typeof(object), "instance");

			var call = Expression.Call(Expression.Convert(instance, method.DeclaringType), method);
			var expr = Expression.Lambda<Func<object, object>>(Expression.Convert(call, typeof(object)), instance);

			return expr.Compile();
		}

		#endregion

		#region Fast method calls

		public static Action<object,object, TEventArgs> BuildHandlerExpression<TEventArgs>(this MethodInfo info)
		{
			var instance = Expression.Parameter(typeof(object), "instance");
			var sender = Expression.Parameter(typeof(object), "sender");
			var args = Expression.Parameter(typeof(TEventArgs), "args");

			var call = Expression.Call(Expression.Convert(instance, info.DeclaringType), info, sender, args);
			var expr = Expression.Lambda<Action<object,object, TEventArgs>>(call, instance, sender, args);

			return expr.Compile();
		}

		#endregion
	}
}
