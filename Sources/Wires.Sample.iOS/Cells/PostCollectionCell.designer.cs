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
    [Register ("PostCollectionCell")]
    partial class PostCollectionCell
    {
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
        }
    }
}