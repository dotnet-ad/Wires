namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Windows.Input;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Source property

		public static TableViewSourceBinding<TSource, TItem, TCell> Source<TSource, TItem, TCell>(this Binder<TSource, UITableView> binder, Expression<Func<TSource, IEnumerable<TItem>>> property, Action<TItem,int,TCell> prepareCell, ICommand select = null,  Func<int, nfloat> heightForItem = null, bool fromNib = true, Action<float> onScroll = null)
			where TSource : class
			where TCell : UITableViewCell
		{
			heightForItem = heightForItem ?? ((i) => 44);

			var s = new BindableCollectionSource<TSource, TItem, UITableView, TCell>(binder.Source, property, binder.Target, (obj) => obj.ReloadData(), prepareCell, select);

			var result = new TableViewSourceBinding<TSource, TItem, TCell>(s, heightForItem, fromNib, onScroll);
			binder.Target.Source = result;
			return result;
		}

		public static GroupedTableViewSourceBinding<TSource, IEnumerable<IGrouping<TSection, TItem>>, TSection, TItem, TCell, THeaderCell> Source<TSource, TSection, TItem, THeaderCell, TCell>(this Binder<TSource, UITableView> binder, Expression<Func<TSource, IEnumerable<IGrouping<TSection,TItem>>>> property, Action<TSection, int, THeaderCell> prepareHeader, Action<TItem, Index, TCell> prepareCell, ICommand select = null, Func<int, nfloat> heightForHeader = null, Func<Index, nfloat> heightForItem = null, bool fromNib = true, Action<float> onScroll = null)
			where TSource : class
			where THeaderCell : UITableViewHeaderFooterView
			where TCell : UITableViewCell
		{
			heightForItem = heightForItem ?? ((i) => 44);
			heightForHeader = heightForHeader ?? ((i) => 32);

			var s = new BindableGroupedCollectionSource<TSource, TSection, TItem, UITableView, TCell, THeaderCell>(binder.Source, property, binder.Target, (obj) => obj.ReloadData(), prepareCell, prepareHeader, select);

			var result = new GroupedTableViewSourceBinding<TSource, IEnumerable<IGrouping<TSection, TItem>>, TSection, TItem, TCell, THeaderCell>(s, heightForItem, heightForHeader, fromNib, onScroll);
			binder.Target.Source = result;
			return result;
		}

		#endregion
	}
}
