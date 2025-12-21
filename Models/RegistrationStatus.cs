namespace Pied_Piper.Models
{
    public class RegistrationStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation Properties
        public ICollection<Registration> Registrations { get; set; } = new List<Registration>();
    }
}
