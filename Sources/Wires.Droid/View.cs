namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Android.Graphics.Drawables;
	using Android.Views;

	public static partial class UIExtensions
	{
		#region Visibility property

		public static Binder<TSource,TView> Visibility<TSource, TView, TPropertyType>(this Binder<TSource, TView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, ViewStates> converter = null)
			where TSource : class
			where TView : View
		{
			return binder.Property(property, b => b.Visibility, converter);
		}

		#endregion


		#region BackgroundColor property

		public static Binder<TSource, TView> Background<TSource, TView, TPropertyType>(this Binder<TSource, TView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, Drawable> converter = null)
			where TSource : class
			where TView : View
		{
			return binder.Property(property, b => b.Background, converter);
		}

		#endregion

		#region Alpha property

		public static Binder<TSource, TView> Alpha<TSource, TView, TPropertyType>(this Binder<TSource, TView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, float> converter = null)
			where TSource : class
			where TView : View
		{
			return binder.Property(property, b => b.Alpha, converter);
		}

		#endregion
	}
}
