namespace Wires
{
	using System;
	using Transmute;

	/// <summary>
	/// A binding that 
	/// </summary>
	public class OneWayBinding<TSource,TTarget,TSourceProperty, TTargetProperty,TSourceChangedEventArgs> : PropertyBinding<TSource, TTarget, TSourceProperty, TTargetProperty> 
		where TSourceChangedEventArgs : EventArgs
		where TSource : class
		where TTarget : class
	{
		public OneWayBinding(TSource source, Func<TSource, TSourceProperty> sourceGetter, Action<TSource, TSourceProperty> sourceSetter, string sourceUpdateEvent, TTarget target,Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter, Func<TSourceChangedEventArgs, bool> sourceEventFilter = null) : base(source, sourceGetter, sourceSetter, target, targetGetter, targetSetter, converter)
		{
			this.sourceEventFilter = sourceEventFilter ?? ((a) => true);
			this.sourceEvent = source.AddWeakHandler<TSourceChangedEventArgs>(sourceUpdateEvent, this.OnSourceChanged);

			this.Update(); // Affect initial source value to target on binding
		}

		readonly Func<TSourceChangedEventArgs, bool> sourceEventFilter;

		readonly WeakEventHandler<TSourceChangedEventArgs> sourceEvent;

		public void OnSourceChanged(object sender, TSourceChangedEventArgs args)
		{
			if (this.sourceEventFilter(args))
				this.Update();
		}

		public override void Dispose()
		{
			this.sourceEvent.Unsubscribe();
			base.Dispose();
		}

	}
}
