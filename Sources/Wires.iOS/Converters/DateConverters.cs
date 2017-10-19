using System;
using Foundation;

namespace Wires
{
	public static partial class PlatformConverters
	{
        #region Timestamps

        /// <summary>
        /// Converts a timestamp to a DateTime
        /// </summary>
        /// <param name="timestamp">The timestamp (milliseconds unix epoch)</param>
        /// <returns>The date time</returns>
        private static DateTime ToDateTime(this long timestamp)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp).ToLocalTime();
		}

		/// <summary>
		/// Converts a DateTime to a timestamp.
		/// </summary>
		/// <param name="datetime">The original date time</param>
		/// <returns>The timestamp (milliseconds unix epoch)</returns>
		private static long ToTimestamp(this DateTime datetime)
		{
			TimeSpan ts = (datetime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
			return (long)ts.TotalSeconds;
		}

		#endregion

		public static IConverter<DateTime, NSDate> DateTimeToNSDate { get; private set; } = new RelayConverter<DateTime, NSDate>((value) =>
		 {
			 return NSDate.FromTimeIntervalSince1970(value.ToTimestamp());
		 }, (value) =>
		  {
			return ToDateTime((long)value.SecondsSinceReferenceDate);
		  });
	}
}
