namespace Wires
{
	using System;

	public class TwoWayBinding<TSource, TTarget, TSourceProperty, TTargetProperty,TSourceChangedEventArgs, TTargetChangedEventArgs> : OneWayBinding<TSource, TTarget, TSourceProperty, TTargetProperty,TSourceChangedEventArgs> 
		where TSourceChangedEventArgs : EventArgs 
		where TTargetChangedEventArgs : EventArgs
	{
		public TwoWayBinding(TSource source, string sourceProperty, string sourceUpdateEvent, TTarget target, string targetProperty, string targetUpdateEvent, IConverter<TSourceProperty, TTargetProperty> converter, Func<TSourceChangedEventArgs, bool> sourceEventFilter = null, Func<TTargetChangedEventArgs, bool> targetEventFilter = null)
			: base(source, sourceProperty, sourceUpdateEvent, target, targetProperty, converter, sourceEventFilter)
		{
			this.targetEventFilter = targetEventFilter ?? ((a) => true);
			this.targetEvent = target.AddWeakHandler<TTargetChangedEventArgs>(targetUpdateEvent, this.OnTargetChanged);
		}

		readonly Func<TTargetChangedEventArgs, bool> targetEventFilter;

		readonly WeakEventHandler<TTargetChangedEventArgs> targetEvent;

		public void OnTargetChanged(object sender, TTargetChangedEventArgs args)
		{
			if(targetEventFilter(args))
				this.UpdateSource();
		}

		public override void Dispose()
		{
			this.targetEvent.Unsubscribe();
			base.Dispose();
		}
	}
}
