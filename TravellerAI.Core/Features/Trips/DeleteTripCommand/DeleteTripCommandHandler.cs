using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.Trips.DeleteTripCommand;

public class DeleteTripCommandHandler : IRequestHandler<DeleteTripCommand, Unit>
{
    private readonly ITripService _tripService;

    public DeleteTripCommandHandler(ITripService tripService)
    {
        _tripService = tripService;
    }

    public async Task<Unit> Handle(DeleteTripCommand command, CancellationToken cancellationToken)
    {
        await _tripService.DeleteTripAsync(command.UserId, command.TripId);

        return Unit.Value;
    }
}
