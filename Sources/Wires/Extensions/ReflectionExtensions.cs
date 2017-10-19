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
			if (property?.SetMethod == null)
				return (owner,v) => { throw new InvalidOperationException("No available setter"); };
			
			var method = property.SetMethod;

			var instance = Expression.Parameter(typeof(TOwner), "instance");
			var value = Expression.Parameter(typeof(TPropertyType));

			var call = Expression.Call(Expression.Convert(instance, method.DeclaringType),method,Expression.Convert(value, method.GetParameters()[0].ParameterType));
			var expr = Expression.Lambda<Action<TOwner, TPropertyType>>(call,instance,value);

			return expr.Compile();
		}

		public static Func<TOwner, TPropertyType> BuildGetExpression<TOwner, TPropertyType>(this PropertyInfo property)
		{

			var instance = Expression.Parameter(typeof(TOwner), "instance");

			Expression<Func<TOwner, TPropertyType>> expr;

			// if no property info, then its the owner itself
			if (property == null)
			{
				expr = Expression.Lambda<Func<TOwner, TPropertyType>>(Expression.Convert(instance, typeof(TPropertyType)), instance);
			}
			else
			{
				if (property.GetMethod == null)
					return (owner) => { throw new InvalidOperationException("No available getter"); };

				var method = property.GetMethod;

				var call = Expression.Call(Expression.Convert(instance, method.DeclaringType), method);
				expr = Expression.Lambda<Func<TOwner, TPropertyType>>(Expression.Convert(call, typeof(TPropertyType)), instance);
			}

			return expr.Compile();
		}

		private static PropertyInfo GetInfo<TOwner, TPropertyType>(this Expression<Func<TOwner, TPropertyType>> property)
		{
			if (property.Body is UnaryExpression)
			{
				var unary = (UnaryExpression)property.Body;
				if (unary.Operand is MemberExpression)
				{
					var unaryMember = (MemberExpression)unary.Operand;
					return (PropertyInfo)unaryMember.Member;
				}
				throw new ArgumentException();
			}

			var member = property.Body as MemberExpression;
			if (member == null)
				return null;

			return (PropertyInfo)member.Member;
		}

		public static Tuple<Func<TOwner, TPropertyType>, Action<TOwner, TPropertyType>,string> BuildAccessors<TOwner, TPropertyType>(this Expression<Func<TOwner,TPropertyType>> property)
		{
			return property.GetInfo().BuildAccessors<TOwner, TPropertyType>();
		}

		public static Tuple<Func<TOwner, TPropertyType>, Action<TOwner, TPropertyType>, string> BuildAccessors<TOwner, TPropertyType>(this PropertyInfo property)
		{
			var getter = property.BuildGetExpression<TOwner, TPropertyType>();
			var setter = property.BuildSetExpression<TOwner, TPropertyType>();

			return new Tuple<Func<TOwner, TPropertyType>, Action<TOwner, TPropertyType>,string>(getter, setter, property?.Name ?? "__UNKNOWN__");
		}

		public static string GetPropertyName<TOwner, TPropertyType>(this Expression<Func<TOwner, TPropertyType>> property) => property.GetInfo().Name;

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
