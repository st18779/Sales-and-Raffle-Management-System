namespace server.Models
{
    public class Gift
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
        public bool IsRandomized { get; set; }
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string DonorId { get; set; }=string.Empty;
        public Donor? Donor { get; set; }
        public string? WinnerUserId { get; set; }
        public User? WinnerUser { get; set; }
        //public ICollection<User> UsersBought { get; set; } = new List<User>();
        public string? ImageUrl { get; set; }

    }
}
