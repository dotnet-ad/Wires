namespace Wire
{
	using System;
	using System.Windows.Input;
	using UIKit;

	public static class UIButtonExtensions
	{
		#region TouchUpInside command

		public static IBinding BindTouchUpInside(this ICommand command, UIButton button)
		{
			return command.Bind<UIButton, EventArgs>(button, nameof(UIButton.TouchUpInside), (b, canExecute) => b.Enabled = canExecute);
		}

		#endregion
	}
}
