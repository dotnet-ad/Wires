

namespace Wires.Sample.iOS
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UIKit;
	using Wires.Sample.ViewModel;

	public partial class TableViewController : UIViewController
	{
		protected TableViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public RedditViewModel ViewModel { get; private set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.ViewModel = new RedditViewModel();

			this.ViewModel
			    	.Bind(this.indicator)
			    		.IsAnimating(vm => vm.IsUpdating)
						.As<UIView>().Visible(vm => vm.IsUpdating)
					.Bind(this.tableView)
			    		.As<UIView>().Hidden(vm => vm.IsUpdating);

			this.UpdateSource();

			this.ViewModel.UpdateCommand.Execute(null);

			this.segmented.ValueChanged += (sender, e) => this.UpdateSource();
		}

		private void UpdateSource()
		{
			if (this.segmented.SelectedSegment == 0)
			{
				this.ViewModel.Bind(this.tableView).Source<RedditViewModel, RedditViewModel.ItemViewModel, PostTableCell>(vm => vm.Simple, (post, index, cell) => cell.ViewModel = post, heightForItem: (c) => 88);
			}
			else
			{
				this.ViewModel.Bind(this.tableView).Source<RedditViewModel,string, RedditViewModel.ItemViewModel,PostTableHeader, PostTableCell>(vm => vm.Grouped,(section, index, cell) => cell.ViewModel = section, (post, index, cell) => cell.ViewModel = post, null, (c) => 68, (c) => 88);
			}
		}

	}
}

