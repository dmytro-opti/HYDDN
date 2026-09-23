namespace TravellerAI.Domain.Entities;

public class ReviewEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;

    public Guid? PlaceId { get; set; }
    public virtual PlaceEntity? Place { get; set; }

    public int Rating { get; set; }
    public string? Comment { get; set; }
    public string? Title { get; set; }
    public bool IsVisible { get; set; } = true;
    public int LikesCount { get; set; }
}
