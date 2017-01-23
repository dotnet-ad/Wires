namespace Wires.Sample.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class RedditViewModel : ViewModelBase
	{
		public class ItemViewModel : ViewModelBase // To test subview-models
		{
			public ItemViewModel(Post model)
			{
				this.model = model;
			}

			readonly Post model;

			public string Title => model.Title;

			public string Author => model.Author;

			public DateTime Datetime => model.Datetime;

			public string Thumbnail => model.Thumbnail;
		}

		public RedditViewModel()
		{
			this.api = new RedditApi();
			this.UpdateCommand = new RelayCommand(ExecuteUpdateCommand, CanExecuteUpdateCommand);
		}

		#region Fields

		private IEnumerable<ItemViewModel> simple;

		readonly RedditApi api;

		private bool isUpdating;

		#endregion

		#region Properties

		public bool IsUpdating
		{
			get { return isUpdating; }
			set
			{
				if (this.Set(ref isUpdating, value))
				{
					this.UpdateCommand.RaiseCanExecuteChanged();
				}
			}
		}

		public IEnumerable<ItemViewModel> Simple
		{
			get { return simple; }
			set 
			{
				if (this.Set(ref simple, value))
				{
					this.RaiseProperty(nameof(Grouped));
				}
			}
		}

		public Collection<string,ItemViewModel> Grouped
		{
			get
			{
				var sections = this.Simple.GroupBy(p => p.Datetime.DayOfYear).OrderByDescending(a => a.Key);

				var result = new Collection<string, ItemViewModel>();

				foreach (var section in sections)
				{
					result.Add(new Collection<string, ItemViewModel>.Section($"Day n°{section.Key}", section.ToArray()));
				}

				return result;
			}
		}

		#endregion

		#region Commands

		public RelayCommand UpdateCommand { get; private set; }

		private bool CanExecuteUpdateCommand() => !this.IsUpdating;

		private async void ExecuteUpdateCommand()
		{
			try
			{
				this.IsUpdating = true;
				this.Simple = (await this.api.GetTopicAsync("earthporn")).Select(p => new ItemViewModel(p));
			}
			catch (System.Exception ex)
			{

			}
			finally
			{
				this.IsUpdating = false;
			}
		}

		#endregion
	}
}
