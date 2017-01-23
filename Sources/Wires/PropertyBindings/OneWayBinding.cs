namespace Wires
{
	using System;

	/// <summary>
	/// A binding that 
	/// </summary>
	public class OneWayBinding<TSource,TTarget,TSourceProperty, TTargetProperty,TSourceChangedEventArgs> : Binding<TSource, TTarget, TSourceProperty, TTargetProperty> 
		where TSourceChangedEventArgs : EventArgs
		where TSource : class
		where TTarget : class
	{
		protected OneWayBinding(bool initialUpdate, TSource source, Func<TSource, TSourceProperty> sourceGetter, Action<TSource, TSourceProperty> sourceSetter, string sourceUpdateEvent, TTarget target,Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter, Func<TSourceChangedEventArgs, bool> sourceEventFilter = null) : base(source, sourceGetter, sourceSetter, target, targetGetter, targetSetter, converter)
		{
			this.sourceEventFilter = sourceEventFilter;
			this.sourceEvent = source.AddWeakHandler<TSourceChangedEventArgs>(sourceUpdateEvent, this.OnSourceChanged);

			if(initialUpdate)
				this.UpdateTarget(); // Affect initial source value to target on binding
		}

		public OneWayBinding(TSource source, Func<TSource, TSourceProperty> sourceGetter, Action<TSource, TSourceProperty> sourceSetter, string sourceUpdateEvent, TTarget target, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter, Func<TSourceChangedEventArgs, bool> sourceEventFilter = null) : this(true, source, sourceGetter, sourceSetter, sourceUpdateEvent, target, targetGetter, targetSetter, converter, sourceEventFilter)
		{
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
			base.Dispose();
		}

	}
}
