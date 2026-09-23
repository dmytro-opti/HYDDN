using FluentValidation;

namespace TravellerAI.Core.Features.Journeys.GetMyJourneysCommand;

public class GetMyJourneysCommandValidator : AbstractValidator<GetMyJourneysCommand>
{
    public GetMyJourneysCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
    }
}
