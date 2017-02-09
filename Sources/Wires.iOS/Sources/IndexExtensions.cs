namespace Wires
{
	using Foundation;

	public static class IndexExtensions
	{
		public static NSIndexPath ToPath(this Index index) => NSIndexPath.FromRowSection(index.Item,index.Section);

		public static Index ToIndex(this NSIndexPath path) => new Index(path.Section, path.Row);
	}
}
