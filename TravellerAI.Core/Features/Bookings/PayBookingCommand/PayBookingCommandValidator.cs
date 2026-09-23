using FluentValidation;
using static TravellerAI.Core.Constants;
using static TravellerAI.Core.Constants.Validation;

namespace TravellerAI.Core.Features.Bookings.PayBookingCommand;

public class PayBookingCommandValidator : AbstractValidator<PayBookingCommand>
{
    public PayBookingCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId cannot be empty");
        RuleFor(x => x.BookingId).NotEmpty().WithMessage("BookingId cannot be empty");
        RuleFor(x => x.PaymentMethod).NotEmpty().WithMessage("PaymentMethod cannot be empty").MaximumLength(MaxNameLength).WithMessage($"PaymentMethod cannot exceed {MaxNameLength} characters");
    }
}
