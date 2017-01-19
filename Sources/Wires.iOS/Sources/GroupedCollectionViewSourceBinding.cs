namespace Wires
{
	using System;
	using Foundation;
	using UIKit;

	public class GroupedCollectionViewSourceBinding<TOwner, TCollection, TSection, TItem, TCellView, THeaderCellView> : UICollectionViewSource
		where TCellView : UICollectionViewCell
		where THeaderCellView : UICollectionReusableView
		where TCollection : class
		where TOwner : class
	{

		#region Constructors

		public GroupedCollectionViewSourceBinding(BindableGroupedCollectionSource<TOwner, TCollection, TSection, TItem, UICollectionView, TCellView, THeaderCellView> source, bool fromNib)
		{
			this.source = source;

			this.cellIdentifier = typeof(TCellView).Name;
			this.headerIdentifier = typeof(THeaderCellView).Name;

			var view = this.source.View;

			if (fromNib)
			{
				view.RegisterNibForCell(NibLocator<TCellView>.Nib, cellIdentifier);
				view.RegisterNibForSupplementaryView(NibLocator<THeaderCellView>.Nib, UICollectionElementKindSection.Header, headerIdentifier);
			}
			else
			{
				view.RegisterClassForCell(typeof(TCellView), cellIdentifier);
				view.RegisterClassForSupplementaryView(typeof(THeaderCellView), UICollectionElementKindSection.Header, headerIdentifier);
			}
		}

		#endregion

		#region Fields

		private readonly string cellIdentifier, headerIdentifier;

		readonly BindableGroupedCollectionSource<TOwner, TCollection, TSection, TItem, UICollectionView, TCellView, THeaderCellView> source;

		#endregion

		#region Implementation

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var view = (TCellView)collectionView.DequeueReusableCell(cellIdentifier, indexPath);
			this.source.PrepareCell(indexPath.ToIndex(), view);
			return view;
		}

		public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
		{
			var view = (THeaderCellView)collectionView.DequeueReusableSupplementaryView(UICollectionElementKindSection.Header, headerIdentifier, indexPath);
			this.source.PrepareHeader(indexPath.Section, view);
			return view;
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath) => this.source?.Select(indexPath.ToIndex());

		public override nint NumberOfSections(UICollectionView collectionView) => this.source?.SectionsCount ?? 0;

		public override nint GetItemsCount(UICollectionView collectionView, nint section)=> this.source?.ItemsCount((int)section) ?? 0;

		#endregion
	}
}
