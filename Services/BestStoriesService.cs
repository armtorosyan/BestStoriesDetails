using BestStoriesDetails_Santander_WebAPI.Infrastructure;
using BestStoriesDetails_Santander_WebAPI.Models;
using System.Collections.Concurrent;

namespace BestStoriesDetails_Santander_WebAPI.Services
{
    public class BestStoriesService : IBestStoriesService
    {
        public readonly IHackerNewsClient _client;

        public BestStoriesService(IHackerNewsClient client)
        {
            _client = client;
        }

        public async Task<IResult> GetBestStoriesDetails(int n)
        {
            var allStoryIds = await _client.GetBestStoryIds();

            var storyIds = allStoryIds.Take(n);
            
            var storiesDetails = new ConcurrentDictionary<int, StoryDetails>();

            await Parallel.ForEachAsync(storyIds, async (id, token) =>
            {
                var storyDetail = await _client.GetStoryDetail(id);
                storiesDetails.TryAdd(id, storyDetail);
            });            

            if (storiesDetails.Keys.Count() > 0)
            {
                return Results.Ok<IEnumerable<StoryDetails>>(storiesDetails.Values.OrderByDescending(s => s.Score));
            }
            else
            {
                return Results.BadRequest();
            }
        }
    }
}
