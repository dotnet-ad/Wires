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

		public static Action<TOwner, TPropertyType> BuildSetExpression<TOwner, TPropertyType>(this PropertyInfo property)
		{
			var method = property.SetMethod;

			var instance = Expression.Parameter(typeof(TOwner), "instance");
			var value = Expression.Parameter(typeof(TPropertyType));

			var call = Expression.Call(Expression.Convert(instance, method.DeclaringType),method,Expression.Convert(value, method.GetParameters()[0].ParameterType));
			var expr = Expression.Lambda<Action<TOwner, TPropertyType>>(call,instance,value);

			return expr.Compile();
		}

		public static Func<TOwner, TPropertyType> BuildGetExpression<TOwner, TPropertyType>(this PropertyInfo property)
		{
			var method = property.GetMethod;

			var instance = Expression.Parameter(typeof(TOwner), "instance");

			var call = Expression.Call(Expression.Convert(instance, method.DeclaringType), method);
			var expr = Expression.Lambda<Func<TOwner, TPropertyType>>(Expression.Convert(call, typeof(TPropertyType)), instance);

			return expr.Compile();
		}

		public static Tuple<Func<TOwner, TPropertyType>, Action<TOwner, TPropertyType>> BuildAccessors<TOwner, TPropertyType>(this object owner, string property)
		{
			var targetPropertyInfo = owner.GetType().GetRuntimeProperty(property);

			if (targetPropertyInfo.PropertyType != typeof(TPropertyType))
				throw new ArgumentException($"The given target property should be of type {typeof(TPropertyType)}", nameof(property));

			var targetGetter = targetPropertyInfo.BuildGetExpression<TOwner, TPropertyType>();
			var targetSetter = targetPropertyInfo.BuildSetExpression<TOwner, TPropertyType>();

			return new Tuple<Func<TOwner, TPropertyType>, Action<TOwner, TPropertyType>>(targetGetter, targetSetter);
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
