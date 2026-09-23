namespace TravellerAI.Core.Exceptions;

/// <summary>
/// Request conflicts with the current state of a resource (HTTP 409),
/// e.g. duplicated email, overlapping booking, frozen booking modification.
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }

    public ConflictException(string message, Exception inner) : base(message, inner) { }
}
