using TravellerAI.Domain.Exceptions;

namespace TravellerAI.Core.Exceptions;

/// <summary>
/// Requested resource does not exist (HTTP 404).
/// </summary>
public class NotFoundException : ResourceNotFoundException
{
    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string resourceName, Guid id) : base($"{resourceName} {id} not found") { }
}
