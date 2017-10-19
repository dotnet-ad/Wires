namespace Wires
{
	public class Index
	{
		public Index(int item) : this(1, item) {}

		public Index(int section, int item)
		{
			this.Section = section;
			this.Item = item;
		}

		public int Section { get; private set; }

		public int Item { get; private set; }
	}
}
