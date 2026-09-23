using FluentValidation;

namespace TravellerAI.Core.Features.User.UpdateUserEmailCommand;

public class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand>
{
    public UpdateUserEmailCommandValidator()
    {
        RuleFor(input => input.UserId)
            .NotEmpty().WithMessage("UserId cannot be empty");
        
        RuleFor(input => input.Email)
            .NotNull().EmailAddress().WithMessage("Email should be valid email");
    }
}