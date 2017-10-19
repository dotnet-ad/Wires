namespace Wire
{
	using System;
	using System.ComponentModel;
	using UIKit;

	public static class UIImageViewExtensions
	{
		#region Image property

		private class ImageConverter : IConverter<string, UIImage>
		{
			public UIImage Convert(string value)
			{
				throw new NotImplementedException();
			}

			public string ConvertBack(UIImage value)
			{
				throw new NotImplementedException();
			}
		}

		public static IBinding BindImage(this INotifyPropertyChanged observable, UIImageView label, string propertyName)
		{
			return observable.BindImage(label, propertyName, new ImageConverter());
		}

		public static IBinding BindImage<TPropertyType>(this INotifyPropertyChanged observable, UIImageView label, string propertyName, IConverter<TPropertyType,UIImage> converter)
		{
			return observable.BindOneWay(propertyName, label, nameof(UIImageView.Image), converter);
		}

		#endregion
	}
}
