namespace Wires
{
	using System;
	using System.ComponentModel;
	using UIKit;

	public static class UITextFieldExtensions
	{
		#region Text property

		public static IBinding BindText(this INotifyPropertyChanged observable, UITextField field, string propertyName)
		{
			return observable.BindText(field, propertyName, Converters.Identity<string>());
		}

		public static IBinding BindText<TProperty>(this INotifyPropertyChanged observable, UITextField field, string propertyName, Func<TProperty, string> convert, Func<string, TProperty> convertBack)
		{
			return observable.BindText(field, propertyName, new RelayConverter<TProperty, string>(convert, convertBack));
		}

		public static IBinding BindText<TProperty>(this INotifyPropertyChanged observable, UITextField field, string propertyName, IConverter<TProperty,string> converter)
		{
			return observable.BindTwoWay<UITextField, TProperty, string, EventArgs>(propertyName, field, nameof(UITextField.Text), nameof(UITextField.EditingChanged), converter);
		}

		#endregion
	}
}
