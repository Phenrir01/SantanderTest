namespace HackerNewsGetterAPI.Features.BestStories.GetAllBestStories;

public sealed record BestStoryDto(string title, string uri, string postedBy, DateTime time, int score, int commentCount);
