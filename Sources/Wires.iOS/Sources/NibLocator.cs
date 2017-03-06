namespace Wires
{
	using System;
	using System.Collections.Generic;
	using Foundation;
	using UIKit;

	public static class NibLocator
	{
		#region Cell nib

		private static Dictionary<Type, UINib> nibs = new Dictionary<Type, UINib>();

		public static UINib Nib(Type t, string nibName = null)
		{
			UINib nib;

			if (!nibs.TryGetValue(t, out nib))
			{
				nib = UINib.FromName(nibName ?? t.Name, NSBundle.MainBundle);
				nibs.Add(t,nib);
			}

			return nib;
		}

		#endregion
	}
}
