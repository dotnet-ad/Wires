using System;
using System.Linq.Expressions;
using UIKit;

namespace Wires
{
	public static partial class UIExtensions
	{
		#region Selected property

		public static Binder<TSource, UISegmentedControl> Selected<TSource, TPropertyType>(this Binder<TSource, UISegmentedControl> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, int> converter = null)
			where TSource : class
		{
			converter = converter ?? Converters.Default<TPropertyType, int>();
			var finalConverter = converter.Chain(Converters.Default<int, nint>());
			return binder.Property(property, b => b.SelectedSegment, finalConverter);
		}

		#endregion

		#region Titles property

		public static Binder<TSource, UISegmentedControl> Titles<TSource, TPropertyType>(this Binder<TSource, UISegmentedControl> binder, Expression<Func<TSource, TPropertyType>> property, IConverter<TPropertyType, string[]> converter = null)
			where TSource : class
		{
			return binder.Property(property, b => 
			{
				var list = new string[b.NumberOfSegments];
				for (int i = 0; i < b.NumberOfSegments; i++)
				{
					list[i] = b.TitleAt(i);
				}

				return list;
			}, (b, v) => 
			{
				if (v == null)
				{
					b.RemoveAllSegments();
				}
				else if (b.NumberOfSegments != v.Length)
				{
					b.RemoveAllSegments();
					for (int i = 0; i < v.Length; i++)
					{
						b.InsertSegment(v[i], i, false);
					}
				}
				else
				{
					for (int i = 0; i < v.Length; i++)
					{
						b.SetTitle(v[i], i);
					}
				}

			}, converter);
		}

		#endregion
	}
}
