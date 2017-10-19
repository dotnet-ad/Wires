namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Transmute;
	using UIKit;

	public static partial class UIExtensions
	{
		#region Image property

		public static Binder<TSource, UIImageView> Image<TSource, TPropertyType>(this Binder<TSource,UIImageView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, UIImage> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => b.Image, converter);
		}

		public static Binder<TSource, UIImageView> ImageAsync<TSource, TPropertyType>(this Binder<TSource, UIImageView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, Task<UIImage>> converter, UIImage loading = null)
			where TSource : class
		{
			return binder.PropertyAsync(property, b => b.Image, converter, loading);
		}

		public static Binder<TSource, UIImageView> ImageAsync<TSource>(this Binder<TSource, UIImageView> binder, Expression<Func<TSource, string>> property, TimeSpan cacheExpiration = default(TimeSpan), UIImage loading = null)
			where TSource : class
		{
			if (cacheExpiration == default(TimeSpan))
				cacheExpiration = TimeSpan.FromDays(1);

			return binder.ImageAsync(property,UIImageConverters.FromStringToCachedImage(cacheExpiration), loading);
		}

		#endregion
	}
}
