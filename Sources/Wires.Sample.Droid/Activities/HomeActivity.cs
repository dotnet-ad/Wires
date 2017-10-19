namespace Wires.Sample.Droid
{
	using Android.App;
	using Android.OS;
	using Android.Widget;
	using Wires.Sample.ViewModel;

	[Activity(Label = "Wires controls", Icon = "@mipmap/icon")]
	public class HomeActivity : Android.Support.V7.App.AppCompatActivity
	{
		public HomeViewModel ViewModel { get; private set; }

		#region Controls

		private TextView label;

		private EditText field;

		private Button button;

		private ImageView image;

		private ProgressBar progressView;

		private SeekBar seekbar;

		//private Switch toggleSwitch;

		private ToggleButton toggleButton;

		private CheckBox checkbox;

		#endregion

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			this.SetContentView(Resource.Layout.activity_controls);

			//Controls
			this.label = this.FindViewById<TextView>(Resource.Id.label);
			this.field = this.FindViewById<EditText>(Resource.Id.field);
			this.button = this.FindViewById<Button>(Resource.Id.button);
			this.image = this.FindViewById<ImageView>(Resource.Id.imageView1);
			this.progressView = this.FindViewById<ProgressBar>(Resource.Id.progressView);
			this.seekbar = this.FindViewById<SeekBar>(Resource.Id.seekbar);
			this.toggleButton = this.FindViewById<ToggleButton>(Resource.Id.toggleButton1);
			this.checkbox = this.FindViewById<CheckBox>(Resource.Id.checkbox);

			this.ViewModel = new HomeViewModel();

			this.ViewModel
				.Bind(this.label)
					.Text(vm => vm.Title)
				.Bind(this.field)
					.Text(vm => vm.Entry)
				.Bind(this.button)
					.Click(vm => vm.LoadCommand)
				.Bind(this.toggleButton)
					.Checked(vm => vm.IsActive)
				.Bind(this.checkbox)
					.Checked(vm => vm.IsActive)
				.Bind(this.seekbar)
					.Progress(vm => vm.Amount)
				.Bind(this.progressView)
					.Visible(vm => vm.IsLoading)
				.Bind(this.image)
					.ImageAsync(vm => vm.Illustration)
					.Alpha(vm => vm.Amount)
					.Visible(vm => vm.IsActive);

			this.DisplayToolbar();
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			if (item.ItemId == Android.Resource.Id.Home)
			{
				this.Finish();
			}

			return base.OnOptionsItemSelected(item);
		}
	}
}
