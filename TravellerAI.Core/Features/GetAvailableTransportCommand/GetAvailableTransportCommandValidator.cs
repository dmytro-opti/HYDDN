using FluentValidation;

namespace TravellerAI.Core.Features.GetAvailableTransportCommand;

public class GetAvailableCommandValidator : AbstractValidator<GetAvailableTransportCommand>
{
    public GetAvailableCommandValidator()
    {
    }
}