namespace Wires.Sample.iOS
{
	using System;

	using Foundation;
	using UIKit;

	public partial class PostTableHeader : UITableViewHeaderFooterView, IView
	{
		public static readonly NSString Key = new NSString("PostTableHeader");

		public static readonly UINib Nib;

		static PostTableHeader()
		{
			Nib = UINib.FromName("PostTableHeader", NSBundle.MainBundle);
		}

		protected PostTableHeader(IntPtr handle) : base(handle)
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
