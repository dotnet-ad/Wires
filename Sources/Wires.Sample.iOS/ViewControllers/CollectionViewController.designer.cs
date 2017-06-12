// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
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
            if (collectionView != null) {
                collectionView.Dispose ();
                collectionView = null;
            }

            if (indicator != null) {
                indicator.Dispose ();
                indicator = null;
            }

            if (segmented != null) {
                segmented.Dispose ();
                segmented = null;
            }
        }
    }
}