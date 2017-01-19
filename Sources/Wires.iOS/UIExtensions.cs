namespace Wires
{
	using System;

	public static partial class UIExtensions
	{
		static UIExtensions()
		{
			//Native casts
			Converters.Register(new RelayConverter<int,nint>(x => x, x => (int)x));
			Converters.Register(new RelayConverter<uint, nuint>(x => x, x => (uint)x));
			Converters.Register(new RelayConverter<float, nfloat>(x => x, x => (float)x));
			Converters.Register(new RelayConverter<double, nfloat>(x => (nfloat)x, x => x));
			Converters.Register(new RelayConverter<int, nfloat>(x => x, x => (int)x));

			// Registering all converters
			Converters.Register(PlatformConverters.IntToColor);
			Converters.Register(PlatformConverters.StringToColor);
			Converters.Register(PlatformConverters.StringToImage);
			Converters.Register(PlatformConverters.DateTimeToNSDate);
		}
	}
}
