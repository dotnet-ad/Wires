namespace Wires.Sample.ViewModel
{
	public class HomeViewModel : ViewModelBase
	{
		public HomeViewModel()
		{
		}

		private string title;

		public string Title
		{
			get { return title; }
			set { this.Set(ref title, value); }
		}

	}
}
