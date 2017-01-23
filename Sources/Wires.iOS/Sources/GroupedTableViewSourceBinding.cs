namespace Wires
{
	using System;
	using Foundation;
	using UIKit;

	public class GroupedTableViewSourceBinding<TOwner, TCollection, TSection, TItem, TCellView, THeaderCellView> : UITableViewSource
		where TCellView : UITableViewCell
		where THeaderCellView : UITableViewHeaderFooterView
		where TCollection : class
		where TOwner : class
	{
		#region Constructors

		public GroupedTableViewSourceBinding(BindableGroupedCollectionSource<TOwner, TCollection, TSection, TItem, UITableView, TCellView, THeaderCellView> source, Func<Index, nfloat> heightForItem, Func<int, nfloat> heightForHeader, bool fromNib)
		{
			this.source = source;
			this.heightForItem = heightForItem;
			this.heightForHeader = heightForHeader;

			var view = this.source.Target;
			cellIdentifier = typeof(TCellView).Name;
			headerIdentifier = typeof(THeaderCellView).Name;

			if (fromNib)
			{
				view.RegisterNibForCellReuse(NibLocator<TCellView>.Nib, cellIdentifier);
				view.RegisterNibForHeaderFooterViewReuse(NibLocator<THeaderCellView>.Nib, headerIdentifier);
			}
			else
			{
				view.RegisterClassForCellReuse(typeof(TCellView), cellIdentifier);
				view.RegisterClassForHeaderFooterViewReuse(typeof(THeaderCellView), headerIdentifier);
			}
		}

		#endregion

		#region Fields

		private readonly string cellIdentifier, headerIdentifier;

		readonly Func<Index, nfloat> heightForItem;

		readonly Func<int, nfloat> heightForHeader;

		readonly BindableGroupedCollectionSource<TOwner, TCollection, TSection, TItem, UITableView, TCellView, THeaderCellView> source;

		#endregion

		#region Implementation

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var view = tableView.DequeueReusableCell(cellIdentifier, indexPath);
			this.source.PrepareCell(indexPath.ToIndex(), (TCellView)view);
			return view;
		}

		public override UIView GetViewForHeader(UITableView tableView, nint section)
		{
			var view = tableView.DequeueReusableHeaderFooterView(headerIdentifier);
			this.source.PrepareHeader((int)section, (THeaderCellView)view);
			return view;
		} 

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath) => this.source?.Select(indexPath.ToIndex());

		public override nint NumberOfSections(UITableView tableView) => this.source?.SectionsCount ?? 0;

		public override nint RowsInSection(UITableView tableview, nint section) => this.source?.ItemsCount((int)section) ?? 0;

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath) => this.heightForItem(indexPath.ToIndex());

		public override nfloat GetHeightForHeader(UITableView tableView, nint section) => this.heightForHeader((int)section);

		#endregion
	}
}
