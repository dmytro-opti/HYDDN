using FluentValidation;

namespace TravellerAI.Core.Features.GetUserProfileCommand;

public class GetUserProfileCommandValidator : AbstractValidator<GetUserProfileCommand>
{
    public GetUserProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty")
            .NotNull().WithMessage("UserId cannot be null");
    }
}