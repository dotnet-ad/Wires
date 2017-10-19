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

		public static Binder<TSource, UITableView> Source<TSource>(this Binder<TSource, UITableView> binder, Expression<Func<TSource, CollectionSource<TSource>>> property, Action<TSource,UITableView,CollectionSource<TSource>> registerViews, bool fromNibs = true)
			where TSource : class
		{
			return binder.ObserveProperty(property, (s, v, collection) =>
			{
				collection.ClearViews();
				registerViews(s,v, collection);
				v.Source = new TableViewSourceBinding<TSource>(v, collection, fromNibs);
				v.ReloadData();
			});
		}

		#endregion
	}
}
