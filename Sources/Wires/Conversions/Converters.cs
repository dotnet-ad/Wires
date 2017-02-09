namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public static class Converters
	{
		static Converters()
		{
			var intToFloat = new RelayConverter<int, float>(v => v, v => (int)v);
			var floatToDouble = new RelayConverter<float, double>(v => v, v => (float)v);

			Register(intToFloat);
			Register(intToFloat.Inverse());
			Register(floatToDouble);
			Register(floatToDouble.Inverse());
			Register(TimestampToDatetime);
		}

		private static List<object> converters = new List<object>();

		/// <summary>
		/// Registers a default converter (and replace the already registered converter if so).
		/// </summary>
		/// <param name="converter">Converter.</param>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		/// <typeparam name="TTarget">The 2nd type parameter.</typeparam>
		public static void Register<TSource, TTarget>(IConverter<TSource, TTarget> converter)
		{
			var existing = converters.OfType<IConverter<TSource, TTarget>>().ToArray();

			foreach (var item in existing)
			{
				converters.Remove(item);	
			}

			converters.Add(converter);
		}

		/// <summary>
		/// Gets the default registered converter fo the two given types.
		/// </summary>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		/// <typeparam name="TTarget">The 2nd type parameter.</typeparam>
		public static IConverter<TSource,TTarget> Default<TSource, TTarget>()
		{
			var tSource = typeof(TSource);
			var tTarget = typeof(TTarget);

			if (tSource == tTarget)
				return (IConverter<TSource, TTarget>) Identity<TSource>();

			var converter = converters.OfType<IConverter<TSource, TTarget>>().FirstOrDefault();

			if (converter != null)
				return converter;

			if (tTarget == typeof(string))
				return new RelayConverter<TSource, TTarget>(v => (TTarget)((object)v?.ToString()));

			if (tSource.IsArray && tTarget.IsArray)
			{

			}

			throw new ArgumentException($"No default converter has been registered for types <{tSource},{tTarget}>.");
		}

		/// <summary>
		/// Chains two converters.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="chained">Chained.</param>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		/// <typeparam name="TIntermediate">The 2nd type parameter.</typeparam>
		/// <typeparam name="TTarget">The 3rd type parameter.</typeparam>
		public static IConverter<TSource, TTarget> Chain<TSource, TIntermediate, TTarget>(this IConverter<TSource, TIntermediate> target, IConverter<TIntermediate, TTarget> chained)
		{
			return new ChainConverter<TSource, TIntermediate, TTarget>(target, chained);
		}

		/// <summary>
		/// Invert the given converter by interchanging convert and convert back.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		/// <typeparam name="TTarget">The 2nd type parameter.</typeparam>
		public static IConverter<TTarget, TSource> Inverse<TSource, TTarget>(this IConverter<TSource, TTarget> target)
		{
			return new RelayConverter<TTarget, TSource>(v => target.ConvertBack(v), v => target.Convert(v));
		}

		/// <summary>
		/// Returns the given value.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static IConverter<T, T> Identity<T>() => new RelayConverter<T, T>(x => x, x => x);


		public static IConverter<TSource[], TTarget[]> ArrayToArray<TSource, TTarget>(IConverter<TSource, TTarget> elementConverter = null)
		{
			elementConverter = elementConverter ?? Default<TSource, TTarget>();
			return new RelayConverter<TSource[], TTarget[]>((arg) => arg.Select(a => elementConverter.Convert(a)).ToArray(), (arg) => arg.Select(a => elementConverter.ConvertBack(a)).ToArray());
		}

		/// <summary>
		/// Inverts the boolean value.
		/// </summary>
		/// <value>The invert.</value>
		public static IConverter<bool, bool> Invert => new RelayConverter<bool,bool>(x => !x, x => !x);

		/// <summary>
		/// Converts the value by casting the value to the target type.
		/// </summary>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		/// <typeparam name="TTarget">The 2nd type parameter.</typeparam>
		public static IConverter<TSource, TTarget> Cast<TSource, TTarget>() => new RelayConverter<TSource, TTarget>(x => (TTarget)Convert.ChangeType(x, typeof(TTarget)), x => (TSource)Convert.ChangeType(x, typeof(TSource)));

		/// <summary>
		/// Converts the timestamp value (total milliseconds from the 1st January of 1970) to datetime.
		/// </summary>
		/// <value>The timestamp to datetime.</value>
		public static IConverter<long, DateTime> TimestampToDatetime { get; private set; } = new RelayConverter<long, DateTime>((value) =>
		 {
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(value).ToLocalTime();
		 }, (value) =>
		  {
			  var ts = (value.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
			  return (long)ts.TotalMilliseconds;
		  });

		#region Cases

		/// <summary>
		/// Converts a string to uppercase.
		/// </summary>
		/// <value>The invert.</value>
		public static IConverter<string, string> Uppercase => new RelayConverter<string, string>(x => x.ToUpperInvariant());

		/// <summary>
		/// Converts a string to lowercase.
		/// </summary>
		/// <value>The invert.</value>
		public static IConverter<string, string> Lowercase => new RelayConverter<string, string>(x => x.ToLowerInvariant());

		#endregion
	}
}
