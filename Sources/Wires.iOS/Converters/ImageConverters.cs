namespace Wires
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Net;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;
	using Foundation;
	using UIKit;

	public static partial class PlatformConverters
	{
		public const string LocalImageCacheFolder = "./.cache-img";

		private static string CreateHash(string input)
		{
			using (var alg = SHA256.Create())
			{
				byte[] data = alg.ComputeHash(Encoding.UTF8.GetBytes(input));
				var builder = new StringBuilder();
				for (int i = 0; i < data.Length; i++)
				{
					builder.Append(data[i].ToString("x2"));
				}
				return builder.ToString();
			}
		}

		public static IConverter<string, UIImage> StringToImage { get; private set; } = new RelayConverter<string, UIImage>((value) =>
		{
			if (string.IsNullOrEmpty(value))
				return null;

			NSError err;
			using (var data = NSData.FromFile(value, NSDataReadingOptions.Mapped, out err))
			{
				return UIImage.LoadFromData(data);
			}
		});

		/// <summary>
		/// Async converter that retrieves an image from an url, stores it in local storage with an expiration date.
		/// </summary>
		/// <returns>The string to cached image.</returns>
		/// <param name="expiration">Expiration.</param>
		public static IConverter<string, Task<UIImage>> AsyncStringToCachedImage(TimeSpan expiration) => new RelayConverter<string,Task<UIImage>>(async (value) =>
		{
			if (string.IsNullOrEmpty(value))
				return null;
			
			var cachePath = Path.Combine(LocalImageCacheFolder, $"{CreateHash(value)}");

			NSError err;

			// If not cached, or expired, download the file content and save it to local storage
			DateTime lastWrite = DateTime.MinValue;
			if (!File.Exists(cachePath) || (lastWrite = File.GetLastWriteTimeUtc(cachePath)) + expiration < DateTime.UtcNow)
			{
				if (!Directory.Exists(LocalImageCacheFolder))
				{
					Directory.CreateDirectory(LocalImageCacheFolder);
				}

				try
				{
					Debug.WriteLine($"[Cache][Images]({cachePath}) Start downloading from \"{value}\" ...");
					var request = HttpWebRequest.Create(value);
					using (var res = (await request.GetResponseAsync()) as HttpWebResponse)
					{
						if (res.LastModified > lastWrite)
						{
							
							using (var content = res.GetResponseStream())
							{
								using (var filestream = File.OpenWrite(cachePath))
								{

									await content.CopyToAsync(filestream);
									Debug.WriteLine($"[Cache][Images]({cachePath}) Updated cache");
								}
							}
						}
						else Debug.WriteLine($"[Cache][Images]({cachePath}) Not updating cache because last write is more recent that request last modified date ({res.LastModified} > {lastWrite}).");
					}
				}
				catch (Exception ex) 
				{
					if (File.Exists(cachePath))
					{

						Debug.WriteLine($"[Cache][Images]({cachePath}) Download failed, but a cached version exists.");
						using (var data = NSData.FromFile(cachePath, NSDataReadingOptions.Mapped, out err))
							return UIImage.LoadFromData(data);
					}

					throw ex;
				}
			}

			// Reading image from cache
			using (var data = NSData.FromFile(cachePath, NSDataReadingOptions.Mapped, out err))
				return UIImage.LoadFromData(data);
		});
	}
}
