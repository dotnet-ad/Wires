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

			this.label.Bind(this.ViewModel).Text(vm => vm.Title);
			this.field.Bind(this.ViewModel).Text(vm => vm.Title);
			this.image.Bind(this.ViewModel).Image(vm => vm.Illustration);
			this.image.Bind(this.ViewModel).As<UIView>().Visible(vm => vm.IsActive);
			this.toggleSwitch.Bind(this.ViewModel).On(vm => vm.IsActive);
			this.slider.Bind(this.ViewModel).Value(vm => vm.Amount);
			this.datePicker.Bind(this.ViewModel).Date(vm => vm.Birthday);
			this.progressView.Bind(this.ViewModel).Progress(vm => vm.Amount);
			this.activityIndicator.Bind(this.ViewModel).IsAnimating(vm => vm.IsLoading);
			this.button.Bind(this.ViewModel.LoadCommand).TouchUpInside();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
