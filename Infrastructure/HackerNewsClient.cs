using BestStoriesDetails_Santander_WebAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BestStoriesDetails_Santander_WebAPI.Infrastructure
{
    public class HackerNewsClient : IHackerNewsClient
    {
        private readonly IMemoryCache _cache;
        private readonly string _baseUrl;
        private readonly string _bestStoriesCacheKey;
        private readonly string _storyDetailsCacheKey;

        public HackerNewsClient(IMemoryCache cache, IConfiguration configuration)
        {
            _cache = cache;
            _baseUrl = configuration.GetValue<string>("ProjectSettings:ServiceBaseUrl");
            _bestStoriesCacheKey = configuration.GetValue<string>("ProjectSettings:IdsCacheKey");
            _storyDetailsCacheKey = configuration.GetValue<string>("ProjectSettings:DetailsCacheKey");
        }

        public async Task<IEnumerable<int>> GetBestStoryIds()
        {
            if (_cache.TryGetValue(_bestStoriesCacheKey, out IEnumerable<int> storyIds))
            {
                return storyIds;
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("beststories.json");

            if (response.IsSuccessStatusCode)
            {
                storyIds = JsonConvert.DeserializeObject<IEnumerable<int>>(await response.Content.ReadAsStringAsync());

                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(20));

                _cache.Set(_bestStoriesCacheKey, storyIds, cacheOptions);

                return storyIds;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

                return Enumerable.Empty<int>();
            }
        }

        public async Task<StoryDetails> GetStoryDetail(int id)
        {
            if (_cache.TryGetValue($"{_storyDetailsCacheKey}-{id}", out StoryDetails storyDetails))
            {
                return storyDetails;
            }
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"item/{id}.json");

            if (response.IsSuccessStatusCode)
            {
                storyDetails = JsonConvert.DeserializeObject<StoryDetails>(await response.Content.ReadAsStringAsync());

                var cacheOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                _cache.Set($"{_storyDetailsCacheKey}-{id}", storyDetails, cacheOptions);

                return storyDetails;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);

                return null;
            }
        }
    }
}
