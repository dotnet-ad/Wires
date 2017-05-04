namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Input;

	public class CollectionSource<TViewModel> 
		where TViewModel : class
	{
		public CollectionSource(TViewModel viewModel)
		{
			this.ViewModelReference = new WeakReference<TViewModel>(viewModel);
		}

		#region Fields

		private List<Func<TViewModel,IEnumerable<Section<TViewModel>>>> sections = new List<Func<TViewModel,IEnumerable<Section<TViewModel>>>>();

		private List<CellDescriptor> cellViews = new List<CellDescriptor>(), headerViews = new List<CellDescriptor>(), footerViews = new List<CellDescriptor>();

		#endregion

		#region Properties

		public TViewModel ViewModel
		{
			get
			{
				TViewModel vm;
				if (this.ViewModelReference.TryGetTarget(out vm))
				{
					return vm;
				}
				throw new InvalidOperationException("Binding is not alive anymore");
			}
		}

		public IEnumerable<CellDescriptor> CellViews => cellViews.ToArray();

		public IEnumerable<CellDescriptor> HeaderViews => headerViews.ToArray();

		public IEnumerable<CellDescriptor> FooterViews => footerViews.ToArray();

		public IEnumerable<Section<TViewModel>> Sections => sections.Select(x => x(this.ViewModel)).Where(x => x != null).SelectMany(x => x);

		public WeakReference<TViewModel> ViewModelReference { get; }

		#endregion

		#region Views

		public void ClearViews()
		{
			this.cellViews.Clear();
			this.headerViews.Clear();
		}

		public CellDescriptor GetCellView(string identifier) => this.cellViews.FirstOrDefault(x => x.Identifier == identifier);

		public CellDescriptor GetHeaderView(string identifier) => this.headerViews.FirstOrDefault(x => x.Identifier == identifier);

		public CellDescriptor GetFooterView(string identifier) => this.footerViews.FirstOrDefault(x => x.Identifier == identifier);

		public CollectionSource<TViewModel> RegisterCellView<T>(string identifier, float height = -1, float width = -1) where T : IView
		{
			this.cellViews.Add(new CellDescriptor(identifier, typeof(T), width, height));
			return this;
		}

		public CollectionSource<TViewModel> RegisterHeaderView<T>(string identifier, float height = -1, float width = -1) where T : IView
		{ 
			this.headerViews.Add(new CellDescriptor(identifier, typeof(T), width, height));
			return this;
		}

		public CollectionSource<TViewModel> RegisterFooterView<T>(string identifier, float height = -1, float width = -1) where T : IView
		{
			this.footerViews.Add(new CellDescriptor(identifier, typeof(T), width, height));
			return this;
		}

		#endregion

		public Section<TViewModel> WithSection()
		{
			var section = new Section<TViewModel>(this);
			sections.Add((vm) => new[] { section });
			return section;
		}

		public CollectionSource<TViewModel> WithSections<TKey, TItem>(string cellIdentifier, string headerIdentifier,Func<TViewModel,IEnumerable<TItem>> items, Func<TItem,TKey> selector, ICommand select)
		{
			sections.Add((vm) => items(vm)?.GroupBy(selector).Select(x =>
			{
				var section  = new Section<TViewModel>(this);

				section.WithHeader(headerIdentifier, vm2 => x.Key);
				section.WithCells(cellIdentifier, vm2 => x,select);

				return section;
			}) );

			return this;
		}
	}
}
