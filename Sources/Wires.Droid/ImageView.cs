namespace Wires
{
	using System;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using Android.Graphics;
	using Android.Graphics.Drawables;
	using Android.Widget;
	using Transmute;

	public static partial class UIExtensions
	{
		#region Image property

		public static Binder<TSource, ImageView> Image<TSource, TPropertyType>(this Binder<TSource, ImageView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, Bitmap> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => ((BitmapDrawable)b.Drawable).Bitmap,  (b,v) => b.SetImageBitmap(v), converter);
		}

		public static Binder<TSource, ImageView> ImageAsync<TSource, TPropertyType>(this Binder<TSource, ImageView> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, Task<Bitmap>> converter, Bitmap loading = null)
			where TSource : class
		{
			return binder.PropertyAsync(property, b => ((BitmapDrawable)b.Drawable).Bitmap, (b, v) => b.SetImageBitmap(v), converter, loading);
		}

		public static Binder<TSource, ImageView> ImageAsync<TSource>(this Binder<TSource, ImageView> binder, Expression<Func<TSource, string>> property, TimeSpan cacheExpiration = default(TimeSpan), Bitmap loading = null)
			where TSource : class
		{
			if (cacheExpiration == default(TimeSpan))
				cacheExpiration = TimeSpan.FromDays(1);

			return binder.ImageAsync(property, Transmute.BitmapConverters.FromStringToCachedImage(cacheExpiration,400,400), loading);
		}

		#endregion
	}
}