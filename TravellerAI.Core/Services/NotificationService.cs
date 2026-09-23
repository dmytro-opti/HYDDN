using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class NotificationService : INotificationService
{
    private readonly IRepository<NotificationEntity> _notificationRepository;
    private readonly IMapper _mapper;

    public NotificationService(IRepository<NotificationEntity> notificationRepository, IMapper mapper)
    {
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<NotificationModel> AddNotificationAsync(Guid userId, NotificationType type, string title, string message)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(message))
        {
            throw new BadRequestException("Notification title and message are required");
        }

        var notification = new NotificationEntity
        {
            UserId = userId,
            Type = type,
            Title = title.Trim(),
            Message = message.Trim()
        };

        await _notificationRepository.AddAsync(notification);

        return _mapper.Map<NotificationModel>(notification);
    }

    public async Task<IEnumerable<NotificationModel>> GetNotificationsAsync(Guid userId, bool unreadOnly = false)
    {
        var notifications = await _notificationRepository.FindAsync(n => n.UserId == userId && (!unreadOnly || !n.IsRead));

        return _mapper.Map<List<NotificationModel>>(notifications.OrderByDescending(n => n.Created));
    }

    public async Task MarkAsReadAsync(Guid userId, Guid notificationId)
    {
        var notification = await GetOwnNotificationAsync(userId, notificationId);
        if (notification.IsRead)
        {
            return;
        }

        notification.IsRead = true;
        notification.ReadAt = DateTime.UtcNow;
        await _notificationRepository.UpdateAsync(notification);
    }

    public async Task DeleteNotificationAsync(Guid userId, Guid notificationId)
    {
        await GetOwnNotificationAsync(userId, notificationId);
        await _notificationRepository.DeleteAsync(notificationId);
    }

    private async Task<NotificationEntity> GetOwnNotificationAsync(Guid userId, Guid notificationId)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId)
                           ?? throw new NotFoundException("Notification", notificationId);

        if (notification.UserId != userId)
        {
            throw new ForbiddenException($"Notification {notificationId} does not belong to user {userId}");
        }

        return notification;
    }
}
