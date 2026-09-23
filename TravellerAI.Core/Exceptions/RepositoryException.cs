namespace TravellerAI.Core.Exceptions;

/// <summary>
/// Unexpected data storage failure (HTTP 500). Wraps the original provider exception.
/// </summary>
public class RepositoryException : Exception
{
    public RepositoryException(string message, Exception inner) : base(message, inner) { }
}
