using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wires.Sample.ViewModel
{
	public class HomeViewModel : ViewModelBase
	{
		private static string[] Images =
		{
			"https://github.com/aloisdeniel/Wires/raw/develop/Documentation/Logo.png",
			"http://i.imgur.com/xzsmoWB.jpg",
			"https://i.reddituploads.com/ca996e077cf949cb9d513356a28e70fc?fit=max&h=1536&w=1536&s=f9f0852b5630467a26911237430b3ef4",
			"https://i.redd.it/e516mt1qujay.jpg",
			"https://i.reddituploads.com/5e1f26943ced4adf94fc610c3b4e3201?fit=max&h=1536&w=1536&s=748a3e8b1fcc4b7f7fcafcbe6f97a9bf",
			"https://i.reddituploads.com/4f6d53d29453403fa5da8607bdc18b60?fit=max&h=1536&w=1536&s=87647d9050b46371c5b41a820d515e60",
			"http://i.imgur.com/qkpagbu.jpg",
		};

		public HomeViewModel()
		{
			this.Title = "Wires";
			this.Illustration = null;
			this.Amount = 0.45;
			this.IsActive = true;
			this.birthday = new DateTime(1988, 6, 2);
			this.loadCommand = new RelayCommand(ExecuteLoadCommand, CanExecuteLoadCommand);
		}

		#region Fields

		private string title;

		private string illustration;

		private bool isLoading;

		private bool isActive;

		private double amount;

		private DateTime birthday;

		private int selected;

		private string entry;

		#endregion

		#region Properties

		public string Title
		{
			get { return title + $" ({selected})({entry})({amount})"; }
			set 
			{ 
				if (this.Set(ref title, value))
				{
					RaiseProperty(nameof(Sections));
				} 
			}
		}

		public string Entry
		{
			get { return entry; }
			set { if (this.Set(ref entry, value)) RaiseProperty(nameof(Title)); }
		}

		public string Illustration
		{
			get { return illustration; }
			set { this.Set(ref illustration, value); }
		}

		public string[] Sections => new[] { Title, Amount.ToString(), IsActive.ToString()  };

		public int Selected 
		{
			get { return selected; }
			set { if (this.Set(ref selected, value)) RaiseProperty(nameof(Title)); }
		}

		public double Amount
		{
			get { return amount; }
			set 
			{ 
				if(this.Set(ref amount, value))
				{
					RaiseProperty(nameof(Sections));
					RaiseProperty(nameof(Title));
				}
			}
		}

		public bool IsActive
		{
			get { return isActive; }
			set { if (this.Set(ref isActive, value)) RaiseProperty(nameof(Sections)); }
		}

		public DateTime Birthday
		{
			get { return birthday; }
			set { this.Set(ref birthday, value); }
		}

		public bool IsLoading
		{
			get { return isLoading; }
			set
			{
				if (this.Set(ref isLoading, value))
				{
					this.loadCommand.RaiseCanExecuteChanged();
				}
			}
		}

		#endregion

		private RelayCommand loadCommand;

		public ICommand LoadCommand => loadCommand;

		int image = -1;

		async void ExecuteLoadCommand()
		{
			this.IsLoading = true;
			await Task.Delay(2000);
			this.Illustration = Images[image = (image + 1) % Images.Length];
			this.IsLoading = false;
		}

		bool CanExecuteLoadCommand() => !this.IsLoading;
	}
}
