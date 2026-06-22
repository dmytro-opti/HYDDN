using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Services;

public class NotificationService : INotificationService
{
    public Task<string> AddNotificationAsync(string message)
    {
        throw new NotImplementedException();
    }

    public Task<string> SelectNotificationsAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<string> UpdateNotificationAsync(Guid id, string message)
    {
        throw new NotImplementedException();
    }

    public Task<string> DeleteNotificationAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}