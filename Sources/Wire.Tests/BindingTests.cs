#pragma warning disable CS0219

using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;

namespace Wire.Tests
{
	[TestFixture()]
	public class BindingTests
	{
		#region Stubs

		public class Stub<TValue>
		{
			public TValue Value { get; set; }
		}

		public class Observable<TValue> : INotifyPropertyChanged
		{
			private TValue value;

			public event PropertyChangedEventHandler PropertyChanged;

			public TValue Value 
			{ 
				get { return value;}
				set 
				{
					if (!EqualityComparer<TValue>.Default.Equals(value,this.value))
					{
						this.value = value;
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
					}
				}
			}

			public bool HasSubscriptions => PropertyChanged != null;
		}

		#endregion

		[TestFixtureSetUp]
		public void Setup() => Bindings.Reset();

		[Test()]
		public void OneWayBinding_UpdateSource_ValueChanged()
		{
			var source = new Observable<int>();
			var target = new Observable<int>();

			var binding = source.BindOneWay(nameof(source.Value), target, nameof(source.Value), IdentityConverter<int>.Default);

			source.Value = 5;

			Assert.AreEqual(source.Value, target.Value);
		}

		[Test()]
		public void OneWayBinding_WithInitialValue_ValueChanged()
		{
			var source = new Observable<int>();
			var target = new Observable<int>();

			source.Value = 5;

			var binding = source.BindOneWay(nameof(source.Value), target, nameof(source.Value), IdentityConverter<int>.Default);

			Assert.AreEqual(source.Value, target.Value);
		}

		[Test()]
		public void TwoWayBinding_UpdateSource_ValueChanged()
		{
			var source = new Observable<int>();
			var target = new Observable<int>();

			var binding = source.BindTwoWay(nameof(source.Value), target, nameof(source.Value), IdentityConverter<int>.Default);

			source.Value = 5;
			Assert.AreEqual(source.Value, target.Value);

			target.Value = 8;
			Assert.AreEqual(source.Value, target.Value);
		}

		[Test()]
		public void TwoWayBinding_ReferencesDestroyed_BindingNotAlive()
		{
			IBinding binding = null;

			WeakHelpers.ExecuteAndCollect(() =>
			{
				var source = new Observable<int>();
				var target = new Observable<int>();

				binding = source.BindTwoWay(nameof(source.Value), target, nameof(source.Value), IdentityConverter<int>.Default);
			});

			Assert.IsFalse(binding.IsAlive);
		}

		[Test()]
		public void TwoWayBinding_Unsubscribed_BindingNotAlive()
		{
			IBinding binding = null;

			var source = new Observable<int>();
			var target = new Observable<int>();
			binding = source.BindTwoWay(nameof(source.Value), target, nameof(source.Value), IdentityConverter<int>.Default);
		
			Assert.IsTrue(binding.IsAlive);

			binding.Dispose();

			Assert.IsFalse(binding.IsAlive);
		}
	}
}
