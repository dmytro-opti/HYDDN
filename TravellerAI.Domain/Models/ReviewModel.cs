namespace TravellerAI.Domain.Models;
public class ReviewModel
{
    public int Id { get; set; }
    public int UserID { get; set; }
    public int PlaceID { get; set; }
    public int Rating { get; set; }
    public string Coment { get; set; }
    public string Titel { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsVisible { get; set; } = true;
    public int LikesCount { get; set; }
}