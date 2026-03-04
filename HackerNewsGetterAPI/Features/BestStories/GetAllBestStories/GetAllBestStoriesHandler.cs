using HackerNewsGetterAPI.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsGetterAPI.Features.BestStories.GetAllBestStories;

public sealed class GetAllBestStoriesHandler(
    IHttpClientFactory httpClientFactory,
    IMemoryCache cache) : IHandler<GetAllBestStoryRequest, Result<GetAllBestStoryResponse>>
{
    public async Task<Result<GetAllBestStoryResponse>> HandleAsync(GetAllBestStoryRequest command, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("HackerNews");

        string cacheKey = $"bestStories_{command.n}";

        if(cache.TryGetValue(cacheKey, out IEnumerable<BestStoryDto>? cachedStories))
        {
            return Result.Success(new GetAllBestStoryResponse(cachedStories!));
        }

        var storyIds = await client.GetFromJsonAsync<List<int>>("beststories.json");
        if (storyIds == null) return Result.Failure<GetAllBestStoryResponse>(BestStoryErrors.NotFoundAll());

        var targetStories = storyIds.Take(command.n).ToList();

        using var semaphore = new SemaphoreSlim(20);

        var tasks = targetStories.Select(async id =>
        {
            await semaphore.WaitAsync();
            try
            {
                var item = await client.GetFromJsonAsync<HackerNewsItem>($"item/{id}.json");
                return item != null ? new BestStoryDto(
                    item.Title,
                    item.Url,
                    item.By,
                    DateTimeOffset.FromUnixTimeSeconds(item.Time).DateTime,
                    item.Score,
                    item.Descendants
                ) : null;
            }
            finally
            {
                semaphore.Release();
            }
        }).AsParallel();
   
        var results = await Task.WhenAll(tasks);

        var BestStoryDtos = results.Where(s =>s != null).OrderByDescending(s => s!.score);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
            .SetSlidingExpiration(TimeSpan.FromMinutes(2));

        cache.Set(cacheKey, BestStoryDtos, cacheOptions);
        
        return Result.Success(new GetAllBestStoryResponse(BestStoryDtos!));
    }
}