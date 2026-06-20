using FluentValidation;

namespace TravellerAI.Core.Features.User.UpdateUserEmailCommand;

public class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand>
{
    public UpdateUserEmailCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotNull().WithMessage("UserId cannot be null");
        
        RuleFor(input => input.Email)
            .NotNull().EmailAddress().WithMessage("Email should be valid email");
    }
}