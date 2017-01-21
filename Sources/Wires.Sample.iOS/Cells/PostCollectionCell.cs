using System;

using Foundation;
using UIKit;

namespace Wires.Sample.iOS
{
	public partial class PostCollectionCell : UICollectionViewCell
	{
		public static readonly NSString Key = new NSString("PostCollectionCell");

		public static readonly UINib Nib;

		static PostCollectionCell()
		{
			Nib = UINib.FromName("PostCollectionCell", NSBundle.MainBundle);
		}

		protected PostCollectionCell(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}
	}
}
