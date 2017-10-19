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
	[Register ("CollectionViewController")]
	partial class CollectionViewController
	{
		[Outlet]
		UIKit.UICollectionView collectionView { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView indicator { get; set; }

		[Outlet]
		UIKit.UISegmentedControl segmented { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (indicator != null) {
				indicator.Dispose ();
				indicator = null;
			}

			if (collectionView != null) {
				collectionView.Dispose ();
				collectionView = null;
			}

			if (segmented != null) {
				segmented.Dispose ();
				segmented = null;
			}
		}
	}
}
