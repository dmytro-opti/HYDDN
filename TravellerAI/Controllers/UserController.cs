using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Responses;

namespace TravellerAI.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ILoggerService<UserController> _loggerService;


    public UserController(ILogger<UserController> logger,  ILoggerService<UserController> loggerService)
    {
        _logger = logger;
        _loggerService = loggerService;
    }

    [HttpGet]
    public UserLoginResponse Get()
    {
        return new UserLoginResponse()
        {
            Token =  "Token",
            User = new UserViewModel()
        };
    }
}