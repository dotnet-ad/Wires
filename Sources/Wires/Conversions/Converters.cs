namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class Converters
	{
		static Converters()
		{
			var floatToDouble = new RelayConverter<float, double>(v => v, v => (float)v);

			Register(floatToDouble);
			Register(floatToDouble.Inverse());
		}

		public static List<object> converters = new List<object>();

		public static void Register<TSource, TTarget>(IConverter<TSource, TTarget> converter)
		{
			converters.Add(converter);
		}

		public static IConverter<TSource,TTarget> Default<TSource, TTarget>()
		{
			if (typeof(TSource) == typeof(TTarget))
				return (IConverter<TSource, TTarget>) Identity<TSource>();

			var converter = converters.OfType<IConverter<TSource, TTarget>>().FirstOrDefault();

			if (converter != null)
				return converter;

			if (typeof(TTarget) == typeof(string))
				return new RelayConverter<TSource, TTarget>(v => (TTarget)((object)v?.ToString()));

			throw new ArgumentException($"No default converter has been registered for types <{typeof(TSource)},{typeof(TTarget)}>.");
		}

		public static IConverter<TSource, TTarget> Chain<TSource, TIntermediate, TTarget>(this IConverter<TSource, TIntermediate> target, IConverter<TIntermediate, TTarget> chained)
		{
			return new ChainConverter<TSource, TIntermediate, TTarget>(target, chained);
		}

		public static IConverter<TTarget, TSource> Inverse<TSource, TTarget>(this IConverter<TSource, TTarget> target)
		{
			return new RelayConverter<TTarget, TSource>(v => target.ConvertBack(v), v => target.Convert(v));
		}

		public static IConverter<T, T> Identity<T>() => new RelayConverter<T, T>(x => x, x => x);

		public static IConverter<bool, bool> Invert => new RelayConverter<bool,bool>(x => !x, x => !x);
	}
}
