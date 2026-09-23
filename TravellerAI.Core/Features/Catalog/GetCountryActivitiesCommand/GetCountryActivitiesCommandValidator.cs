using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Catalog.GetCountryActivitiesCommand;

public class GetCountryActivitiesCommandValidator : AbstractValidator<GetCountryActivitiesCommand>
{
    public GetCountryActivitiesCommandValidator()
    {
        RuleFor(x => x.CountryId).NotEmpty().WithMessage("CountryId cannot be empty");
        RuleFor(x => x.City).MaximumLength(MaxLocationNameLength).WithMessage($"City cannot exceed {MaxLocationNameLength} characters");
    }
}
