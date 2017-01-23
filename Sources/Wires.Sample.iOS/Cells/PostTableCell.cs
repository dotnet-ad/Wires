using System;

using Foundation;
using UIKit;
using Wires.Sample.ViewModel;

namespace Wires.Sample.iOS
{
	public partial class PostTableCell : UITableViewCell
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
					this.title.Bind(value).Text(vm => vm.Title);
					this.author.Bind(value).Text(vm => vm.Author);
					this.date.Bind(value).Text(vm => vm.Datetime);
					this.illustration.Bind(value).ImageAsync(vm => vm.Thumbnail, PlatformConverters.AsyncStringToCachedImage(TimeSpan.FromHours(1)));
				}
			}
		}
	}
}
