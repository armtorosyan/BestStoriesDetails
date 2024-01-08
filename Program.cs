using BestStoriesDetails_Santander_WebAPI.Infrastructure;
using BestStoriesDetails_Santander_WebAPI.Services;

namespace BestStoriesDetails_Santander_WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.            
            builder.Services.AddScoped<IBestStoriesService, BestStoriesService>();
            builder.Services.AddScoped<IHackerNewsClient, HackerNewsClient>();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.MapGet("/beststoriesdetails/{count}", async (int count, IBestStoriesService storiesService)
                => await storiesService.GetBestStoriesDetails(count));
            
            app.Run();
        }
    }
}
