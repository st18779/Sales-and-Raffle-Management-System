using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class User
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Password { get; set; }
        public string Phone { get; set; } = string.Empty;
        public bool IsAdmin { get; set; } = false;
        //public Gift? Gift { get; set; }
        //public ICollection<Gift> SelectedGifts { get; set; } = new List<Gift>();
    }
}
