using System;

using Foundation;
using UIKit;
using Wires.Sample.ViewModel;

namespace Wires.Sample.iOS
{
	public partial class PostCollectionCell : UICollectionViewCell, IView
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

					value
						.Bind(title)
							.Text(vm => vm.Title)
						.Bind(illustration)
							.ImageAsync(vm => vm.Thumbnail, PlatformConverters.AsyncStringToCachedImage(TimeSpan.FromHours(1)));
				}
			}
		}

		object IView.ViewModel
		{
			get { return this.ViewModel; }
			set { this.ViewModel = value as RedditViewModel.ItemViewModel; }
		}
	}
}
