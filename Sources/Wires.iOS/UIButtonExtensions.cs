namespace Wires
{
	using System;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using System.Windows.Input;
	using UIKit;

	public static partial class UIExtensions
	{
		#region TouchUpInside command

		public static IBinding TouchUpInside(this Binder<ICommand,UIButton> binder)
		{
			return binder.Command<UIButton,EventArgs>(nameof(UIButton.TouchUpInside), (b, canExecute) => b.Enabled = canExecute);
		}

		#endregion

		#region Title property

		public static IBinding Title<TSource, TPropertyType>(this Binder<TSource, UIButton> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			Action<UIButton, string> setter = (b, v) => b.SetTitle(v, UIControlState.Normal);
			Func<UIButton, string> getter = (b) => b.Title(UIControlState.Normal);
			return binder.Property(property, getter, setter, converter);
		}

		#endregion

		#region Image property

		public static IBinding Image<TSource,TPropertyType>(this Binder<TSource, UIButton> binder, Expression<Func<TSource,TPropertyType>> property, IConverter<TPropertyType, UIImage> converter = null)
			where TSource : class
		{
			Action<UIButton, UIImage> setter = (b, v) => b.SetImage(v, UIControlState.Normal);
			Func<UIButton, UIImage> getter = (b) => b.CurrentImage;
			return binder.Property(property, getter, setter, converter);
		}

		#endregion
	}
}
