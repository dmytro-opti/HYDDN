using AutoMapper;
using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Trips.CreateTripCommand;

public class CreateTripCommandHandler : IRequestHandler<CreateTripCommand, TripModel>
{
    private readonly ITripService _tripService;
    private readonly IMapper _mapper;

    public CreateTripCommandHandler(ITripService tripService, IMapper mapper)
    {
        _tripService = tripService;
        _mapper = mapper;
    }

    public async Task<TripModel> Handle(CreateTripCommand command, CancellationToken cancellationToken)
    {
        return await _tripService.CreateTripAsync(command.UserId, _mapper.Map<TripDraftModel>(command));
    }
}
