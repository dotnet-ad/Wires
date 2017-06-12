namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Android.Widget;

	public static partial class UIExtensions
	{
		#region Checked property

		public static Binder<TSource, RadioButton> Checked<TSource, TPropertyType>(this Binder<TSource, RadioButton> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, bool, CompoundButton.CheckedChangeEventArgs>(property, b => b.Checked, nameof(RadioButton.CheckedChange), converter);
		}

		#endregion
	}
}