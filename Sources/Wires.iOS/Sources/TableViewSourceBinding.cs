namespace Wires
{
	using System;
	using Foundation;
	using UIKit;

	public class TableViewSourceBinding<TOwner, TItem, TCellView> : UITableViewSource
		where TCellView : UITableViewCell
		where TOwner : class
	{
		#region Constructors

		public TableViewSourceBinding(BindableCollectionSource<TOwner, TItem, UITableView, TCellView> source, Func<int, nfloat> heightForItem, bool fromNib)
		{
			this.source = source;
			this.heightForItem = heightForItem;

			var view = this.source.View;
			cellIdentifier = typeof(TCellView).Name;

			if (fromNib)
			{
				view.RegisterNibForCellReuse(NibLocator<TCellView>.Nib, cellIdentifier);
			}
			else
			{
				view.RegisterClassForCellReuse(typeof(TCellView), cellIdentifier);
			}
		}

		#endregion

		#region Fields

		private readonly string cellIdentifier;

		readonly Func<int, nfloat> heightForItem;

		readonly BindableCollectionSource<TOwner, TItem, UITableView, TCellView> source;

		#endregion

		#region Implementation

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var view = tableView.DequeueReusableCell(cellIdentifier, indexPath);
			this.source.PrepareCell(indexPath.Row, (TCellView)view);
			return view;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath) => this.source?.Select(indexPath.Row);

		public override nint NumberOfSections(UITableView tableView) => 1;

		public override nint RowsInSection(UITableView tableview, nint section) => this.source.Count;

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => this.heightForItem(indexPath.Row);

		#endregion
	}
}
