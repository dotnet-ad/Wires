

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
						.Visible(vm => vm.IsUpdating)
					.Bind(this.tableView)
			    		.Hidden(vm => vm.IsUpdating)
			    	.Bind(this.tableView)
			    		.Source(vm => vm.Items, (vm,v,c) =>
						{
							c.RegisterCellView<PostTableCell>("cell", 44);
							c.RegisterHeaderView<PostTableHeader>("header", 88);
						});

			this.ViewModel.UpdateCommand.Execute(null);

			this.segmented.ValueChanged += (sender, e) => this.ViewModel.IsGrouped = this.segmented.SelectedSegment > 0;
		}
	}
}

