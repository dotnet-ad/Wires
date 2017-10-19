namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Android.Text;
	using Android.Widget;

	public static partial class UIExtensions
	{
		#region Text property

		public static Binder<TSource, EditText> Text<TSource, TPropertyType>(this Binder<TSource, EditText> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, string, TextChangedEventArgs>(property, b => b.Text, nameof(EditText.TextChanged), converter);
		}

		#endregion
	}
}
