namespace Wires
{
	using System;
	using UIKit;

	public class CollectionViewSourceBinding<TOwner, TItem, TCellView> : UICollectionViewSource
		where TCellView : UICollectionViewCell
		where TOwner : class
	{
		#region Constructors

		public CollectionViewSourceBinding(TOwner source, string sourceProperty, UICollectionView view, bool fromNib, Action<TItem, int, TCellView> prepareCell = null)
		{
			this.source = new BindableCollectionSource<TOwner, TItem, UICollectionView, TCellView>(source,sourceProperty,view,(c) => c.ReloadData(), prepareCell);

			if (fromNib)
			{
				view.RegisterNibForCell(NibLocator<TCellView>.Nib, this.source.CellIdentifer);
			}
			else
			{
				view.RegisterClassForCell(typeof(TCellView), this.source.CellIdentifer);
			}
		}

		#endregion

		#region Fields

		private readonly BindableCollectionSource<TOwner, TItem, UICollectionView, TCellView> source;

		#endregion

		#region Implementation

		public override UICollectionViewCell GetCell(UICollectionView collectionView, Foundation.NSIndexPath indexPath)
		{
			var view = (UICollectionViewCell)collectionView.DequeueReusableCell(this.source.CellIdentifer, indexPath);
			this.source.PrepareCell(indexPath.Row, (TCellView)view);
			return view;
		}

		public override nint NumberOfSections(UICollectionView collectionView) => 1;

		public override nint GetItemsCount(UICollectionView collectionView, nint section) => this.source.Count;

		public override bool ShouldHighlightItem(UICollectionView collectionView, Foundation.NSIndexPath indexPath) => false;

		#endregion
	}
}
