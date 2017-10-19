namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Transmute;
	using UIKit;

	public static partial class UIExtensions
	{
		#region IsAnimating property

		public static Binder<TSource, UIActivityIndicatorView> IsAnimating<TSource, TPropertyType>(this Binder<TSource, UIActivityIndicatorView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.IsAnimating, (b, v) => { if (v) b.StartAnimating(); else b.StopAnimating(); }, converter);
		}

		#endregion

		#region Color property

		public static Binder<TSource, UIActivityIndicatorView> Color<TSource, TPropertyType>(this Binder<TSource, UIActivityIndicatorView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, UIColor> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Color, converter);
		}

		#endregion
	}
}
