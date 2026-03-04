namespace HackerNewsGetterAPI.Features.BestStories.GetAllBestStories;

public sealed record GetAllBestStoryResponse(IEnumerable<BestStoryDto> BestStories);
