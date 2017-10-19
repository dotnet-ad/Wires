namespace Wires
{
	using Android.Views;
	using Android.Widget;

	/// <summary>
	/// This class will force to include symbols that are used by reflection and may be ignored during linking phases.
	/// Kinda hacky, but is still the best option for now.
	/// </summary>
	public static class Linking
	{
		public static void All()
		{
			var x1 = new Button(null);
			x1.Click += (s, e) => { };
			x1.Text = x1.Text;

			var x2 = new Switch(null);
			x2.CheckedChange += (s, e) => { };
			x2.Checked = x2.Checked;

			var x3 = new EditText(null);
			x3.TextChanged += (s, e) => { };
			x3.Text = x3.Text;

			var x4 = new View(null);
			x4.Visibility = x4.Visibility;
			x4.Background = x4.Background;
			x4.Alpha = x4.Alpha;

			var x5 = new TextView(null);
			x5.Text = x5.Text;

			var x6 = new CheckBox(null);
			x6.CheckedChange += (s, e) => { };
			x6.Checked = x6.Checked;

			var x7 = new SeekBar(null);
			x7.ProgressChanged += (s, e) => { };
			x7.Progress = x7.Progress;

			var x8 = new ToggleButton(null);
			x8.TextChanged += (s, e) => { };
			x8.Text = x8.Text;
			x8.CheckedChange += (s, e) => { };
			x8.Checked = x8.Checked;

			var x9 = new RadioButton(null);
			x9.CheckedChange += (s, e) => { };
			x9.Checked = x9.Checked;
		}
	}
}
