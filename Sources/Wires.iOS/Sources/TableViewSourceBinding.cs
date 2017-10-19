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

		public TableViewSourceBinding(TOwner source, string sourceProperty, UITableView view, bool fromNib, nfloat rowHeight, Action<TItem, int, TCellView> prepareCell = null)
		{
			this.source = new BindableCollectionSource<TOwner, TItem, UITableView, TCellView>(source, sourceProperty, view, (c) => c.ReloadData(), prepareCell);
			this.rowHeight = rowHeight;

			if (fromNib)
			{
				view.RegisterNibForCellReuse(NibLocator<TCellView>.Nib, this.source.CellIdentifer);
			}
			else
			{
				view.RegisterClassForCellReuse(typeof(TCellView), this.source.CellIdentifer);
			}
		}

		#endregion

		#region Fields

		private readonly nfloat rowHeight;

		private readonly BindableCollectionSource<TOwner, TItem, UITableView, TCellView> source;

		#endregion

		#region Implementation

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var view = tableView.DequeueReusableCell(this.source.CellIdentifer, indexPath);
			this.source.PrepareCell(indexPath.Row, (TCellView)view);
			return view;
		}

		public override nint NumberOfSections(UITableView tableView) => 1;

		public override nint RowsInSection(UITableView tableview, nint section) => this.source.Count;

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => this.rowHeight;

		#endregion
	}
}
