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
	[Register ("PostTableCell")]
	partial class PostTableCell
	{
		[Outlet]
		UIKit.UILabel author { get; set; }

		[Outlet]
		UIKit.UILabel date { get; set; }

		[Outlet]
		UIKit.UIImageView illustration { get; set; }

		[Outlet]
		UIKit.UILabel title { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (illustration != null) {
				illustration.Dispose ();
				illustration = null;
			}

			if (title != null) {
				title.Dispose ();
				title = null;
			}

			if (author != null) {
				author.Dispose ();
				author = null;
			}

			if (date != null) {
				date.Dispose ();
				date = null;
			}
		}
	}
}
