

namespace Wires.Sample.iOS
{
	using System;

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

			this.UpdateSource();

			this.ViewModel.UpdateCommand.Execute(null);

			this.segmented.ValueChanged += (sender, e) => this.UpdateSource();
		}

		private void UpdateSource()
		{
			if (this.segmented.SelectedSegment == 0)
			{
				var source = new BindableCollectionSource<RedditViewModel, RedditViewModel.ItemViewModel, UITableView, PostTableCell>(this.ViewModel, vm => vm.Simple, this.tableView, (obj) => obj.ReloadData(), (post, index, cell) => cell.ViewModel = post);
				this.tableView.Source = new TableViewSourceBinding<RedditViewModel, RedditViewModel.ItemViewModel, PostTableCell>(source, (arg) => 88, true);
			}
			else
			{
				var source = new BindableGroupedCollectionSource<RedditViewModel, string, RedditViewModel.ItemViewModel, UITableView, PostTableCell, PostTableHeader>(this.ViewModel, vm => vm.Grouped, this.tableView, (obj) => obj.ReloadData(), (post, index, cell) => cell.ViewModel = post, (section, index, cell) => cell.ViewModel = section);
				this.tableView.Source = new GroupedTableViewSourceBinding<RedditViewModel, Collection<string,RedditViewModel.ItemViewModel>, string,  RedditViewModel.ItemViewModel, PostTableCell, PostTableHeader>(source, (arg) => 88,(arg) => 68, true);
			}
		}

	}
}

