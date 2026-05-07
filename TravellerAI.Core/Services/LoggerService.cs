using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class LoggerService<T> : ILoggerService<T> where T : class
{
    public T LogObject { get; set; }
    public void Log(ErrorLevel level, string message)
    {
        Console.WriteLine($"[{DateTime.Now}]: {typeof(T).FullName} -- {level}: {message}");
    }
}

/*public class Work
{
    public void DoSomething()
    {
        LoggerService<ActivityModel> logger = new LoggerService<ActivityModel>();
        logger.LogObject = new ActivityModel();
        
        LoggerService<BookingModel> bookingLogger = new LoggerService<BookingModel>();
        bookingLogger.LogObject = new BookingModel();
    }
}*/