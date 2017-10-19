namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Transmute;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Value property

		public static Binder<TSource, UIStepper> Value<TSource, TPropertyType>(this Binder<TSource, UIStepper> binder, Expression<Func<TSource, TPropertyType>> property, TwoWayConverter<TPropertyType, double> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, double, EventArgs>(property, b => b.Value, nameof(UIStepper.ValueChanged), converter);
		}

		#endregion

		#region MaximumValue property

		public static Binder<TSource, UIStepper> MaximumValue<TSource, TPropertyType>(this Binder<TSource, UIStepper> binder, Expression<Func<TSource, TPropertyType>> property, TwoWayConverter<TPropertyType, double> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, double, EventArgs>(property, b => b.MaximumValue, nameof(UIStepper.ValueChanged), converter);
		}

		#endregion

		#region MinimumValue property

		public static Binder<TSource, UIStepper> MinimumValue<TSource, TPropertyType>(this Binder<TSource, UIStepper> binder, Expression<Func<TSource, TPropertyType>> property, TwoWayConverter<TPropertyType, double> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, double, EventArgs>(property, b => b.MinimumValue, nameof(UIStepper.ValueChanged), converter);
		}

		#endregion

	}
}
