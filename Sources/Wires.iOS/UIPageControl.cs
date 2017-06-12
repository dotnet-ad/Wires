using System;
using System.Linq.Expressions;
using UIKit;
namespace Wires
{
    public static partial class UIExtensions
    {

        #region CurrentPageIndicatorTintColor property

        public static Binder<TSource, UIPageControl> CurrentPageIndicatorTintColor<TSource, TPropertyType>(this Binder<TSource, UIPageControl> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, UIColor> converter = null)
             where TSource : class
        {
            return binder.Property(property, b => b.CurrentPageIndicatorTintColor, converter);
        }

		#endregion

		#region CurrentPage property

		public static Binder<TSource, UIPageControl> CurrentPage<TSource, TPropertyType>(this Binder<TSource, UIPageControl> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, nint> converter = null)
			where TSource : class
		{
            return binder.Property(property, b => b.CurrentPage, converter);
		}

        #endregion

        #region Pages

        public static Binder<TSource, UIPageControl> Pages<TSource, TPropertyType>(this Binder<TSource, UIPageControl> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, nint> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Pages, converter);
		}

        #endregion
    }
}
