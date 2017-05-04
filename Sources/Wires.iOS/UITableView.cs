namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Source property

		public static Binder<TSource, UITableView> Source<TSource, TPropertyType>(this Binder<TSource, UITableView> binder, Expression<Func<TSource, TPropertyType>> property, Action<TSource,UITableView,CollectionSource<TSource>> registerViews, IConverter<TPropertyType, CollectionSource<TSource>> converter = null, bool fromNibs = true)
			where TSource : class
		{
			return binder.ObserveProperty(property, (s, v, collection) =>
			{
				collection.ClearViews();
				registerViews(s,v, collection);
				v.Source = new TableViewSourceBinding<TSource>(v, collection, fromNibs);
				v.ReloadData();
			}, converter);
		}

		#endregion
	}
}
