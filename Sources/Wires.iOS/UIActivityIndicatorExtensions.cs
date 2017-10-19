namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Image property

		public static IBinding IsAnimating<TSource, TPropertyType>(this Binder<TSource, UIActivityIndicatorView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.IsAnimating, (b, v) => { if (v) b.StartAnimating(); else b.StopAnimating(); }, converter);
		}

		#endregion
	}
}
