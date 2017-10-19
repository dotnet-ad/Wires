namespace Wire.Tests
{
	using NUnit.Framework;
	using System;
	using System.Collections.Generic;
	using System.Linq;

	[TestFixture()]
	public class WeakEventHandlerTests
	{
		#region Stubs

		public class Publisher
		{
			public event EventHandler Event;

			public void RaiseEvent()
			{
				this.Event?.Invoke(this, new EventArgs());
			}

			public bool HasSubscriptions => Event != null;
		}

		public class Subscriber : Publisher
		{
			public int RaisedEvents { get; private set; }

			public void OnEventRaised(object sender, EventArgs args)
			{
				RaisedEvents++;
			}
		}

		#endregion

		[Test()]
		public void AddWeakHandler_WhenPublisherRaises_SubscriberReceives()
		{
			var publisher = new Publisher();
			var subscriber = new Subscriber();

			publisher.AddWeakHandler<EventArgs>(nameof(publisher.Event), subscriber.OnEventRaised);

			int expected = 5;
			for (int i = 0; i < expected; i++)
			{
				publisher.RaiseEvent();
			}

			Assert.AreEqual(expected, subscriber.RaisedEvents);
		}

		[Test()]
		public void AddWeakHandler_WhenSubscriberDestroyed_Unsubscription()
		{

			var publisher = new Publisher();

			WeakHelpers.ExecuteAndCollect(() =>
			{
				var subscriber = new Subscriber();
				publisher.AddWeakHandler<EventArgs>(nameof(publisher.Event), subscriber.OnEventRaised);
			});

			publisher.RaiseEvent();

			Assert.IsFalse(publisher.HasSubscriptions);
		}

		[Test()]
		public void AddWeakHandler_WhenCrossSubscriptions_NotAlives()
		{
			var handlers = new List<WeakEventHandler<EventArgs>>();

			WeakHelpers.ExecuteAndCollect(() =>
			{
				var pubsub1 = new Subscriber();
				var pubsub2 = new Subscriber();
				handlers.Add(pubsub2.AddWeakHandler<EventArgs>(nameof(Publisher.Event), pubsub1.OnEventRaised));
				handlers.Add(pubsub1.AddWeakHandler<EventArgs>(nameof(Publisher.Event), pubsub2.OnEventRaised));
			});

			Assert.IsTrue(handlers.Any((a) => !a.IsAlive));
		}
	}
}
