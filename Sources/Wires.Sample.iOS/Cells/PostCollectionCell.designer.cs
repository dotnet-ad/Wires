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
	[Register ("PostCollectionCell")]
	partial class PostCollectionCell
	{
		[Outlet]
		UIKit.UIImageView illustration { get; set; }

		[Outlet]
		UIKit.UILabel title { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (title != null) {
				title.Dispose ();
				title = null;
			}

			if (illustration != null) {
				illustration.Dispose ();
				illustration = null;
			}
		}
	}
}
