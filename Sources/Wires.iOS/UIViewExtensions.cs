namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Hidden property

		public static IBinding Visible<TSource, TPropertyType>(this Binder<TSource,UIView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			converter = converter ?? Converters.Default<TPropertyType, bool>();
			return binder.Hidden(property, converter.Chain(Converters.Invert));
		}

		public static IBinding Hidden<TSource, TPropertyType>(this Binder<TSource,UIView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Hidden, converter);
		}

		#endregion

	}
}
