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

		public static CollectionViewSourceBinding<TSource, TItem, TCell> Source<TSource, TItem, TCell>(this Binder<TSource, UICollectionView> binder, Expression<Func<TSource, IEnumerable<TItem>>> property, Action<TItem, int, TCell> prepareCell, ICommand select = null, bool fromNib = true)
			where TSource : class
			where TCell : UICollectionViewCell
		{
			var s = new BindableCollectionSource<TSource, TItem, UICollectionView, TCell>(binder.Source, property, binder.Target, (obj) => obj.ReloadData(), prepareCell, select);

			var result = new CollectionViewSourceBinding<TSource, TItem, TCell>(s, fromNib);
			binder.Target.Source = result;
			return result;
		}

		public static GroupedCollectionViewSourceBinding<TSource, IEnumerable<IGrouping<TSection, TItem>>, TSection, TItem, TCell, THeaderCell> Source<TSource, TSection, TItem, THeaderCell, TCell>(this Binder<TSource, UICollectionView> binder, Expression<Func<TSource, IEnumerable<IGrouping<TSection, TItem>>>> property, Action<TSection, int, THeaderCell> prepareHeader, Action<TItem, Index, TCell> prepareCell, ICommand select = null, bool fromNib = true)
			where TSource : class
			where THeaderCell : UICollectionReusableView
			where TCell : UICollectionViewCell
		{
			var s = new BindableGroupedCollectionSource<TSource, TSection, TItem, UICollectionView, TCell, THeaderCell>(binder.Source, property, binder.Target, (obj) => obj.ReloadData(), prepareCell, prepareHeader, select);

			var result = new GroupedCollectionViewSourceBinding<TSource, IEnumerable<IGrouping<TSection, TItem>>, TSection, TItem, TCell, THeaderCell>(s, fromNib);
			binder.Target.Source = result;
			return result;
		}

		#endregion
	}
}
