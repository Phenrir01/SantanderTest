using HackerNewsGetterAPI.Abstractions.Errors;
namespace HackerNewsGetterAPI.Features.BestStories;

public static class BestStoryErrors
{
    public static Error NotFound(Guid id) => Error.NotFound("BestStoriess.NotFound", $"The Story with Id '{id}' was not found.");
    public static Error NotFoundAll() => Error.NotFound("BestStoriess.NotFoundAll", $"No stories were found.");

}