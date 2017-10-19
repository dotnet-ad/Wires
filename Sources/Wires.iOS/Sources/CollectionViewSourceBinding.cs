namespace Wires
{
	using System;
	using Foundation;
	using UIKit;

	public class CollectionViewSourceBinding<TOwner, TItem, TCellView> : UICollectionViewSource
		where TCellView : UICollectionViewCell
		where TOwner : class
	{

		#region Constructors

		public CollectionViewSourceBinding(BindableCollectionSource<TOwner, TItem, UICollectionView, TCellView> source, bool fromNib)
		{
			this.source = source;
			this.cellIdentifier = typeof(TCellView).Name;

			var view = this.source.View;

			if (fromNib)
			{
				view.RegisterNibForCell(NibLocator<TCellView>.Nib, cellIdentifier);
			}
			else
			{
				view.RegisterClassForCell(typeof(TCellView), cellIdentifier);
			}
		}

		#endregion

		#region Fields

		private readonly string cellIdentifier;

		readonly BindableCollectionSource<TOwner, TItem, UICollectionView, TCellView> source;

		#endregion

		#region Implementation

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			var view = (TCellView)collectionView.DequeueReusableCell(cellIdentifier, indexPath);
			this.source.PrepareCell(indexPath.Row, view);
			return view;
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath) => this.source?.Select(indexPath.Row);

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => this.source.Count;

		#endregion
	}
}
