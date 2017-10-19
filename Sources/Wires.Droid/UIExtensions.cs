namespace Wires
{
	public static partial class UIExtensions
	{
		static UIExtensions()
		{
			// Registering all converters
			Converters.Register(PlatformConverters.BoolToViewState);
			Converters.Register(PlatformConverters.StringToImage);
		}
	}
}
