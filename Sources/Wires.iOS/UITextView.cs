namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using System.Windows.Input;
	using Transmute;
	using UIKit;

	public partial class UIExtensions
	{
		#region Text property

		public static Binder<TSource, UITextView> Text<TSource, TPropertyType>(this Binder<TSource, UITextView> binder, Expression<Func<TSource, TPropertyType>> property, ITwoWayConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, string, EventArgs> (property, b => b.Text, nameof(UITextView.Changed), converter);
		}

		#endregion
	}
}
