namespace Wires
{
	using System.Linq;
	using System.Collections.Generic;
	using System.Windows.Input;
	using System;

	public class Section<TViewModel>
		where TViewModel : class
	{
		public Section(CollectionSource<TViewModel> source)
		{
			this.source = source;
		}

		#region Fields

		private List<Func<TViewModel, IEnumerable<ICell>>> cells = new List<Func<TViewModel, IEnumerable<ICell>>>();

		private Func<TViewModel, ICell> header, footer;

		private readonly CollectionSource<TViewModel> source;

		#endregion

		#region Properties

		public IEnumerable<ICell> Cells => cells.Select(x => x(this.source.ViewModel)).Where(x => x != null).SelectMany(x => x);

		public ICell Header => header?.Invoke(this.source.ViewModel);

		public ICell Footer => footer?.Invoke(this.source.ViewModel);

		#endregion

		public Section<TViewModel> WithSection() => source.WithSection();

		public Section<TViewModel> WithCells<TItem>(string viewIdentifier, Func<TViewModel, IEnumerable<TItem>> items, ICommand select = null)
		{
			cells.Add((vm) => items(vm)?.Select(x => new Cell(viewIdentifier,x) { Select = select }));
			return this;
		}

		public Section<TViewModel> WithCell<TItem>(string viewIdentifier, Func<TViewModel,TItem> item, ICommand select = null)
		{
			cells.Add(vm => new[] { new Cell(viewIdentifier,item) { Select = select } });
			return this;
		}

		public Section<TViewModel> WithHeader<TItem>(string viewIdentifier, Func<TViewModel,TItem> item)
		{
			header = vm => new Cell(viewIdentifier,item(vm));
			return this;
		}

		public Section<TViewModel> WithFooter<TItem>(string viewIdentifier, Func<TViewModel, TItem> item)
		{
			footer = vm => new Cell(viewIdentifier, item(vm));
			return this;
		}
	}
}
