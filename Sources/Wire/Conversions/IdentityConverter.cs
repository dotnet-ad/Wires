using System;

namespace Wire
{
	public class IdentityConverter<TSource> : IConverter<TSource,TSource>
	{
		public static Lazy<IdentityConverter<TSource>> def = new Lazy<IdentityConverter<TSource>>();

		public static IdentityConverter<TSource> Default => def.Value;

		public TSource Convert(TSource value) => value;

		public TSource ConvertBack(TSource value) => value;
	}
}
