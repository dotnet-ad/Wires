using System;
namespace Wires
{
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
}
