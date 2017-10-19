using System;

using Foundation;
using UIKit;
using Wires.Sample.ViewModel;

namespace Wires.Sample.iOS
{
	public partial class PostCollectionCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString("PostCollectionCell");

		public static readonly UINib Nib;

		static PostCollectionCell()
		{
			Nib = UINib.FromName("PostCollectionCell", NSBundle.MainBundle);
		}

		protected PostCollectionCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		private RedditViewModel.ItemViewModel viewModel;

		public RedditViewModel.ItemViewModel ViewModel
		{
			get { return this.viewModel; }
			set
			{
				if (this.viewModel != value)
				{
					this.viewModel = value;
					this.title.Bind(value).Text(vm => vm.Title);
					this.illustration.Bind(value).ImageAsync(vm => vm.Thumbnail, PlatformConverters.AsyncStringToCachedImage(TimeSpan.FromHours(1)));
				}
			}
		}
	}
}
