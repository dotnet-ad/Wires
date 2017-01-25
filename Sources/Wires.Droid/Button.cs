namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using System.Windows.Input;
	using Android.Widget;

	public static partial class UIExtensions
	{
		#region TouchUpInside command

		public static Binder<TSource, Button> TouchUpInside<TSource>(this Binder<TSource, Button> binder, Expression<Func<TSource, ICommand>> property)
			where TSource : class
		{
			return binder.Command<EventArgs>(property, nameof(Button.Click), (b, v) => b.Enabled = v);
		}

		#endregion

		#region Title property

		public static Binder<TSource, Button> Title<TSource, TPropertyType>(this Binder<TSource, Button> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			return binder.Property(property, x => x.Text, converter);
		}

		#endregion
	}
}
