namespace server.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Gift> Gifts { get; set; } = new List<Gift>();
    }
}
