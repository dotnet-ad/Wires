using Transmute;
using System;
namespace Wires.Sample.Droid
{
	using Android.Support.V7.Widget;
	using Android.Views;
	using Android.Widget;
	using Wires.Sample.ViewModel;

	public class PostCellHolder : RecyclerView.ViewHolder, IView
	{
		private TextView title, author, date;

		private ImageView illustration;

		public PostCellHolder(LayoutInflater inflater, ViewGroup vgroup) : base(inflater.Inflate(Resource.Layout.cell_post, vgroup, false))
		{
			this.title = this.ItemView.FindViewById<TextView>(Resource.Id.title);
			this.author = this.ItemView.FindViewById<TextView>(Resource.Id.author);
			this.date = this.ItemView.FindViewById<TextView>(Resource.Id.date);
			this.illustration = this.ItemView.FindViewById<ImageView>(Resource.Id.illustration);
		}

		private RedditViewModel.ItemViewModel viewModel;

		public RedditViewModel.ItemViewModel ViewModel
		{
			get { return this.viewModel; }
			set
			{
				if (this.viewModel != value)
				{
					this.viewModel = value;

					value
						.Bind(title)
							.Text(vm => vm.Title)
						.Bind(author)
							.Text(vm => vm.Author)
						.Bind(date)
							.Text(vm => vm.Datetime)
						.Bind(illustration)
							.ImageAsync(vm => vm.Thumbnail); //PlatformConverters.AsyncStringToCachedImage(TimeSpan.FromHours(1))
				}
			}
		}

		object IView.ViewModel
		{
			get { return this.ViewModel; }
			set { this.ViewModel = value as RedditViewModel.ItemViewModel; }
		}
	}
}
