namespace Wires
{
	using System;
	using System.ComponentModel;
	using UIKit;

	public static class UIImageViewExtensions
	{
		#region Image property

		public static IBinding BindImage(this INotifyPropertyChanged observable, UIImageView label, string propertyName)
		{
			return observable.BindImage(label, propertyName, PlatformConverters.StringToImage);
		}

		public static IBinding BindImage<TPropertyType>(this INotifyPropertyChanged observable, UIImageView label, string propertyName, Func<TPropertyType, UIImage> converter)
		{
			return observable.BindImage(label, propertyName, new RelayConverter<TPropertyType, UIImage>(converter));
		}

		public static IBinding BindImage<TPropertyType>(this INotifyPropertyChanged observable, UIImageView label, string propertyName, IConverter<TPropertyType,UIImage> converter)
		{
			return observable.BindOneWay(propertyName, label, nameof(UIImageView.Image), converter);
		}

		#endregion
	}
}
