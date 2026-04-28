using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Responses;

namespace TravellerAI.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
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