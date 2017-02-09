namespace Wires.Sample.ViewModel
{
	using Newtonsoft.Json;

	public class PostData
	{
		[JsonProperty("data")]
		public Post Data { get; set; }
	}
}
