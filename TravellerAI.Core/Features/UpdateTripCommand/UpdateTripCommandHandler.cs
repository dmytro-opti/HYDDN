using AutoMapper;
using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.UpdateTripCommand;

public class UpdateTripCommandHandler : IRequestHandler<UpdateTripCommand, TripModel>
{
    private readonly ITripService _tripService;
    private readonly IMapper _mapper;

    public UpdateTripCommandHandler(ITripService tripService, IMapper mapper)
    {
        _tripService = tripService;
        _mapper = mapper;
    }

    public async Task<TripModel> Handle(UpdateTripCommand request, CancellationToken cancellationToken)
    {
        // throws NotFoundException when missing
        var trip = await _tripService.GetTripAsync(request.TripId);

        _mapper.Map(request, trip);
        await _tripService.UpdateTripAsync(trip);

        // return persisted state (ids of newly created booking etc.)
        return await _tripService.GetTripAsync(request.TripId);
    }
}
