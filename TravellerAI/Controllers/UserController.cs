using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Exceptions;

namespace TravellerAI.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ILoggerService<UserController> _loggerService;

    private readonly IMediator _mediator;

    public UserController(ILogger<UserController> logger,  ILoggerService<UserController> loggerService, IMediator mediator)
    {
        _logger = logger;
        _loggerService = loggerService;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var result = await _mediator.Send(new BuildJourneyCommand());
            
            return new JsonResult(result);
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}