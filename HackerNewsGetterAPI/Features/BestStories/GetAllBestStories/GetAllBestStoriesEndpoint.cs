
using HackerNewsGetterAPI.Abstractions;
using HackerNewsGetterAPI.Constants;
using HackerNewsGetterAPI.Extensions;

namespace HackerNewsGetterAPI.Features.BestStories.GetAllBestStories;

internal sealed class GetAllBestStoriesEndpoint : IApiEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("BestStories/{n:int}", async (int n, IHandler<GetAllBestStoryRequest, Result<GetAllBestStoryResponse>> handler, CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetAllBestStoryRequest(n), cancellationToken);
            return result.Match(
                onSuccess: () => Results.Ok(result.Value),
                onFailure: error => Results.BadRequest(error));
        })
        .WithTags(ApiTags.HackerNews)
        .Produces<GetAllBestStoryResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
