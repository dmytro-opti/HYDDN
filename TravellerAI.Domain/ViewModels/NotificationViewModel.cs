using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels;

public class NotificationViewModel
{
    public Guid Id { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
