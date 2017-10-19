namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using System.Windows.Input;
	using Transmute;
	using UIKit;

	public static partial class UIExtensions
	{
		#region TouchUpInside command

		public static Binder<TSource, UIButton> TouchUpInside<TSource>(this Binder<TSource, UIButton> binder, Expression<Func<TSource, ICommand>> property)
			where TSource : class
		{
			return binder.Command<EventArgs>(property, nameof(UIButton.TouchUpInside),(b, v) => b.Enabled = v);
		}

		#endregion

		#region Title property

		public static Binder<TSource, UIButton>  Title<TSource, TPropertyType>(this Binder<TSource, UIButton> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			Action<UIButton, string> setter = (b, v) => b.SetTitle(v, UIControlState.Normal);
			Func<UIButton, string> getter = (b) => b.Title(UIControlState.Normal);
			return binder.Property(property, getter, setter, converter);
		}

		#endregion

		#region Image property

		public static Binder<TSource, UIButton> Image<TSource,TPropertyType>(this Binder<TSource, UIButton> binder, Expression<Func<TSource,TPropertyType>> property, IConverter<TPropertyType, UIImage> converter = null)
			where TSource : class
		{
			Action<UIButton, UIImage> setter = (b, v) => b.SetImage(v, UIControlState.Normal);
			Func<UIButton, UIImage> getter = (b) => b.CurrentImage;
			return binder.Property(property, getter, setter, converter);
		}

		#endregion
	}
}
