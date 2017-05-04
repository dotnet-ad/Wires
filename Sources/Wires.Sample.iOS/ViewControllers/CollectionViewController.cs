namespace Wires.Sample.iOS
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
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

			this.ViewModel = new RedditViewModel();

			this.ViewModel
			    	.Bind(this.indicator)
			    		.IsAnimating(vm => vm.IsUpdating)
			    		.Visible(vm => vm.IsUpdating)
					.Bind(this.collectionView)
			    		.Hidden(vm => vm.IsUpdating)
			    	.Bind(this.collectionView)
						.Source(vm => vm.Items, (vm, v, c) =>
						{
							c.RegisterCellView<PostCollectionCell>("cell", 44, 44);
							c.RegisterHeaderView<PostCollectionHeader>("header", 88, 100);
						});

			this.ViewModel.UpdateCommand.Execute(null);

			this.segmented.ValueChanged += (sender, e) => this.ViewModel.IsGrouped = this.segmented.SelectedSegment > 0;
		}
	}
}

