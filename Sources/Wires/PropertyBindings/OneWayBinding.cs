namespace Wires
{
	using System;

	/// <summary>
	/// A binding that 
	/// </summary>
	public class OneWayBinding<TSource,TTarget,TSourceProperty, TTargetProperty,TSourceChangedEventArgs> : Binding<TSource, TTarget, TSourceProperty, TTargetProperty> where TSourceChangedEventArgs : EventArgs
	{
		public OneWayBinding(TSource source, string sourceProperty, string sourceUpdateEvent, TTarget target, string targetProperty, IConverter<TSourceProperty, TTargetProperty> converter, Func<TSourceChangedEventArgs, bool> sourceEventFilter = null) : base(source, sourceProperty, target, targetProperty, converter)
		{
			this.sourceEventFilter = sourceEventFilter;
			this.sourceEvent = source.AddWeakHandler<TSourceChangedEventArgs>(sourceUpdateEvent, this.OnSourceChanged);

			this.UpdateTarget(); // Affect initial source value to target on binding
		}

		readonly Func<TSourceChangedEventArgs, bool> sourceEventFilter;

		readonly WeakEventHandler<TSourceChangedEventArgs> sourceEvent;

		public void OnSourceChanged(object sender, TSourceChangedEventArgs args)
		{
			if (this.sourceEventFilter(args))
				this.UpdateTarget();
		}

		public override void Dispose()
		{
			this.sourceEvent.Unsubscribe();
		}

	}
}
