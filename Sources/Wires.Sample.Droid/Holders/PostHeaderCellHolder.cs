
namespace Wires.Sample.Droid
{
	using Android.Support.V7.Widget;
	using Android.Views;
	using Android.Widget;

	public class PostHeaderCellHolder : RecyclerView.ViewHolder, IView
	{
		private TextView title;

		public PostHeaderCellHolder(LayoutInflater inflater, ViewGroup vgroup) : base(inflater.Inflate(Resource.Layout.cell_post, vgroup, false))
		{
			this.title = this.ItemView.FindViewById<TextView>(Resource.Id.title);
		}

		private string viewModel;

		public string ViewModel
		{
			get { return this.viewModel; }
			set
			{
				if (this.viewModel != value)
				{
					this.viewModel = value;

					value
						.Bind(title)
							.Text(vm => vm);
				}
			}
		}

		object IView.ViewModel
		{
			get { return this.ViewModel; }
			set { this.ViewModel = value as string; }
		}
	}
}
