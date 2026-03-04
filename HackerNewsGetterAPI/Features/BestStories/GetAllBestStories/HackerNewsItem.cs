namespace HackerNewsGetterAPI.Features.BestStories.GetAllBestStories
{
    public record HackerNewsItem(string Title, string Url, string By, long Time, int Score, int Descendants);
}
