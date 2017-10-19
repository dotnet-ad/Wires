namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using System.Windows.Input;
	using Transmute;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Text property

		public static Binder<TSource, UITextField> Text<TSource, TPropertyType>(this Binder<TSource, UITextField> binder, Expression<Func<TSource, TPropertyType>> property, ITwoWayConverter<TPropertyType, string> converter = null)
			where TSource : class
		{
			return binder.Property<TPropertyType, string, EventArgs> (property, b => b.Text,nameof(UITextField.EditingChanged), converter);
		}

		#endregion

		#region ShouldReturn command

		public static Binder<TSource, UITextField> ShouldReturn<TSource>(this Binder<TSource, UITextField> binder, Expression<Func<TSource, ICommand>> property)
			where TSource : class
		{
			// No weak event use since Should return is a delegate with return type but
			// this is not an issue since there is no subscription to the source
			var compiled = property.Compile();
			binder.Target.ShouldReturn += (textField) => {
				var command = compiled(binder.Source);
				if(command?.CanExecute(null) ?? false)
				{
					command.Execute(null);
				}
				return false;
			};
			return binder.Command<EventArgs>(property, nameof(UIButton.TouchUpInside), (b, v) => b.Enabled = v);
		}

		#endregion
	}
}
