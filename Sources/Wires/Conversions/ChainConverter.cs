namespace Wires
{
	using System;

	public class ChainConverter<TSource,TIntermediate,TTarget> : IConverter<TSource, TTarget>
	{
		public ChainConverter(IConverter<TSource, TIntermediate> first, IConverter<TIntermediate, TTarget> second)
		{
			this.first = first;
			this.second = second;
		}

		readonly IConverter<TSource, TIntermediate> first;

		readonly IConverter<TIntermediate, TTarget> second;

		public TTarget Convert(TSource value) => second.Convert(first.Convert(value));

		public TSource ConvertBack(TTarget value) => first.ConvertBack(second.ConvertBack(value));
	}
}
