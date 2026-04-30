using DateTime = System.DateTime;
using Guid = System.Guid;
using TravellerAI.Domain.Enums;


namespace TravellerAI.Domain.Models;

    public class BookingModel
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public Guid PropertyId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public string Currency { get; set; } 
        public bool IsPaid { get; set; }
        public string PaymentMethod { get; set; }
        public int GuestCount { get; set; }
        public BookingStatus Status { get; set; } 
        public BudgetModel Budget { get; set; }
    }


