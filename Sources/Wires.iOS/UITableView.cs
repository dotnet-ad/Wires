namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Windows.Input;
	using Transmute;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Source property

		public static Binder<TSource, UITableView> Source<TSource, TPropertyType>(this Binder<TSource, UITableView> binder, Expression<Func<TSource, TPropertyType>> property, Action<TSource, UITableView, CollectionSource<TSource>> registerViews, IConverter<TPropertyType, CollectionSource<TSource>> converter = null, bool fromNibs = true)
			where TSource : class
		{
			return binder.ObserveProperty(property, (s, v, collection) =>
			{
				collection.ClearViews();
				registerViews(s, v, collection);
				v.Source = new TableViewSourceBinding<TSource>(v, collection, fromNibs);
				v.ReloadData();
			}, converter);
		}

		#endregion

		#region Simple helpers

		public static Binder<TSource, UITableView> Source<TSource, TItem, TCell>(this Binder<TSource, UITableView> binder, Expression<Func<TSource, IEnumerable<TItem>>> property, ICommand select = null, int heightForItem = 44, bool fromNibs = true, Action<float> onScroll = null)
			where TSource : class
			where TCell : IView
		{
			return binder.Source(property,(s, v, c) => 
			{
				c.RegisterCellView<TCell>("cell", heightForItem);
			}, new RelayConverter<IEnumerable<TItem>, CollectionSource<TSource>>((x) => 
			{
				var collection = new CollectionSource<TSource>(binder.Source);
				collection.WithSection().WithCells("cell", (arg) => x, select);
				return collection;
			}), fromNibs);
		}

		public static Binder<TSource, UITableView> Source<TSource, TSection, TItem, THeaderCell, TCell>(this Binder<TSource, UITableView> binder, Expression<Func<TSource, IEnumerable<IGrouping<TSection, TItem>>>> property, ICommand select = null, int heightForHeader = 44, int heightForItem = 44, bool fromNibs = true, Action<float> onScroll = null)
			where TSource : class
			where TCell : IView
			where THeaderCell : IView
		{
			return binder.Source(property, (s, v, c) =>
			 {
				 c.RegisterCellView<TCell>("cell", heightForItem);
				 c.RegisterHeaderView<THeaderCell>("header", heightForHeader);
			 }, new RelayConverter<IEnumerable<IGrouping<TSection, TItem>>, CollectionSource<TSource>>((x) =>
			 {
				 var collection = new CollectionSource<TSource>(binder.Source);
				 collection.WithSections((vm) =>
				{
					return x.Select(e =>
					{
						var section = new Section<TSource>(collection);
						section.WithHeader("header", vm2 => e.Key);
						section.WithCells("cell", vm2 => e, select);
						return section;
					});
				});
				 return collection;
			 }), fromNibs);
		}

		#endregion
	}
}
