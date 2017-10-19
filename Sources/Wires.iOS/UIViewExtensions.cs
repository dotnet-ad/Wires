namespace Wires
{
	using System;
	using System.ComponentModel;
	using UIKit;

	public static class UIViewExtensions
	{
		#region Hidden property

		public static IBinding BindHidden(this INotifyPropertyChanged observable, UIView view, string propertyName)
		{
			return observable.BindHidden(view, propertyName, Converters.Identity<bool>());
		}

		public static IBinding BindHidden<TPropertyType>(this INotifyPropertyChanged observable, UIView label, string propertyName, Func<TPropertyType, bool> converter)
		{
			return observable.BindHidden(label, propertyName, new RelayConverter<TPropertyType, bool>(converter));
		}

		public static IBinding BindHidden<TPropertyType>(this INotifyPropertyChanged observable, UIView label, string propertyName, IConverter<TPropertyType, bool> converter)
		{
			return observable.BindOneWay(propertyName, label, nameof(UIView.Hidden), converter);
		}

		#endregion

	}
}
