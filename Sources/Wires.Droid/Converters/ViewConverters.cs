using System;
using Android.Views;

namespace Wires
{
	public partial class PlatformConverters
	{
		public static IConverter<bool, ViewStates> BoolToViewState => new RelayConverter<bool, ViewStates>(v => v ? ViewStates.Visible : ViewStates.Gone);
	}
}
