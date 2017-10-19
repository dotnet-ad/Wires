namespace Wires
{
	/// <summary>
	/// Represents an item index in a grouped collection.
	/// </summary>
	public class Index
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Wires.Index"/> class.
		/// </summary>
		/// <param name="item">Item.</param>
		public Index(int item) : this(1, item) {}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Wires.Index"/> class.
		/// </summary>
		/// <param name="section">Section index.</param>
		/// <param name="item">Item index into section.</param>
		public Index(int section, int item)
		{
			this.Section = section;
			this.Item = item;
		}

		/// <summary>
		/// Gets the section index.
		/// </summary>
		/// <value>The section.</value>
		public int Section { get; private set; }

		/// <summary>
		/// Gets the item index into its section.
		/// </summary>
		/// <value>The item.</value>
		public int Item { get; private set; }
	}
}
