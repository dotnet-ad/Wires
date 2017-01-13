namespace Wires
{
	using System;
	using UIKit;

	public partial class PlatformConverters
	{
		public static IConverter<string, UIImage> StringToImage { get; private set; } = new RelayConverter<string,UIImage>((value) =>
		{
			return new UIImage();
		});
	}
}
