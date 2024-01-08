using Newtonsoft.Json;

namespace BestStoriesDetails_Santander_WebAPI.Models
{
    public class StoryDetails
    {
        private object? _time;

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("url")]
        public string? Uri { get; set; }

        [JsonProperty("by")]
        public string? PostedBy { get; set; }

        [JsonProperty("time")]
        public object? Time 
        { 
            get => _time;
            set => _time = DateTimeOffset.FromUnixTimeSeconds((long)value).DateTime;
        }

        [JsonProperty("score")]
        public int? Score { get; set; }

        [JsonProperty("descendants")]
        public int CommentCount {  get; set; }
    }
}
