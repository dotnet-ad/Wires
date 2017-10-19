namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Text property

		public static Binder<TSource, UISwitch> On<TSource, TPropertyType>(this Binder<TSource, UISwitch> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, bool, EventArgs>(property, b => b.On, nameof(UISwitch.ValueChanged), converter);
		}

		public static Binder<TSource, UISwitch> Off<TSource, TPropertyType>(this Binder<TSource, UISwitch> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			converter = converter ?? Converters.Default<TPropertyType, bool>();
			return binder.Off(property, converter.Chain(Converters.Invert));
		}

		#endregion
	}
}
