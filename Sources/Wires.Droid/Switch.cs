namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Android.Widget;
	using Transmute;

	public static partial class UIExtensions
	{
		#region Checked property

		public static Binder<TSource, Switch> Checked<TSource, TPropertyType>(this Binder<TSource, Switch> binder, Expression<Func<TSource, TPropertyType>> property, ITwoWayConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, bool, CompoundButton.CheckedChangeEventArgs>(property, b => b.Checked, nameof(Switch.CheckedChange), converter);
		}

		#endregion
	}
}
