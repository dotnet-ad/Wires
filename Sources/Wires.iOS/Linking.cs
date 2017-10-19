namespace Wires
{
	using UIKit;

	/// <summary>
	/// This class will force to include symbols that are used by reflection and may be ignored during linking phases.
	/// Kinda hacky, but is still the best option for now.
	/// </summary>
	public static class Linking
	{
		public static void All()
		{
			var x1 = new UIButton();
			x1.TouchUpInside += (s, e) => { };

			var x2 = new UISlider();
			x2.ValueChanged += (s, e) => { };

			var x3 = new UIStepper();
			x3.ValueChanged += (s, e) => { };

			var x4 = new UISwitch();
			x4.ValueChanged += (s, e) => { };

			var x5 = new UITextField();
			x5.Text = x5.Text;
			x5.EditingChanged += (s, e) => { };

			var x6 = new UIView();
			x6.Hidden = x6.Hidden;
			x6.Frame = x6.Frame;
			x6.TintColor = x6.TintColor;
			x6.BackgroundColor = x6.BackgroundColor;
			x6.Alpha = x6.Alpha;
			x6.UserInteractionEnabled = x6.UserInteractionEnabled;

			var x7 = new UIViewController();
			x7.Title = x7.Title;

			var x8 = new UIDatePicker();
			x8.Date = x8.Date;

			var x9 = new UIProgressView();
			x9.Progress = x9.Progress;
			x9.ProgressTintColor = x9.ProgressTintColor;
			x9.TrackTintColor = x9.TrackTintColor;

			var x10 = new UIImageView();
			x10.Image = x10.Image;

			var x11 = new UILabel();
			x11.Text = x11.Text;
			x11.TextColor = x11.TextColor;

			var x12 = new UISegmentedControl();
			x12.SelectedSegment = x12.SelectedSegment;

			var x13 = new UISlider();
			x13.Value = x13.Value;
			x13.MaxValue = x13.MaxValue;
			x13.MinValue = x13.MinValue;

			var x14 = new UIStepper();
			x14.Value = x14.Value;
			x14.MaximumValue = x14.MaximumValue;
			x14.MinimumValue = x14.MinimumValue;

			var x15 = new UISwitch();
			x15.On = x15.On;

			var x16 = new UITextView();
			x16.Text = x16.Text;
			x16.Changed += (sender, e) => { };
		}
	}
}
