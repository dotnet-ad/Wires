// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Wires.Sample.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIActivityIndicatorView activityIndicator { get; set; }

		[Outlet]
		UIKit.UIButton button { get; set; }

		[Outlet]
		UIKit.UIDatePicker datePicker { get; set; }

		[Outlet]
		UIKit.UITextField field { get; set; }

		[Outlet]
		UIKit.UIImageView image { get; set; }

		[Outlet]
		UIKit.UILabel label { get; set; }

		[Outlet]
		UIKit.UIProgressView progressView { get; set; }

		[Outlet]
		UIKit.UISegmentedControl segmented { get; set; }

		[Outlet]
		UIKit.UISlider slider { get; set; }

		[Outlet]
		UIKit.UISwitch toggleSwitch { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (activityIndicator != null) {
				activityIndicator.Dispose ();
				activityIndicator = null;
			}

			if (button != null) {
				button.Dispose ();
				button = null;
			}

			if (datePicker != null) {
				datePicker.Dispose ();
				datePicker = null;
			}

			if (field != null) {
				field.Dispose ();
				field = null;
			}

			if (image != null) {
				image.Dispose ();
				image = null;
			}

			if (label != null) {
				label.Dispose ();
				label = null;
			}

			if (progressView != null) {
				progressView.Dispose ();
				progressView = null;
			}

			if (slider != null) {
				slider.Dispose ();
				slider = null;
			}

			if (toggleSwitch != null) {
				toggleSwitch.Dispose ();
				toggleSwitch = null;
			}

			if (segmented != null) {
				segmented.Dispose ();
				segmented = null;
			}
		}
	}
}
