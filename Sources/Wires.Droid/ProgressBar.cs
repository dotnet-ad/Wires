namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Android.Widget;
	using Transmute;

	public static partial class UIExtensions
	{
		#region Progress property

		public static Binder<TSource, ProgressBar> Progress<TSource, TPropertyType>(this Binder<TSource, ProgressBar> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, int> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Progress, converter);
		}

		#endregion
	}
}