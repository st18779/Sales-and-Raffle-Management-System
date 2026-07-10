
namespace server.Models
{
    public class Donor
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public ICollection<Gift> Gifts { get; set; } = new List<Gift>();
    }
}
