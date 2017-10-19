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
            if (author != null) {
                author.Dispose ();
                author = null;
            }

            if (date != null) {
                date.Dispose ();
                date = null;
            }

            if (illustration != null) {
                illustration.Dispose ();
                illustration = null;
            }

            if (title != null) {
                title.Dispose ();
                title = null;
            }
        }
    }
}