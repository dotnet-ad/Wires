#pragma warning disable CS0219

using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel;

namespace Wires.Tests
{
	[TestFixture()]
	public class BindableCollectionSourceTests
	{
		#region Stubs

		public class ItemViewModel
		{

		}

		public class ViewModel : INotifyPropertyChanged
		{
			private IEnumerable<ItemViewModel> items;

			public event PropertyChangedEventHandler PropertyChanged;

			public IEnumerable<ItemViewModel> Items
			{
				get { return this.items; }
				set
				{
					if (value != this.items)
					{
						this.items = value;
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Items)));
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
			
		}
	}
}
