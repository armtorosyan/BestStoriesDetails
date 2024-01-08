using BestStoriesDetails_Santander_WebAPI.Models;

namespace BestStoriesDetails_Santander_WebAPI.Infrastructure
{
    public interface IHackerNewsClient
    {
        Task<IEnumerable<int>> GetBestStoryIds();
        Task<StoryDetails> GetStoryDetail(int id);
    }
}