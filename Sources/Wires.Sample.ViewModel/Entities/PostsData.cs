namespace Wires.Sample.ViewModel
{
	using System.Collections.Generic;
	using Newtonsoft.Json;

	public class PostsData
	{
		[JsonProperty("children")]
		public IEnumerable<PostData> Children { get; set; }
	}
}
