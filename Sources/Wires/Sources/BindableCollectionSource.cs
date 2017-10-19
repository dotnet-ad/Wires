namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Windows.Input;

	public class BindableCollectionSource<TOwner, TItem, TView, TCellView> : IDisposable
		where TOwner : class
		where TView : class
	{
		public BindableCollectionSource(TOwner source, Expression<Func<TOwner,IEnumerable<TItem>>>sourceProperty, TView view, Action<TView> triggerReloading, Action<TItem, int, TCellView> prepareCell = null, ICommand selectCommand = null)
		{
			var sourceAccessors = sourceProperty.BuildAccessors();

			if (source is INotifyPropertyChanged)
			{
				this.propertyChangedEvent = source.AddWeakHandler<PropertyChangedEventArgs>(nameof(INotifyPropertyChanged.PropertyChanged), this.OnPropertyChanged);
			}

			this.SelectCommand = selectCommand;
			this.view = new WeakReference<TView>(view);
			this.owner = new WeakReference<TOwner>(source);
			this.triggerReloading = triggerReloading;
			this.prepareCell = prepareCell;
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

		public void Select(int itemIndex) => this.Select(this[itemIndex]);

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
					this.collectionChangedEvent = items.AddWeakHandler<NotifyCollectionChangedEventArgs>(nameof(INotifyCollectionChanged.CollectionChanged), this.OnCollectionChanged);
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

		readonly WeakReference<TView> view;

		readonly Action<TView> triggerReloading;

		private readonly WeakReference<TOwner> owner;

		private readonly Action<TItem, int, TCellView> prepareCell;

		private readonly Func<TOwner, IEnumerable<TItem>> getter;

		private WeakEventHandler<NotifyCollectionChangedEventArgs> collectionChangedEvent;

		readonly WeakEventHandler<PropertyChangedEventArgs> propertyChangedEvent;

		#endregion

		public ICommand SelectCommand { get; private set; }

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

		private IEnumerable<TItem> Collection => getter(this.Owner);

		public int Count => this.Collection?.Count() ?? 0;

		public TItem this[int i]
		{
			get
			{
				var items = this.Collection;
				if (items != null)
					return items.ElementAt(i);
				
				return default(TItem);
			}
		}

		public void PrepareCell(int index, TCellView view)
		{
			var item = this[index];

			if (prepareCell != null)
				this.prepareCell(item, index, view);
		}

		public void Dispose()
		{
			this.propertyChangedEvent.Unsubscribe();
			this.collectionChangedEvent.Unsubscribe();
		}
	}
}
