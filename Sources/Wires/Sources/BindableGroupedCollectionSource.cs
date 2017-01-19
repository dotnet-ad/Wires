namespace Wires
{
	using System;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using System.Windows.Input;

	public class BindableGroupedCollectionSource<TOwner, TSection, TItem, TView, TCellView, THeaderCellView> : BindableGroupedCollectionSource<TOwner, Collection<TSection, TItem>, TSection, TItem, TView, TCellView, THeaderCellView>
		where TOwner : class
		where TView : class
	{
		public BindableGroupedCollectionSource(TOwner source, Expression<Func<TOwner, Collection<TSection, TItem>>> sourceProperty, TView view, Action<TView> triggerReloading, Action<TItem, Index, TCellView> prepareCell = null, Action<TSection, int, THeaderCellView> prepareHeader = null, ICommand selectCommand = null) : base(source, sourceProperty, ((a, i) => a[i].Key), ((a) => a.Count), ((a, i) => a[i].Count), ((a, i) => a[i.Section][i.Item]), view, triggerReloading, prepareCell, prepareHeader, selectCommand) { }
	}

	public class BindableGroupedCollectionSource<TOwner, TCollection, TSection, TItem, TView, TCellView, THeaderCellView> : IDisposable
		where TOwner : class
		where TView : class
		where TCollection : class
	{
		public BindableGroupedCollectionSource(TOwner source, Expression<Func<TOwner, TCollection>> sourceProperty, Func<TCollection, int, TSection> getSection, Func<TCollection, int> countSections, Func<TCollection, int, int> countItems, Func<TCollection, Index, TItem> getItem, TView view, Action<TView> triggerReloading, Action<TItem, Index, TCellView> prepareCell = null, Action<TSection, int, THeaderCellView> prepareHeader = null, ICommand selectCommand = null)
		{
			var sourceAccessors = sourceProperty.BuildAccessors();

			if (source is INotifyPropertyChanged)
			{
				this.propertyChangedEvent = source.AddWeakHandler<PropertyChangedEventArgs>(nameof(INotifyPropertyChanged.PropertyChanged), this.OnPropertyChanged);
			}

			this.getSection = getSection;
			this.getItem = getItem;
			this.countItems = countItems;
			this.countSections = countSections;
			this.SelectCommand = selectCommand;
			this.view = new WeakReference<TView>(view);
			this.owner = new WeakReference<TOwner>(source);
			this.triggerReloading = triggerReloading;
			this.prepareCell = prepareCell;
			this.prepareHeader = prepareHeader;
			this.getter = sourceAccessors.Item1;
			this.sourceProperty = sourceAccessors.Item3;

			UpdateProperty();
		}

		#region Dynamic updates

		private void Reload()
		{
			TView view;
			if (this.view.TryGetTarget(out view))
			{
				this.triggerReloading(view);
			}
		}

		public void Select(TItem item)
		{
			if (this.SelectCommand?.CanExecute(item) ?? false)
			{
				this.SelectCommand.Execute(item);
			}
		}

		public void Select(Index itemIndex) => this.Select(this[itemIndex]);

		private void UpdateProperty()
		{
			if (this.collectionChangedEvent != null)
			{
				this.collectionChangedEvent.Unsubscribe();
				this.collectionChangedEvent = null;
			}

			TOwner owner;
			if (this.owner.TryGetTarget(out owner))
			{
				var items = getter(owner);

				if (items is INotifyCollectionChanged)
				{
					this.collectionChangedEvent = items.AddWeakHandler<NotifyCollectionChangedEventArgs>(nameof(INotifyPropertyChanged.PropertyChanged), this.OnCollectionChanged);
				}
			}

			this.Reload();
		}

		public void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == sourceProperty)
			{
				this.UpdateProperty();
			}
		}


		public void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			this.Reload(); // TODO Reload few elements only
		}

		#endregion

		#region Fields

		private string sourceProperty;

		readonly Func<TCollection, int, TSection> getSection;

		readonly Func<TCollection, Index, TItem> getItem;

		readonly Func<TCollection, int, int> countItems;

		readonly Func<TCollection, int> countSections;

		readonly WeakReference<TView> view;

		readonly Action<TView> triggerReloading;

		private readonly WeakReference<TOwner> owner;

		readonly Action<TSection, int, THeaderCellView> prepareHeader;

		private readonly Action<TItem, Index, TCellView> prepareCell;

		private readonly Func<TOwner, TCollection> getter;

		private WeakEventHandler<NotifyCollectionChangedEventArgs> collectionChangedEvent;

		readonly WeakEventHandler<PropertyChangedEventArgs> propertyChangedEvent;

		#endregion

		public ICommand SelectCommand { get; private set; }

		public TCollection Collection => getter(this.Owner);

		public TView View
		{
			get
			{
				TView view;
				if (this.view.TryGetTarget(out view))
				{
					return view;
				}

				throw new InvalidOperationException("View's owner has been garbage collected");
			}
		}

		public TOwner Owner
		{
			get
			{
				TOwner owner;
				if (this.owner.TryGetTarget(out owner))
				{
					return owner;
				}

				throw new InvalidOperationException("Source's owner has been garbage collected");
			}
		}

		public int SectionsCount
		{
			get
			{
				var items = this.Collection;
				return items != null ? countSections(items) : 0;
			}
		}

		public int ItemsCount(int section)
		{
			var items = this.Collection;
			return items != null ? countItems(items, section) : 0;
		}

		public TItem this[Index i]
		{
			get
			{
				var items = this.Collection;
				return getItem(items, i);
			}
		}

		public TSection this[int section]
		{
			get
			{
				var items = this.Collection;
				return getSection(items, section);
			}
		}

		public void PrepareCell(Index index, TCellView view)
		{
			var item = this[index];

			if (prepareCell != null)
				this.prepareCell(item, index, view);
		}

		public void PrepareHeader(int section, THeaderCellView view)
		{
			var item = this[section];

			if (prepareCell != null)
				this.prepareHeader(item, section, view);
		}

		public void Dispose()
		{
			this.propertyChangedEvent.Unsubscribe();
			this.collectionChangedEvent.Unsubscribe();
		}
	}
}
