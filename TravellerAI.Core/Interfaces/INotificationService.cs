namespace TravellerAI.Core.Interfaces;

public interface INotificationService
{
    Task<string> AddNotificationAsync(string message);
    Task<string> SelectNotificationsAsync(Guid id);
    Task<string> UpdateNotificationAsync(Guid id, string message);
    Task<string> DeleteNotificationAsync(Guid id);
}