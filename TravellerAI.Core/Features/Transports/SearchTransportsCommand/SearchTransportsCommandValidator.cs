using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Transports.SearchTransportsCommand;

public class SearchTransportsCommandValidator : AbstractValidator<SearchTransportsCommand>
{
    public SearchTransportsCommandValidator()
    {
        RuleFor(x => x.Type).IsInEnum().WithMessage("Invalid transport type");
        RuleFor(x => x.SeatClass).IsInEnum().WithMessage("Invalid seat class");
        RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(MinPrice).When(x => x.MaxPrice.HasValue).WithMessage($"MaxPrice must be greater than or equal {MinPrice}");
        RuleFor(x => x.MinSeats).GreaterThanOrEqualTo(MinSeatCount).WithMessage($"MinSeats must be at least {MinSeatCount}");
        RuleFor(x => x.DepartureTo).GreaterThanOrEqualTo(x => x.DepartureFrom).When(x => x.DepartureFrom.HasValue && x.DepartureTo.HasValue).WithMessage("DepartureTo must be after DepartureFrom");
    }
}
