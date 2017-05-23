namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using System.Windows.Input;
	using UIKit;

	public static partial class UIExtensions
	{
		#region TouchUpInside command

		public static Binder<TSource, UIBarButtonItem> Clicked<TSource>(this Binder<TSource, UIBarButtonItem> binder, Expression<Func<TSource, ICommand>> property)
			where TSource : class
		{
			return binder.Command<EventArgs>(property, nameof(UIBarButtonItem.Clicked), (bb, v) => bb.Enabled = v);
		}

		#endregion

		#region Title property

		public static Binder<TSource, UIBarButtonItem> Title<TSource, TPropertyType>(this Binder<TSource, UIBarButtonItem> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			Action<UIBarButtonItem, string> setter = (b, v) => b.Title = v;
			Func<UIBarButtonItem, string> getter = (b) => b.Title;
			return binder.Property(property, getter, setter, converter);
		}

		#endregion
	}
}
