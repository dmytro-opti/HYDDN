namespace TravellerAI.Domain.Models;
public class ReviewModel
{
    public Guid Id { get; set; }
    public Guid UserID { get; set; }
    public Guid PlaceID { get; set; }
    public int Rating { get; set; }
    public string Coment { get; set; }
    public string Titel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsVisible { get; set; } = true;
    public int LikesCount { get; set; }
}