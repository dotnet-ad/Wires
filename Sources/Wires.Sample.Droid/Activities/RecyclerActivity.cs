namespace Wires.Sample.Droid
{
	using Android.Widget;
	using Wires.Sample.ViewModel;
	using Android.Support.V7.Widget;
	using Android.App;
	using Android.OS;

	[Activity(Label = "Wires recycler", MainLauncher = true)]
	public class RecyclerActivity : Android.Support.V7.App.AppCompatActivity
	{
		private RecyclerView recycler;

		private ProgressBar indicator;

		public RedditViewModel ViewModel { get; private set; }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			this.SetContentView(Resource.Layout.activity_recycler);

			this.recycler = this.FindViewById<RecyclerView>(Resource.Id.recycler);
			this.indicator = this.FindViewById<ProgressBar>(Resource.Id.indicator);

			var layout = new LinearLayoutManager(this);
			recycler.SetLayoutManager(layout);

			this.ViewModel = new RedditViewModel();

			this.ViewModel
					.Bind(this.indicator)
						.Visible(vm => vm.IsUpdating)
			    	.Bind(this.recycler)
						.Hidden(vm => vm.IsUpdating)
			    	.Bind(this.recycler)
						.Source(vm => vm.Items, (vm, v, c) =>
						{
							c.RegisterCellView<PostCellHolder>("cell", 44, 44);
							c.RegisterHeaderView<PostHeaderCellHolder>("header", 88, 100);
						});

			this.ViewModel.UpdateCommand.Execute(null);

			this.DisplayToolbar();
		}

		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			if(item.ItemId == Android.Resource.Id.Home)
			{
				this.StartActivity(typeof(HomeActivity));
			}

			return base.OnOptionsItemSelected(item);
		}
	}
}
