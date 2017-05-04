using System;

using Foundation;
using UIKit;

namespace Wires.Sample.iOS
{
	public partial class PostCollectionHeader : UICollectionReusableView, IView
	{
		public static readonly NSString Key = new NSString("PostCollectionHeader");

		public static readonly UINib Nib;

		static PostCollectionHeader()
		{
			Nib = UINib.FromName("PostCollectionHeader", NSBundle.MainBundle);
		}

		protected PostCollectionHeader(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
		private string viewModel;

		public string ViewModel
		{
			get { return this.viewModel; }
			set
			{
				if (this.viewModel != value)
				{
					this.viewModel = value;

					value
						.Bind(title)
							.Text(vm => vm);
				}
			}
		}

		object IView.ViewModel
		{
			get { return this.ViewModel; }
			set { this.ViewModel = value as string; }
		}
	}
}
