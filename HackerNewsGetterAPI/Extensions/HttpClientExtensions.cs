
namespace HackerNewsGetterAPI.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClientConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddHttpClient("HackerNews", client =>
            {
                client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
            });

            return services;
        }
    }
}
