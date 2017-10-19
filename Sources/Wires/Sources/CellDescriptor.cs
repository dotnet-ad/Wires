using System;

namespace Wires
{
	public class CellDescriptor
	{
		public CellDescriptor(string id, Type viewType, float width, float height) : this(id,viewType, (arg) => new Tuple<float, float>(width, height))
		{

		}

		public CellDescriptor(string id, Type viewType, Func<object, Tuple<float, float>> getSize)
		{
			this.Identifier = id;
			this.ViewType = viewType;
			this.getSize = getSize;
		}

		public string Identifier { get; }

		public Type ViewType { get; }

		private Func<object,Tuple<float, float>> getSize;

		public Tuple<float, float> GetSize(object item) => getSize(item);
	}
}
