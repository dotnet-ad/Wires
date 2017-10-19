using System;
using Transmute;

namespace Wires
{
	public class OneTimeBinding<TSource, TTarget, TSourceProperty, TTargetProperty> : PropertyBinding<TSource, TTarget, TSourceProperty, TTargetProperty>
		where TSource : class
		where TTarget : class
	{
		public OneTimeBinding(TSource source, Func<TSource, TSourceProperty> sourceGetter, TTarget target, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter) : base(source, sourceGetter, null, target, targetGetter, targetSetter, converter)
		{
			this.Update(); // Affect initial source value to target on binding
			this.Dispose(); // Dispose binding because we don't need it anymore since its one time.
		}
	}
}
