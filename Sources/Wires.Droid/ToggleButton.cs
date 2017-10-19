namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Android.Widget;
	using Transmute;

	public static partial class UIExtensions
	{
		#region Checked property

		public static Binder<TSource, ToggleButton> Checked<TSource, TPropertyType>(this Binder<TSource, ToggleButton> binder, Expression<Func<TSource, TPropertyType>> property, ITwoWayConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, bool, CompoundButton.CheckedChangeEventArgs>(property, b => b.Checked, nameof(ToggleButton.CheckedChange), converter);
		}

		#endregion

		#region Text property

		public static Binder<TSource, ToggleButton> Text<TSource, TPropertyType>(this Binder<TSource, ToggleButton> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Text, converter);
		}

		#endregion
	}
}