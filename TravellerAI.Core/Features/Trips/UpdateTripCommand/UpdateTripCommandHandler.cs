using AutoMapper;
using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Trips.UpdateTripCommand;

public class UpdateTripCommandHandler : IRequestHandler<UpdateTripCommand, TripModel>
{
    private readonly ITripService _tripService;
    private readonly IMapper _mapper;

    public UpdateTripCommandHandler(ITripService tripService, IMapper mapper)
    {
        _tripService = tripService;
        _mapper = mapper;
    }

    public async Task<TripModel> Handle(UpdateTripCommand command, CancellationToken cancellationToken)
    {
        return await _tripService.UpdateTripAsync(command.UserId, command.TripId, _mapper.Map<TripDraftModel>(command));
    }
}
