namespace Wires.Sample.iOS
{
	using System;
	using UIKit;
	using Wires.Sample.ViewModel;

	public partial class CollectionViewController : UIViewController
	{
		public CollectionViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public RedditViewModel ViewModel { get; private set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var layout = this.collectionView.CollectionViewLayout as UICollectionViewFlowLayout;
			layout.ItemSize = new CoreGraphics.CGSize(60,60);
			layout.HeaderReferenceSize = new CoreGraphics.CGSize(1000, 40);

			this.ViewModel = new RedditViewModel();

			this.UpdateSource();

			this.ViewModel.UpdateCommand.Execute(null);

			this.segmented.ValueChanged += (sender, e) => this.UpdateSource();
		}

		private void UpdateSource()
		{
			if (this.segmented.SelectedSegment == 0)
			{
				var source = new BindableCollectionSource<RedditViewModel, RedditViewModel.ItemViewModel, UICollectionView, PostCollectionCell>(this.ViewModel, vm => vm.Simple, this.collectionView, (obj) => obj.ReloadData(), (post, index, cell) => cell.ViewModel = post);
				this.collectionView.Source = new CollectionViewSourceBinding<RedditViewModel, RedditViewModel.ItemViewModel, PostCollectionCell>(source, true);
			}
			else
			{
				var source = new BindableGroupedCollectionSource<RedditViewModel, string, RedditViewModel.ItemViewModel, UICollectionView, PostCollectionCell, PostCollectionHeader>(this.ViewModel, vm => vm.Grouped, this.collectionView, (obj) => obj.ReloadData(), (post, index, cell) => cell.ViewModel = post, (section, index, cell) => cell.ViewModel = section);
				this.collectionView.Source = new GroupedCollectionViewSourceBinding<RedditViewModel, Collection<string, RedditViewModel.ItemViewModel>, string, RedditViewModel.ItemViewModel, PostCollectionCell, PostCollectionHeader>(source, true);
			}
		}
	}
}

