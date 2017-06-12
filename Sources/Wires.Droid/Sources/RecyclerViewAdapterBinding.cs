

namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Android.Support.V7.Widget;
	using Android.Views;

	public class RecyclerViewAdapterBinding<TViewModel> : RecyclerView.Adapter
		         where TViewModel : class
	{


		public RecyclerViewAdapterBinding(CollectionSource<TViewModel> datasource)
		{
			this.datasource = datasource;
			this.descriptors = datasource.CellViews.Union(datasource.HeaderViews).Union(datasource.FooterViews).ToList();
		}

		private List<WeakEventHandler<EventArgs>> clickHandlers = new List<WeakEventHandler<EventArgs>>();

		readonly List<CellDescriptor> descriptors;

		readonly CollectionSource<TViewModel> datasource;

		private IEnumerable<ICell> FlatCells => this.datasource.Sections.SelectMany(x =>
		{
			var result = x.Cells.ToList();
			if (x.Header != null) result.Insert(0, x.Header);
			if (x.Footer != null) result.Add(x.Header);
			return result;
		});

		public override int ItemCount => FlatCells.Count();

		public override int GetItemViewType(int position)
		{
			var id = FlatCells.ElementAt(position).ViewIdentifier;
		
			for (int i = 0; i < descriptors.Count; i++)
			{
				if (descriptors[i].Identifier == id)
					return i;
			}
			return -1;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var data = FlatCells.ElementAt(position);
			((IView)holder).ViewModel = data.Item;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			var type = this.descriptors[viewType].ViewType;
			var holder = Activator.CreateInstance(type, new object[] { parent }) as RecyclerView.ViewHolder;
			holder.ItemView.AddWeakHandler<EventArgs>(nameof(View.Click), (s, e) =>
			{
				var i = holder.AdapterPosition;  //FIXME weak reference on holder
				var cell = this.FlatCells.ElementAt(i);
				if (cell.Select?.CanExecute(cell.Item) ?? false)
				{
					cell.Select.Execute(cell.Item);
				}

			}); 
			return holder;
		}
	}
}
