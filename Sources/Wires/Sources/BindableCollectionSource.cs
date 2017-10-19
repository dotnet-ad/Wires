namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Linq;
	using System.Linq.Expressions;

	public class BindableCollectionSource<TOwner, TItem, TView, TCellView>
		where TOwner : class
		where TView : class
	{
		public BindableCollectionSource(TOwner source, Expression<Func<TOwner,IEnumerable<TItem>> >sourceProperty, TView view, Action<TView> triggerReloading, Action<TItem, int, TCellView> prepareCell = null)
		{
			var sourceAccessors = sourceProperty.BuildAccessors();

			if (source is INotifyPropertyChanged)
			{
				this.propertyChangedEvent = source.AddWeakHandler<PropertyChangedEventArgs>(nameof(INotifyPropertyChanged.PropertyChanged), this.OnPropertyChanged);
			}

			this.view = new WeakReference<TView>(view);
			this.owner = new WeakReference<TOwner>(source);
			this.triggerReloading = triggerReloading;
			this.prepareCell = prepareCell;
			this.CellIdentifer = typeof(TCellView).Name;
			this.getter = sourceAccessors.Item1;
			this.sourceProperty = sourceAccessors.Item3;
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

		public void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			if (args.PropertyName == sourceProperty)
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

		public string CellIdentifer { get; private set; }

		public int Count
		{
			get
			{
				TOwner owner;
				if (this.owner.TryGetTarget(out owner))
				{
					var items = getter(owner);
					return items?.Count() ?? 0;
				}

				throw new InvalidOperationException("Source's owner has been garbage collected");
			}
		}

		public TItem this[int i]
		{
			get
			{
				TOwner owner;
				if (this.owner.TryGetTarget(out owner))
				{
					var items = getter(owner);
					return items.ElementAt(i);
				}

				throw new InvalidOperationException("Source's owner has been garbage collected");
			}
		}

		public void PrepareCell(int index, TCellView view)
		{
			var item = this[index];

			if (prepareCell != null)
				this.prepareCell(item, index, view);
		}
	}
}
