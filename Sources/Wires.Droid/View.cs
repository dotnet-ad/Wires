namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Android.Graphics.Drawables;
	using Android.Views;
	using Transmute;

	public static partial class UIExtensions
	{
		#region Visibility property

		private static IConverter invert = new RelayConverter<bool, bool>(x => !x);

		public static Binder<TSource,TView> Visibility<TSource, TView, TPropertyType>(this Binder<TSource, TView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, ViewStates> converter = null)
			where TSource : class
			where TView : View
		{
			return binder.Property(property, b => b.Visibility, converter);
		}

		public static Binder<TSource, TView> Visible<TSource, TView, TPropertyType>(this Binder<TSource, TView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
			where TView : View
		{
			converter = converter ?? Transmuter.Default.GetConverter<TPropertyType, bool>();
			var toState = Transmuter.Default.GetConverter<bool, ViewStates>();
			var stateconverter = new TypedConverter<TPropertyType, ViewStates>(new ChainConverter(converter, toState));
			return binder.Visibility(property, stateconverter);
		}

		public static Binder<TSource, TView> Hidden<TSource, TView, TPropertyType>(this Binder<TSource, TView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
			where TView : View
		{
			converter = converter ?? Transmuter.Default.GetConverter<TPropertyType, bool>();
			converter = new TypedConverter<TPropertyType, bool>(new ChainConverter(converter, invert));
			return binder.Visible(property, converter);
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
