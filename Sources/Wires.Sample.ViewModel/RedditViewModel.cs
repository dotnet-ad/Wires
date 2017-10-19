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

		private IEnumerable<ItemViewModel> items;

		readonly RedditApi api;

		private bool isUpdating, isGrouped;

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

		public bool IsGrouped
		{
			get { return isGrouped; }
			set
			{
				if (this.Set(ref isGrouped, value))
				{
					this.RaiseProperty(nameof(Items));
				}
			}
		}

		public CollectionSource<RedditViewModel> Items
		{
			get 
			{
				var result = new CollectionSource<RedditViewModel>(this);

				Func<RedditViewModel,IEnumerable<ItemViewModel>> getItems = vm => (IEnumerable<ItemViewModel>)vm.items?.OrderByDescending(a => a.Datetime) ?? new ItemViewModel[0];

				if (IsGrouped)
				{
					result.WithSections("cell", "header", getItems, (p) => $"Day n°{p.Datetime.DayOfYear}", null);
				}
				else
				{
					result.WithSection().WithCells("cell", getItems);
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
				this.items = (await this.api.GetTopicAsync("earthporn")).Select(p => new ItemViewModel(p));
				this.RaiseProperty(nameof(Items));
			}
			catch (System.Exception)
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
