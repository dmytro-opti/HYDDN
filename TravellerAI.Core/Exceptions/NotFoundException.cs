namespace TravellerAI.Core.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string messege) : base(messege) { }
}