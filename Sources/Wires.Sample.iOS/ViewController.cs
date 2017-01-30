using System;

using UIKit;
using Wires.Sample.ViewModel;

namespace Wires.Sample.iOS
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public HomeViewModel ViewModel { get; private set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			this.ViewModel = new HomeViewModel();

			this.ViewModel
			    .Bind(this.label)
			    	.Text(vm => vm.Title, Converters.Uppercase)
				.Bind(this.field)
			    	.Text(vm => vm.Title)
				.Bind(this.image)
			    	.ImageAsync(vm => vm.Illustration)
			    	.Alpha(vm => vm.Amount)
			    	.Visible(vm => vm.IsActive)
				.Bind(this.toggleSwitch)
			    	.On(vm => vm.IsActive)
				.Bind(this.slider)
			    	.Value(vm => vm.Amount)
				.Bind(this.datePicker)
			    	.Date(vm => vm.Birthday)
				.Bind(this.progressView)
			    	.Progress(vm => vm.Amount)
				.Bind(this.activityIndicator)
			    	.IsAnimating(vm => vm.IsLoading)
				.Bind(this.segmented)
			    	.Titles(vm => vm.Sections)
			    	.Selected(vm => vm.Selected)
				.Bind(this.button)
			    	.TouchUpInside(vm => vm.LoadCommand);
		}
	}
}
