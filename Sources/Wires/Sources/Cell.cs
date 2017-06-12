namespace Wires
{
	using System;
	using System.Windows.Input;

	public interface ICell
	{
		#region Properties

		string ViewIdentifier { get; }

		object Item { get; }

		ICommand Select { get; }

		#endregion
	}

	public class Cell : ICell
	{
		public Cell(string viewIdentifier, object item)
		{
			this.Item = item;
			this.ViewIdentifier = viewIdentifier;
		}

		#region Properties

		public string ViewIdentifier { get; }

		public object Item { get; private set; }

		public ICommand Select { get; set; }

		#endregion
	}
}
