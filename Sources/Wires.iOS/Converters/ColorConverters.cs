namespace Wires
{
	using System;
	using UIKit;

	public static partial class PlatformConverters
	{

		public static UIColor ToUIColor(this int color) { return UIColor.FromRGB((((nfloat)((color & 0xFF0000) >> 16)) / 255.0f), (((nfloat)((color & 0xFF00) >> 8)) / 255.0f), (((nfloat)(color & 0xFF)) / 255.0f)); }

		public static IConverter<int, UIColor> IntToColor { get; private set; } = new RelayConverter<int, UIColor>((value) =>
		 {
			 var a = ((nfloat)((value & 0xFF000000) >> 32)) / 255.0f;
			 var r = ((nfloat)((value & 0x00FF00000) >> 16)) / 255.0f;
			 var g = ((nfloat)((value & 0x0000FF00) >> 8)) / 255.0f;
			 var b = ((nfloat)(value & 0x000000FF)) / 255.0f;
			 return UIColor.FromRGBA(r,g,b,a);
		 },(value) =>
		 {

			 var a = (int)(value.CGColor.Alpha * 255);
			 var r = (int)(value.CGColor.Components[0] * 255);
			 var g = (int)(value.CGColor.Components[1] * 255);
			 var b = (int)(value.CGColor.Components[2] * 255);
			return (a << 32) + (r << 16) + (g << 8) + b;
		 });


		public static IConverter<int, UIColor> StringToColor { get; private set; } = new RelayConverter<int, UIColor>((value) =>
		 {
			 return UIColor.Black;
		 }, (value) =>
		  {
			  return 0x000000;
		  });
	}
}
