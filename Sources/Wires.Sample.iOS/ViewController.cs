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

			this.label.Bind(this.ViewModel).Text(vm => vm.Title, Converters.Uppercase);
			this.field.Bind(this.ViewModel).Text(vm => vm.Title);
			this.image.Bind(this.ViewModel).ImageAsync(vm => vm.Illustration)
			    .As<UIView>().Alpha(vm => vm.Amount).Visible(vm => vm.IsActive);
			this.toggleSwitch.Bind(this.ViewModel).On(vm => vm.IsActive);
			this.slider.Bind(this.ViewModel).Value(vm => vm.Amount);
			this.datePicker.Bind(this.ViewModel).Date(vm => vm.Birthday);
			this.progressView.Bind(this.ViewModel).Progress(vm => vm.Amount);
			this.activityIndicator.Bind(this.ViewModel).IsAnimating(vm => vm.IsLoading);
			this.segmented.Bind(this.ViewModel).Titles(vm => vm.Sections);
			this.button.Bind(this.ViewModel).TouchUpInside(vm => vm.LoadCommand);
		}
	}
}
