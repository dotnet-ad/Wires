namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using CoreGraphics;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Hidden property

		public static IBinding Visible<TSource, TPropertyType>(this Binder<TSource,UIView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			converter = converter ?? Converters.Default<TPropertyType, bool>();
			return binder.Hidden(property, converter.Chain(Converters.Invert));
		}

		public static IBinding Hidden<TSource, TPropertyType>(this Binder<TSource,UIView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, bool> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Hidden, converter);
		}

		#endregion

		#region TintColor property

		public static IBinding TintColor<TSource, TPropertyType>(this Binder<TSource, UIView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, UIColor> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.TintColor, converter);
		}

		#endregion

		#region BackgroundColor property

		public static Binder<TSource, UIView> BackgroundColor<TSource, TPropertyType>(this Binder<TSource, UIView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, UIColor> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.BackgroundColor, converter);
		}

		#endregion

		#region Alpha property

		public static Binder<TSource, UIView> Alpha<TSource, TPropertyType>(this Binder<TSource, UIView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, nfloat> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Alpha, converter);
		}

		#endregion

		#region Frame property

		public static Binder<TSource, UIView> Frame<TSource, TPropertyType>(this Binder<TSource, UIView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, CGRect> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Frame, converter);
		}

		#endregion

	}
}
