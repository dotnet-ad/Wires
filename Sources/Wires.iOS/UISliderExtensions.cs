namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Image property

		public static IBinding Value<TSource, TPropertyType>(this Binder<TSource,UISlider> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, double> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, double, EventArgs>(property, b => b.Value, nameof(UISlider.ValueChanged), converter);
		}

		#endregion
	}
}
