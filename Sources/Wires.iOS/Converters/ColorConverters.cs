namespace Wires
{
	using System;
	using UIKit;

	public static partial class PlatformConverters
	{
		public static IConverter<int, UIColor> IntToColor { get; private set; } = new RelayConverter<int, UIColor>((value) =>
		 {
			return UIColor.Black;
		 },(value) =>
		 {
			 return 0x000000;
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
