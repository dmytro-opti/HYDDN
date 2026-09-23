using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.BuildTripCommand;

public class BuildTripCommandHandler : IRequestHandler<BuildTripCommand, TripModel>
{
    private readonly ITripService _tripService;
    private readonly IJourneyService _journeyService;
    private readonly IBudgetService _budgetService;
    private readonly ITransportService _transportService;

    public BuildTripCommandHandler(ITripService tripService, IJourneyService journeyService,
        IBudgetService budgetService, ITransportService transportService)
    {
        _tripService = tripService;
        _journeyService = journeyService;
        _budgetService = budgetService;
        _transportService = transportService;
    }

    public async Task<TripModel> Handle(BuildTripCommand command, CancellationToken cancellationToken)
    {
        var trip = await _tripService.GetTripAsync(command.TripId);

        await _budgetService.SetBudget(trip, command.Budget);
        
        await _tripService.SelectPeriod(trip, command.Period);
        
        foreach (var journeyObj in command.Journeys ?? Enumerable.Empty<Domain.ViewModels.JourneyViewModel>())
        {
            var journey = await _journeyService.GetJourneyAsync(journeyObj.Id);

            if (journeyObj.Period != null)
            {
                await _journeyService.SelectPeriod(journey, journeyObj.Period);
            }

            if (journeyObj.Members != null)
            {
                await _journeyService.SetMembers(journey, journeyObj.Members);
            }

            if (journeyObj.Budget > 0)
            {
                await _budgetService.UpdateBudget(journey, journeyObj.Budget);
            }

            var transport = journeyObj.Transport ?? command.Transport;
            if (transport != null)
            {
                await _transportService.AddTransportAsync(trip.TripId, journey.Id, transport.Type, transport.Company,
                    transport.SeatClass, transport.SeatCount, transport.Price);
            }
        }

        // recalculates budget totals from added transports and booking
        await _tripService.Build(trip);
        
        return await _tripService.Show(trip);
    }
}
