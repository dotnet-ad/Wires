namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Image property

		public static IBinding Image<TSource, TPropertyType>(this Binder<TSource,UIImageView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, UIImage> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Image, converter);
		}

		#endregion
	}
}
