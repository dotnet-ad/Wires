namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Text property

		public static IBinding Text<TSource, TPropertyType>(this Binder<TSource, UITextField> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, string, EventArgs> (property, b => b.Text,nameof(UITextField.EditingChanged), converter);
		}

		#endregion
	}
}
