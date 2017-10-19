namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Image property

		public static IBinding Progress<TSource, TPropertyType>(this Binder<TSource, UIProgressView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, float> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Progress, converter);
		}

		#endregion
	}
}
