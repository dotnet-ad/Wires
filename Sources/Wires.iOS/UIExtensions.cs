namespace Wires
{
	public static partial class UIExtensions
	{
		static UIExtensions()
		{
			// Registering all converters
			Converters.Register(PlatformConverters.IntToColor);
			Converters.Register(PlatformConverters.StringToColor);
			Converters.Register(PlatformConverters.StringToImage);
			Converters.Register(PlatformConverters.DateTimeToNSDate);
		}
	}
}
