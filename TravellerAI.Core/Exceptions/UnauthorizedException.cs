namespace TravellerAI.Core.Exceptions;

/// <summary>
/// Missing or invalid credentials / tokens (HTTP 401).
/// </summary>
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message) { }
}
