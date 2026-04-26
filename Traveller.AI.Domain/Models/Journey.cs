namespace Traveller.AI.Domain.Models;

public class Journey
{
    // Ідентифікатори
    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; } // Зв'язок з розробником User
    public Guid TripId { get; private set; }  // Зв'язок з розробником Trip (шаблон)

    // Конкретні дані цієї подорожі
    public string? CustomName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    // Стан подорожі
    public JourneyStatus Status { get; private set; }

    // Контейнер для додаткових сутностей
    public List<Guid> ParticipantIds { get; set; } = new List<Guid>();
    public List<Guid> BookingIds { get; set; } = new List<Guid>(); // Зв'язок з розробником Booking

    // Конструктор
    public Journey(Guid ownerId, Guid tripId, DateTime start, DateTime end)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        TripId = tripId;
        StartDate = start;
        EndDate = end;
        Status = JourneyStatus.Draft;
    }

    // Методи керування (Твоя робота як TL)
    
    public void ConfirmJourney(Guid paymentId)
    {
        // Тут може бути виклик логіки розробника Balance
        if (paymentId != Guid.Empty)
        {
            Status = JourneyStatus.Confirmed;
            Console.WriteLine($"Journey {Id} підтверджено та оплачено.");
        }
    }

    public void StartJourney()
    {
        if (DateTime.Now >= StartDate && Status == JourneyStatus.Confirmed)
        {
            Status = JourneyStatus.Active;
            Console.WriteLine("Подорож розпочалася!");
        }
    }

    public bool IsOverlapping(DateTime otherStart, DateTime otherEnd)
    {
        // Логіка перевірки, чи не збігається ця подорож з іншою
        return StartDate < otherEnd && otherStart < EndDate;
    }
}