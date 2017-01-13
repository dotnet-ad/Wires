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
				this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
				return true;
			}

			return false;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
