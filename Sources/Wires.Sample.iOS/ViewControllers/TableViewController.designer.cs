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
    [Register ("TableViewController")]
    partial class TableViewController
    {
        [Outlet]
        UIKit.UIActivityIndicatorView indicator { get; set; }


        [Outlet]
        UIKit.UISegmentedControl segmented { get; set; }


        [Outlet]
        UIKit.UITableView tableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (indicator != null) {
                indicator.Dispose ();
                indicator = null;
            }

            if (segmented != null) {
                segmented.Dispose ();
                segmented = null;
            }

            if (tableView != null) {
                tableView.Dispose ();
                tableView = null;
            }
        }
    }
}