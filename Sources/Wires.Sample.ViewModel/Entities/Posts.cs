namespace Wires.Sample.ViewModel
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class Posts
	{
		[JsonProperty("data")]
		public PostsData Data { get; set; }

	}
}
