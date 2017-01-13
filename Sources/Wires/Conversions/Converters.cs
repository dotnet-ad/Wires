namespace Wires
{
	public partial class Converters
	{
		public static IConverter<T, T> Identity<T>() => new RelayConverter<T, T>(x => x, x => x);

		public static IConverter<bool, bool> Invert => new RelayConverter<bool,bool>(x => !x, x => !x);
	}
}
