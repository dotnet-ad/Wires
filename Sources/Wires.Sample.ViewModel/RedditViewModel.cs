namespace Wires.Sample.ViewModel
{
	using System.Collections.Generic;
	using System.Linq;

	public class RedditViewModel : ViewModelBase
	{
		public RedditViewModel()
		{
			this.api = new RedditApi();
			this.UpdateCommand = new RelayCommand(ExecuteUpdateCommand, CanExecuteUpdateCommand);
		}

		#region Fields

		private IEnumerable<Post> simple;

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

		public IEnumerable<Post> Simple
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

		public Collection<string,Post> Grouped
		{
			get
			{
				var sections = this.Simple.GroupBy(p => p.Datetime.DayOfYear).OrderByDescending(a => a.Key);

				var result = new Collection<string, Post>();

				foreach (var section in sections)
				{
					result.Add(new Collection<string, Post>.Section($"Day n°{section.Key}", section.ToArray()));
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
				this.Simple = await this.api.GetTopicAsync("earthporn");
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
