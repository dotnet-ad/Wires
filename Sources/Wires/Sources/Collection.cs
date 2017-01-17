namespace Wires
{
	using System.Collections.Generic;

	public class Collection<TKey,TItem> : List<Collection<TKey, TItem>.Section>
	{
		public class Section : List<TItem>
		{
			public TKey Key { get; set; }
		}

		public Collection()
		{
		}
	}
}
