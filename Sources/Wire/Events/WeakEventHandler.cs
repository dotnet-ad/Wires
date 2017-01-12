namespace Wire
{
	using System;
	using System.Reflection;

	/// <summary>
	/// A lightweight proxy instance that will subscribe to a given event with a weak reference to the subscribed target.
	/// If the subscriber is garbage collected, then only this WeakEventHandler will remain subscribed and keeped 
	/// in memory instead of the actual subscriber.
	/// This could be considered as an acceptable solution in most cases.
	/// </summary>
	public class WeakEventHandler<TEventArgs> where TEventArgs : EventArgs
	{
		public WeakEventHandler(object source, string eventName, EventHandler<TEventArgs> target)
		{
			this.eventInfo = source.GetType().GetRuntimeEvent(eventName);

			if (this.eventInfo == null)
				throw new ArgumentException($"Source of type {source.GetType()} does not contains an event with {eventName} name.", nameof(eventName));

			this.targetReference = new WeakReference(target.Target);
			this.sourceReference = new WeakReference(source);
			var methodInfo = this.GetType().FindMethod(nameof(OnEvent), typeof(object), typeof(TEventArgs));
			eventHandler = methodInfo.CreateDelegate(this.eventInfo.EventHandlerType, this);
			this.eventInfo.AddEventHandler(source, eventHandler);
			this.targetMethod = target.GetMethodInfo().BuildHandlerExpression<TEventArgs>();
		}

		#region Fields

		private Delegate eventHandler;

		private readonly EventInfo eventInfo;

		private readonly WeakReference sourceReference;

		private readonly WeakReference targetReference;

		private readonly Action<object,object,TEventArgs> targetMethod;

		#endregion

		public bool IsAlive => sourceReference.IsAlive && targetReference.IsAlive;

		private void OnEvent(object sender, TEventArgs args)
		{
			if (this.targetReference.IsAlive)
			{
				this.targetMethod(this.targetReference.Target, sender, args);
			}
			else
			{
				this.Unsubscribe();
			}
		}

		private bool isUnsubscribed;

		public void Unsubscribe()
		{
			if (!isUnsubscribed)
			{
				if (this.sourceReference.IsAlive)
				{
					this.eventInfo.RemoveEventHandler(this.sourceReference.Target, eventHandler);
				}

				isUnsubscribed = true;
			}
		}
	}

	public static class WeakEventHandlerExtensions
	{
		public static WeakEventHandler<TEventArgs> AddWeakHandler<TEventArgs>(this object source, string eventName, EventHandler<TEventArgs> handler) where TEventArgs : EventArgs
		{
			return new WeakEventHandler<TEventArgs>(source, eventName, handler);
		}
	}
}