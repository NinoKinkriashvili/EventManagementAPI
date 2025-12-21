namespace Pied_Piper.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation Properties
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
