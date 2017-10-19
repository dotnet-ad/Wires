namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Text property

		public static Binder<TSource, UIViewController> Title<TSource, TPropertyType>(this Binder<TSource, UIViewController> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Title, converter);
		}

		#endregion
	}
}
