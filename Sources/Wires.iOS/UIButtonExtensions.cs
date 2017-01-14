namespace Wires
{
	using System;
	using System.ComponentModel;
	using System.Windows.Input;
	using UIKit;

	public static class UIButtonExtensions
	{
		#region TouchUpInside command

		public static IBinding BindTouchUpInside(this ICommand command, UIButton button)
		{
			return command.Bind<UIButton, EventArgs>(button, nameof(UIButton.TouchUpInside), (b, canExecute) => b.Enabled = canExecute);
		}

		#endregion

		#region Title property

		public static IBinding BindTitle(this INotifyPropertyChanged observable, UIButton label, string propertyName)
		{
			return observable.BindTitle(label, propertyName, Converters.Identity<string>());
		}

		public static IBinding BindTitle<TPropertyType>(this INotifyPropertyChanged observable, UIButton label, string propertyName, Func<TPropertyType, string> converter)
		{
			return observable.BindTitle(label, propertyName, new RelayConverter<TPropertyType, string>(converter));
		}

		public static IBinding BindTitle<TPropertyType>(this INotifyPropertyChanged observable, UIButton label, string propertyName, IConverter<TPropertyType, string> converter)
		{
			return observable.BindOneWay(propertyName, label, (b) => b.Title(UIControlState.Normal), (b,v) => b.SetTitle(v, UIControlState.Normal) , converter);
		}

		#endregion

		#region Image property

		public static IBinding BindImage(this INotifyPropertyChanged observable, UIButton label, string propertyName)
		{
			return observable.BindImage(label, propertyName, PlatformConverters.StringToImage);
		}

		public static IBinding BindImage<TPropertyType>(this INotifyPropertyChanged observable, UIButton label, string propertyName, Func<TPropertyType, UIImage> converter)
		{
			return observable.BindImage(label, propertyName, new RelayConverter<TPropertyType, UIImage>(converter));
		}

		public static IBinding BindImage<TPropertyType>(this INotifyPropertyChanged observable, UIButton label, string propertyName, IConverter<TPropertyType, UIImage> converter)
		{
			return observable.BindOneWay(propertyName, label, (b) => b.CurrentImage, (b, v) => b.SetImage(v, UIControlState.Normal), converter);
		}

		#endregion
	}
}
