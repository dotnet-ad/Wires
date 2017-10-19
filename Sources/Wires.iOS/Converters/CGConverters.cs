namespace Wires
{
	using System;
	using CoreGraphics;
	using Foundation;

	public static partial class PlatformConverters
	{
		public static IConverter<float[], CGRect> FloatArrayToCGRect { get; private set; } = new RelayConverter<float[], CGRect>((value) => new CGRect(value[0], value[1], value[2], value[3]), (value) => new float[] { (float)value.X, (float)value.Y, (float)value.Width, (float)value.Height, } );
	}
}
