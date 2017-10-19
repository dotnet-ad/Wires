using System;

using Foundation;
using UIKit;
using Wires.Sample.ViewModel;

namespace Wires.Sample.iOS
{
	public partial class PostTableCell : UITableViewCell, IView
	{
		public static readonly NSString Key = new NSString("PostTableCell");

		public static readonly UINib Nib;

		static PostTableCell()
		{
			Nib = UINib.FromName("PostTableCell", NSBundle.MainBundle);
		}

		protected PostTableCell(IntPtr handle) : base(handle)
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
					this.ViewModel?.Unbind(this.title, this.author, this.illustration);

					this.viewModel = value;

					value
						.Bind(title)
							.Text(vm => vm.Title)
						.Bind(author)
							.Text(vm => vm.Author)
						.Bind(date)
							.Text(vm => vm.Datetime)
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
