using System;
using System.Linq.Expressions;
using Android.Support.V7.Widget;

namespace Wires
{
	public static partial class UIExtensions
	{
		#region Sources property

		public static Binder<TSource, RecyclerView> Source<TSource, TPropertyType>(this Binder<TSource, RecyclerView> binder, Expression<Func<TSource, TPropertyType>> property, Action<TSource, RecyclerView, CollectionSource<TSource>> registerViews, IConverter<TPropertyType, CollectionSource<TSource>> converter = null)
			where TSource : class
		{
			return binder.ObserveProperty(property, (s, v, collection) =>
			{
				collection.ClearViews();
				registerViews(s, v, collection);
				var adapter = new RecyclerViewAdapterBinding<TSource>(collection);
				v.SetAdapter(adapter);
				adapter.NotifyDataSetChanged(); // TODO ? Needed
			}, converter);
		}

		#endregion
	}
}