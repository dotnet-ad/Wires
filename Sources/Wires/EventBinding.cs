namespace Wires
{
	using System;

	public abstract class EventBinding<TSource, TTarget, TSourceChangedEventArgs> : Binding<TSource, TTarget>
		where TSourceChangedEventArgs : EventArgs
		where TSource : class
		where TTarget : class
	{
		public EventBinding(TSource source, TTarget target, string sourceUpdateEvent, Func<TSourceChangedEventArgs, bool> sourceEventFilter = null) : base(source, target)
		{
			this.sourceEventFilter = sourceEventFilter ?? ((a) => true);
			this.sourceEvent = source.AddWeakHandler<TSourceChangedEventArgs>(sourceUpdateEvent, this.OnSourceChanged);
		}

		readonly Func<TSourceChangedEventArgs, bool> sourceEventFilter;

		readonly WeakEventHandler<TSourceChangedEventArgs> sourceEvent;

		public void OnSourceChanged(object sender, TSourceChangedEventArgs args)
		{
			if (this.sourceEventFilter(args))
				this.OnEvent();
		}

		protected abstract void OnEvent();

		public override void Dispose()
		{
			this.sourceEvent.Unsubscribe();
			base.Dispose();
		}
	}

	public class RelayEventBinding<TSource, TTarget, TSourceChangedEventArgs> : EventBinding<TSource, TTarget, TSourceChangedEventArgs>
		where TSourceChangedEventArgs : EventArgs
		where TSource : class
		where TTarget : class
	{
		public RelayEventBinding(TSource source, TTarget target, string sourceUpdateEvent, Action<TSource,TTarget> onEvent, Func<TSourceChangedEventArgs, bool> sourceEventFilter = null) : base(source, target, sourceUpdateEvent, sourceEventFilter)
		{
			this.onEvent = onEvent;
		}

		readonly Action<TSource, TTarget> onEvent;

		protected override void OnEvent() => onEvent(this.Source, this.Target);
	}
}
