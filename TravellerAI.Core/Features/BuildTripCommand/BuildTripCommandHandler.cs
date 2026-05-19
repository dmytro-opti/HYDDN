using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.BuildTripCommand;

public class BuildTripCommandHandler : IRequestHandler<BuildTripCommand, TripModel>
{
    private readonly ITripService _tripService;
    private readonly IJourneyService _journeyService;
    private readonly IBudgetService _budgetService;
    private readonly IBookingService _bookingService;

    public BuildTripCommandHandler(ITripService tripService, IJourneyService journeyService, IBudgetService budgetService, IBookingService bookingService)
    {
        _tripService = tripService;
        _journeyService = journeyService;
        _budgetService = budgetService;
        _bookingService = bookingService;
    }

    public async Task<TripModel> Handle(BuildTripCommand command, CancellationToken cancellationToken)
    {
        var trip = await _tripService.GetTrip(command.TripId);

        _budgetService.SetBudget(trip, command.Budget);
        
        await _tripService.SelectPeriod(trip, command.Period);
        
        foreach (var journeyObj in command.Journeys)
        {
            var journey = await _journeyService.GetJourney(journeyObj.Id);
            
            await _journeyService.SelectPeriod(journey, journeyObj.Period);
            
            await _journeyService.SetMembers(journey, journeyObj.Members);

            await _journeyService.AddTransport(journey, command.Transport);

            await _budgetService.UpdateBudget(journey, command.Transport.TotalBudget);

            await _bookingService.CheckAvailableDates(journey, command.Booking);
        
            await _bookingService.SelectPlace(journey, command.Booking);

            await _budgetService.UpdateBudget(journey, command.Booking.TotalBudget);
        }

        await _tripService.Build(trip);
        
        return await _tripService.Show(trip);
    }
}