namespace Wires.Sample.Droid
{
	using Android.App;
	using Android.Support.V7.App;

	public static class ActivityExtensions
	{
		public static void DisplayToolbar(this AppCompatActivity activity)
		{
			var toolbar = activity.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			activity.SetSupportActionBar(toolbar);

			activity.SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			activity.SupportActionBar.SetDisplayShowHomeEnabled(true);
		}
	}
}
