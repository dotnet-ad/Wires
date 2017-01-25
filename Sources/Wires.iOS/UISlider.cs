namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Value property

		public static Binder<TSource, UISlider> Value<TSource, TPropertyType>(this Binder<TSource,UISlider> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, double> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, double, EventArgs>(property, b => b.Value, nameof(UISlider.ValueChanged), converter);
		}

		#endregion

		#region MaxValue property

		public static Binder<TSource, UISlider> MaxValue<TSource, TPropertyType>(this Binder<TSource, UISlider> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, double> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, double, EventArgs>(property, b => b.MaxValue, nameof(UISlider.ValueChanged), converter);
		}

		#endregion

		#region MinValue property

		public static Binder<TSource, UISlider> MinValue<TSource, TPropertyType>(this Binder<TSource, UISlider> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, double> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, double, EventArgs>(property, b => b.MinValue, nameof(UISlider.ValueChanged), converter);
		}

		#endregion
	}
}
