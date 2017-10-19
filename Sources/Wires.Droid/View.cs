namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Android.Graphics.Drawables;
	using Android.Views;
	using Android.Widget;

	public static partial class UIExtensions
	{
		#region Hidden property

		public static Binder<TSource, View> Visibility<TSource, TPropertyType>(this Binder<TSource, View> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, ViewStates> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Visibility, converter);
		}

		#endregion


		#region BackgroundColor property

		public static Binder<TSource, View> Background<TSource, TPropertyType>(this Binder<TSource, View> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, Drawable> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Background, converter);
		}

		#endregion

		#region Alpha property

		public static Binder<TSource, View> Alpha<TSource, TPropertyType>(this Binder<TSource, View> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, float> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Alpha, converter);
		}

		#endregion
	}
}
