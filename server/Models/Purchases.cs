namespace server.Models
{
    public class Purchases
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; }
        public int GiftId { get; set; }
        public Gift Gift { get; set; }
        public int NumOfTickets { get; set; }
        public bool IsClosed { get; set; } = false;
        public DateTime PurchaseDate { get; set; }
    }
}
