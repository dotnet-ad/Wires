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

		public static Binder<TSource, UICollectionView> Source<TSource>(this Binder<TSource, UICollectionView> binder, Expression<Func<TSource, CollectionSource<TSource>>> property, Action<TSource,UICollectionView,CollectionSource<TSource>> registerViews, bool fromNibs = true)
		where TSource : class
		{
			return binder.ObserveProperty(property, (s, v, collection) =>
			{
				collection.ClearViews();
				registerViews(s,v,collection);
				v.Source = new CollectionViewSourceBinding<TSource>(v, collection, fromNibs);
				v.ReloadData();
			});
		}

		#endregion
	}
}
