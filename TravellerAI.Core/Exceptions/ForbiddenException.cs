namespace TravellerAI.Core.Exceptions;

/// <summary>
/// User is not allowed to access or change the resource (HTTP 403).
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}
