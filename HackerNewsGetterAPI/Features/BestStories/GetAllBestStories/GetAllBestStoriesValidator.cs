using FluentValidation;

namespace HackerNewsGetterAPI.Features.BestStories.GetAllBestStories;

public class GetAllBestStoriesValidator : AbstractValidator<GetAllBestStoryRequest>
{
    public GetAllBestStoriesValidator()
    {
        RuleFor(c => c.n)
            .NotEmpty().WithMessage("The number of best stories to retrieve is required.")
            .GreaterThan(0).WithMessage("The number of best stories to retrieve must be grater than 0.");
    }
}
