namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Text property

		public static IBinding Text<TSource, TPropertyType>(this Binder<TSource, UILabel> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Text, converter);
		}

		#endregion

		#region TextColor property

		public static IBinding TextColor<TSource, TPropertyType>(this Binder<TSource,UILabel> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, UIColor> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.TextColor, converter);
		}

		#endregion
	}
}