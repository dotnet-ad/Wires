namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using Transmute;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Progress property

		public static Binder<TSource, UIProgressView> Progress<TSource, TPropertyType>(this Binder<TSource, UIProgressView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, float> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Progress, converter);
		}

		#endregion

		#region ProgressTintColor property

		public static Binder<TSource, UIProgressView> ProgressTintColor<TSource, TPropertyType>(this Binder<TSource, UIProgressView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, UIColor> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.ProgressTintColor, converter);
		}

		#endregion

		#region TrackTintColor property

		public static Binder<TSource, UIProgressView> TrackTintColor<TSource, TPropertyType>(this Binder<TSource, UIProgressView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, UIColor> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.TrackTintColor, converter);
		}

		#endregion
	}
}
