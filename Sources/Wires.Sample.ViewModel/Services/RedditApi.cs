namespace Wires.Sample.ViewModel
{
	using System.Collections.Generic;
	using System.Net.Http;
	using System.Threading.Tasks;
	using Newtonsoft.Json;
	using System.Linq;

	public class RedditApi
	{
		private HttpClient Client = new HttpClient();

		public async Task<IEnumerable<Post>> GetTopicAsync(string topic)
		{
			var url = $"https://www.reddit.com/r/{topic}.json";

			var json = await this.Client.GetStringAsync(url);
			var response = JsonConvert.DeserializeObject<Posts>(json);
			return response?.Data?.Children?.Select(c => c.Data) ?? new Post[0];

		}
	}
}
