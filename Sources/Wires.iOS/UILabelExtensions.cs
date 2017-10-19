namespace Wires
{
	using System;
	using System.ComponentModel;
	using UIKit;

	public static class UILabelExtensions
	{
		#region Text property

		public static IBinding BindText(this INotifyPropertyChanged observable, UILabel label, string propertyName)
		{
			return observable.BindText(label, propertyName, Converters.Identity<string>());
		}

		public static IBinding BindText<TPropertyType>(this INotifyPropertyChanged observable, UILabel label, string propertyName, Func<TPropertyType, string> converter)
		{
			return observable.BindText(label, propertyName, new RelayConverter<TPropertyType, string>(converter));
		}

		public static IBinding BindText<TPropertyType>(this INotifyPropertyChanged observable, UILabel label, string propertyName, IConverter<TPropertyType, string> converter)
		{
			return observable.BindOneWay(propertyName, label, nameof(UILabel.Text), converter);
		}

		#endregion


		#region Text property

		public static IBinding BindTextColor(this INotifyPropertyChanged observable, UILabel label, string propertyName)
		{
			return observable.BindTextColor(label, propertyName, PlatformConverters.IntToColor);
		}

		public static IBinding BindText<TPropertyType>(this INotifyPropertyChanged observable, UILabel label, string propertyName, Func<TPropertyType, UIColor> converter)
		{
			return observable.BindTextColor(label, propertyName, new RelayConverter<TPropertyType, UIColor>(converter));
		}

		public static IBinding BindTextColor<TPropertyType>(this INotifyPropertyChanged observable, UILabel label, string propertyName, IConverter<TPropertyType, UIColor> converter)
		{
			return observable.BindOneWay(propertyName, label, nameof(UILabel.TextColor), converter);
		}

		#endregion
	}
}
