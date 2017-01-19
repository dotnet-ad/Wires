namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Value property

		public static Binder<TSource, UIStepper> Value<TSource, TPropertyType>(this Binder<TSource, UIStepper> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, double> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, double, EventArgs>(property, b => b.Value, nameof(UIStepper.ValueChanged), converter);
		}

		#endregion

	}
}
