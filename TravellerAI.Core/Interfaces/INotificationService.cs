using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface INotificationService
{
    Task<NotificationModel> AddNotificationAsync(Guid userId, NotificationType type, string title, string message);
    Task<IEnumerable<NotificationModel>> GetNotificationsAsync(Guid userId, bool unreadOnly = false);
    Task MarkAsReadAsync(Guid userId, Guid notificationId);
    Task DeleteNotificationAsync(Guid userId, Guid notificationId);
}
