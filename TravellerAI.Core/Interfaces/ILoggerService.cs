using TravellerAI.Domain.Enums;

namespace TravellerAI.Core.Interfaces;

public interface ILoggerService<T> where T : class
{
    public T LogObject {get; set; }
    public void Log(ErrorLevel level, string message);
}