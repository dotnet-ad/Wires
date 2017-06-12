namespace Wires
{
	using System;
	using System.Linq;
	using Foundation;
	using UIKit;

	public class TableViewSourceBinding<TViewModel> : UITableViewSource
		where TViewModel : class
	{
		#region Constructors

		public TableViewSourceBinding(UITableView view, CollectionSource<TViewModel> datasource, bool fromNibs = true)
		{
			this.datasource = datasource;
			this.RegisterCells(view, fromNibs);

		}

		#endregion

		#region Fields

		readonly CollectionSource<TViewModel> datasource;

		#endregion

		#region Properties

		public Action<float> OnScrolled { get; set; }

  		#endregion

		#region Implementation

		private ICell GetCellData(NSIndexPath indexPath)
		{
			var section = datasource.Sections.ElementAt(indexPath.Section);
			return section.Cells.ElementAt(indexPath.Row);
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var item = GetCellData(indexPath);
			var view = tableView.DequeueReusableCell(item.ViewIdentifier, indexPath) as IView;
			view.ViewModel = item.Item;

			return view as UITableViewCell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			var item = GetCellData(indexPath);
			item.Select?.Execute(item.Item);
		}

		public override nint NumberOfSections(UITableView tableView) => datasource.Sections.Count();

		public override nint RowsInSection(UITableView tableview, nint section) => datasource.Sections.ElementAt((int)section).Cells.Count();

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			var item = GetCellData(indexPath);
			var descriptor = this.datasource.GetCellView(item.ViewIdentifier);
			var size = descriptor.GetSize(item.Item);
			return size.Item2;
		}

		public override nfloat GetHeightForHeader(UITableView tableView, nint section)
		{
			var headerdata = datasource.Sections.ElementAt((int)section).Header;
			if (headerdata == null)
				return 0;

			var descriptor = this.datasource.GetHeaderView(headerdata.ViewIdentifier);
			var size = descriptor.GetSize(headerdata);
			return size.Item2;
		}

		public override nfloat GetHeightForFooter(UITableView tableView, nint section)
		{
			var headerdata = datasource.Sections.ElementAt((int)section).Footer;
			if (headerdata == null)
				return 0;

			var descriptor = this.datasource.GetFooterView(headerdata.ViewIdentifier);
			var size = descriptor.GetSize(headerdata);
			return size.Item2;
		}

		public override UIView GetViewForHeader(UITableView tableView, nint section)
		{
			var item = datasource.Sections.ElementAt((int)section).Header;
			if (item == null)
				return null;
			
			var view = tableView.DequeueReusableHeaderFooterView(item.ViewIdentifier) as IView;
			view.ViewModel = item.Item;
			return view as UIView;
		}

		public override UIView GetViewForFooter(UITableView tableView, nint section)
		{
			var item = datasource.Sections.ElementAt((int)section).Footer;
			if (item == null)
				return null;

			var view = tableView.DequeueReusableHeaderFooterView(item.ViewIdentifier) as IView;
			view.ViewModel = item.Item;
			return view as UIView;
		}

		public override void Scrolled(UIScrollView scrollView) => this.OnScrolled?.Invoke((float)scrollView.ContentOffset.Y);

		#endregion

		#region Initialization

		private void RegisterCells(UITableView view, bool fromNibs)
		{
			if (fromNibs)
			{
				foreach (var item in datasource.HeaderViews.Union(datasource.FooterViews))
				{
					view.RegisterNibForHeaderFooterViewReuse(NibLocator.Nib(item.ViewType), item.Identifier);
				}

				foreach (var item in datasource.CellViews)
				{
					view.RegisterNibForCellReuse(NibLocator.Nib(item.ViewType), item.Identifier);
				}
			}
			else
			{
				foreach (var item in datasource.HeaderViews.Union(datasource.FooterViews))
				{
					view.RegisterClassForHeaderFooterViewReuse(item.ViewType, item.Identifier);
				}

				foreach (var item in datasource.CellViews)
				{
					view.RegisterClassForCellReuse(item.ViewType, item.Identifier);
				}

			}
		}

		#endregion
	}
}
