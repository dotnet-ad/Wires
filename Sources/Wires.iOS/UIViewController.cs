namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Transmute;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Title property

		public static Binder<TSource, UIViewController> Title<TSource, TPropertyType>(this Binder<TSource, UIViewController> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Title, converter);
		}

		#endregion

		#region Back property

		public static Binder<TSource, UIViewController> BackTitle<TSource, TPropertyType>(this Binder<TSource, UIViewController> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.NavigationItem.BackBarButtonItem.Title, (b,v) => new UIBarButtonItem(v,UIBarButtonItemStyle.Plain, null) , converter);
		}

		#endregion
	}
}
