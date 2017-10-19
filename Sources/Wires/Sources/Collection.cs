namespace Wires
{
	using System.Collections.Generic;
	using System.Linq;

	public class Collection<TKey,TItem> : List<Collection<TKey, TItem>.Section>
	{
		public class Section : List<TItem>
		{
			public TKey Key { get; set; }

			public Section(TKey key,IEnumerable<TItem> items) : base(items)
			{
				this.Key = key;
			}

			public Section(TKey key) : base()
			{
				this.Key = key;
			}
		}

		public Collection()
		{
		}

		public void AddSection(TKey key, IEnumerable<TItem> items)
		{
			this.Add(new Section(key, items));
		}

		public void RemoveSection(TKey key)
		{
			var existing = this.Where(s => EqualityComparer<TKey>.Default.Equals(s.Key, key));
			foreach (var item in existing)
			{
				this.Remove(item);
			}
		}
	}
}
