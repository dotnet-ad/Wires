namespace Wire
{
	using System.ComponentModel;
	using UIKit;

	public static class UILabelExtensions
	{
		#region Text property

		public static IBinding BindText(this INotifyPropertyChanged observable, UILabel label, string propertyName)
		{
			return observable.BindText(label, propertyName, IdentityConverter<string>.Default);
		}

		public static IBinding BindText<TPropertyType>(this INotifyPropertyChanged observable, UILabel label, string propertyName, IConverter<TPropertyType, string> converter)
		{
			return observable.BindOneWay(propertyName, label, nameof(UILabel.Text), converter);
		}

		#endregion
	}
}
