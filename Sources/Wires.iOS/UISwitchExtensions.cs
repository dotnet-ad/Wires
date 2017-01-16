namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Text property

		public static IBinding On<TSource, TPropertyType>(this Binder<TSource, UISwitch> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, bool, EventArgs>(property, b => b.On, nameof(UISwitch.ValueChanged), converter);
		}

		#endregion
	}
}
