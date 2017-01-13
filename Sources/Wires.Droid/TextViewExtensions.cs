namespace Wires
{
	using System;
	using System.ComponentModel;
	using Android.Widget;

	public static class TextViewExtensions
	{
		#region Text property

		public static IBinding BindText(this INotifyPropertyChanged observable, TextView label, string propertyName)
		{
			return observable.BindText(label, propertyName, Converters.Identity<string>());
		}

		public static IBinding BindText<TPropertyType>(this INotifyPropertyChanged observable, TextView label, string propertyName, Func<TPropertyType, string> converter)
		{
			return observable.BindText(label, propertyName, new RelayConverter<TPropertyType, string>(converter));
		}

		public static IBinding BindText<TPropertyType>(this INotifyPropertyChanged observable, TextView label, string propertyName, IConverter<TPropertyType, string> converter)
		{
			return observable.BindOneWay(propertyName, label, nameof(TextView.Text), converter);
		}

		#endregion
	}
}
