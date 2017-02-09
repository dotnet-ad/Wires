namespace Wires.Sample.ViewModel
{
	using System;
	using Newtonsoft.Json;

	public class Post
	{
		[JsonProperty("id")]
		public string Identifier { get; set; }

		[JsonProperty("author")]
		public string Author { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("created")]
		public long Timestamp { get; set; }

		[JsonIgnore]
		public DateTime Datetime => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(this.Timestamp).ToLocalTime();

		[JsonProperty("thumbnail")]
		public string Thumbnail { get; set; }
	}
}
