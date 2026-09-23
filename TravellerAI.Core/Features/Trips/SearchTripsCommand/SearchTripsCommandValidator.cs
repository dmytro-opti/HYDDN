using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Trips.SearchTripsCommand;

public class SearchTripsCommandValidator : AbstractValidator<SearchTripsCommand>
{
    public SearchTripsCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.CountryId).NotEmpty().WithMessage("CountryId cannot be empty");
        RuleFor(x => x.City).MaximumLength(MaxLocationNameLength).WithMessage($"City cannot exceed {MaxLocationNameLength} characters");
    }
}
