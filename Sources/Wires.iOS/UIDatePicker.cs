namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Foundation;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Image property

		public static Binder<TSource, UIDatePicker> Date<TSource, TPropertyType>(this Binder<TSource, UIDatePicker> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, NSDate> converter = null)
			where TSource : class
		{
			return binder.Property< TPropertyType, NSDate, EventArgs>(property, v => v.Date, nameof(UISlider.ValueChanged), converter);
		}

		#endregion
	}
}
