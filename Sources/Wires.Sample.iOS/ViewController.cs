namespace Wires.Sample.iOS
{
	using System;
	using UIKit;
	using Wires.Sample.ViewModel;

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
			    .Text(vm => vm.Title)//, Converters.Uppercase)
				.Bind(this.field)
					.Text(vm => vm.Entry)
				.Bind(this.textView)
					.Text(vm => vm.Title)
					.UserInteractionEnabled(vm => vm.IsActive)
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
					//.Color(vm => vm.Amount, new RelayConverter<double, UIColor>(d => new UIColor((nfloat)d / 3, (nfloat)d, (nfloat)d / 2, 1)))
				.Bind(this.segmented)
					.Titles(vm => vm.Sections)
					.Selected(vm => vm.Selected)
				.Bind(this.button)
					.TouchUpInside(vm => vm.LoadCommand);
		}
	}
}
