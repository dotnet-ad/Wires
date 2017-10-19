namespace Wires
{
	using System;
	using System.Linq;
	using Foundation;
	using UIKit;

	public class CollectionViewSourceBinding<TViewModel> : UICollectionViewSource
		where TViewModel : class
	{
		public class Layout : UICollectionViewDelegateFlowLayout
		{
			public Layout(CollectionSource<TViewModel> datasource)
			{
				this.datasource = datasource;
			}

			private CollectionSource<TViewModel> datasource;

			public override CoreGraphics.CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
			{
				var section = datasource.Sections.ElementAt(indexPath.Section);
				var cell = section.Cells.ElementAt(indexPath.Row);
				var descriptor = datasource.GetCellView(cell.ViewIdentifier);
				var size = descriptor.GetSize(cell.Item);
				return new CoreGraphics.CGSize(size.Item1,size.Item2);
			}

			public override CoreGraphics.CGSize GetReferenceSizeForHeader(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
			{
				var headerdata = datasource.Sections.ElementAt((int)section).Header;
				if (headerdata == null)
					return CoreGraphics.CGSize.Empty;

				var descriptor = this.datasource.GetHeaderView(headerdata.ViewIdentifier);
				var size = descriptor.GetSize(headerdata);
				return new CoreGraphics.CGSize(size.Item1, size.Item2);
			}

			public override CoreGraphics.CGSize GetReferenceSizeForFooter(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
			{
				var headerdata = datasource.Sections.ElementAt((int)section).Footer;
				if (headerdata == null)
					return CoreGraphics.CGSize.Empty;

				var descriptor = this.datasource.GetFooterView(headerdata.ViewIdentifier);
				var size = descriptor.GetSize(headerdata);
				return new CoreGraphics.CGSize(size.Item1, size.Item2);
			}
		}

		#region Constructors

		public CollectionViewSourceBinding(UICollectionView view, CollectionSource<TViewModel> datasource, bool fromNibs = true)
		{
			this.datasource = datasource;
			view.Delegate = new Layout(datasource);
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

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var item = GetCellData(indexPath);
			var view = collectionView.DequeueReusableCell(item.ViewIdentifier, indexPath) as IView;
			view.ViewModel = item.Item;

			return view as UICollectionViewCell;
		}

		public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
		{
			var section = datasource.Sections.ElementAt(indexPath.Section);
			var item = (elementKind == UICollectionElementKindSectionKey.Header) ? section.Header : section.Footer;

			if (item == null)
			{
				return collectionView.DequeueReusableSupplementaryView(elementKind, "___empty___", indexPath);
			}

			var view = collectionView.DequeueReusableSupplementaryView(elementKind, item.ViewIdentifier, indexPath) as IView;
			view.ViewModel = item.Item;
			return view as UICollectionReusableView;
		}


		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var item = GetCellData(indexPath);
			item.Select?.Execute(item.Item);
		}

		public override nint NumberOfSections(UICollectionView collectionView) => datasource.Sections.Count();

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => datasource.Sections.ElementAt((int)section).Cells.Count();

		public override void Scrolled(UIScrollView scrollView) => this.OnScrolled?.Invoke((float)scrollView.ContentOffset.Y);

		#endregion

		#region Initialization

		private void RegisterCells(UICollectionView view, bool fromNibs)
		{
			view.RegisterClassForSupplementaryView(typeof(UICollectionReusableView), UICollectionElementKindSection.Header, "___empty___");

			if (fromNibs)
			{
				foreach (var item in datasource.HeaderViews)
				{
					view.RegisterNibForSupplementaryView(NibLocator.Nib(item.ViewType), UICollectionElementKindSection.Header, item.Identifier);
				}

				foreach (var item in datasource.FooterViews)
				{
					view.RegisterNibForSupplementaryView(NibLocator.Nib(item.ViewType), UICollectionElementKindSection.Footer, item.Identifier);
				}

				foreach (var item in datasource.CellViews)
				{
					view.RegisterNibForCell(NibLocator.Nib(item.ViewType), item.Identifier);
				}
			}
			else
			{
				foreach (var item in datasource.HeaderViews)
				{
					view.RegisterClassForSupplementaryView(item.ViewType, UICollectionElementKindSection.Header, item.Identifier);
				}

				foreach (var item in datasource.FooterViews)
				{
					view.RegisterClassForSupplementaryView(item.ViewType, UICollectionElementKindSection.Footer, item.Identifier);
				}

				foreach (var item in datasource.CellViews)
				{
					view.RegisterClassForCell(item.ViewType, item.Identifier);
				}
			}
		}

		#endregion
	}
}
