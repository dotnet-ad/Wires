namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using System.Windows.Input;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Refresh command

		// TODO Add on updating property binding to stop refreshing
		/*public static IBinding Refresh(this Binder<ICommand, UIRefreshControl> binder, Action endRefreshing)
		{
			return binder.Command<UIRefreshControl, EventArgs>(nameof(UIButton.TouchUpInside), (b, canExecute) => b.Enabled = canExecute);
		}*/

		#endregion

	}
}
