namespace Wires
{
	using System;
	using Foundation;

	public static partial class PlatformConverters
	{
		private static DateTime reference = new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		public static IConverter<DateTime, NSDate> DateTimeToNSDate { get; private set; } = new RelayConverter<DateTime, NSDate>((value) =>
		 {
			 var utcDateTime = value.ToUniversalTime();
			 var date = NSDate.FromTimeIntervalSinceReferenceDate((utcDateTime - reference).TotalSeconds);
			 return date;
		 }, (value) =>
		  {
			  var utcDateTime = reference.AddSeconds(value.SecondsSinceReferenceDate);
			  var dateTime = utcDateTime.ToLocalTime();
			  return dateTime;
		  });
	}
}
