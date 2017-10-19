namespace Wires
{
	using Foundation;
	using UIKit;

	public static partial class PlatformConverters
	{
		public static IConverter<string, UIImage> StringToImage { get; private set; } = new RelayConverter<string,UIImage>((value) =>
		{
			if (string.IsNullOrEmpty(value))
				return null;
			
			NSError err;
			using (var url = new NSUrl(value))
			using (var data = NSData.FromUrl(url, NSDataReadingOptions.Mapped, out err))
			{
				if (data == null)
					return null;
				
				return UIImage.LoadFromData(data);
			}
		});
	}
}
