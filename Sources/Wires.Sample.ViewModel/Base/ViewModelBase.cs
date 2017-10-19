namespace Wires.Sample.ViewModel
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;

	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		protected bool Set<T>(ref T field, T value, [CallerMemberName]string name = null)
		{
			if (!EqualityComparer<T>.Default.Equals(field, value))
			{
				field = value;
				RaiseProperty(name);
				return true;
			}

			return false;
		}

		public void RaiseProperty(string property) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
