using FluentValidation;

namespace TravellerAI.Core.Features.Bookings.GetBookingCommand;

public class GetBookingCommandValidator : AbstractValidator<GetBookingCommand>
{
    public GetBookingCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.BookingId).NotEmpty().WithMessage("BookingId cannot be empty");
    }
}
