namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Android.Widget;
	using Transmute;

	public static partial class UIExtensions
	{
		#region Text property

		public static Binder<TSource, TextView> Text<TSource, TPropertyType>(this Binder<TSource, TextView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Text, converter);
		}

		#endregion
	}
}
