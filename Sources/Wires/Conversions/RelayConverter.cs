namespace Wires
{
	using System;

	public class RelayConverter<TSource, TTarget> : IConverter<TSource,TTarget>
	{
		public RelayConverter(Func<TSource, TTarget> sourceToTarget)
		{
			this.sourceToTarget = sourceToTarget;
			this.targetToSource = (value) => { throw new NotSupportedException(); };
		}

		public RelayConverter(Func<TSource,TTarget> sourceToTarget, Func<TTarget, TSource> targetToSource)
		{
			this.sourceToTarget = sourceToTarget;
			this.targetToSource = targetToSource;
		}

		readonly Func<TSource, TTarget> sourceToTarget;

		readonly Func<TTarget, TSource> targetToSource;

		public TTarget Convert(TSource value) => sourceToTarget(value);

		public TSource ConvertBack(TTarget value) => targetToSource(value);

	}
}
