namespace BestStoriesDetails_Santander_WebAPI.Services
{
    public interface IBestStoriesService
    {
        Task<IResult> GetBestStoriesDetails(int n);
    }
}
