using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Exceptions;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.AddTransportCommand;

public class AddTransportCommandHandler : IRequestHandler<AddTransportCommand, Guid>
{
    private readonly ITransportService _transportService;
    private readonly IJourneyService _journeyService;
    private readonly IUserService _userService;
    private readonly IMediator _mediator;

    public AddTransportCommandHandler(ITransportService transportService, IJourneyService journeyService,
        IUserService userService, IMediator mediator)
    {
        _transportService = transportService;
        _journeyService = journeyService;
        _userService = userService;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(AddTransportCommand command, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUser(command.UserId);
        if (user == null)
        {
            throw new Exception($"User {command.UserId} does not exist");
        }

        var journey = await _journeyService.GetJourney(command.JourneyId);
        if (journey == null)
        {
            throw new Exception($"Journey {command.JourneyId} does not exist");
        }

        var availableTransports = await _transportService.GetAvailableTransports(command.Type, command.Company);

        if (!availableTransports.Any())
        {
            throw new Exception($"No transport available for {command.Company}");
        }
        
        var selectedTransport = availableTransports.First();

        var availableSeats =
            await _transportService.CheckSeats(selectedTransport.TransportId, command.Type, command.Company, command.SeatClass);
        if (!availableSeats.Any())
        {
            throw new Exception($"No seats available for {command.Company} in {command.SeatClass}");
        }

        var transport = new TransportModel
        {
            Type = command.Type,
            Company = command.Company,
            SeatCount = availableSeats.Count(),
            SeatClass = command.SeatClass
        };
        return await _transportService.CreateTransport(transport);
    }
}
