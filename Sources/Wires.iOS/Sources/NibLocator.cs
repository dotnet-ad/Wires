namespace Wires
{
	using System;
	using UIKit;

	public static class NibLocator<TCellView>
	{
		#region Cell nib

		private static Lazy<UINib> nib = new Lazy<UINib>(() => UINib.FromName(typeof(TCellView).Name, NSBundle.MainBundle));

		public static UINib Nib => nib.Value;

		#endregion
	}
}
